<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCUiCodeGen
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.gboxColumns = New System.Windows.Forms.GroupBox
        Me.lvHtmlColumns = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.gboxHyperlink = New System.Windows.Forms.GroupBox
        Me.cboHyperlink = New System.Windows.Forms.ComboBox
        Me.gboxSecondaryKeyTable = New System.Windows.Forms.GroupBox
        Me.cboSecondary = New System.Windows.Forms.ComboBox
        Me.gboxPrimaryKeyTable = New System.Windows.Forms.GroupBox
        Me.cboPrimary = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnWriteFile = New System.Windows.Forms.Button
        Me.rbUrls = New System.Windows.Forms.RadioButton
        Me.rbDetails = New System.Windows.Forms.RadioButton
        Me.rbListItem = New System.Windows.Forms.RadioButton
        Me.rbContainer = New System.Windows.Forms.RadioButton
        Me.rbList = New System.Windows.Forms.RadioButton
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rbReadonly = New System.Windows.Forms.RadioButton
        Me.rbEditable = New System.Windows.Forms.RadioButton
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.tabCtrlOutput = New System.Windows.Forms.TabControl
        Me.tabAspx = New System.Windows.Forms.TabPage
        Me.txtAspx = New CodeGenerator.UCCopyAndPaste
        Me.tabVb = New System.Windows.Forms.TabPage
        Me.txtCodeBehind = New CodeGenerator.UCCopyAndPaste
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.txtFolderEditable = New System.Windows.Forms.TextBox
        Me.txtFolderReadonly = New System.Windows.Forms.TextBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.gboxColumns.SuspendLayout()
        Me.gboxHyperlink.SuspendLayout()
        Me.gboxSecondaryKeyTable.SuspendLayout()
        Me.gboxPrimaryKeyTable.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.tabCtrlOutput.SuspendLayout()
        Me.tabAspx.SuspendLayout()
        Me.tabVb.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.gboxColumns)
        Me.SplitContainer1.Panel1.Controls.Add(Me.gboxHyperlink)
        Me.SplitContainer1.Panel1.Controls.Add(Me.gboxSecondaryKeyTable)
        Me.SplitContainer1.Panel1.Controls.Add(Me.gboxPrimaryKeyTable)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox5)
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox4)
        Me.SplitContainer1.Size = New System.Drawing.Size(752, 449)
        Me.SplitContainer1.SplitterDistance = 203
        Me.SplitContainer1.TabIndex = 0
        '
        'gboxColumns
        '
        Me.gboxColumns.Controls.Add(Me.lvHtmlColumns)
        Me.gboxColumns.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxColumns.Location = New System.Drawing.Point(0, 296)
        Me.gboxColumns.Name = "gboxColumns"
        Me.gboxColumns.Size = New System.Drawing.Size(203, 153)
        Me.gboxColumns.TabIndex = 17
        Me.gboxColumns.TabStop = False
        Me.gboxColumns.Text = "Include Columns"
        '
        'lvHtmlColumns
        '
        Me.lvHtmlColumns.CheckBoxes = True
        Me.lvHtmlColumns.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lvHtmlColumns.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvHtmlColumns.GridLines = True
        Me.lvHtmlColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvHtmlColumns.Location = New System.Drawing.Point(3, 16)
        Me.lvHtmlColumns.Name = "lvHtmlColumns"
        Me.lvHtmlColumns.Size = New System.Drawing.Size(197, 134)
        Me.lvHtmlColumns.TabIndex = 9
        Me.lvHtmlColumns.UseCompatibleStateImageBehavior = False
        Me.lvHtmlColumns.View = System.Windows.Forms.View.List
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Repeat for Columns"
        Me.ColumnHeader1.Width = 250
        '
        'gboxHyperlink
        '
        Me.gboxHyperlink.Controls.Add(Me.cboHyperlink)
        Me.gboxHyperlink.Dock = System.Windows.Forms.DockStyle.Top
        Me.gboxHyperlink.Location = New System.Drawing.Point(0, 256)
        Me.gboxHyperlink.Name = "gboxHyperlink"
        Me.gboxHyperlink.Size = New System.Drawing.Size(203, 40)
        Me.gboxHyperlink.TabIndex = 18
        Me.gboxHyperlink.TabStop = False
        Me.gboxHyperlink.Text = "Hyperlink Column"
        '
        'cboHyperlink
        '
        Me.cboHyperlink.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboHyperlink.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHyperlink.FormattingEnabled = True
        Me.cboHyperlink.Location = New System.Drawing.Point(3, 16)
        Me.cboHyperlink.Name = "cboHyperlink"
        Me.cboHyperlink.Size = New System.Drawing.Size(197, 21)
        Me.cboHyperlink.TabIndex = 0
        '
        'gboxSecondaryKeyTable
        '
        Me.gboxSecondaryKeyTable.Controls.Add(Me.cboSecondary)
        Me.gboxSecondaryKeyTable.Dock = System.Windows.Forms.DockStyle.Top
        Me.gboxSecondaryKeyTable.Location = New System.Drawing.Point(0, 216)
        Me.gboxSecondaryKeyTable.Name = "gboxSecondaryKeyTable"
        Me.gboxSecondaryKeyTable.Size = New System.Drawing.Size(203, 40)
        Me.gboxSecondaryKeyTable.TabIndex = 20
        Me.gboxSecondaryKeyTable.TabStop = False
        Me.gboxSecondaryKeyTable.Text = "Related Table #2"
        Me.gboxSecondaryKeyTable.Visible = False
        '
        'cboSecondary
        '
        Me.cboSecondary.DisplayMember = "ClassName"
        Me.cboSecondary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboSecondary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSecondary.FormattingEnabled = True
        Me.cboSecondary.Location = New System.Drawing.Point(3, 16)
        Me.cboSecondary.Name = "cboSecondary"
        Me.cboSecondary.Size = New System.Drawing.Size(197, 21)
        Me.cboSecondary.TabIndex = 0
        '
        'gboxPrimaryKeyTable
        '
        Me.gboxPrimaryKeyTable.Controls.Add(Me.cboPrimary)
        Me.gboxPrimaryKeyTable.Dock = System.Windows.Forms.DockStyle.Top
        Me.gboxPrimaryKeyTable.Location = New System.Drawing.Point(0, 176)
        Me.gboxPrimaryKeyTable.Name = "gboxPrimaryKeyTable"
        Me.gboxPrimaryKeyTable.Size = New System.Drawing.Size(203, 40)
        Me.gboxPrimaryKeyTable.TabIndex = 19
        Me.gboxPrimaryKeyTable.TabStop = False
        Me.gboxPrimaryKeyTable.Text = "Related Table #1"
        Me.gboxPrimaryKeyTable.Visible = False
        '
        'cboPrimary
        '
        Me.cboPrimary.DisplayMember = "ClassName"
        Me.cboPrimary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboPrimary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPrimary.FormattingEnabled = True
        Me.cboPrimary.Location = New System.Drawing.Point(3, 16)
        Me.cboPrimary.Name = "cboPrimary"
        Me.cboPrimary.Size = New System.Drawing.Size(197, 21)
        Me.cboPrimary.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnWriteFile)
        Me.GroupBox1.Controls.Add(Me.rbUrls)
        Me.GroupBox1.Controls.Add(Me.rbListItem)
        Me.GroupBox1.Controls.Add(Me.rbContainer)
        Me.GroupBox1.Controls.Add(Me.rbList)
        Me.GroupBox1.Controls.Add(Me.rbDetails)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 42)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(203, 134)
        Me.GroupBox1.TabIndex = 11
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Template"
        '
        'btnWriteFile
        '
        Me.btnWriteFile.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnWriteFile.Location = New System.Drawing.Point(3, 109)
        Me.btnWriteFile.Name = "btnWriteFile"
        Me.btnWriteFile.Size = New System.Drawing.Size(197, 22)
        Me.btnWriteFile.TabIndex = 18
        Me.btnWriteFile.Text = "Generate Files"
        Me.btnWriteFile.UseVisualStyleBackColor = True
        '
        'rbUrls
        '
        Me.rbUrls.AutoSize = True
        Me.rbUrls.Dock = System.Windows.Forms.DockStyle.Top
        Me.rbUrls.Location = New System.Drawing.Point(3, 84)
        Me.rbUrls.Name = "rbUrls"
        Me.rbUrls.Size = New System.Drawing.Size(197, 17)
        Me.rbUrls.TabIndex = 16
        Me.rbUrls.Text = "Menu - Sitemap/Urls"
        Me.rbUrls.UseVisualStyleBackColor = True
        '
        'rbDetails
        '
        Me.rbDetails.AutoSize = True
        Me.rbDetails.Checked = True
        Me.rbDetails.Dock = System.Windows.Forms.DockStyle.Top
        Me.rbDetails.Location = New System.Drawing.Point(3, 16)
        Me.rbDetails.Name = "rbDetails"
        Me.rbDetails.Size = New System.Drawing.Size(197, 17)
        Me.rbDetails.TabIndex = 15
        Me.rbDetails.TabStop = True
        Me.rbDetails.Text = "Page - Add/Edit"
        Me.rbDetails.UseVisualStyleBackColor = True
        '
        'rbListItem
        '
        Me.rbListItem.AutoSize = True
        Me.rbListItem.Dock = System.Windows.Forms.DockStyle.Top
        Me.rbListItem.Location = New System.Drawing.Point(3, 67)
        Me.rbListItem.Name = "rbListItem"
        Me.rbListItem.Size = New System.Drawing.Size(197, 17)
        Me.rbListItem.TabIndex = 14
        Me.rbListItem.Text = "UserControl - Item"
        Me.rbListItem.UseVisualStyleBackColor = True
        '
        'rbContainer
        '
        Me.rbContainer.AutoSize = True
        Me.rbContainer.Dock = System.Windows.Forms.DockStyle.Top
        Me.rbContainer.Location = New System.Drawing.Point(3, 50)
        Me.rbContainer.Name = "rbContainer"
        Me.rbContainer.Size = New System.Drawing.Size(197, 17)
        Me.rbContainer.TabIndex = 17
        Me.rbContainer.Text = "UserControl - Container"
        Me.rbContainer.UseVisualStyleBackColor = True
        '
        'rbList
        '
        Me.rbList.AutoSize = True
        Me.rbList.Dock = System.Windows.Forms.DockStyle.Top
        Me.rbList.Location = New System.Drawing.Point(3, 33)
        Me.rbList.Name = "rbList"
        Me.rbList.Size = New System.Drawing.Size(197, 17)
        Me.rbList.TabIndex = 13
        Me.rbList.Text = "Page - List/Search"
        Me.rbList.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbReadonly)
        Me.GroupBox2.Controls.Add(Me.rbEditable)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(203, 42)
        Me.GroupBox2.TabIndex = 12
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Mode"
        '
        'rbReadonly
        '
        Me.rbReadonly.AutoSize = True
        Me.rbReadonly.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbReadonly.Location = New System.Drawing.Point(66, 16)
        Me.rbReadonly.Name = "rbReadonly"
        Me.rbReadonly.Size = New System.Drawing.Size(72, 23)
        Me.rbReadonly.TabIndex = 15
        Me.rbReadonly.Text = "ReadOnly"
        Me.rbReadonly.UseVisualStyleBackColor = True
        '
        'rbEditable
        '
        Me.rbEditable.AutoSize = True
        Me.rbEditable.Checked = True
        Me.rbEditable.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbEditable.Location = New System.Drawing.Point(3, 16)
        Me.rbEditable.Name = "rbEditable"
        Me.rbEditable.Size = New System.Drawing.Size(63, 23)
        Me.rbEditable.TabIndex = 16
        Me.rbEditable.TabStop = True
        Me.rbEditable.Text = "Editable"
        Me.rbEditable.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.tabCtrlOutput)
        Me.GroupBox5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox5.Location = New System.Drawing.Point(0, 41)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(545, 408)
        Me.GroupBox5.TabIndex = 11
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Generated Code (Copy && Paste)"
        '
        'tabCtrlOutput
        '
        Me.tabCtrlOutput.Controls.Add(Me.tabAspx)
        Me.tabCtrlOutput.Controls.Add(Me.tabVb)
        Me.tabCtrlOutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabCtrlOutput.Location = New System.Drawing.Point(3, 16)
        Me.tabCtrlOutput.Name = "tabCtrlOutput"
        Me.tabCtrlOutput.SelectedIndex = 0
        Me.tabCtrlOutput.Size = New System.Drawing.Size(539, 389)
        Me.tabCtrlOutput.TabIndex = 11
        '
        'tabAspx
        '
        Me.tabAspx.Controls.Add(Me.txtAspx)
        Me.tabAspx.Location = New System.Drawing.Point(4, 22)
        Me.tabAspx.Name = "tabAspx"
        Me.tabAspx.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAspx.Size = New System.Drawing.Size(531, 363)
        Me.tabAspx.TabIndex = 0
        Me.tabAspx.Text = "Sample.aspx"
        Me.tabAspx.UseVisualStyleBackColor = True
        '
        'txtAspx
        '
        Me.txtAspx.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtAspx.Location = New System.Drawing.Point(3, 3)
        Me.txtAspx.Name = "txtAspx"
        Me.txtAspx.Size = New System.Drawing.Size(525, 357)
        Me.txtAspx.TabIndex = 11
        '
        'tabVb
        '
        Me.tabVb.Controls.Add(Me.txtCodeBehind)
        Me.tabVb.Location = New System.Drawing.Point(4, 22)
        Me.tabVb.Name = "tabVb"
        Me.tabVb.Padding = New System.Windows.Forms.Padding(3)
        Me.tabVb.Size = New System.Drawing.Size(531, 363)
        Me.tabVb.TabIndex = 1
        Me.tabVb.Text = "Sample.aspx.vb"
        Me.tabVb.UseVisualStyleBackColor = True
        '
        'txtCodeBehind
        '
        Me.txtCodeBehind.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtCodeBehind.Location = New System.Drawing.Point(3, 3)
        Me.txtCodeBehind.Name = "txtCodeBehind"
        Me.txtCodeBehind.Size = New System.Drawing.Size(525, 357)
        Me.txtCodeBehind.TabIndex = 12
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.txtFolderEditable)
        Me.GroupBox4.Controls.Add(Me.txtFolderReadonly)
        Me.GroupBox4.Controls.Add(Me.btnBrowse)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox4.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(545, 41)
        Me.GroupBox4.TabIndex = 12
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Output Folder (website root)"
        '
        'txtFolderEditable
        '
        Me.txtFolderEditable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFolderEditable.Location = New System.Drawing.Point(3, 16)
        Me.txtFolderEditable.Name = "txtFolderEditable"
        Me.txtFolderEditable.Size = New System.Drawing.Size(464, 20)
        Me.txtFolderEditable.TabIndex = 3
        '
        'txtFolderReadonly
        '
        Me.txtFolderReadonly.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFolderReadonly.Location = New System.Drawing.Point(3, 16)
        Me.txtFolderReadonly.Name = "txtFolderReadonly"
        Me.txtFolderReadonly.Size = New System.Drawing.Size(464, 20)
        Me.txtFolderReadonly.TabIndex = 2
        '
        'btnBrowse
        '
        Me.btnBrowse.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnBrowse.Location = New System.Drawing.Point(467, 16)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 22)
        Me.btnBrowse.TabIndex = 1
        Me.btnBrowse.Text = "Browse..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'FolderBrowserDialog1
        '
        Me.FolderBrowserDialog1.ShowNewFolderButton = False
        '
        'UCUiCodeGen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "UCUiCodeGen"
        Me.Size = New System.Drawing.Size(752, 449)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.gboxColumns.ResumeLayout(False)
        Me.gboxHyperlink.ResumeLayout(False)
        Me.gboxSecondaryKeyTable.ResumeLayout(False)
        Me.gboxPrimaryKeyTable.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.tabCtrlOutput.ResumeLayout(False)
        Me.tabAspx.ResumeLayout(False)
        Me.tabVb.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents tabCtrlOutput As System.Windows.Forms.TabControl
    Friend WithEvents tabAspx As System.Windows.Forms.TabPage
    Friend WithEvents txtAspx As UCCopyAndPaste
    Friend WithEvents tabVb As System.Windows.Forms.TabPage
    Friend WithEvents txtCodeBehind As UCCopyAndPaste
    Friend WithEvents gboxColumns As System.Windows.Forms.GroupBox
    Friend WithEvents lvHtmlColumns As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbEditable As System.Windows.Forms.RadioButton
    Friend WithEvents rbReadonly As System.Windows.Forms.RadioButton
    Friend WithEvents rbDetails As System.Windows.Forms.RadioButton
    Friend WithEvents rbListItem As System.Windows.Forms.RadioButton
    Friend WithEvents rbList As System.Windows.Forms.RadioButton
    Friend WithEvents rbUrls As System.Windows.Forms.RadioButton
    Friend WithEvents gboxHyperlink As System.Windows.Forms.GroupBox
    Friend WithEvents cboHyperlink As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents txtFolderReadonly As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtFolderEditable As System.Windows.Forms.TextBox
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents rbContainer As System.Windows.Forms.RadioButton
    Friend WithEvents gboxSecondaryKeyTable As System.Windows.Forms.GroupBox
    Friend WithEvents cboSecondary As System.Windows.Forms.ComboBox
    Friend WithEvents gboxPrimaryKeyTable As System.Windows.Forms.GroupBox
    Friend WithEvents cboPrimary As System.Windows.Forms.ComboBox
    Friend WithEvents btnWriteFile As System.Windows.Forms.Button

End Class
