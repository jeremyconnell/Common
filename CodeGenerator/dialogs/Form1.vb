Imports System.Configuration.ConfigurationManager
Imports System.Collections.Generic

Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        InitializeComponent()
        Me.Text &= " " & My.Application.Info.Version.ToString
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents MSAccessToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CtestmdbToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SqlServerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MySqlToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OleDbToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OracleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabOutput As System.Windows.Forms.TabPage
    Friend WithEvents UcOutput1 As CodeGenerator.UCOutput
    Friend WithEvents tabConnection As System.Windows.Forms.TabPage
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabXml As System.Windows.Forms.TabPage
    Friend WithEvents UcXsdClassGenerator1 As CodeGenerator.UCXsdClassGenerator
    Friend WithEvents tabTemplates As System.Windows.Forms.TabPage
    Friend WithEvents UcTemplates1 As CodeGenerator.UCTemplates
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents miInstructions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miInstructionsRelational As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miInstructionsXml As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miFrameworkdll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miFrameworkPdb As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miAuditSql As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miAuditDll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miAuditPdb As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miCodeplex As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miResources As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents UcConnections1 As CodeGenerator.UCConnections
    Friend WithEvents miTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miScriptDiagrams As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.MSAccessToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CtestmdbToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SqlServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MySqlToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OleDbToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OracleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabOutput = New System.Windows.Forms.TabPage
        Me.UcOutput1 = New CodeGenerator.UCOutput
        Me.tabConnection = New System.Windows.Forms.TabPage
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.UcConnections1 = New CodeGenerator.UCConnections
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabXml = New System.Windows.Forms.TabPage
        Me.UcXsdClassGenerator1 = New CodeGenerator.UCXsdClassGenerator
        Me.tabTemplates = New System.Windows.Forms.TabPage
        Me.UcTemplates1 = New CodeGenerator.UCTemplates
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.miInstructions = New System.Windows.Forms.ToolStripMenuItem
        Me.miInstructionsRelational = New System.Windows.Forms.ToolStripMenuItem
        Me.miInstructionsXml = New System.Windows.Forms.ToolStripMenuItem
        Me.miCodeplex = New System.Windows.Forms.ToolStripMenuItem
        Me.miResources = New System.Windows.Forms.ToolStripMenuItem
        Me.miFrameworkdll = New System.Windows.Forms.ToolStripMenuItem
        Me.miFrameworkPdb = New System.Windows.Forms.ToolStripMenuItem
        Me.miAuditSql = New System.Windows.Forms.ToolStripMenuItem
        Me.miAuditDll = New System.Windows.Forms.ToolStripMenuItem
        Me.miAuditPdb = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.miTools = New System.Windows.Forms.ToolStripMenuItem
        Me.miScriptDiagrams = New System.Windows.Forms.ToolStripMenuItem
        Me.tabOutput.SuspendLayout()
        Me.tabConnection.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tabXml.SuspendLayout()
        Me.tabTemplates.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'FolderBrowserDialog1
        '
        Me.FolderBrowserDialog1.Description = "Browse to the root folder of your Schema project"
        Me.FolderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer
        '
        'MSAccessToolStripMenuItem
        '
        Me.MSAccessToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CtestmdbToolStripMenuItem})
        Me.MSAccessToolStripMenuItem.Name = "MSAccessToolStripMenuItem"
        Me.MSAccessToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.MSAccessToolStripMenuItem.Text = "MSAccess"
        '
        'CtestmdbToolStripMenuItem
        '
        Me.CtestmdbToolStripMenuItem.Name = "CtestmdbToolStripMenuItem"
        Me.CtestmdbToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.CtestmdbToolStripMenuItem.Text = "c:\test.mdb"
        '
        'SqlServerToolStripMenuItem
        '
        Me.SqlServerToolStripMenuItem.Name = "SqlServerToolStripMenuItem"
        Me.SqlServerToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.SqlServerToolStripMenuItem.Text = "SqlServer"
        '
        'MySqlToolStripMenuItem
        '
        Me.MySqlToolStripMenuItem.Name = "MySqlToolStripMenuItem"
        Me.MySqlToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.MySqlToolStripMenuItem.Text = "MySql"
        '
        'OleDbToolStripMenuItem
        '
        Me.OleDbToolStripMenuItem.Name = "OleDbToolStripMenuItem"
        Me.OleDbToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.OleDbToolStripMenuItem.Text = "OleDb"
        '
        'OracleToolStripMenuItem
        '
        Me.OracleToolStripMenuItem.Name = "OracleToolStripMenuItem"
        Me.OracleToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.OracleToolStripMenuItem.Text = "Oracle"
        '
        'tabOutput
        '
        Me.tabOutput.Controls.Add(Me.UcOutput1)
        Me.tabOutput.Location = New System.Drawing.Point(4, 22)
        Me.tabOutput.Name = "tabOutput"
        Me.tabOutput.Size = New System.Drawing.Size(192, 74)
        Me.tabOutput.TabIndex = 3
        Me.tabOutput.Text = "Database O/R Mapping"
        Me.tabOutput.UseVisualStyleBackColor = True
        '
        'UcOutput1
        '
        Me.UcOutput1.Architecture = CodeGenerator.EArchitecture.Dynamic
        Me.UcOutput1.CanUseAuditTrail = False
        Me.UcOutput1.CSharp = False
        Me.UcOutput1.CSharpNamespace = "SchemaSample"
        Me.UcOutput1.Database = Nothing
        Me.UcOutput1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcOutput1.HasNamespace = True
        Me.UcOutput1.Location = New System.Drawing.Point(0, 0)
        Me.UcOutput1.Name = "UcOutput1"
        Me.UcOutput1.OutputFolder = ""
        Me.UcOutput1.Size = New System.Drawing.Size(192, 74)
        Me.UcOutput1.StoredProcPrefix = ""
        Me.UcOutput1.TabIndex = 0
        Me.UcOutput1.TablePrefix = ""
        Me.UcOutput1.UseAuditTrail = False
        '
        'tabConnection
        '
        Me.tabConnection.Controls.Add(Me.GroupBox1)
        Me.tabConnection.Location = New System.Drawing.Point(4, 22)
        Me.tabConnection.Name = "tabConnection"
        Me.tabConnection.Size = New System.Drawing.Size(975, 611)
        Me.tabConnection.TabIndex = 0
        Me.tabConnection.Text = "Database Connection"
        Me.tabConnection.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.UcConnections1)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(975, 611)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Platform"
        '
        'UcConnections1
        '
        Me.UcConnections1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcConnections1.Location = New System.Drawing.Point(3, 16)
        Me.UcConnections1.Name = "UcConnections1"
        Me.UcConnections1.Size = New System.Drawing.Size(969, 592)
        Me.UcConnections1.Tab = XmlConnections.EDriverTab.SqlServer
        Me.UcConnections1.TabIndex = 5
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabConnection)
        Me.TabControl1.Controls.Add(Me.tabOutput)
        Me.TabControl1.Controls.Add(Me.tabXml)
        Me.TabControl1.Controls.Add(Me.tabTemplates)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 24)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(983, 637)
        Me.TabControl1.TabIndex = 0
        '
        'tabXml
        '
        Me.tabXml.Controls.Add(Me.UcXsdClassGenerator1)
        Me.tabXml.Location = New System.Drawing.Point(4, 22)
        Me.tabXml.Name = "tabXml"
        Me.tabXml.Size = New System.Drawing.Size(192, 74)
        Me.tabXml.TabIndex = 4
        Me.tabXml.Text = "Xml O/R Mapping"
        Me.tabXml.UseVisualStyleBackColor = True
        '
        'UcXsdClassGenerator1
        '
        Me.UcXsdClassGenerator1.DefaultNameSpace = "XmlNorthWind"
        Me.UcXsdClassGenerator1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcXsdClassGenerator1.Language = CodeGenerator.ELanguage.VbNet
        Me.UcXsdClassGenerator1.Location = New System.Drawing.Point(0, 0)
        Me.UcXsdClassGenerator1.Name = "UcXsdClassGenerator1"
        Me.UcXsdClassGenerator1.Size = New System.Drawing.Size(192, 74)
        Me.UcXsdClassGenerator1.TabIndex = 0
        '
        'tabTemplates
        '
        Me.tabTemplates.Controls.Add(Me.UcTemplates1)
        Me.tabTemplates.Location = New System.Drawing.Point(4, 22)
        Me.tabTemplates.Name = "tabTemplates"
        Me.tabTemplates.Size = New System.Drawing.Size(192, 74)
        Me.tabTemplates.TabIndex = 6
        Me.tabTemplates.Text = "Templates"
        Me.tabTemplates.UseVisualStyleBackColor = True
        '
        'UcTemplates1
        '
        Me.UcTemplates1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcTemplates1.Location = New System.Drawing.Point(0, 0)
        Me.UcTemplates1.Name = "UcTemplates1"
        Me.UcTemplates1.Size = New System.Drawing.Size(192, 74)
        Me.UcTemplates1.TabIndex = 0
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miInstructions, Me.miResources, Me.miTools})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.MenuStrip1.Size = New System.Drawing.Size(983, 24)
        Me.MenuStrip1.TabIndex = 1
        '
        'miInstructions
        '
        Me.miInstructions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.miInstructions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miInstructionsRelational, Me.miInstructionsXml, Me.miCodeplex})
        Me.miInstructions.Name = "miInstructions"
        Me.miInstructions.ShortcutKeyDisplayString = "Alt-I"
        Me.miInstructions.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.miInstructions.Size = New System.Drawing.Size(76, 20)
        Me.miInstructions.Text = "Instructions"
        '
        'miInstructionsRelational
        '
        Me.miInstructionsRelational.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.miInstructionsRelational.Name = "miInstructionsRelational"
        Me.miInstructionsRelational.Size = New System.Drawing.Size(338, 22)
        Me.miInstructionsRelational.Text = "Getting started with Database Schema"
        '
        'miInstructionsXml
        '
        Me.miInstructionsXml.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.miInstructionsXml.Name = "miInstructionsXml"
        Me.miInstructionsXml.Size = New System.Drawing.Size(338, 22)
        Me.miInstructionsXml.Text = "Getting started with Xml Schema (*.xsd)"
        '
        'miCodeplex
        '
        Me.miCodeplex.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.miCodeplex.Name = "miCodeplex"
        Me.miCodeplex.Size = New System.Drawing.Size(338, 22)
        Me.miCodeplex.Text = "Open Source Publication (http://picasso.codeplex.com)"
        '
        'miResources
        '
        Me.miResources.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.miResources.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miFrameworkdll, Me.miFrameworkPdb, Me.miAuditSql, Me.miAuditDll, Me.miAuditPdb})
        Me.miResources.Name = "miResources"
        Me.miResources.ShortcutKeyDisplayString = ""
        Me.miResources.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.miResources.Size = New System.Drawing.Size(69, 20)
        Me.miResources.Text = "Resources"
        '
        'miFrameworkdll
        '
        Me.miFrameworkdll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.miFrameworkdll.Name = "miFrameworkdll"
        Me.miFrameworkdll.Size = New System.Drawing.Size(196, 22)
        Me.miFrameworkdll.Text = "Framework.dll"
        '
        'miFrameworkPdb
        '
        Me.miFrameworkPdb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.miFrameworkPdb.Name = "miFrameworkPdb"
        Me.miFrameworkPdb.Size = New System.Drawing.Size(196, 22)
        Me.miFrameworkPdb.Text = "Framework.pdb"
        '
        'miAuditSql
        '
        Me.miAuditSql.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.miAuditSql.Name = "miAuditSql"
        Me.miAuditSql.Size = New System.Drawing.Size(196, 22)
        Me.miAuditSql.Text = "SchemaAudit.sql  (T-SQL)"
        '
        'miAuditDll
        '
        Me.miAuditDll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.miAuditDll.Name = "miAuditDll"
        Me.miAuditDll.Size = New System.Drawing.Size(196, 22)
        Me.miAuditDll.Text = "SchemaAudit.dll"
        '
        'miAuditPdb
        '
        Me.miAuditPdb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.miAuditPdb.Name = "miAuditPdb"
        Me.miAuditPdb.Size = New System.Drawing.Size(196, 22)
        Me.miAuditPdb.Text = "SchemaAudit.pdb"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.AddExtension = False
        Me.SaveFileDialog1.SupportMultiDottedExtensions = True
        Me.SaveFileDialog1.Title = "Select a location to save the file to"
        '
        'miTools
        '
        Me.miTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miScriptDiagrams})
        Me.miTools.Name = "miTools"
        Me.miTools.Size = New System.Drawing.Size(44, 20)
        Me.miTools.Text = "Tools"
        '
        'miScriptDiagrams
        '
        Me.miScriptDiagrams.Name = "miScriptDiagrams"
        Me.miScriptDiagrams.Size = New System.Drawing.Size(152, 22)
        Me.miScriptDiagrams.Text = "Script Diagrams"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(983, 661)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Class Generator"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabOutput.ResumeLayout(False)
        Me.tabConnection.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.tabXml.ResumeLayout(False)
        Me.tabTemplates.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Enums"
    Public Enum ETemplate
        Aspx
        Load
        Save
    End Enum
    Public Enum ETab
        Connection
        ProjectAndSchema
        XmlGenerator
        TemplateEditor
        Documentation
    End Enum
