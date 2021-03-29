Option Explicit On
Option Strict On
Public Class frmTrnLogMgmt

    Public gtvViewMain As TreeView
    Public gstrTrnLogFN As String
    Public gstrCurrentDBFN As String '*
    Public gstrPreviousDBFN As String
    Public gintCurrentDBVersionNbr As Integer
    Public gstrContentDirectory As String '*
    Public gstrCurrentPIMFileBasename As String '*
    Public gboolRestart As Boolean = False
    Public L As cLogging

    Private TODO As New cToDoItem
    Private mOurTreeCopy As TreeView
    Dim mlclTRN As cTreeManipulator

    ' on entry, main TRN log is CLOSED
    ' passes in pointers to current tree and
    '   name of TRN log that was just closed
    '   name of last DB file written
    '   name of previous DB file written (needed?)
    Private Sub frmTrnLogMgmt_Load(sender As Object, e As EventArgs) Handles Me.Load
        '
        Dim fFileRead As IO.StreamReader
        Dim strBuffer As String
        Dim strAttributes As String
        Dim strElements() As String
        Dim intNdx As Integer
        Dim strTRNOperation As String
        Dim boolHasAttributes As Boolean
        Dim intState As Integer = 0

        ' initially we construct what should be a copy of the main tree by base + trn log
        ' and verify it matches the main tree
        '
        tbReviewLog.Text = ""
        lbTrnSteps.Items.Clear()

        fFileRead = System.IO.File.OpenText(gstrTrnLogFN)
        ' advance until we see SAVING/SAVED for previous version
        ' *** handle multiple partial opens
        ' (DONE) *** when writing to TRNLOG use same encode/decode as DB
        Do While (fFileRead.Peek <> -1)
            strBuffer = fFileRead.ReadLine
            If (strBuffer.Trim = "") Then
                Continue Do
            End If
            If (strBuffer.Substring(0, 1) <> "<") Or (strBuffer.Substring(strBuffer.Length - 1, 1) <> ">") Then
                Throw New Exception("bad transaction log record")
            End If
            strBuffer = strBuffer.Substring(1, strBuffer.Length - 2)
            ' advance to first space
            intNdx = strBuffer.IndexOf(" ")
            If (intNdx = -1) Then
                boolHasAttributes = False
                intNdx = strBuffer.Length - 1
            Else
                boolHasAttributes = True
                intNdx -= 1
            End If
            strTRNOperation = strBuffer.Substring(0, intNdx + 1)
            If (boolHasAttributes) Then
                strAttributes = strBuffer.Substring(intNdx + 2)
                strElements = Split(strAttributes, ":")
            Else
                strAttributes = ""
            End If
            Select Case intState
                Case 0
                    ' looking for SAVING for current version (i.e. current base file before current live changes)
                    If (strTRNOperation = "Saving") Then
                        If (Convert.ToInt32(strElements(0)) = gintCurrentDBVersionNbr) Then
                            intState = 1
                        End If
                    End If
                Case 1
                    ' should now get SAVED else error
                    If (strTRNOperation <> "Saved") Then
                        Throw New Exception("Bad state in TRN log 001")
                    End If
                    intState = 2
                Case 2
                    ' ignore any closing tag
                    If (strTRNOperation = "Closing") Then
                        Continue Do
                    End If
                    ' must have at least one OPEN tag
                    If (strTRNOperation <> "Open") Then
                        Throw New Exception("Bad state in TRN log 002")
                    End If
                    intState = 3
                Case 3
                    ' skip over any extra OPEN tags
                    If (strTRNOperation <> "Open") Then
                        Exit Do
                    End If
                Case Else
                    Throw New Exception("Bad state while pre-analyzing transaction log")
            End Select
        Loop
        ' when we get here we should have the first real trn record loaded
        ' we will by default create a local tree created from the last saved DB
        ' then apply all the transactions since it was saved, to the local tree
        ' then compare the local tree to the tree passed in, content should match exactly
        'Dim lcltv As New TreeView
        mOurTreeCopy = New TreeView
        Dim lclTRN As New cTreeManipulator
        lclTRN.mTree = mOurTreeCopy
        lclTRN.mstrContentDirectory = gstrContentDirectory
        lclTRN.mstrCurrentDBFileName = gstrCurrentDBFN
        lclTRN.mstrCurrentPIMFileBasename = gstrCurrentPIMFileBasename
        lclTRN.L = L
        ' load the lcl treeview from last base file
        lclTRN.LoadDB()
        strBuffer = " "
        While (True)
            ' current data is loaded
            ' show for now, but also process
            If (strBuffer <> "") Then
                lbTrnSteps.Items.Add(strTRNOperation & "->(" & strAttributes & ")")
                '
                ' Process it here
                '
                lclTRN.ApplyTransactionFromLog(mOurTreeCopy, strTRNOperation, strElements)
            End If
            '
            If (fFileRead.Peek = -1) Then
                Exit While
            End If
            '
            strBuffer = fFileRead.ReadLine
            If (strBuffer.Trim = "") Then
                Continue While
            End If
            strBuffer = strBuffer.Substring(1, strBuffer.Length - 2)
            ' advance to first space
            intNdx = strBuffer.IndexOf(" ")
            If (intNdx = -1) Then
                boolHasAttributes = False
                intNdx = strBuffer.Length - 1
            Else
                boolHasAttributes = True
                intNdx -= 1
            End If
            strTRNOperation = strBuffer.Substring(0, intNdx + 1)
            If (boolHasAttributes) Then
                strAttributes = strBuffer.Substring(intNdx + 2)
                strElements = Split(strAttributes, ":")
            Else
                strAttributes = ""
            End If
            '
        End While

        fFileRead.Close()

        mlclTRN = lclTRN
    End Sub
    Private Sub btnCheckToThisPoint_Click(sender As Object, e As EventArgs) Handles btnCheckToThisPoint.Click
        ' simply verify that the reconstructed tree matches the main tree
        Dim intErrorCount As Integer = 0

        Dim dCopy, dMain As New Dictionary(Of Integer, String)
        WalkTree(dCopy, mOurTreeCopy.Nodes(0))
        WalkTree(dMain, gtvViewMain.Nodes(0))
        ' verify the dictionaries are the same
        Dim lCopyKeys, lMainKeys As List(Of Integer)
        Dim intCopyNdx, intCopyLastNdx, intMainNdx, intMainLastNdx As Integer

        lCopyKeys = dCopy.Keys.ToList
        lCopyKeys.Sort()
        lMainKeys = dMain.Keys.ToList
        lMainKeys.Sort()
        intCopyLastNdx = lCopyKeys.Count - 1
        intMainLastNdx = lMainKeys.Count - 1
        tbReviewLog.Text &= "Count of nodes in Main: " & lMainKeys.Count & vbCrLf
        tbReviewLog.Text &= "Count of nodes in Copy: " & lCopyKeys.Count & vbCrLf

        intCopyNdx = 0
        intMainNdx = 0
        Do While (True)
            If (intCopyNdx <= intCopyLastNdx) And (intMainNdx <= intMainLastNdx) Then
                If (lCopyKeys(intCopyNdx) < lMainKeys(intMainNdx)) Then
                    tbReviewLog.Text &= "Copy has unexpected node " & lCopyKeys(intCopyNdx) & vbCrLf
                    tbReviewLog.Text &= "..<" & dCopy(lCopyKeys(intCopyNdx)) & ">" & vbCrLf
                    intErrorCount += 1
                    intCopyNdx += 1
                ElseIf (lCopyKeys(intCopyNdx) > lMainKeys(intMainNdx)) Then
                    tbReviewLog.Text &= "Main has unexpected node " & lCopyKeys(intCopyNdx) & vbCrLf
                    tbReviewLog.Text &= "..<" & dMain(lMainKeys(intMainNdx)) & ">" & vbCrLf
                    intErrorCount += 1
                    intMainNdx += 1
                Else
                    If (dCopy(lCopyKeys(intCopyNdx)) <> dMain(lMainKeys(intMainNdx))) Then
                        tbReviewLog.Text &= "Copy and Main node content differ for node " & lCopyKeys(intCopyNdx)
                        tbReviewLog.Text &= ".. Main <" & dMain(lMainKeys(intMainNdx)) & ">" & vbCrLf
                        tbReviewLog.Text &= ".. Copy <" & dCopy(lCopyKeys(intCopyNdx)) & ">" & vbCrLf
                        intErrorCount += 1
                    End If
                    intCopyNdx += 1
                    intMainNdx += 1
                End If
            Else
                Do While (intCopyNdx <= intCopyLastNdx)
                    tbReviewLog.Text &= "Copy has unexpected node " & lCopyKeys(intCopyNdx) & vbCrLf
                    tbReviewLog.Text &= "..<" & dCopy(lCopyKeys(intCopyNdx)) & ">" & vbCrLf
                    intErrorCount += 1
                    intCopyNdx += 1
                Loop
                Do While (intMainNdx <= intMainLastNdx)
                    tbReviewLog.Text &= "Main has unexpected node " & lCopyKeys(intCopyNdx) & vbCrLf
                    tbReviewLog.Text &= "..<" & dMain(lMainKeys(intMainNdx)) & ">" & vbCrLf
                    intErrorCount += 1
                    intMainNdx += 1
                Loop
                Exit Do
            End If
        Loop
        tbReviewLog.Text &= "Number of errors: " & intErrorCount & vbCrLf
        tbReviewLog.SelectionStart = tbReviewLog.Text.Length - 1
        tbReviewLog.ScrollToCaret()
    End Sub

    Sub WalkTree(d As Dictionary(Of Integer, String), n As TreeNode)
        Dim item As cToDoItem.sItemInfo
        Dim intNdx As Integer
        Dim intLastNdx As Integer
        Dim strEncodedItem As String

        item = CType(n.Tag, cToDoItem.sItemInfo)
        strEncodedItem = TODO.EncodeItem(item)
        d.Add(item.intNodeNbr, strEncodedItem)

        intLastNdx = n.Nodes.Count - 1
        If (n.Nodes.Count >= 0) Then
            For intNdx = 0 To intLastNdx
                WalkTree(d, n.Nodes(intNdx))
            Next
        End If
    End Sub

    Private Sub btnSetAsRepair_Click(sender As Object, e As EventArgs) Handles btnSetAsRepair.Click
        Dim tf As System.IO.StreamWriter
        tbReviewLog.Text &= "Trying to SaveCurrent" & vbCrLf
        tf = System.IO.File.AppendText(gstrTrnLogFN)
        mlclTRN.ApplyNewTransaction(tf, mOurTreeCopy, "Saving", Nothing, Nothing)
        mlclTRN.SaveCurrent()
        mlclTRN.ApplyNewTransaction(tf, mOurTreeCopy, "Saved", Nothing, Nothing)
        mlclTRN.ApplyNewTransaction(tf, mOurTreeCopy, "Closing", Nothing, Nothing)
        tf.Close()
        tbReviewLog.Text &= "SaveCurrent Done" & vbCrLf
        gboolRestart = True
    End Sub
End Class