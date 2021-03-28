Public Class cSortableDictionaryWithDupKeys

    Dim dict As New Dictionary(Of String, String)
    Dim sortedKeys As List(Of String)
    Public Sub AddValueForKey(strKey As String, intValue As Integer)
        Dim strCurrentValues As String

        If dict.TryGetValue(strKey, strCurrentValues) Then
            strCurrentValues &= "," & intValue
            dict(strKey) = strCurrentValues
        Else
            dict.Add(strKey, intValue.ToString)
        End If
    End Sub

    Public Sub SortDictionary(boolSpecialSortFunction As Boolean)
        Dim intNdx, intLastNdx As Integer
        sortedKeys = New List(Of String)
        intLastNdx = dict.Count - 1
        For intNdx = 0 To intLastNdx
            sortedKeys.Add(dict.Keys(intNdx))
        Next
        If (boolSpecialSortFunction) Then

            ' key order/mapping: (date of event 0..7)(bump to top date 8..15)(pri 16..18)(node num 19..26)
            sortedKeys.Sort(Function(s1 As String, s2 As String) As Integer
                                ' result -1 s1 before s2, 1 s2 before s1, 0 no difference
                                Dim intResult As Integer = 0
                                Dim t1, t2 As String
                                Dim d1, d2 As Date
                                Dim i1, i2 As Integer
                                Dim intDOEResult As Integer = 0

                                ' so the issue is if the date of event yields "later" because it's in the future, bump to top date is never checked
                                ' date of event first
                                t1 = s1.Substring(0, 8)
                                t2 = s2.Substring(0, 8)
                                If ((t1 = "________") And (t2 <> "________")) Then
                                    ' second one has a date portion, first one does not
                                    ' if the date portion of the second one is in the future, it comes later
                                    d2 = DateValue(t2.Substring(4, 2) & "/" & t2.Substring(6, 2) & "/" & t2.Substring(0, 4))
                                    If (Date.Compare(d2, Date.Now.Date) > 0) Then
                                        intDOEResult = -1
                                    Else
                                        intDOEResult = 1
                                    End If
                                ElseIf ((t1 <> "________") And (t2 = "________")) Then
                                    d1 = DateValue(t1.Substring(4, 2) & "/" & t1.Substring(6, 2) & "/" & t1.Substring(0, 4))
                                    If (Date.Compare(d1, Date.Now.Date) > 0) Then
                                        intDOEResult = 1
                                    Else
                                        intDOEResult = -1
                                    End If
                                ElseIf ((t1 = "________") And (t2 = "________")) Then
                                    intDOEResult = 0
                                Else
                                    d1 = DateValue(t1.Substring(4, 2) & "/" & t1.Substring(6, 2) & "/" & t1.Substring(0, 4))
                                    d2 = DateValue(t2.Substring(4, 2) & "/" & t2.Substring(6, 2) & "/" & t2.Substring(0, 4))
                                    intDOEResult = Date.Compare(d1, d2) 'if d1<d2 returns -1
                                End If
                                If (intDOEResult = 0) Then
                                    t1 = s1.Substring(8, 8)
                                    t2 = s2.Substring(8, 8)
                                    If ((t1 = "________") And (t2 <> "________")) Then
                                        ' second one has a date portion, first one does not
                                        ' if the date portion of the second one is in the future, it comes later
                                        d2 = DateValue(t2.Substring(4, 2) & "/" & t2.Substring(6, 2) & "/" & t2.Substring(0, 4))
                                        If (Date.Compare(d2, Date.Now.Date) > 0) Then
                                            intResult = -1
                                        Else
                                            intResult = 1
                                        End If
                                    ElseIf ((t1 <> "________") And (t2 = "________")) Then
                                        d1 = DateValue(t1.Substring(4, 2) & "/" & t1.Substring(6, 2) & "/" & t1.Substring(0, 4))
                                        If (Date.Compare(d1, Date.Now.Date) > 0) Then
                                            intResult = 1
                                        Else
                                            intResult = -1
                                        End If
                                    ElseIf ((t1 = "________") And (t2 = "________")) Then
                                        intResult = 0
                                    Else
                                        d1 = DateValue(t1.Substring(4, 2) & "/" & t1.Substring(6, 2) & "/" & t1.Substring(0, 4))
                                        d2 = DateValue(t2.Substring(4, 2) & "/" & t2.Substring(6, 2) & "/" & t2.Substring(0, 4))
                                        intResult = Date.Compare(d1, d2) 'if d1<d2 returns -1
                                    End If
                                End If
                                ' now resolve DOE vs BTTD
                                If (intResult = 0) Then
                                    intResult = intDOEResult
                                End If
                                '
                                If (intResult = 0) Then
                                    i1 = Convert.ToInt32(s1.Substring(16, 3))
                                    i2 = Convert.ToInt32(s2.Substring(16, 3))
                                    If (i1 = 0) And (i2 = 0) Then
                                        intResult = 0
                                    ElseIf (i1 = 0) And (i2 <> 0) Then
                                        intResult = 1
                                        ' 03/24/2021 well that was dumb
                                        'ElseIf (i1 <> 0) And (i2 <> 0) Then
                                        '    intResult = -1
                                    ElseIf (i1 <> 0) And (i2 = 0) Then
                                        intResult = -1
                                    Else
                                        ' was also dumb
                                        If (i1 > i2) Then
                                            intResult = -1
                                        ElseIf (i1 < i2) Then
                                            intResult = 1
                                        Else
                                            intResult = 0
                                        End If
                                    End If
                                End If
                                If (intResult = 0) Then
                                    i1 = Convert.ToInt32(s1.Substring(19, 8))
                                    i2 = Convert.ToInt32(s2.Substring(19, 8))
                                    If (i1 > i2) Then
                                        intResult = 1
                                    Else
                                        intResult = -1
                                    End If
                                End If

                                Return intResult
                            End Function)
        Else
            sortedKeys.Sort()
        End If

    End Sub

    Public ReadOnly Property SortedCount
        Get
            Return sortedKeys.Count
        End Get
    End Property
    Public Function GetValuesForSortedIndex(intSlotNdx As Integer) As Integer()
        Dim intReturn() As Integer
        Dim strTemp() As String
        Dim intNdx, intLastNdx As Integer

        strTemp = Split(dict(sortedKeys(intSlotNdx)), ",")
        intLastNdx = UBound(strTemp)
        ReDim intReturn(intLastNdx)
        For intNdx = 0 To intLastNdx
            intReturn(intNdx) = Convert.ToInt32(strTemp(intNdx))
        Next

        Return intReturn
    End Function
End Class
