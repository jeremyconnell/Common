Public Class UCOutput

#Region "Database"
    Private m_database As CDataSrcLocal
    Public Property Database() As CDataSrcLocal
        Get
            Return m_database
        End Get
        Set(ByVal value As CDataSrcLocal)
            m_database = value
            Me.Enabled = Not IsNothing(value)
        End Set
    End Property
#End Region

#Region "Events"
    Public Event StoreSettings()
#End Region

#Region "Properties - Interface"
    Public Sub Init()
        If Len(OutputFolder) = 0 OrElse Not IO.Directory.Exists(OutputFolder) Then btnBrowse_Click(Nothing, Nothing)
    End Sub
    Public Property OutputFolder() As String
        Get
            Return txtFolderPath.Text
        End Get
        Set(ByVal value As String)
            txtFolderPath.Text = value
        End Set
    End Property
    Public Property TablePrefix() As String
        Get
            Return txtTablePrefix.Text
        End Get
        Set(ByVal value As String)
            txtTablePrefix.Text = value
        End Set
    End Property
    Public Property StoredProcPrefix() As String
        Get
            Return txtStoredProcPrefix.Text
        End Get
        Set(ByVal value As String)
            txtStoredProcPrefix.Text = value
        End Set
    End Property
    Public Property UseAuditTrail() As Boolean
        Get
            Return chkUseAuditTrail.Checked
        End Get
        Set(ByVal value As Boolean)
            chkUseAuditTrail.Checked = value
        End Set
    End Property
    Public Property CanUseAuditTrail() As Boolean
        Get
            Return chkUseAuditTrail.Enabled
        End Get
        Set(ByVal value As Boolean)
            chkUseAuditTrail.Enabled = value
        End Set
    End Property
    Public Property CSharpNamespace() As String
        Get
            Return txtNamespace.Text
        End Get
        Set(ByVal value As String)
            txtNamespace.Text = value
        End Set
    End Property
    Public Property HasNamespace() As Boolean
        Get
            Return txtNamespace.Enabled
        End Get
        Set(ByVal value As Boolean)
            txtNamespace.Visible = value
            lblNamespace.Visible = value
        End Set
    End Property
    Public Property CSharp() As Boolean
        Get
            Return rbCSharp.Checked
        End Get
        Set(ByVal value As Boolean)
            If value Then rbCSharp.Checked = True Else rbVbNet.Checked = True
            HasNamespace = value
        End Set
    End Property
    Public Property Architecture() As EArchitecture
        Get
            If rbCompact.Checked Then Return EArchitecture.Dynamic
            If rbComplete.Checked Then Return EArchitecture.Smart
            If rbStoredProcs.Checked Then Return EArchitecture.StoredProcs
            Return EArchitecture.Dynamic
        End Get
        Set(ByVal value As EArchitecture)
            Select Case value
                Case EArchitecture.Dynamic : rbCompact.Checked = True
                Case EArchitecture.Smart : rbComplete.Checked = True
                Case EArchitecture.StoredProcs : rbStoredProcs.Checked = True
                Case Else : rbCompact.Checked = True
            End Select
            rbArchitecture_CheckedChanged(Nothing, Nothing)
        End Set
    End Property


    'Derived
    Public ReadOnly Property Language() As ELanguage
        Get
            If CSharp Then Return ELanguage.CSharp Else Return ELanguage.VbNet
        End Get
    End Property

    'WriteOnly
    Private m_tableNames As String() = Nothing
    Public WriteOnly Property TableNames() As String()
        Set(ByVal value As String())
            m_tableNames = value
            ShowTableNames()
        End Set
    End Property

    'Output-path dependant
    Private m_metadata As CMetadata = Nothing
    Public ReadOnly Property Metadata() As CMetadata
        Get
            If IsNothing(m_metadata) OrElse m_metadata.FolderPath.ToLower <> txtFolderPath.Text.ToLower Then
                m_metadata = New CMetadata(txtFolderPath.Text, Database)
            End If
            Return m_metadata
        End Get
    End Property
#End Region

