Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CSettingList : Inherits List(Of CSetting)

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
    Public Sub New(ByVal list As CSettingList)
        MyBase.New(list)
        _index = list._index 
    End Sub

    'Generic list (eg. from paging control), have to assume type
    Public Sub New(ByVal list As IList)
        MyBase.New(list.Count)
        For Each i As CSetting In list
            MyBase.Add(i)
        Next
    End Sub
#End Region

#Region "Top/Bottom"
    Public Function Top(ByVal count As Integer) As CSettingList
        Return New CSettingList(CUtilities.Page(Me, count, 0))
    End Function

    Public Function Bottom(ByVal count As Integer) As CSettingList
        If count > Me.Count Then count = Me.Count
        Return New CSettingList(Me.GetRange(Me.Count - count - 1, count))
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
        For Each i As CSetting In Me
            i.Save(txOrNull)
        Next
    End Sub
    Public Sub DeleteAll(ByVal txOrNull As IDbTransaction)
        For Each i As CSetting In Me
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
    Public Shadows Sub Add(ByVal item As CSetting)
        If Not IsNothing(_index) Then
            If Not _index.ContainsKey(item.SettingId) Then
                _index(item.SettingId) = item
            End If
        End If
        MyBase.Add(item)
    End Sub
    Public Shadows Sub Remove(ByVal item As CSetting)
        If Not IsNothing(_index) Then
            If _index.ContainsKey(item.SettingId) Then
                _index.Remove(item.SettingId)
            End If
        End If
        MyBase.Remove(item)
    End Sub
#End Region

#Region "Main Index (on SettingId)"
    'Only uncomment if type is NOT an Integer (e.g. string/guid)
    'Default Public Overloads ReadOnly Property Item(ByVal settingId As Integer) As CSetting
    '    Get
    '        Return GetById(settingId)
    '    End Get
    'End Property
    Public Function GetById(ByVal settingId As Integer) As CSetting
        Dim c As CSetting = Nothing
        Index.TryGetValue(settingId, c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of Integer, CSetting)
    Private ReadOnly Property Index() As Dictionary(Of Integer, CSetting)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _Index
                End If
            End If
            _index = New Dictionary(Of Integer, CSetting)(Me.Count)
            For Each i As CSetting in Me
                _index(i.SettingId) = i
            Next
            Return _index
        End Get
    End Property
#End Region
    
