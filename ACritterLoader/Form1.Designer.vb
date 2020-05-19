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
        Me.btnRawLoad = New System.Windows.Forms.Button()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnRawLoad
        '
        Me.btnRawLoad.Location = New System.Drawing.Point(23, 190)
        Me.btnRawLoad.Name = "btnRawLoad"
        Me.btnRawLoad.Size = New System.Drawing.Size(197, 23)
        Me.btnRawLoad.TabIndex = 0
        Me.btnRawLoad.Text = "Load all from file"
        Me.btnRawLoad.UseVisualStyleBackColor = True
        '
        'lblMessage
        '
        Me.lblMessage.Location = New System.Drawing.Point(20, 31)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(746, 23)
        Me.lblMessage.TabIndex = 1
        '
        'tbLog
        '
        Me.tbLog.Location = New System.Drawing.Point(23, 80)
        Me.tbLog.Multiline = True
        Me.tbLog.Name = "tbLog"
        Me.tbLog.ReadOnly = True
        Me.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLog.Size = New System.Drawing.Size(730, 91)
        Me.tbLog.TabIndex = 2
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.tbLog)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnRawLoad)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnRawLoad As Button
    Friend WithEvents lblMessage As Label
    Friend WithEvents tbLog As TextBox
End Class
