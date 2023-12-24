Public Class UCRelationships

#Region "Event"
    Public Event SetTable(ByVal newTable As CTable, ByVal isFk As Boolean, ByVal newOtherTable As CTable, ByVal foreignKey As String)
#End Region

#Region "Enums"
    Public Enum EPattern
        Cached = 0
        Member = 1
        OnDemand = 2
    End Enum
#End Region

#Region "Members"
    Private m_table As CTable
    Private m_info As CTableInformation
    Private m_metadata As CMetadata

#End Region

#Region "Interface"
    Public Sub Display(ByVal metadata As CMetadata, ByVal table As CTable, ByVal info As CTableInformation)
        m_table = table
        m_info = info
        m_metadata = metadata

        gbox3rdTable.Visible = False
        lnkInvert.Enabled = False

        'Show all tables
        ctrlTables.Clear()
        For Each i As CTable In metadata
            'If Not IsNothing(table) AndAlso i.TableName = table.TableName Then Continue For
            ctrlTables.Add(i)
        Next

        'Show my columns
        With lvMyColumns.Items
            .Clear()
            If Not IsNothing(table) Then
                For Each i As String In table.ColumnNames
                    .Add(New ListViewItem(i))
                Next
            End If
        End With

        lvOtherColumns.Items.Clear()


        Dim className As String = "ClassName"
        If Not IsNothing(table) Then
            className = table.ClassName
        End If
        Dim fileExt As String = CStr(IIf(info.Language = ELanguage.CSharp, ".cs", ".vb"))
        gboxCustomisation.Text = String.Concat(className, ".customisation", fileExt)
        gboxCustomisationList.Text = String.Concat(className, "List.customisation", fileExt)
    End Sub
#End Region

#Region "Form"
    Public Property IsFks() As Boolean
        Get
            Return rbFKs.Checked
        End Get
        Set(ByVal value As Boolean)
            If value Then rbFKs.Checked = True Else rbChildren.Checked = True
        End Set
    End Property
    Public Property OtherTable() As CTable
        Get
            Return ctrlTables.Table
        End Get
        Set(ByVal value As CTable)
            ctrlTables.Table = value
        End Set
    End Property
    Public Property MyColumn() As String
        Get
            With lvMyColumns.SelectedItems
                If .Count = 0 Then Return Nothing
                Return .Item(0).Text
            End With
        End Get
        Set(ByVal value As String)
            For Each i As ListViewItem In lvMyColumns.Items
                If i.Text = value Then
                    i.Selected = True
                    Exit Property
                End If
            Next
        End Set
    End Property

    Public Property Pattern() As EPattern
        Get
            If rbPatternCached.Checked Then Return EPattern.Cached
            If rbPatternMember.Checked Then Return EPattern.Member
            Return EPattern.OnDemand
        End Get
        Set(ByVal value As EPattern)
            Select Case value
                Case EPattern.Cached : rbPatternCached.Checked = True
                Case EPattern.Member : rbPatternMember.Checked = True
                Case EPattern.OnDemand : rbPatternOnDemand.Checked = True
            End Select
        End Set
    End Property
    Public Property OtherFKColumn() As String
        Get
            With lvOtherColumns.SelectedItems
                If .Count = 0 Then Return String.Empty
                Return .Item(0).Text
            End With
        End Get
        Set(ByVal value As String)
            For Each i As ListViewItem In lvOtherColumns.Items
                If i.Text = value Then
                    i.Selected = True
                    Exit Property
                End If
            Next
        End Set
    End Property
    Public ReadOnly Property OtherFKColumnShorter() As String
        Get
            Return Shorter(OtherFKColumn)
        End Get
    End Property
    Public ReadOnly Property ThirdTable() As CTable
        Get
            If Not gbox3rdTable.Visible Then Return Nothing
            Return ctrl3rdTable.Table
        End Get
    End Property
    Public ReadOnly Property OtherKey() As String
        Get
            If IsNothing(OtherTable) Then Return String.Empty
            With OtherTable
                If Not .IsAssociative Then Return String.Empty
                If .PrimaryKeyName = OtherFKColumn Then
                    Return .SecondaryKeyName
                Else
                    Return .PrimaryKeyName
                End If
            End With
        End Get
    End Property
    Public ReadOnly Property OtherKeyShorter() As String
        Get
            Return Shorter(OtherKey)
        End Get
    End Property



    'Accessors
    Public ReadOnly Property MyTable() As CTable
        Get
            Return m_table
        End Get
    End Property
    Public Property ShowList() As Boolean
        Get
            Return SplitContainer2.Panel2Collapsed
        End Get
        Set(ByVal value As Boolean)
            SplitContainer2.Panel2Collapsed = Not value
            If Not value Then txtCustomList.Text = String.Empty
        End Set
    End Property
