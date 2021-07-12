Option Explicit On
Option Strict On
Public Class cUtility

    Dim mdtNull As Date = DateValue("01/01/1900")

    Public Function FormatDateAsText(d As Date) As String
        If (d = mdtNull) Then
            Return ""
        Else
            Return Format(d, "ddd MM/dd/yyyy")
        End If
    End Function

    Public Function TextDateToDate(strTextDate As String, ByRef d As Date) As Boolean
        Dim strTemp As String
        Dim strPieces() As String

        strTemp = strTextDate.Trim.Replace("  ", " ")
        If (strTemp = "") Then
            d = mdtNull
            Return True
        Else
            strPieces = Split(strTemp, " ")
            If (UBound(strPieces) = 1) Then
                strTemp = strPieces(1)
            End If
            If (IsDate(strTemp)) Then
                d = Convert.ToDateTime(strTemp)
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Function FormatDateTimeAsText(d As Date) As String
        If (d = mdtNull) Then
            Return ""
        Else
            Return Format(d, "ddd MM/dd/yyyy hh:mm tt")
        End If
    End Function

    Function ConvertToCompactDate(d As Date) As String
        If (d = mdtNull) Then
            Return ""
        Else
            Return Format(d, "MMddyyyy")
        End If
    End Function

    Function ConvertFromCompactDate(strInput As String) As Date
        If (strInput = "") Then
            Return mdtNull
        Else
            Return Convert.ToDateTime(strInput.Substring(0, 2) & "/" & strInput.Substring(2, 2) & "/" & strInput.Substring(4))
        End If
    End Function

    Function ConvertToCompactDateAndTime(d As Date) As String
        If (d = mdtNull) Then
            Return ""
        Else
            Return Format(d, "MMddyyyyHHmmss")
        End If
    End Function

    Function ConvertFromCompactDateAndTime(strInput As String) As Date
        Dim strTemp As String

        If (strInput = "") Then
            Return mdtNull
        Else
            strTemp = strInput.Substring(0, 2) & "/" & strInput.Substring(2, 2) & "/" & strInput.Substring(4, 4)
            strTemp &= " "
            strTemp &= strInput.Substring(8, 2) & ":" & strInput.Substring(10, 2) & ":" & strInput.Substring(12)
            Return Convert.ToDateTime(strTemp)
        End If
    End Function

    Function EncodeToAvoid(strOriginal As String, strAvoidChars As String) As String
        Dim strOutput As String = ""
        Dim strChar As String
        Dim intNdx, intLastNdx As Integer
        Dim strActualAvoidChars As String

        strActualAvoidChars = strAvoidChars & "%" & ChrW(10) & ChrW(13) & ChrW(9)

        intLastNdx = strOriginal.Length - 1
        If (intLastNdx = -1) Then
            Return ""
        End If
        For intNdx = 0 To intLastNdx
            strChar = strOriginal.Substring(intNdx, 1)
            If (strActualAvoidChars.IndexOf(strChar) <> -1) Then
                strOutput &= "%" & AscW(strChar) & "%"
            Else
                strOutput &= strChar
            End If
        Next
        Return strOutput
    End Function

    Function DecodeFromAvoid(strOriginal As String) As String
        Dim strOutput As String = ""
        Dim strChar As String
        Dim intNdx, intLastNdx As Integer
        Dim intNdx2 As Integer
        Dim intChar As Integer
        intNdx = 0
        intLastNdx = strOriginal.Length - 1
        While (intNdx <= intLastNdx)
            strChar = strOriginal.Substring(intNdx, 1)
            If (strChar = "%") Then
                intNdx += 1
                intNdx2 = strOriginal.IndexOf("%", intNdx) - 1
                intChar = Convert.ToInt32(strOriginal.Substring(intNdx, intNdx2 - intNdx + 1))
                strOutput &= ChrW(intChar)
                intNdx = intNdx2 + 1
            Else
                strOutput &= strChar
            End If
            intNdx += 1
        End While
        Return strOutput
    End Function

    Function BoolToYN(b As Boolean) As String
        If (b) Then
            Return "Y"
        Else
            Return "N"
        End If
    End Function

    Function YNToBool(strChar As String) As Boolean
        Select Case strChar
            Case "Y"
                Return True
            Case "N"
                Return False
            Case Else
                Throw New Exception("Bad YN value")
        End Select
    End Function

    Public Sub MergeItemsIntoSortedCSL(ByRef strCSL As String, strMergeList As String)
        Dim strMaster As String
        Dim strTagList() As String
        Dim strATag As String
        Dim intNdx, intLastNdx As Integer

        If (strMergeList = "") Then
            Exit Sub
        End If
        strMaster = strCSL
        If (strMaster <> "") Then
            strMaster = "," & strMaster & ","
        End If
        strTagList = Split(strMergeList, ",")
        intLastNdx = UBound(strTagList)
        For intNdx = 0 To intLastNdx
            strATag = strTagList(intNdx)
            If (strMaster.IndexOf("," & strATag & ",") = -1) Then
                If (strMaster = "") Then
                    strMaster = ","
                End If
                strMaster &= strATag & ","
            End If
        Next
        strMaster = strMaster.Substring(1, strMaster.Length - 2)
        strTagList = Split(strMaster, ",")
        Array.Sort(strTagList)
        intLastNdx = UBound(strTagList)
        For intNdx = 0 To intLastNdx
            If (intNdx = 0) Then
                strMaster = strTagList(0)
            Else
                strMaster &= "," & strTagList(intNdx)
            End If
        Next
        strCSL = strMaster
    End Sub

    Public Function CompareCSL(strMaster As String, strCompareTo As String) As Integer
        ' 1: lists match exactly
        ' -1: strCompareTo is a subset of strMaster
        ' 0: compare fails (strCompareTo has an element not in strMaster)
        Dim strElements() As String
        Dim strMasterCopy As String
        Dim intNdx, intLastNdx As Integer
        If (strMaster = strCompareTo) Then
            Return 1
        End If
        ' master might be blank or nonblank but in either case empty compareto will be a subset
        If (strCompareTo = "") Then
            Return -1
        End If
        ' at this point we know strCompareTo is nonblank so if master is blank it is automatically a failure
        If (strMaster = "") Then
            Return 0
        End If
        strMasterCopy = "," & strMaster & ","
        strElements = Split(strCompareTo, ",")
        intLastNdx = UBound(strElements)
        For intNdx = 0 To intLastNdx
            If (strMasterCopy.IndexOf("," & strElements(intNdx) & ",") = -1) Then
                Return 0
            End If
        Next
        Return -1
    End Function
End Class