#End Region

#Region "UC Events"
    Private Sub UcConnections1_TestOk() Handles UcConnections1.TestOk
        UcOutput1.Database = Nothing

        miTools.Visible = TypeOf UcConnections1.DataSrc Is CSqlClient

        'Jump to 2nd tab and populate it
        TabControl1.SelectedIndex = ETab.ProjectAndSchema
        UcOutput1.rbStoredProcs.Enabled = UcConnections1.Platform <> EPlatform.Other
        With UcConnections1.SchemaInfo
            UcOutput1.TablePrefix = .TablePrefix
            UcOutput1.StoredProcPrefix = .StoredProcPrefix
            UcOutput1.CSharp = .CSharp
            UcOutput1.CSharpNamespace = .CSharpNamespace
            UcOutput1.Architecture = CType(.Architecture, EArchitecture)
            UcOutput1.OutputFolder = .OutputFolder
            UcOutput1.ctrlClassGen.UcUiCodeGen1.OutputFolderReadOnly = .OutputFolderReadonly
            UcOutput1.ctrlClassGen.UcUiCodeGen1.OutputFolderEditable = .OutputFolderEditable
        End With

        UcOutput1.Database = UcConnections1.DataSrc
        UcOutput1.Init()


        'Try to list all the tables, if the driver supports it
        With UcConnections1.DataSrc
            Try
                Dim tables As List(Of String) = .AllTableNames
                If IsNothing(tables) Then UcOutput1.TableNames = Nothing Else UcOutput1.TableNames = tables.ToArray
            Catch ex As Exception
                Dim msg As String = "List table names failed"
                If .ConnectionString.ToLower.Contains(".mdb") Then msg = "Note: For MsAccess databases, it is recommended that you grant read permits to MSysObjects (tools=>security=>user and group permits: Grant Read on MSysObjects)"
                msg &= vbCrLf & vbCrLf & ex.Message
                MsgBox(msg, MsgBoxStyle.OkOnly, "List Table Names Failed")
                UcOutput1.TableNames = Nothing
                Exit Sub
            End Try
        End With
    End Sub
    Private Sub UcOutput1_StoreSettings() Handles UcOutput1.StoreSettings
        StoreSettings()
    End Sub
