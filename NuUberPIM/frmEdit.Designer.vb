<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEdit
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbText = New System.Windows.Forms.TextBox()
        Me.tbNotes = New System.Windows.Forms.TextBox()
        Me.tbDOE = New System.Windows.Forms.TextBox()
        Me.tbBTTD = New System.Windows.Forms.TextBox()
        Me.tbPri = New System.Windows.Forms.TextBox()
        Me.btnEditDOE = New System.Windows.Forms.Button()
        Me.btnEditBTTD = New System.Windows.Forms.Button()
        Me.btnPriTop = New System.Windows.Forms.Button()
        Me.btnPriPlus = New System.Windows.Forms.Button()
        Me.btnPriMinus = New System.Windows.Forms.Button()
        Me.btnRevert = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnValidate = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbCreated = New System.Windows.Forms.TextBox()
        Me.tbModified = New System.Windows.Forms.TextBox()
        Me.clbTags = New System.Windows.Forms.CheckedListBox()
        Me.tbNewTag = New System.Windows.Forms.TextBox()
        Me.btnAddTag = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(44, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Item"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(44, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Notes"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(44, 279)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(94, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Date of Event"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(44, 319)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(128, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Bump To Top Date"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(580, 279)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(52, 17)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Priority"
        '
        'tbText
        '
        Me.tbText.Location = New System.Drawing.Point(110, 20)
        Me.tbText.Name = "tbText"
        Me.tbText.Size = New System.Drawing.Size(496, 22)
        Me.tbText.TabIndex = 5
        '
        'tbNotes
        '
        Me.tbNotes.Location = New System.Drawing.Point(110, 65)
        Me.tbNotes.Multiline = True
        Me.tbNotes.Name = "tbNotes"
        Me.tbNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbNotes.Size = New System.Drawing.Size(496, 167)
        Me.tbNotes.TabIndex = 6
        '
        'tbDOE
        '
        Me.tbDOE.Location = New System.Drawing.Point(187, 274)
        Me.tbDOE.Name = "tbDOE"
        Me.tbDOE.Size = New System.Drawing.Size(198, 22)
        Me.tbDOE.TabIndex = 7
        '
        'tbBTTD
        '
        Me.tbBTTD.Location = New System.Drawing.Point(187, 319)
        Me.tbBTTD.Name = "tbBTTD"
        Me.tbBTTD.Size = New System.Drawing.Size(198, 22)
        Me.tbBTTD.TabIndex = 8
        '
        'tbPri
        '
        Me.tbPri.Location = New System.Drawing.Point(653, 276)
        Me.tbPri.Name = "tbPri"
        Me.tbPri.Size = New System.Drawing.Size(78, 22)
        Me.tbPri.TabIndex = 9
        '
        'btnEditDOE
        '
        Me.btnEditDOE.Location = New System.Drawing.Point(408, 274)
        Me.btnEditDOE.Name = "btnEditDOE"
        Me.btnEditDOE.Size = New System.Drawing.Size(75, 23)
        Me.btnEditDOE.TabIndex = 10
        Me.btnEditDOE.Text = "Edit"
        Me.btnEditDOE.UseVisualStyleBackColor = True
        '
        'btnEditBTTD
        '
        Me.btnEditBTTD.Location = New System.Drawing.Point(408, 317)
        Me.btnEditBTTD.Name = "btnEditBTTD"
        Me.btnEditBTTD.Size = New System.Drawing.Size(75, 23)
        Me.btnEditBTTD.TabIndex = 11
        Me.btnEditBTTD.Text = "Edit"
        Me.btnEditBTTD.UseVisualStyleBackColor = True
        '
        'btnPriTop
        '
        Me.btnPriTop.Enabled = False
        Me.btnPriTop.Location = New System.Drawing.Point(583, 313)
        Me.btnPriTop.Name = "btnPriTop"
        Me.btnPriTop.Size = New System.Drawing.Size(75, 28)
        Me.btnPriTop.TabIndex = 12
        Me.btnPriTop.Text = "To Top"
        Me.btnPriTop.UseVisualStyleBackColor = True
        '
        'btnPriPlus
        '
        Me.btnPriPlus.Location = New System.Drawing.Point(583, 359)
        Me.btnPriPlus.Name = "btnPriPlus"
        Me.btnPriPlus.Size = New System.Drawing.Size(75, 23)
        Me.btnPriPlus.TabIndex = 13
        Me.btnPriPlus.Text = "+ 1"
        Me.btnPriPlus.UseVisualStyleBackColor = True
        '
        'btnPriMinus
        '
        Me.btnPriMinus.Location = New System.Drawing.Point(679, 359)
        Me.btnPriMinus.Name = "btnPriMinus"
        Me.btnPriMinus.Size = New System.Drawing.Size(75, 23)
        Me.btnPriMinus.TabIndex = 14
        Me.btnPriMinus.Text = "- 1"
        Me.btnPriMinus.UseVisualStyleBackColor = True
        '
        'btnRevert
        '
        Me.btnRevert.Location = New System.Drawing.Point(539, 407)
        Me.btnRevert.Name = "btnRevert"
        Me.btnRevert.Size = New System.Drawing.Size(75, 23)
        Me.btnRevert.TabIndex = 15
        Me.btnRevert.Text = "Revert"
        Me.btnRevert.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(620, 407)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 16
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(701, 407)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 17
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnValidate
        '
        Me.btnValidate.Location = New System.Drawing.Point(458, 407)
        Me.btnValidate.Name = "btnValidate"
        Me.btnValidate.Size = New System.Drawing.Size(75, 23)
        Me.btnValidate.TabIndex = 18
        Me.btnValidate.Text = "Validate"
        Me.btnValidate.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(44, 359)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 17)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "Created"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(44, 394)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(61, 17)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "Modified"
        '
        'tbCreated
        '
        Me.tbCreated.Location = New System.Drawing.Point(187, 356)
        Me.tbCreated.Name = "tbCreated"
        Me.tbCreated.ReadOnly = True
        Me.tbCreated.Size = New System.Drawing.Size(198, 22)
        Me.tbCreated.TabIndex = 21
        '
        'tbModified
        '
        Me.tbModified.Location = New System.Drawing.Point(187, 394)
        Me.tbModified.Name = "tbModified"
        Me.tbModified.ReadOnly = True
        Me.tbModified.Size = New System.Drawing.Size(198, 22)
        Me.tbModified.TabIndex = 22
        '
        'clbTags
        '
        Me.clbTags.CheckOnClick = True
        Me.clbTags.FormattingEnabled = True
        Me.clbTags.Location = New System.Drawing.Point(620, 116)
        Me.clbTags.Name = "clbTags"
        Me.clbTags.Size = New System.Drawing.Size(213, 123)
        Me.clbTags.TabIndex = 23
        '
        'tbNewTag
        '
        Me.tbNewTag.Location = New System.Drawing.Point(620, 68)
        Me.tbNewTag.Name = "tbNewTag"
        Me.tbNewTag.Size = New System.Drawing.Size(100, 22)
        Me.tbNewTag.TabIndex = 24
        '
        'btnAddTag
        '
        Me.btnAddTag.Location = New System.Drawing.Point(726, 67)
        Me.btnAddTag.Name = "btnAddTag"
        Me.btnAddTag.Size = New System.Drawing.Size(107, 23)
        Me.btnAddTag.TabIndex = 25
        Me.btnAddTag.Text = "Add New Tag"
        Me.btnAddTag.UseVisualStyleBackColor = True
        '
        'frmEdit
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(878, 462)
        Me.Controls.Add(Me.btnAddTag)
        Me.Controls.Add(Me.tbNewTag)
        Me.Controls.Add(Me.clbTags)
        Me.Controls.Add(Me.tbModified)
        Me.Controls.Add(Me.tbCreated)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnValidate)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnRevert)
        Me.Controls.Add(Me.btnPriMinus)
        Me.Controls.Add(Me.btnPriPlus)
        Me.Controls.Add(Me.btnPriTop)
        Me.Controls.Add(Me.btnEditBTTD)
        Me.Controls.Add(Me.btnEditDOE)
        Me.Controls.Add(Me.tbPri)
        Me.Controls.Add(Me.tbBTTD)
        Me.Controls.Add(Me.tbDOE)
        Me.Controls.Add(Me.tbNotes)
        Me.Controls.Add(Me.tbText)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmEdit"
        Me.Text = "frmEdit"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents tbText As TextBox
    Friend WithEvents tbNotes As TextBox
    Friend WithEvents tbDOE As TextBox
    Friend WithEvents tbBTTD As TextBox
    Friend WithEvents tbPri As TextBox
    Friend WithEvents btnEditDOE As Button
    Friend WithEvents btnEditBTTD As Button
    Friend WithEvents btnPriTop As Button
    Friend WithEvents btnPriPlus As Button
    Friend WithEvents btnPriMinus As Button
    Friend WithEvents btnRevert As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents btnValidate As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents tbCreated As TextBox
    Friend WithEvents tbModified As TextBox
    Friend WithEvents clbTags As CheckedListBox
    Friend WithEvents tbNewTag As TextBox
    Friend WithEvents btnAddTag As Button
End Class
