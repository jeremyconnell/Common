Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CListList : Inherits List(Of CList)

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
    Public Sub New(ByVal list As CListList)
        MyBase.New(list)
        _index = list._index 
    End Sub

    'Generic list (eg. from paging control), have to assume type
    Public Sub New(ByVal list As IList)
        MyBase.New(list.Count)
        For Each i As CList In list
            MyBase.Add(i)
        Next
    End Sub
#End Region

#Region "Top/Bottom"
    Public Function Top(ByVal count As Integer) As CListList
        Return New CListList(CUtilities.Page(Me, count, 0))
    End Function

    Public Function Bottom(ByVal count As Integer) As CListList
        If count > Me.Count Then count = Me.Count
        Return New CListList(Me.GetRange(Me.Count - count - 1, count))
    End Function
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
        For Each i As CList In Me
            i.Save(txOrNull)
        Next
    End Sub
    Public Sub DeleteAll(ByVal txOrNull As IDbTransaction)
        For Each i As CList In Me
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

#Region "Cache-Control"
    Public Shadows Sub Add(ByVal item As CList)
        If Not IsNothing(_index) Then
            If Not _index.ContainsKey(item.ListId) Then
                _index(item.ListId) = item
            End If
        End If
        MyBase.Add(item)
    End Sub
    Public Shadows Sub Remove(ByVal item As CList)
        If Not IsNothing(_index) Then
            If _index.ContainsKey(item.ListId) Then
                _index.Remove(item.ListId)
            End If
        End If
        MyBase.Remove(item)
    End Sub
#End Region

#Region "Main Index (on ListId)"
    'Only uncomment if type is NOT an Integer (e.g. string/guid)
    'Default Public Overloads ReadOnly Property Item(ByVal listId As Integer) As CList
    '    Get
    '        Return GetById(listId)
    '    End Get
    'End Property
    Public Function GetById(ByVal listId As Integer) As CList
        Dim c As CList = Nothing
        Index.TryGetValue(listId, c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of Integer, CList)
    Private ReadOnly Property Index() As Dictionary(Of Integer, CList)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _Index
                End If
            End If
            _index = New Dictionary(Of Integer, CList)(Me.Count)
            For Each i As CList in Me
                _index(i.ListId) = i
            Next
            Return _index
        End Get
    End Property
#End Region
    
#Region "Foreign-Key Indices (Subsets)"
    'Index by ListIsExternal
    Public Function GetByIsExternal(ByVal isExternal As Boolean) As CListList
        Dim temp As CListList = Nothing
        If Not IndexByIsExternal.TryGetValue(isExternal, temp) Then
            temp = New CListList()
            IndexByIsExternal(isExternal) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByIsExternal As Dictionary(Of Boolean, CListList)
    Private ReadOnly Property IndexByIsExternal() As Dictionary(Of Boolean, CListList)
        Get
            If IsNothing(_indexByIsExternal) Then
                'Instantiate
                Dim index As New Dictionary(Of Boolean, CListList)()

                'Populate
                Dim temp As CListList = Nothing
                For Each i As CList in Me
                    If Not index.TryGetValue(i.ListIsExternal, temp) Then
                        temp = New CListList()
                        index(i.ListIsExternal) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByIsExternal = index
            End If
            Return _indexByIsExternal
        End Get
    End Property

#End Region

End Class