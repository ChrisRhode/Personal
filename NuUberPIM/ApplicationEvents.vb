﻿Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Dim fOutFile As IO.StreamWriter
            Dim strPath As String
            strPath = Form1.gstrCurrentErrorFile
            If (strPath <> "") Then
                fOutFile = System.IO.File.AppendText(strPath)
                fOutFile.WriteLine("Error at " & Format(Date.Now, "MM/dd/yyyy HH:mm:ss") & ": " & e.Exception.ToString)
                fOutFile.Close()
            Else
                MessageBox.Show("Error encountered before logging enabled: " & e.Exception.ToString)
            End If
            MessageBox.Show("An unexpected error occurred.  Repair will be necessary.  See the errors file for more details")
            End
        End Sub
    End Class
End Namespace
