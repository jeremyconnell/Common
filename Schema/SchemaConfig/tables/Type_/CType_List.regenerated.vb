Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CType_List : Inherits List(Of CType_)

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
    Public Sub New(ByVal list As CType_List)
        MyBase.New(list)
        _index = list._index 
    End Sub

    'Generic list (eg. from paging control), have to assume type
    Public Sub New(ByVal list As IList)
        MyBase.New(list.Count)
        For Each i As CType_ In list
            MyBase.Add(i)
        Next
    End Sub
#End Region

#Region "Top/Bottom"
    Public Function Top(ByVal count As Integer) As CType_List
        Return New CType_List(CUtilities.Page(Me, count, 0))
    End Function

    Public Function Bottom(ByVal count As Integer) As CType_List
        If count > Me.Count Then count = Me.Count
        Return New CType_List(Me.GetRange(Me.Count - count - 1, count))
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
        For Each i As CType_ In Me
            i.Save(txOrNull)
        Next
    End Sub
    Public Sub DeleteAll(ByVal txOrNull As IDbTransaction)
        For Each i As CType_ In Me
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
    Public Shadows Sub Add(ByVal item As CType_)
        If Not IsNothing(_index) Then
            If Not _index.ContainsKey(item.TypeId) Then
                _index(item.TypeId) = item
            End If
        End If
        MyBase.Add(item)
    End Sub
    Public Shadows Sub Remove(ByVal item As CType_)
        If Not IsNothing(_index) Then
            If _index.ContainsKey(item.TypeId) Then
                _index.Remove(item.TypeId)
            End If
        End If
        MyBase.Remove(item)
    End Sub
#End Region

#Region "Main Index (on TypeId)"
    'Only uncomment if type is NOT an Integer (e.g. string/guid)
    'Default Public Overloads ReadOnly Property Item(ByVal typeId As Integer) As CType_
    '    Get
    '        Return GetById(typeId)
    '    End Get
    'End Property
    Public Function GetById(ByVal typeId As Integer) As CType_
        Dim c As CType_ = Nothing
        Index.TryGetValue(typeId, c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of Integer, CType_)
    Private ReadOnly Property Index() As Dictionary(Of Integer, CType_)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _Index
                End If
            End If
            _index = New Dictionary(Of Integer, CType_)(Me.Count)
            For Each i As CType_ in Me
                _index(i.TypeId) = i
            Next
            Return _index
        End Get
    End Property
#End Region
    
#Region "Foreign-Key Indices (Subsets)"
#End Region

End Class