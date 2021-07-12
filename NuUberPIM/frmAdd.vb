Option Explicit On
Option Strict On
Public Class frmAdd

    Private gdtNull As Date = DateValue("01/01/1900")
    Public Structure sMatchItem
        Dim strItemText As String
        Dim intNodeNbr As Integer
        Dim intWordMatchCnt As Integer
        Dim dtMostImportantDate As Date
    End Structure

    Public tv As TreeView
    Public gBoolCancel As Boolean = True
    Public gintGoToNodeNbr As Integer = -1
    Public item As cToDoItem.sItemInfo
    Public mstrCurrentMasterTagList As String
    Dim gstrCurrentTargetWords() As String
    Dim gboolInTrashTree As Boolean = False
    Dim Matches As New List(Of sMatchItem)
    Dim TODO As New cToDoItem
    Private UGBL As New cUtility

    Class MatchCompare : Implements IComparer(Of sMatchItem)
        Public Function Compare(ByVal i1m As sMatchItem, ByVal i2m As sMatchItem) As Integer Implements IComparer(Of sMatchItem).Compare
            'Dim i1m, i2m As sMatchItem
            'i1m = CType(i1, sMatchItem)
            'i2m = CType(i2, sMatchItem)
            If (i1m.intWordMatchCnt = i2m.intWordMatchCnt) Then
                Return StrComp(i1m.strItemText, i2m.strItemText)
            ElseIf (i1m.intWordMatchCnt < i2m.intWordMatchCnt) Then
                Return 1
            Else
                Return -1
            End If
        End Function

    End Class

    Class MatchDateCompare : Implements IComparer(Of sMatchItem)
        Public Function Compare(ByVal i1m As sMatchItem, ByVal i2m As sMatchItem) As Integer Implements IComparer(Of sMatchItem).Compare
            'Dim i1m, i2m As sMatchItem
            'i1m = CType(i1, sMatchItem)
            'i2m = CType(i2, sMatchItem)
            Dim intResult As Long

            intResult = DateDiff(DateInterval.Day, i1m.dtMostImportantDate, i2m.dtMostImportantDate)
            If (intResult = 0) Then
                Return 0
            ElseIf (intResult > 0) Then
                Return -1
            Else
                Return 1
            End If
        End Function

    End Class

    Class MatchTagCompare : Implements IComparer(Of sMatchItem)
        Public Function Compare(ByVal i1m As sMatchItem, ByVal i2m As sMatchItem) As Integer Implements IComparer(Of sMatchItem).Compare
            'Dim i1m, i2m As sMatchItem
            'i1m = CType(i1, sMatchItem)
            'i2m = CType(i2, sMatchItem)

            If (i1m.intNodeNbr < i2m.intNodeNbr) Then
                Return -1
            Else
                Return 1
            End If
        End Function

    End Class
    Private Sub frmAdd_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' May be unneeded, look at load behavior, triggered on Show or ShowDialog
        ' item is blank on entry so should be ok
        tbNewItem.Text = ""
        '
        LoadTagList()
    End Sub

    Sub LoadTagList()
        Dim intNdx, intLastNdx As Integer
        Dim strTagListElements() As String

        clbTags.Items.Clear()
        If (mstrCurrentMasterTagList <> "") Then
            strTagListElements = Split(mstrCurrentMasterTagList, ",")
            intLastNdx = UBound(strTagListElements)
            For intNdx = 0 To intLastNdx
                clbTags.Items.Add(strTagListElements(intNdx))
            Next
        End If
    End Sub
    Private Sub tbNewItem_TextChanged(sender As Object, e As EventArgs) Handles tbNewItem.TextChanged
        GenerateMatches()
    End Sub

    Sub GenerateMatches()
        Dim intNdx, intLastNdx As Integer
        Dim strSearchstring As String
        strSearchstring = tbNewItem.Text.Trim.ToUpper.Replace("  ", " ")
        If (strSearchstring.Length < 3) Then
            Exit Sub
        End If
        gstrCurrentTargetWords = Split(strSearchstring, " ")
        lbMatches.Items.Clear()
        Matches.Clear()
        gboolInTrashTree = False
        FindStringInNodes(tv.Nodes(0))
        intLastNdx = Matches.Count - 1
        If (intLastNdx >= 0) Then
            ' sort the list based on number of word matches first then alphabetically
            Matches.Sort(New MatchCompare)
            '
            For intNdx = 0 To intLastNdx
                lbMatches.Items.Add(Matches(intNdx).intWordMatchCnt & ") " & Matches(intNdx).strItemText)
            Next
        End If
    End Sub
    Sub FindStringInNodes(n As TreeNode)
        Dim item As cToDoItem.sItemInfo
        Dim match_item As sMatchItem
        Dim intNdx, intLastNdx As Integer
        Dim intNdx2, intLastNdx2 As Integer
        Dim strThisItem As String
        Dim strThisItemWords() As String
        Dim intMatchCnt As Integer = 0
        item = CType(n.Tag, cToDoItem.sItemInfo)
        If (item.intNodeNbr = 3) Then
            gboolInTrashTree = True
        End If
        strThisItem = ""
        If (cbPartialWordMatches.Checked) Then
            For intNdx = 0 To UBound(gstrCurrentTargetWords)
                If (item.strText.Trim.ToUpper.IndexOf(gstrCurrentTargetWords(intNdx)) <> -1) Then
                    intMatchCnt += 1
                End If
            Next
        Else
            strThisItemWords = Split(item.strText.Trim.ToUpper.Replace("  ", " "), " ")
            intLastNdx2 = UBound(strThisItemWords)
            For intNdx = 0 To UBound(gstrCurrentTargetWords)
                For intNdx2 = 0 To intLastNdx2
                    If (strThisItemWords(intNdx2) = gstrCurrentTargetWords(intNdx)) Then
                        intMatchCnt += 1
                        Exit For
                    End If
                Next
            Next
        End If

        If (intMatchCnt <> 0) Then
            If (gboolInTrashTree) Then
                strThisItem &= "(DELETED) "
            End If
            strThisItem &= item.strText
            match_item = New sMatchItem
            match_item.strItemText = strThisItem
            match_item.intNodeNbr = item.intNodeNbr
            match_item.intWordMatchCnt = intMatchCnt
            Matches.Add(match_item)
        End If
        intLastNdx = n.Nodes.Count - 1
        If (intLastNdx >= 0) Then
            For intNdx = 0 To intLastNdx
                FindStringInNodes(n.Nodes(intNdx))
            Next
        End If
    End Sub

    Sub FindNodesWithDates(n As TreeNode)
        Dim item As cToDoItem.sItemInfo
        Dim match_item As sMatchItem
        Dim intNdx, intLastNdx As Integer
        item = CType(n.Tag, cToDoItem.sItemInfo)
        If (item.dtDateOfEvent <> gdtnull) Or (item.dtDateOfBumpToTop <> gdtnull) Then
            match_item = New sMatchItem
            match_item.strItemText = TODO.GetDisplayTextForItem(item)
            match_item.intNodeNbr = item.intNodeNbr
            match_item.intWordMatchCnt = 0
            If (item.dtDateOfEvent <> gdtNull) Then
                match_item.dtMostImportantDate = item.dtDateOfEvent
            Else
                match_item.dtMostImportantDate = item.dtDateOfBumpToTop
            End If
            Matches.Add(match_item)
        End If

        intLastNdx = n.Nodes.Count - 1
        If (intLastNdx >= 0) Then
            For intNdx = 0 To intLastNdx
                FindNodesWithDates(n.Nodes(intNdx))
            Next
        End If
    End Sub

    Sub FindNodesWithTags(n As TreeNode, strTags As String)
        Dim item As cToDoItem.sItemInfo
        Dim match_item As sMatchItem
        Dim intNdx, intLastNdx As Integer
        Dim intResult As Integer
        item = CType(n.Tag, cToDoItem.sItemInfo)
        If (item.strTags <> "") Then
            intResult = UGBL.CompareCSL(item.strTags, strTags)
            If (intResult <> 0) Then
                match_item = New sMatchItem
                match_item.strItemText = TODO.GetDisplayTextForItem(item)
                match_item.intNodeNbr = item.intNodeNbr
                match_item.intWordMatchCnt = 0
                Matches.Add(match_item)
            End If
        End If

        intLastNdx = n.Nodes.Count - 1
        If (intLastNdx >= 0) Then
            For intNdx = 0 To intLastNdx
                FindNodesWithTags(n.Nodes(intNdx), strTags)
            Next
        End If
    End Sub
    ' handle transitons from and to edit form (text copy)
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        gBoolCancel = False
        item.strText = tbNewItem.Text.Trim
        If (item.strText = "") Then
            gBoolCancel = True
        End If
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        gBoolCancel = True
        Me.Close()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim fEdit As New frmEdit
        item.strText = tbNewItem.Text.Trim
        fEdit.sOriginal = item
        fEdit.mCurrentMasterTagList = mstrCurrentMasterTagList
        fEdit.ShowDialog()
        If (Not fEdit.gboolDidCancel) Then
            mstrCurrentMasterTagList = fEdit.mCurrentMasterTagList
            item = fEdit.sNew
            ' *** may or may not trigger text changed
            tbNewItem.Text = item.strText
        End If
    End Sub

    Private Sub cbPartialWordMatches_CheckedChanged(sender As Object, e As EventArgs) Handles cbPartialWordMatches.CheckedChanged
        GenerateMatches()
    End Sub

    Private Sub lbMatches_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbMatches.SelectedIndexChanged
        Dim match_item As sMatchItem
        Dim item As cToDoItem.sItemInfo
        Dim currnode As TreeNode
        Dim parentnode As TreeNode = Nothing
        Dim displayNode As TreeNode
        Dim path As New List(Of Integer)
        Dim intThisNodeNbr As Integer
        Dim intNdx, intLastNdx As Integer

        If (lbMatches.SelectedIndex <> -1) Then
            match_item = Matches(lbMatches.SelectedIndex)
            intThisNodeNbr = match_item.intNodeNbr
            tvWhereFound.Nodes.Clear()
            path.Add(intThisNodeNbr)
            Do While (True)
                currnode = TODO.FindNodeByNodeNbr(tv, intThisNodeNbr)
                item = CType(currnode.Tag, cToDoItem.sItemInfo)
                If (item.intParentNodeNbr = -1) Then
                    Exit Do
                End If
                intThisNodeNbr = item.intParentNodeNbr
                path.Add(intThisNodeNbr)
            Loop
            intLastNdx = path.Count - 1
            For intNdx = intLastNdx To 0 Step -1
                currnode = TODO.FindNodeByNodeNbr(tv, path(intNdx))
                item = CType(currnode.Tag, cToDoItem.sItemInfo)
                displayNode = New TreeNode
                displayNode.Text = item.strText
                If (IsNothing(parentnode)) Then
                    tvWhereFound.Nodes.Add(displayNode)
                    parentnode = displayNode
                Else
                    parentnode.Nodes.Add(displayNode)
                    parentnode = displayNode
                End If
            Next
            tvWhereFound.ExpandAll()
        End If
    End Sub

    Private Sub btnGoto_Click(sender As Object, e As EventArgs) Handles btnGoto.Click
        If (lbMatches.SelectedIndex <> -1) Then
            gintGoToNodeNbr = Matches(lbMatches.SelectedIndex).intNodeNbr
            Me.Close()
        End If
    End Sub

    Private Sub btnFindItemsWithDates_Click(sender As Object, e As EventArgs) Handles btnFindItemsWithDates.Click
        Dim intNdx, intLastNdx As Integer
        lbMatches.Items.Clear()
        Matches.Clear()
        FindNodesWithDates(tv.Nodes(0))
        intLastNdx = Matches.Count - 1
        If (intLastNdx >= 0) Then
            ' sort the list based on number of word matches first then alphabetically
            Matches.Sort(New MatchDateCompare)
            '
            For intNdx = 0 To intLastNdx
                lbMatches.Items.Add(Matches(intNdx).strItemText)
            Next
        End If
    End Sub

    Private Sub btnFindItemsWithTags_Click(sender As Object, e As EventArgs) Handles btnFindItemsWithTags.Click
        Dim intNdx, intLastNdx As Integer
        Dim strTagList As String = ""

        lbMatches.Items.Clear()
        Matches.Clear()
        intLastNdx = clbTags.Items.Count - 1
        For intNdx = 0 To intLastNdx
            If (clbTags.GetItemChecked(intNdx)) Then
                If (strTagList <> "") Then
                    strTagList &= ","
                End If
                strTagList &= clbTags.Items(intNdx).ToString
            End If
        Next
        FindNodesWithTags(tv.Nodes(0), strTagList)
        intLastNdx = Matches.Count - 1
        If (intLastNdx >= 0) Then
            ' sort the list based on number of word matches first then alphabetically
            Matches.Sort(New MatchTagCompare)
            '
            For intNdx = 0 To intLastNdx
                lbMatches.Items.Add(Matches(intNdx).strItemText)
            Next
        End If
    End Sub
End Class