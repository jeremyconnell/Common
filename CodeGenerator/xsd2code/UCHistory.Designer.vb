<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCHistory
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
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.chFile = New System.Windows.Forms.ColumnHeader
        Me.chFolder = New System.Windows.Forms.ColumnHeader
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.miRemove = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbByFolder = New System.Windows.Forms.RadioButton
        Me.rbByFileName = New System.Windows.Forms.RadioButton
        Me.rbRecent = New System.Windows.Forms.RadioButton
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chFile, Me.chFolder})
        Me.ListView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(0, 21)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(748, 333)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'chFile
        '
        Me.chFile.Text = "File Name"
        Me.chFile.Width = 120
        '
        'chFolder
        '
        Me.chFolder.Text = "Folder Path"
        Me.chFolder.Width = 582
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miRemove})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(153, 48)
        '
        'miRemove
        '
        Me.miRemove.Name = "miRemove"
        Me.miRemove.Size = New System.Drawing.Size(152, 22)
        Me.miRemove.Text = "Remove"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbByFolder)
        Me.Panel1.Controls.Add(Me.rbByFileName)
        Me.Panel1.Controls.Add(Me.rbRecent)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(748, 21)
        Me.Panel1.TabIndex = 1
        '
        'rbByFolder
        '
        Me.rbByFolder.AutoSize = True
        Me.rbByFolder.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbByFolder.Location = New System.Drawing.Point(142, 0)
        Me.rbByFolder.Name = "rbByFolder"
        Me.rbByFolder.Size = New System.Drawing.Size(69, 21)
        Me.rbByFolder.TabIndex = 2
        Me.rbByFolder.Text = "By Folder"
        Me.rbByFolder.UseVisualStyleBackColor = True
        '
        'rbByFileName
        '
        Me.rbByFileName.AutoSize = True
        Me.rbByFileName.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbByFileName.Location = New System.Drawing.Point(60, 0)
        Me.rbByFileName.Name = "rbByFileName"
        Me.rbByFileName.Size = New System.Drawing.Size(82, 21)
        Me.rbByFileName.TabIndex = 1
        Me.rbByFileName.Text = "By Filename"
        Me.rbByFileName.UseVisualStyleBackColor = True
        '
        'rbRecent
        '
        Me.rbRecent.AutoSize = True
        Me.rbRecent.Checked = True
        Me.rbRecent.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbRecent.Location = New System.Drawing.Point(0, 0)
        Me.rbRecent.Name = "rbRecent"
        Me.rbRecent.Size = New System.Drawing.Size(60, 21)
        Me.rbRecent.TabIndex = 0
        Me.rbRecent.TabStop = True
        Me.rbRecent.Text = "Recent"
        Me.rbRecent.UseVisualStyleBackColor = True
        '
        'UCHistory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "UCHistory"
        Me.Size = New System.Drawing.Size(748, 354)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents chFile As System.Windows.Forms.ColumnHeader
    Friend WithEvents chFolder As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbByFileName As System.Windows.Forms.RadioButton
    Friend WithEvents rbRecent As System.Windows.Forms.RadioButton
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents miRemove As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbByFolder As System.Windows.Forms.RadioButton

End Class
