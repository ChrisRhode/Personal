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
        Me.btnFindItemsWithTags = New System.Windows.Forms.Button()
        Me.clbTags = New System.Windows.Forms.CheckedListBox()
        Me.btnExportMatches = New System.Windows.Forms.Button()
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
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(724, 20)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 32)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(806, 20)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 32)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.Location = New System.Drawing.Point(724, 62)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(157, 32)
        Me.btnEdit.TabIndex = 4
        Me.btnEdit.Text = "Extended Edit"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'tvWhereFound
        '
        Me.tvWhereFound.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvWhereFound.Location = New System.Drawing.Point(25, 312)
        Me.tvWhereFound.Name = "tvWhereFound"
        Me.tvWhereFound.Size = New System.Drawing.Size(675, 145)
        Me.tvWhereFound.TabIndex = 5
        '
        'lbMatches
        '
        Me.lbMatches.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbMatches.FormattingEnabled = True
        Me.lbMatches.ItemHeight = 16
        Me.lbMatches.Location = New System.Drawing.Point(25, 80)
        Me.lbMatches.Name = "lbMatches"
        Me.lbMatches.Size = New System.Drawing.Size(675, 196)
        Me.lbMatches.TabIndex = 6
        '
        'cbPartialWordMatches
        '
        Me.cbPartialWordMatches.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbPartialWordMatches.AutoSize = True
        Me.cbPartialWordMatches.Location = New System.Drawing.Point(722, 116)
        Me.cbPartialWordMatches.Name = "cbPartialWordMatches"
        Me.cbPartialWordMatches.Size = New System.Drawing.Size(165, 21)
        Me.cbPartialWordMatches.TabIndex = 7
        Me.cbPartialWordMatches.Text = "Partial Word Matches"
        Me.cbPartialWordMatches.UseVisualStyleBackColor = True
        '
        'btnGoto
        '
        Me.btnGoto.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGoto.Location = New System.Drawing.Point(724, 167)
        Me.btnGoto.Name = "btnGoto"
        Me.btnGoto.Size = New System.Drawing.Size(75, 32)
        Me.btnGoto.TabIndex = 8
        Me.btnGoto.Text = "Go To Selected"
        Me.btnGoto.UseVisualStyleBackColor = True
        '
        'btnFindItemsWithDates
        '
        Me.btnFindItemsWithDates.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFindItemsWithDates.Location = New System.Drawing.Point(722, 256)
        Me.btnFindItemsWithDates.Name = "btnFindItemsWithDates"
        Me.btnFindItemsWithDates.Size = New System.Drawing.Size(205, 32)
        Me.btnFindItemsWithDates.TabIndex = 9
        Me.btnFindItemsWithDates.Text = "Find Items With Dates"
        Me.btnFindItemsWithDates.UseVisualStyleBackColor = True
        '
        'btnFindItemsWithTags
        '
        Me.btnFindItemsWithTags.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFindItemsWithTags.Location = New System.Drawing.Point(724, 294)
        Me.btnFindItemsWithTags.Name = "btnFindItemsWithTags"
        Me.btnFindItemsWithTags.Size = New System.Drawing.Size(205, 32)
        Me.btnFindItemsWithTags.TabIndex = 10
        Me.btnFindItemsWithTags.Text = "Find Items With Tags"
        Me.btnFindItemsWithTags.UseVisualStyleBackColor = True
        '
        'clbTags
        '
        Me.clbTags.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clbTags.CheckOnClick = True
        Me.clbTags.FormattingEnabled = True
        Me.clbTags.Location = New System.Drawing.Point(724, 333)
        Me.clbTags.Name = "clbTags"
        Me.clbTags.Size = New System.Drawing.Size(205, 123)
        Me.clbTags.TabIndex = 11
        '
        'btnExportMatches
        '
        Me.btnExportMatches.Location = New System.Drawing.Point(724, 206)
        Me.btnExportMatches.Name = "btnExportMatches"
        Me.btnExportMatches.Size = New System.Drawing.Size(124, 32)
        Me.btnExportMatches.TabIndex = 12
        Me.btnExportMatches.Text = "Export Matches"
        Me.btnExportMatches.UseVisualStyleBackColor = True
        '
        'frmAdd
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(951, 542)
        Me.Controls.Add(Me.btnExportMatches)
        Me.Controls.Add(Me.clbTags)
        Me.Controls.Add(Me.btnFindItemsWithTags)
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
    Friend WithEvents btnFindItemsWithTags As Button
    Friend WithEvents clbTags As CheckedListBox
    Friend WithEvents btnExportMatches As Button
End Class
