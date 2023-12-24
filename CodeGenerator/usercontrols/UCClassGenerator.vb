Public Class UCClassGenerator

    Public Enum ETab
        ClassGen
        Relationships
        Sorting
        UI
    End Enum

#Region "Events"
    Public Event FolderChanged()
    Public Event Generated()
    Public Event SetTable(ByVal newTable As CTable, ByVal isFk As Boolean, ByVal newOtherTable As CTable, ByVal foreignKey As String)
#End Region

#Region "Members"
    Private m_tableName As String
    Private m_dataSrc As CDataSrcLocal
    Private m_parent As UCOutput

    Public GENERATE_ALL As Boolean = False
#End Region

#Region "Interface"
    Public Function Display(ByVal tableName As TextBox, ByVal dataSrc As CDataSrcLocal, ByVal parent As UCOutput, ByVal table As CTable, ByVal metadata As CMetadata) As Boolean
        'Store refs
        m_tableName = tableName.Text
        m_dataSrc = dataSrc
        m_parent = parent

        Dim info As CMainLogic = Generator()

        'Test table
        Try
            Dim columnNames As String() = info.TableColumnNames
            InitDropdowns(columnNames)
        Catch ex As Exception
            If Not m_tableName.Contains("[") Then
                'Try again with square brackets
                If TypeOf dataSrc Is COleDb AndAlso dataSrc.ConnectionString.ToLower.Contains(".xls") AndAlso Not m_tableName.Contains("$") Then
                    tableName.Text = String.Concat("[", m_tableName, "$]")
                Else
                    tableName.Text = String.Concat("[", m_tableName, "]")
                End If
                Return Display(tableName, dataSrc, parent, table, metadata)
            Else
                'Exit with message
                MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Invalid table name: " & m_tableName)
                Me.Enabled = False
                Return False
            End If
        End Try

        'Init generator
        SetSelectedQueryCols(info)
        If IsNothing(table) Then
            'Guess default values
            txtOrderBy.Text = GuessOrderby()
            txtViewName.Text = "vw" & info.Plural()
            txtClassName.Text = CTableInformation.CLASS_PREFIX & info.Singular
            SetValue(cboSortingColumn, GuessSortingColumn)
            chkHasSortColumn.Checked = GuessSortingColumn.Length > 0
        Else
            'Apply last values used
            With table
                txtOrderBy.Text = .OrderBy
                Try
                    txtViewName.Text = metadata.GetByTableName(.TableName).ViewName
                Catch ex As Exception
                    txtViewName.Text = .ViewName
                End Try
                txtClassName.Text = .ClassName

                SetValue(cboPrimaryKey, .PrimaryKeyName)
                SetValue(cboSecondKey, .SecondaryKeyName)
                SetValue(cboThirdKey, .TertiaryKeyName)
                chkAutoIncrement.Checked = .AutoPk
                chkM2M.Checked = Not String.IsNullOrEmpty(.SecondaryKeyName)
                chk3Way.Checked = Not String.IsNullOrEmpty(.TertiaryKeyName)

                chkView.Checked = Not String.IsNullOrEmpty(.ViewName)
                If Not chkView.Checked Then txtViewName.Text = "vw" & info.Plural()

                chkCaching.Checked = .UseCaching

                SetValue(cboSortingColumn, .SortingColumn)
                chkHasSortColumn.Checked = .SortingColumn.Length > 0
                If Not chkHasSortColumn.Checked Then SetValue(cboSortingColumn, GuessSortingColumn())
            End With
        End If
        btnGenerate.Enabled = True

        'Refresh object
        info = Generator()

        UcRelationships1.Display(metadata, table, info)
        UcSorting1.Display(table, info)
        UcUiCodeGen1.Display(metadata, table, info)

        Me.Enabled = True
        Return True
    End Function
#End Region

#Region "Form"
    Public Property Tab() As ETab
        Get
            Return CType(TabControl1.SelectedIndex, ETab)
        End Get
        Set(ByVal value As ETab)
            TabControl1.SelectedIndex = value
        End Set
    End Property
#End Region

#Region "Event Handlers - UserControls, event bubbling"
    Private Sub UcRelationships1_SetTable(ByVal newTable As CTable, ByVal isFk As Boolean, ByVal newOtherTable As CTable, ByVal foreignKey As String) Handles UcRelationships1.SetTable
        RaiseEvent SetTable(newTable, isFk, newOtherTable, foreignKey)
    End Sub
    Private Sub UcUiCodeGen1_FolderChanged() Handles UcUiCodeGen1.FolderChanged
        RaiseEvent FolderChanged()
    End Sub
#End Region

