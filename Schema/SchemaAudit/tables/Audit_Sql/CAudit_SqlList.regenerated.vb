Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CAudit_SqlList : Inherits List(Of CAudit_Sql)

#Region "Constructors"
    'Basic constructor
    Public Sub New()
        MyBase.New()  
    End Sub
    
    'More efficient memory-allocation if size is known
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)  
    End Sub
    
    'Shares the index (if its already been computed)
    Public Sub New(ByVal list As CAudit_SqlList)
        MyBase.New(list)
        _index = list._index 
    End Sub

    'Generic list (eg. from paging control), have to assume type
    Public Sub New(ByVal list As IList)
        MyBase.New(list.Count)
        For Each i As CAudit_Sql In list
            MyBase.Add(i)
        Next
    End Sub
#End Region

#Region "Top/Bottom/Page"
    Public Function Top(ByVal count As Integer) As CAudit_SqlList
        If count >= Me.Count Then Return Me
        Return New CAudit_SqlList(CUtilities.Page(Me, count, 0))
    End Function
    Public Function Bottom(ByVal count As Integer) As CAudit_SqlList
        If count > Me.Count Then count = Me.Count
        Return New CAudit_SqlList(Me.GetRange(Me.Count - count - 1, count))
    End Function
    Public Function Page(pageSize As Integer, pageIndex As Integer) As CAudit_SqlList
        Return New CAudit_SqlList( CUtilities.Page(Me, pageSize, pageIndex) )
    End Function
#End Region

#Region "BulkEditLogic"
    Public Function HaveSameValue(ByVal propertyName As String) As Boolean
        Return CReflection.HaveSameValue(Me, propertyName)
    End Function
    Public Sub SetSameValue(ByVal propertyName As String, ByVal value As Object)
        CReflection.SetSameValue(Me, propertyName, value)
    End Sub
#End Region

#Region "SortBy"
    'Public
    Public Function SortBy(ByVal propertyName As String, Optional ByVal descending As Boolean = False) As CAudit_SqlList
        Dim copy As New CAudit_SqlList(Me)
        If Me.Count = 0 Then
            Return copy
        End If
        copy.Sort(New CAudit_SqlList_SortBy(propertyName, descending, Me))
        Return copy
    End Function
    'Private 
    Private Class CAudit_SqlList_SortBy : Inherits CReflection.GenericSortBy : Implements IComparer(Of CAudit_Sql)
        Public Sub New(ByVal propertyName As String, ByVal descending As Boolean, ByVal list As IList)
            MyBase.New(propertyName, descending, list)
        End Sub
        Public Overloads Function Compare(ByVal x As CAudit_Sql, ByVal y As CAudit_Sql) As Integer Implements IComparer(Of CAudit_Sql).Compare
            Return MyBase.Compare(x, y)
        End Function
    End Class
#End Region

#Region "SaveAll/DeleteAll"
    'Use default connection (may be overridden in base class)
    Public Sub SaveAll()
        If Me.Count = 0 Then Exit Sub
        SaveAll(Me(0).DataSrc)
    End Sub
    Public Sub DeleteAll()
        If Me.Count = 0 Then Exit Sub
        DeleteAll(Me(0).DataSrc)
    End Sub
    
    'Use connection supplied
    Public Sub SaveAll(dataSrc as CDatasrc)
        dataSrc.BulkSave(Me)
    End Sub
    Public Sub DeleteAll(dataSrc as CDatasrc)
        dataSrc.BulkDelete(Me)
    End Sub

    'Use transaction supplied
    Public Sub SaveAll(ByVal txOrNull As IDbTransaction)
        For Each i As CAudit_Sql In Me
            i.Save(txOrNull)
        Next
    End Sub
    Public Sub DeleteAll(ByVal txOrNull As IDbTransaction)
        For Each i As CAudit_Sql In Me
            i.Delete(txOrNull)
        Next
    End Sub

    'Use a specified isolation level
    Public Sub SaveAll(ByVal txIsolationLevel As IsolationLevel)
        If Me.Count = 0 Then Exit Sub
        SaveAll(Me(0).DataSrc, txIsolationLevel)
    End Sub
    Public Sub DeleteAll(ByVal txIsolationLevel As IsolationLevel)
        If Me.Count = 0 Then Exit Sub
        DeleteAll(Me(0).DataSrc, txIsolationLevel)
    End Sub
    
    'Use a specified connection and isolation level
    Public Sub SaveAll(ByVal dataSrc as CDatasrc, ByVal txIsolationLevel As IsolationLevel)
        dataSrc.BulkSave(Me, txIsolationLevel)
    End Sub
    Public Sub DeleteAll(ByVal dataSrc as CDatasrc, ByVal txIsolationLevel As IsolationLevel)
        dataSrc.BulkDelete(Me, txIsolationLevel)
    End Sub
#End Region

#Region "List of Ids"
    Private m_ids As List(Of Integer)
    Public ReadOnly Property Ids() As List(Of Integer)
        Get
            If m_ids Is Nothing Then
                Dim list As New List(Of Integer)(Me.Count)
                For Each i As CAudit_Sql In Me
                    list.Add(i.SqlId)
                Next
                m_ids = list
            End If
            Return m_ids
        End Get
    End Property
    Public Function GetByIds(ByVal ids As List(Of Integer)) As CAudit_SqlList
        Dim list As New CAudit_SqlList(ids.Count)
        For Each id As Integer In ids
            If GetById(id) IsNot Nothing Then
                list.Add(GetById(id))
            End If
        Next
        Return list
    End Function
#End Region

#Region "Cache-Control"
    'Main Logic
    Public Shadows Sub Add(ByVal item As CAudit_Sql)
        If Not IsNothing(_index) Then
            If Not _index.ContainsKey(item.SqlId) Then
                _index(item.SqlId) = item
            End If
        End If
        MyBase.Add(item)
    End Sub
    Public Shadows Sub Remove(ByVal item As CAudit_Sql)
        If Not IsNothing(_index) Then
            If _index.ContainsKey(item.SqlId) Then
                _index.Remove(item.SqlId)
            End If
        End If
        MyBase.Remove(item)
    End Sub
    
    'Supplementary List Overloads
    Public Shadows Sub Add(ByVal itemsToAdd As IList(Of CAudit_Sql))
        For Each i As CAudit_Sql In itemsToAdd
            Add(i)
        Next
    End Sub
    Public Shadows Sub Remove(ByVal itemsToRemove As IList(Of CAudit_Sql))
        For Each i As CAudit_Sql In itemsToRemove
            Remove(i)
        Next
    End Sub
#End Region

#Region "Main Index (on SqlId)"
    'Only uncomment if type is NOT an Integer (e.g. string/guid)
    'Default Public Overloads ReadOnly Property Item(ByVal sqlId As Integer) As CAudit_Sql
    '    Get
    '        Return GetById(sqlId)
    '    End Get
    'End Property
    Public Function GetById(ByVal [sqlId] As Integer) As CAudit_Sql
        Dim c As CAudit_Sql = Nothing
        Index.TryGetValue([sqlId], c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of Integer, CAudit_Sql)
    Private ReadOnly Property Index() As Dictionary(Of Integer, CAudit_Sql)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _Index
                End If
            End If
            _index = New Dictionary(Of Integer, CAudit_Sql)(Me.Count)
            For Each i As CAudit_Sql in Me
                _index(i.SqlId) = i
            Next
            Return _index
        End Get
    End Property
#End Region
    
#Region "Foreign-Key Indices (Subsets)"
#End Region

End Class