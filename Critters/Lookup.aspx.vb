Imports System.Configuration.Internal
Imports System.EnterpriseServices
Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Security.Policy

Public Class Lookup
    Inherits System.Web.UI.Page

    Dim gds As DataSet
    ' *** need outer exception handler or global exception handler
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strVersion As String
        Dim strVersionPieces() As String
        If (Not IsPostBack) Then
            strVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
            strVersionPieces = Split(strVersion, ".")
            lblVersion.Text = "v" & strVersionPieces(0) & "." & strVersionPieces(1)
            rblCritterType.SelectedIndex = 2
            rblAvailability.SelectedIndex = 1
            rblSellPrice.SelectedIndex = 0
            'v1.4 change to default of no sort terms instead of Name by default
            tbSortTerms.Text = ""
        End If
        If (rblSellPrice.SelectedValue <> "Manual") Then
            tbLow.Enabled = False
            tbHigh.Enabled = False
        Else
            tbLow.Enabled = True
            tbHigh.Enabled = True
        End If
        tblResults.Rows.Clear()
        lblMessage.Text = ""
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        DisplayData()
    End Sub

    Sub DisplayData()
        Dim DBcon As New SqlClient.SqlConnection
        Dim DBcmd As New SqlClient.SqlCommand
        Dim DBAdapter As SqlClient.SqlDataAdapter

        Dim CS As New cConnections

        Dim tr As TableRow
        Dim tc As TableCell
        Dim intCritterType As Integer
        Dim intNdx As Integer
        Dim intLastNdx As Integer
        Dim intLow As Integer
        Dim intHigh As Integer
        Dim strSQL As String
        Dim strTemp As String
        Dim strSortClause As String
        Dim strSortItems() As String
        Dim intDisplayedRecordCnt As Integer = 0

        Dim intMonthNumNow As Integer
        Dim intMonthNumNextMonth As Integer
        Dim intHourNow As Integer
        Dim strSearchTerm As String = ""
        Dim strCheck As String
        Dim boolIgnoreFilters As Boolean = False

        Dim strCritterTypeSelected As String
        Dim strAvailabilitySelected As String
        Dim strSellPriceSelected As String

        Dim dt As Date
        ' this will yield correct current PST/PDT even if in Daylight Saving
        Dim dtz = System.TimeZoneInfo.ConvertTime(Date.Now, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"))
        dt = dtz
        intMonthNumNow = dt.Month
        intMonthNumNextMonth = dt.AddMonths(1).Month
        intHourNow = dt.Hour

        strCritterTypeSelected = rblCritterType.SelectedValue
        strAvailabilitySelected = rblAvailability.SelectedValue
        strSellPriceSelected = rblSellPrice.SelectedValue

        tblResults.BorderStyle = BorderStyle.Double
        tblResults.BorderWidth = New Unit(2, UnitType.Pixel)
        tblResults.CellPadding = 5
        tblResults.CellSpacing = 5
        tblResults.GridLines = GridLines.Both

        tr = New TableRow
        tc = New TableCell
        tc.Text = "Critter" '.item(1)
        tc.Font.Bold = True
        tr.Cells.Add(tc)
        tc = Nothing
        tc = New TableCell
        tc.Text = "Type"    '2
        tc.Font.Bold = True
        tr.Cells.Add(tc)
        tc = Nothing
        tc = New TableCell
        tc.Text = "Sell"    '3
        tc.Font.Bold = True
        tr.Cells.Add(tc)
        tc = Nothing
        tc = New TableCell
        tc.Text = "Where"   '4
        tc.Font.Bold = True
        tr.Cells.Add(tc)
        tc = Nothing
        tc = New TableCell
        tc.Text = "Size"    '5
        tc.Font.Bold = True
        tr.Cells.Add(tc)
        tc = Nothing
        tc = New TableCell
        tc.Text = "Month(s)"    '7
        tc.Font.Bold = True
        tr.Cells.Add(tc)
        tc = Nothing
        tc = New TableCell
        tc.Text = "Hours"   '6
        tc.Font.Bold = True
        tr.Cells.Add(tc)
        tc = Nothing
        tblResults.Rows.Add(tr)
        tr = Nothing
        ' *** use an error message scheme instead of !!! display
        If (strSellPriceSelected = "Manual") Then
            strTemp = tbLow.Text.Trim
            If (Not IsNumeric(strTemp)) Then
                tbLow.Text = "!!!"
                Exit Sub
            End If
            intLow = Convert.ToInt32(strTemp)
            If (intLow < 0) Or (intLow > 30000) Then
                tbLow.Text = "!!!"
                Exit Sub
            End If
            strTemp = tbHigh.Text.Trim
            If (Not IsNumeric(strTemp)) Then
                tbHigh.Text = "!!!"
                Exit Sub
            End If
            intHigh = Convert.ToInt32(strTemp)
            If (intHigh < 0) Or (intHigh > 30000) Then
                tbHigh.Text = "!!!"
                Exit Sub
            End If
            If (intLow > intHigh) Then
                tbLow.Text = "!!!"
                tbHigh.Text = "!!!"
            End If
        End If

        strSearchTerm = tbWordSearch.Text.Replace("  ", " ").Trim
        If (strSearchTerm.Length > 0) Then
            intLastNdx = strSearchTerm.Length - 1
            For intNdx = 0 To intLastNdx
                strCheck = strSearchTerm.Substring(intNdx, 1).ToUpper
                If ((Asc(strCheck) < 65) Or (Asc(strCheck) > 90)) And (strCheck <> " ") Then
                    tbWordSearch.Text = "!!!"
                    Exit Sub
                End If
            Next
        End If
        If (strSearchTerm <> "") And (cbIgnoreFilters.Checked) Then
            boolIgnoreFilters = True
        End If

        gds = New DataSet
        ' v1.3 cleaned up query to remove old fields, add joined month numbers
        strSQL = "SELECT A.IDNum,A.CritterName,A.CritterType,A.BellsValue,A.WhereFound,A.ShadowSize,A.TimeOfDayRange,"
        strSQL &= "STUFF((SELECT ',' + CONVERT(varchar,MonthNum) FROM [ACCritters].[MonthsAvailable] WHERE (IDNum = A.IDNum) FOR XML PATH('')),1,1,'') AS ValidMonths "
        strSQL &= "FROM [ACCritters].[Critters] A "
        ' this is a cheat so any/all subsequent WHERE clause modifications can start with AND
        strSQL &= " WHERE (A.IDNUM IS NOT NULL) "
        '
        If (Not boolIgnoreFilters) Then
            If (strAvailabilitySelected = "Current Month") Or (strAvailabilitySelected = "Now") Or (strAvailabilitySelected = "Evening + Early Morning") Then
                strSQL &= "AND (A.IDNum IN (SELECT DISTINCT IDNum FROM [ACCritters].[MonthsAvailable] WHERE (MonthNum = " & intMonthNumNow & "))) "
            End If
            If (strAvailabilitySelected = "This Month or Next Month") Then
                strSQL &= "AND (A.IDNum IN (SELECT DISTINCT IDNum FROM [ACCritters].[MonthsAvailable] WHERE (MonthNum IN (" & intMonthNumNow & "," & intMonthNumNextMonth & ")))) "
            End If
            intCritterType = 0
            If (strCritterTypeSelected = "Bugs") Then
                intCritterType = 1
            End If
            If (strCritterTypeSelected = "Fish") Then
                intCritterType = 2
            End If
            If (intCritterType <> 0) Then
                strSQL &= "And (A.CritterType = " & intCritterType & ") "
            End If
            ' v1.4 fixed to use correct variable for current hour ... oops!!!
            If (strAvailabilitySelected = "Now") Then
                strSQL &= "AND (A.IDNum IN (SELECT DISTINCT IDNum FROM [ACCritters].[HoursAvailable] WHERE ([Hour] = " & intHourNow & "))) "
            End If
            ' *** issue at month boundaries
            If (strAvailabilitySelected = "Evening + Early Morning") Then
                strSQL &= "AND (A.IDNum IN (SELECT DISTINCT IDNum FROM [ACCritters].[HoursAvailable] WHERE ([Hour] IN (0,1,2,3,4,19,20,21,22,23,24)))) "
            End If
            If (strSellPriceSelected = "GE2500") Then
                strSQL &= "AND (A.BellsValue >= 2500) "
            End If
            If (strSellPriceSelected = "GE500") Then
                strSQL &= "AND (A.BellsValue >= 500) "
            End If
            If (strSellPriceSelected = "LT250") Then
                strSQL &= "AND (A.BellsValue < 250) "
            End If
            If (strSellPriceSelected = "Midrange") Then
                strSQL &= "AND ((A.BellsValue >= 250) AND (A.BellsValue < 2500)) "
            End If
            If (strSellPriceSelected = "Manual") Then
                strSQL &= "AND ((A.BellsValue >= " & intLow & ") AND (A.BellsValue <= " & intHigh & ")) "
            End If
        End If
        If (strSearchTerm <> "") Then
            strSQL &= " AND (A.CritterName LIKE '%" & strSearchTerm & "%') "
        End If

        strSQL &= "ORDER BY "
        strTemp = tbSortTerms.Text.Trim
        If (strTemp <> "") Then
            strSortItems = Split(tbSortTerms.Text, ",")
            intLastNdx = UBound(strSortItems)
            strSortClause = ""
            For intNdx = 0 To intLastNdx
                strTemp = strSortItems(intNdx)
                If (strTemp = "Name") Then
                    If (strSortClause <> "") Then
                        strSortClause &= ","
                    End If
                    strSortClause &= "A.CritterName"
                ElseIf (strTemp = "Type") Then
                    If (strSortClause <> "") Then
                        strSortClause &= ","
                    End If
                    strSortClause &= "A.CritterType"
                ElseIf (strTemp = "Sell") Then
                    If (strSortClause <> "") Then
                        strSortClause &= ","
                    End If
                    strSortClause &= "A.BellsValue DESC"
                ElseIf (strTemp = "Location") Then
                    If (strSortClause <> "") Then
                        strSortClause &= ","
                    End If
                    strSortClause &= "A.WhereFound"
                ElseIf (strTemp = "Size") Then
                    If (strSortClause <> "") Then
                        strSortClause &= ","
                    End If
                    strSortClause &= "A.ShadowSize"
                ElseIf (strTemp = "When") Then
                    If (strSortClause <> "") Then
                        strSortClause &= ","
                    End If
                    strSortClause &= "A.TimeOfDayRange"
                Else
                    Throw New Exception("Bad sort term")
                End If
            Next
            strSQL &= strSortClause
        Else
            strSQL &= "A.IDNum"
        End If

        Try
            DBcon.ConnectionString = CS.MainConnection
            DBcon.Open()
            DBcmd.Connection = DBcon
            DBcmd.CommandType = CommandType.Text
            DBcmd.CommandText = strSQL
            DBcmd.Parameters.Clear()
            DBAdapter = New SqlClient.SqlDataAdapter(DBcmd)
            DBAdapter.TableMappings.Clear()
            DBAdapter.TableMappings.Add("Table", "Results")
            DBAdapter.Fill(gds)
        Catch ex As Exception
            Throw New Exception("Data Load Failed", ex)
        Finally
            If (DBcon.State <> ConnectionState.Closed) Then DBcon.Close()
        End Try

        intLastNdx = gds.Tables("Results").Rows.Count - 1
        For intNdx = 0 To intLastNdx
            With gds.Tables("Results").Rows(intNdx)
                tr = New TableRow
                tc = New TableCell
                tc.Text = .Item(1)
                tr.Cells.Add(tc)
                tc = Nothing
                tc = New TableCell
                If (.Item(2) = 1) Then
                    tc.Text = "Bug"
                Else
                    tc.Text = "Fish"
                End If
                tr.Cells.Add(tc)
                tc = Nothing
                tc = New TableCell
                tc.Text = .Item(3).ToString
                tr.Cells.Add(tc)
                tc = Nothing
                tc = New TableCell
                tc.Text = .Item(4)
                tr.Cells.Add(tc)
                tc = Nothing
                tc = New TableCell
                tc.Text = .Item(5)
                tr.Cells.Add(tc)
                tc = Nothing
                tc = New TableCell
                tc.Text = .Item(7)
                tr.Cells.Add(tc)
                tc = Nothing
                tc = New TableCell
                tc.Text = .Item(6)
                tr.Cells.Add(tc)
                tc = Nothing
                tblResults.Rows.Add(tr)
                tr = Nothing
                intDisplayedRecordCnt += 1
            End With
        Next

        lblMessage.Text = "Total matches: " & intDisplayedRecordCnt
        If (strAvailabilitySelected = "Now") Then
            lblMessage.Text &= " Used Hour: " & dtz.Hour
        End If

    End Sub

    Protected Sub btnSortClear_Click(sender As Object, e As EventArgs) Handles btnSortClear.Click
        tbSortTerms.Text = ""
    End Sub

    Protected Sub btnSortName_Click(sender As Object, e As EventArgs) Handles btnSortName.Click
        If (tbSortTerms.Text <> "") Then
            If (tbSortTerms.Text.IndexOf("Name") <> -1) Then
                Exit Sub
            End If
            tbSortTerms.Text &= ","
        End If
        tbSortTerms.Text &= "Name"
    End Sub

    Protected Sub btnSortType_Click(sender As Object, e As EventArgs) Handles btnSortType.Click
        If (tbSortTerms.Text <> "") Then
            If (tbSortTerms.Text.IndexOf("Type") <> -1) Then
                Exit Sub
            End If
            tbSortTerms.Text &= ","
        End If
        tbSortTerms.Text &= "Type"
    End Sub
    Protected Sub btnSortPrice_Click(sender As Object, e As EventArgs) Handles btnSortPrice.Click
        If (tbSortTerms.Text <> "") Then
            If (tbSortTerms.Text.IndexOf("Sell") <> -1) Then
                Exit Sub
            End If
            tbSortTerms.Text &= ","
        End If
        tbSortTerms.Text &= "Sell"
    End Sub

    Protected Sub btnSortLocation_Click(sender As Object, e As EventArgs) Handles btnSortLocation.Click
        If (tbSortTerms.Text <> "") Then
            If (tbSortTerms.Text.IndexOf("Location") <> -1) Then
                Exit Sub
            End If
            tbSortTerms.Text &= ","
        End If
        tbSortTerms.Text &= "Location"
    End Sub

    Protected Sub btnSortSize_Click(sender As Object, e As EventArgs) Handles btnSortSize.Click
        If (tbSortTerms.Text <> "") Then
            If (tbSortTerms.Text.IndexOf("Size") <> -1) Then
                Exit Sub
            End If
            tbSortTerms.Text &= ","
        End If
        tbSortTerms.Text &= "Size"
    End Sub

    Protected Sub btnSortTime_Click(sender As Object, e As EventArgs) Handles btnSortTime.Click
        If (tbSortTerms.Text <> "") Then
            If (tbSortTerms.Text.IndexOf("When") <> -1) Then
                Exit Sub
            End If
            tbSortTerms.Text &= ","
        End If
        tbSortTerms.Text &= "When"
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Response.Redirect("About.aspx", True)
    End Sub


End Class