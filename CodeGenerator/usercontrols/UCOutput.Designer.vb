<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCOutput
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
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.ctrlTables = New CodeGenerator.UCTables
        Me.pnlFilterTables = New System.Windows.Forms.Panel
        Me.btnGenerateAll = New System.Windows.Forms.Button
        Me.chkFilterTables = New System.Windows.Forms.CheckBox
        Me.pnlTableTextbox = New System.Windows.Forms.Panel
        Me.gboxTableName = New System.Windows.Forms.GroupBox
        Me.txtTableName = New System.Windows.Forms.TextBox
        Me.cboTables = New System.Windows.Forms.ComboBox
        Me.btnTestTable = New System.Windows.Forms.Button
        Me.gboxProjectSettings = New System.Windows.Forms.GroupBox
        Me.pnlPrefix = New System.Windows.Forms.Panel
        Me.btnSave = New System.Windows.Forms.Button
        Me.txtStoredProcPrefix = New System.Windows.Forms.TextBox
        Me.lblStoredProcPrefix = New System.Windows.Forms.Label
        Me.txtTablePrefix = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.pnlLanguage = New System.Windows.Forms.Panel
        Me.txtNamespace = New System.Windows.Forms.TextBox
        Me.lblNamespace = New System.Windows.Forms.Label
        Me.rbCSharp = New System.Windows.Forms.RadioButton
        Me.rbVbNet = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.pnlAudit = New System.Windows.Forms.Panel
        Me.chkUseAuditTrail = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.pnlArchitecture = New System.Windows.Forms.Panel
        Me.rbStoredProcs = New System.Windows.Forms.RadioButton
        Me.rbComplete = New System.Windows.Forms.RadioButton
        Me.rbCompact = New System.Windows.Forms.RadioButton
        Me.Label5 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtFolderPath = New System.Windows.Forms.TextBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.gboxTable = New System.Windows.Forms.GroupBox
        Me.ctrlClassGen = New CodeGenerator.UCClassGenerator
        Me.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.pnlFilterTables.SuspendLayout()
        Me.pnlTableTextbox.SuspendLayout()
        Me.gboxTableName.SuspendLayout()
        Me.gboxProjectSettings.SuspendLayout()
        Me.pnlPrefix.SuspendLayout()
        Me.pnlLanguage.SuspendLayout()
        Me.pnlAudit.SuspendLayout()
        Me.pnlArchitecture.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.gboxTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'FolderBrowserDialog1
        '
        Me.FolderBrowserDialog1.Description = "Browse to the root folder of your schema project, which should be inside your sol" & _
            "ution folder"
        Me.FolderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.SplitContainer1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1364, 644)
        Me.Panel1.TabIndex = 30
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.gboxProjectSettings)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.gboxTable)
        Me.SplitContainer1.Size = New System.Drawing.Size(1364, 644)
        Me.SplitContainer1.SplitterDistance = 646
        Me.SplitContainer1.TabIndex = 31
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.ctrlTables)
        Me.GroupBox3.Controls.Add(Me.pnlFilterTables)
        Me.GroupBox3.Controls.Add(Me.pnlTableTextbox)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox3.Location = New System.Drawing.Point(0, 149)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(646, 495)
        Me.GroupBox3.TabIndex = 30
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Existing Tables and Classes"
        '
        'ctrlTables
        '
        Me.ctrlTables.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctrlTables.Enabled = False
        Me.ctrlTables.Location = New System.Drawing.Point(3, 38)
        Me.ctrlTables.Name = "ctrlTables"
        Me.ctrlTables.ShowCheckboxes = True
        Me.ctrlTables.Size = New System.Drawing.Size(640, 410)
        Me.ctrlTables.TabIndex = 2
        Me.ctrlTables.Table = Nothing
        Me.ctrlTables.TableName = ""
        '
        'pnlFilterTables
        '
        Me.pnlFilterTables.Controls.Add(Me.chkFilterTables)
        Me.pnlFilterTables.Controls.Add(Me.btnGenerateAll)
        Me.pnlFilterTables.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlFilterTables.Location = New System.Drawing.Point(3, 16)
        Me.pnlFilterTables.Name = "pnlFilterTables"
        Me.pnlFilterTables.Size = New System.Drawing.Size(640, 22)
        Me.pnlFilterTables.TabIndex = 4
        '
        'btnGenerateAll
        '
        Me.btnGenerateAll.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnGenerateAll.Location = New System.Drawing.Point(509, 0)
        Me.btnGenerateAll.Name = "btnGenerateAll"
        Me.btnGenerateAll.Size = New System.Drawing.Size(131, 22)
        Me.btnGenerateAll.TabIndex = 3
        Me.btnGenerateAll.Text = "Generate Selected"
        Me.btnGenerateAll.UseVisualStyleBackColor = True
        '
        'chkFilterTables
        '
        Me.chkFilterTables.Checked = True
        Me.chkFilterTables.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFilterTables.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkFilterTables.Location = New System.Drawing.Point(0, 0)
        Me.chkFilterTables.Name = "chkFilterTables"
        Me.chkFilterTables.Size = New System.Drawing.Size(509, 22)
        Me.chkFilterTables.TabIndex = 2
        Me.chkFilterTables.Text = "Only show tables begining with 'tbl'"
        Me.chkFilterTables.UseVisualStyleBackColor = True
        '
        'pnlTableTextbox
        '
        Me.pnlTableTextbox.Controls.Add(Me.gboxTableName)
        Me.pnlTableTextbox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlTableTextbox.Location = New System.Drawing.Point(3, 448)
        Me.pnlTableTextbox.Name = "pnlTableTextbox"
        Me.pnlTableTextbox.Size = New System.Drawing.Size(640, 44)
        Me.pnlTableTextbox.TabIndex = 3
        '
        'gboxTableName
        '
        Me.gboxTableName.Controls.Add(Me.txtTableName)
        Me.gboxTableName.Controls.Add(Me.cboTables)
        Me.gboxTableName.Controls.Add(Me.btnTestTable)
        Me.gboxTableName.Dock = System.Windows.Forms.DockStyle.Top
        Me.gboxTableName.Location = New System.Drawing.Point(0, 0)
        Me.gboxTableName.Name = "gboxTableName"
        Me.gboxTableName.Size = New System.Drawing.Size(640, 40)
        Me.gboxTableName.TabIndex = 25
        Me.gboxTableName.TabStop = False
        Me.gboxTableName.Text = "Table Name"
        '
        'txtTableName
        '
        Me.txtTableName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTableName.Location = New System.Drawing.Point(3, 16)
        Me.txtTableName.Name = "txtTableName"
        Me.txtTableName.Size = New System.Drawing.Size(543, 20)
        Me.txtTableName.TabIndex = 3
        '
        'cboTables
        '
        Me.cboTables.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboTables.Location = New System.Drawing.Point(3, 16)
        Me.cboTables.Name = "cboTables"
        Me.cboTables.Size = New System.Drawing.Size(543, 21)
        Me.cboTables.TabIndex = 2
        Me.cboTables.Visible = False
        '
        'btnTestTable
        '
        Me.btnTestTable.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnTestTable.Location = New System.Drawing.Point(546, 16)
        Me.btnTestTable.Name = "btnTestTable"
        Me.btnTestTable.Size = New System.Drawing.Size(91, 21)
        Me.btnTestTable.TabIndex = 4
        Me.btnTestTable.Text = "Validate"
        '
        'gboxProjectSettings
        '
        Me.gboxProjectSettings.Controls.Add(Me.pnlPrefix)
        Me.gboxProjectSettings.Controls.Add(Me.pnlLanguage)
        Me.gboxProjectSettings.Controls.Add(Me.pnlAudit)
        Me.gboxProjectSettings.Controls.Add(Me.pnlArchitecture)
        Me.gboxProjectSettings.Dock = System.Windows.Forms.DockStyle.Top
        Me.gboxProjectSettings.Location = New System.Drawing.Point(0, 40)
        Me.gboxProjectSettings.Name = "gboxProjectSettings"
        Me.gboxProjectSettings.Size = New System.Drawing.Size(646, 109)
        Me.gboxProjectSettings.TabIndex = 31
        Me.gboxProjectSettings.TabStop = False
        Me.gboxProjectSettings.Text = "Schema/Project Settings"
        '
        'pnlPrefix
        '
        Me.pnlPrefix.Controls.Add(Me.btnSave)
        Me.pnlPrefix.Controls.Add(Me.txtStoredProcPrefix)
        Me.pnlPrefix.Controls.Add(Me.lblStoredProcPrefix)
        Me.pnlPrefix.Controls.Add(Me.txtTablePrefix)
        Me.pnlPrefix.Controls.Add(Me.Label6)
        Me.pnlPrefix.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlPrefix.Location = New System.Drawing.Point(3, 82)
        Me.pnlPrefix.Name = "pnlPrefix"
        Me.pnlPrefix.Size = New System.Drawing.Size(640, 22)
        Me.pnlPrefix.TabIndex = 38
        '
        'btnSave
        '
        Me.btnSave.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnSave.Location = New System.Drawing.Point(483, 0)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(91, 22)
        Me.btnSave.TabIndex = 36
        Me.btnSave.Text = "Save Settings"
        '
        'txtStoredProcPrefix
        '
        Me.txtStoredProcPrefix.Dock = System.Windows.Forms.DockStyle.Left
        Me.txtStoredProcPrefix.Location = New System.Drawing.Point(333, 0)
        Me.txtStoredProcPrefix.Name = "txtStoredProcPrefix"
        Me.txtStoredProcPrefix.Size = New System.Drawing.Size(150, 20)
        Me.txtStoredProcPrefix.TabIndex = 35
        '
        'lblStoredProcPrefix
        '
        Me.lblStoredProcPrefix.Dock = System.Windows.Forms.DockStyle.Left
        Me.lblStoredProcPrefix.Location = New System.Drawing.Point(197, 0)
        Me.lblStoredProcPrefix.Name = "lblStoredProcPrefix"
        Me.lblStoredProcPrefix.Size = New System.Drawing.Size(136, 22)
        Me.lblStoredProcPrefix.TabIndex = 34
        Me.lblStoredProcPrefix.Text = "Stored Procedure Prefix: "
        Me.lblStoredProcPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtTablePrefix
        '
        Me.txtTablePrefix.Dock = System.Windows.Forms.DockStyle.Left
        Me.txtTablePrefix.Location = New System.Drawing.Point(78, 0)
        Me.txtTablePrefix.Name = "txtTablePrefix"
        Me.txtTablePrefix.Size = New System.Drawing.Size(119, 20)
        Me.txtTablePrefix.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label6.Location = New System.Drawing.Point(0, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(78, 22)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Table Prefix: "
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlLanguage
        '
        Me.pnlLanguage.Controls.Add(Me.txtNamespace)
        Me.pnlLanguage.Controls.Add(Me.lblNamespace)
        Me.pnlLanguage.Controls.Add(Me.rbCSharp)
        Me.pnlLanguage.Controls.Add(Me.rbVbNet)
        Me.pnlLanguage.Controls.Add(Me.Label1)
        Me.pnlLanguage.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlLanguage.Location = New System.Drawing.Point(3, 60)
        Me.pnlLanguage.Name = "pnlLanguage"
        Me.pnlLanguage.Size = New System.Drawing.Size(640, 22)
        Me.pnlLanguage.TabIndex = 31
        '
        'txtNamespace
        '
        Me.txtNamespace.Dock = System.Windows.Forms.DockStyle.Left
        Me.txtNamespace.Location = New System.Drawing.Point(333, 0)
        Me.txtNamespace.Name = "txtNamespace"
        Me.txtNamespace.Size = New System.Drawing.Size(150, 20)
        Me.txtNamespace.TabIndex = 33
        '
        'lblNamespace
        '
        Me.lblNamespace.Dock = System.Windows.Forms.DockStyle.Left
        Me.lblNamespace.Location = New System.Drawing.Point(197, 0)
        Me.lblNamespace.Name = "lblNamespace"
        Me.lblNamespace.Size = New System.Drawing.Size(136, 22)
        Me.lblNamespace.TabIndex = 32
        Me.lblNamespace.Text = "Namespace (C#-only): "
        Me.lblNamespace.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'rbCSharp
        '
        Me.rbCSharp.AutoSize = True
        Me.rbCSharp.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbCSharp.Location = New System.Drawing.Point(137, 0)
        Me.rbCSharp.Name = "rbCSharp"
        Me.rbCSharp.Size = New System.Drawing.Size(60, 22)
        Me.rbCSharp.TabIndex = 31
        Me.rbCSharp.Text = "CSharp"
        Me.rbCSharp.UseVisualStyleBackColor = True
        '
        'rbVbNet
        '
        Me.rbVbNet.AutoSize = True
        Me.rbVbNet.Checked = True
        Me.rbVbNet.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbVbNet.Location = New System.Drawing.Point(78, 0)
        Me.rbVbNet.Name = "rbVbNet"
        Me.rbVbNet.Size = New System.Drawing.Size(59, 22)
        Me.rbVbNet.TabIndex = 30
        Me.rbVbNet.TabStop = True
        Me.rbVbNet.Text = "VB.Net"
        Me.rbVbNet.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 22)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Language"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlAudit
        '
        Me.pnlAudit.Controls.Add(Me.chkUseAuditTrail)
        Me.pnlAudit.Controls.Add(Me.Label3)
        Me.pnlAudit.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlAudit.Location = New System.Drawing.Point(3, 38)
        Me.pnlAudit.Name = "pnlAudit"
        Me.pnlAudit.Size = New System.Drawing.Size(640, 22)
        Me.pnlAudit.TabIndex = 34
        '
        'chkUseAuditTrail
        '
        Me.chkUseAuditTrail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkUseAuditTrail.Enabled = False
        Me.chkUseAuditTrail.Location = New System.Drawing.Point(78, 0)
        Me.chkUseAuditTrail.Name = "chkUseAuditTrail"
        Me.chkUseAuditTrail.Size = New System.Drawing.Size(562, 22)
        Me.chkUseAuditTrail.TabIndex = 3
        Me.chkUseAuditTrail.Text = "Use Audit Trail  - Need to run script && reference SchemaAudit.dll"
        Me.chkUseAuditTrail.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 22)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Audit Trail"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlArchitecture
        '
        Me.pnlArchitecture.Controls.Add(Me.rbStoredProcs)
        Me.pnlArchitecture.Controls.Add(Me.rbComplete)
        Me.pnlArchitecture.Controls.Add(Me.rbCompact)
        Me.pnlArchitecture.Controls.Add(Me.Label5)
        Me.pnlArchitecture.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlArchitecture.Location = New System.Drawing.Point(3, 16)
        Me.pnlArchitecture.Name = "pnlArchitecture"
        Me.pnlArchitecture.Size = New System.Drawing.Size(640, 22)
        Me.pnlArchitecture.TabIndex = 37
        '
        'rbStoredProcs
        '
        Me.rbStoredProcs.AutoSize = True
        Me.rbStoredProcs.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbStoredProcs.Location = New System.Drawing.Point(275, 0)
        Me.rbStoredProcs.Name = "rbStoredProcs"
        Me.rbStoredProcs.Size = New System.Drawing.Size(113, 22)
        Me.rbStoredProcs.TabIndex = 6
        Me.rbStoredProcs.Text = "Stored Procedures"
        Me.rbStoredProcs.UseVisualStyleBackColor = True
        '
        'rbComplete
        '
        Me.rbComplete.AutoSize = True
        Me.rbComplete.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbComplete.Location = New System.Drawing.Point(222, 0)
        Me.rbComplete.Name = "rbComplete"
        Me.rbComplete.Size = New System.Drawing.Size(53, 22)
        Me.rbComplete.TabIndex = 5
        Me.rbComplete.Text = "Bullky"
        Me.rbComplete.UseVisualStyleBackColor = True
        '
        'rbCompact
        '
        Me.rbCompact.AutoSize = True
        Me.rbCompact.Checked = True
        Me.rbCompact.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbCompact.Location = New System.Drawing.Point(78, 0)
        Me.rbCompact.Name = "rbCompact"
        Me.rbCompact.Size = New System.Drawing.Size(144, 22)
        Me.rbCompact.TabIndex = 4
        Me.rbCompact.TabStop = True
        Me.rbCompact.Text = "Standard (recommended)"
        Me.rbCompact.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label5.Location = New System.Drawing.Point(0, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 22)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Architecture"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtFolderPath)
        Me.GroupBox1.Controls.Add(Me.btnBrowse)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(646, 40)
        Me.GroupBox1.TabIndex = 32
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Output Folder"
        '
        'txtFolderPath
        '
        Me.txtFolderPath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFolderPath.Location = New System.Drawing.Point(3, 16)
        Me.txtFolderPath.Name = "txtFolderPath"
        Me.txtFolderPath.Size = New System.Drawing.Size(549, 20)
        Me.txtFolderPath.TabIndex = 3
        '
        'btnBrowse
        '
        Me.btnBrowse.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnBrowse.Location = New System.Drawing.Point(552, 16)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(91, 21)
        Me.btnBrowse.TabIndex = 4
        Me.btnBrowse.Text = "Browse..."
        '
        'gboxTable
        '
        Me.gboxTable.Controls.Add(Me.ctrlClassGen)
        Me.gboxTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxTable.Location = New System.Drawing.Point(0, 0)
        Me.gboxTable.Name = "gboxTable"
        Me.gboxTable.Size = New System.Drawing.Size(714, 644)
        Me.gboxTable.TabIndex = 31
        Me.gboxTable.TabStop = False
        Me.gboxTable.Text = "Generate Code From Table - No Table Selected"
        '
        'ctrlClassGen
        '
        Me.ctrlClassGen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctrlClassGen.Enabled = False
        Me.ctrlClassGen.Location = New System.Drawing.Point(3, 16)
        Me.ctrlClassGen.Name = "ctrlClassGen"
        Me.ctrlClassGen.Size = New System.Drawing.Size(708, 625)
        Me.ctrlClassGen.Tab = CodeGenerator.UCClassGenerator.ETab.ClassGen
        Me.ctrlClassGen.TabIndex = 0
        '
        'UCOutput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel1)
        Me.Name = "UCOutput"
        Me.Size = New System.Drawing.Size(1364, 644)
        Me.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.pnlFilterTables.ResumeLayout(False)
        Me.pnlTableTextbox.ResumeLayout(False)
        Me.gboxTableName.ResumeLayout(False)
        Me.gboxTableName.PerformLayout()
        Me.gboxProjectSettings.ResumeLayout(False)
        Me.pnlPrefix.ResumeLayout(False)
        Me.pnlPrefix.PerformLayout()
        Me.pnlLanguage.ResumeLayout(False)
        Me.pnlLanguage.PerformLayout()
        Me.pnlAudit.ResumeLayout(False)
        Me.pnlArchitecture.ResumeLayout(False)
        Me.pnlArchitecture.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.gboxTable.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents ctrlTables As CodeGenerator.UCTables
    Friend WithEvents pnlTableTextbox As System.Windows.Forms.Panel
    Friend WithEvents gboxTableName As System.Windows.Forms.GroupBox
    Friend WithEvents txtTableName As System.Windows.Forms.TextBox
    Friend WithEvents cboTables As System.Windows.Forms.ComboBox
    Friend WithEvents btnTestTable As System.Windows.Forms.Button
    Friend WithEvents gboxTable As System.Windows.Forms.GroupBox
    Friend WithEvents ctrlClassGen As CodeGenerator.UCClassGenerator
    Friend WithEvents gboxProjectSettings As System.Windows.Forms.GroupBox
    Friend WithEvents pnlAudit As System.Windows.Forms.Panel
    Friend WithEvents chkUseAuditTrail As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents pnlLanguage As System.Windows.Forms.Panel
    Friend WithEvents txtNamespace As System.Windows.Forms.TextBox
    Friend WithEvents lblNamespace As System.Windows.Forms.Label
    Friend WithEvents rbCSharp As System.Windows.Forms.RadioButton
    Friend WithEvents rbVbNet As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pnlPrefix As System.Windows.Forms.Panel
    Friend WithEvents txtStoredProcPrefix As System.Windows.Forms.TextBox
    Friend WithEvents lblStoredProcPrefix As System.Windows.Forms.Label
    Friend WithEvents txtTablePrefix As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents pnlArchitecture As System.Windows.Forms.Panel
    Friend WithEvents rbStoredProcs As System.Windows.Forms.RadioButton
    Friend WithEvents rbComplete As System.Windows.Forms.RadioButton
    Friend WithEvents rbCompact As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtFolderPath As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents pnlFilterTables As System.Windows.Forms.Panel
    Friend WithEvents btnGenerateAll As System.Windows.Forms.Button
    Friend WithEvents chkFilterTables As System.Windows.Forms.CheckBox

End Class
