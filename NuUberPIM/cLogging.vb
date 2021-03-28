Public Class cLogging
    Dim mtbLog As TextBox
    Dim intCtr As Integer = 0
    Dim boolShowDebugs As Boolean = False
    Sub New(tbLog As TextBox)
        mtbLog = tbLog
        mtbLog.Text = ""
    End Sub
    Sub WriteToLog(strMessage As String, Optional isDebug As Boolean = False)
        If (isDebug And Not boolShowDebugs) Then
            Exit Sub
        End If

        If (isDebug) Then
            intCtr += 1
            mtbLog.Text &= intCtr & ": "
        End If
        mtbLog.Text &= strMessage & vbCrLf
        mtbLog.SelectionStart = mtbLog.Text.Length - 1
        mtbLog.ScrollToCaret()
        'Application.DoEvents()
    End Sub
End Class
