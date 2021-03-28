Public Class cTrnLog
    ' Allow this to be instanced for live writing of TRN log as list is edited, OR for TrnLog maintenance (fix crashes etc)
    Public G As cGlobals

    Public gtvViewToUse As TreeView

    Public gintLastNodeNbr As Integer   ' Initialize?  It's local to our instance
    Private gMoveToParent As nothing
    Private gRootNode As nothing    ' OUTPUT
    '
    Sub ApplyTransaction(strTransactionName As String, theItem As Form1.sItemInfo, theNode As TreeNode)
        ApplyTransactionHandler(True, strTransactionName, theItem, theNode, Nothing)
    End Sub

    Sub ApplyTransactionViaMetaData(strTransactionName As String, strAttributeElements As String())
        ApplyTransactionHandler(False, strTransactionName, Nothing, Nothing, strAttributeElements)
    End Sub
    Sub ApplyTransactionHandler(boolLiveMode As Boolean, strTransactionName As String, thePassedItem As Form1.sItemInfo, thePassedNode As TreeNode, strAttributeElements As String())
        ' If boolLiveMode is true, use strTransactionName / thePassedItem - > theItem / thePassedNode -> theNode
        ' Else (replay mode) use strTransactionName, strAttributeElements()
        Dim localItem As Form1.sItemInfo
        Dim localItem2 As Form1.sItemInfo
        Dim aNode As TreeNode
        Dim bNode As TreeNode
        Dim strAux As String

        Dim theItem As Form1.sItemInfo
        Dim theNode As TreeNode

        Select Case strTransactionName.ToUpper
            Case "OPEN"
                'ApplyTransaction("Open", Nothing, Nothing)
                If (Not boolLiveMode) Then

                End If
                ' Set up data needed for transaction record
                If (boolLiveMode) Then
                    gfTransactionFile.WriteLine("<OPEN>")
                    gfTransactionFile.Flush()
                End If
                ' Perform Transaction
            Case "INIT"
                'ApplyTransaction("Init", Nothing, Nothing)
                If (Not boolLiveMode) Then

                End If
                ' Set up data needed for transaction record
                If (boolLiveMode) Then
                    gfTransactionFile.WriteLine("<INIT>")
                    gfTransactionFile.Flush()
                End If
                ' Perform Transaction

                gintLastNodeNbr = -1
                gtvViewToUse.Nodes.Clear()

                localItem = CreateNewItem() ' ID 0
                localItem.strText = gstrCurrentPIMFileBasename
                aNode = New TreeNode()
                gRootNode = aNode
                aNode.Text = localItem.strText
                aNode.Tag = localItem
                tvMain.Nodes.Add(aNode)

                localItem = CreateNewItem() ' ID 1
                'localItem.eNodeType = eNodeTypes.eValueQuickAdd
                localItem.intParentNodeNbr = 0
                localItem.strText = "QuickAdd"
                aNode = New TreeNode()
                aNode.Text = localItem.strText
                aNode.Tag = localItem
                tvMain.Nodes(0).Nodes.Add(aNode)

                localItem = CreateNewItem() 'ID 2
                'localItem.eNodeType = eNodeTypes.eValueRoot
                localItem.intParentNodeNbr = 0
                localItem.strText = "ToDo"
                aNode = New TreeNode()
                aNode.Text = localItem.strText
                aNode.Tag = localItem
                tvMain.Nodes(0).Nodes.Add(aNode)

                localItem = CreateNewItem() 'ID 3
                'localItem.eNodeType = eNodeTypes.eValueTrash
                localItem.intParentNodeNbr = 0
                localItem.strText = "Trash"
                aNode = New TreeNode()
                aNode.Text = localItem.strText
                aNode.Tag = localItem
                tvMain.Nodes(0).Nodes.Add(aNode)

            Case "SAVING"
                ' ApplyTransaction("Saving", Nothing, Nothing)
                If (Not boolLiveMode) Then

                End If
                ' Set up data needed for transaction record
                If (boolLiveMode) Then
                    gfTransactionFile.WriteLine("<SAVING " & (gintLastVersionWritten + 1) & ">")
                    gfTransactionFile.Flush()
                End If
                ' Perform Transaction
            Case "SAVED"
                'ApplyTransaction("Saved", Nothing, Nothing)
                If (Not boolLiveMode) Then

                End If
                ' Set up data needed for transaction record
                If (boolLiveMode) Then
                    gfTransactionFile.WriteLine("<SAVED " & gintLastVersionWritten & ">")
                    gfTransactionFile.Flush()
                End If
                ' Perform Transaction
                WriteToLog("Database Saved")
            'Case "Writing"
            '    gfTransactionFile.WriteLine("<WRITING>")
            '    gfTransactionFile.Flush()
            'Case "Written"
            '    gfTransactionFile.WriteLine("<WRITTEN>")
            '    gfTransactionFile.Flush()
            Case "CLOSE"
                If (Not boolLiveMode) Then

                End If
                ' Set up data needed for transaction record
                If (boolLiveMode) Then
                    gfTransactionFile.WriteLine("<CLOSE>")
                    gfTransactionFile.Flush()
                End If
                ' Perform Transaction
            Case "ADD"
                If (Not boolLiveMode) Then

                End If
                ' Set up data needed for transaction record
                If (boolLiveMode) Then
                    gfTransactionFile.WriteLine("<ADD " & strAux & ">")
                    gfTransactionFile.Flush()
                End If
                ' Perform Transaction
                'ApplyTransaction("Add", item, parent)
                strAux = theItem.intNodeNbr & ":" & theItem.intParentNodeNbr & ":" & theItem.strText & ":" & theItem.intPriority & ":" & BoolToYN(theItem.isChecked)
                aNode = FindNodeByNodeNbr(theItem.intParentNodeNbr)
                If (aNode Is Nothing) Then
                    Throw New Exception("Could not find parent")
                End If
                localItem = aNode.Tag
                ' Add the new node in the appropriate place
                aNode = New TreeNode
                aNode.Text = GetDisplayTextForItem(theItem)
                aNode.Tag = theItem
                theNode.Nodes.Add(aNode)
                WriteToLog("Added (" & theItem.strText & ") to (" & localItem.strText & ")")
            Case "DELETE"
                If (Not boolLiveMode) Then

                End If
                ' Set up data needed for transaction record
                If (boolLiveMode) Then
                    gfTransactionFile.WriteLine("<DELETE " & theItem.intNodeNbr & ">")
                    gfTransactionFile.Flush()
                End If
                ' Perform Transaction
                'ApplyTransaction("Delete", localItem, Nothing)
                'theItem: (tag for node) to be moved
                aNode = FindNodeByNodeNbr(theItem.intNodeNbr)
                MoveNode(aNode, FindSpecialNode(eNodeTypes.eValueTrash))
                WriteToLog("Deleted (" & theItem.strText & ")")
            Case "MOVESETPARENT"
                If (Not boolLiveMode) Then

                End If
                ' Set up data needed for transaction record
                If (boolLiveMode) Then
                    gfTransactionFile.WriteLine("<MOVESETPARENT " & strAux & ">")
                    gfTransactionFile.Flush()
                End If
                ' Perform Transaction

                gMoveToParent = theNode
                localItem = gMoveToParent.tag
                strAux = localItem.intNodeNbr
            Case "MOVE"
                ' ApplyTransaction("Move", Nothing, currNode) but has global too
                ' *** protect against moving special nodes
                If (Not boolLiveMode) Then

                End If
                ' Set up data needed for transaction record
                If (boolLiveMode) Then
                    gfTransactionFile.WriteLine("<MOVE " & strAux & ">")
                    gfTransactionFile.Flush()
                End If
                ' Perform Transaction
                localItem = theNode.Tag
                localItem2 = gMoveToParent.Tag

                strAux = localItem.intNodeNbr & ":" & localItem2.intNodeNbr
                gfTransactionFile.Flush()
                tvMain.BeginUpdate()
                ' grab the node we are moving
                aNode = theNode
                ' remove it from its current parent
                bNode = FindNodeByNodeNbr(localItem.intParentNodeNbr)
                bNode.Nodes.Remove(aNode)

                ' add to new parent
                aNode = theNode 'redundant?
                localItem.intParentNodeNbr = localItem2.intNodeNbr
                aNode.Tag = localItem   ' redundant?
                gMoveToParent.Nodes.Add(aNode)
                ' redisplay both subtrees
                ResortAndRedisplayParent(bNode)
                ResortAndRedisplayParent(aNode)
                tvMain.EndUpdate()
                Application.DoEvents()
                gBoolBypassEvents = False
                WriteToLog("Moved (" & localItem.strText & ") to (" & localItem2.strText & ")")
            Case "PRIUP"
                'ApplyTransaction("PriUp", Nothing, currNode)
                If (Not boolLiveMode) Then

                End If
                ' Set up data needed for transaction record
                If (boolLiveMode) Then
                    gfTransactionFile.WriteLine("<PRIUP " & strAux & ">")
                    gfTransactionFile.Flush()
                End If
                ' Perform Transaction
                If (Not boolLiveMode) Then

                Else

                End If
                localItem = theNode.Tag
                strAux = localItem.intNodeNbr
                gfTransactionFile.Flush()
                WriteToLog("Increase Priority " & localItem.intPriority & " to " & localItem.intPriority + 1 & " (" & localItem.strText & ")")
                localItem.intPriority += 1
                theNode.Text = GetDisplayTextForItem(localItem)
                theNode.Tag = localItem
                ResortAndRedisplayParent(theNode)
            Case "PRIDOWN"
                ' ApplyTransaction("PriDown", Nothing, currNode)
                If (Not boolLiveMode) Then

                End If
                ' Set up data needed for transaction record
                If (boolLiveMode) Then
                    gfTransactionFile.WriteLine("<PRIDOWN " & strAux & ">")
                    gfTransactionFile.Flush()
                End If
                ' Perform Transaction
                If (Not boolLiveMode) Then

                Else

                End If
                localItem = theNode.Tag
                strAux = localItem.intNodeNbr
                gfTransactionFile.Flush()
                WriteToLog("Decrease Priority " & localItem.intPriority & " to " & localItem.intPriority - 1 & " (" & localItem.strText & ")")
                localItem.intPriority -= 1
                theNode.Text = GetDisplayTextForItem(localItem)
                theNode.Tag = localItem
                ResortAndRedisplayParent(theNode)
            Case Else
                Throw New Exception("Unknown Transaction Name")
        End Select
    End Sub

End Class
