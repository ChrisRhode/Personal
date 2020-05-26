Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports ACritterLoader.My
Imports Microsoft.VisualBasic.CompilerServices

Public Class Form1

    Enum eMessageType
        isInProgress
        isError
        isSuccess
    End Enum
    Sub DisplayMessage(strMessage As String, msgType As eMessageType)
        lblMessage.Text = strMessage
        Select Case msgType
            Case eMessageType.isInProgress
                lblMessage.ForeColor = Color.Orange
            Case eMessageType.isError
                lblMessage.ForeColor = Color.Red
            Case eMessageType.isSuccess
                lblMessage.ForeColor = Color.Green
        End Select
        Application.DoEvents()
    End Sub

    Sub WriteLog(strMessage As String)
        tbLog.Text &= strMessage & vbCrLf
        Application.DoEvents()
    End Sub

    Function TimeRangesToHours(strInput As String, ByRef strReformatted As String) As Boolean()
        Dim boolInHour(24) As Boolean
        Dim strRanges() As String
        Dim strHours() As String
        Dim intNdx As Integer
        Dim intNdx2 As Integer
        Dim intLastNdx As Integer
        Dim intStartHour As Integer
        Dim intEndHour As Integer


        If (strInput.Trim.ToUpper = "ALL DAY") Then
            For intNdx = 1 To 24
                boolInHour(intNdx) = True
            Next
            strReformatted = "All Day"
            Return boolInHour
        End If

        strReformatted = ""

        For intNdx = 1 To 24
            boolInHour(intNdx) = False
        Next

        strRanges = Split(strInput.Trim, "  ")
        intLastNdx = UBound(strRanges)
        For intNdx = 0 To intLastNdx
            If (intNdx > 0) Then
                strReformatted &= " , "
            End If
            strHours = Split(strRanges(intNdx).Trim, "-")
            intStartHour = StringHourToIntHour(strHours(0))
            strReformatted &= Format(intStartHour, "00") & ":00"
            intEndHour = StringHourToIntHour(strHours(1))
            strReformatted &= " - " & Format(intEndHour, "00") & ":00"
            If (intStartHour > intEndHour) Then
                'e.g. 23:00 - 8:00
                For intNdx2 = intStartHour To 24
                    boolInHour(intNdx2) = True
                Next
                For intNdx2 = 1 To intEndHour - 1
                    boolInHour(intNdx2) = True
                Next
            Else
                For intNdx2 = intStartHour To intEndHour - 1
                    boolInHour(intNdx2) = True
                Next
            End If
        Next

        Return boolInHour
    End Function

    Function StringHourToIntHour(strHour As String) As Integer
        Dim intTemp As Integer
        Dim intNdx As Integer

        intNdx = strHour.IndexOf(":00")
        If (intNdx = -1) Then
            Throw New Exception("Did not find :00 in " & strHour)
        End If

        strHour = strHour.Substring(0, intNdx)

        If (Not IsNumeric(strHour)) Then
            Throw New Exception("Non-numeric hour in a range")
        End If
        intTemp = Convert.ToInt32(strHour)
        If (intTemp < 1) Or (intTemp > 23) Then
            Throw New Exception("Invalid hour value in a range (not 1-23)")
        End If
        Return intTemp
    End Function
    Private Sub btnRawLoad_Click(sender As Object, e As EventArgs) Handles btnRawLoad.Click
        Dim strFN As String = "C:\Users\ratcl\Documents\DBData\ACNH_icebear.csv"

        Dim rdr As New System.IO.StreamReader(strFN)
        Dim strBuffer As String
        Dim strElements() As String
        Dim intNumElems As Integer = -1
        Dim intThisNumElems As Integer
        Dim intRowCtr As Integer = 0

        Dim strThingName As String
        Dim intBells As Integer
        Dim strLocation As String
        Dim strSize As String
        Dim strTimeOfDayRange As String
        Dim strTemp As String
        Dim intNdx As Integer
        Dim intNewCritterID As Integer

        Dim DBcon As New SqlClient.SqlConnection
        Dim DBcmd As New SqlClient.SqlCommand
        Dim CS As New cConnections
        Dim boolDBError As Boolean = False
        Dim boolPresentAtHours() As Boolean
        Dim strReformattedTimeRange As String

        tbLog.Text = ""

        DisplayMessage("Processing file...", eMessageType.isInProgress)

        Try
            Do While (rdr.Peek <> -1)
                strBuffer = rdr.ReadLine
                intRowCtr += 1
                If ((intRowCtr Mod 25) = 0) Then
                    WriteLog("Processing row " & intRowCtr & "...")
                End If
                strElements = Split(strBuffer, ",")
                intThisNumElems = UBound(strElements) + 1
                If (intNumElems = -1) Then
                    intNumElems = intThisNumElems
                ElseIf (intThisNumElems <> intNumElems) Then
                    DisplayMessage("Inconsistent number of columns at row " & intRowCtr, eMessageType.isError)
                    WriteLog("Offending record: " & strBuffer)
                    WriteLog("Expected " & intNumElems & " columns, saw " & intThisNumElems & " columns")
                    Throw New Exception("Error, see log")
                Else
                    strThingName = strElements(2)
                    strTemp = strElements(3).Replace(" ", "")
                    If (Not IsNumeric(strTemp)) Then
                        DisplayMessage("Bad item cost value at row " & intRowCtr, eMessageType.isError)
                        WriteLog("Offending value: " & strElements(3))
                        Throw New Exception("Error, see log")
                    End If

                    intBells = Convert.ToInt32(strTemp)
                    strLocation = strElements(4)
                    strSize = strElements(5)
                    strTimeOfDayRange = strElements(6)

                    boolPresentAtHours = TimeRangesToHours(strTimeOfDayRange, strReformattedTimeRange)
                    '
                    Try
                        DBcon.ConnectionString = CS.MainConnection
                        DBcon.Open()
                        DBcmd.Connection = DBcon
                        DBcmd.CommandType = CommandType.StoredProcedure
                        DBcmd.CommandText = "[ACCritters].[AddCritter]"
                        DBcmd.Parameters.Clear()
                        DBcmd.Parameters.Add(New SqlClient.SqlParameter("@CritterName", strThingName))
                        DBcmd.Parameters.Add(New SqlClient.SqlParameter("@BellsValue", intBells))
                        DBcmd.Parameters.Add(New SqlClient.SqlParameter("@WhereFound", strLocation))
                        DBcmd.Parameters.Add(New SqlClient.SqlParameter("@ShadowSize", strSize))
                        DBcmd.Parameters.Add(New SqlClient.SqlParameter("@TimeOfDayRange", strReformattedTimeRange))
                        Dim intRetParam As New SqlClient.SqlParameter("RETURN_VALUE", SqlDbType.Int)
                        intRetParam.Direction = ParameterDirection.ReturnValue
                        DBcmd.Parameters.Add(intRetParam)
                        DBcmd.ExecuteNonQuery()
                        intNewCritterID = DBcmd.Parameters("RETURN_VALUE").Value
                    Catch ex As Exception
                        DisplayMessage("DB error", eMessageType.isError)
                        WriteLog("Add error: " & ex.Message)
                        boolDBError = True
                    Finally
                        If (DBcon.State <> ConnectionState.Closed) Then DBcon.Close()
                    End Try

                    If (boolDBError) Then
                        Throw New Exception("Error, see log")
                    End If

                    For intNdx = 7 To 18
                        If (strElements(intNdx) = "Yes") Then
                            Try
                                DBcon.ConnectionString = CS.MainConnection
                                DBcon.Open()
                                DBcmd.Connection = DBcon
                                DBcmd.CommandType = CommandType.StoredProcedure
                                DBcmd.CommandText = "[ACCritters].[AddCritterMonth]"
                                DBcmd.Parameters.Clear()
                                DBcmd.Parameters.Add(New SqlClient.SqlParameter("@CritterID", intNewCritterID))
                                DBcmd.Parameters.Add(New SqlClient.SqlParameter("@MonthNum", (intNdx - 6)))
                                DBcmd.ExecuteNonQuery()
                            Catch ex As Exception
                                DisplayMessage("DB error", eMessageType.isError)
                                WriteLog("Add Month error: " & ex.Message)
                                boolDBError = True
                            Finally
                                If (DBcon.State <> ConnectionState.Closed) Then DBcon.Close()
                            End Try

                            If (boolDBError) Then
                                Throw New Exception("Error, see log")
                            End If
                        ElseIf (strElements(intNdx) = "No") Then
                            '
                        Else
                            DisplayMessage("Bad month value at row " & intRowCtr, eMessageType.isError)
                            WriteLog("Offending value: " & strElements(intNdx))
                            Throw New Exception("Error, see log")
                        End If

                    Next
                    '


                    Try
                        DBcon.ConnectionString = CS.MainConnection
                        DBcon.Open()
                        DBcmd.Connection = DBcon
                        DBcmd.CommandType = CommandType.StoredProcedure
                        DBcmd.CommandText = "[ACCritters].[AddCritterHour]"
                        For intNdx = 1 To 24
                            If (boolPresentAtHours(intNdx)) Then
                                DBcmd.Parameters.Clear()
                                DBcmd.Parameters.Add(New SqlClient.SqlParameter("@CritterID", intNewCritterID))
                                DBcmd.Parameters.Add(New SqlClient.SqlParameter("@Hour", intNdx))
                                DBcmd.ExecuteNonQuery()
                            End If
                        Next
                    Catch ex As Exception
                        DisplayMessage("DB error", eMessageType.isError)
                        WriteLog("Add Hour error: " & ex.Message)
                        boolDBError = True
                    Finally
                        If (DBcon.State <> ConnectionState.Closed) Then DBcon.Close()
                    End Try

                    If (boolDBError) Then
                        Throw New Exception("Error, see log")
                    End If
                    '
                End If
                '
            Loop
        Catch ex As Exception
            WriteLog("Unexpected error during processing:" & ex.Message)
            WriteLog("Record being processed: " & intRowCtr)
        Finally
            rdr.Close()
        End Try

        DisplayMessage("Processsed " & intRowCtr & " rows", eMessageType.isSuccess)

    End Sub
End Class
