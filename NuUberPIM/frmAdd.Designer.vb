<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdd
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tbNewItem = New System.Windows.Forms.TextBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.tvWhereFound = New System.Windows.Forms.TreeView()
        Me.lbMatches = New System.Windows.Forms.ListBox()
        Me.cbPartialWordMatches = New System.Windows.Forms.CheckBox()
        Me.btnGoto = New System.Windows.Forms.Button()
        Me.btnFindItemsWithDates = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'tbNewItem
        '
        Me.tbNewItem.Location = New System.Drawing.Point(25, 22)
        Me.tbNewItem.Name = "tbNewItem"
        Me.tbNewItem.Size = New System.Drawing.Size(560, 22)
        Me.tbNewItem.TabIndex = 0
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(606, 20)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 32)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(688, 20)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 32)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(606, 62)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(157, 32)
        Me.btnEdit.TabIndex = 4
        Me.btnEdit.Text = "Extended Edit"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'tvWhereFound
        '
        Me.tvWhereFound.Location = New System.Drawing.Point(25, 256)
        Me.tvWhereFound.Name = "tvWhereFound"
        Me.tvWhereFound.Size = New System.Drawing.Size(560, 152)
        Me.tvWhereFound.TabIndex = 5
        '
        'lbMatches
        '
        Me.lbMatches.FormattingEnabled = True
        Me.lbMatches.ItemHeight = 16
        Me.lbMatches.Location = New System.Drawing.Point(25, 78)
        Me.lbMatches.Name = "lbMatches"
        Me.lbMatches.Size = New System.Drawing.Size(560, 148)
        Me.lbMatches.TabIndex = 6
        '
        'cbPartialWordMatches
        '
        Me.cbPartialWordMatches.AutoSize = True
        Me.cbPartialWordMatches.Location = New System.Drawing.Point(604, 116)
        Me.cbPartialWordMatches.Name = "cbPartialWordMatches"
        Me.cbPartialWordMatches.Size = New System.Drawing.Size(165, 21)
        Me.cbPartialWordMatches.TabIndex = 7
        Me.cbPartialWordMatches.Text = "Partial Word Matches"
        Me.cbPartialWordMatches.UseVisualStyleBackColor = True
        '
        'btnGoto
        '
        Me.btnGoto.Location = New System.Drawing.Point(606, 167)
        Me.btnGoto.Name = "btnGoto"
        Me.btnGoto.Size = New System.Drawing.Size(75, 32)
        Me.btnGoto.TabIndex = 8
        Me.btnGoto.Text = "Go To Selected"
        Me.btnGoto.UseVisualStyleBackColor = True
        '
        'btnFindItemsWithDates
        '
        Me.btnFindItemsWithDates.Location = New System.Drawing.Point(604, 256)
        Me.btnFindItemsWithDates.Name = "btnFindItemsWithDates"
        Me.btnFindItemsWithDates.Size = New System.Drawing.Size(205, 32)
        Me.btnFindItemsWithDates.TabIndex = 9
        Me.btnFindItemsWithDates.Text = "Find Items With Dates"
        Me.btnFindItemsWithDates.UseVisualStyleBackColor = True
        '
        'frmAdd
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(821, 437)
        Me.Controls.Add(Me.btnFindItemsWithDates)
        Me.Controls.Add(Me.btnGoto)
        Me.Controls.Add(Me.cbPartialWordMatches)
        Me.Controls.Add(Me.lbMatches)
        Me.Controls.Add(Me.tvWhereFound)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.tbNewItem)
        Me.Name = "frmAdd"
        Me.Text = "Add/Find"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbNewItem As TextBox
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents tvWhereFound As TreeView
    Friend WithEvents lbMatches As ListBox
    Friend WithEvents cbPartialWordMatches As CheckBox
    Friend WithEvents btnGoto As Button
    Friend WithEvents btnFindItemsWithDates As Button
End Class
