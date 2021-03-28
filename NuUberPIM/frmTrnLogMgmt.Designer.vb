<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrnLogMgmt
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
        Me.tvCompare = New System.Windows.Forms.TreeView()
        Me.tbReviewLog = New System.Windows.Forms.TextBox()
        Me.btnCheckToThisPoint = New System.Windows.Forms.Button()
        Me.lbTrnSteps = New System.Windows.Forms.ListBox()
        Me.btnSetAsRepair = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'tvCompare
        '
        Me.tvCompare.Location = New System.Drawing.Point(26, 21)
        Me.tvCompare.Name = "tvCompare"
        Me.tvCompare.Size = New System.Drawing.Size(121, 97)
        Me.tvCompare.TabIndex = 0
        Me.tvCompare.Visible = False
        '
        'tbReviewLog
        '
        Me.tbReviewLog.Location = New System.Drawing.Point(26, 318)
        Me.tbReviewLog.Multiline = True
        Me.tbReviewLog.Name = "tbReviewLog"
        Me.tbReviewLog.ReadOnly = True
        Me.tbReviewLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbReviewLog.Size = New System.Drawing.Size(741, 110)
        Me.tbReviewLog.TabIndex = 1
        '
        'btnCheckToThisPoint
        '
        Me.btnCheckToThisPoint.Location = New System.Drawing.Point(364, 21)
        Me.btnCheckToThisPoint.Name = "btnCheckToThisPoint"
        Me.btnCheckToThisPoint.Size = New System.Drawing.Size(196, 23)
        Me.btnCheckToThisPoint.TabIndex = 2
        Me.btnCheckToThisPoint.Text = "Check Current Instance"
        Me.btnCheckToThisPoint.UseVisualStyleBackColor = True
        '
        'lbTrnSteps
        '
        Me.lbTrnSteps.FormattingEnabled = True
        Me.lbTrnSteps.ItemHeight = 16
        Me.lbTrnSteps.Location = New System.Drawing.Point(27, 149)
        Me.lbTrnSteps.Name = "lbTrnSteps"
        Me.lbTrnSteps.Size = New System.Drawing.Size(740, 148)
        Me.lbTrnSteps.TabIndex = 3
        '
        'btnSetAsRepair
        '
        Me.btnSetAsRepair.Location = New System.Drawing.Point(364, 62)
        Me.btnSetAsRepair.Name = "btnSetAsRepair"
        Me.btnSetAsRepair.Size = New System.Drawing.Size(196, 23)
        Me.btnSetAsRepair.TabIndex = 4
        Me.btnSetAsRepair.Text = "Set As Repair"
        Me.btnSetAsRepair.UseVisualStyleBackColor = True
        '
        'frmTrnLogMgmt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnSetAsRepair)
        Me.Controls.Add(Me.lbTrnSteps)
        Me.Controls.Add(Me.btnCheckToThisPoint)
        Me.Controls.Add(Me.tbReviewLog)
        Me.Controls.Add(Me.tvCompare)
        Me.Name = "frmTrnLogMgmt"
        Me.Text = "frmTrnLogMgmt"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tvCompare As TreeView
    Friend WithEvents tbReviewLog As TextBox
    Friend WithEvents btnCheckToThisPoint As Button
    Friend WithEvents lbTrnSteps As ListBox
    Friend WithEvents btnSetAsRepair As Button
End Class
