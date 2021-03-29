Option Explicit On
Option Strict On
Public Class frmEdit
    Public sOriginal As New cToDoItem.sItemInfo
    Public sNew As New cToDoItem.sItemInfo
    Public gboolDidCancel As Boolean = False
    Dim U As New cUtility

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
        tbDOE.Text = U.FormatDateAsText(sNew.dtDateOfEvent)
        tbBTTD.Text = U.FormatDateAsText(sNew.dtDateOfBumpToTop)
        tbCreated.Text = U.FormatDateTimeAsText(sNew.dtCreated)
        tbModified.Text = U.FormatDateTimeAsText(sNew.dtModified)
    End Sub
    ' *** all intracacies of load/save
    Function LoadFromDisplay() As Boolean
        If (Not ContentValidate()) Then
            Return False
        End If
        sNew.strText = tbText.Text.Trim
        sNew.strNotes = tbNotes.Text.Trim
        sNew.intPriority = Convert.ToInt32(tbPri.Text)
        U.TextDateToDate(tbDOE.Text, sNew.dtDateOfEvent)
        U.TextDateToDate(tbBTTD.Text, sNew.dtDateOfBumpToTop)
        Return True
    End Function

    Function ContentValidate() As Boolean
        Dim d As Date
        Dim intTemp As Integer

        Dim boolResult As Boolean = True
        Dim boolPriResult As Boolean = True

        If (U.TextDateToDate(tbDOE.Text, d)) Then
            tbDOE.Text = U.FormatDateAsText(d)
            tbDOE.BackColor = Color.White
        Else
            tbDOE.BackColor = Color.Red
            boolResult = False
        End If

        If (U.TextDateToDate(tbBTTD.Text, d)) Then
            tbBTTD.Text = U.FormatDateAsText(d)
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

        If (Not U.TextDateToDate(tbDOE.Text, dt)) Then
            MessageBox.Show("Date of Event value is not currently valid")
            Exit Sub
        End If
        fEditDate.gdt = dt
        fEditDate.ShowDialog()
        If (Not fEditDate.gboolCancel) Then
            tbDOE.Text = U.FormatDateAsText(fEditDate.gdt)
        End If
    End Sub

    Private Sub btnEditBTTD_Click(sender As Object, e As EventArgs) Handles btnEditBTTD.Click
        Dim fEditDate As New frmEditDate
        Dim dt As Date

        If (Not U.TextDateToDate(tbBTTD.Text, dt)) Then
            MessageBox.Show("Bump To Top Date value is not currently valid")
            Exit Sub
        End If
        fEditDate.gdt = dt
        fEditDate.ShowDialog()
        If (Not fEditDate.gboolCancel) Then
            tbBTTD.Text = U.FormatDateAsText(fEditDate.gdt)
        End If
    End Sub
End Class