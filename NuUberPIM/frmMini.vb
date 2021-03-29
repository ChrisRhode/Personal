Option Explicit On
Option Strict On
Public Class frmMini
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        'Quick Add
        Dim strQuickAddText As String

        strQuickAddText = InputBox("Enter Quick Add", "Quick Add", "")
        strQuickAddText = strQuickAddText.Trim
        If (strQuickAddText <> "") Then
            Form1.PerformAdd(strQuickAddText)
        End If
    End Sub

    Private Sub btnMain_Click(sender As Object, e As EventArgs) Handles btnMain.Click
        Form1.gboolMiniMode = False
        Me.Hide()
        Form1.Show()
    End Sub
End Class