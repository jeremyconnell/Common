<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCRelationships
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lnkInvert = New System.Windows.Forms.LinkLabel
        Me.rbChildren = New System.Windows.Forms.RadioButton
        Me.rbFKs = New System.Windows.Forms.RadioButton
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.Splitter2 = New System.Windows.Forms.Splitter
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Splitter3 = New System.Windows.Forms.Splitter
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.pnlRight = New System.Windows.Forms.Panel
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.lvOtherColumns = New System.Windows.Forms.ListView
        Me.gbox3rdTable = New System.Windows.Forms.GroupBox
        Me.pnlLeft = New System.Windows.Forms.Panel
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lvMyColumns = New System.Windows.Forms.ListView
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.gboxCustomisation = New System.Windows.Forms.GroupBox
        Me.gboxCustomisationList = New System.Windows.Forms.GroupBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.rbPatternOnDemand = New System.Windows.Forms.RadioButton
        Me.rbPatternMember = New System.Windows.Forms.RadioButton
        Me.rbPatternCached = New System.Windows.Forms.RadioButton
        Me.ctrlTables = New CodeGenerator.UCTables
        Me.ctrl3rdTable = New CodeGenerator.UCTables
        Me.txtCustom = New CodeGenerator.UCCopyAndPaste
        Me.txtCustomList = New CodeGenerator.UCCopyAndPaste
        Me.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.pnlRight.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.gbox3rdTable.SuspendLayout()
        Me.pnlLeft.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.gboxCustomisation.SuspendLayout()
        Me.gboxCustomisationList.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lnkInvert)
        Me.Panel1.Controls.Add(Me.rbChildren)
        Me.Panel1.Controls.Add(Me.rbFKs)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(592, 24)
        Me.Panel1.TabIndex = 0
        '
        'lnkInvert
        '
        Me.lnkInvert.AutoSize = True
        Me.lnkInvert.Dock = System.Windows.Forms.DockStyle.Right
        Me.lnkInvert.Location = New System.Drawing.Point(499, 0)
        Me.lnkInvert.Name = "lnkInvert"
        Me.lnkInvert.Size = New System.Drawing.Size(93, 13)
        Me.lnkInvert.TabIndex = 2
        Me.lnkInvert.TabStop = True
        Me.lnkInvert.Text = "Invert Perspective"
        '
        'rbChildren
        '
        Me.rbChildren.AutoSize = True
        Me.rbChildren.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbChildren.Location = New System.Drawing.Point(139, 0)
        Me.rbChildren.Name = "rbChildren"
        Me.rbChildren.Size = New System.Drawing.Size(158, 24)
        Me.rbChildren.TabIndex = 1
        Me.rbChildren.Text = "Child Collections (Other FKs)"
        Me.rbChildren.UseVisualStyleBackColor = True
        '
        'rbFKs
        '
        Me.rbFKs.AutoSize = True
        Me.rbFKs.Checked = True
        Me.rbFKs.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbFKs.Location = New System.Drawing.Point(0, 0)
        Me.rbFKs.Name = "rbFKs"
        Me.rbFKs.Size = New System.Drawing.Size(139, 24)
        Me.rbFKs.TabIndex = 0
        Me.rbFKs.TabStop = True
        Me.rbFKs.Text = "Parent Objects (My FKs)"
        Me.rbFKs.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Splitter2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Splitter1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.pnlLeft)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox1)
        Me.SplitContainer1.Size = New System.Drawing.Size(592, 389)
        Me.SplitContainer1.SplitterDistance = 197
        Me.SplitContainer1.TabIndex = 1
        '
        'Splitter2
        '
        Me.Splitter2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Splitter2.Location = New System.Drawing.Point(589, 0)
        Me.Splitter2.Name = "Splitter2"
        Me.Splitter2.Size = New System.Drawing.Size(3, 197)
        Me.Splitter2.TabIndex = 4
        Me.Splitter2.TabStop = False
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(201, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 197)
        Me.Splitter1.TabIndex = 3
        Me.Splitter1.TabStop = False
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.Splitter3)
        Me.Panel4.Controls.Add(Me.GroupBox4)
        Me.Panel4.Controls.Add(Me.pnlRight)
        Me.Panel4.Controls.Add(Me.gbox3rdTable)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(201, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(391, 197)
        Me.Panel4.TabIndex = 2
        '
        'Splitter3
        '
        Me.Splitter3.Location = New System.Drawing.Point(0, 0)
        Me.Splitter3.Name = "Splitter3"
        Me.Splitter3.Size = New System.Drawing.Size(3, 197)
        Me.Splitter3.TabIndex = 5
        Me.Splitter3.TabStop = False
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.ctrlTables)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox4.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(50, 197)
        Me.GroupBox4.TabIndex = 3
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Select Related Table"
        '
        'pnlRight
        '
        Me.pnlRight.Controls.Add(Me.GroupBox5)
        Me.pnlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlRight.Location = New System.Drawing.Point(50, 0)
        Me.pnlRight.Name = "pnlRight"
        Me.pnlRight.Size = New System.Drawing.Size(180, 197)
        Me.pnlRight.TabIndex = 4
        Me.pnlRight.Visible = False
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.lvOtherColumns)
        Me.GroupBox5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox5.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(180, 197)
        Me.GroupBox5.TabIndex = 3
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Select Foreign Key"
        '
        'lvOtherColumns
        '
        Me.lvOtherColumns.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvOtherColumns.GridLines = True
        Me.lvOtherColumns.HideSelection = False
        Me.lvOtherColumns.LabelWrap = False
        Me.lvOtherColumns.Location = New System.Drawing.Point(3, 16)
        Me.lvOtherColumns.MultiSelect = False
        Me.lvOtherColumns.Name = "lvOtherColumns"
        Me.lvOtherColumns.Size = New System.Drawing.Size(174, 178)
        Me.lvOtherColumns.TabIndex = 2
        Me.lvOtherColumns.UseCompatibleStateImageBehavior = False
        Me.lvOtherColumns.View = System.Windows.Forms.View.List
        '
        'gbox3rdTable
        '
        Me.gbox3rdTable.Controls.Add(Me.ctrl3rdTable)
        Me.gbox3rdTable.Dock = System.Windows.Forms.DockStyle.Right
        Me.gbox3rdTable.Location = New System.Drawing.Point(230, 0)
        Me.gbox3rdTable.Name = "gbox3rdTable"
        Me.gbox3rdTable.Size = New System.Drawing.Size(161, 197)
        Me.gbox3rdTable.TabIndex = 6
        Me.gbox3rdTable.TabStop = False
        Me.gbox3rdTable.Text = "3rd Table (Associative)"
        Me.gbox3rdTable.Visible = False
        '
        'pnlLeft
        '
        Me.pnlLeft.Controls.Add(Me.GroupBox2)
        Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlLeft.Location = New System.Drawing.Point(0, 0)
        Me.pnlLeft.Name = "pnlLeft"
        Me.pnlLeft.Size = New System.Drawing.Size(201, 197)
        Me.pnlLeft.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lvMyColumns)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(201, 197)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Select Foreign Key"
        '
        'lvMyColumns
        '
        Me.lvMyColumns.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvMyColumns.GridLines = True
        Me.lvMyColumns.HideSelection = False
        Me.lvMyColumns.LabelWrap = False
        Me.lvMyColumns.Location = New System.Drawing.Point(3, 16)
        Me.lvMyColumns.MultiSelect = False
        Me.lvMyColumns.Name = "lvMyColumns"
        Me.lvMyColumns.Size = New System.Drawing.Size(195, 178)
        Me.lvMyColumns.TabIndex = 2
        Me.lvMyColumns.UseCompatibleStateImageBehavior = False
        Me.lvMyColumns.View = System.Windows.Forms.View.List
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.SplitContainer2)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(592, 188)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Generated Code (Copy && Paste)"
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(3, 16)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.gboxCustomisation)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.gboxCustomisationList)
        Me.SplitContainer2.Size = New System.Drawing.Size(586, 133)
        Me.SplitContainer2.SplitterDistance = 300
        Me.SplitContainer2.TabIndex = 4
        '
        'gboxCustomisation
        '
        Me.gboxCustomisation.Controls.Add(Me.txtCustom)
        Me.gboxCustomisation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxCustomisation.Location = New System.Drawing.Point(0, 0)
        Me.gboxCustomisation.Name = "gboxCustomisation"
        Me.gboxCustomisation.Size = New System.Drawing.Size(300, 133)
        Me.gboxCustomisation.TabIndex = 3
        Me.gboxCustomisation.TabStop = False
        Me.gboxCustomisation.Text = "CEvent.customisation.vb"
        '
        'gboxCustomisationList
        '
        Me.gboxCustomisationList.Controls.Add(Me.txtCustomList)
        Me.gboxCustomisationList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxCustomisationList.Location = New System.Drawing.Point(0, 0)
        Me.gboxCustomisationList.Name = "gboxCustomisationList"
        Me.gboxCustomisationList.Size = New System.Drawing.Size(282, 133)
        Me.gboxCustomisationList.TabIndex = 4
        Me.gboxCustomisationList.TabStop = False
        Me.gboxCustomisationList.Text = "CEventList.customisation.vb"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.rbPatternOnDemand)
        Me.GroupBox3.Controls.Add(Me.rbPatternMember)
        Me.GroupBox3.Controls.Add(Me.rbPatternCached)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox3.Location = New System.Drawing.Point(3, 149)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(586, 36)
        Me.GroupBox3.TabIndex = 5
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Design Pattern (based on caching policies):"
        '
        'rbPatternOnDemand
        '
        Me.rbPatternOnDemand.AutoSize = True
        Me.rbPatternOnDemand.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbPatternOnDemand.Enabled = False
        Me.rbPatternOnDemand.Location = New System.Drawing.Point(270, 16)
        Me.rbPatternOnDemand.Name = "rbPatternOnDemand"
        Me.rbPatternOnDemand.Size = New System.Drawing.Size(148, 17)
        Me.rbPatternOnDemand.TabIndex = 5
        Me.rbPatternOnDemand.TabStop = True
        Me.rbPatternOnDemand.Text = "No Caching (always fresh)"
        Me.rbPatternOnDemand.UseVisualStyleBackColor = True
        '
        'rbPatternMember
        '
        Me.rbPatternMember.AutoSize = True
        Me.rbPatternMember.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbPatternMember.Enabled = False
        Me.rbPatternMember.Location = New System.Drawing.Point(97, 16)
        Me.rbPatternMember.Name = "rbPatternMember"
        Me.rbPatternMember.Size = New System.Drawing.Size(173, 17)
        Me.rbPatternMember.TabIndex = 4
        Me.rbPatternMember.TabStop = True
        Me.rbPatternMember.Text = "Lazy Loading (member-cached)"
        Me.rbPatternMember.UseVisualStyleBackColor = True
        '
        'rbPatternCached
        '
        Me.rbPatternCached.AutoSize = True
        Me.rbPatternCached.Dock = System.Windows.Forms.DockStyle.Left
        Me.rbPatternCached.Enabled = False
        Me.rbPatternCached.Location = New System.Drawing.Point(3, 16)
        Me.rbPatternCached.Name = "rbPatternCached"
        Me.rbPatternCached.Size = New System.Drawing.Size(94, 17)
        Me.rbPatternCached.TabIndex = 3
        Me.rbPatternCached.TabStop = True
        Me.rbPatternCached.Text = "Cached-based"
        Me.rbPatternCached.UseVisualStyleBackColor = True
        '
        'ctrlTables
        '
        Me.ctrlTables.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctrlTables.Location = New System.Drawing.Point(3, 16)
        Me.ctrlTables.Name = "ctrlTables"
        Me.ctrlTables.ShowCheckboxes = False
        Me.ctrlTables.Size = New System.Drawing.Size(44, 178)
        Me.ctrlTables.TabIndex = 3
        Me.ctrlTables.Table = Nothing
        '
        'ctrl3rdTable
        '
        Me.ctrl3rdTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctrl3rdTable.Location = New System.Drawing.Point(3, 16)
        Me.ctrl3rdTable.Name = "ctrl3rdTable"
        Me.ctrl3rdTable.ShowCheckboxes = False
        Me.ctrl3rdTable.Size = New System.Drawing.Size(155, 178)
        Me.ctrl3rdTable.TabIndex = 4
        Me.ctrl3rdTable.Table = Nothing
        '
        'txtCustom
        '
        Me.txtCustom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtCustom.Location = New System.Drawing.Point(3, 16)
        Me.txtCustom.Name = "txtCustom"
        Me.txtCustom.Size = New System.Drawing.Size(294, 114)
        Me.txtCustom.TabIndex = 2
        '
        'txtCustomList
        '
        Me.txtCustomList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtCustomList.Location = New System.Drawing.Point(3, 16)
        Me.txtCustomList.Name = "txtCustomList"
        Me.txtCustomList.Size = New System.Drawing.Size(276, 114)
        Me.txtCustomList.TabIndex = 2
        '
        'UCRelationships
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "UCRelationships"
        Me.Size = New System.Drawing.Size(592, 413)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.pnlRight.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.gbox3rdTable.ResumeLayout(False)
        Me.pnlLeft.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.gboxCustomisation.ResumeLayout(False)
        Me.gboxCustomisationList.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents rbChildren As System.Windows.Forms.RadioButton
    Friend WithEvents rbFKs As System.Windows.Forms.RadioButton
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents pnlLeft As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lvMyColumns As System.Windows.Forms.ListView
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents ctrlTables As CodeGenerator.UCTables
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents gboxCustomisation As System.Windows.Forms.GroupBox
    Friend WithEvents txtCustom As UCCopyAndPaste
    Friend WithEvents gboxCustomisationList As System.Windows.Forms.GroupBox
    Friend WithEvents txtCustomList As UCCopyAndPaste
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents rbPatternOnDemand As System.Windows.Forms.RadioButton
    Friend WithEvents rbPatternMember As System.Windows.Forms.RadioButton
    Friend WithEvents rbPatternCached As System.Windows.Forms.RadioButton
    Friend WithEvents pnlRight As System.Windows.Forms.Panel
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents lvOtherColumns As System.Windows.Forms.ListView
    Friend WithEvents Splitter3 As System.Windows.Forms.Splitter
    Friend WithEvents gbox3rdTable As System.Windows.Forms.GroupBox
    Friend WithEvents ctrl3rdTable As CodeGenerator.UCTables
    Friend WithEvents lnkInvert As System.Windows.Forms.LinkLabel

End Class
