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
        End With

        Return item
    End Function

    Public Sub MoveNode(tree As TreeView, theNode As TreeNode, toParent As TreeNode)
        Dim theNodeItem As sItemInfo
        Dim theToParentItem As sItemInfo
        Dim intNodeCurrentParentNbr As Integer
        Dim theCurrentParent As TreeNode

        theNodeItem = CType(theNode.Tag, cToDoItem.sItemInfo)
        theToParentItem = CType(toParent.Tag, cToDoItem.sItemInfo)

        intNodeCurrentParentNbr = theNodeItem.intParentNodeNbr
        If (intNodeCurrentParentNbr > -1) Then
            theCurrentParent = FindNodeByNodeNbr(tree, intNodeCurrentParentNbr)
            theCurrentParent.Nodes.Remove(theNode)
        End If
        theNodeItem.intParentNodeNbr = theToParentItem.intNodeNbr
        theNode.Tag = theNodeItem
        toParent.Nodes.Add(theNode)
    End Sub

    Function GetDisplayTextForItem(item As sItemInfo) As String
        Dim strTemp As String = ""

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
        End With

        Return strOutput
    End Function

    Public Function DecodeItem(strItemData As String) As sItemInfo
        Dim strElements() As String
        Dim sNew As New sItemInfo

        strElements = Split(strItemData, ":")
        If (UBound(strElements) <> 9) Then
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
End Class
