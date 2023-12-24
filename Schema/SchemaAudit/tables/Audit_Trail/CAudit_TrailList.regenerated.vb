Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CAudit_TrailList : Inherits List(Of CAudit_Trail)

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
    Public Sub New(ByVal list As CAudit_TrailList)
        MyBase.New(list)
        _index = list._index 
    End Sub

    'Generic list (eg. from paging control), have to assume type
    Public Sub New(ByVal list As IList)
        MyBase.New(list.Count)
        For Each i As CAudit_Trail In list
            MyBase.Add(i)
        Next
    End Sub
#End Region

#Region "Top/Bottom/Page"
    Public Function Top(ByVal count As Integer) As CAudit_TrailList
        If count >= Me.Count Then Return Me
        Return New CAudit_TrailList(CUtilities.Page(Me, count, 0))
    End Function
    Public Function Bottom(ByVal count As Integer) As CAudit_TrailList
        If count > Me.Count Then count = Me.Count
        Return New CAudit_TrailList(Me.GetRange(Me.Count - count - 1, count))
    End Function
    Public Function Page(pageSize As Integer, pageIndex As Integer) As CAudit_TrailList
        Return New CAudit_TrailList( CUtilities.Page(Me, pageSize, pageIndex) )
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
    Public Function SortBy(ByVal propertyName As String, Optional ByVal descending As Boolean = False) As CAudit_TrailList
        Dim copy As New CAudit_TrailList(Me)
        If Me.Count = 0 Then
            Return copy
        End If
        copy.Sort(New CAudit_TrailList_SortBy(propertyName, descending, Me))
        Return copy
    End Function
    'Private 
    Private Class CAudit_TrailList_SortBy : Inherits CReflection.GenericSortBy : Implements IComparer(Of CAudit_Trail)
        Public Sub New(ByVal propertyName As String, ByVal descending As Boolean, ByVal list As IList)
            MyBase.New(propertyName, descending, list)
        End Sub
        Public Overloads Function Compare(ByVal x As CAudit_Trail, ByVal y As CAudit_Trail) As Integer Implements IComparer(Of CAudit_Trail).Compare
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
        For Each i As CAudit_Trail In Me
            i.Save(txOrNull)
        Next
    End Sub
    Public Sub DeleteAll(ByVal txOrNull As IDbTransaction)
        For Each i As CAudit_Trail In Me
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
                For Each i As CAudit_Trail In Me
                    list.Add(i.AuditId)
                Next
                m_ids = list
            End If
            Return m_ids
        End Get
    End Property
    Public Function GetByIds(ByVal ids As List(Of Integer)) As CAudit_TrailList
        Dim list As New CAudit_TrailList(ids.Count)
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
    Public Shadows Sub Add(ByVal item As CAudit_Trail)
        If Not IsNothing(_index) Then
            If Not _index.ContainsKey(item.AuditId) Then
                _index(item.AuditId) = item
            End If
        End If
        MyBase.Add(item)
    End Sub
    Public Shadows Sub Remove(ByVal item As CAudit_Trail)
        If Not IsNothing(_index) Then
            If _index.ContainsKey(item.AuditId) Then
                _index.Remove(item.AuditId)
            End If
        End If
        MyBase.Remove(item)
    End Sub
    
    'Supplementary List Overloads
    Public Shadows Sub Add(ByVal itemsToAdd As IList(Of CAudit_Trail))
        For Each i As CAudit_Trail In itemsToAdd
            Add(i)
        Next
    End Sub
    Public Shadows Sub Remove(ByVal itemsToRemove As IList(Of CAudit_Trail))
        For Each i As CAudit_Trail In itemsToRemove
            Remove(i)
        Next
    End Sub
#End Region

#Region "Main Index (on AuditId)"
    'Only uncomment if type is NOT an Integer (e.g. string/guid)
    'Default Public Overloads ReadOnly Property Item(ByVal auditId As Integer) As CAudit_Trail
    '    Get
    '        Return GetById(auditId)
    '    End Get
    'End Property
    Public Function GetById(ByVal [auditId] As Integer) As CAudit_Trail
        Dim c As CAudit_Trail = Nothing
        Index.TryGetValue([auditId], c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of Integer, CAudit_Trail)
    Private ReadOnly Property Index() As Dictionary(Of Integer, CAudit_Trail)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _Index
                End If
            End If
            _index = New Dictionary(Of Integer, CAudit_Trail)(Me.Count)
            For Each i As CAudit_Trail in Me
                _index(i.AuditId) = i
            Next
            Return _index
        End Get
    End Property
#End Region
    
#Region "Foreign-Key Indices (Subsets)"
    'Index by AuditTypeId
    Public Function GetByTypeId(ByVal typeId As Integer) As CAudit_TrailList
        Dim temp As CAudit_TrailList = Nothing
        If Not IndexByTypeId.TryGetValue(typeId, temp) Then
            temp = New CAudit_TrailList()
            IndexByTypeId(typeId) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByTypeId As Dictionary(Of Integer, CAudit_TrailList)
    Private ReadOnly Property IndexByTypeId() As Dictionary(Of Integer, CAudit_TrailList)
        Get
            If IsNothing(_indexByTypeId) Then
                'Instantiate
                Dim index As New Dictionary(Of Integer, CAudit_TrailList)()

                'Populate
                Dim temp As CAudit_TrailList = Nothing
                For Each i As CAudit_Trail in Me
                    If Not index.TryGetValue(i.AuditTypeId, temp) Then
                        temp = New CAudit_TrailList()
                        index(i.AuditTypeId) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByTypeId = index
            End If
            Return _indexByTypeId
        End Get
    End Property

#End Region

End Class