<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCClassGenerator
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
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.cboThirdKey = New System.Windows.Forms.ComboBox
        Me.chk3Way = New System.Windows.Forms.CheckBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.cboSecondKey = New System.Windows.Forms.ComboBox
        Me.chkM2M = New System.Windows.Forms.CheckBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.cboPrimaryKey = New System.Windows.Forms.ComboBox
        Me.chkAutoIncrement = New System.Windows.Forms.CheckBox
        Me.gboxOptional = New System.Windows.Forms.GroupBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cboFilters = New System.Windows.Forms.ListBox
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.txtViewName = New System.Windows.Forms.TextBox
        Me.chkView = New System.Windows.Forms.CheckBox
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.txtOrderBy = New System.Windows.Forms.TextBox
        Me.chkOrderBy = New System.Windows.Forms.CheckBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.cboSortingColumn = New System.Windows.Forms.ComboBox
        Me.chkHasSortColumn = New System.Windows.Forms.CheckBox
        Me.Panel7 = New System.Windows.Forms.Panel
        Me.chkCaching = New System.Windows.Forms.CheckBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtClassName = New System.Windows.Forms.TextBox
        Me.btnGenerate = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tpMain = New System.Windows.Forms.TabPage
        Me.tpRelationships = New System.Windows.Forms.TabPage
        Me.UcRelationships1 = New CodeGenerator.UCRelationships
        Me.tbSorting = New System.Windows.Forms.TabPage
        Me.UcSorting1 = New CodeGenerator.UCSorting
        Me.tpUI = New System.Windows.Forms.TabPage
        Me.UcUiCodeGen1 = New CodeGenerator.UCUiCodeGen
        Me.GroupBox4.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.gboxOptional.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tpMain.SuspendLayout()
        Me.tpRelationships.SuspendLayout()
        Me.tbSorting.SuspendLayout()
        Me.tpUI.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Panel4)
        Me.GroupBox4.Controls.Add(Me.Panel3)
        Me.GroupBox4.Controls.Add(Me.Panel2)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox4.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(759, 90)
        Me.GroupBox4.TabIndex = 28
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Primary Key(s)"
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.cboThirdKey)
        Me.Panel4.Controls.Add(Me.chk3Way)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(3, 64)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(753, 24)
        Me.Panel4.TabIndex = 2
        '
        'cboThirdKey
        '
        Me.cboThirdKey.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboThirdKey.Enabled = False
        Me.cboThirdKey.Location = New System.Drawing.Point(106, 0)
        Me.cboThirdKey.Name = "cboThirdKey"
        Me.cboThirdKey.Size = New System.Drawing.Size(647, 21)
        Me.cboThirdKey.TabIndex = 10
        '
        'chk3Way
        '
        Me.chk3Way.Dock = System.Windows.Forms.DockStyle.Left
        Me.chk3Way.Location = New System.Drawing.Point(0, 0)
        Me.chk3Way.Name = "chk3Way"
        Me.chk3Way.Size = New System.Drawing.Size(106, 24)
        Me.chk3Way.TabIndex = 9
        Me.chk3Way.Text = "3-Way Key"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.cboSecondKey)
        Me.Panel3.Controls.Add(Me.chkM2M)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(3, 40)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(753, 24)
        Me.Panel3.TabIndex = 1
        '
        'cboSecondKey
        '
        Me.cboSecondKey.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboSecondKey.Enabled = False
        Me.cboSecondKey.Location = New System.Drawing.Point(106, 0)
        Me.cboSecondKey.Name = "cboSecondKey"
        Me.cboSecondKey.Size = New System.Drawing.Size(647, 21)
        Me.cboSecondKey.TabIndex = 7
        '
        'chkM2M
        '
        Me.chkM2M.Dock = System.Windows.Forms.DockStyle.Left
        Me.chkM2M.Location = New System.Drawing.Point(0, 0)
        Me.chkM2M.Name = "chkM2M"
        Me.chkM2M.Size = New System.Drawing.Size(106, 24)
        Me.chkM2M.TabIndex = 8
        Me.chkM2M.Text = "Many 2 Many"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cboPrimaryKey)
        Me.Panel2.Controls.Add(Me.chkAutoIncrement)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(3, 16)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(753, 24)
        Me.Panel2.TabIndex = 0
        '
        'cboPrimaryKey
        '
        Me.cboPrimaryKey.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboPrimaryKey.Location = New System.Drawing.Point(106, 0)
        Me.cboPrimaryKey.Name = "cboPrimaryKey"
        Me.cboPrimaryKey.Size = New System.Drawing.Size(647, 21)
        Me.cboPrimaryKey.TabIndex = 5
        '
        'chkAutoIncrement
        '
        Me.chkAutoIncrement.Checked = True
        Me.chkAutoIncrement.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoIncrement.Dock = System.Windows.Forms.DockStyle.Left
        Me.chkAutoIncrement.Location = New System.Drawing.Point(0, 0)
        Me.chkAutoIncrement.Name = "chkAutoIncrement"
        Me.chkAutoIncrement.Size = New System.Drawing.Size(106, 24)
        Me.chkAutoIncrement.TabIndex = 6
        Me.chkAutoIncrement.Text = "AutoIncrement"
        '
        'gboxOptional
        '
        Me.gboxOptional.Controls.Add(Me.GroupBox1)
        Me.gboxOptional.Controls.Add(Me.Panel5)
        Me.gboxOptional.Controls.Add(Me.Panel6)
        Me.gboxOptional.Controls.Add(Me.Panel1)
        Me.gboxOptional.Controls.Add(Me.Panel7)
        Me.gboxOptional.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxOptional.Location = New System.Drawing.Point(3, 93)
        Me.gboxOptional.Name = "gboxOptional"
        Me.gboxOptional.Size = New System.Drawing.Size(759, 303)
        Me.gboxOptional.TabIndex = 29
        Me.gboxOptional.TabStop = False
        Me.gboxOptional.Text = "Optional"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cboFilters)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(3, 107)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(753, 193)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filter Columns (GetBy/SelectWhere)"
        '
        'cboFilters
        '
        Me.cboFilters.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboFilters.FormattingEnabled = True
        Me.cboFilters.Location = New System.Drawing.Point(3, 16)
        Me.cboFilters.Name = "cboFilters"
        Me.cboFilters.ScrollAlwaysVisible = True
        Me.cboFilters.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.cboFilters.Size = New System.Drawing.Size(747, 173)
        Me.cboFilters.TabIndex = 14
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.txtViewName)
        Me.Panel5.Controls.Add(Me.chkView)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(3, 83)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(753, 24)
        Me.Panel5.TabIndex = 0
        '
        'txtViewName
        '
        Me.txtViewName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtViewName.Enabled = False
        Me.txtViewName.Location = New System.Drawing.Point(173, 0)
        Me.txtViewName.Name = "txtViewName"
        Me.txtViewName.Size = New System.Drawing.Size(580, 20)
        Me.txtViewName.TabIndex = 10
        '
        'chkView
        '
        Me.chkView.Dock = System.Windows.Forms.DockStyle.Left
        Me.chkView.Location = New System.Drawing.Point(0, 0)
        Me.chkView.Name = "chkView"
        Me.chkView.Size = New System.Drawing.Size(173, 24)
        Me.chkView.TabIndex = 9
        Me.chkView.Text = "Use View For Selects"
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.txtOrderBy)
        Me.Panel6.Controls.Add(Me.chkOrderBy)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel6.Location = New System.Drawing.Point(3, 59)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(753, 24)
        Me.Panel6.TabIndex = 1
        '
        'txtOrderBy
        '
        Me.txtOrderBy.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtOrderBy.Location = New System.Drawing.Point(173, 0)
        Me.txtOrderBy.Name = "txtOrderBy"
        Me.txtOrderBy.Size = New System.Drawing.Size(580, 20)
        Me.txtOrderBy.TabIndex = 12
        '
        'chkOrderBy
        '
        Me.chkOrderBy.Checked = True
        Me.chkOrderBy.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOrderBy.Dock = System.Windows.Forms.DockStyle.Left
        Me.chkOrderBy.Location = New System.Drawing.Point(0, 0)
        Me.chkOrderBy.Name = "chkOrderBy"
        Me.chkOrderBy.Size = New System.Drawing.Size(173, 24)
        Me.chkOrderBy.TabIndex = 11
        Me.chkOrderBy.Text = "Default Order-By Column(s)"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cboSortingColumn)
        Me.Panel1.Controls.Add(Me.chkHasSortColumn)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 35)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(753, 24)
        Me.Panel1.TabIndex = 16
        '
        'cboSortingColumn
        '
        Me.cboSortingColumn.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboSortingColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSortingColumn.Enabled = False
        Me.cboSortingColumn.Location = New System.Drawing.Point(173, 0)
        Me.cboSortingColumn.Name = "cboSortingColumn"
        Me.cboSortingColumn.Size = New System.Drawing.Size(580, 21)
        Me.cboSortingColumn.TabIndex = 10
        '
        'chkHasSortColumn
        '
        Me.chkHasSortColumn.Dock = System.Windows.Forms.DockStyle.Left
        Me.chkHasSortColumn.Location = New System.Drawing.Point(0, 0)
        Me.chkHasSortColumn.Name = "chkHasSortColumn"
        Me.chkHasSortColumn.Size = New System.Drawing.Size(173, 24)
        Me.chkHasSortColumn.TabIndex = 9
        Me.chkHasSortColumn.Text = "Contains a Sorting Column"
        '
        'Panel7
        '
        Me.Panel7.Controls.Add(Me.chkCaching)
        Me.Panel7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel7.Location = New System.Drawing.Point(3, 16)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(753, 19)
        Me.Panel7.TabIndex = 15
        '
        'chkCaching
        '
        Me.chkCaching.Checked = True
        Me.chkCaching.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCaching.Location = New System.Drawing.Point(0, 0)
        Me.chkCaching.Name = "chkCaching"
        Me.chkCaching.Size = New System.Drawing.Size(397, 19)
        Me.chkCaching.TabIndex = 0
        Me.chkCaching.Text = "Use Caching  - Used if table is small, has static data, or is read frequently"
        Me.chkCaching.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtClassName)
        Me.GroupBox2.Controls.Add(Me.btnGenerate)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox2.Location = New System.Drawing.Point(3, 396)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(759, 45)
        Me.GroupBox2.TabIndex = 30
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Class Name"
        '
        'txtClassName
        '
        Me.txtClassName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtClassName.Location = New System.Drawing.Point(3, 16)
        Me.txtClassName.Name = "txtClassName"
        Me.txtClassName.Size = New System.Drawing.Size(643, 20)
        Me.txtClassName.TabIndex = 14
        '
        'btnGenerate
        '
        Me.btnGenerate.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnGenerate.Enabled = False
        Me.btnGenerate.Location = New System.Drawing.Point(646, 16)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(110, 26)
        Me.btnGenerate.TabIndex = 15
        Me.btnGenerate.Text = "Generate Classes"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tpMain)
        Me.TabControl1.Controls.Add(Me.tpRelationships)
        Me.TabControl1.Controls.Add(Me.tbSorting)
        Me.TabControl1.Controls.Add(Me.tpUI)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(773, 470)
        Me.TabControl1.TabIndex = 31
        '
        'tpMain
        '
        Me.tpMain.Controls.Add(Me.gboxOptional)
        Me.tpMain.Controls.Add(Me.GroupBox2)
        Me.tpMain.Controls.Add(Me.GroupBox4)
        Me.tpMain.Location = New System.Drawing.Point(4, 22)
        Me.tpMain.Name = "tpMain"
        Me.tpMain.Padding = New System.Windows.Forms.Padding(3)
        Me.tpMain.Size = New System.Drawing.Size(765, 444)
        Me.tpMain.TabIndex = 0
        Me.tpMain.Text = "Class Generator"
        Me.tpMain.UseVisualStyleBackColor = True
        '
        'tpRelationships
        '
        Me.tpRelationships.Controls.Add(Me.UcRelationships1)
        Me.tpRelationships.Location = New System.Drawing.Point(4, 22)
        Me.tpRelationships.Name = "tpRelationships"
        Me.tpRelationships.Size = New System.Drawing.Size(765, 444)
        Me.tpRelationships.TabIndex = 2
        Me.tpRelationships.Text = "Relationship-Mapping"
        Me.tpRelationships.UseVisualStyleBackColor = True
        '
        'UcRelationships1
        '
        Me.UcRelationships1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcRelationships1.IsFks = True
        Me.UcRelationships1.Location = New System.Drawing.Point(0, 0)
        Me.UcRelationships1.MyColumn = Nothing
        Me.UcRelationships1.Name = "UcRelationships1"
        Me.UcRelationships1.OtherFKColumn = ""
        Me.UcRelationships1.OtherTable = Nothing
        Me.UcRelationships1.Pattern = CodeGenerator.UCRelationships.EPattern.OnDemand
        Me.UcRelationships1.ShowList = False
        Me.UcRelationships1.Size = New System.Drawing.Size(765, 444)
        Me.UcRelationships1.TabIndex = 0
        '
        'tbSorting
        '
        Me.tbSorting.Controls.Add(Me.UcSorting1)
        Me.tbSorting.Location = New System.Drawing.Point(4, 22)
        Me.tbSorting.Name = "tbSorting"
        Me.tbSorting.Size = New System.Drawing.Size(765, 444)
        Me.tbSorting.TabIndex = 3
        Me.tbSorting.Text = "Alternative Sorting"
        Me.tbSorting.UseVisualStyleBackColor = True
        '
        'UcSorting1
        '
        Me.UcSorting1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcSorting1.Location = New System.Drawing.Point(0, 0)
        Me.UcSorting1.Name = "UcSorting1"
        Me.UcSorting1.Size = New System.Drawing.Size(765, 444)
        Me.UcSorting1.TabIndex = 0
        '
        'tpUI
        '
        Me.tpUI.Controls.Add(Me.UcUiCodeGen1)
        Me.tpUI.Location = New System.Drawing.Point(4, 22)
        Me.tpUI.Name = "tpUI"
        Me.tpUI.Padding = New System.Windows.Forms.Padding(3)
        Me.tpUI.Size = New System.Drawing.Size(765, 444)
        Me.tpUI.TabIndex = 1
        Me.tpUI.Text = "Asp.Net"
        Me.tpUI.UseVisualStyleBackColor = True
        '
        'UcUiCodeGen1
        '
        Me.UcUiCodeGen1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcUiCodeGen1.Location = New System.Drawing.Point(3, 3)
        Me.UcUiCodeGen1.Name = "UcUiCodeGen1"
        Me.UcUiCodeGen1.OutputFolderEditable = ""
        Me.UcUiCodeGen1.OutputFolderReadOnly = ""
        Me.UcUiCodeGen1.Size = New System.Drawing.Size(759, 438)
        Me.UcUiCodeGen1.TabIndex = 0
        '
        'UCClassGenerator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "UCClassGenerator"
        Me.Size = New System.Drawing.Size(773, 470)
        Me.GroupBox4.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.gboxOptional.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.tpMain.ResumeLayout(False)
        Me.tpRelationships.ResumeLayout(False)
        Me.tbSorting.ResumeLayout(False)
        Me.tpUI.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents cboThirdKey As System.Windows.Forms.ComboBox
    Friend WithEvents chk3Way As System.Windows.Forms.CheckBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents cboSecondKey As System.Windows.Forms.ComboBox
    Friend WithEvents chkM2M As System.Windows.Forms.CheckBox
    Friend WithEvents cboPrimaryKey As System.Windows.Forms.ComboBox
    Friend WithEvents chkAutoIncrement As System.Windows.Forms.CheckBox
    Friend WithEvents gboxOptional As System.Windows.Forms.GroupBox
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents chkCaching As System.Windows.Forms.CheckBox
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents txtViewName As System.Windows.Forms.TextBox
    Friend WithEvents chkView As System.Windows.Forms.CheckBox
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents txtOrderBy As System.Windows.Forms.TextBox
    Friend WithEvents chkOrderBy As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtClassName As System.Windows.Forms.TextBox
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tpMain As System.Windows.Forms.TabPage
    Friend WithEvents tpUI As System.Windows.Forms.TabPage
    Friend WithEvents tpRelationships As System.Windows.Forms.TabPage
    Friend WithEvents UcRelationships1 As CodeGenerator.UCRelationships
    Friend WithEvents tbSorting As System.Windows.Forms.TabPage
    Friend WithEvents UcSorting1 As CodeGenerator.UCSorting
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents UcUiCodeGen1 As CodeGenerator.UCUiCodeGen
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cboSortingColumn As System.Windows.Forms.ComboBox
    Friend WithEvents chkHasSortColumn As System.Windows.Forms.CheckBox
    Friend WithEvents cboFilters As System.Windows.Forms.ListBox

End Class
