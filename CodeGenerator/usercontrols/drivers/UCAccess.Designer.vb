<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCAccess
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
        Me.components = New System.ComponentModel.Container
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lvRecent = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.miDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbName = New System.Windows.Forms.RadioButton
        Me.rbRecent = New System.Windows.Forms.RadioButton
        Me.gbAccess = New System.Windows.Forms.GroupBox
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.btnTest = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.GroupBox1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.gbAccess.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lvRecent)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 41)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(274, 178)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Recent"
        '
        'lvRecent
        '
        Me.lvRecent.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvRecent.ContextMenuStrip = Me.ContextMenuStrip1
        Me.lvRecent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvRecent.FullRowSelect = True
        Me.lvRecent.GridLines = True
        Me.lvRecent.Location = New System.Drawing.Point(3, 33)
        Me.lvRecent.Name = "lvRecent"
        Me.lvRecent.Size = New System.Drawing.Size(268, 142)
        Me.lvRecent.TabIndex = 0
        Me.lvRecent.UseCompatibleStateImageBehavior = False
        Me.lvRecent.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Filename"
        Me.ColumnHeader1.Width = 120
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Folder"
        Me.ColumnHeader2.Width = 400
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miDelete})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(106, 26)
        '
        'miDelete
        '
        Me.miDelete.Name = "miDelete"
        Me.miDelete.Size = New System.Drawing.Size(105, 22)
        Me.miDelete.Text = "Delete"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbName)
        Me.Panel1.Controls.Add(Me.rbRecent)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 16)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(268, 17)
        Me.Panel1.TabIndex = 5
        '
        'rbName
        '
        Me.rbName.AutoSize = True
        Me.rbName.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbName.Location = New System.Drawing.Point(123, 0)
        Me.rbName.Name = "rbName"
        Me.rbName.Size = New System.Drawing.Size(90, 17)
        Me.rbName.TabIndex = 5
        Me.rbName.Text = "Sort By Name"
        Me.rbName.UseVisualStyleBackColor = True
        '
        'rbRecent
        '
        Me.rbRecent.AutoSize = True
        Me.rbRecent.Checked = True
        Me.rbRecent.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbRecent.Location = New System.Drawing.Point(0, 0)
        Me.rbRecent.Name = "rbRecent"
        Me.rbRecent.Size = New System.Drawing.Size(123, 17)
        Me.rbRecent.TabIndex = 4
        Me.rbRecent.TabStop = True
        Me.rbRecent.Text = "Sort By Most Recent"
        Me.rbRecent.UseVisualStyleBackColor = True
        '
        'gbAccess
        '
        Me.gbAccess.Controls.Add(Me.txtPath)
        Me.gbAccess.Controls.Add(Me.btnBrowse)
        Me.gbAccess.Controls.Add(Me.btnTest)
        Me.gbAccess.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbAccess.Location = New System.Drawing.Point(0, 0)
        Me.gbAccess.Name = "gbAccess"
        Me.gbAccess.Size = New System.Drawing.Size(274, 41)
        Me.gbAccess.TabIndex = 2
        Me.gbAccess.TabStop = False
        Me.gbAccess.Text = "File Location"
        '
        'txtPath
        '
        Me.txtPath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPath.Location = New System.Drawing.Point(3, 16)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(93, 18)
        Me.txtPath.TabIndex = 3
        '
        'btnBrowse
        '
        Me.btnBrowse.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnBrowse.Location = New System.Drawing.Point(96, 16)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(59, 22)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "Browse..."
        '
        'btnTest
        '
        Me.btnTest.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnTest.Location = New System.Drawing.Point(155, 16)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(116, 22)
        Me.btnTest.TabIndex = 4
        Me.btnTest.Text = "Test Connection"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "MSAccess files (*.mdb,*.accdb)|*.mdb;*.accdb|All files|*.*"
        Me.OpenFileDialog1.Title = "Select a MS Access Database"
        '
        'UCAccess
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbAccess)
        Me.Name = "UCAccess"
        Me.Size = New System.Drawing.Size(274, 219)
        Me.GroupBox1.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.gbAccess.ResumeLayout(False)
        Me.gbAccess.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lvRecent As System.Windows.Forms.ListView
    Friend WithEvents gbAccess As System.Windows.Forms.GroupBox
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbName As System.Windows.Forms.RadioButton
    Friend WithEvents rbRecent As System.Windows.Forms.RadioButton
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents miDelete As System.Windows.Forms.ToolStripMenuItem

End Class
