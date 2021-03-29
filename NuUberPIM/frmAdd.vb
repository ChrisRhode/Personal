Option Explicit On
Option Strict On
Public Class frmAdd

    Public tv As TreeView
    Public gBoolCancel As Boolean = True
    Public item As cToDoItem.sItemInfo
    Dim gstrCurrentTarget As String
    Dim gboolInTrashTree As Boolean = False

    Private Sub frmAdd_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' May be unneeded, look at load behavior, triggered on Show or ShowDialog
        ' item is blank on entry so should be ok
        tbNewItem.Text = ""
    End Sub
    Private Sub tbNewItem_TextChanged(sender As Object, e As EventArgs) Handles tbNewItem.TextChanged
        gstrCurrentTarget = tbNewItem.Text.Trim.ToUpper
        tbMatches.Text = ""
        gboolInTrashTree = False
        FindStringInNodes(tv.Nodes(0))
    End Sub

    Sub FindStringInNodes(n As TreeNode)
        Dim item As cToDoItem.sItemInfo
        Dim intNdx As Integer
        Dim intLastNdx As Integer

        item = CType(n.Tag, cToDoItem.sItemInfo)
        If (item.intNodeNbr = 3) Then
            gboolInTrashTree = True
        End If
        If (item.strText.ToUpper.IndexOf(gstrCurrentTarget) <> -1) Then
            If (gboolInTrashTree) Then
                tbMatches.Text &= "(DELETED) "
            End If
            tbMatches.Text &= item.strText & vbCrLf
        End If
        intLastNdx = n.Nodes.Count - 1
        If (intLastNdx >= 0) Then
            For intNdx = 0 To intLastNdx
                FindStringInNodes(n.Nodes(intNdx))
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
        fEdit.ShowDialog()
        If (Not fEdit.gboolDidCancel) Then
            item = fEdit.sNew
            ' *** may or may not trigger text changed
            tbNewItem.Text = item.strText
        End If
    End Sub
End Class