#Region "Foreign-Key Indices (Subsets)"
    'Index by SettingGroupId
    Public Function GetByGroupId(ByVal groupId As Integer) As CSettingList
        Dim temp As CSettingList = Nothing
        If Not IndexByGroupId.TryGetValue(groupId, temp) Then
            temp = New CSettingList()
            IndexByGroupId(groupId) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByGroupId As Dictionary(Of Integer, CSettingList)
    Private ReadOnly Property IndexByGroupId() As Dictionary(Of Integer, CSettingList)
        Get
            If IsNothing(_indexByGroupId) Then
                'Instantiate
                Dim index As New Dictionary(Of Integer, CSettingList)()

                'Populate
                Dim temp As CSettingList = Nothing
                For Each i As CSetting in Me
                    If Not index.TryGetValue(i.SettingGroupId, temp) Then
                        temp = New CSettingList()
                        index(i.SettingGroupId) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByGroupId = index
            End If
            Return _indexByGroupId
        End Get
    End Property

    'Index by SettingTypeId
    Public Function GetByTypeId(ByVal typeId As Integer) As CSettingList
        Dim temp As CSettingList = Nothing
        If Not IndexByTypeId.TryGetValue(typeId, temp) Then
            temp = New CSettingList()
            IndexByTypeId(typeId) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByTypeId As Dictionary(Of Integer, CSettingList)
    Private ReadOnly Property IndexByTypeId() As Dictionary(Of Integer, CSettingList)
        Get
            If IsNothing(_indexByTypeId) Then
                'Instantiate
                Dim index As New Dictionary(Of Integer, CSettingList)()

                'Populate
                Dim temp As CSettingList = Nothing
                For Each i As CSetting in Me
                    If Not index.TryGetValue(i.SettingTypeId, temp) Then
                        temp = New CSettingList()
                        index(i.SettingTypeId) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByTypeId = index
            End If
            Return _indexByTypeId
        End Get
    End Property

    'Index by SettingListId
    Public Function GetByListId(ByVal listId As Integer) As CSettingList
        Dim temp As CSettingList = Nothing
        If Not IndexByListId.TryGetValue(listId, temp) Then
            temp = New CSettingList()
            IndexByListId(listId) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByListId As Dictionary(Of Integer, CSettingList)
    Private ReadOnly Property IndexByListId() As Dictionary(Of Integer, CSettingList)
        Get
            If IsNothing(_indexByListId) Then
                'Instantiate
                Dim index As New Dictionary(Of Integer, CSettingList)()

                'Populate
                Dim temp As CSettingList = Nothing
                For Each i As CSetting in Me
                    If Not index.TryGetValue(i.SettingListId, temp) Then
                        temp = New CSettingList()
                        index(i.SettingListId) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByListId = index
            End If
            Return _indexByListId
        End Get
    End Property

    'Index by SettingClientCanEdit
    Public Function GetByClientCanEdit(ByVal clientCanEdit As Boolean) As CSettingList
        Dim temp As CSettingList = Nothing
        If Not IndexByClientCanEdit.TryGetValue(clientCanEdit, temp) Then
            temp = New CSettingList()
            IndexByClientCanEdit(clientCanEdit) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByClientCanEdit As Dictionary(Of Boolean, CSettingList)
    Private ReadOnly Property IndexByClientCanEdit() As Dictionary(Of Boolean, CSettingList)
        Get
            If IsNothing(_indexByClientCanEdit) Then
                'Instantiate
                Dim index As New Dictionary(Of Boolean, CSettingList)()

                'Populate
                Dim temp As CSettingList = Nothing
                For Each i As CSetting in Me
                    If Not index.TryGetValue(i.SettingClientCanEdit, temp) Then
                        temp = New CSettingList()
                        index(i.SettingClientCanEdit) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByClientCanEdit = index
            End If
            Return _indexByClientCanEdit
        End Get
    End Property

    'Index by SettingValueBoolean
    Public Function GetByValueBoolean(ByVal valueBoolean As Boolean) As CSettingList
        Dim temp As CSettingList = Nothing
        If Not IndexByValueBoolean.TryGetValue(valueBoolean, temp) Then
            temp = New CSettingList()
            IndexByValueBoolean(valueBoolean) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByValueBoolean As Dictionary(Of Boolean, CSettingList)
    Private ReadOnly Property IndexByValueBoolean() As Dictionary(Of Boolean, CSettingList)
        Get
            If IsNothing(_indexByValueBoolean) Then
                'Instantiate
                Dim index As New Dictionary(Of Boolean, CSettingList)()

                'Populate
                Dim temp As CSettingList = Nothing
                For Each i As CSetting in Me
                    If Not index.TryGetValue(i.SettingValueBoolean, temp) Then
                        temp = New CSettingList()
                        index(i.SettingValueBoolean) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByValueBoolean = index
            End If
            Return _indexByValueBoolean
        End Get
    End Property

#End Region

#Region "Move Up/Down"
    Public Sub MoveUp(ByVal s As CSetting)
        Move(s, -1)
    End Sub
    Public Sub MoveDown(ByVal s As CSetting)
        Move(s, 1)
    End Sub

    'Private
    Private Sub Move(ByVal s As CSetting, ByVal change As Integer)
        If IsNothing(s) Then Exit Sub
        Dim index As Integer = Me.IndexOf(s) + change
        If index < 0 Then Exit Sub
        If index > Me.Count - 1 Then Exit Sub

        'Modify a copy of the array for threadsafety
        Dim dd As New CSettingList(Me.Count)
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
            Me(i).SettingSortOrder = i
        Next
    End Sub
#End Region

End Class