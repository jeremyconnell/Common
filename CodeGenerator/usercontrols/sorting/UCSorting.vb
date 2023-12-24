Public Class UCSorting

#Region "Members"
    Private m_table As CTable
    Private m_info As CMainLogic
#End Region

#Region "Interface"
    Public Sub Display(ByVal table As CTable, ByVal info As CMainLogic)
        m_table = table
        m_info = info

        UcSortColumn1.Clear()
        UcSortColumn2.Clear()
        UcSortColumn3.Clear()
        txtCustom.Text = String.Empty
        txtCustomList.Text = String.Empty

        UcSortColumn1.Enabled = False
        If IsNothing(m_table) Then Exit Sub
        UcSortColumn1.Enabled = True

        With table
            'Show order-by selections
            UcSortColumn1.Display(.ColumnNames)
            UcSortColumn2.Display(.ColumnNames)
            UcSortColumn3.Display(.ColumnNames)
            UcSortColumn1.SelectedIndex = 1

            UcSortColumn2.Enabled = False
            UcSortColumn3.Enabled = False

            'Show class name
            Dim className As String = "ClassName"
            If Not IsNothing(table) Then
                className = table.ClassName
            End If
            Dim fileExt As String = CStr(IIf(info.Language = ELanguage.CSharp, ".cs", ".vb"))
            gboxCustomisation.Text = String.Concat(className, ".customisation", fileExt)
            gboxCustomisationList.Text = String.Concat(className, "List.customisation", fileExt)

            If table.UseCaching Then SplitContainer2.Panel1Collapsed = True
        End With
    End Sub
#End Region

#Region "Event Handlers"
    Private Sub UcSortColumn_Changed() Handles UcSortColumn1.Changed, UcSortColumn2.Changed, UcSortColumn3.Changed
        UcSortColumn2.Enabled = UcSortColumn1.IsSelected
        UcSortColumn3.Enabled = UcSortColumn2.IsSelected
        If Not UcSortColumn2.Enabled Then UcSortColumn2.SelectedIndex = 0
        If Not UcSortColumn3.Enabled Then UcSortColumn3.SelectedIndex = 0

        If Not UcSortColumn1.IsSelected Then Exit Sub
        MemorySort()
        SqlSort()
    End Sub
#End Region

#Region "Memory Sort"
    Private Sub MemorySort()
        Try
            Dim t As New CTemplate("MemorySort.txt", m_info.TemplateFolderSorting)
            t.Replace("ClassName", m_table.ClassName)
            t.Replace("SortName", SortName)
            t.Replace("Compare", Compare)
            txtCustomList.Text = t.Template
        Catch ex As Exception
            txtCustomList.Text = ex.ToString
        End Try
    End Sub

    'CompareTo
    Private Function Compare() As String
        With UcSortColumn3
            If .IsSelected Then Return m_info.CompareToFunction3(m_info.TemplateFolderSorting, UcSortColumn1.ColumnName, UcSortColumn1.IsDescending, UcSortColumn2.ColumnName, UcSortColumn2.IsDescending, .ColumnName, .IsDescending)
        End With
        With UcSortColumn2
            If .IsSelected Then Return m_info.CompareToFunction2(m_info.TemplateFolderSorting, UcSortColumn1.ColumnName, UcSortColumn1.IsDescending, .ColumnName, .IsDescending)
        End With
        With UcSortColumn1
            Return m_info.CompareToFunction1(m_info.TemplateFolderSorting, .ColumnName, .IsDescending)
        End With
    End Function

    'Sort function-name prefix
    Private Function SortName() As String
        Dim sb As New StringBuilder()
        SortName(sb, UcSortColumn1)
        SortName(sb, UcSortColumn2)
        SortName(sb, UcSortColumn3)
        Return sb.ToString
    End Function
    Private Sub SortName(ByVal sb As StringBuilder, ByVal ctrl As UCSortColumn)
        With ctrl
            If .IsSelected Then
                sb.Append(ctrl.ShortName(m_table)) 'sb.Append(.ColumnName)
                If .IsDescending Then sb.Append("Desc")
            End If
        End With
    End Sub
#End Region

#Region "SqlSort"
    Private Sub SqlSort()
        Try
            Dim t As New CTemplate("SqlSort.txt", m_info.TemplateFolderSorting)
            t.Replace("ClassName", m_table.ClassName)
            t.Replace("SortName", SortName)
            t.Replace("SortNameUpperCase", SortNameUpperCase)
            t.Replace("OrderBy", OrderBy)
            txtCustom.Text = t.Template
        Catch ex As Exception
            txtCustom.Text = ex.ToString
        End Try
    End Sub

    'Sort function-name prefix
    Private Function SortNameUpperCase() As String
        Dim sb As New StringBuilder()
        SortNameUpperCase(sb, UcSortColumn1)
        SortNameUpperCase(sb, UcSortColumn2)
        SortNameUpperCase(sb, UcSortColumn3)
        Return sb.ToString
    End Function
    Private Sub SortNameUpperCase(ByVal sb As StringBuilder, ByVal ctrl As UCSortColumn)
        With ctrl
            If .IsSelected Then
                If sb.Length > 0 Then sb.Append("_")
                sb.Append(ctrl.ShortName(m_table).ToUpper) 'sb.Append(.ColumnName)
                If .IsDescending Then sb.Append("_DESC")
            End If
        End With
    End Sub
    Private Function OrderBy() As String
        Dim sb As New StringBuilder()
        OrderBy(sb, UcSortColumn1)
        OrderBy(sb, UcSortColumn2)
        OrderBy(sb, UcSortColumn3)
        Return sb.ToString
    End Function
    Private Sub OrderBy(ByVal sb As StringBuilder, ByVal ctrl As UCSortColumn)
        With ctrl
            If .IsSelected Then
                If sb.Length > 0 Then sb.Append(", ")
                sb.Append(ctrl.ColumnName)
                If .IsDescending Then sb.Append(" DESC")
            End If
        End With
    End Sub
#End Region

End Class
