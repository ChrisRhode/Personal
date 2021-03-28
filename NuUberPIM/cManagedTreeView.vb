Public Class cManagedTreeView
    Public G As cGlobals

    Private tvOurTreeView As TreeView
    Private ourTrnLog As cTrnLog
    Private isMainTreeView As Boolean

    Sub New(tv As TreeView, boolIsMainTreeView As Boolean, gPtr As cGlobals)
        tvOurTreeView = tv
        isMainTreeView = boolIsMainTreeView
        G = gPtr
    End Sub


End Class
