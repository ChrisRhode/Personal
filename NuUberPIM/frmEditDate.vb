Public Class frmEditDate

    Public gdt As Date
    Public gboolCancel As Boolean = False

    Dim UGBL As New cUtility

    Sub UpdateDisplay()
        tbDate.Text = UGBL.FormatDateAsText(gdt)
    End Sub

    Function ContentValidate() As Boolean
        Dim dtTemp As Date

        If (Not UGBL.TextDateToDate(tbDate.Text, dtTemp)) Then
            tbDate.BackColor = Color.Red
            Return False
        Else
            gdt = dtTemp
            tbDate.Text = UGBL.FormatDateTimeAsText(gdt)
            tbDate.BackColor = Color.White
            Return True
        End If
    End Function
    Private Sub btnToday_Click(sender As Object, e As EventArgs) Handles btnToday.Click
        gdt = Date.Now.Date
        UpdateDisplay()
    End Sub

    Private Sub btnTomorrow_Click(sender As Object, e As EventArgs) Handles btnTomorrow.Click
        gdt = Date.Now.Date.AddDays(1)
        UpdateDisplay()
    End Sub

    Private Sub btnNextMonday_Click(sender As Object, e As EventArgs) Handles btnNextMonday.Click
        Dim dt As Date = Date.Now.Date
        While (True)
            dt = dt.AddDays(1)
            If (dt.DayOfWeek = DayOfWeek.Monday) Then
                Exit While
            End If
        End While
        gdt = dt
        UpdateDisplay()
    End Sub

    Private Sub btnPlusOne_Click(sender As Object, e As EventArgs) Handles btnPlusOne.Click
        gdt = gdt.AddDays(1)
        UpdateDisplay()
    End Sub

    Private Sub btnMinusOne_Click(sender As Object, e As EventArgs) Handles btnMinusOne.Click
        gdt = gdt.AddDays(-1)
        UpdateDisplay()
    End Sub

    Private Sub btnPlus7_Click(sender As Object, e As EventArgs) Handles btnPlus7.Click
        gdt = gdt.AddDays(7)
        UpdateDisplay()
    End Sub

    Private Sub btnMinus7_Click(sender As Object, e As EventArgs) Handles btnMinus7.Click
        gdt = gdt.AddDays(-7)
        UpdateDisplay()
    End Sub

    Private Sub btnValidate_Click(sender As Object, e As EventArgs) Handles btnValidate.Click
        ContentValidate()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If (Not ContentValidate()) Then
            Exit Sub
        End If
        gboolCancel = False
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        gboolCancel = True
        Me.Close()
    End Sub

    Private Sub frmEditDate_Load(sender As Object, e As EventArgs) Handles Me.Load
        UpdateDisplay()
    End Sub
End Class