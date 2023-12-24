<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCXsdClassGenerator
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.pnlInput = New System.Windows.Forms.Panel
        Me.txtFilePath = New System.Windows.Forms.TextBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.txtDotNetNamespace = New System.Windows.Forms.TextBox
        Me.btnGenerate = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.rbCSharp = New System.Windows.Forms.RadioButton
        Me.rbVB = New System.Windows.Forms.RadioButton
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.txtOutputPath = New System.Windows.Forms.TextBox
        Me.btnBrowseFolder = New System.Windows.Forms.Button
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.UcHistory1 = New CodeGenerator.UCHistory
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.GroupBox1.SuspendLayout()
        Me.pnlInput.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pnlInput)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(484, 39)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Input Schema File (*.xsd)"
        '
        'pnlInput
        '
        Me.pnlInput.Controls.Add(Me.txtFilePath)
        Me.pnlInput.Controls.Add(Me.btnBrowse)
        Me.pnlInput.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlInput.Location = New System.Drawing.Point(3, 16)
        Me.pnlInput.Name = "pnlInput"
        Me.pnlInput.Size = New System.Drawing.Size(478, 23)
        Me.pnlInput.TabIndex = 3
        '
        'txtFilePath
        '
        Me.txtFilePath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFilePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFilePath.Location = New System.Drawing.Point(0, 0)
        Me.txtFilePath.Name = "txtFilePath"
        Me.txtFilePath.Size = New System.Drawing.Size(415, 18)
        Me.txtFilePath.TabIndex = 1
        '
        'btnBrowse
        '
        Me.btnBrowse.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnBrowse.Location = New System.Drawing.Point(415, 0)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(63, 23)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "Browse..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Panel1)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox2.Location = New System.Drawing.Point(0, 78)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(484, 39)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Language"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.rbCSharp)
        Me.Panel1.Controls.Add(Me.rbVB)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 16)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(478, 23)
        Me.Panel1.TabIndex = 7
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.txtDotNetNamespace)
        Me.Panel2.Controls.Add(Me.btnGenerate)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(98, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(380, 23)
        Me.Panel2.TabIndex = 5
        '
        'txtDotNetNamespace
        '
        Me.txtDotNetNamespace.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDotNetNamespace.Location = New System.Drawing.Point(80, 0)
        Me.txtDotNetNamespace.Name = "txtDotNetNamespace"
        Me.txtDotNetNamespace.Size = New System.Drawing.Size(237, 20)
        Me.txtDotNetNamespace.TabIndex = 4
        Me.txtDotNetNamespace.Text = "XmlNorthWind"
        '
        'btnGenerate
        '
        Me.btnGenerate.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnGenerate.Location = New System.Drawing.Point(317, 0)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(63, 23)
        Me.btnGenerate.TabIndex = 5
        Me.btnGenerate.Text = "Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label3.Location = New System.Drawing.Point(10, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 23)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Namespace"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label5.Location = New System.Drawing.Point(0, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(10, 23)
        Me.Label5.TabIndex = 0
        '
        'rbCSharp
        '
        Me.rbCSharp.AutoSize = True
        Me.rbCSharp.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbCSharp.Location = New System.Drawing.Point(59, 0)
        Me.rbCSharp.Name = "rbCSharp"
        Me.rbCSharp.Size = New System.Drawing.Size(39, 23)
        Me.rbCSharp.TabIndex = 4
        Me.rbCSharp.Text = "C#"
        Me.rbCSharp.UseVisualStyleBackColor = True
        '
        'rbVB
        '
        Me.rbVB.AutoSize = True
        Me.rbVB.Checked = True
        Me.rbVB.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbVB.Location = New System.Drawing.Point(0, 0)
        Me.rbVB.Name = "rbVB"
        Me.rbVB.Size = New System.Drawing.Size(59, 23)
        Me.rbVB.TabIndex = 3
        Me.rbVB.TabStop = True
        Me.rbVB.Text = "VB.Net"
        Me.rbVB.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtOutputPath)
        Me.GroupBox3.Controls.Add(Me.btnBrowseFolder)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox3.Location = New System.Drawing.Point(0, 39)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(484, 39)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Output Folder"
        '
        'txtOutputPath
        '
        Me.txtOutputPath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtOutputPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOutputPath.Location = New System.Drawing.Point(3, 16)
        Me.txtOutputPath.Name = "txtOutputPath"
        Me.txtOutputPath.Size = New System.Drawing.Size(415, 18)
        Me.txtOutputPath.TabIndex = 3
        '
        'btnBrowseFolder
        '
        Me.btnBrowseFolder.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnBrowseFolder.Location = New System.Drawing.Point(418, 16)
        Me.btnBrowseFolder.Name = "btnBrowseFolder"
        Me.btnBrowseFolder.Size = New System.Drawing.Size(63, 20)
        Me.btnBrowseFolder.TabIndex = 4
        Me.btnBrowseFolder.Text = "Browse..."
        Me.btnBrowseFolder.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.UcHistory1)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox4.Location = New System.Drawing.Point(0, 117)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(484, 168)
        Me.GroupBox4.TabIndex = 3
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "History"
        '
        'UcHistory1
        '
        Me.UcHistory1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcHistory1.Location = New System.Drawing.Point(3, 16)
        Me.UcHistory1.Name = "UcHistory1"
        Me.UcHistory1.Size = New System.Drawing.Size(478, 149)
        Me.UcHistory1.TabIndex = 0
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.DefaultExt = "xsd"
        Me.OpenFileDialog1.Filter = "Schema Files (*.xsd)|*.xsd"
        Me.OpenFileDialog1.RestoreDirectory = True
        Me.OpenFileDialog1.SupportMultiDottedExtensions = True
        '
        'UCXsdClassGenerator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "UCXsdClassGenerator"
        Me.Size = New System.Drawing.Size(484, 285)
        Me.GroupBox1.ResumeLayout(False)
        Me.pnlInput.ResumeLayout(False)
        Me.pnlInput.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents pnlInput As System.Windows.Forms.Panel
    Friend WithEvents txtFilePath As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents txtDotNetNamespace As System.Windows.Forms.TextBox
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents rbCSharp As System.Windows.Forms.RadioButton
    Friend WithEvents rbVB As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtOutputPath As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseFolder As System.Windows.Forms.Button
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents UcHistory1 As CodeGenerator.UCHistory

End Class
