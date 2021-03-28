<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditDate
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
        Me.tbDate = New System.Windows.Forms.TextBox()
        Me.btnToday = New System.Windows.Forms.Button()
        Me.btnTomorrow = New System.Windows.Forms.Button()
        Me.btnNextMonday = New System.Windows.Forms.Button()
        Me.btnPlusOne = New System.Windows.Forms.Button()
        Me.btnMinusOne = New System.Windows.Forms.Button()
        Me.btnPlus7 = New System.Windows.Forms.Button()
        Me.btnMinus7 = New System.Windows.Forms.Button()
        Me.btnValidate = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'tbDate
        '
        Me.tbDate.Location = New System.Drawing.Point(23, 28)
        Me.tbDate.Name = "tbDate"
        Me.tbDate.Size = New System.Drawing.Size(175, 22)
        Me.tbDate.TabIndex = 0
        '
        'btnToday
        '
        Me.btnToday.Location = New System.Drawing.Point(23, 72)
        Me.btnToday.Name = "btnToday"
        Me.btnToday.Size = New System.Drawing.Size(75, 32)
        Me.btnToday.TabIndex = 1
        Me.btnToday.Text = "Today"
        Me.btnToday.UseVisualStyleBackColor = True
        '
        'btnTomorrow
        '
        Me.btnTomorrow.Location = New System.Drawing.Point(23, 110)
        Me.btnTomorrow.Name = "btnTomorrow"
        Me.btnTomorrow.Size = New System.Drawing.Size(92, 32)
        Me.btnTomorrow.TabIndex = 2
        Me.btnTomorrow.Text = "Tomorrow"
        Me.btnTomorrow.UseVisualStyleBackColor = True
        '
        'btnNextMonday
        '
        Me.btnNextMonday.Location = New System.Drawing.Point(23, 149)
        Me.btnNextMonday.Name = "btnNextMonday"
        Me.btnNextMonday.Size = New System.Drawing.Size(124, 32)
        Me.btnNextMonday.TabIndex = 3
        Me.btnNextMonday.Text = "Next Monday"
        Me.btnNextMonday.UseVisualStyleBackColor = True
        '
        'btnPlusOne
        '
        Me.btnPlusOne.Location = New System.Drawing.Point(23, 188)
        Me.btnPlusOne.Name = "btnPlusOne"
        Me.btnPlusOne.Size = New System.Drawing.Size(75, 32)
        Me.btnPlusOne.TabIndex = 4
        Me.btnPlusOne.Text = "+ 1d"
        Me.btnPlusOne.UseVisualStyleBackColor = True
        '
        'btnMinusOne
        '
        Me.btnMinusOne.Location = New System.Drawing.Point(123, 188)
        Me.btnMinusOne.Name = "btnMinusOne"
        Me.btnMinusOne.Size = New System.Drawing.Size(75, 32)
        Me.btnMinusOne.TabIndex = 5
        Me.btnMinusOne.Text = "- 1d"
        Me.btnMinusOne.UseVisualStyleBackColor = True
        '
        'btnPlus7
        '
        Me.btnPlus7.Location = New System.Drawing.Point(23, 231)
        Me.btnPlus7.Name = "btnPlus7"
        Me.btnPlus7.Size = New System.Drawing.Size(75, 32)
        Me.btnPlus7.TabIndex = 6
        Me.btnPlus7.Text = "+ 7d"
        Me.btnPlus7.UseVisualStyleBackColor = True
        '
        'btnMinus7
        '
        Me.btnMinus7.Location = New System.Drawing.Point(123, 231)
        Me.btnMinus7.Name = "btnMinus7"
        Me.btnMinus7.Size = New System.Drawing.Size(75, 32)
        Me.btnMinus7.TabIndex = 7
        Me.btnMinus7.Text = "- 7d"
        Me.btnMinus7.UseVisualStyleBackColor = True
        '
        'btnValidate
        '
        Me.btnValidate.Location = New System.Drawing.Point(502, 397)
        Me.btnValidate.Name = "btnValidate"
        Me.btnValidate.Size = New System.Drawing.Size(75, 32)
        Me.btnValidate.TabIndex = 8
        Me.btnValidate.Text = "Validate"
        Me.btnValidate.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(583, 397)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 32)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(674, 397)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 32)
        Me.btnOK.TabIndex = 10
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'frmEditDate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnValidate)
        Me.Controls.Add(Me.btnMinus7)
        Me.Controls.Add(Me.btnPlus7)
        Me.Controls.Add(Me.btnMinusOne)
        Me.Controls.Add(Me.btnPlusOne)
        Me.Controls.Add(Me.btnNextMonday)
        Me.Controls.Add(Me.btnTomorrow)
        Me.Controls.Add(Me.btnToday)
        Me.Controls.Add(Me.tbDate)
        Me.Name = "frmEditDate"
        Me.Text = "frmEditDate"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbDate As TextBox
    Friend WithEvents btnToday As Button
    Friend WithEvents btnTomorrow As Button
    Friend WithEvents btnNextMonday As Button
    Friend WithEvents btnPlusOne As Button
    Friend WithEvents btnMinusOne As Button
    Friend WithEvents btnPlus7 As Button
    Friend WithEvents btnMinus7 As Button
    Friend WithEvents btnValidate As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnOK As Button
End Class