#End Region

#Region "Event Handlers"
    Private Sub rb_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbChildren.CheckedChanged
        pnlLeft.Visible = IsFks
        pnlRight.Visible = Not IsFks
        ToggleThirdPanel()
        Generate()
    End Sub
    Private Sub rbKey_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Generate()
    End Sub
    Private Sub ctrlTables_TableClicked(ByVal tableName As String, ByVal metadata As CTable) Handles ctrlTables.TableClicked
        'Show columns for selected table
        With lvOtherColumns.Items
            .Clear()
            If Not IsNothing(OtherTable) Then
                For Each i As String In OtherTable.ColumnNames
                    .Add(New ListViewItem(i))
                    If lvOtherColumns.SelectedItems.Count = 0 Then
                        With i.ToLower
                            If .EndsWith("id") OrElse .EndsWith("_pk") OrElse .EndsWith("_fk") Then
                                Dim index As Integer = lvOtherColumns.Items.Count - 1
                                If Not OtherTable.IsAssociative AndAlso index = 0 Then Continue For
                                lvOtherColumns.SelectedIndices.Add(index)
                            End If
                        End With
                    End If
                Next
            End If
        End With

        ToggleThirdPanel()

        Generate()
    End Sub
    Private Sub ctrl3rdTable_TableClicked(ByVal tableName As String, ByVal metadata As CTable) Handles ctrl3rdTable.TableClicked
        Generate()
    End Sub
    Private Sub lvColumns_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvMyColumns.SelectedIndexChanged, lvOtherColumns.SelectedIndexChanged
        Generate()
    End Sub
    Private Sub lnkInvert_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkInvert.LinkClicked
        Dim fk As String = CStr(IIf(IsFks, MyColumn, OtherFKColumn))
        RaiseEvent SetTable(OtherTable, Not IsFks, MyTable, fk)
    End Sub
#End Region

