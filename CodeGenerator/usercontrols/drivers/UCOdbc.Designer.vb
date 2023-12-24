<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCOdbc
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
        Me.gbAccess = New System.Windows.Forms.GroupBox
        Me.txtConnectionString = New System.Windows.Forms.TextBox
        Me.btnTest = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.miDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox1.SuspendLayout()
        Me.gbAccess.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lvRecent)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 41)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(451, 304)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Recent"
        '
        'lvRecent
        '
        Me.lvRecent.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lvRecent.ContextMenuStrip = Me.ContextMenuStrip1
        Me.lvRecent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvRecent.FullRowSelect = True
        Me.lvRecent.GridLines = True
        Me.lvRecent.Location = New System.Drawing.Point(3, 16)
        Me.lvRecent.Name = "lvRecent"
        Me.lvRecent.Size = New System.Drawing.Size(445, 285)
        Me.lvRecent.TabIndex = 0
        Me.lvRecent.UseCompatibleStateImageBehavior = False
        Me.lvRecent.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Connection String (ODBC)"
        Me.ColumnHeader1.Width = 400
        '
        'gbAccess
        '
        Me.gbAccess.Controls.Add(Me.txtConnectionString)
        Me.gbAccess.Controls.Add(Me.btnTest)
        Me.gbAccess.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbAccess.Location = New System.Drawing.Point(0, 0)
        Me.gbAccess.Name = "gbAccess"
        Me.gbAccess.Size = New System.Drawing.Size(451, 41)
        Me.gbAccess.TabIndex = 6
        Me.gbAccess.TabStop = False
        Me.gbAccess.Text = "Connection String (ODBC)"
        '
        'txtConnectionString
        '
        Me.txtConnectionString.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtConnectionString.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConnectionString.Location = New System.Drawing.Point(3, 16)
        Me.txtConnectionString.Name = "txtConnectionString"
        Me.txtConnectionString.Size = New System.Drawing.Size(406, 18)
        Me.txtConnectionString.TabIndex = 3
        '
        'btnTest
        '
        Me.btnTest.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnTest.Location = New System.Drawing.Point(409, 16)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(39, 22)
        Me.btnTest.TabIndex = 4
        Me.btnTest.Text = "Test"
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
        'UCOdbc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbAccess)
        Me.Name = "UCOdbc"
        Me.Size = New System.Drawing.Size(451, 345)
        Me.GroupBox1.ResumeLayout(False)
        Me.gbAccess.ResumeLayout(False)
        Me.gbAccess.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lvRecent As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents gbAccess As System.Windows.Forms.GroupBox
    Friend WithEvents txtConnectionString As System.Windows.Forms.TextBox
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents miDelete As System.Windows.Forms.ToolStripMenuItem

End Class