#Region "Event Handlers - Class Gen"
    Private Sub chkView_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkView.CheckedChanged
        txtViewName.Enabled = chkView.Checked
    End Sub
    Private Sub chkOrderBy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOrderBy.CheckedChanged
        txtOrderBy.Enabled = chkOrderBy.Checked
    End Sub
    Private Sub chkM2M_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkM2M.CheckedChanged
        cboSecondKey.Enabled = chkM2M.Checked
        If chkM2M.Checked Then chkAutoIncrement.Checked = False Else chk3Way.Checked = False
        If chkM2M.Checked Then
            chkM2M.Font = New Font(chkM2M.Font, FontStyle.Bold)
        Else
            chkM2M.Font = New Font(chkM2M.Font, FontStyle.Regular)
        End If
        SetSelectedQueryCols()
    End Sub
    Private Sub chk3Way_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk3Way.CheckedChanged
        cboThirdKey.Enabled = chk3Way.Checked
        If chk3Way.Checked Then
            chkM2M.Checked = True
            chkAutoIncrement.Checked = False
            chk3Way.Font = New Font(chk3Way.Font, FontStyle.Bold)
        Else
            chk3Way.Font = New Font(chk3Way.Font, FontStyle.Regular)
        End If
        SetSelectedQueryCols()
    End Sub
    Private Sub chkAutoIncrement_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoIncrement.CheckedChanged
        If Len(Trim(m_tableName)) = 0 Then Exit Sub


        If chkAutoIncrement.Checked Then
            chkM2M.Checked = False
            chk3Way.Checked = False
            cboSecondKey.Enabled = False
            cboThirdKey.Enabled = False
            chkAutoIncrement.Font = New Font(chkAutoIncrement.Font, FontStyle.Regular)
        Else
            chkAutoIncrement.Font = New Font(chkAutoIncrement.Font, FontStyle.Bold)
        End If
        SetSelectedQueryCols()
    End Sub
    Private Sub chkHasSortColumn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkHasSortColumn.CheckedChanged
        cboSortingColumn.Enabled = chkHasSortColumn.Checked
    End Sub
    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        CheckSortingColumn()
        CheckSortingColumnInSortOrder()

        Me.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Dim info As CTableInformation = Generator
            With info
                If .Architecture = EArchitecture.StoredProcs And .Platform <> EPlatform.Other Then
                    .ExecuteScripts = (MsgBoxResult.Ok = MsgBox("Run Sql Scripts?", MsgBoxStyle.OkCancel, "Run Sql Scripts"))
                End If

                If Not .Generate() Then
                    Me.Enabled = True
                    Me.Cursor = Cursors.Arrow
                    Exit Sub
                End If

                RaiseEvent Generated()

                Dim msg As String = "Files Generated Successfully"
                If .Architecture = EArchitecture.StoredProcs Then
                    msg &= vbCrLf & vbCrLf & "Warning: Check datatypes for Insert/Update" & vbCrLf & " - all strings are represented as VARCHAR(255), which may not be correct"
                End If
                MsgBox(msg)
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Me.Enabled = True
        Me.Cursor = Cursors.Arrow
    End Sub
#End Region

#Region "Private - Generator Info"
    Public ReadOnly Property Generator() As CMainLogic
        Get
            Generator = New CMainLogic

            With Generator
                .Database = m_dataSrc
                .TableName = m_tableName
                .TableNamePrefix = m_parent.txtTablePrefix.Text
                .Platform = UCConnections.GetPlatForm(m_dataSrc)

                .ClassName = txtClassName.Text
                .UseCaching = chkCaching.Checked

                .PrimaryKeyName = CStr(cboPrimaryKey.SelectedItem)
                .SecondaryKeyName = CStr(IIf(chkM2M.Checked, cboSecondKey.SelectedItem, String.Empty))
                .TertiaryKeyName = CStr(IIf(chk3Way.Checked, cboThirdKey.SelectedItem, String.Empty))

                .ViewName = CStr(IIf(chkView.Checked, txtViewName.Text, String.Empty))
                .OrderByColumns = CStr(IIf(chkOrderBy.Checked, txtOrderBy.Text, String.Empty))
                .SortingColumn = CStr(IIf(chkHasSortColumn.Checked, cboSortingColumn.Text, String.Empty))

                If chkAutoIncrement.Checked Then
                    .PrimaryKeyType = EPrimaryKeyType.AutoNumber
                Else
                    .PrimaryKeyType = EPrimaryKeyType.Manual
                    If chkM2M.Checked Then .PrimaryKeyType = EPrimaryKeyType.Many2Many
                    If chk3Way.Checked Then .PrimaryKeyType = EPrimaryKeyType.ThreeWay
                End If

                If chkHasSortColumn.Checked Then .SortingColumn = CStr(cboSortingColumn.SelectedValue)

                .Language = m_parent.Language
                .CSharpNamespace = m_parent.CSharpNamespace
                .Architecture = m_parent.Architecture
                .UseAuditTrail = m_parent.UseAuditTrail

                Dim al As New List(Of String)
                For Each i As String In cboFilters.SelectedItems
                    If i <> .PrimaryKeyName And i <> .SecondaryKeyName Then
                        al.Add(i)
                    End If
                Next
                If .IsManyToMany Or .Is3Way Then
                    al.Add(.PrimaryKeyName)
                    al.Add(.SecondaryKeyName)
                    If .Is3Way Then al.Add(.TertiaryKeyName)
                End If
                .OptionalFilters = al.ToArray()

                .TemplateFolder = "/dotnet/" & .Language.ToString() & "/" 'IO.Path.GetDirectoryName(Application.ExecutablePath) & 
                .TargetFolder = m_parent.txtFolderPath.Text
            End With
        End Get
    End Property
