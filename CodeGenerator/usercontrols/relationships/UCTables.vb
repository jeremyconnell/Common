Public Class UCTables
    'Events
    Public Event TableClicked(ByVal tableName As String, ByVal metadata As CTable)

    'Event Handlers
    Private Sub lvTables_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvTables.SelectedIndexChanged
        If Not IsSelected Then Exit Sub
        RaiseEvent TableClicked(TableName, Table)
    End Sub

    'Properties - State
    Public Shadows Property Enabled() As Boolean
        Get
            Return lvTables.Enabled
        End Get
        Set(ByVal value As Boolean)
            lvTables.Enabled = value
        End Set
    End Property
    Public ReadOnly Property IsSelected() As Boolean
        Get
            Return lvTables.SelectedItems.Count > 0
        End Get
    End Property
    Public Property Table() As CTable
        Get
            If Not IsSelected Then Return Nothing
            Return CType(lvTables.SelectedItems(0).Tag, CTable)

        End Get
        Set(ByVal value As CTable)
            If IsNothing(value) Then
                lvTables.SelectedItems.Clear()
            Else
                For Each i As ListViewItem In lvTables.Items
                    Dim t As CTable = CType(i.Tag, CTable)
                    If IsNothing(t) Then Continue For
                    If t.TableName = value.TableName Then i.Selected = True
                Next
            End If
        End Set
    End Property
    Public Property TableName() As String
        Get
            If Not IsSelected Then Return String.Empty
            Return lvTables.SelectedItems(0).Text
        End Get
        Set(ByVal value As String)
            For Each i As ListViewItem In lvTables.Items
                If LCase(i.Text) = LCase(value) Then i.Selected = True
            Next
        End Set
    End Property
    Public ReadOnly Property CheckedTables() As List(Of String)
        Get
            Dim list As New List(Of String)(lvTables.CheckedItems.Count)
            For Each i As ListViewItem In lvTables.CheckedItems
                list.Add(i.Text)
            Next
            Return list
        End Get
    End Property
    Public Property ShowCheckboxes() As Boolean
        Get
            Return lvTables.CheckBoxes
        End Get
        Set(ByVal value As Boolean)
            lvTables.CheckBoxes = value
        End Set
    End Property

    'Methods - Clear/Add Item
    Public Sub Clear()
        lvTables.Items.Clear()
    End Sub
    Public Function Add(ByVal tableName As String) As ListViewItem 'Table that has not yet been don
        Dim lvi As New ListViewItem(tableName)
        lvTables.Items.Add(lvi)
        lvi.Checked = False
        Return lvi
    End Function
    Public Sub Add(ByVal table As CTable)
        With table
            Dim lvi As ListViewItem = Add(.TableName)
            lvi.SubItems.Add(.ClassName)
            lvi.SubItems.Add(Bool(.UseCaching))
            lvi.SubItems.Add(Bool(.AuditTrail))
            lvi.SubItems.Add(.OrderBy)
            lvi.SubItems.Add(Bool(.AutoPk And Not .IsAssociative))
            lvi.SubItems.Add(.Keys)
            lvi.SubItems.Add(.ViewName)

            lvi.Tag = table
            lvi.Checked = True
        End With
    End Sub
    Private Function Bool(ByVal value As Boolean) As String
        If value Then Return "yes" Else Return "no"
    End Function

End Class
