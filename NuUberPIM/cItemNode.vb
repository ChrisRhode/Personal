Public Class cItemNode
    ' Try to localize anything related to the "database" structure here
    ' Some stuff is hybrid as it crosses streams with meta management of the database and the tree controls
    Public G As cGlobals

    Public Structure sItemInfo
        Dim intNodeNbr As Integer
        Dim intParentNodeNbr As Integer
        'Dim eNodeType As eNodeTypes
        Dim dtCreated As Date
        Dim dtModified As Date
        Dim strText As String
        Dim strNotes As String
        Dim dtDateOfEvent As Date
        Dim dtDateOfBumpToTop As Date
        Dim intPriority As Integer
        Dim isChecked As Boolean
    End Structure

    Public Enum eNodeTypes
        eValueNormal = 0
        eValueQuickAdd = 1
        eValueRoot = 2
        eValueTrash = 3
        eValueHiddenRoot = 4
    End Enum

    Dim gdtNull As Date = DateValue("01/01/1900")

    Public Function CreateNewItem() As sItemInfo
        Dim item As New sItemInfo

        G.gintLastNodeNbr += 1
        With item
            .intNodeNbr = G.gintLastNodeNbr
            .intParentNodeNbr = -1
            .dtCreated = Date.Now
            .dtModified = gdtNull
            .strText = "(Undefined)"
            .strNotes = ""
            .dtDateOfEvent = gdtNull
            .dtDateOfBumpToTop = gdtNull
            .intPriority = 0
            .isChecked = False
        End With

        Return item
    End Function
End Class
