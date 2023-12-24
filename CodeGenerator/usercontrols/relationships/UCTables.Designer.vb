<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCTables
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
        Me.lvTables = New System.Windows.Forms.ListView
        Me.chTable = New System.Windows.Forms.ColumnHeader
        Me.chClass = New System.Windows.Forms.ColumnHeader
        Me.chCaching = New System.Windows.Forms.ColumnHeader
        Me.chAudit = New System.Windows.Forms.ColumnHeader
        Me.chOrderBy = New System.Windows.Forms.ColumnHeader
        Me.chAutoPK = New System.Windows.Forms.ColumnHeader
        Me.chKeys = New System.Windows.Forms.ColumnHeader
        Me.chView = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'lvTables
        '
        Me.lvTables.AllowColumnReorder = True
        Me.lvTables.CheckBoxes = True
        Me.lvTables.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chTable, Me.chClass, Me.chCaching, Me.chAudit, Me.chOrderBy, Me.chAutoPK, Me.chKeys, Me.chView})
        Me.lvTables.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvTables.FullRowSelect = True
        Me.lvTables.GridLines = True
        Me.lvTables.HideSelection = False
        Me.lvTables.Location = New System.Drawing.Point(0, 0)
        Me.lvTables.MultiSelect = False
        Me.lvTables.Name = "lvTables"
        Me.lvTables.Size = New System.Drawing.Size(743, 315)
        Me.lvTables.TabIndex = 0
        Me.lvTables.UseCompatibleStateImageBehavior = False
        Me.lvTables.View = System.Windows.Forms.View.Details
        '
        'chTable
        '
        Me.chTable.Text = "Table"
        Me.chTable.Width = 169
        '
        'chClass
        '
        Me.chClass.Text = "Class"
        Me.chClass.Width = 150
        '
        'chCaching
        '
        Me.chCaching.Text = "Caching"
        Me.chCaching.Width = 51
        '
        'chAudit
        '
        Me.chAudit.Text = "Audit"
        Me.chAudit.Width = 47
        '
        'chOrderBy
        '
        Me.chOrderBy.Text = "OrderBy"
        Me.chOrderBy.Width = 200
        '
        'chAutoPK
        '
        Me.chAutoPK.Text = "AutoPK"
        Me.chAutoPK.Width = 49
        '
        'chKeys
        '
        Me.chKeys.Text = "Primary Keys"
        Me.chKeys.Width = 150
        '
        'chView
        '
        Me.chView.Text = "View"
        '
        'UCTables
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lvTables)
        Me.Name = "UCTables"
        Me.Size = New System.Drawing.Size(743, 315)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvTables As System.Windows.Forms.ListView
    Friend WithEvents chTable As System.Windows.Forms.ColumnHeader
    Friend WithEvents chCaching As System.Windows.Forms.ColumnHeader
    Friend WithEvents chAudit As System.Windows.Forms.ColumnHeader
    Friend WithEvents chKeys As System.Windows.Forms.ColumnHeader
    Friend WithEvents chAutoPK As System.Windows.Forms.ColumnHeader
    Friend WithEvents chOrderBy As System.Windows.Forms.ColumnHeader
    Friend WithEvents chView As System.Windows.Forms.ColumnHeader
    Friend WithEvents chClass As System.Windows.Forms.ColumnHeader

End Class
