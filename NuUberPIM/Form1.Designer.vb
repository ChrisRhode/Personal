<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.tvMain = New System.Windows.Forms.TreeView()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnPriUp = New System.Windows.Forms.Button()
        Me.btnPriDown = New System.Windows.Forms.Button()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.btnStartMultiMoveHere = New System.Windows.Forms.Button()
        Me.btnEndMultiMove = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnTrnLog = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnMiniMode = New System.Windows.Forms.Button()
        Me.btnMove = New System.Windows.Forms.Button()
        Me.btnCheck = New System.Windows.Forms.Button()
        Me.btnMoveUp = New System.Windows.Forms.Button()
        Me.btnMoveDown = New System.Windows.Forms.Button()
        Me.btnToggleNodeType = New System.Windows.Forms.Button()
        Me.btnMoveBelow = New System.Windows.Forms.Button()
        Me.btnMoveToTop = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'tvMain
        '
        Me.tvMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvMain.Location = New System.Drawing.Point(22, 64)
        Me.tvMain.Name = "tvMain"
        Me.tvMain.Size = New System.Drawing.Size(628, 341)
        Me.tvMain.TabIndex = 0
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Location = New System.Drawing.Point(661, 16)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(123, 32)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "Add / Find"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnPriUp
        '
        Me.btnPriUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPriUp.Location = New System.Drawing.Point(661, 64)
        Me.btnPriUp.Name = "btnPriUp"
        Me.btnPriUp.Size = New System.Drawing.Size(75, 32)
        Me.btnPriUp.TabIndex = 2
        Me.btnPriUp.Text = "Priority +"
        Me.btnPriUp.UseVisualStyleBackColor = True
        '
        'btnPriDown
        '
        Me.btnPriDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPriDown.Location = New System.Drawing.Point(742, 64)
        Me.btnPriDown.Name = "btnPriDown"
        Me.btnPriDown.Size = New System.Drawing.Size(75, 32)
        Me.btnPriDown.TabIndex = 3
        Me.btnPriDown.Text = "Priority -"
        Me.btnPriDown.UseVisualStyleBackColor = True
        '
        'tbLog
        '
        Me.tbLog.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLog.Location = New System.Drawing.Point(22, 431)
        Me.tbLog.Multiline = True
        Me.tbLog.Name = "tbLog"
        Me.tbLog.ReadOnly = True
        Me.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLog.Size = New System.Drawing.Size(871, 101)
        Me.tbLog.TabIndex = 4
        '
        'btnStartMultiMoveHere
        '
        Me.btnStartMultiMoveHere.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartMultiMoveHere.Location = New System.Drawing.Point(661, 169)
        Me.btnStartMultiMoveHere.Name = "btnStartMultiMoveHere"
        Me.btnStartMultiMoveHere.Size = New System.Drawing.Size(123, 32)
        Me.btnStartMultiMoveHere.TabIndex = 5
        Me.btnStartMultiMoveHere.Text = "Move Many"
        Me.btnStartMultiMoveHere.UseVisualStyleBackColor = True
        '
        'btnEndMultiMove
        '
        Me.btnEndMultiMove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEndMultiMove.Location = New System.Drawing.Point(785, 169)
        Me.btnEndMultiMove.Name = "btnEndMultiMove"
        Me.btnEndMultiMove.Size = New System.Drawing.Size(123, 32)
        Me.btnEndMultiMove.TabIndex = 6
        Me.btnEndMultiMove.Text = "End Move"
        Me.btnEndMultiMove.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.Location = New System.Drawing.Point(828, 64)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 32)
        Me.btnDelete.TabIndex = 7
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(737, 335)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 32)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnTrnLog
        '
        Me.btnTrnLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTrnLog.Location = New System.Drawing.Point(656, 373)
        Me.btnTrnLog.Name = "btnTrnLog"
        Me.btnTrnLog.Size = New System.Drawing.Size(247, 32)
        Me.btnTrnLog.TabIndex = 9
        Me.btnTrnLog.Text = "TRN Log Functions"
        Me.btnTrnLog.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.Location = New System.Drawing.Point(828, 16)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(75, 32)
        Me.btnEdit.TabIndex = 10
        Me.btnEdit.Text = "Edit"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnMiniMode
        '
        Me.btnMiniMode.Location = New System.Drawing.Point(22, 16)
        Me.btnMiniMode.Name = "btnMiniMode"
        Me.btnMiniMode.Size = New System.Drawing.Size(99, 32)
        Me.btnMiniMode.TabIndex = 11
        Me.btnMiniMode.Text = "Mini-Mode"
        Me.btnMiniMode.UseVisualStyleBackColor = True
        '
        'btnMove
        '
        Me.btnMove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMove.Location = New System.Drawing.Point(661, 131)
        Me.btnMove.Name = "btnMove"
        Me.btnMove.Size = New System.Drawing.Size(75, 32)
        Me.btnMove.TabIndex = 12
        Me.btnMove.Text = "Move"
        Me.btnMove.UseVisualStyleBackColor = True
        '
        'btnCheck
        '
        Me.btnCheck.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCheck.Location = New System.Drawing.Point(656, 335)
        Me.btnCheck.Name = "btnCheck"
        Me.btnCheck.Size = New System.Drawing.Size(75, 32)
        Me.btnCheck.TabIndex = 13
        Me.btnCheck.Text = "Check"
        Me.btnCheck.UseVisualStyleBackColor = True
        '
        'btnMoveUp
        '
        Me.btnMoveUp.Location = New System.Drawing.Point(661, 246)
        Me.btnMoveUp.Name = "btnMoveUp"
        Me.btnMoveUp.Size = New System.Drawing.Size(75, 32)
        Me.btnMoveUp.TabIndex = 14
        Me.btnMoveUp.Text = "Move ^"
        Me.btnMoveUp.UseVisualStyleBackColor = True
        '
        'btnMoveDown
        '
        Me.btnMoveDown.Location = New System.Drawing.Point(744, 247)
        Me.btnMoveDown.Name = "btnMoveDown"
        Me.btnMoveDown.Size = New System.Drawing.Size(75, 32)
        Me.btnMoveDown.TabIndex = 15
        Me.btnMoveDown.Text = "Move v"
        Me.btnMoveDown.UseVisualStyleBackColor = True
        '
        'btnToggleNodeType
        '
        Me.btnToggleNodeType.Location = New System.Drawing.Point(661, 208)
        Me.btnToggleNodeType.Name = "btnToggleNodeType"
        Me.btnToggleNodeType.Size = New System.Drawing.Size(156, 32)
        Me.btnToggleNodeType.TabIndex = 16
        Me.btnToggleNodeType.Text = "Toggle Node Type"
        Me.btnToggleNodeType.UseVisualStyleBackColor = True
        '
        'btnMoveBelow
        '
        Me.btnMoveBelow.Location = New System.Drawing.Point(825, 246)
        Me.btnMoveBelow.Name = "btnMoveBelow"
        Me.btnMoveBelow.Size = New System.Drawing.Size(75, 32)
        Me.btnMoveBelow.TabIndex = 17
        Me.btnMoveBelow.Text = "Move _"
        Me.btnMoveBelow.UseVisualStyleBackColor = True
        '
        'btnMoveToTop
        '
        Me.btnMoveToTop.Location = New System.Drawing.Point(661, 285)
        Me.btnMoveToTop.Name = "btnMoveToTop"
        Me.btnMoveToTop.Size = New System.Drawing.Size(123, 32)
        Me.btnMoveToTop.TabIndex = 18
        Me.btnMoveToTop.Text = "Move To Top"
        Me.btnMoveToTop.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(920, 544)
        Me.Controls.Add(Me.btnMoveToTop)
        Me.Controls.Add(Me.btnMoveBelow)
        Me.Controls.Add(Me.btnToggleNodeType)
        Me.Controls.Add(Me.btnMoveDown)
        Me.Controls.Add(Me.btnMoveUp)
        Me.Controls.Add(Me.btnCheck)
        Me.Controls.Add(Me.btnMove)
        Me.Controls.Add(Me.btnMiniMode)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnTrnLog)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnEndMultiMove)
        Me.Controls.Add(Me.btnStartMultiMoveHere)
        Me.Controls.Add(Me.tbLog)
        Me.Controls.Add(Me.btnPriDown)
        Me.Controls.Add(Me.btnPriUp)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.tvMain)
        Me.Name = "Form1"
        Me.Text = "NuUberPIM"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tvMain As TreeView
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnPriUp As Button
    Friend WithEvents btnPriDown As Button
    Friend WithEvents tbLog As TextBox
    Friend WithEvents btnStartMultiMoveHere As Button
    Friend WithEvents btnEndMultiMove As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnTrnLog As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnMiniMode As Button
    Friend WithEvents btnMove As Button
    Friend WithEvents btnCheck As Button
    Friend WithEvents btnMoveUp As Button
    Friend WithEvents btnMoveDown As Button
    Friend WithEvents btnToggleNodeType As Button
    Friend WithEvents btnMoveBelow As Button
    Friend WithEvents btnMoveToTop As Button
End Class