#End Region

#Region "Private Guess Values"
    Private Sub InitDropdowns(ByVal columnNames As String())
        cboPrimaryKey.DataSource = New List(Of String)(columnNames).ToArray
        cboSecondKey.DataSource = New List(Of String)(columnNames).ToArray
        cboThirdKey.DataSource = New List(Of String)(columnNames).ToArray
        cboSortingColumn.DataSource = New List(Of String)(columnNames).ToArray

        If cboThirdKey.Items.Count > 2 Then cboThirdKey.SelectedIndex = 2
        If cboSecondKey.Items.Count > 1 Then cboSecondKey.SelectedIndex = 1
        cboPrimaryKey.SelectedIndex = 0

        cboFilters.DataSource = columnNames
    End Sub
    Private Sub SetValue(ByVal dd As ComboBox, ByVal value As String)
        For Each i As Object In dd.Items
            If i.Equals(value) Then
                dd.SelectedIndex = dd.Items.IndexOf(i)
                Exit Sub
            End If
        Next
    End Sub
    Private Sub SetSelectedQueryCols()
        SetSelectedQueryCols(Generator)
    End Sub
    Private Sub SetSelectedQueryCols(ByVal info As CMainLogic)
        cboFilters.SelectedIndex = -1
        Dim i As Integer
        Dim skipCols As Integer = 1
        If chkM2M.Checked Then
            skipCols = 2
            If chk3Way.Checked Then skipCols = 3
        End If
        Dim types As Type() = info.TableColumnTypes
        For i = skipCols To cboFilters.Items.Count - 1
            With CStr(cboFilters.Items(i)).ToLower
                If .EndsWith("id") Or .EndsWith("_pk") Or .EndsWith("_fk") Then
                    cboFilters.SetSelected(i, True)
                End If
                If types(i) Is GetType(Boolean) Then
                    cboFilters.SetSelected(i, True)
                End If
            End With
        Next
    End Sub
    Private Function GuessOrderby() As String
        Dim orderBy As String = String.Empty

        For Each i As String In cboPrimaryKey.Items
            If i.EndsWith("Ordinal", StringComparison.CurrentCultureIgnoreCase) Then orderBy = i
            If i.EndsWith("SortOrder", StringComparison.CurrentCultureIgnoreCase) Then orderBy = i
        Next

        For Each i As String In cboPrimaryKey.Items
            If i.EndsWith("name", StringComparison.CurrentCultureIgnoreCase) Then
                If Len(orderBy) = 0 Then orderBy = i Else orderBy = orderBy & ", " & i
            End If
        Next

        If Len(orderBy) = 0 Then orderBy = CStr(cboPrimaryKey.Items(0))
        Return orderBy
    End Function
    Private Function GuessSortingColumn() As String
        For Each i As String In cboPrimaryKey.Items
            With i.ToLower
                If .Contains("ordinal") Then Return i
                If .Contains("sort") Then Return i
                If .EndsWith("order") Then Return i
            End With
        Next
        Return String.Empty
    End Function
    Private Sub CheckSortingColumn()
        'Ensure sorting columns are correctly identified
        If chkHasSortColumn.Checked Then Exit Sub
        Dim suggest As String = GuessSortingColumn()
        If String.IsNullOrEmpty(suggest) Then Exit Sub

        Dim msg As String = String.Concat("Based on naming conventions, this column is probably used for sorting records: '", suggest, "'", vbCrLf, vbCrLf, "Did you want to mark this as a Sort-Order column?", vbCrLf, vbCrLf, "This will generate MoveUp/MoveDown methods on the list class.")
        If MessageBox.Show(msg, "Sorting column?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
            chkHasSortColumn.Checked = True
        End If
    End Sub
    Private Sub CheckSortingColumnInSortOrder()
        If Not chkHasSortColumn.Checked Then Exit Sub

        If txtOrderBy.Text.ToLower.Contains(CStr(cboSortingColumn.SelectedValue).ToLower) Then Exit Sub
        Dim msg As String = String.Concat("The nominated sorting column (", cboSortingColumn.SelectedValue, ") is not included in the sort order expression.", vbCrLf, vbCrLf, "This is necessary for the controlled sorting to work.", vbCrLf, vbCrLf, "Update the sort expression now?")
        If MessageBox.Show(msg, "Sorting column?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
            With txtOrderBy
                If .Text.ToLower.Trim() = "" Then
                    .Text = CStr(cboSortingColumn.SelectedValue)
                Else
                    .Text = String.Concat(cboSortingColumn.SelectedValue, ", ", .Text)
                End If
            End With
        End If
    End Sub
#End Region


End Class