#Region "Private - Generate"
    Private Sub Generate()
        txtCustom.Text = String.Empty

        lnkInvert.Enabled = False
        If IsNothing(MyTable) Then Exit Sub
        If IsNothing(OtherTable) Then Exit Sub
        lnkInvert.Enabled = True

        If IsFks Then
            If IsNothing(MyColumn) Then Exit Sub

            '3 FK/Parent Patterns:
            If OtherTable.UseCaching Then
                Me.Pattern = EPattern.Cached    '1: Cached => always use cache
            ElseIf MyTable.UseCaching Then
                Me.Pattern = EPattern.OnDemand  '2: Not Cached, but I am => use load-on-demand (rare pattern, normally parent cachability would match)
            Else
                Me.Pattern = EPattern.Member    '3: Not Cached, nor am I => use lazy-loading, with setter to preload
            End If
            Parent_(OtherTable, MyColumn)

            'Associative tables have 2 extra properties (one implemented here), representing a 2-step walk in each direction
            If m_table.IsAssociative Then
                Parent_Associative(OtherTable, MyColumn) 'If OtherTable.UseCaching Then 
            End If
        Else

            '3 Child collection Patterns:
            If OtherTable.UseCaching Then
                Me.Pattern = EPattern.Cached        '1: Cached => always use cache
            ElseIf MyTable.UseCaching Then
                Me.Pattern = EPattern.OnDemand      '2: Not Cached, but I am => use load-on-demand (rare pattern, normally parent cachability would match)
            Else
                Me.Pattern = EPattern.Member        '3: Not Cached, nor am I => use lazy-loading, with setter to preload
            End If
            ChildCollection(OtherTable)

            'Associative tables have 2 extra join-based queries, representing a 2-step walk 
            If OtherTable.IsAssociative Then
                If Not IsNothing(ThirdTable) Then
                    If ThirdTable.UseCaching Then
                        ChildCollection_ThirdTableCached(ThirdTable)
                    ElseIf Not MyTable.UseCaching Then
                        ChildCollection_ThirdTableMember(ThirdTable)
                    Else
                        ChildCollection_ThirdTableOnDemand(ThirdTable)
                    End If
                End If

                If Not OtherTable.UseCaching Then
                    ChildCollection_Associative(OtherTable)
                End If
            End If
        End If
    End Sub

    'Parent Methods
    Private Sub Parent_(ByVal other As CTable, ByVal fkColumnName As String)
        Select Case Pattern
            Case EPattern.Cached : Parent_Cached(other, fkColumnName)
            Case EPattern.Member : Parent_Member(other, fkColumnName)
            Case EPattern.OnDemand : Parent_OnDemand(other, fkColumnName)
        End Select
    End Sub
    Private Sub Parent_Cached(ByVal other As CTable, ByVal fkColumnName As String)
        Try
            Dim t As New CTemplate("ParentCached.txt", m_info.TemplateFolderRelationships)
            t.Replace("ClassName", other.ClassName)
            t.Replace("ForeignKey", fkColumnName)
            t.Replace("ProperCase", other.Singular)
            txtCustom.Text = t.Template
        Catch ex As Exception
            txtCustom.Text = ex.ToString
        End Try

        ShowList = False
    End Sub
    Private Sub Parent_Member(ByVal other As CTable, ByVal fkColumnName As String)
        Try
            Dim t As New CTemplate("ParentMember.txt", m_info.TemplateFolderRelationships)
            t.Replace("ClassName", other.ClassName)
            t.Replace("ThisClassName", m_info.ClassName)
            t.Replace("ForeignKey", fkColumnName)
            t.Replace("ForeignKeyCamelCase", CTableInformation.CamelCase(fkColumnName))
            t.Replace("ProperCase", other.Singular)
            t.Replace("CamelCase", other.SingularCamelCase)
            t.Replace("UpperCase", other.Singular.ToUpper())
            t.Replace("PrimaryKey", other.PrimaryKeyName)
            t.Replace("PrimaryKeyCamelCase", other.PrimaryKeyCamelCase)
            t.Replace("PrimaryKeyType", other.PrimaryKeyTypeName(m_info.Language))
            txtCustom.Text = t.Template
        Catch ex As Exception
            txtCustom.Text = ex.ToString
        End Try

        ShowList = True

        Try
            Dim t As New CTemplate("ParentMemberList.txt", m_info.TemplateFolderRelationships)
            t.Replace("ClassName", m_table.ClassName)
            t.Replace("ParentClassName", other.ClassName)
            t.Replace("ForeignKey", fkColumnName)
            t.Replace("ProperCase", other.Singular)
            t.Replace("Plural", m_table.Plural)
            txtCustomList.Text = t.Template
        Catch ex As Exception
            txtCustomList.Text = ex.ToString
        End Try
    End Sub
    Private Sub Parent_OnDemand(ByVal other As CTable, ByVal fkColumnName As String)
        Try
            Dim t As New CTemplate("ParentOnDemand.txt", m_info.TemplateFolderRelationships)
            t.Replace("ClassName", other.ClassName)
            t.Replace("ForeignKey", fkColumnName)
            t.Replace("ProperCase", other.Singular)
            txtCustom.Text = t.Template
        Catch ex As Exception
            txtCustom.Text = ex.ToString
        End Try

        ShowList = False
    End Sub
    Private Sub Parent_Associative(ByVal other As CTable, ByVal myColumn As String)
        ShowList = True

        Try
            Dim t As New CTemplate("AssociativeList.txt", m_info.TemplateFolderRelationships)
            Dim c As New CTemplate("AssociativeListRemaining_Cached.txt", m_info.TemplateFolderRelationships)
            With other
                t.Replace("Cached", CStr(IIf(.UseCaching, c.Template, String.Empty)))

                t.Replace("Singular", .Singular)
                t.Replace("Plural", .Plural)
                t.Replace("SingularCamelCase", .SingularCamelCase)
                t.Replace("PluralCamelCase", .PluralCamelCase)
                t.Replace("ClassName", .ClassName)
                t.Replace("PrimaryKeyName", myColumn)
                t.Replace("DataType", .PrimaryKeyTypeName(m_info.Language))
            End With
            t.Replace("MyClassName", m_table.ClassName)
            txtCustomList.Text &= t.Template
        Catch ex As Exception
            txtCustomList.Text &= ex.ToString
        End Try
    End Sub

    'Child Methods
    Private Sub ChildCollection(ByVal other As CTable)
        Select Case Me.Pattern
            Case EPattern.Cached : ChildCollection_Cached(other)
            Case EPattern.Member : ChildCollection_Member(other)
            Case EPattern.OnDemand : ChildCollection_OnDemand(other)
        End Select
    End Sub
    Private Sub ChildCollection_Cached(ByVal other As CTable)
        Try
            Dim t As New CTemplate("ChildCollectionCached.txt", m_info.TemplateFolderRelationships)
            t.Replace("ClassName", other.ClassName)
            t.Replace("PrimaryKey", MyTable.PrimaryKeyName)
            t.Replace("ForeignKey", OtherFKColumn)
            t.Replace("ForeignKeyShorter", OtherFKColumnShorter)
            t.Replace("Plural", other.Plural)
            t.Replace("Singular", other.Singular)
            t.Replace("SingularCamelCase", other.SingularCamelCase)
            t.Replace("Entity", MyTable.Singular)
            txtCustom.Text = t.Template
        Catch ex As Exception
            txtCustom.Text = ex.ToString
        End Try

        ShowList = False
    End Sub
    Private Sub ChildCollection_Member(ByVal other As CTable)
        Try
            Dim t As New CTemplate("ChildCollectionMember.txt", m_info.TemplateFolderRelationships)
            t.Replace("ClassName", other.ClassName)
            t.Replace("PrimaryKey", MyTable.PrimaryKeyName)
            t.Replace("ForeignKey", OtherFKColumn)
            t.Replace("ForeignKeyShorter", OtherFKColumnShorter)
            t.Replace("Plural", other.Plural)
            t.Replace("Parent", m_table.Singular)
            t.Replace("CamelCase", other.PluralCamelCase)
            txtCustom.Text = t.Template
        Catch ex As Exception
            txtCustom.Text = ex.ToString
        End Try

        ShowList = True

        Try
            Dim t As New CTemplate("ChildCollectionMemberList.txt", m_info.TemplateFolderRelationships)
            t.Replace("ClassName", other.ClassName)
            t.Replace("Plural", other.Plural)
            t.Replace("MyClassName", m_table.ClassName)
            t.Replace("MyEntity", m_table.Singular)
            t.Replace("PrimaryKey", MyTable.PrimaryKeyName)
            t.Replace("ForeignKeyShorter", OtherFKColumnShorter)
            txtCustomList.Text = t.Template
        Catch ex As Exception
            txtCustomList.Text = ex.ToString
        End Try
    End Sub
    Private Sub ChildCollection_OnDemand(ByVal other As CTable)
        Try
            Dim t As New CTemplate("ChildCollectionOnDemand.txt", m_info.TemplateFolderRelationships)
            t.Replace("ClassName", other.ClassName)
            t.Replace("PrimaryKey", MyTable.PrimaryKeyName)
            t.Replace("ForeignKey", OtherFKColumn)
            t.Replace("ForeignKeyShorter", OtherFKColumnShorter)
            t.Replace("Plural", other.Plural)
            txtCustom.Text = t.Template
        Catch ex As Exception
            txtCustom.Text = ex.ToString
        End Try

        ShowList = False
    End Sub

    Private Sub ChildCollection_Associative(ByVal other As CTable)
        Try
            Dim t As New CTemplate("AssociativeJoin.txt", m_info.TemplateFolderRelationships)
            With m_table
                t.Replace("MySingular", .Singular)
                t.Replace("MyUpperCase", .Singular.ToUpper)
                t.Replace("MyClassName", .ClassName)
                t.Replace("MyPlural", .Plural)
                t.Replace("MyPk", .PrimaryKeyName)
                t.Replace("MySk", .SecondaryKeyName)
            End With
            With other
                t.Replace("OtherUpperCase", .Singular.ToUpper)
                t.Replace("OtherClassName", .ClassName)
            End With
            t.Replace("FkJoin", Me.OtherFKColumn)
            t.Replace("FkProperCase", Me.OtherKey)
            t.Replace("FkShorter", Me.OtherKeyShorter)
            t.Replace("FkCamelCase", CTableInformation.CamelCase(Me.OtherKeyShorter))
            t.Replace("FkDataType", m_info.GetTypeName(Me.OtherTable.GetColumnType(Me.OtherKey)))

            txtCustom.Text &= t.Template
        Catch ex As Exception
            txtCustom.Text &= ex.ToString
        End Try
    End Sub

    Private Sub ChildCollection_ThirdTableCached(ByVal third As CTable)
        Try
            Dim t As New CTemplate("2StepCached.txt", m_info.TemplateFolderRelationships)
            With third
                t.Replace("ClassName", .ClassName)
                t.Replace("Plural", .Plural)
                t.Replace("ChildCollection", OtherTable.Plural)
            End With

            txtCustom.Text &= t.Template
        Catch ex As Exception
            txtCustom.Text &= ex.ToString
        End Try
    End Sub
    Private Sub ChildCollection_ThirdTableMember(ByVal third As CTable)
        Try
            Dim t As New CTemplate("2StepMember.txt", m_info.TemplateFolderRelationships)
            With third
                t.Replace("ClassName", .ClassName)
                t.Replace("Plural", .Plural)
                t.Replace("PluralCamelCase", .PluralCamelCase)
                t.Replace("PrimaryKey", MyTable.PrimaryKeyName)
            End With

            txtCustom.Text &= t.Template
        Catch ex As Exception
            txtCustom.Text &= ex.ToString
        End Try
    End Sub
    Private Sub ChildCollection_ThirdTableOnDemand(ByVal third As CTable)
        Try
            Dim t As New CTemplate("2StepOnDemand.txt", m_info.TemplateFolderRelationships)
            With third
                t.Replace("ClassName", .ClassName)
                t.Replace("Plural", .Plural)
                t.Replace("PluralCamelCase", .PluralCamelCase)
                t.Replace("PrimaryKey", MyTable.PrimaryKeyName)
            End With

            txtCustom.Text &= t.Template
        Catch ex As Exception
            txtCustom.Text &= ex.ToString
        End Try
    End Sub

    Private Function Shorter(ByVal colName As String) As String
        With OtherTable
            Return .GetShortName(colName)
        End With
    End Function
    Private Sub ToggleThirdPanel()
        gbox3rdTable.Visible = Not IsFks AndAlso Not IsNothing(OtherTable) AndAlso OtherTable.IsAssociative
        With ctrl3rdTable
            .Clear()
            If gbox3rdTable.Visible Then
                For Each i As CTable In m_metadata
                    If i.TableName = MyTable.TableName Then Continue For
                    If i.TableName = OtherTable.TableName Then Continue For
                    .Add(i)
                    If IsNothing(.Table) Then .Table = i
                Next
            End If
        End With
    End Sub
#End Region

End Class