#End Region

#Region "Local Events"
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim count As Integer = CUserSettings.Singleton.Skins.Count
        'Dim skin As CSkin = CUser_Connections.AddSkin("test")
        'count = CUserSettings.Singleton.Skins.Count
        'CUserSettings.RemoveSkin(skin)
        'With New Threading.ThreadStart(AddressOf CUser_Templates.UpdateFromNetwork)
        '    .BeginInvoke(Nothing, Nothing)
        'End With

        UcConnections1.Test()
    End Sub
    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        'No validation for last 2 tabs
        If TabControl1.SelectedIndex >= ETab.XmlGenerator Then Exit Sub

        If TabControl1.SelectedIndex > ETab.Connection Then
            If Not UcConnections1.IsReady Then
                TabControl1.SelectedIndex = ETab.Connection
                UcConnections1.Test()
                Exit Sub
            End If
        End If
        If TabControl1.SelectedIndex > ETab.ProjectAndSchema Then
            If UcOutput1.Metadata.FolderPath.Length = 0 Then
                TabControl1.SelectedIndex = ETab.ProjectAndSchema
                MessageBox.Show("You must first select an Output folder and generate some classes, before Relationship properties can be generated", "Select an Output Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "Generator Events"
    Private WithEvents _tableInfo As CTableInformation
    Private Sub TableInfo_Error(ByVal spName As String, ByVal ex As Exception) Handles _tableInfo.RaiseError
        MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Execute Script Error")
    End Sub
#End Region

#Region "Private - StoreSettings"
    Private Sub StoreSettings()
        With UcConnections1.SchemaInfo
            .OutputFolder = UcOutput1.OutputFolder
            .TablePrefix = UcOutput1.TablePrefix
            .StoredProcPrefix = UcOutput1.StoredProcPrefix
            .Architecture = UcOutput1.Architecture
            .CSharpNamespace = UcOutput1.CSharpNamespace
            .CSharp = UcOutput1.CSharp

            CUser_Connections.Storage = .Root
        End With
    End Sub
#End Region

#Region "Private - Downloads"
    Private Sub Launch(ByVal relPath As String, Optional ByVal makeFullPath As Boolean = True)
        If makeFullPath Then relPath = String.Concat(My.Application.Info.DirectoryPath, "/", relPath)
        Process.Start(relPath)
    End Sub
    Private Sub Download(ByVal relPath As String)
        Dim sourcePath As String = String.Concat(My.Application.Info.DirectoryPath, "/", relPath)
        SaveFileDialog1.FileName = IO.Path.GetFileName(relPath)
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            Try
                IO.File.Copy(sourcePath, SaveFileDialog1.FileName)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Error Saving file")
            End Try
        End If
    End Sub
    Private Sub ScriptDiagrams()
        SaveFileDialog1.FileName = "diagrams.sql"
        SaveFileDialog1.Title = "Select a location to save the script to"
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            Try
                ScriptDiagrams(SaveFileDialog1.FileName)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Error Saving script")
            End Try
        End If
    End Sub
    Private Sub ScriptDiagrams(ByVal filePath As String)
        Dim diagrams As DataSet = UcConnections1.DataSrc.ExecuteDataSet("SELECT name,definition FROM sysdiagrams")
        Dim sw As New IO.StreamWriter(filePath)
        Try
            For Each i As DataRow In diagrams.Tables(0).Rows
                sw.Write("INSERT INTO [dbo].[sysdiagrams] ([name], [principal_id], [version], [definition]) VALUES ('")
                sw.Write(i("name"))
                sw.Write("',1,1,0x")
                sw.Write(CBinary.BytesToHex(CType(i("definition"), Byte())))
                sw.WriteLine(")")
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error saving script", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            sw.Close()
        End Try
    End Sub
#End Region

    Private Sub miInstructionsRelational_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miInstructionsRelational.Click
        Launch("downloads/instructions.txt")
    End Sub
    Private Sub miInstructionsXml_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miInstructionsXml.Click
        Launch("downloads/instructionsXml.txt")
    End Sub
    Private Sub miCodeplex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miCodeplex.Click
        Launch("http://picasso.codeplex.com", False)
    End Sub

    Private Sub miFrameworkdll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miFrameworkdll.Click
        Download("Framework.dll")
    End Sub
    Private Sub miFrameworkPdb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miFrameworkPdb.Click
        Download("Framework.pdb")
    End Sub
    Private Sub miAuditSql_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miAuditSql.Click
        Download("SchemaAudit.sql")
    End Sub
    Private Sub miAuditDll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miAuditDll.Click
        Download("SchemaAudit.dll")
    End Sub
    Private Sub miAuditPdb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miAuditPdb.Click
        Download("SchemaAudit.pdb")
    End Sub
    Private Sub miScriptDiagrams_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miScriptDiagrams.Click
        ScriptDiagrams()
    End Sub
End Class
