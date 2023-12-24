<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCOracle
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
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.btnTest = New System.Windows.Forms.Button
        Me.pnlDetails = New System.Windows.Forms.Panel
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtUser = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtServer = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lvRecent = New System.Windows.Forms.ListView
        Me.chServer = New System.Windows.Forms.ColumnHeader
        Me.chUser = New System.Windows.Forms.ColumnHeader
        Me.chPassword = New System.Windows.Forms.ColumnHeader
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.miDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlDetails.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Panel2)
        Me.GroupBox1.Controls.Add(Me.pnlDetails)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(457, 181)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Connection Details"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnTest)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(3, 148)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(451, 24)
        Me.Panel2.TabIndex = 17
        '
        'btnTest
        '
        Me.btnTest.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnTest.Location = New System.Drawing.Point(283, 0)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(168, 24)
        Me.btnTest.TabIndex = 11
        Me.btnTest.Text = "Test Connection"
        '
        'pnlDetails
        '
        Me.pnlDetails.Controls.Add(Me.txtPassword)
        Me.pnlDetails.Controls.Add(Me.Label5)
        Me.pnlDetails.Controls.Add(Me.txtUser)
        Me.pnlDetails.Controls.Add(Me.Label6)
        Me.pnlDetails.Controls.Add(Me.txtServer)
        Me.pnlDetails.Controls.Add(Me.Label3)
        Me.pnlDetails.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlDetails.Location = New System.Drawing.Point(3, 16)
        Me.pnlDetails.Name = "pnlDetails"
        Me.pnlDetails.Size = New System.Drawing.Size(451, 132)
        Me.pnlDetails.TabIndex = 15
        '
        'txtPassword
        '
        Me.txtPassword.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtPassword.Location = New System.Drawing.Point(0, 102)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.txtPassword.Size = New System.Drawing.Size(451, 20)
        Me.txtPassword.TabIndex = 8
        '
        'Label5
        '
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label5.Location = New System.Drawing.Point(0, 82)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(451, 20)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Password"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txtUser
        '
        Me.txtUser.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtUser.Location = New System.Drawing.Point(0, 62)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(451, 20)
        Me.txtUser.TabIndex = 6
        '
        'Label6
        '
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label6.Location = New System.Drawing.Point(0, 44)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(451, 18)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "User:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txtServer
        '
        Me.txtServer.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtServer.Location = New System.Drawing.Point(0, 24)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(451, 20)
        Me.txtServer.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(451, 24)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Server:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lvRecent)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 181)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(457, 227)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Recent"
        '
        'lvRecent
        '
        Me.lvRecent.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chServer, Me.chUser, Me.chPassword})
        Me.lvRecent.ContextMenuStrip = Me.ContextMenuStrip1
        Me.lvRecent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvRecent.FullRowSelect = True
        Me.lvRecent.GridLines = True
        Me.lvRecent.Location = New System.Drawing.Point(3, 16)
        Me.lvRecent.Name = "lvRecent"
        Me.lvRecent.Size = New System.Drawing.Size(451, 208)
        Me.lvRecent.TabIndex = 0
        Me.lvRecent.UseCompatibleStateImageBehavior = False
        Me.lvRecent.View = System.Windows.Forms.View.Details
        '
        'chServer
        '
        Me.chServer.Text = "Server"
        Me.chServer.Width = 137
        '
        'chUser
        '
        Me.chUser.Text = "User"
        Me.chUser.Width = 112
        '
        'chPassword
        '
        Me.chPassword.Text = "Password"
        Me.chPassword.Width = 106
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
        'UCOracle
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "UCOracle"
        Me.Size = New System.Drawing.Size(457, 408)
        Me.GroupBox1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.pnlDetails.ResumeLayout(False)
        Me.pnlDetails.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents pnlDetails As System.Windows.Forms.Panel
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtUser As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lvRecent As System.Windows.Forms.ListView
    Friend WithEvents chServer As System.Windows.Forms.ColumnHeader
    Friend WithEvents chUser As System.Windows.Forms.ColumnHeader
    Friend WithEvents chPassword As System.Windows.Forms.ColumnHeader
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents miDelete As System.Windows.Forms.ToolStripMenuItem

End Class
