<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCTemplates
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
        Me.gboxTemplates = New System.Windows.Forms.GroupBox
        Me.tvTemplates = New System.Windows.Forms.TreeView
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.chkModifiedOnly = New System.Windows.Forms.CheckBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.gboxSkin = New System.Windows.Forms.GroupBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ddSkins = New System.Windows.Forms.ComboBox
        Me.btnCreate = New System.Windows.Forms.Button
        Me.btnEdit = New System.Windows.Forms.Button
        Me.lvChildren = New System.Windows.Forms.ListView
        Me.chRelativePath = New System.Windows.Forms.ColumnHeader
        Me.chTemplate = New System.Windows.Forms.ColumnHeader
        Me.gboxTemplate = New System.Windows.Forms.GroupBox
        Me.txtTemplate = New System.Windows.Forms.TextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblRelativePath = New System.Windows.Forms.Label
        Me.btnRestore = New System.Windows.Forms.Button
        Me.btnLock = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.gboxTemplates.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.gboxSkin.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.gboxTemplate.SuspendLayout()
        Me.Panel1.SuspendLayout()
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
        Me.SplitContainer1.Panel1.Controls.Add(Me.gboxTemplates)
        Me.SplitContainer1.Panel1.Controls.Add(Me.gboxSkin)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lvChildren)
        Me.SplitContainer1.Panel2.Controls.Add(Me.gboxTemplate)
        Me.SplitContainer1.Size = New System.Drawing.Size(572, 406)
        Me.SplitContainer1.SplitterDistance = 247
        Me.SplitContainer1.TabIndex = 0
        '
        'gboxTemplates
        '
        Me.gboxTemplates.Controls.Add(Me.tvTemplates)
        Me.gboxTemplates.Controls.Add(Me.Panel4)
        Me.gboxTemplates.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxTemplates.Location = New System.Drawing.Point(0, 39)
        Me.gboxTemplates.Name = "gboxTemplates"
        Me.gboxTemplates.Size = New System.Drawing.Size(247, 367)
        Me.gboxTemplates.TabIndex = 0
        Me.gboxTemplates.TabStop = False
        Me.gboxTemplates.Text = "Templates"
        '
        'tvTemplates
        '
        Me.tvTemplates.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvTemplates.Location = New System.Drawing.Point(3, 36)
        Me.tvTemplates.Name = "tvTemplates"
        Me.tvTemplates.PathSeparator = "/"
        Me.tvTemplates.Size = New System.Drawing.Size(241, 328)
        Me.tvTemplates.TabIndex = 5
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.Panel5)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(3, 16)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(241, 20)
        Me.Panel4.TabIndex = 7
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.txtSearch)
        Me.Panel5.Controls.Add(Me.chkModifiedOnly)
        Me.Panel5.Controls.Add(Me.btnSearch)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(241, 20)
        Me.Panel5.TabIndex = 9
        '
        'txtSearch
        '
        Me.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtSearch.Location = New System.Drawing.Point(172, 0)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(10, 20)
        Me.txtSearch.TabIndex = 7
        '
        'chkModifiedOnly
        '
        Me.chkModifiedOnly.AutoSize = True
        Me.chkModifiedOnly.Dock = System.Windows.Forms.DockStyle.Left
        Me.chkModifiedOnly.Location = New System.Drawing.Point(0, 0)
        Me.chkModifiedOnly.Name = "chkModifiedOnly"
        Me.chkModifiedOnly.Size = New System.Drawing.Size(172, 20)
        Me.chkModifiedOnly.TabIndex = 8
        Me.chkModifiedOnly.Text = "Only Show Modified Templates"
        Me.chkModifiedOnly.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnSearch.Location = New System.Drawing.Point(182, 0)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(59, 20)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'gboxSkin
        '
        Me.gboxSkin.Controls.Add(Me.Panel2)
        Me.gboxSkin.Dock = System.Windows.Forms.DockStyle.Top
        Me.gboxSkin.Location = New System.Drawing.Point(0, 0)
        Me.gboxSkin.Name = "gboxSkin"
        Me.gboxSkin.Size = New System.Drawing.Size(247, 39)
        Me.gboxSkin.TabIndex = 2
        Me.gboxSkin.TabStop = False
        Me.gboxSkin.Text = "Current Skin"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.ddSkins)
        Me.Panel2.Controls.Add(Me.btnCreate)
        Me.Panel2.Controls.Add(Me.btnEdit)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(3, 16)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(241, 22)
        Me.Panel2.TabIndex = 5
        '
        'ddSkins
        '
        Me.ddSkins.DisplayMember = "Name"
        Me.ddSkins.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ddSkins.FormattingEnabled = True
        Me.ddSkins.Location = New System.Drawing.Point(0, 0)
        Me.ddSkins.Name = "ddSkins"
        Me.ddSkins.Size = New System.Drawing.Size(123, 21)
        Me.ddSkins.TabIndex = 8
        '
        'btnCreate
        '
        Me.btnCreate.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnCreate.Location = New System.Drawing.Point(123, 0)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(59, 22)
        Me.btnCreate.TabIndex = 4
        Me.btnCreate.Text = "Create..."
        Me.btnCreate.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnEdit.Location = New System.Drawing.Point(182, 0)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(59, 22)
        Me.btnEdit.TabIndex = 5
        Me.btnEdit.Text = "Edit..."
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'lvChildren
        '
        Me.lvChildren.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chRelativePath, Me.chTemplate})
        Me.lvChildren.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvChildren.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvChildren.FullRowSelect = True
        Me.lvChildren.GridLines = True
        Me.lvChildren.Location = New System.Drawing.Point(0, 0)
        Me.lvChildren.MultiSelect = False
        Me.lvChildren.Name = "lvChildren"
        Me.lvChildren.Size = New System.Drawing.Size(321, 406)
        Me.lvChildren.TabIndex = 2
        Me.lvChildren.UseCompatibleStateImageBehavior = False
        Me.lvChildren.View = System.Windows.Forms.View.List
        Me.lvChildren.Visible = False
        '
        'chRelativePath
        '
        Me.chRelativePath.Text = "Path"
        Me.chRelativePath.Width = 500
        '
        'chTemplate
        '
        Me.chTemplate.Text = "Template"
        Me.chTemplate.Width = 500
        '
        'gboxTemplate
        '
        Me.gboxTemplate.Controls.Add(Me.txtTemplate)
        Me.gboxTemplate.Controls.Add(Me.Panel1)
        Me.gboxTemplate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxTemplate.Location = New System.Drawing.Point(0, 0)
        Me.gboxTemplate.Name = "gboxTemplate"
        Me.gboxTemplate.Size = New System.Drawing.Size(321, 406)
        Me.gboxTemplate.TabIndex = 1
        Me.gboxTemplate.TabStop = False
        Me.gboxTemplate.Text = "Selected Template"
        '
        'txtTemplate
        '
        Me.txtTemplate.AcceptsReturn = True
        Me.txtTemplate.AcceptsTab = True
        Me.txtTemplate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTemplate.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTemplate.Location = New System.Drawing.Point(3, 37)
        Me.txtTemplate.Multiline = True
        Me.txtTemplate.Name = "txtTemplate"
        Me.txtTemplate.ReadOnly = True
        Me.txtTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtTemplate.Size = New System.Drawing.Size(315, 366)
        Me.txtTemplate.TabIndex = 1
        Me.txtTemplate.WordWrap = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblRelativePath)
        Me.Panel1.Controls.Add(Me.btnRestore)
        Me.Panel1.Controls.Add(Me.btnLock)
        Me.Panel1.Controls.Add(Me.btnSave)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 16)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(315, 21)
        Me.Panel1.TabIndex = 0
        '
        'lblRelativePath
        '
        Me.lblRelativePath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblRelativePath.Location = New System.Drawing.Point(0, 0)
        Me.lblRelativePath.Name = "lblRelativePath"
        Me.lblRelativePath.Size = New System.Drawing.Size(79, 21)
        Me.lblRelativePath.TabIndex = 10
        Me.lblRelativePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnRestore
        '
        Me.btnRestore.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnRestore.Location = New System.Drawing.Point(79, 0)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(59, 21)
        Me.btnRestore.TabIndex = 9
        Me.btnRestore.Text = "Restore"
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'btnLock
        '
        Me.btnLock.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnLock.Location = New System.Drawing.Point(138, 0)
        Me.btnLock.Name = "btnLock"
        Me.btnLock.Size = New System.Drawing.Size(59, 21)
        Me.btnLock.TabIndex = 6
        Me.btnLock.Text = "Edit"
        Me.btnLock.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(197, 0)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(59, 21)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnCancel.Enabled = False
        Me.btnCancel.Location = New System.Drawing.Point(256, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(59, 21)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'UCTemplates
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "UCTemplates"
        Me.Size = New System.Drawing.Size(572, 406)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.gboxTemplates.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.gboxSkin.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.gboxTemplate.ResumeLayout(False)
        Me.gboxTemplate.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents gboxTemplates As System.Windows.Forms.GroupBox
    Friend WithEvents gboxTemplate As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtTemplate As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnLock As System.Windows.Forms.Button
    Friend WithEvents gboxSkin As System.Windows.Forms.GroupBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ddSkins As System.Windows.Forms.ComboBox
    Friend WithEvents btnCreate As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnRestore As System.Windows.Forms.Button
    Friend WithEvents tvTemplates As System.Windows.Forms.TreeView
    Friend WithEvents lblRelativePath As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents lvChildren As System.Windows.Forms.ListView
    Friend WithEvents chRelativePath As System.Windows.Forms.ColumnHeader
    Friend WithEvents chTemplate As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkModifiedOnly As System.Windows.Forms.CheckBox

End Class
