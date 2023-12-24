Imports System.Xml

<CLSCompliant(True)> _
 Public Class CDifferences
    'Constructor
    Public Sub New(ByVal trail As CAudit_Trail, ByVal includeExisting As Boolean)
        Select Case trail.AuditTypeId
            Case EAuditType.Delete
                LoadDeletes(trail.Datas)
            Case EAuditType.Insert
                LoadAdds(trail.Datas)
            Case Else
                If includeExisting Then
                    LoadChanges(trail.Datas, GetCurrent(trail.AuditDataTableName, trail.AuditDataPrimaryKey))
                Else
                    LoadChanges(trail.Datas, New Dictionary(Of String, String))
                End If
        End Select
    End Sub

    'Members
    Private m_added As New CChangeList
    Private m_changed As New CChangeList
    Private m_removed As New CChangeList
    Private m_same As New CChangeList

    'Accessors
    Public ReadOnly Property Added() As CChangeList
        Get
            Return m_added
        End Get
    End Property
    Public ReadOnly Property Changed() As CChangeList
        Get
            Return m_changed
        End Get
    End Property
    Public ReadOnly Property Removed() As CChangeList
        Get
            Return m_removed
        End Get
    End Property
    Public ReadOnly Property Same() As CChangeList
        Get
            Return m_same
        End Get
    End Property

    'Derived
    Public ReadOnly Property ColumnNames() As List(Of String)
        Get
            Dim list As New List(Of String)(Added.Count + Changed.Count + Removed.Count + Same.Count)
            list.AddRange(Added.ColumnNames)
            list.AddRange(Changed.ColumnNames)
            list.AddRange(Removed.ColumnNames)
            list.AddRange(Same.ColumnNames)
            Return list
        End Get
    End Property
    Public ReadOnly Property ColumnNamesChanged() As List(Of String)
        Get
            Dim list As New List(Of String)(Added.Count + Changed.Count + Removed.Count)
            list.AddRange(Added.ColumnNames)
            list.AddRange(Changed.ColumnNames)
            list.AddRange(Removed.ColumnNames)
            Return list
        End Get
    End Property

    'Private
    Private Sub LoadAdds(ByVal after As CAudit_DataList)
        For Each i As CAudit_Data In after
            Added.Add(i.DataName, String.Empty, i.DataValue)
        Next
    End Sub
    Private Sub LoadDeletes(ByVal before As CAudit_DataList)
        For Each i As CAudit_Data In before
            Removed.Add(i.DataName, i.DataValue, String.Empty)
        Next
    End Sub
    Private Sub LoadChanges(ByVal data As CAudit_DataList, ByVal current As Dictionary(Of String, String))
        Dim before As New Dictionary(Of String, String)(data.Before.Count)
        For Each i As CAudit_Data In data.Before
            before.Add(i.DataName, i.DataValue)
        Next

        Dim after As New Dictionary(Of String, String)(data.After.Count)
        For Each i As CAudit_Data In data.After
            after.Add(i.DataName, i.DataValue)
        Next

        For Each i As String In before.Keys
            If after.ContainsKey(i) Then
                Me.Changed.Add(i, before(i), after(i))
            Else
                Me.Changed.Add(i, before(i), "Error")
            End If
        Next
        For Each i As String In after.Keys
            If before.ContainsKey(i) Then
                'Me.Changed.Add(i, before(i), after(i))
            Else
                Me.Changed.Add(i, "Error", after(i))
            End If
        Next
        For Each i As String In current.Keys
            If Not before.ContainsKey(i) AndAlso Not after.ContainsKey(i) Then
                Me.Same.Add(i, current(i), current(i))
            End If
        Next
    End Sub
    Private Function GetCurrent(ByVal table As String, ByVal pks As String) As Dictionary(Of String, String)
        Dim current As New Dictionary(Of String, String)
        Try
            Dim ds As DataSet = CDataSrc.Default.SelectWhere_Dataset("*", table, Nothing, "1=0")
            Dim keys As New List(Of String)(pks.Split(CChar("/")))
            Dim where As New CCriteriaList(keys.Count)
            For Each i As String In keys
                where.Add(ds.Tables(0).Columns(keys.IndexOf(i)).ColumnName, i)
            Next
            ds = CDataSrc.Default.SelectWhere_Dataset(table, where)
            If ds.Tables(0).Rows.Count = 1 Then
                Dim dr As DataRow = ds.Tables(0).Rows(0)
                For Each i As DataColumn In dr.Table.Columns
                    current.Add(i.ColumnName, CAdoData.GetStr(dr, i.Ordinal))
                Next
            End If
        Catch
        End Try
        Return current
    End Function
End Class

<CLSCompliant(True)> _
Public Class CChange
    Public Sub New(ByVal name As String, ByVal oldValue As String, ByVal newValue As String)
        Me.ColumnName = name
        Me.OldValue = oldValue
        Me.NewValue = newValue
    End Sub

    Public ColumnName As String
    Public OldValue As String
    Public NewValue As String
End Class

<CLSCompliant(True)> _
Public Class CChangeList : Inherits List(Of CChange)

    Public Overloads Sub Add(ByVal name As String, ByVal before As String, ByVal after As String)
        Me.Add(New CChange(name, before, after))
    End Sub
    Public Function ColumnNames() As List(Of String)
        Dim list As New List(Of String)(Me.Count)
        For Each i As CChange In Me
            list.Add(i.ColumnName)
        Next
        Return list
    End Function

End Class

<CLSCompliant(True)> _
Public Class CData
    Public Sub New(ByVal name As String, ByVal value As String)
        Me.ColumnName = name
        Me.Value = value
    End Sub

    Public ColumnName As String
    Public Value As String
End Class
