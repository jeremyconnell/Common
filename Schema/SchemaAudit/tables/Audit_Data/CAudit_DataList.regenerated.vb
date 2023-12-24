Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CAudit_DataList : Inherits List(Of CAudit_Data)

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
    Public Sub New(ByVal list As CAudit_DataList)
        MyBase.New(list)
        _index = list._index 
    End Sub

    'Generic list (eg. from paging control), have to assume type
    Public Sub New(ByVal list As IList)
        MyBase.New(list.Count)
        For Each i As CAudit_Data In list
            MyBase.Add(i)
        Next
    End Sub
#End Region

#Region "Top/Bottom/Page"
    Public Function Top(ByVal count As Integer) As CAudit_DataList
        If count >= Me.Count Then Return Me
        Return New CAudit_DataList(CUtilities.Page(Of CAudit_Data)(Me, count, 0))
    End Function
    Public Function Bottom(ByVal count As Integer) As CAudit_DataList
        If count > Me.Count Then count = Me.Count
        Return New CAudit_DataList(Me.GetRange(Me.Count - count - 1, count))
    End Function
    Public Function Page(pageSize As Integer, pageIndex As Integer) As CAudit_DataList
        Return New CAudit_DataList(CUtilities.Page(Of CAudit_Data)(Me, pageSize, pageIndex))
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
    Public Function SortBy(ByVal propertyName As String, Optional ByVal descending As Boolean = False) As CAudit_DataList
        Dim copy As New CAudit_DataList(Me)
        If Me.Count = 0 Then
            Return copy
        End If
        copy.Sort(New CAudit_DataList_SortBy(propertyName, descending, Me))
        Return copy
    End Function
    'Private 
    Private Class CAudit_DataList_SortBy : Inherits CReflection.GenericSortBy : Implements IComparer(Of CAudit_Data)
        Public Sub New(ByVal propertyName As String, ByVal descending As Boolean, ByVal list As IList)
            MyBase.New(propertyName, descending, list)
        End Sub
        Public Overloads Function Compare(ByVal x As CAudit_Data, ByVal y As CAudit_Data) As Integer Implements IComparer(Of CAudit_Data).Compare
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
        For Each i As CAudit_Data In Me
            i.Save(txOrNull)
        Next
    End Sub
    Public Sub DeleteAll(ByVal txOrNull As IDbTransaction)
        For Each i As CAudit_Data In Me
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
                For Each i As CAudit_Data In Me
                    list.Add(i.DataId)
                Next
                m_ids = list
            End If
            Return m_ids
        End Get
    End Property
    Public Function GetByIds(ByVal ids As List(Of Integer)) As CAudit_DataList
        Dim list As New CAudit_DataList(ids.Count)
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
    Public Shadows Sub Add(ByVal item As CAudit_Data)
        If Not IsNothing(_index) Then
            If Not _index.ContainsKey(item.DataId) Then
                _index(item.DataId) = item
            End If
        End If
        MyBase.Add(item)
    End Sub
    Public Shadows Sub Remove(ByVal item As CAudit_Data)
        If Not IsNothing(_index) Then
            If _index.ContainsKey(item.DataId) Then
                _index.Remove(item.DataId)
            End If
        End If
        MyBase.Remove(item)
    End Sub
    
    'Supplementary List Overloads
    Public Shadows Sub Add(ByVal itemsToAdd As IList(Of CAudit_Data))
        For Each i As CAudit_Data In itemsToAdd
            Add(i)
        Next
    End Sub
    Public Shadows Sub Remove(ByVal itemsToRemove As IList(Of CAudit_Data))
        For Each i As CAudit_Data In itemsToRemove
            Remove(i)
        Next
    End Sub
#End Region

#Region "Main Index (on DataId)"
    'Only uncomment if type is NOT an Integer (e.g. string/guid)
    'Default Public Overloads ReadOnly Property Item(ByVal dataId As Integer) As CAudit_Data
    '    Get
    '        Return GetById(dataId)
    '    End Get
    'End Property
    Public Function GetById(ByVal [dataId] As Integer) As CAudit_Data
        Dim c As CAudit_Data = Nothing
        Index.TryGetValue([dataId], c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of Integer, CAudit_Data)
    Private ReadOnly Property Index() As Dictionary(Of Integer, CAudit_Data)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _Index
                End If
            End If
            _index = New Dictionary(Of Integer, CAudit_Data)(Me.Count)
            For Each i As CAudit_Data in Me
                _index(i.DataId) = i
            Next
            Return _index
        End Get
    End Property
#End Region
    
#Region "Foreign-Key Indices (Subsets)"
    'Index by DataTrailId
    Public Function GetByTrailId(ByVal trailId As Integer) As CAudit_DataList
        Dim temp As CAudit_DataList = Nothing
        If Not IndexByTrailId.TryGetValue(trailId, temp) Then
            temp = New CAudit_DataList()
            IndexByTrailId(trailId) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByTrailId As Dictionary(Of Integer, CAudit_DataList)
    Private ReadOnly Property IndexByTrailId() As Dictionary(Of Integer, CAudit_DataList)
        Get
            If IsNothing(_indexByTrailId) Then
                'Instantiate
                Dim index As New Dictionary(Of Integer, CAudit_DataList)()

                'Populate
                Dim temp As CAudit_DataList = Nothing
                For Each i As CAudit_Data in Me
                    If Not index.TryGetValue(i.DataTrailId, temp) Then
                        temp = New CAudit_DataList()
                        index(i.DataTrailId) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByTrailId = index
            End If
            Return _indexByTrailId
        End Get
    End Property

    'Index by DataIsBefore
    Public Function GetByIsBefore(ByVal isBefore As Boolean) As CAudit_DataList
        Dim temp As CAudit_DataList = Nothing
        If Not IndexByIsBefore.TryGetValue(isBefore, temp) Then
            temp = New CAudit_DataList()
            IndexByIsBefore(isBefore) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByIsBefore As Dictionary(Of Boolean, CAudit_DataList)
    Private ReadOnly Property IndexByIsBefore() As Dictionary(Of Boolean, CAudit_DataList)
        Get
            If IsNothing(_indexByIsBefore) Then
                'Instantiate
                Dim index As New Dictionary(Of Boolean, CAudit_DataList)()

                'Populate
                Dim temp As CAudit_DataList = Nothing
                For Each i As CAudit_Data in Me
                    If Not index.TryGetValue(i.DataIsBefore, temp) Then
                        temp = New CAudit_DataList()
                        index(i.DataIsBefore) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByIsBefore = index
            End If
            Return _indexByIsBefore
        End Get
    End Property

#End Region

End Class