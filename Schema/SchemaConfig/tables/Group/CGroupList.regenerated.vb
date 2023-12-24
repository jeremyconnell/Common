Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CGroupList : Inherits List(Of CGroup)

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
    Public Sub New(ByVal list As CGroupList)
        MyBase.New(list)
        _index = list._index 
    End Sub

    'Generic list (eg. from paging control), have to assume type
    Public Sub New(ByVal list As IList)
        MyBase.New(list.Count)
        For Each i As CGroup In list
            MyBase.Add(i)
        Next
    End Sub
#End Region

#Region "Top/Bottom"
    Public Function Top(ByVal count As Integer) As CGroupList
        Return New CGroupList(CUtilities.Page(Me, count, 0))
    End Function

    Public Function Bottom(ByVal count As Integer) As CGroupList
        If count > Me.Count Then count = Me.Count
        Return New CGroupList(Me.GetRange(Me.Count - count - 1, count))
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
        For Each i As CGroup In Me
            i.Save(txOrNull)
        Next
    End Sub
    Public Sub DeleteAll(ByVal txOrNull As IDbTransaction)
        For Each i As CGroup In Me
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
    Public Shadows Sub Add(ByVal item As CGroup)
        If Not IsNothing(_index) Then
            If Not _index.ContainsKey(item.GroupId) Then
                _index(item.GroupId) = item
            End If
        End If
        MyBase.Add(item)
    End Sub
    Public Shadows Sub Remove(ByVal item As CGroup)
        If Not IsNothing(_index) Then
            If _index.ContainsKey(item.GroupId) Then
                _index.Remove(item.GroupId)
            End If
        End If
        MyBase.Remove(item)
    End Sub
#End Region

#Region "Main Index (on GroupId)"
    'Only uncomment if type is NOT an Integer (e.g. string/guid)
    'Default Public Overloads ReadOnly Property Item(ByVal groupId As Integer) As CGroup
    '    Get
    '        Return GetById(groupId)
    '    End Get
    'End Property
    Public Function GetById(ByVal groupId As Integer) As CGroup
        Dim c As CGroup = Nothing
        Index.TryGetValue(groupId, c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of Integer, CGroup)
    Private ReadOnly Property Index() As Dictionary(Of Integer, CGroup)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _Index
                End If
            End If
            _index = New Dictionary(Of Integer, CGroup)(Me.Count)
            For Each i As CGroup in Me
                _index(i.GroupId) = i
            Next
            Return _index
        End Get
    End Property
#End Region
    
#Region "Foreign-Key Indices (Subsets)"
#End Region

#Region "Move Up/Down"
    Public Sub MoveUp(ByVal s As CGroup)
        Move(s, -1)
    End Sub
    Public Sub MoveDown(ByVal s As CGroup)
        Move(s, 1)
    End Sub

    'Private
    Private Sub Move(ByVal s As CGroup, ByVal change As Integer)
        If IsNothing(s) Then Exit Sub
        Dim index As Integer = Me.IndexOf(s) + change
        If index < 0 Then Exit Sub
        If index > Me.Count - 1 Then Exit Sub

        'Modify a copy of the array for threadsafety
        Dim dd As New CGroupList(Me.Count)
        dd.AddRange(Me)
        With dd
            .Remove(s)
            .Insert(index, s)
            .ResetOrdinals()
        End With

        Me.SaveAll()
    End Sub
    Private Sub ResetOrdinals()
        For i As Integer = 0 To Me.Count - 1
            Me(i).GroupSortOrder = i
        Next
    End Sub
#End Region

End Class