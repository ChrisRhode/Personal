Option Explicit On
Option Strict On
Public Class frmEdit
    Public sOriginal As New cToDoItem.sItemInfo
    Public sNew As New cToDoItem.sItemInfo
    Public mCurrentMasterTagList As String
    Public gboolDidCancel As Boolean = False
    Dim UGBL As New cUtility

    Private Sub frmEdit_Load(sender As Object, e As EventArgs) Handles Me.Load
        sNew = sOriginal
        DisplayNew()
    End Sub
    Private Sub btnRevert_Click(sender As Object, e As EventArgs) Handles btnRevert.Click
        sNew = sOriginal
        DisplayNew()
        ContentValidate()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        gboolDidCancel = True
        Me.Close()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If (Not LoadFromDisplay()) Then
            Exit Sub
        End If
        gboolDidCancel = False
        Me.Close()
    End Sub

    Sub DisplayNew()

        tbText.Text = sNew.strText
        tbNotes.Text = sNew.strNotes
        tbPri.Text = sNew.intPriority.ToString
        tbDOE.Text = UGBL.FormatDateAsText(sNew.dtDateOfEvent)
        tbBTTD.Text = UGBL.FormatDateAsText(sNew.dtDateOfBumpToTop)
        tbCreated.Text = UGBL.FormatDateTimeAsText(sNew.dtCreated)
        tbModified.Text = UGBL.FormatDateTimeAsText(sNew.dtModified)
        '
        UpdateAssignedTagList()
    End Sub

    Sub UpdateAssignedTagList()
        Dim strTags() As String
        Dim strTagList As String
        Dim intNdx, intLastNdx As Integer
        clbTags.Items.Clear()
        If (mCurrentMasterTagList <> "") Then
            strTags = Split(mCurrentMasterTagList, ",")
            intLastNdx = UBound(strTags)
            For intNdx = 0 To intLastNdx
                clbTags.Items.Add(strTags(intNdx))
            Next
        End If
        If (sNew.strTags <> "") Then
            strTagList = "," & sNew.strTags & ","
            intLastNdx = clbTags.Items.Count - 1
            For intNdx = 0 To intLastNdx
                'clbTags.Items.Add(strTags(intNdx))
                ' find the tag in the checked box list that matches this tag, and set the check mark
                If (strTagList.IndexOf("," & clbTags.Items(intNdx).ToString & ",") <> -1) Then
                    clbTags.SetItemChecked(intNdx, True)
                End If
            Next
        End If
    End Sub
    ' *** all intracacies of load/save
    Function LoadFromDisplay() As Boolean
        Dim strTags As String = ""
        Dim intNdx, intLastNdx As Integer
        If (Not ContentValidate()) Then
            Return False
        End If
        sNew.strText = tbText.Text.Trim
        sNew.strNotes = tbNotes.Text.Trim
        sNew.intPriority = Convert.ToInt32(tbPri.Text)
        UGBL.TextDateToDate(tbDOE.Text, sNew.dtDateOfEvent)
        UGBL.TextDateToDate(tbBTTD.Text, sNew.dtDateOfBumpToTop)
        '
        intLastNdx = clbTags.Items.Count - 1
        For intNdx = 0 To intLastNdx
            If (clbTags.GetItemChecked(intNdx)) Then
                If (strTags <> "") Then
                    strTags &= ","
                End If
                strTags &= clbTags.Items(intNdx).ToString
            End If
        Next
        sNew.strTags = strTags
        Return True
    End Function

    Function ContentValidate() As Boolean
        Dim d As Date
        Dim intTemp As Integer

        Dim boolResult As Boolean = True
        Dim boolPriResult As Boolean = True

        If (UGBL.TextDateToDate(tbDOE.Text, d)) Then
            tbDOE.Text = UGBL.FormatDateAsText(d)
            tbDOE.BackColor = Color.White
        Else
            tbDOE.BackColor = Color.Red
            boolResult = False
        End If

        If (UGBL.TextDateToDate(tbBTTD.Text, d)) Then
            tbBTTD.Text = UGBL.FormatDateAsText(d)
            tbBTTD.BackColor = Color.White
        Else
            tbBTTD.BackColor = Color.Red
            boolResult = False
        End If

        tbPri.Text = tbPri.Text.Trim

        If (IsNumeric(tbPri.Text)) Then
            intTemp = Convert.ToInt32(tbPri.Text)
            If (intTemp < 0) Then
                boolPriResult = False
            End If
        Else
            boolPriResult = False
        End If
        If (boolPriResult) Then
            tbPri.BackColor = Color.White
        Else
            boolResult = False
            tbPri.BackColor = Color.Red
        End If

        Return boolResult
    End Function

    Private Sub btnPriPlus_Click(sender As Object, e As EventArgs) Handles btnPriPlus.Click
        If (Not ContentValidate()) Then
            Exit Sub
        End If
        sNew.intPriority += 1
        DisplayNew()
    End Sub

    Private Sub btnPriMinus_Click(sender As Object, e As EventArgs) Handles btnPriMinus.Click
        If (Not ContentValidate()) Then
            Exit Sub
        End If
        sNew.intPriority -= 1
        DisplayNew()
    End Sub

    Private Sub btnValidate_Click(sender As Object, e As EventArgs) Handles btnValidate.Click
        ContentValidate()
    End Sub

    Private Sub btnEditDOE_Click(sender As Object, e As EventArgs) Handles btnEditDOE.Click
        Dim fEditDate As New frmEditDate
        Dim dt As Date

        If (Not UGBL.TextDateToDate(tbDOE.Text, dt)) Then
            MessageBox.Show("Date of Event value is not currently valid")
            Exit Sub
        End If
        fEditDate.gdt = dt
        fEditDate.ShowDialog()
        If (Not fEditDate.gboolCancel) Then
            tbDOE.Text = UGBL.FormatDateAsText(fEditDate.gdt)
        End If
    End Sub

    Private Sub btnEditBTTD_Click(sender As Object, e As EventArgs) Handles btnEditBTTD.Click
        Dim fEditDate As New frmEditDate
        Dim dt As Date

        If (Not UGBL.TextDateToDate(tbBTTD.Text, dt)) Then
            MessageBox.Show("Bump To Top Date value is not currently valid")
            Exit Sub
        End If
        fEditDate.gdt = dt
        fEditDate.ShowDialog()
        If (Not fEditDate.gboolCancel) Then
            tbBTTD.Text = UGBL.FormatDateAsText(fEditDate.gdt)
        End If
    End Sub

    Private Sub btnAddTag_Click(sender As Object, e As EventArgs) Handles btnAddTag.Click
        Dim strValidChars As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_-"
        Dim strNewTag As String
        Dim intNdx, intLastNdx As Integer

        strNewTag = tbNewTag.Text.Trim.ToUpper
        If (strNewTag = "") Then
            MessageBox.Show("You must enter a new tag")
            Exit Sub
        End If
        ' only allow valid chars A-Z, 0-9,_,-
        intLastNdx = strNewTag.Length - 1
        For intNdx = 0 To intLastNdx
            If (strValidChars.IndexOf(strNewTag.Substring(intNdx, 1)) = -1) Then
                MessageBox.Show("Tags can only contain A-Z, 0-9, dash and underscore")
                Exit Sub
            End If
        Next
        If (("," & mCurrentMasterTagList & ",").IndexOf("," & strNewTag & ",") <> -1) Then
            MessageBox.Show("Tag is already known")
        End If
        UGBL.MergeItemsIntoSortedCSL(mCurrentMasterTagList, strNewTag)
        UGBL.MergeItemsIntoSortedCSL(sNew.strTags, strNewTag)
        UpdateAssignedTagList()
    End Sub
End Class