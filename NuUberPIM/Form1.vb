Option Explicit On
Option Strict On

Imports System.ComponentModel
' On any release build, eliminate any code warnings
' AutoSave after N changes? (if TRN works well this should not be needed, try very log TRN recovery)
' Clean up transaction functions form
' .. what does check current instance do vs Check button now on main form
' .. display fake tree based on start to item selected in transaction list box (validate it after setting it up)
' .. save selected state as new current (default is using the whole selection)
' Then work rewind/apply step by step with show, and ability to then resave that new state as current
' .. work code to allow apply not all of trn file ... auto applying all may just crash again (manual edit to TRN file for now)
' .. ready to work on rollback from start of previous version / can do an undo also
' .. also auto verify and save when idle or after N operations (option)
' .. Exercise TRN log base + TRN so far = current
' number the transaction records so can record rollbacks?  Silly if allowing manual edits to fix?
' using new change counter, save new version on close only if there are changes since last save
' do we need/want form indicator for resort,validation etc in progress?
' work on preventing illegal moves
' (DONE) Fix for validation after initial load only
' (DONE) after simple move, select node that was moved not where it was moved to?
' (FIXED) priority algorithm ... was not handling priority corrrectly in several cases
' ReSort is done on Add or Edit ... but not on initial build, if we do it better, can avoid needing to re-sort so much
' .. appropriate use of BeginUpdate and EndUpdate
' (DONE) Should there be a tree sanity validator that can be called on demand?
' (DONE) need to walk through again, node expand and after selects that happen towards and moving a node
' (DONE) Clean up event sequence / bypass of events during move / DoEvents
' Further testing and research on events
' .. Danger/bad practice of DoEvents in logging? (research)
' .. Apparently, adding an expanded node to a node triggers after expand??  curious ... problematical?  look at later
' .. seems like excessive chatter, resort/redisplay node is part of it, get nested select/expand events
' .... fixed sudden breakage of move due to nested select/expand events, expand undid ignore events
' ...... this will become a problem if other event procs want to ignore events
' ...... go through expand, resort, select/unselect chains
' ..(FIXED - AfterSelect nests during sort after move)(FIXED - had dup node from previous bug) 
' INFO: Load event fires on Show or ShowDialog, not on instantiation
' INFO: TextChanged does not fire if you set the text field manually in code
' INFO: VisibleChanged event for form will nested fire immediately on .Show or .Hide
' set up "related to" and maybe "dependent on" (will be some work!)
' (DONE) Two options set at start of every source file
' (DONE) Magic to display version # of app
' Passing nodes by value, but altering their Tag?
' proper mix of passing nodes or node tags (items) to routines
' Passing TreeViews by value?  Or does it do ByRef under the hood?
' What defines node a == node b ... text? tag? address/checksum? other?
' Make sure on ref to structure vs new structure, and copy of reference vs direct reference
' .. consistency on New vs not for items ... be sure to set into tag of node
' cTreeManipulator should be INSTANCED as either live or copy mode ... New() ... do not pass params to every call
' ..Sanity check all trn log class creation/entry, clean up (e.g. should just pass tv on New)
' ..should not be passing tree into calls any more, its in the transaction class
' Also heavy testing needed for all operations (disallow and perform)
' (DONE) Soon need to be able to use TRN to recover from code crash
' (WORKED AROUND) weirdness with EOL on TRNLOG during recovery
' look for sanity and extra new version after auto recovery
' .. appropriate use of verify sanity calls
' initial form size weirdness vs in designer
' FULL CODE LOGIC CHECK AND REORGANIZE
' (DONE) outer exception handler + check file
' move things "around" between main, tree manipulator and ToDoItem
' (DONE) special protections needed for special nodes
' Disallow deletes of any child of trash... done?
' global and module variables nothing, check if nothing
' parameters check to assure nothing
' (FIXED) TRN file is recreating/overwriting each time ... not good
' SelectedNode, always one or can be many?  Assuming exactly one right now e.g. for Edit
' .. look at the weirdness of actual select auto highlight vs manually clearing/setting it
' (FIXED) Move is not being permanent ... probably not refreshing tag in nodes? (no, need to set intParentNbr when moving)
' (INVALIDATED) Need to record the node types in the data for sense on special node types to work (node number implies special node for now)
' Double check for ContentFile state before doing backup/rename/write (?)
' (FIXED) System.IO.File.OpenText, CreateText, AppendText vs old Open (New vs not)
' Check that same or similar Save sequence used everywhere
' (FIXED) on open existing verify last node number
' After we implement true delete, there will be holes in node numbers (should mostly handle now)
' .. once we allow housekeeping of trash, simple "last node seen is max node number created" will not work
' (DONE) verify files properly closed where appropriate
' .. double check transition from recovery to close app
' Incremental search should not show special nodes (only issue during new file?)
' (DONE) Auto expand ToDo and QuickAccess when first child is added
' (FIXED) Quick Add + Enhanced Add / allow for other attributes and write to database
' (DONE) Add display (done)/sorting (to do) handlers for Dates etc.
' (FIXED) ... item with a bump to top of today still comes before an item with a date of event of today
' (FIXED/MERGED) Ensure text and notes are converted/protected ... especially control chars like NL,CR,LF
' .. tab attempt in text boxes actually goes to next field
' .. double check encode/decode and display as node text or in Edit
' .. full protection against binary/line break chars when in text files
' .. Test encode/decode with legitimate unicode (emdash family)(emoticons as an example)(umlauted chars?)
'.... use of TextBox may foil this ... e.g emdash is apparently changed to a regular dash
' (FIXED) Any change to a node content (not position) should change modify date
' .. Do not change modify date for certain types of changes (?)
' (FIXED) compact storage of all node attributes
' Where is intParentNodeNbr set/looked at ... verify appropriate use everywhere
' All forms: Form closing block or act as Ok/Cancel (FIXED: on main form can lose data if close without save)
' (FIXED) Current brain freeze: add then extended edit ... node with values is not created yet
' (FIXED) Store/Display created/modified dates including time part?  GMT?
' .. Decide on GMT store/display/offset for create/modify dates
' On edit detect if any actual changes / show actual changes
' implement priority to top
' implement show all dated items / all prioritized items regardless of location (warn on load?)
' implement find ... two modes?
' (FIXED) redo storage for all item fields / check on read for < and >
' (FIXED) store gdtnull for dates as null string
' (FIXED) check sanity on load for -1 node, global last node number not incremented (node nbr is -1)
' (FIXED/MERGED) implement date editor, and include in sort criteria
' .. saw a case where it stored/displayed time in addition to date / fully handle day included or not
' (FIXED) have to solve logic/event issue with (close minimode)(open main)(force add)(add self closes)(close main)(reopen minimode)
' (DONE) Need to figure out how to pass sort comparer function to Sort method (ICompare implement vs Lambda vs AddressOf vs ...)
' Compartmentalizing item structure outside of Form1 / global variable block etc.
' (DONE) Review practice of ID'ing node type by node number instead of node type stored in record
' Implement checkboxes including transactions
' Consistent prefixes for data types + g/m
' reserved words to avoid: Parent, Validate
' (DONE) Ensure all significant changes to tree/data are done as transactions
' (DONE?) Strip or comment code that uses embedded node types / code based on known node numbers
' Audit mainline code for things that are simplified/altered/omitted e.g. new mini mode form
Public Class Form1

    Dim gdtNull As Date = DateValue("01/01/1900")

    Public gboolMiniMode As Boolean = False
    Dim gfmMiniMode As New frmMini

    ' *** is "current"/"last version written" now always private to transaction class?
    Dim gstrContentDirectory As String = ""
    Dim gstrCurrentPIMFileBasename As String = ""
    Dim gstrCurrentContentFile As String
    Dim gstrCurrentTransactionFile As String
    Public gstrCurrentErrorFile As String = ""
    Dim gfContentFile As IO.StreamWriter
    Dim gfTransactionFile As System.IO.StreamWriter
    Dim gMoveToParent As TreeNode = Nothing
    Dim gNodeToMove As TreeNode = Nothing
    Dim gintMoveMode As Integer = 0
    ' *** how/where needed and where should event handlers be, here or in the transaction class? (for each, needed if not main visible tree?)
    Dim gBoolBypassEvents As Boolean = False

    Dim UGBL As New cUtility
    Dim gActual As cTreeManipulator
    Dim L As cLogging
    Dim TODO As New cToDoItem
    '
    ' Startup and Exit
    '
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim fOutFile As IO.StreamWriter
        Dim fInFile As IO.StreamReader
        Dim strConfigFileFN As String
        Dim fConfigFile As IO.StreamReader
        Dim strBuffer As String
        Dim strBufferElements() As String
        Dim strStartupOptions As String = ""
        Dim strLastTransaction As String = ""
        Dim intNdx As Integer
        Dim intWantToAutoValidateAfterThisManyChanges As Integer = -1
        ' Testing block for Encode/Decode
        'Dim strTest As String
        'strTest = "Hello World"
        'strTest = EncodeToAvoid(strTest, ":")
        'strTest = DecodeFromAvoid(strTest)
        'strTest = "Hello:World"
        'strTest = EncodeToAvoid(strTest, ":")
        'strTest = DecodeFromAvoid(strTest)
        'strTest = "Hello%World"
        'strTest = EncodeToAvoid(strTest, ":")
        'strTest = DecodeFromAvoid(strTest)
        ' End testing block

        Me.Text = "NuUberPim v" & Reflection.Assembly.GetExecutingAssembly.GetName().Version.ToString
        L = New cLogging(tbLog)
        ' *** Currently assumes config file is in bin directory of app, not MyDocuments,AppData or elsewhere.
        ' *** Currently assumes everything in the file is a name=value line
        ' ***    It does handle comments (partial or entire line following ') and blank lines.
        ' *** Confirm TRIM take out tabs as well as spaces
        strConfigFileFN = System.IO.Directory.GetCurrentDirectory & "\Config.txt"
        If (Not FileIO.FileSystem.FileExists(strConfigFileFN)) Then
            Throw New Exception("Config.txt does not exist")
        End If
        fConfigFile = System.IO.File.OpenText(strConfigFileFN)
        While (fConfigFile.Peek <> -1)
            strBuffer = fConfigFile.ReadLine
            strBuffer = strBuffer.Trim
            intNdx = strBuffer.IndexOf("'")
            If (intNdx <> -1) Then
                If (intNdx = 0) Then
                    strBuffer = ""
                Else
                    strBuffer = strBuffer.Substring(0, intNdx)
                End If
            End If
            strBuffer = strBuffer.Trim
            If (strBuffer = "") Then
                Continue While
            End If
            If (strBuffer.Length < 3) Then
                    Throw New Exception("Line too short in Config.txt")
                End If
                strBufferElements = Split(strBuffer, "=")
            If (UBound(strBufferElements) <> 1) Then
                Throw New Exception("Line in Config.txt is not in name=value format")
            End If
            Select Case strBufferElements(0).ToUpper
                Case "DBFolder".ToUpper
                    gstrContentDirectory = strBufferElements(1)
                    If (gstrContentDirectory.Substring(gstrContentDirectory.Length - 1, 1) = "\") Then
                        gstrContentDirectory = gstrContentDirectory.Substring(0, gstrContentDirectory.Length - 1)
                    End If
                Case "OpenDefaultBaseName".ToUpper
                    gstrCurrentPIMFileBasename = strBufferElements(1)
                Case "OnStartup".ToUpper
                    strStartupOptions = strBufferElements(1)
                Case "AutoValidateEvery".ToUpper
                    If (Not IsNumeric(strBufferElements(1))) Then
                        Throw New Exception("AutoValidateEvery value must be a positive integer")
                    End If
                    intWantToAutoValidateAfterThisManyChanges = Convert.ToInt32(strBufferElements(1))
                    If (intWantToAutoValidateAfterThisManyChanges < 1) Then
                        Throw New Exception("AutoValidateEvery value must be a positive integer")
                    End If
                Case Else
                    Throw New Exception("Unknown name tag in Config.txt")
            End Select
        End While
        fConfigFile.Close()
        If (gstrContentDirectory = "") Then
            Throw New Exception("DBFolder was not defined in Config.txt")
        End If
        If (gstrCurrentPIMFileBasename = "") Then
            Throw New Exception("OpenDefaultBaseName was not defined in Config.txt")
        End If

        gstrCurrentContentFile = gstrContentDirectory & "\" & gstrCurrentPIMFileBasename & ".txt"
        gstrCurrentTransactionFile = gstrContentDirectory & "\" & gstrCurrentPIMFileBasename & "_TRN.txt"
        gstrCurrentErrorFile = gstrContentDirectory & "\" & gstrCurrentPIMFileBasename & "_Errors.txt"

        ' Should check for combos that make no sense
        ' e.g. TRN file but no DB file etc,
        If (Not FileIO.FileSystem.FileExists(gstrCurrentTransactionFile)) Then
            fOutFile = System.IO.File.CreateText(gstrCurrentTransactionFile)
            fOutFile.Close()
        Else
            ' TRN file already exists.  Verify it ends with Closing tag, else we did not exit cleanly last time.
            fInFile = System.IO.File.OpenText(gstrCurrentTransactionFile)
            Do While (fInFile.Peek <> -1)
                strBuffer = fInFile.ReadLine
                strBuffer = strBuffer.Trim
                If (strBuffer <> "") Then
                    strLastTransaction = strBuffer
                End If
            Loop
            fInFile.Close()
            If (strLastTransaction <> "<Closing>") Then
                ' If AutoRecovery was requested then do it if the current transaction log
                If (strStartupOptions.ToUpper = "AutoRecovery".ToUpper) Then
                    MessageBox.Show("Last run did not close properly, attempting repair mode")
                    RebuildAttemptFromTRNLog()
                End If
                Throw New Exception("Last run did not close properly, repair is necessary")
            End If
        End If

        gfTransactionFile = System.IO.File.AppendText(gstrCurrentTransactionFile)
        ' *** verify for "create new db" vs "open last known database" (?)

        ' *** ensure all module globals are initialized where appropriate, consistent anytime cTreeManipulator is instanced
        gActual = New cTreeManipulator()
        gActual.mTree = tvMain
        gActual.mstrContentDirectory = gstrContentDirectory
        gActual.mstrCurrentDBFileName = gstrCurrentContentFile
        gActual.mstrCurrentPIMFileBasename = gstrCurrentPIMFileBasename
        gActual.mintValidateAfterEveryThisManyChanges = intWantToAutoValidateAfterThisManyChanges
        gActual.L = L

        If (Not FileIO.FileSystem.FileExists(gstrCurrentContentFile)) Then
            ' create the initial data
            CreateNewList()
            gActual.mintLoadedDBVersion = 0
            gActual.mintLastNodeNbrUsed = 3
            gActual.mintNumberOfNodesInTree = 4
            gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Saving", Nothing, Nothing)
            gActual.SaveCurrent() ' saves base as version 1 so we can successfully do rollbacks/compares from this point on
            gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Saved", Nothing, Nothing)
        Else
            gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Open", Nothing, Nothing)
            gActual.LoadDB()
        End If

        ' *** expand it on open?
        tvMain.SelectedNode = TODO.FindNodeByNodeNbr(tvMain, 2)
        ' *** review button enable/disable states, thorough consistency
        btnStartMultiMoveHere.Enabled = True
        btnEndMultiMove.Enabled = False
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Saving", Nothing, Nothing)
        gActual.SaveCurrent()
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Saved", Nothing, Nothing)
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Closing", Nothing, Nothing)
        gfTransactionFile.Close()
    End Sub
    '
    ' Active Control handlers
    '
    Private Sub btnMiniMode_Click(sender As Object, e As EventArgs) Handles btnMiniMode.Click
        gboolMiniMode = True
        Me.Hide()
        gfmMiniMode.TopMost = True
        gfmMiniMode.Show()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        PerformAdd()
    End Sub

    'Encapuslate Add functionality so it can be done by Add button on main form or Add button on mini-form
    Public Sub PerformAdd(Optional strQuickAdd As String = "")
        Dim item As cToDoItem.sItemInfo
        Dim parent As TreeNode
        Dim parentItem As cToDoItem.sItemInfo

        Dim fmAdd As frmAdd

        If (strQuickAdd <> "") Then
            parent = TODO.FindNodeByNodeNbr(tvMain, 1)
            parentItem = CType(parent.Tag, cToDoItem.sItemInfo)
        Else
            parent = tvMain.SelectedNode
            ' *** "x Is Nothing" vs IsNothing(x)
            If (parent Is Nothing) Then
                MessageBox.Show("You must select a parent node first")
                Exit Sub
            End If
            ' cannot add manually to trash or root nodes
            parentItem = CType(parent.Tag, cToDoItem.sItemInfo)
            If (parentItem.intNodeNbr = 0) Or (parentItem.intNodeNbr = 3) Then
                MessageBox.Show("You cannot manually add a node here")
                Exit Sub
            End If
        End If
        ' Item shell is created with new item number, if we cancel, need to back off the global max item number
        gActual.mintLastNodeNbrUsed += 1
        item = TODO.CreateNewItem(gActual.mintLastNodeNbrUsed)
        If (strQuickAdd <> "") Then
            item.strText = strQuickAdd
            item.intParentNodeNbr = parentItem.intNodeNbr
            gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Add", item, Nothing)
            ' If node just added, was first new node in its parent, expand the parent and make the parent selected
            If (parent.Nodes.Count = 1) Then
                parent.Expand()
                ' *** consistent setting of selected node where needed/appropriate
                'tvMain.SelectedNode = parent
            End If
            Exit Sub
        End If

        fmAdd = New frmAdd

        fmAdd.tv = tvMain
        fmAdd.item = item
        fmAdd.ShowDialog()

        If (fmAdd.gintGoToNodeNbr <> -1) Then
            gActual.mintLastNodeNbrUsed -= 1
            tvMain.SelectedNode = TODO.FindNodeByNodeNbr(tvMain, fmAdd.gintGoToNodeNbr)
            Exit Sub
        End If

        If (Not fmAdd.gBoolCancel) Then
            item = fmAdd.item
            item.intParentNodeNbr = parentItem.intNodeNbr
            gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Add", item, Nothing)
            ' If node just added, was first new node in its parent, expand the parent and make the parent selected
            If (parent.Nodes.Count = 1) Then
                parent.Expand()
                tvMain.SelectedNode = parent
            End If
        Else
            gActual.mintLastNodeNbrUsed -= 1
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim currentNode As TreeNode
        Dim currentItem As cToDoItem.sItemInfo

        currentNode = tvMain.SelectedNode
        If (currentNode Is Nothing) Then
            MessageBox.Show("You must select a node to edit first")
            Exit Sub
        End If
        currentItem = CType(currentNode.Tag, cToDoItem.sItemInfo)
        If (currentItem.intNodeNbr <= 3) Then
            MessageBox.Show("You cannot edit this node")
            Exit Sub
        End If

        Dim fEdit As New frmEdit
        fEdit.sOriginal = currentItem
        fEdit.ShowDialog()
        If (Not fEdit.gboolDidCancel) Then
            L.WriteToLog(TODO.GetDiffDescr(currentItem, fEdit.sNew))
            gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Edit", fEdit.sNew, Nothing)
        End If
    End Sub

    Private Sub btnMove_Click(sender As Object, e As EventArgs) Handles btnMove.Click
        Dim localItem As cToDoItem.sItemInfo

        gNodeToMove = tvMain.SelectedNode

        If (gNodeToMove Is Nothing) Then
            MessageBox.Show("Must select node to move first")
        End If
        localItem = CType(gNodeToMove.Tag, cToDoItem.sItemInfo)
        If (localItem.intNodeNbr <= 3) Then
            MessageBox.Show("You cannot move this node")
            gNodeToMove = Nothing
            Exit Sub
        End If
        L.WriteToLog("Select destination for: " & localItem.strText)
        'btnMove.Enabled = False
        'btnStartMultiMoveHere.Enabled = False
        ' ** so I overloaded this to permit cancellation of a move before completion?
        'btnEndMultiMove.Enabled = True
        gintMoveMode = 1
        SetButtons1()
    End Sub

    Private Sub btnStartMultiMoveHere_Click(sender As Object, e As EventArgs) Handles btnStartMultiMoveHere.Click
        Dim localItem As cToDoItem.sItemInfo
        ' *** as super sanity check, ensure it's Nothing on entry
        gMoveToParent = tvMain.SelectedNode()
        If (gMoveToParent Is Nothing) Then
            MessageBox.Show("Must select destination parent node first")
            Exit Sub
        End If
        localItem = CType(gMoveToParent.Tag, cToDoItem.sItemInfo)
        ' *** ensure Exit Sub done on error cases ... in fact I missed the one above
        If (localItem.intNodeNbr = 0) Or (localItem.intNodeNbr = 3) Then
            MessageBox.Show("You cannot move to this node")
            gMoveToParent = Nothing
            Exit Sub
        End If
        L.WriteToLog("Will start moving subsequent selected items to: " & localItem.strText)
        ' *** should also disable most or all of the other buttons!
        'btnMove.Enabled = False
        'btnStartMultiMoveHere.Enabled = False
        'btnEndMultiMove.Enabled = True
        gintMoveMode = 2
        SetButtons1()
    End Sub

    Private Sub btnEndMultiMove_Click(sender As Object, e As EventArgs) Handles btnEndMultiMove.Click
        Dim localItem As cToDoItem.sItemInfo
        ' This button can also be used to abort a single move
        If (Not IsNothing(gNodeToMove)) Then
            gNodeToMove = Nothing
            'btnStartMultiMoveHere.Enabled = True
            'btnEndMultiMove.Enabled = False
            'btnMove.Enabled = True
            gintMoveMode = 0
            SetButtons1()
            Exit Sub
        End If
        ' multimove end
        localItem = CType(gMoveToParent.Tag, cToDoItem.sItemInfo)
        L.WriteToLog("End moving items to: " & localItem.strText)
        gMoveToParent = Nothing
        'btnStartMultiMoveHere.Enabled = True
        'btnEndMultiMove.Enabled = False
        'btnMove.Enabled = True
        'gBoolBypassEvents = False
        gintMoveMode = 0
        SetButtons1()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim localItem As cToDoItem.sItemInfo
        Dim aNode As TreeNode

        ' *** ensure exactly one node is selected
        aNode = tvMain.SelectedNode()
        If (aNode Is Nothing) Then
            MessageBox.Show("Must select node to delete first")
        End If
        localItem = CType(aNode.Tag, cToDoItem.sItemInfo)
        If (localItem.intNodeNbr <= 3) Then
            MessageBox.Show("You cannot delete this node")
            Exit Sub
        End If
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Delete", localItem, Nothing)
    End Sub
    Private Sub btnPriUp_Click(sender As Object, e As EventArgs) Handles btnPriUp.Click
        Dim item As New cToDoItem.sItemInfo
        Dim aNode As TreeNode

        aNode = tvMain.SelectedNode
        If (aNode Is Nothing) Then
            MessageBox.Show("You must select a node first")
            Exit Sub
        End If
        item = CType(aNode.Tag, cToDoItem.sItemInfo)
        If (item.intNodeNbr <= 3) Then
            MessageBox.Show("You cannot alter this node")
            Exit Sub
        End If
        If (TODO.isAnOrderedChild(tvMain, aNode)) Then
            MessageBox.Show("Changing priority is nonsensical when in an ordered sublist")
            Exit Sub
        End If
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "PriUp", item, Nothing)
    End Sub

    Private Sub btnPriDown_Click(sender As Object, e As EventArgs) Handles btnPriDown.Click
        Dim item As New cToDoItem.sItemInfo
        Dim aNode As TreeNode

        aNode = tvMain.SelectedNode
        If (aNode Is Nothing) Then
            MessageBox.Show("You must select a node first")
            Exit Sub
        End If
        item = CType(aNode.Tag, cToDoItem.sItemInfo)
        If (item.intNodeNbr <= 3) Then
            MessageBox.Show("You cannot alter this node")
            Exit Sub
        End If
        If (TODO.isAnOrderedChild(tvMain, aNode)) Then
            MessageBox.Show("Changing priority is nonsensical when in an ordered sublist")
            Exit Sub
        End If
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "PriDown", item, Nothing)
    End Sub
    Private Sub btnToggleNodeType_Click(sender As Object, e As EventArgs) Handles btnToggleNodeType.Click
        Dim item As New cToDoItem.sItemInfo
        Dim aNode As TreeNode

        aNode = tvMain.SelectedNode
        If (aNode Is Nothing) Then
            MessageBox.Show("You must select a node first")
            Exit Sub
        End If
        item = CType(aNode.Tag, cToDoItem.sItemInfo)
        If (item.intNodeNbr <= 3) Then
            MessageBox.Show("You cannot alter this node")
            Exit Sub
        End If
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Toggle", item, Nothing)
    End Sub

    Private Sub btnMoveUp_Click(sender As Object, e As EventArgs) Handles btnMoveUp.Click
        Dim item, pitem As New cToDoItem.sItemInfo
        Dim aNode, pNode As TreeNode

        aNode = tvMain.SelectedNode
        If (aNode Is Nothing) Then
            MessageBox.Show("You must select a node first")
            Exit Sub
        End If
        item = CType(aNode.Tag, cToDoItem.sItemInfo)
        ' check parent
        If (item.intParentNodeNbr = -1) Then
            MessageBox.Show("Node does not have a valid parent")
            Exit Sub
        End If
        pNode = TODO.FindNodeByNodeNbr(tvMain, item.intParentNodeNbr)
        pitem = CType(pNode.Tag, cToDoItem.sItemInfo)
        If (pitem.strChildOrder = "") Then
            MessageBox.Show("Node parent is not set to ordered")
            Exit Sub
        End If
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "MoveUp", item, pNode)
    End Sub

    Private Sub btnMoveDown_Click(sender As Object, e As EventArgs) Handles btnMoveDown.Click
        Dim item, pitem As New cToDoItem.sItemInfo
        Dim aNode, pNode As TreeNode

        aNode = tvMain.SelectedNode
        If (aNode Is Nothing) Then
            MessageBox.Show("You must select a node first")
            Exit Sub
        End If
        item = CType(aNode.Tag, cToDoItem.sItemInfo)
        ' check parent
        If (item.intParentNodeNbr = -1) Then
            MessageBox.Show("Node does not have a valid parent")
            Exit Sub
        End If
        pNode = TODO.FindNodeByNodeNbr(tvMain, item.intParentNodeNbr)
        pitem = CType(pNode.Tag, cToDoItem.sItemInfo)
        If (pitem.strChildOrder = "") Then
            MessageBox.Show("Node parent is not set to ordered")
            Exit Sub
        End If
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "MoveDown", item, pNode)
    End Sub

    Private Sub btnMoveBelow_Click(sender As Object, e As EventArgs) Handles btnMoveBelow.Click
        ' this will become a two step like regular move
        Dim localItem As New cToDoItem.sItemInfo
        Dim aNode As TreeNode

        gNodeToMove = tvMain.SelectedNode
        If (gNodeToMove Is Nothing) Then
            MessageBox.Show("You must select a node first")
            Exit Sub
        End If
        localItem = CType(gNodeToMove.Tag, cToDoItem.sItemInfo)
        If (localItem.intNodeNbr <= 3) Then
            MessageBox.Show("You cannot move this node")
            gNodeToMove = Nothing
            Exit Sub
        End If
        L.WriteToLog("Select destination for: " & localItem.strText)
        'btnMoveBelow.Enabled = False
        'btnStartMultiMoveHere.Enabled = False
        'btnEndMultiMove.Enabled = True
        gintMoveMode = 3
        SetButtons1()
    End Sub

    Private Sub btnMoveToTop_Click(sender As Object, e As EventArgs) Handles btnMoveToTop.Click
        Dim item, pitem As New cToDoItem.sItemInfo
        Dim aNode, pNode As TreeNode

        aNode = tvMain.SelectedNode
        If (aNode Is Nothing) Then
            MessageBox.Show("You must select a node first")
            Exit Sub
        End If
        item = CType(aNode.Tag, cToDoItem.sItemInfo)
        ' check parent
        If (item.intParentNodeNbr = -1) Then
            MessageBox.Show("Node does not have a valid parent")
            Exit Sub
        End If
        pNode = TODO.FindNodeByNodeNbr(tvMain, item.intParentNodeNbr)
        pitem = CType(pNode.Tag, cToDoItem.sItemInfo)
        If (pitem.strChildOrder = "") Then
            MessageBox.Show("Node parent is not set to ordered")
            Exit Sub
        End If
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "MoveToTop", item, pNode)
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Saving", Nothing, Nothing)
        gActual.SaveCurrent()
        gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Saved", Nothing, Nothing)
    End Sub

    Private Sub btnTrnLog_Click(sender As Object, e As EventArgs) Handles btnTrnLog.Click
        Dim fmTrnLog As New frmTrnLogMgmt

        fmTrnLog.gtvViewMain = tvMain
        fmTrnLog.gstrTrnLogFN = gstrCurrentTransactionFile
        fmTrnLog.gstrCurrentDBFN = gstrCurrentContentFile
        fmTrnLog.gstrPreviousDBFN = gstrContentDirectory & "\" & gstrCurrentPIMFileBasename & "_BACKUP_" & Format((gActual.mintLoadedDBVersion - 1), "000000") & ".txt"
        fmTrnLog.gintCurrentDBVersionNbr = gActual.mintLoadedDBVersion
        fmTrnLog.gstrCurrentPIMFileBasename = gstrCurrentPIMFileBasename
        fmTrnLog.gstrContentDirectory = gstrContentDirectory
        fmTrnLog.L = L

        gfTransactionFile.Close()
        fmTrnLog.ShowDialog()
        gfTransactionFile = System.IO.File.AppendText(gstrCurrentTransactionFile)
    End Sub
    '
    ' Passive Control Handlers
    '
    ' *** do we need to do anything special when main form goes invisible or becomes visible to/from minimode?
    'Private Sub Form1_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
    '    'If (Me.Visible) Then
    '    '    If (gboolMiniMode) Then
    '    '        PerformAdd()
    '    '        ' swap back of forms is done in return to callstack in frmMini
    '    '        'Me.Hide()
    '    '        'gfMiniMode.Show()
    '    '    End If
    '    'End If
    'End Sub


    Private Sub tvMain_BeforeSelect(sender As Object, e As TreeViewCancelEventArgs) Handles tvMain.BeforeSelect
        Dim currNode As TreeNode
        L.WriteToLog("Before select: " & TODO.DebugGetNodeDescr(tvMain.SelectedNode), True)
        currNode = tvMain.SelectedNode
        If (currNode Is Nothing) Then
            Exit Sub
        End If
        L.WriteToLog("Before select: unhighlight", True)
        currNode.BackColor = Color.White
    End Sub
    Private Sub tvMain_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvMain.AfterSelect
        Dim currNode As TreeNode
        Dim currNodeItem As cToDoItem.sItemInfo
        Dim tempItem As cToDoItem.sItemInfo
        Dim boolRememberBypass As Boolean
        ' Need to track how this event gets called and when ... even inside other event handlers?  Without DoEvents?
        ' AfterSelect fires during re-sort of parent, after move it complete, so it is needed for that at least
        L.WriteToLog("After select: " & TODO.DebugGetNodeDescr(tvMain.SelectedNode), True)
        If (gBoolBypassEvents) Then
            L.WriteToLog("After select: bypass", True)
            Exit Sub
        End If
        currNode = tvMain.SelectedNode
        If (currNode Is Nothing) Then
            Exit Sub
        End If
        Select Case gintMoveMode
            Case 0
                ' no move mode is in progress
                L.WriteToLog("After select: highlight", True)
                currNode.BackColor = Color.LightBlue
            Case 1
                ' move already selected gNodeToMove into this newly selected node
                If (IsNothing(gNodeToMove)) Then
                    Throw New Exception("Mode mode 1 but node to move is not defined")
                End If
                ' we've selected node to move to ... set up for move gNodeToMove to currNode
                tempItem = CType(currNode.Tag, cToDoItem.sItemInfo)
                If (tempItem.intNodeNbr = 0) Or (tempItem.intNodeNbr = 3) Then
                    MessageBox.Show("You cannot move to this node")
                    Exit Sub
                End If
                tempItem = CType(gNodeToMove.Tag, cToDoItem.sItemInfo)
                ' need to bypass this routine now until after move, because move triggers re-sort which will come back to us
                L.WriteToLog("AfterSelect: Bypass enable", True)
                gBoolBypassEvents = True
                ' now that bypass is on, deselect the node now so it won't pass through here 
                tvMain.SelectedNode = Nothing
                L.WriteToLog("After select: Start move to node", True)
                ' ** move transaction must handle if old or new parent is ordered
                gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Move", tempItem, currNode)
                L.WriteToLog("After select: End move to node", True)
                ' note by analysis of event chain: no node is selected at this point
                ' now unwind move
                ' gBoolBypassEvents = False
                currNode = gNodeToMove
                gNodeToMove = Nothing
                gintMoveMode = 0
                'btnMove.Enabled = True
                'btnStartMultiMoveHere.Enabled = True
                L.WriteToLog("AfterSelect: Bypass disable", True)
                gBoolBypassEvents = False
                ' now make the node we move selected
                tvMain.SelectedNode = currNode
                SetButtons1()
            Case 2
                ' move this newly selected node under the already selected gMoveToParent
                If (IsNothing(gMoveToParent)) Then
                    Throw New Exception("Mode mode 2 but parent is not defined")
                End If
                ' we've selected a node to move
                boolRememberBypass = gBoolBypassEvents
                L.WriteToLog("AfterSelect: Bypass enable", True)
                gBoolBypassEvents = True
                '' now that bypass is on, deselect the node now so it won't pass through here 
                'tvMain.SelectedNode = Nothing
                currNodeItem = CType(currNode.Tag, cToDoItem.sItemInfo)
                ' *** perform checks here for illegal moves
                ' *** need to implement: do not allow a parent to move to a child
                If (currNodeItem.intNodeNbr <= 3) Then
                    MessageBox.Show("You cannot move this node")
                    Exit Sub
                End If '
                L.WriteToLog("After select: Start move to parent", True)
                ' ** move transaction must handle if old or new parent is ordered
                gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Move", currNodeItem, gMoveToParent)
                L.WriteToLog("After select: End move to parent", True)
                L.WriteToLog("AfterSelect: Bypass disable", True)
                gBoolBypassEvents = boolRememberBypass
            Case 3
                ' move already selected gNodeToMove BELOW this newly selected node
                If (IsNothing(gNodeToMove)) Then
                    Throw New Exception("Mode mode 3 but node to move is not defined")
                End If
                tempItem = CType(currNode.Tag, cToDoItem.sItemInfo)
                If (tempItem.intNodeNbr <= 3) Then
                    MessageBox.Show("You cannot move below this node")
                    Exit Sub
                End If
                ' ** must not allow it to go "below" a node that does not have an ordered parent
                If (Not TODO.isOrdered(TODO.ParentOf(tvMain, currNode))) Then
                    MessageBox.Show("Parent of that node is not ordered")
                    Exit Sub
                End If
                ' ** for regular Move transaction, now needs to handle
                '   move a into b (parent is ordered or not ordered)(if ordered, put it at the bottom)
                ' ** MoveBelow is new transaction and assumes parent is ordered
                ' **** we need some deep thought here
                ' ** at this point, we need special handling to move gNodeToMove below currNode
                ' it must get inserted, and then the parent ordering list updated, and then the parent redisplayed with new order
                tempItem = CType(gNodeToMove.Tag, cToDoItem.sItemInfo)
                ' need to bypass this routine now until after move, because move triggers re-sort which will come back to us
                L.WriteToLog("AfterSelect: Bypass enable", True)
                gBoolBypassEvents = True
                ' now that bypass is on, deselect the node now so it won't pass through here 
                tvMain.SelectedNode = Nothing
                L.WriteToLog("After select: Start move below node", True)
                ' ** move transaction must handle if old or new parent is ordered
                gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "MoveBelow", tempItem, currNode)
                L.WriteToLog("After select: End move below node", True)
                ' note by analysis of event chain: no node is selected at this point
                ' now unwind move
                ' gBoolBypassEvents = False
                currNode = gNodeToMove
                gNodeToMove = Nothing
                gintMoveMode = 0
                'btnMove.Enabled = True
                'btnStartMultiMoveHere.Enabled = True
                L.WriteToLog("AfterSelect: Bypass disable", True)
                gBoolBypassEvents = False
                ' now make the node we move selected
                tvMain.SelectedNode = currNode
                SetButtons1()
            Case Else
                Throw New Exception("Bad case for move mode")
        End Select
        'If (Not (gMoveToParent Is Nothing)) Then

        '    ' we've selected a node to move
        '    boolRememberBypass = gBoolBypassEvents
        '    L.WriteToLog("AfterSelect: Bypass enable", True)
        '    gBoolBypassEvents = True
        '    '' now that bypass is on, deselect the node now so it won't pass through here 
        '    'tvMain.SelectedNode = Nothing
        '    currNodeItem = CType(currNode.Tag, cToDoItem.sItemInfo)
        '    ' *** perform checks here for illegal moves
        '    ' *** need to implement: do not allow a parent to move to a child
        '    If (currNodeItem.intNodeNbr <= 3) Then
        '        MessageBox.Show("You cannot move this node")
        '        Exit Sub
        '    End If '
        '    L.WriteToLog("After select: Start move to parent", True)
        '    gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Move", currNodeItem, gMoveToParent)
        '    L.WriteToLog("After select: End move to parent", True)
        '    L.WriteToLog("AfterSelect: Bypass disable", True)
        '    gBoolBypassEvents = boolRememberBypass
        'ElseIf (Not (gNodeToMove Is Nothing)) Then
        '    ' we've selected node to move to ... set up for move gNodeToMove to currNode
        '    tempItem = CType(currNode.Tag, cToDoItem.sItemInfo)
        '    If (tempItem.intNodeNbr = 0) Or (tempItem.intNodeNbr = 3) Then
        '        MessageBox.Show("You cannot move to this node")
        '        Exit Sub
        '    End If
        '    tempItem = CType(gNodeToMove.Tag, cToDoItem.sItemInfo)
        '    ' need to bypass this routine now until after move, because move triggers re-sort which will come back to us
        '    L.WriteToLog("AfterSelect: Bypass enable", True)
        '    gBoolBypassEvents = True
        '    ' now that bypass is on, deselect the node now so it won't pass through here 
        '    tvMain.SelectedNode = Nothing
        '    L.WriteToLog("After select: Start move to node", True)
        '    gActual.ApplyNewTransaction(gfTransactionFile, tvMain, "Move", tempItem, currNode)
        '    L.WriteToLog("After select: End move to node", True)
        '    ' note by analysis of event chain: no node is selected at this point
        '    ' now unwind move
        '    ' gBoolBypassEvents = False
        '    currNode = gNodeToMove
        '    gNodeToMove = Nothing
        '    btnMove.Enabled = True
        '    btnStartMultiMoveHere.Enabled = True
        '    L.WriteToLog("AfterSelect: Bypass disable", True)
        '    gBoolBypassEvents = False
        '    ' now make the node we move selected
        '    tvMain.SelectedNode = currNode
        'Else
        '    L.WriteToLog("After select: highlight", True)
        '    currNode.BackColor = Color.LightBlue
        'End If
    End Sub

    Private Sub tvMain_AfterExpand(sender As Object, e As TreeViewEventArgs) Handles tvMain.AfterExpand
        ' e.node is the expanded node
        Dim node As TreeNode
        Dim info As cToDoItem.sItemInfo
        Dim nodeWeExpand As TreeNode
        Dim boolRememberBypass As Boolean

        node = e.Node
        nodeWeExpand = node
        info = CType(node.Tag, cToDoItem.sItemInfo)
        L.WriteToLog("After expand: " & TODO.DebugGetNodeDescr(node), True)
        If (info.intNodeNbr <> 0) And (info.intNodeNbr <> 1) And (info.intNodeNbr <> 3) Then
            If (node.Nodes.Count > 0) Then
                node = node.Nodes(0)
                L.WriteToLog("After expand: Resort/Display parent of: " & TODO.DebugGetNodeDescr(node), True)
                ' bypass After Select so we do not superfluously select node in expanded tree
                boolRememberBypass = gBoolBypassEvents
                L.WriteToLog("AfterExpand: Bypass enable", True)
                gBoolBypassEvents = True
                gActual.ResortAndRedisplayParent(tvMain, node)
                L.WriteToLog("After expand: Resort/Display parent done", True)
                L.WriteToLog("AfterExpand: Bypass disable", True)
                gBoolBypassEvents = boolRememberBypass
                ' for now, after expand, don't change selected node, it seems jarring
                'tvMain.SelectedNode = nodeWeExpand
            End If
        End If
    End Sub
    '
    ' Other support routines
    '\
    Sub SetButtons1()
        Select Case gintMoveMode
            Case 0
                btnAdd.Enabled = True
                btnEdit.Enabled = True
                btnDelete.Enabled = True
                btnPriUp.Enabled = True
                btnPriDown.Enabled = True
                btnMove.Enabled = True
                btnStartMultiMoveHere.Enabled = True
                btnEndMultiMove.Enabled = False
                btnToggleNodeType.Enabled = True
                btnMoveUp.Enabled = True
                btnMoveDown.Enabled = True
                btnMoveBelow.Enabled = True
                btnCheck.Enabled = True
                btnSave.Enabled = True
                btnTrnLog.Enabled = True
            Case 1
                ' single move into upcoming selected parent
                btnAdd.Enabled = False
                btnEdit.Enabled = False
                btnDelete.Enabled = False
                btnPriUp.Enabled = False
                btnPriDown.Enabled = False
                btnMove.Enabled = False
                btnStartMultiMoveHere.Enabled = False
                btnEndMultiMove.Enabled = True
                btnToggleNodeType.Enabled = False
                btnMoveUp.Enabled = False
                btnMoveDown.Enabled = False
                btnMoveBelow.Enabled = False
                btnCheck.Enabled = False
                btnSave.Enabled = False
                btnTrnLog.Enabled = False
            Case 2
                ' multi move ongoing into selected parent until end
                btnAdd.Enabled = False
                btnEdit.Enabled = False
                btnDelete.Enabled = False
                btnPriUp.Enabled = False
                btnPriDown.Enabled = False
                btnMove.Enabled = False
                btnStartMultiMoveHere.Enabled = False
                btnEndMultiMove.Enabled = True
                btnToggleNodeType.Enabled = False
                btnMoveUp.Enabled = False
                btnMoveDown.Enabled = False
                btnMoveBelow.Enabled = False
                btnCheck.Enabled = False
                btnSave.Enabled = False
                btnTrnLog.Enabled = False
            Case 3
                ' single move below another node (ordered)
                btnAdd.Enabled = False
                btnEdit.Enabled = False
                btnDelete.Enabled = False
                btnPriUp.Enabled = False
                btnPriDown.Enabled = False
                btnMove.Enabled = False
                btnStartMultiMoveHere.Enabled = False
                btnEndMultiMove.Enabled = True
                btnToggleNodeType.Enabled = False
                btnMoveUp.Enabled = False
                btnMoveDown.Enabled = False
                btnMoveBelow.Enabled = False
                btnCheck.Enabled = False
                btnSave.Enabled = False
                btnTrnLog.Enabled = False
            Case Else
                Throw New Exception("SetButtons, bad value for move move")
        End Select
    End Sub
    Sub CreateNewList()
        Dim aNode As TreeNode
        Dim localItem As cToDoItem.sItemInfo

        tvMain.Nodes.Clear()
        localItem = TODO.CreateNewItem(0)
        localItem.strText = gstrCurrentPIMFileBasename
        aNode = New TreeNode()
        aNode.Text = localItem.strText
        aNode.Tag = localItem
        tvMain.Nodes.Add(aNode)   ' should become tv.Nodes(0), all other nodes children from here

        localItem = TODO.CreateNewItem(1)
        localItem.intParentNodeNbr = 0
        localItem.strText = "QuickAdd"
        aNode = New TreeNode()
        aNode.Text = localItem.strText
        aNode.Tag = localItem
        tvMain.Nodes(0).Nodes.Add(aNode)

        localItem = TODO.CreateNewItem(2)
        localItem.intParentNodeNbr = 0
        localItem.strText = "ToDo"
        aNode = New TreeNode()
        aNode.Text = localItem.strText
        aNode.Tag = localItem
        tvMain.Nodes(0).Nodes.Add(aNode)

        localItem = TODO.CreateNewItem(3)
        localItem.intParentNodeNbr = 0
        localItem.strText = "Trash"
        aNode = New TreeNode()
        aNode.Text = localItem.strText
        aNode.Tag = localItem
        tvMain.Nodes(0).Nodes.Add(aNode)
    End Sub

    Sub RebuildAttemptFromTRNLog()
        Dim fmTrnLog As New frmTrnLogMgmt

        Dim fContentFile As IO.StreamReader
        Dim strBuffer As String
        Dim strElements() As String

        fmTrnLog.gtvViewMain = tvMain
        fmTrnLog.gstrTrnLogFN = gstrCurrentTransactionFile
        fmTrnLog.gstrCurrentDBFN = gstrCurrentContentFile
        fmTrnLog.gstrCurrentPIMFileBasename = gstrCurrentPIMFileBasename
        fmTrnLog.gstrContentDirectory = gstrContentDirectory
        fmTrnLog.L = L

        fContentFile = System.IO.File.OpenText(gstrCurrentContentFile)
        strBuffer = fContentFile.ReadLine()
        strElements = Split(strBuffer, ",")
        If (strElements(0) <> "V0001") Then
            Throw New Exception("Bad file format version number")
        End If
        fmTrnLog.gintCurrentDBVersionNbr = Convert.ToInt32(strElements(1))
        fContentFile.Close()

        fmTrnLog.ShowDialog()

        If (fmTrnLog.gboolRestart) Then
            End
        End If
        Throw New Exception("AutoRepair did not complete properly")
    End Sub

    Private Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click
        If (gActual.ValidateTree(tvMain, gActual.mintNumberOfNodesInTree, gActual.mintLastNodeNbrUsed)) Then
            L.WriteToLog("Validation passed")
        Else
            L.WriteToLog("Validation failed (" & gActual.gstrValidationErrorReason & ")")
        End If
    End Sub


End Class
