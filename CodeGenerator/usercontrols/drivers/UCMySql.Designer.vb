<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCMySql
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
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lvRecent = New System.Windows.Forms.ListView
        Me.chServer = New System.Windows.Forms.ColumnHeader
        Me.chPort = New System.Windows.Forms.ColumnHeader
        Me.chUser = New System.Windows.Forms.ColumnHeader
        Me.chPassword = New System.Windows.Forms.ColumnHeader
        Me.chDatabase = New System.Windows.Forms.ColumnHeader
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbName = New System.Windows.Forms.RadioButton
        Me.rbRecent = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.btnTest = New System.Windows.Forms.Button
        Me.pnlDetails = New System.Windows.Forms.Panel
        Me.txtDatabase = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtUser = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtServer = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.miDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlDetails.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lvRecent)
        Me.GroupBox2.Controls.Add(Me.Panel1)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 253)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(470, 148)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Recent"
        '
        'lvRecent
        '
        Me.lvRecent.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chServer, Me.chPort, Me.chUser, Me.chPassword, Me.chDatabase})
        Me.lvRecent.ContextMenuStrip = Me.ContextMenuStrip1
        Me.lvRecent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvRecent.FullRowSelect = True
        Me.lvRecent.GridLines = True
        Me.lvRecent.Location = New System.Drawing.Point(3, 33)
        Me.lvRecent.Name = "lvRecent"
        Me.lvRecent.Size = New System.Drawing.Size(464, 112)
        Me.lvRecent.TabIndex = 0
        Me.lvRecent.UseCompatibleStateImageBehavior = False
        Me.lvRecent.View = System.Windows.Forms.View.Details
        '
        'chServer
        '
        Me.chServer.Text = "Server"
        Me.chServer.Width = 95
        '
        'chPort
        '
        Me.chPort.Text = "Port"
        '
        'chUser
        '
        Me.chUser.Text = "User"
        Me.chUser.Width = 90
        '
        'chPassword
        '
        Me.chPassword.Text = "Password"
        Me.chPassword.Width = 90
        '
        'chDatabase
        '
        Me.chDatabase.Text = "Database"
        Me.chDatabase.Width = 176
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbName)
        Me.Panel1.Controls.Add(Me.rbRecent)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 16)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(464, 17)
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
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Panel2)
        Me.GroupBox1.Controls.Add(Me.pnlDetails)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(470, 253)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Connection Details"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnTest)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(3, 226)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(464, 24)
        Me.Panel2.TabIndex = 17
        '
        'btnTest
        '
        Me.btnTest.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnTest.Location = New System.Drawing.Point(296, 0)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(168, 24)
        Me.btnTest.TabIndex = 6
        Me.btnTest.Text = "Test Connection"
        '
        'pnlDetails
        '
        Me.pnlDetails.Controls.Add(Me.txtDatabase)
        Me.pnlDetails.Controls.Add(Me.Label4)
        Me.pnlDetails.Controls.Add(Me.txtPort)
        Me.pnlDetails.Controls.Add(Me.Label1)
        Me.pnlDetails.Controls.Add(Me.txtPassword)
        Me.pnlDetails.Controls.Add(Me.Label5)
        Me.pnlDetails.Controls.Add(Me.txtUser)
        Me.pnlDetails.Controls.Add(Me.Label6)
        Me.pnlDetails.Controls.Add(Me.txtServer)
        Me.pnlDetails.Controls.Add(Me.Label3)
        Me.pnlDetails.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlDetails.Location = New System.Drawing.Point(3, 16)
        Me.pnlDetails.Name = "pnlDetails"
        Me.pnlDetails.Size = New System.Drawing.Size(464, 204)
        Me.pnlDetails.TabIndex = 15
        '
        'txtDatabase
        '
        Me.txtDatabase.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtDatabase.Location = New System.Drawing.Point(0, 186)
        Me.txtDatabase.Name = "txtDatabase"
        Me.txtDatabase.Size = New System.Drawing.Size(464, 20)
        Me.txtDatabase.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Location = New System.Drawing.Point(0, 162)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(464, 24)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Default Schema:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txtPort
        '
        Me.txtPort.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtPort.Location = New System.Drawing.Point(0, 142)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.txtPort.Size = New System.Drawing.Size(464, 20)
        Me.txtPort.TabIndex = 4
        Me.txtPort.Text = "3306"
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(0, 122)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(464, 20)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Port"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txtPassword
        '
        Me.txtPassword.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtPassword.Location = New System.Drawing.Point(0, 102)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.txtPassword.Size = New System.Drawing.Size(464, 20)
        Me.txtPassword.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label5.Location = New System.Drawing.Point(0, 82)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(464, 20)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Password"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txtUser
        '
        Me.txtUser.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtUser.Location = New System.Drawing.Point(0, 62)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(464, 20)
        Me.txtUser.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label6.Location = New System.Drawing.Point(0, 44)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(464, 18)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Username:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txtServer
        '
        Me.txtServer.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtServer.Location = New System.Drawing.Point(0, 24)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(464, 20)
        Me.txtServer.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(464, 24)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Server Host:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft
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
        'UCMySql
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "UCMySql"
        Me.Size = New System.Drawing.Size(470, 401)
        Me.GroupBox2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.pnlDetails.ResumeLayout(False)
        Me.pnlDetails.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lvRecent As System.Windows.Forms.ListView
    Friend WithEvents chServer As System.Windows.Forms.ColumnHeader
    Friend WithEvents chDatabase As System.Windows.Forms.ColumnHeader
    Friend WithEvents chUser As System.Windows.Forms.ColumnHeader
    Friend WithEvents chPassword As System.Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents pnlDetails As System.Windows.Forms.Panel
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtUser As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtDatabase As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbName As System.Windows.Forms.RadioButton
    Friend WithEvents rbRecent As System.Windows.Forms.RadioButton
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chPort As System.Windows.Forms.ColumnHeader
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents miDelete As System.Windows.Forms.ToolStripMenuItem

End Class
