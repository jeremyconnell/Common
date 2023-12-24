<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCCopyAndPaste
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.txtCopyAndPaste = New System.Windows.Forms.TextBox
        Me.btnCopy = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtCopyAndPaste
        '
        Me.txtCopyAndPaste.BackColor = System.Drawing.Color.LightYellow
        Me.txtCopyAndPaste.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtCopyAndPaste.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCopyAndPaste.Location = New System.Drawing.Point(0, 0)
        Me.txtCopyAndPaste.Multiline = True
        Me.txtCopyAndPaste.Name = "txtCopyAndPaste"
        Me.txtCopyAndPaste.ReadOnly = True
        Me.txtCopyAndPaste.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtCopyAndPaste.Size = New System.Drawing.Size(290, 202)
        Me.txtCopyAndPaste.TabIndex = 3
        Me.txtCopyAndPaste.WordWrap = False
        '
        'btnCopy
        '
        Me.btnCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopy.Location = New System.Drawing.Point(230, 2)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(43, 23)
        Me.btnCopy.TabIndex = 4
        Me.btnCopy.Text = "Copy"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'UCCopyAndPaste
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.txtCopyAndPaste)
        Me.Name = "UCCopyAndPaste"
        Me.Size = New System.Drawing.Size(290, 202)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtCopyAndPaste As System.Windows.Forms.TextBox
    Friend WithEvents btnCopy As System.Windows.Forms.Button

End Class
