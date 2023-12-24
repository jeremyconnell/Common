<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCSorting
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.UcSortColumn3 = New CodeGenerator.UCSortColumn
        Me.UcSortColumn2 = New CodeGenerator.UCSortColumn
        Me.UcSortColumn1 = New CodeGenerator.UCSortColumn
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.gboxCustomisation = New System.Windows.Forms.GroupBox
        Me.txtCustom = New UCCopyAndPaste
        Me.gboxCustomisationList = New System.Windows.Forms.GroupBox
        Me.txtCustomList = New UCCopyAndPaste
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.gboxCustomisation.SuspendLayout()
        Me.gboxCustomisationList.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.UcSortColumn3)
        Me.GroupBox1.Controls.Add(Me.UcSortColumn2)
        Me.GroupBox1.Controls.Add(Me.UcSortColumn1)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(371, 91)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Order By Column(s)"
        '
        'UcSortColumn3
        '
        Me.UcSortColumn3.Dock = System.Windows.Forms.DockStyle.Top
        Me.UcSortColumn3.Location = New System.Drawing.Point(3, 64)
        Me.UcSortColumn3.Name = "UcSortColumn3"
        Me.UcSortColumn3.Size = New System.Drawing.Size(365, 24)
        Me.UcSortColumn3.TabIndex = 2
        '
        'UcSortColumn2
        '
        Me.UcSortColumn2.Dock = System.Windows.Forms.DockStyle.Top
        Me.UcSortColumn2.Location = New System.Drawing.Point(3, 40)
        Me.UcSortColumn2.Name = "UcSortColumn2"
        Me.UcSortColumn2.Size = New System.Drawing.Size(365, 24)
        Me.UcSortColumn2.TabIndex = 1
        '
        'UcSortColumn1
        '
        Me.UcSortColumn1.Dock = System.Windows.Forms.DockStyle.Top
        Me.UcSortColumn1.Location = New System.Drawing.Point(3, 16)
        Me.UcSortColumn1.Name = "UcSortColumn1"
        Me.UcSortColumn1.Size = New System.Drawing.Size(365, 24)
        Me.UcSortColumn1.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.SplitContainer2)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 91)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(371, 132)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Generated Code (Copy && Paste)"
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
        Me.SplitContainer2.Size = New System.Drawing.Size(365, 113)
        Me.SplitContainer2.SplitterDistance = 186
        Me.SplitContainer2.TabIndex = 4
        '
        'gboxCustomisation
        '
        Me.gboxCustomisation.Controls.Add(Me.txtCustom)
        Me.gboxCustomisation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxCustomisation.Location = New System.Drawing.Point(0, 0)
        Me.gboxCustomisation.Name = "gboxCustomisation"
        Me.gboxCustomisation.Size = New System.Drawing.Size(186, 113)
        Me.gboxCustomisation.TabIndex = 3
        Me.gboxCustomisation.TabStop = False
        Me.gboxCustomisation.Text = "CEvent.customisation.vb"
        '
        'txtCustom
        '
        Me.txtCustom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtCustom.Location = New System.Drawing.Point(3, 16)
        Me.txtCustom.Name = "txtCustom"
        Me.txtCustom.Size = New System.Drawing.Size(180, 94)
        Me.txtCustom.TabIndex = 2
        '
        'gboxCustomisationList
        '
        Me.gboxCustomisationList.Controls.Add(Me.txtCustomList)
        Me.gboxCustomisationList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gboxCustomisationList.Location = New System.Drawing.Point(0, 0)
        Me.gboxCustomisationList.Name = "gboxCustomisationList"
        Me.gboxCustomisationList.Size = New System.Drawing.Size(175, 113)
        Me.gboxCustomisationList.TabIndex = 4
        Me.gboxCustomisationList.TabStop = False
        Me.gboxCustomisationList.Text = "CEventList.customisation.vb"
        '
        'txtCustomList
        '
        Me.txtCustomList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtCustomList.Location = New System.Drawing.Point(3, 16)
        Me.txtCustomList.Name = "txtCustomList"
        Me.txtCustomList.Size = New System.Drawing.Size(169, 94)
        Me.txtCustomList.TabIndex = 2
        '
        'UCSorting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "UCSorting"
        Me.Size = New System.Drawing.Size(371, 223)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.gboxCustomisation.ResumeLayout(False)
        Me.gboxCustomisation.PerformLayout()
        Me.gboxCustomisationList.ResumeLayout(False)
        Me.gboxCustomisationList.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents UcSortColumn3 As CodeGenerator.UCSortColumn
    Friend WithEvents UcSortColumn2 As CodeGenerator.UCSortColumn
    Friend WithEvents UcSortColumn1 As CodeGenerator.UCSortColumn
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents gboxCustomisation As System.Windows.Forms.GroupBox
    Friend WithEvents txtCustom As UCCopyAndPaste
    Friend WithEvents gboxCustomisationList As System.Windows.Forms.GroupBox
    Friend WithEvents txtCustomList As UCCopyAndPaste

End Class
