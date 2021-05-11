Option Explicit On
Option Strict On
Public Class cTreeManipulator

    'Private gboolValidateAfterEveryChange As Boolean = True

    Private gboolValidationBypass As Boolean = True
    Private gdtNull As Date = DateValue("01/01/1900")

    Public mTree As TreeView = Nothing
    Public mintLastNodeNbrUsed As Integer = -1
    Public mintNumberOfNodesInTree As Integer = -1
    Public mintLoadedDBVersion As Integer = -1
    Public mstrContentDirectory As String = ""
    Public mstrCurrentPIMFileBasename As String = ""
    Public mstrCurrentDBFileName As String = ""
    Public mintValidateAfterEveryThisManyChanges As Integer = -1
    Public mintChangeCounter As Integer = 0

    Private mintWriteNodeCounter As Integer
    Private mintWriteMaxNodeNbrSeen As Integer

    Dim gfNewContentFile As IO.StreamWriter = Nothing
    Public gstrValidationErrorReason As String

    Public L As cLogging = Nothing

    Dim TODO As New cToDoItem

    Private Sub CheckReady()
        If (IsNothing(mTree)) Then
            Throw New Exception("Tree not defined")
        End If
        If (mintLastNodeNbrUsed = -1) Then
            Throw New Exception("Last node number used not defined")
        End If
        If (mintNumberOfNodesInTree = -1) Then
            Throw New Exception("Number of nodes not defined")
        End If
        If (mintLoadedDBVersion = -1) Then
            Throw New Exception("Loaded DB Version not defined")
        End If
        If (mstrContentDirectory = "") Then
            Throw New Exception("Content directory not defined")
        End If
        If (mstrCurrentPIMFileBasename = "") Then
            Throw New Exception("File basename not defined")
        End If
        If (mstrCurrentDBFileName = "") Then
            Throw New Exception("DB filename not defined")
        End If
    End Sub

    Public Sub ApplyNewTransaction(ft As System.IO.StreamWriter, tree As TreeView, strTransactionName As String, theItem As cToDoItem.sItemInfo, theNode As TreeNode)
        ApplyTransaction(ft, tree, strTransactionName, theItem, theNode, Nothing)
        'If (gboolValidateAfterEveryChange And Not (gboolValidationBypass)) Then
        '    If (Not ValidateTree(tree, mintNumberOfNodesInTree, mintLastNodeNbrUsed)) Then
        '        MessageBox.Show("Warning!  Tree fails validation after transaction!")
        '    End If
        'End If
    End Sub

    Public Sub ApplyTransactionFromLog(tree As TreeView, strTransactionName As String, strAttributeElements As String())
        ApplyTransaction(Nothing, tree, strTransactionName, Nothing, Nothing, strAttributeElements)
    End Sub

    ' if ft is nothing, do not alter transaction log, process transaction retrieved already from log (strTransactionName,strAttributeElements())
    Sub ApplyTransaction(ft As System.IO.StreamWriter, tree As TreeView, strTransactionName As String, theItem As cToDoItem.sItemInfo, theNode As TreeNode, strAttributeElements As String())

        Dim pItem As cToDoItem.sItemInfo
        Dim pNode As TreeNode

        Dim localItem As cToDoItem.sItemInfo
        Dim aNode As TreeNode
        Dim intANodeNbr As Integer

        Dim strAux As String
        Dim intNdx, intLastNdx As Integer
        Dim boolIsChange As Boolean = False

        ' *** this is hacky, it's because during TRN log scan for recovery, nothing has been init'd yet
        If (strTransactionName <> "Open") Then
            CheckReady()
        End If

        Select Case strTransactionName
            Case "Open"
                If (Not IsNothing(ft)) Then
                    ' User driven
                    ft.WriteLine("<Open>")
                    ft.Flush()
                Else
                    ' Transaction file driven
                End If
                ' Perform actual action
            'Case Init was undone, just done as a call when code is first run on a new DB
            ' *** ensure mintLoadedDBVersion is properly initialized with current DB file version
            Case "Saving"
                If (Not IsNothing(ft)) Then
                    ft.WriteLine("<Saving " & (mintLoadedDBVersion + 1) & ">")
                    ft.Flush()
                End If
            ' *** ensure mintLoadedDBVersion has been incremented after physical save is complete
            Case "Saved"
                If (Not IsNothing(ft)) Then
                    ft.WriteLine("<Saved " & mintLoadedDBVersion & ">")
                    ft.Flush()
                    L.WriteToLog("Database Saved")
                    mintChangeCounter = 0
                End If
                ' Closing tag is used to verify that a run of the program completed without error
                ' if it's missing on program startup, program will exit or attempt DB recovery from TRN data
            Case "Closing"
                If (Not IsNothing(ft)) Then
                    ft.WriteLine("<Closing>")
                    ft.Flush()
                End If
            Case "Add"
                ' item: item to add (fully populated)
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    pItem = theItem
                    strAux = TODO.EncodeItem(theItem)
                    ft.WriteLine("<Add " & strAux & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    If UBound(strAttributeElements) <> 9 Then
                        Throw New Exception("Add, wrong number of attribute elements")
                    End If
                    strAux = ""
                    For intNdx = 0 To 9
                        If (strAux <> "") Then
                            strAux &= ":"
                        End If
                        strAux &= strAttributeElements(intNdx)
                    Next
                    pItem = TODO.DecodeItem(strAux)
                    ' *** ensure such variable updates are only done in transactions or tightly bound callers
                    If (pItem.intNodeNbr > mintLastNodeNbrUsed) Then
                        mintLastNodeNbrUsed = pItem.intNodeNbr
                    End If
                End If
                ' Add the new node to its designated parent
                pNode = TODO.FindNodeByNodeNbr(tree, pItem.intParentNodeNbr)
                localItem = CType(pNode.Tag, cToDoItem.sItemInfo)

                aNode = New TreeNode
                aNode.Text = TODO.GetDisplayTextForItem(pItem)
                aNode.Checked = pItem.isChecked
                aNode.Tag = pItem
                pNode.Nodes.Add(aNode)
                If (localItem.strChildOrder <> "") Then
                    TODO.BuildUpdatedOrderListInThisParentNode(pNode)
                End If
                ResortAndRedisplayParent(tree, aNode)
                mintNumberOfNodesInTree += 1
                If (Not IsNothing(ft)) Then
                    boolIsChange = True
                    ' provide human understandable description of action to log
                    L.WriteToLog("Added (" & pItem.strText & ") to (" & localItem.strText & ")")
                End If
            Case "Edit"
                ' item: item that was edited.  We do not currently diff it vs its previous state.
                ' *** when doing Edit, be able to display exactly what changed.
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    pItem = theItem
                    pItem.dtModified = Date.Now
                    strAux = TODO.EncodeItem(pItem)
                    ft.WriteLine("<Edit " & strAux & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    If UBound(strAttributeElements) <> 9 Then
                        Throw New Exception("Edit, wrong number of attribute elements")
                    End If
                    strAux = ""
                    For intNdx = 0 To 9
                        If (strAux <> "") Then
                            strAux &= ":"
                        End If
                        strAux &= strAttributeElements(intNdx)
                    Next
                    pItem = TODO.DecodeItem(strAux)
                End If
                ' replace the contents of that node with new content
                aNode = TODO.FindNodeByNodeNbr(tree, pItem.intNodeNbr)
                aNode.Tag = pItem
                aNode.Text = TODO.GetDisplayTextForItem(pItem)
                ' *** ensure this is called consistently where needed.  Maybe only if node known to be visible now.
                ' *** ensure ResortAndRedisplayParent is always getting 2nd param as node not its parent
                ' *** ensure that events are handled properly (selection before and after)
                ResortAndRedisplayParent(tree, aNode)
                If (Not IsNothing(ft)) Then
                    boolIsChange = True
                    ' provide human understandable description of action to log
                    L.WriteToLog("Edited (" & pItem.strText & ")")
                End If
            Case "Checkbox"
                ' input : theItem only, item to have checkbox toggled
                ' for user mode, toggles state of checkbox, records the new state
                ' for trn mode sets the checkbox state
                ' item: increase priority by 1
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    pItem = theItem
                    pNode = TODO.FindNodeByNodeNbr(tree, theItem.intNodeNbr)
                    pItem.isChecked = Not pItem.isChecked
                    strAux = "0"
                    If (pItem.isChecked) Then
                        strAux = "1"
                    End If
                    ft.WriteLine("<Checkbox " & pItem.intNodeNbr & ":" & strAux & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    pNode = TODO.FindNodeByNodeNbr(tree, Convert.ToInt32(strAttributeElements(0)))
                    pItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                    Select Case strAttributeElements(1)
                        Case "0"
                            If (Not pItem.isChecked) Then
                                Throw New Exception("Node not in correct checked state")
                            End If
                            pItem.isChecked = False
                        Case "1"
                            If (pItem.isChecked) Then
                                Throw New Exception("Node not in correct checked state")
                            End If
                            pItem.isChecked = True
                        Case Else
                            Throw New Exception("Bad value for checkbox in transaction log")
                    End Select
                End If
                pNode.Tag = pItem
                'ResortAndRedisplayParent(tree, pNode)
                If (Not IsNothing(ft)) Then
                    boolIsChange = True
                    ' provide human understandable description of action to log
                    If (pItem.isChecked) Then
                        L.WriteToLog("Checked set to YES for (" & pItem.strText & ")")
                    Else
                        L.WriteToLog("Checked set to NO for (" & pItem.strText & ")")
                    End If
                End If
            Case "Delete"
                ' item: item to be deleted, i.e. move its node to Trash tree
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    intANodeNbr = theItem.intNodeNbr
                    ft.WriteLine("<Delete " & intANodeNbr & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    If (Not IsNumeric(strAttributeElements(0))) Then
                        Throw New Exception("Delete: bad attribute")
                    End If
                    intANodeNbr = Convert.ToInt32(strAttributeElements(0))
                End If

                aNode = TODO.FindNodeByNodeNbr(tree, intANodeNbr)
                TODO.MoveNode(tree, aNode, TODO.FindNodeByNodeNbr(tree, 3))
                If (Not IsNothing(ft)) Then
                    boolIsChange = True
                    ' provide human understandable description of action to log
                    L.WriteToLog("Deleted (" & theItem.strText & ")")
                End If
            Case "Move"
                ' move item into (child of) node
                ' *** protect against moving special nodes
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    pItem = theItem
                    pNode = theNode
                    localItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                    ft.WriteLine("<Move " & pItem.intNodeNbr & ":" & localItem.intNodeNbr & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    aNode = TODO.FindNodeByNodeNbr(tree, Convert.ToInt32(strAttributeElements(0)))
                    pItem = CType(aNode.Tag, cToDoItem.sItemInfo)
                    pNode = TODO.FindNodeByNodeNbr(tree, Convert.ToInt32(strAttributeElements(1)))
                End If
                ' make node-item in pItem now be child of pNode
                ' MoveNode wants *node* of pItem as its parameter
                tree.BeginUpdate()
                ' *** do this more efficiently
                L.WriteToLog("Perform MoveNode Call", True)
                TODO.MoveNode(tree, TODO.FindNodeByNodeNbr(tree, pItem.intNodeNbr), pNode)
                L.WriteToLog("Return from MoveNode Call", True)
                ' redisplay both subtrees
                ' parent of original location (not necessary)
                'ResortAndRedisplayParent(bNode)
                ' parent of new location
                L.WriteToLog("Move into node done so re-sort parent", True)
                ResortAndRedisplayParent(tree, TODO.FindNodeByNodeNbr(tree, pItem.intNodeNbr))
                tree.EndUpdate()
                ' *** why were these here ... needed?
                'Application.DoEvents()
                'gBoolBypassEvents = False
                If (Not IsNothing(ft)) Then
                    boolIsChange = True
                    ' provide human understandable description of action to log
                    localItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                    ' *** decoding?
                    L.WriteToLog("Moved (" & pItem.strText & ") to (" & localItem.strText & ")")
                End If
            Case "PriUp"
                ' item: increase priority by 1
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    pItem = theItem
                    pNode = TODO.FindNodeByNodeNbr(tree, pItem.intNodeNbr)
                    ft.WriteLine("<PriUp " & pItem.intNodeNbr & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    pNode = TODO.FindNodeByNodeNbr(tree, Convert.ToInt32(strAttributeElements(0)))
                    pItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                End If
                pItem.intPriority += 1
                pNode.Text = TODO.GetDisplayTextForItem(pItem)  'because priority is included in text
                pNode.Tag = pItem
                ' improvement, blank out updates because it can move significantly
                tree.BeginUpdate()
                ResortAndRedisplayParent(tree, pNode)
                tree.EndUpdate()
                If (Not IsNothing(ft)) Then
                    boolIsChange = True
                    ' provide human understandable description of action to log
                    L.WriteToLog("Increase Priority " & pItem.intPriority - 1 & " to " & pItem.intPriority & " (" & pItem.strText & ")")
                End If
            Case "PriDown"
                ' item: decrease priority by 1
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    pItem = theItem
                    pNode = TODO.FindNodeByNodeNbr(tree, pItem.intNodeNbr)
                    ft.WriteLine("<PriDown " & pItem.intNodeNbr & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    pNode = TODO.FindNodeByNodeNbr(tree, Convert.ToInt32(strAttributeElements(0)))
                    pItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                End If
                pItem.intPriority -= 1
                pNode.Text = TODO.GetDisplayTextForItem(pItem)  'because priority is included in text
                pNode.Tag = pItem
                ' improvement, blank out updates because it can move significantly
                tree.BeginUpdate()
                ResortAndRedisplayParent(tree, pNode)
                tree.EndUpdate()
                If (Not IsNothing(ft)) Then
                    boolIsChange = True
                    ' provide human understandable description of action to log
                    L.WriteToLog("Decrease Priority " & pItem.intPriority + 1 & " to " & pItem.intPriority & " (" & pItem.strText & ")")
                End If
            Case "Toggle"
                ' if node is normal, switch it to ordered children, else change it back to normal
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    pItem = theItem
                    ft.WriteLine("<Toggle " & pItem.intNodeNbr & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    pNode = TODO.FindNodeByNodeNbr(tree, Convert.ToInt32(strAttributeElements(0)))
                    pItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                End If
                ' do the toggle
                ' can use intNdx, intlastNdx, localItem and aNode freely; tree points to tree being maniupulated
                Dim intFirstChildNodeNbr As Integer
                Dim boolToOrdered As Boolean = False
                aNode = TODO.FindNodeByNodeNbr(tree, pItem.intNodeNbr)
                If (pItem.strChildOrder = "") Then
                    ' make it ordered
                    boolToOrdered = True
                    pItem.strChildOrder = "-1"
                    aNode.Tag = pItem
                    TODO.BuildUpdatedOrderListInThisParentNode(aNode)
                Else
                    ' make it normal
                    pItem.strChildOrder = ""
                    aNode.Tag = pItem
                End If
                ' fix: have to change display text now
                aNode.Text = TODO.GetDisplayTextForItem(pItem)  'because priority is included in text
                '
                intFirstChildNodeNbr = -1
                If (aNode.Nodes.Count > 0) Then
                    localItem = CType(aNode.Nodes(0).Tag, cToDoItem.sItemInfo)
                    intFirstChildNodeNbr = localItem.intNodeNbr
                End If
                If (intFirstChildNodeNbr <> -1) Then
                    aNode = TODO.FindNodeByNodeNbr(tree, intFirstChildNodeNbr)
                    ResortAndRedisplayParent(tree, aNode)
                End If
                If (Not IsNothing(ft)) Then
                    boolIsChange = True
                    ' provide human understandable description of action to log
                    If (boolToOrdered) Then
                        L.WriteToLog("Item (" & pItem.strText & ") set to Ordered")
                    Else
                        L.WriteToLog("Item (" & pItem.strText & ") set to Normal")
                    End If
                End If
            Case "MoveUp"
                ' ** do a context diff of this and MoveDown
                ' item is node to move up within children of parent node
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    pItem = theItem
                    pNode = theNode
                    ft.WriteLine("<MoveUp " & pItem.intNodeNbr & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    pNode = TODO.FindNodeByNodeNbr(tree, Convert.ToInt32(strAttributeElements(0)))
                    pItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                    pNode = TODO.FindNodeByNodeNbr(tree, pItem.intParentNodeNbr)
                End If
                ' move node with item pItem up within children of node pNode
                ' can use intNdx, intlastNdx, localItem and aNode freely; tree points to tree being maniupulate
                ' make localItem the Tag of the parent
                localItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                If (localItem.strChildOrder = "") Then
                    Throw New Exception("MoveUp called for non-Ordered node")
                End If
                Dim strElements() As String
                Dim intElements() As Integer
                'Dim intTemp As Integer
                Dim intNodeNbrToMove As Integer
                intNodeNbrToMove = pItem.intNodeNbr
                strElements = Split(localItem.strChildOrder, ",")
                If (strElements(0) <> "-1") Then
                    intLastNdx = UBound(strElements)
                    ReDim intElements(intLastNdx)
                    For intNdx = 0 To intLastNdx
                        intElements(intNdx) = Convert.ToInt32(strElements(intNdx))
                    Next
                    For intNdx = 0 To intLastNdx
                        If (intElements(intNdx) = intNodeNbrToMove) Then
                            If (intNdx > 0) Then
                                'intTemp = intElements(intNdx - 1)
                                'intElements(intNdx - 1) = intNodeNbrToMove
                                ' intElements(intNdx) = intTemp
                                aNode = TODO.FindNodeByNodeNbr(tree, intNodeNbrToMove)
                                pNode.Nodes.Remove(aNode)
                                pNode.Nodes.Insert(intNdx - 1, aNode)
                            Else
                                If (Not IsNothing(ft)) Then
                                    L.WriteToLog("Warning: Item at limit, will not actually be moved")
                                End If
                            End If
                            Exit For
                        End If
                    Next
                    TODO.BuildUpdatedOrderListInThisParentNode(pNode)
                    aNode = TODO.FindNodeByNodeNbr(tree, intNodeNbrToMove)
                    tree.SelectedNode = aNode
                    ResortAndRedisplayParent(tree, aNode)
                    If (Not IsNothing(ft)) Then
                        boolIsChange = True
                        ' provide human understandable description of action to log
                        L.WriteToLog("Moved item (" & pItem.strText & ") Up")
                    End If
                Else
                    Throw New Exception("MoveUp not possible, parent has no children in order list")
                End If
            Case "MoveToTop"
                ' item is node to move to top within children of parent node
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    pItem = theItem
                    pNode = theNode
                    ft.WriteLine("<MoveToTop " & pItem.intNodeNbr & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    pNode = TODO.FindNodeByNodeNbr(tree, Convert.ToInt32(strAttributeElements(0)))
                    pItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                    pNode = TODO.FindNodeByNodeNbr(tree, pItem.intParentNodeNbr)
                End If
                ' move node with item pItem to top within children of node pNode
                ' can use intNdx, intlastNdx, localItem and aNode freely; tree points to tree being maniupulate
                ' make localItem the Tag of the parent
                localItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                If (localItem.strChildOrder = "") Then
                    Throw New Exception("MoveToTop called for non-Ordered node")
                End If
                Dim strElements() As String
                Dim intElements() As Integer
                'Dim intTemp As Integer
                Dim intNodeNbrToMove As Integer
                intNodeNbrToMove = pItem.intNodeNbr
                strElements = Split(localItem.strChildOrder, ",")
                If (strElements(0) <> "-1") Then
                    intLastNdx = UBound(strElements)
                    ReDim intElements(intLastNdx)
                    For intNdx = 0 To intLastNdx
                        intElements(intNdx) = Convert.ToInt32(strElements(intNdx))
                    Next
                    For intNdx = 0 To intLastNdx
                        If (intElements(intNdx) = intNodeNbrToMove) Then
                            If (intNdx > 0) Then
                                'intTemp = intElements(intNdx - 1)
                                'intElements(intNdx - 1) = intNodeNbrToMove
                                ' intElements(intNdx) = intTemp
                                aNode = TODO.FindNodeByNodeNbr(tree, intNodeNbrToMove)
                                pNode.Nodes.Remove(aNode)
                                pNode.Nodes.Insert(0, aNode)
                            Else
                                If (Not IsNothing(ft)) Then
                                    L.WriteToLog("Warning: Item at limit, will not actually be moved")
                                End If
                            End If
                            Exit For
                        End If
                    Next
                    TODO.BuildUpdatedOrderListInThisParentNode(pNode)
                    aNode = TODO.FindNodeByNodeNbr(tree, intNodeNbrToMove)
                    tree.SelectedNode = aNode
                    ResortAndRedisplayParent(tree, aNode)
                    If (Not IsNothing(ft)) Then
                        boolIsChange = True
                        ' provide human understandable description of action to log
                        L.WriteToLog("Moved item (" & pItem.strText & ") To Top")
                    End If
                Else
                    Throw New Exception("MoveToTop not possible, parent has no children in order list")
                End If
            Case "MoveDown"
                ' ** do a context diff of this and MoveUp
                ' item is node to move up within children of parent node
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    pItem = theItem
                    pNode = theNode
                    ft.WriteLine("<MoveDown " & pItem.intNodeNbr & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    pNode = TODO.FindNodeByNodeNbr(tree, Convert.ToInt32(strAttributeElements(0)))
                    pItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                    pNode = TODO.FindNodeByNodeNbr(tree, pItem.intParentNodeNbr)
                End If
                ' move node with item pItem up within children of node pNode
                ' can use intNdx, intlastNdx, localItem and aNode freely; tree points to tree being maniupulate
                ' make localItem the Tag of the parent
                localItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                If (localItem.strChildOrder = "") Then
                    Throw New Exception("MoveDown called for non-Ordered node")
                End If
                Dim strElements() As String
                Dim intElements() As Integer
                'Dim intTemp As Integer
                Dim intNodeNbrToMove As Integer
                intNodeNbrToMove = pItem.intNodeNbr
                strElements = Split(localItem.strChildOrder, ",")
                If (strElements(0) <> "-1") Then
                    intLastNdx = UBound(strElements)
                    ReDim intElements(intLastNdx)
                    For intNdx = 0 To intLastNdx
                        intElements(intNdx) = Convert.ToInt32(strElements(intNdx))
                    Next
                    For intNdx = 0 To intLastNdx
                        If (intElements(intNdx) = intNodeNbrToMove) Then
                            If (intNdx < intLastNdx) Then
                                'intTemp = intElements(intNdx + 1)
                                'intElements(intNdx + 1) = intNodeNbrToMove
                                'intElements(intNdx) = intTemp
                                aNode = TODO.FindNodeByNodeNbr(tree, intNodeNbrToMove)
                                pNode.Nodes.Remove(aNode)
                                pNode.Nodes.Insert(intNdx + 1, aNode)
                            Else
                                If (Not IsNothing(ft)) Then
                                    L.WriteToLog("Warning: Item at limit, will not actually be moved")
                                End If
                            End If
                            Exit For
                        End If
                    Next
                    TODO.BuildUpdatedOrderListInThisParentNode(pNode)
                    aNode = TODO.FindNodeByNodeNbr(tree, intNodeNbrToMove)
                    tree.SelectedNode = aNode
                    ResortAndRedisplayParent(tree, aNode)
                    If (Not IsNothing(ft)) Then
                        boolIsChange = True
                        ' provide human understandable description of action to log
                        L.WriteToLog("Moved item (" & pItem.strText & ") Down")
                    End If
                Else
                    Throw New Exception("MoveDown not possible, parent has no children in order list")
                End If
            Case "MoveBelow"
                ' item: node to move, node: node to move below
                Dim intNodeNbrWeAreMovingBelow As Integer
                Dim strTextOfNodeWeAreMovingBelow As String
                If (Not IsNothing(ft)) Then
                    ' set up pItem and/or pNode based on theItem/theNode
                    ' also write the transaction to the transaction log
                    pItem = theItem 'node we are moving
                    pNode = theNode 'node we are moving below
                    localItem = CType(pNode.Tag, cToDoItem.sItemInfo)
                    strTextOfNodeWeAreMovingBelow = localItem.strText
                    intNodeNbrWeAreMovingBelow = localItem.intNodeNbr
                    ft.WriteLine("<MoveBelow " & pItem.intNodeNbr & ":" & localItem.intNodeNbr & ">")
                    ft.Flush()
                Else
                    ' set up pItem and/or pNode based on strAttributeElements() only
                    aNode = TODO.FindNodeByNodeNbr(tree, Convert.ToInt32(strAttributeElements(0)))
                    pItem = CType(aNode.Tag, cToDoItem.sItemInfo)
                    intNodeNbrWeAreMovingBelow = Convert.ToInt32(strAttributeElements(1))
                    pNode = TODO.FindNodeByNodeNbr(tree, intNodeNbrWeAreMovingBelow)
                End If
                ' pItem: tag of node we are moving; pNode: node we want to move pItem below (at same level)
                ' can use intNdx, intlastNdx, localItem and aNode freely; tree points to tree being maniupulated
                '
                ' get a permanent pointer to the node we are moving
                Dim bNode As TreeNode
                bNode = TODO.FindNodeByNodeNbr(tree, pItem.intNodeNbr)
                '
                ' first make aNode the current parent of pItem
                ' this is so we can "remove" it from the old parent
                aNode = TODO.FindNodeByNodeNbr(tree, pItem.intParentNodeNbr)
                localItem = CType(aNode.Tag, cToDoItem.sItemInfo)
                ' fix: the node being moved CAN be from a non-ordered parent
                'If (localItem.strChildOrder = "") Then
                '    Throw New Exception("MoveBelow called for non-Ordered node")
                'End If
                aNode.Nodes.Remove(bNode)
                If (localItem.strChildOrder <> "") Then
                    TODO.BuildUpdatedOrderListInThisParentNode(aNode)
                End If
                ' Now move it to its new parent at correct location
                ' make aNode the current parent of pNode
                aNode = TODO.ParentOf(tree, pNode)
                Dim localItem2 As cToDoItem.sItemInfo
                localItem2 = CType(aNode.Tag, cToDoItem.sItemInfo)
                ' which "position" in Parent Nodes is pNode?
                ' ** move below will not work for an empty parent
                Dim intIndexOfNodeInParent As Integer = -1
                intLastNdx = aNode.Nodes.Count - 1
                For intNdx = 0 To intLastNdx
                    localItem = CType(aNode.Nodes(intNdx).Tag, cToDoItem.sItemInfo)
                    If (localItem.intNodeNbr = intNodeNbrWeAreMovingBelow) Then
                        intIndexOfNodeInParent = intNdx
                        ' fix, also set new parent in node being moved
                        pItem.intParentNodeNbr = localItem2.intNodeNbr
                        bNode.Tag = pItem
                        Exit For
                    End If
                Next
                If (intIndexOfNodeInParent = -1) Then
                    Throw New Exception("Node to move below is not registered to parent")
                End If
                ' ** going to assume insert at index n puts it "above" n
                aNode.Nodes.Insert(intIndexOfNodeInParent + 1, bNode)
                TODO.BuildUpdatedOrderListInThisParentNode(aNode)
                '
                'ResortAndRedisplayParent(tree, pNode)
                If (Not IsNothing(ft)) Then
                    boolIsChange = True
                    ' provide human understandable description of action to log
                    L.WriteToLog("Moved item (" & pItem.strText & ") Below (" & strTextOfNodeWeAreMovingBelow & ")")
                End If
            Case Else
                Throw New Exception("Unknown Transaction Name")
        End Select

        If (boolIsChange) Then
            mintChangeCounter += 1
            If (mintValidateAfterEveryThisManyChanges <> -1) Then
                If ((mintChangeCounter Mod mintValidateAfterEveryThisManyChanges) = 0) Then
                    If (Not ValidateTree(tree, mintNumberOfNodesInTree, mintLastNodeNbrUsed)) Then
                        MessageBox.Show("Warning!  Tree fails validation after transaction! (" & gstrValidationErrorReason & ")")
                    Else
                        L.WriteToLog("Auto validation done, passed")
                    End If
                End If
            End If
        End If
    End Sub
    '
    ' Load and Save
    '
    Public Sub LoadDB()
        Dim fContentFile As IO.StreamReader
        Dim strBuffer As String
        Dim strElements() As String
        Dim item, item2 As cToDoItem.sItemInfo
        Dim aNode, rootnode As TreeNode
        Dim parentNode As TreeNode
        Dim intExpectedLastNodeWritten As Integer
        Dim intExpectedNumberOfNodes As Integer
        Dim intNumberOfNodesEncountered As Integer = 0

        'fake out CheckReady()
        mintLastNodeNbrUsed = 0
        mintLoadedDBVersion = 0
        mintNumberOfNodesInTree = 0
        CheckReady()

        fContentFile = System.IO.File.OpenText(mstrCurrentDBFileName)
        strBuffer = fContentFile.ReadLine()
        strElements = Split(strBuffer, ",")
        If (strElements(0) <> "V0001") Then
            Throw New Exception("Bad file format version number")
        End If
        mintLoadedDBVersion = Convert.ToInt32(strElements(1))
        ' *** this may break once we do housekeeping
        intExpectedLastNodeWritten = Convert.ToInt32(strElements(2))
        intExpectedNumberOfNodes = Convert.ToInt32(strElements(3))
        mintLastNodeNbrUsed = -1

        mTree.BeginUpdate()

        mTree.Nodes.Clear()
        item = TODO.CreateNewItem(0)
        item.strText = mstrCurrentPIMFileBasename
        rootnode = New TreeNode()
        rootnode.Text = TODO.GetDisplayTextForItem(item)
        rootnode.Tag = item
        mTree.Nodes.Add(rootnode)

        While (fContentFile.Peek <> -1)
            strBuffer = fContentFile.ReadLine
            If (strBuffer.Substring(0, 1) <> "<") Then
                Throw New Exception("Bad format for item record")
            End If
            If (strBuffer.Substring(strBuffer.Length - 1, 1) <> ">") Then
                Throw New Exception("Bad format for item record")
            End If
            strBuffer = strBuffer.Substring(1, strBuffer.Length - 2)
            ' *** on load, it's going to increment node numbers, thus assume no gaps in data
            ' *** actual issue with hidden root node does not get a create?
            intNumberOfNodesEncountered += 1
            item = TODO.DecodeItem(strBuffer)
            If (item.intNodeNbr = 0) Then
                ' copy the create date to our manually created node 0
                item2 = CType(rootnode.Tag, cToDoItem.sItemInfo)
                item2.dtCreated = item.dtCreated
                rootnode.Tag = item2
                Continue While
            End If
            If (item.intNodeNbr > mintLastNodeNbrUsed) Then
                mintLastNodeNbrUsed = item.intNodeNbr
            End If
            ' *** can this ever be a problem?  Parent is expected to be loaded already.
            parentNode = TODO.FindNodeByNodeNbr(mTree, item.intParentNodeNbr)
            If (parentNode Is Nothing) Then
                Throw New Exception("parent node not found during load")
            End If
            aNode = New TreeNode
            aNode.Text = TODO.GetDisplayTextForItem(item)
            aNode.Checked = item.isChecked
            aNode.Tag = item
            parentNode.Nodes.Add(aNode)
        End While

        mTree.CollapseAll()
        ' *** another case where sort should be done ... look at how sort works, it is destructive in a sense
        mTree.Nodes(0).Expand()
        TODO.FindNodeByNodeNbr(mTree, 2).Expand()
        ' *** don't auto select an initial node?
        mTree.EndUpdate()

        fContentFile.Close()
        ' *** do sanity checking
        If (intExpectedLastNodeWritten <> mintLastNodeNbrUsed) Then
            Throw New Exception("Last node number in database is incorrect")
        End If
        If (intNumberOfNodesEncountered <> intExpectedNumberOfNodes) Then
            Throw New Exception("Number of nodes in database is incorrect")
        End If
        mintNumberOfNodesInTree = intNumberOfNodesEncountered
        '
        gboolValidationBypass = False
        If (Not ValidateTree(mTree, mintNumberOfNodesInTree, mintLastNodeNbrUsed)) Then
            MessageBox.Show("Warning! Validation failed after load! (" & gstrValidationErrorReason & ")")
        End If
    End Sub

    ' saves the tree in this manipulator instance to the disk
    Public Sub SaveCurrent()
        Dim strBackupFile As String
        Dim fSourceContentFile As IO.StreamReader
        Dim fBackupFile As IO.StreamWriter
        Dim strBuffer As String
        Dim strElements() As String

        CheckReady()
        If (Not ValidateTree(mTree, mintNumberOfNodesInTree, mintLastNodeNbrUsed)) Then
            MessageBox.Show("Current tree structure did not pass validation, save cannot be done (" & gstrValidationErrorReason & ")")
            Exit Sub
        End If
        ' if not the first version, verify the current database file is gIntLastVersionWritten
        '     then make a backup copy of it
        If (mintLoadedDBVersion > 0) Then
            fSourceContentFile = System.IO.File.OpenText(mstrCurrentDBFileName)
            strBuffer = fSourceContentFile.ReadLine()
            fSourceContentFile.Close()
            strElements = Split(strBuffer, ",")
            If (Convert.ToInt32(strElements(1)) <> mintLoadedDBVersion) Then
                Throw New Exception("Last file written version error")
            End If
            strBackupFile = mstrContentDirectory & "\" & mstrCurrentPIMFileBasename & "_BACKUP_" & Format(mintLoadedDBVersion, "000000") & ".txt"
            fBackupFile = System.IO.File.CreateText(strBackupFile)
            fSourceContentFile = System.IO.File.OpenText(mstrCurrentDBFileName)
            Do While (fSourceContentFile.Peek <> -1)
                strBuffer = fSourceContentFile.ReadLine
                fBackupFile.WriteLine(strBuffer)
            Loop
            fSourceContentFile.Close()
            fBackupFile.Close()
        End If
        gfNewContentFile = System.IO.File.CreateText(mstrCurrentDBFileName)
        gfNewContentFile.WriteLine("V0001," & (mintLoadedDBVersion + 1) & "," & mintLastNodeNbrUsed & "," & mintNumberOfNodesInTree)
        '
        mintWriteNodeCounter = 0
        mintWriteMaxNodeNbrSeen = -1
        WriteANode(mTree.Nodes(0))
        If (mintWriteNodeCounter <> mintNumberOfNodesInTree) Then
            Throw New Exception("Wrong number of nodes found in tree on write")
        End If
        If (mintWriteMaxNodeNbrSeen <> mintLastNodeNbrUsed) Then
            Throw New Exception("Wrong max node number found in tree on write")
        End If
        '
        gfNewContentFile.Close()
        mintLoadedDBVersion += 1
    End Sub


    Private Sub WriteANode(n As TreeNode)
        Dim item As cToDoItem.sItemInfo
        Dim strOutput As String
        Dim intNdx As Integer
        Dim intLastNdx As Integer

        item = CType(n.Tag, cToDoItem.sItemInfo)
        strOutput = "<" & TODO.EncodeItem(item) & ">"
        gfNewContentFile.WriteLine(strOutput)
        mintWriteNodeCounter += 1
        If (item.intNodeNbr > mintWriteMaxNodeNbrSeen) Then
            mintWriteMaxNodeNbrSeen = item.intNodeNbr
        End If
        intLastNdx = n.Nodes.Count - 1
        If (n.Nodes.Count >= 0) Then
            For intNdx = 0 To intLastNdx
                WriteANode(n.Nodes(intNdx))
            Next
        End If
    End Sub

    Function ValidateTree(tree As TreeView, intExpectedNumberOfNodes As Integer, intExpectedMaxNodeNbr As Integer) As Boolean
        Dim intValidateNodeCtr As Integer = 0
        Dim intValidateMaxNodeNbr As Integer = -1
        Dim boolResult As Boolean

        gstrValidationErrorReason = ""

        boolResult = ValidateHelper(tree.Nodes(0), intValidateNodeCtr, intValidateMaxNodeNbr)
        If (Not boolResult) Then
            Return False
        ElseIf (intValidateNodeCtr <> intExpectedNumberOfNodes) Then
            gstrValidationErrorReason = "Wrong number of nodes (seen:" & intValidateNodeCtr & ", expected:" & intExpectedNumberOfNodes & ")"
            Return False
        ElseIf (intValidateMaxNodeNbr <> intExpectedMaxNodeNbr) Then
            gstrValidationErrorReason = "Wrong last node number in use (seen:" & intValidateMaxNodeNbr & ", expected:" & intExpectedMaxNodeNbr & ")"
            Return False
        Else
            Return True
        End If
    End Function

    Function ValidateHelper(n As TreeNode, ByRef intValidateNodeCtr As Integer, ByRef intValidateMaxNodeNbr As Integer) As Boolean
        Dim thisItem, childItem As cToDoItem.sItemInfo
        Dim intNdx, intLastNdx As Integer
        Dim lowerOkay As Boolean
        Dim strElements() As String
        Dim boolIsOrdered As Boolean = False

        intValidateNodeCtr += 1
        thisItem = CType(n.Tag, cToDoItem.sItemInfo)
        L.WriteToLog("Checking node " & thisItem.intNodeNbr, True)
        If (thisItem.intNodeNbr > intValidateMaxNodeNbr) Then
            intValidateMaxNodeNbr = thisItem.intNodeNbr
        End If
        If (thisItem.isChecked <> n.Checked) Then
            gstrValidationErrorReason = "(" & thisItem.intNodeNbr & ") " & thisItem.strText & ": " & "Check state of node does not match tag data"
            Return False
        End If
        intLastNdx = n.Nodes.Count - 1
        ' if this node is ordered, get metainfo for checking order is correct
        If (thisItem.strChildOrder <> "") Then
            If (thisItem.strChildOrder = "-1") Then
                If (n.Nodes.Count <> 0) Then
                    gstrValidationErrorReason = "(" & thisItem.intNodeNbr & ") " & thisItem.strText & ": " & "Node is ordered and expected empty but is not empty"
                    Return False
                End If
            Else
                strElements = Split(thisItem.strChildOrder, ",")
                If ((n.Nodes.Count - 1) <> UBound(strElements)) Then
                    gstrValidationErrorReason = "(" & thisItem.intNodeNbr & ") " & thisItem.strText & ": " & "Node is ordered and not empty but has wrong number of children"
                    Return False
                End If
                boolIsOrdered = True
            End If
        End If
        If (n.Nodes.Count >= 0) Then
            For intNdx = 0 To intLastNdx
                childItem = CType(n.Nodes(intNdx).Tag, cToDoItem.sItemInfo)
                If (childItem.intParentNodeNbr <> thisItem.intNodeNbr) Then
                    gstrValidationErrorReason = "(" & childItem.intNodeNbr & ") " & childItem.strText & ": " & "Child node does not have correct parent"
                    Return False
                End If
                If (boolIsOrdered) Then
                    If (childItem.intNodeNbr <> Convert.ToInt32(strElements(intNdx))) Then
                        gstrValidationErrorReason = "(" & childItem.intNodeNbr & ") " & childItem.strText & ": " & "Child node not in correct order under ordered parent"
                        Return False
                    End If
                End If
                lowerOkay = ValidateHelper(n.Nodes(intNdx), intValidateNodeCtr, intValidateMaxNodeNbr)
                If (Not lowerOkay) Then
                    Return False
                End If
            Next
        End If
        Return True
    End Function
    Public Sub ResortAndRedisplayParent(tree As TreeView, ofNode As TreeNode)
        Dim parent As TreeNode
        Dim item As cToDoItem.sItemInfo
        Dim intNdx, intLastNdx As Integer
        Dim intNdx2, intlastNdx2 As Integer
        Dim intSortListNdx, intSortListLastNdx As Integer

        Dim sdict As New cSortableDictionaryWithDupKeys
        Dim strKey As String
        Dim intValue As Integer
        Dim intValues() As Integer
        Dim aNodes() As TreeNode
        'Dim tmpNode As TreeNode
        'Dim expandedParentNode As TreeNode
        'Dim intCurrentNodeNbr As Integer
        Dim rememberSelectedNode As TreeNode

        item = CType(ofNode.Tag, cToDoItem.sItemInfo)
        L.WriteToLog("In ReSort(): parent of :" & item.strText, True)
        ' 03/24/2021 due to odd behavior when a node's position changes due to sort order change
        ' remember currently selected node if any
        rememberSelectedNode = tree.SelectedNode()
        L.WriteToLog("In ReSort(): clear selected node: " & TODO.DebugGetNodeDescr(rememberSelectedNode), True)
        tree.SelectedNode = Nothing
        ' change 03/22/2021 this was used incorrectly to make first son of parent node selected at the end
        'intCurrentNodeNbr = item.intNodeNbr
        ' instead we don't choose a new selected node in this routine

        parent = TODO.FindNodeByNodeNbr(tree, item.intParentNodeNbr)
        If (parent Is Nothing) Then
            Throw New Exception("Parent not found")
        End If

        ' new : if an ordered parent, order by the list in the node
        item = CType(parent.Tag, cToDoItem.sItemInfo)
        If (item.strChildOrder <> "") Then
            Dim strElements() As String
            Dim intWantedChildNodeNbr As Integer
            If (item.strChildOrder <> "-1") Then
                strElements = Split(item.strChildOrder, ",")
                intLastNdx = parent.Nodes.Count - 1
                If (intLastNdx <> UBound(strElements)) Then
                    Throw New Exception("ResortAndRedisplayParent: Order list element count does not match child node count")
                End If
                ReDim aNodes(intLastNdx)
                    For intNdx = 0 To intLastNdx
                        aNodes(intNdx) = parent.Nodes(intNdx)
                    Next
                    parent.Nodes.Clear()
                For intNdx = 0 To intLastNdx
                    intWantedChildNodeNbr = Convert.ToInt32(strElements(intNdx))
                    For intNdx2 = 0 To intLastNdx
                        item = CType(aNodes(intNdx2).Tag, cToDoItem.sItemInfo)
                        If (item.intNodeNbr = intWantedChildNodeNbr) Then
                            parent.Nodes.Add(aNodes(intNdx2))
                            intWantedChildNodeNbr = -1
                            Exit For
                        End If
                    Next
                    If (intWantedChildNodeNbr <> -1) Then
                        Throw New Exception("Child of ordered node not found")
                    End If
                Next
            Else
                If (parent.Nodes.Count <> 0) Then
                    Throw New Exception("ResortAndRedisplayParent: Order list is empty but there are children nodes")
                End If
            End If
            tree.SelectedNode = rememberSelectedNode
            L.WriteToLog("In ReSort(): restore selected node: " & TODO.DebugGetNodeDescr(rememberSelectedNode), True)
            tree.SelectedNode = rememberSelectedNode
            L.WriteToLog("In ReSort(): Finished", True)
            Exit Sub
        End If

        '' node num: 8 digits (last, smaller values higher)(19..26)
        '' pri: 3 digits (3rd highest, larger values higher,zero is lower)(16..18)
        '' date of event: 8 chars (highest, earlier dates higher,null is lower)(0..7)
        '' bump to top date: 8 chars (2nd highest, earlier dates higher, null is lower)(8..15)
        ' key order: (date of event 0..7)(bump to top date 8..15)(pri 16..18)(node num 19..26)
        ' change to that 03/24/2021

        intLastNdx = parent.Nodes.Count - 1
        For intNdx = 0 To intLastNdx
            item = CType(parent.Nodes(intNdx).Tag, cToDoItem.sItemInfo)
            strKey = ""

            If (item.dtDateOfEvent <> gdtNull) Then
                strKey &= Format(item.dtDateOfEvent, "yyyyMMdd")
            Else
                strKey &= "________"
            End If
            If (item.dtDateOfBumpToTop <> gdtNull) Then
                strKey &= Format(item.dtDateOfBumpToTop, "yyyyMMdd")
            Else
                strKey &= "________"
            End If
            strKey &= Format(item.intPriority, "000")
            strKey &= Format(item.intNodeNbr, "00000000")

            intValue = item.intNodeNbr
            sdict.AddValueForKey(strKey, intValue)
        Next

        sdict.SortDictionary(True)

        ReDim aNodes(intLastNdx)

        intNdx = 0
        intSortListLastNdx = sdict.SortedCount - 1

        Dim strA As String = ""

        For intSortListNdx = 0 To intSortListLastNdx
            intValues = sdict.GetValuesForSortedIndex(intSortListNdx)
            strA = ""
            intlastNdx2 = UBound(intValues)
            For intNdx2 = 0 To intlastNdx2
                strA &= intValues(intNdx2)
            Next

            For intNdx2 = 0 To intlastNdx2
                aNodes(intNdx) = TODO.FindNodeByNodeNbr(tree, intValues(intNdx2))
                If (aNodes(intNdx) Is Nothing) Then
                    Throw New Exception("Error #1 during sort processing")
                End If
                intNdx += 1
            Next
        Next
        If (intNdx <> (intLastNdx + 1)) Then
            Throw New Exception("Error #2 during sort processing")
        End If
        ' *** address this later, nested begin/end when called from Move transaction
        'tree.BeginUpdate()
        L.WriteToLog("In ReSort(): Rebuild node with items sorted", True)
        parent.Nodes.Clear()
        For intNdx = 0 To intLastNdx
            parent.Nodes.Add(aNodes(intNdx))
        Next
        'tmpNode = TODO.FindNodeByNodeNbr(tree, intCurrentNodeNbr)
        'tree.SelectedNode = tmpNode
        'tree.SelectedNode = parent
        'tree.EndUpdate()
        L.WriteToLog("In ReSort(): restore selected node: " & TODO.DebugGetNodeDescr(rememberSelectedNode), True)
        tree.SelectedNode = rememberSelectedNode
        L.WriteToLog("In ReSort(): Finished", True)
    End Sub

End Class