#Region "Event Handlers"
    'Folder
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        FolderBrowserDialog1.SelectedPath = txtFolderPath.Text
        If FolderBrowserDialog1.ShowDialog(Me) = DialogResult.OK Then
            txtFolderPath.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
    Private Sub txtFolderPath_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFolderPath.TextChanged
        ctrlTables.Enabled = txtFolderPath.Text.Length > 0
        gboxProjectSettings.Enabled = txtFolderPath.Text.Length > 0

        ShowTableNames()
        If Not IsNothing(m_database) Then RaiseEvent StoreSettings()
    End Sub
    Private Sub ctrlClassGen_FolderChanged() Handles ctrlClassGen.FolderChanged
        RaiseEvent StoreSettings()
    End Sub

    'Radios
    Private Sub rbLanguage_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbCSharp.CheckedChanged, rbVbNet.CheckedChanged
        HasNamespace = CSharp
    End Sub
    Private Sub rbArchitecture_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCompact.CheckedChanged, rbComplete.CheckedChanged, rbStoredProcs.CheckedChanged
        txtStoredProcPrefix.Visible = rbStoredProcs.Checked
        lblStoredProcPrefix.Visible = rbStoredProcs.Checked
    End Sub

    'Tables
    Private Sub txtTablePrefix_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTablePrefix.TextChanged
        chkFilterTables.Text = String.Concat("Only show tables begining with: ", txtTablePrefix.Text)
        pnlFilterTables.Visible = txtTablePrefix.Text.Length > 0
        If Not pnlFilterTables.Visible Then chkFilterTables.Checked = False
        ShowTableNames()
    End Sub
    Private Sub chkFilterTables_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilterTables.CheckedChanged
        ShowTableNames()
    End Sub

    'Button
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        RaiseEvent StoreSettings()
    End Sub


    'Usercontrols
    Private Sub ctrlClassGen_SetTable(ByVal newTable As CTable, ByVal isFk As Boolean, ByVal newOtherTable As CTable, ByVal foreignKey As String) Handles ctrlClassGen.SetTable
        ctrlTables.Table = newTable
        ctrlClassGen.Tab = UCClassGenerator.ETab.Relationships
        With ctrlClassGen.UcRelationships1
            .IsFks = isFk
            .OtherTable = newOtherTable
            If Not String.IsNullOrEmpty(foreignKey) Then
                If isFk Then
                    .MyColumn = foreignKey
                Else
                    .OtherFKColumn = foreignKey
                End If
            End If
        End With
    End Sub
    Private Sub ctrlTables_TableClicked(ByVal tableName As String, ByVal table As CTable) Handles ctrlTables.TableClicked
        txtTableName.Text = tableName
        TestTable(table)
    End Sub
    Private Sub ctrlClassGen_Generated() Handles ctrlClassGen.Generated
        Dim temp As CTable = ctrlTables.Table
        m_metadata = Nothing
        ShowTableNames()

        If Not IsNothing(temp) Then 'Todo - some weird effects when classname is changed
            ctrlTables.Table = Metadata.GetByTableName(temp.TableName)
            TestTable(ctrlTables.Table)
        End If
    End Sub
    Private Sub btnGenerateAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerateAll.Click
        If MessageBox.Show("Generate/Regenerate classes for all selected tables?", "Confirm Regenerate All", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then Exit Sub

        Try
            Me.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            Dim overwrite As New COverwriteFiles
            Dim form As New FormOverwriteFiles(overwrite, Me.Architecture = EArchitecture.StoredProcs)
            If form.ShowDialog() = DialogResult.Cancel Then Exit Sub

            For Each i As String In ctrlTables.CheckedTables
                ctrlTables.TableName = i
                ctrlClassGen.Generator.Generate(overwrite)
            Next

            MessageBox.Show("Generated classes for " & ctrlTables.CheckedTables.Count & " entities", "Generate All Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error generating classes", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub
#End Region

#Region "Private - ShowTableNames"
    Private Sub ShowTableNames()
        If IsNothing(m_database) Then Exit Sub

        ctrlTables.Clear()


        If IsNothing(m_tableNames) Then
            pnlTableTextbox.Visible = True 'Can't select table name from list, so have to type it in

            'Some databases can't list tables
            chkUseAuditTrail.Enabled = True

            For Each i As CTable In Metadata
                ctrlTables.Add(i)
            Next
        Else
            pnlTableTextbox.Visible = False

            'Assume no audit trail tables
            chkUseAuditTrail.Enabled = False
            chkUseAuditTrail.Checked = False

            For Each i As String In m_tableNames
                'Check for audit trail tables
                If i.ToLower.StartsWith("tblaudit_") Then
                    chkUseAuditTrail.Enabled = True
                    chkUseAuditTrail.Checked = True
                End If

                'Filter the list
                If chkFilterTables.Checked Then
                    If Not i.ToLower.StartsWith(TablePrefix.ToLower) Then Continue For
                End If

                'Display the table name (and info from generated class)
                If Metadata.ContainsTable(i) Then
                    ctrlTables.Add(Metadata.GetByTableName(i))
                Else
                    ctrlTables.Add(i)
                End If
            Next
        End If
    End Sub
#End Region

#Region "Private - TestTable"
    Private Sub btnTestTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestTable.Click
        'Validate no table
        If Len(Trim(txtTableName.Text)) = 0 Then
            MsgBox("Enter a table Name", MsgBoxStyle.OkOnly, "No Table Name")
            txtTableName.Focus()
            Exit Sub
        End If

        TestTable(Nothing)
        m_metadata = Nothing
    End Sub
    Private Sub TestTable(ByVal table As CTable)
        'Test connection
        Me.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Dim testResult As Boolean = ctrlClassGen.Display(txtTableName, Database, Me, table, Me.Metadata)
        Dim tableName As String = "No Table Selected"
        If testResult Then tableName = ctrlTables.TableName
        gboxTable.Text = String.Concat("Generate Code From Table: ", tableName)

        Me.Enabled = True
        Me.Cursor = Cursors.Arrow
    End Sub
#End Region

End Class
