Option Explicit On
Option Strict On
Public Class cToDoItem

    Private gdtNull As Date = DateValue("01/01/1900")

    Private UGBL As New cUtility
    Public Structure sItemInfo
        Dim intNodeNbr As Integer
        Dim intParentNodeNbr As Integer
        Dim dtCreated As Date
        Dim dtModified As Date
        Dim strText As String
        Dim strNotes As String
        Dim dtDateOfEvent As Date
        Dim dtDateOfBumpToTop As Date
        Dim intPriority As Integer
        Dim isChecked As Boolean
        Dim strChildOrder As String
    End Structure

    Public Function CreateNewItem(intNewNodeNbr As Integer) As sItemInfo
        Dim item As New sItemInfo

        With item
            .intNodeNbr = intNewNodeNbr
            .intParentNodeNbr = -1
            .dtCreated = Date.Now
            .dtModified = gdtNull
            .strText = "(Undefined)"
            .strNotes = ""
            .dtDateOfEvent = gdtNull
            .dtDateOfBumpToTop = gdtNull
            .intPriority = 0
            .isChecked = False
            .strChildOrder = ""
        End With

        Return item
    End Function

    Public Sub MoveNode(tree As TreeView, theNode As TreeNode, toParent As TreeNode)
        ' 05/02/2021 modify so if the old parent or the new parent are ordered, it is handled properly
        Dim theNodeItem As sItemInfo
        Dim theParentNodeItem As sItemInfo
        Dim theToParentItem As sItemInfo
        Dim intNodeCurrentParentNbr As Integer
        Dim theCurrentParent As TreeNode

        theNodeItem = CType(theNode.Tag, cToDoItem.sItemInfo)
        theToParentItem = CType(toParent.Tag, cToDoItem.sItemInfo)
        If (theToParentItem.intNodeNbr = 3) Then
            ' fix (we will see), if deleting a node, clear it's selection state ... no node will remain selected
            tree.SelectedNode = Nothing
            ' doesn't fire before select/after select in main, so clear any highlighting manually
            theNode.BackColor = Color.White
        End If
        intNodeCurrentParentNbr = theNodeItem.intParentNodeNbr
        If (intNodeCurrentParentNbr > -1) Then
            theCurrentParent = FindNodeByNodeNbr(tree, intNodeCurrentParentNbr)
            theCurrentParent.Nodes.Remove(theNode)
            theParentNodeItem = CType(theCurrentParent.Tag, cToDoItem.sItemInfo)
            If (theParentNodeItem.strChildOrder <> "") Then
                BuildUpdatedOrderListInThisParentNode(theCurrentParent)
            End If
        End If
        theNodeItem.intParentNodeNbr = theToParentItem.intNodeNbr
        theNode.Tag = theNodeItem
        toParent.Nodes.Add(theNode)
        theParentNodeItem = CType(toParent.Tag, cToDoItem.sItemInfo)
        If (theParentNodeItem.strChildOrder <> "") Then
            BuildUpdatedOrderListInThisParentNode(toParent)
        End If
    End Sub

    Function GetDisplayTextForItem(item As sItemInfo) As String
        Dim strTemp As String = ""

        If (item.strChildOrder <> "") Then
            strTemp &= "(ORDERED)"
        End If

        If (item.intPriority > 0) Then
            strTemp &= "(PRI:" & item.intPriority & ")"
        End If
        If (item.dtDateOfBumpToTop <> gdtNull) Then
            strTemp &= "(Bump To Top:" & Format(item.dtDateOfBumpToTop, "ddd MM/dd/yyyy") & ")"
        End If
        If (item.dtDateOfEvent <> gdtNull) Then
            strTemp &= "(Date of Event:" & Format(item.dtDateOfEvent, "ddd MM/dd/yyyy") & ")"
        End If

        If (strTemp <> "") Then
            strTemp = strTemp & " " & item.strText
        Else
            strTemp = item.strText
        End If

        Return strTemp
    End Function

    Public Function EncodeItem(item As sItemInfo) As String
        Dim strOutput As String = ""

        With item
            strOutput &= .intNodeNbr &
                ":" & .intParentNodeNbr &
                ":" & UGBL.EncodeToAvoid(.strText, ":") &
                ":" & UGBL.EncodeToAvoid(.strNotes, ":") &
                ":" & .intPriority.ToString &
                ":" & UGBL.BoolToYN(.isChecked) &
                ":" & UGBL.ConvertToCompactDate(.dtDateOfEvent) &
                ":" & UGBL.ConvertToCompactDate(.dtDateOfBumpToTop) &
                ":" & UGBL.ConvertToCompactDateAndTime(.dtCreated) &
                ":" & UGBL.ConvertToCompactDateAndTime(.dtModified)
            If (.strChildOrder <> "") Then
                strOutput &= ":" & .strChildOrder
            End If
        End With

        Return strOutput
    End Function

    Public Function DecodeItem(strItemData As String) As sItemInfo
        Dim strElements() As String
        Dim sNew As New sItemInfo

        strElements = Split(strItemData, ":")
        If (Not ((UBound(strElements) = 9) Or (UBound(strElements) = 10))) Then
            Throw New Exception("Bad format for item data as string")
        End If
        With sNew
            .intNodeNbr = Convert.ToInt32(strElements(0))
            .intParentNodeNbr = Convert.ToInt32(strElements(1))
            .strText = UGBL.DecodeFromAvoid(strElements(2))
            .strNotes = UGBL.DecodeFromAvoid(strElements(3))
            .intPriority = Convert.ToInt32(strElements(4))
            .isChecked = UGBL.YNToBool(strElements(5))
            .dtDateOfEvent = UGBL.ConvertFromCompactDate(strElements(6))
            .dtDateOfBumpToTop = UGBL.ConvertFromCompactDate(strElements(7))
            .dtCreated = UGBL.ConvertFromCompactDateAndTime(strElements(8))
            .dtModified = UGBL.ConvertFromCompactDateAndTime(strElements(9))
            If (UBound(strElements) = 10) Then
                .strChildOrder = strElements(10)
            Else
                .strChildOrder = ""
            End If
        End With

        Return sNew
    End Function
    '
    ' Find nodes
    '
    Public Function FindNodeByNodeNbr(tv As TreeView, intNodeNbr As Integer) As TreeNode
        Dim n As TreeNode
        Dim item_check As sItemInfo

        item_check = CType(tv.Nodes(0).Tag, cToDoItem.sItemInfo)
        If (item_check.intNodeNbr <> 0) Or (item_check.intParentNodeNbr <> -1) Then
            Throw New Exception("Invalid hidden root")
        End If
        ' *** this works because by design, root has just one node and it has every other node as a descendant
        n = FindNodeByNodeNbrHelper(tv.Nodes(0), intNodeNbr)

        Return n
    End Function

    Private Function FindNodeByNodeNbrHelper(n As TreeNode, intNodeNbrToFind As Integer) As TreeNode
        Dim item As sItemInfo
        Dim intNdx As Integer
        Dim intLastNdx As Integer
        Dim rn As TreeNode

        item = CType(n.Tag, cToDoItem.sItemInfo)
        If (item.intNodeNbr = intNodeNbrToFind) Then
            Return n
        Else
            intLastNdx = n.Nodes.Count - 1
            If (intLastNdx >= 0) Then
                For intNdx = 0 To intLastNdx
                    rn = FindNodeByNodeNbrHelper(n.Nodes(intNdx), intNodeNbrToFind)
                    If (Not (rn Is Nothing)) Then
                        Return rn
                    End If
                Next
            End If
            Return Nothing
        End If
    End Function

    Public Function DebugGetNodeDescr(n As TreeNode) As String
        Dim item As cToDoItem.sItemInfo

        If (IsNothing(n)) Then
            Return "(nil)"
        Else
            item = CType(n.Tag, cToDoItem.sItemInfo)
            Return item.strText
        End If
    End Function

    Public Function GetDiffDescr(itemOld As sItemInfo, itemNew As sItemInfo) As String
        Dim strDescr As String = ""

        If (itemOld.strText <> itemNew.strText) Then
            strDescr &= "Text changed" & vbCrLf & "  Old:" & itemOld.strText & vbCrLf & "  New:" & itemNew.strText & vbCrLf
        End If
        If (itemOld.strNotes <> itemNew.strNotes) Then
            strDescr &= "Notes changed" & vbCrLf
        End If
        If (itemOld.dtDateOfEvent <> itemNew.dtDateOfEvent) Then
            strDescr &= "Date of event changed" & vbCrLf & "  Old:" & UGBL.FormatDateAsText(itemOld.dtDateOfEvent) & vbCrLf & "  New:" & UGBL.FormatDateAsText(itemNew.dtDateOfEvent) & vbCrLf
        End If
        If (itemOld.dtDateOfBumpToTop <> itemNew.dtDateOfBumpToTop) Then
            strDescr &= "Bump To Top Date changed" & vbCrLf & "  Old:" & UGBL.FormatDateAsText(itemOld.dtDateOfBumpToTop) & vbCrLf & "  New:" & UGBL.FormatDateAsText(itemNew.dtDateOfBumpToTop) & vbCrLf
        End If
        If (itemOld.intPriority <> itemNew.intPriority) Then
            strDescr &= "Priority changed" & vbCrLf & "  Old:" & itemOld.intPriority & vbCrLf & "  New:" & itemNew.intPriority & vbCrLf
        End If

        If (strDescr = "") Then
            strDescr = "No differences"
        Else
            strDescr = strDescr.Substring(0, strDescr.Length - 2)
        End If

        Return strDescr
    End Function
    '
    '
    Public Function ParentOf(tv As TreeView, node As TreeNode) As TreeNode
        Dim item As cToDoItem.sItemInfo
        Dim parentNode As TreeNode
        item = CType(node.Tag, cToDoItem.sItemInfo)
        If (item.intParentNodeNbr <> -1) Then
            parentNode = FindNodeByNodeNbr(tv, item.intParentNodeNbr)
        Else
            parentNode = Nothing '
        End If
        Return parentNode
    End Function

    Public Function isOrdered(node As TreeNode) As Boolean
        Dim item As cToDoItem.sItemInfo
        If (IsNothing(node)) Then
            Return False
        End If
        item = CType(node.Tag, cToDoItem.sItemInfo)
        Return (item.strChildOrder <> "")
    End Function

    Public Function isAnOrderedChild(tv As TreeView, node As TreeNode) As Boolean
        ' ** will assume node is valid and has a parent (may only be of concern for special nodes / check here, or before called?)
        Dim item As cToDoItem.sItemInfo
        Dim parentNode As TreeNode
        item = CType(node.Tag, cToDoItem.sItemInfo)
        parentNode = FindNodeByNodeNbr(tv, item.intParentNodeNbr)
        item = CType(parentNode.Tag, cToDoItem.sItemInfo)
        Return (item.strChildOrder <> "")
    End Function

    Public Sub BuildUpdatedOrderListInThisParentNode(node As TreeNode)
        Dim item, thisItem As cToDoItem.sItemInfo
        Dim intNdx, intLastndx As Integer
        item = CType(node.Tag, cToDoItem.sItemInfo)
        If (item.strChildOrder = "") Then
            Throw New Exception("BuildUpdatedOrderListInThisParentNode called for non-ordered node")
        End If
        item.strChildOrder = ""
        intLastndx = node.Nodes.Count - 1
        For intNdx = 0 To intLastndx
            thisItem = CType(node.Nodes(intNdx).Tag, cToDoItem.sItemInfo)
            If (intNdx > 0) Then
                item.strChildOrder &= ","
            End If
            item.strChildOrder &= thisItem.intNodeNbr
        Next
        If (item.strChildOrder = "") Then
            item.strChildOrder = "-1"
        End If
        node.Tag = item
    End Sub
End Class
