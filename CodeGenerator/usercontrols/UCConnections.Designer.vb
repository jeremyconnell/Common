<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCConnections
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
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabSqlServer = New System.Windows.Forms.TabPage
        Me.UcSqlServer1 = New CodeGenerator.UCSqlServer
        Me.tabMsAccess = New System.Windows.Forms.TabPage
        Me.UcAccess1 = New CodeGenerator.UCAccess
        Me.tabMySql = New System.Windows.Forms.TabPage
        Me.UcMySql1 = New CodeGenerator.UCMySql
        Me.tabOracle = New System.Windows.Forms.TabPage
        Me.UcOracle1 = New CodeGenerator.UCOracle
        Me.tabOleDb = New System.Windows.Forms.TabPage
        Me.UcOleDb1 = New CodeGenerator.UCOleDb
        Me.tabOdbc = New System.Windows.Forms.TabPage
        Me.UcOdbc1 = New CodeGenerator.UCOdbc
        Me.tabMsExcel = New System.Windows.Forms.TabPage
        Me.UcExcel1 = New CodeGenerator.UCExcel
        Me.tabTextFile = New System.Windows.Forms.TabPage
        Me.UcTextFile1 = New CodeGenerator.UCTextFile
        Me.TabControl1.SuspendLayout()
        Me.tabSqlServer.SuspendLayout()
        Me.tabMsAccess.SuspendLayout()
        Me.tabMySql.SuspendLayout()
        Me.tabOracle.SuspendLayout()
        Me.tabOleDb.SuspendLayout()
        Me.tabOdbc.SuspendLayout()
        Me.tabMsExcel.SuspendLayout()
        Me.tabTextFile.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabMsAccess)
        Me.TabControl1.Controls.Add(Me.tabSqlServer)
        Me.TabControl1.Controls.Add(Me.tabMySql)
        Me.TabControl1.Controls.Add(Me.tabOracle)
        Me.TabControl1.Controls.Add(Me.tabOleDb)
        Me.TabControl1.Controls.Add(Me.tabOdbc)
        Me.TabControl1.Controls.Add(Me.tabMsExcel)
        Me.TabControl1.Controls.Add(Me.tabTextFile)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(441, 301)
        Me.TabControl1.TabIndex = 0
        '
        'tabSqlServer
        '
        Me.tabSqlServer.Controls.Add(Me.UcSqlServer1)
        Me.tabSqlServer.Location = New System.Drawing.Point(4, 22)
        Me.tabSqlServer.Name = "tabSqlServer"
        Me.tabSqlServer.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSqlServer.Size = New System.Drawing.Size(433, 275)
        Me.tabSqlServer.TabIndex = 1
        Me.tabSqlServer.Text = "Sql Server"
        Me.tabSqlServer.UseVisualStyleBackColor = True
        '
        'UcSqlServer1
        '
        Me.UcSqlServer1.Database = "Database"
        Me.UcSqlServer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcSqlServer1.Location = New System.Drawing.Point(3, 3)
        Me.UcSqlServer1.Name = "UcSqlServer1"
        Me.UcSqlServer1.Password = ""
        Me.UcSqlServer1.Server = "."
        Me.UcSqlServer1.Size = New System.Drawing.Size(427, 269)
        Me.UcSqlServer1.TabIndex = 0
        Me.UcSqlServer1.User = ""
        Me.UcSqlServer1.WindowsAuthentication = True
        '
        'tabMsAccess
        '
        Me.tabMsAccess.Controls.Add(Me.UcAccess1)
        Me.tabMsAccess.Location = New System.Drawing.Point(4, 22)
        Me.tabMsAccess.Name = "tabMsAccess"
        Me.tabMsAccess.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMsAccess.Size = New System.Drawing.Size(433, 275)
        Me.tabMsAccess.TabIndex = 0
        Me.tabMsAccess.Text = "Access"
        Me.tabMsAccess.UseVisualStyleBackColor = True
        '
        'UcAccess1
        '
        Me.UcAccess1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcAccess1.FilePath = ""
        Me.UcAccess1.Location = New System.Drawing.Point(3, 3)
        Me.UcAccess1.Name = "UcAccess1"
        Me.UcAccess1.Size = New System.Drawing.Size(427, 269)
        Me.UcAccess1.TabIndex = 0
        '
        'tabMySql
        '
        Me.tabMySql.Controls.Add(Me.UcMySql1)
        Me.tabMySql.Location = New System.Drawing.Point(4, 22)
        Me.tabMySql.Name = "tabMySql"
        Me.tabMySql.Size = New System.Drawing.Size(433, 275)
        Me.tabMySql.TabIndex = 2
        Me.tabMySql.Text = "MySql"
        Me.tabMySql.UseVisualStyleBackColor = True
        '
        'UcMySql1
        '
        Me.UcMySql1.Database = ""
        Me.UcMySql1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcMySql1.Location = New System.Drawing.Point(0, 0)
        Me.UcMySql1.Name = "UcMySql1"
        Me.UcMySql1.Password = ""
        Me.UcMySql1.Port = 3306
        Me.UcMySql1.Server = ""
        Me.UcMySql1.Size = New System.Drawing.Size(433, 275)
        Me.UcMySql1.TabIndex = 0
        Me.UcMySql1.User = ""
        '
        'tabOracle
        '
        Me.tabOracle.Controls.Add(Me.UcOracle1)
        Me.tabOracle.Location = New System.Drawing.Point(4, 22)
        Me.tabOracle.Name = "tabOracle"
        Me.tabOracle.Size = New System.Drawing.Size(433, 275)
        Me.tabOracle.TabIndex = 5
        Me.tabOracle.Text = "Oracle"
        Me.tabOracle.UseVisualStyleBackColor = True
        '
        'UcOracle1
        '
        Me.UcOracle1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcOracle1.Location = New System.Drawing.Point(0, 0)
        Me.UcOracle1.Name = "UcOracle1"
        Me.UcOracle1.Password = ""
        Me.UcOracle1.Server = ""
        Me.UcOracle1.Size = New System.Drawing.Size(433, 275)
        Me.UcOracle1.TabIndex = 0
        Me.UcOracle1.User = ""
        '
        'tabOleDb
        '
        Me.tabOleDb.Controls.Add(Me.UcOleDb1)
        Me.tabOleDb.Location = New System.Drawing.Point(4, 22)
        Me.tabOleDb.Name = "tabOleDb"
        Me.tabOleDb.Size = New System.Drawing.Size(433, 275)
        Me.tabOleDb.TabIndex = 3
        Me.tabOleDb.Text = "OleDb"
        Me.tabOleDb.UseVisualStyleBackColor = True
        '
        'UcOleDb1
        '
        Me.UcOleDb1.ConnectionString = ""
        Me.UcOleDb1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcOleDb1.Location = New System.Drawing.Point(0, 0)
        Me.UcOleDb1.Name = "UcOleDb1"
        Me.UcOleDb1.Size = New System.Drawing.Size(433, 275)
        Me.UcOleDb1.TabIndex = 0
        '
        'tabOdbc
        '
        Me.tabOdbc.Controls.Add(Me.UcOdbc1)
        Me.tabOdbc.Location = New System.Drawing.Point(4, 22)
        Me.tabOdbc.Name = "tabOdbc"
        Me.tabOdbc.Size = New System.Drawing.Size(433, 275)
        Me.tabOdbc.TabIndex = 4
        Me.tabOdbc.Text = "Odbc"
        Me.tabOdbc.UseVisualStyleBackColor = True
        '
        'UcOdbc1
        '
        Me.UcOdbc1.ConnectionString = ""
        Me.UcOdbc1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcOdbc1.Location = New System.Drawing.Point(0, 0)
        Me.UcOdbc1.Name = "UcOdbc1"
        Me.UcOdbc1.Size = New System.Drawing.Size(433, 275)
        Me.UcOdbc1.TabIndex = 0
        '
        'tabMsExcel
        '
        Me.tabMsExcel.Controls.Add(Me.UcExcel1)
        Me.tabMsExcel.Location = New System.Drawing.Point(4, 22)
        Me.tabMsExcel.Name = "tabMsExcel"
        Me.tabMsExcel.Size = New System.Drawing.Size(433, 275)
        Me.tabMsExcel.TabIndex = 6
        Me.tabMsExcel.Text = "Excel"
        Me.tabMsExcel.UseVisualStyleBackColor = True
        '
        'UcExcel1
        '
        Me.UcExcel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcExcel1.FilePath = ""
        Me.UcExcel1.Location = New System.Drawing.Point(0, 0)
        Me.UcExcel1.Name = "UcExcel1"
        Me.UcExcel1.Size = New System.Drawing.Size(433, 275)
        Me.UcExcel1.TabIndex = 0
        '
        'tabTextFile
        '
        Me.tabTextFile.Controls.Add(Me.UcTextFile1)
        Me.tabTextFile.Location = New System.Drawing.Point(4, 22)
        Me.tabTextFile.Name = "tabTextFile"
        Me.tabTextFile.Size = New System.Drawing.Size(433, 275)
        Me.tabTextFile.TabIndex = 7
        Me.tabTextFile.Text = "Text File"
        Me.tabTextFile.UseVisualStyleBackColor = True
        '
        'UcTextFile1
        '
        Me.UcTextFile1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcTextFile1.FilePath = ""
        Me.UcTextFile1.Location = New System.Drawing.Point(0, 0)
        Me.UcTextFile1.Name = "UcTextFile1"
        Me.UcTextFile1.Size = New System.Drawing.Size(433, 275)
        Me.UcTextFile1.TabIndex = 0
        '
        'UCConnections
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "UCConnections"
        Me.Size = New System.Drawing.Size(441, 301)
        Me.TabControl1.ResumeLayout(False)
        Me.tabSqlServer.ResumeLayout(False)
        Me.tabMsAccess.ResumeLayout(False)
        Me.tabMySql.ResumeLayout(False)
        Me.tabOracle.ResumeLayout(False)
        Me.tabOleDb.ResumeLayout(False)
        Me.tabOdbc.ResumeLayout(False)
        Me.tabMsExcel.ResumeLayout(False)
        Me.tabTextFile.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabMsAccess As System.Windows.Forms.TabPage
    Friend WithEvents tabSqlServer As System.Windows.Forms.TabPage
    Friend WithEvents tabMySql As System.Windows.Forms.TabPage
    Friend WithEvents tabOleDb As System.Windows.Forms.TabPage
    Friend WithEvents tabOdbc As System.Windows.Forms.TabPage
    Friend WithEvents tabOracle As System.Windows.Forms.TabPage
    Friend WithEvents UcAccess1 As CodeGenerator.UCAccess
    Friend WithEvents UcSqlServer1 As CodeGenerator.UCSqlServer
    Friend WithEvents UcMySql1 As CodeGenerator.UCMySql
    Friend WithEvents UcOracle1 As CodeGenerator.UCOracle
    Friend WithEvents UcOleDb1 As CodeGenerator.UCOleDb
    Friend WithEvents UcOdbc1 As CodeGenerator.UCOdbc
    Friend WithEvents tabMsExcel As System.Windows.Forms.TabPage
    Friend WithEvents UcExcel1 As CodeGenerator.UCExcel
    Friend WithEvents tabTextFile As System.Windows.Forms.TabPage
    Friend WithEvents UcTextFile1 As CodeGenerator.UCTextFile

End Class
