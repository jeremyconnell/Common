Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CUserList : Inherits List(Of CUser)

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
    Public Sub New(ByVal list As CUserList)
        MyBase.New(list)
        _index = list._index 
    End Sub

    'Generic list (eg. from paging control), have to assume type
    Public Sub New(ByVal list As IList)
        MyBase.New(list.Count)
        For Each i As CUser In list
            MyBase.Add(i)
        Next
    End Sub
#End Region

#Region "Top/Bottom/Page"
    Public Function Top(ByVal count As Integer) As CUserList
        If count >= Me.Count Then Return Me
        Return New CUserList(CUtilities.Page(Me, count, 0))
    End Function
    Public Function Bottom(ByVal count As Integer) As CUserList
        If count > Me.Count Then count = Me.Count
        Return New CUserList(Me.GetRange(Me.Count - count - 1, count))
    End Function
    Public Function Page(pageSize As Integer, pageIndex As Integer) As CUserList
        Return New CUserList( CUtilities.Page(Me, pageSize, pageIndex) )
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
    Public Function SortBy(ByVal propertyName As String, Optional ByVal descending As Boolean = False) As CUserList
        Dim copy As New CUserList(Me)
        If Me.Count = 0 Then
            Return copy
        End If
        copy.Sort(New CUserList_SortBy(propertyName, descending, Me))
        Return copy
    End Function
    'Private 
    Private Class CUserList_SortBy : Inherits CReflection.GenericSortBy : Implements IComparer(Of CUser)
        Public Sub New(ByVal propertyName As String, ByVal descending As Boolean, ByVal list As IList)
            MyBase.New(propertyName, descending, list)
        End Sub
        Public Overloads Function Compare(ByVal x As CUser, ByVal y As CUser) As Integer Implements IComparer(Of CUser).Compare
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
        For Each i As CUser In Me
            i.Save(txOrNull)
        Next
    End Sub
    Public Sub DeleteAll(ByVal txOrNull As IDbTransaction)
        For Each i As CUser In Me
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
    Private m_ids As List(Of String)
    Public ReadOnly Property Ids() As List(Of String)
        Get
            If m_ids Is Nothing Then
                Dim list As New List(Of String)(Me.Count)
                For Each i As CUser In Me
                    list.Add(i.UserLoginName)
                Next
                m_ids = list
            End If
            Return m_ids
        End Get
    End Property
    Public Function GetByIds(ByVal ids As List(Of String)) As CUserList
        Dim list As New CUserList(ids.Count)
        For Each id As String In ids
            If GetById(id) IsNot Nothing Then
                list.Add(GetById(id))
            End If
        Next
        Return list
    End Function
#End Region

#Region "Cache-Control"
    'Main Logic
    Public Shadows Sub Add(ByVal item As CUser)
        If Not IsNothing(_index) Then
            If Not _index.ContainsKey(item.UserLoginName) Then
                _index(item.UserLoginName) = item
            End If
        End If
        MyBase.Add(item)
    End Sub
    Public Shadows Sub Remove(ByVal item As CUser)
        If Not IsNothing(_index) Then
            If _index.ContainsKey(item.UserLoginName) Then
                _index.Remove(item.UserLoginName)
            End If
        End If
        MyBase.Remove(item)
    End Sub
    
    'Supplementary List Overloads
    Public Shadows Sub Add(ByVal itemsToAdd As IList(Of CUser))
        For Each i As CUser In itemsToAdd
            Add(i)
        Next
    End Sub
    Public Shadows Sub Remove(ByVal itemsToRemove As IList(Of CUser))
        For Each i As CUser In itemsToRemove
            Remove(i)
        Next
    End Sub
#End Region

#Region "Main Index (on UserLoginName)"
    'Only uncomment if type is NOT an Integer (e.g. string/guid)
    'Default Public Overloads ReadOnly Property Item(ByVal userLoginName As String) As CUser
    '    Get
    '        Return GetById(userLoginName)
    '    End Get
    'End Property
    Public Function GetById(ByVal [userLoginName] As String) As CUser
        Dim c As CUser = Nothing
        Index.TryGetValue([userLoginName], c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of String, CUser)
    Private ReadOnly Property Index() As Dictionary(Of String, CUser)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _Index
                End If
            End If
            _index = New Dictionary(Of String, CUser)(Me.Count)
            For Each i As CUser in Me
                _index(i.UserLoginName) = i
            Next
            Return _index
        End Get
    End Property
#End Region
    
#Region "Foreign-Key Indices (Subsets)"
    'Index by UserIsDisabled
    Public Function GetByIsDisabled(ByVal isDisabled As Boolean) As CUserList
        Dim temp As CUserList = Nothing
        If Not IndexByIsDisabled.TryGetValue(isDisabled, temp) Then
            temp = New CUserList()
            IndexByIsDisabled(isDisabled) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByIsDisabled As Dictionary(Of Boolean, CUserList)
    Private ReadOnly Property IndexByIsDisabled() As Dictionary(Of Boolean, CUserList)
        Get
            If IsNothing(_indexByIsDisabled) Then
                'Instantiate
                Dim index As New Dictionary(Of Boolean, CUserList)()

                'Populate
                Dim temp As CUserList = Nothing
                For Each i As CUser in Me
                    If Not index.TryGetValue(i.UserIsDisabled, temp) Then
                        temp = New CUserList()
                        index(i.UserIsDisabled) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByIsDisabled = index
            End If
            Return _indexByIsDisabled
        End Get
    End Property

    'Index by UserIsLockedOut
    Public Function GetByIsLockedOut(ByVal isLockedOut As Boolean) As CUserList
        Dim temp As CUserList = Nothing
        If Not IndexByIsLockedOut.TryGetValue(isLockedOut, temp) Then
            temp = New CUserList()
            IndexByIsLockedOut(isLockedOut) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByIsLockedOut As Dictionary(Of Boolean, CUserList)
    Private ReadOnly Property IndexByIsLockedOut() As Dictionary(Of Boolean, CUserList)
        Get
            If IsNothing(_indexByIsLockedOut) Then
                'Instantiate
                Dim index As New Dictionary(Of Boolean, CUserList)()

                'Populate
                Dim temp As CUserList = Nothing
                For Each i As CUser in Me
                    If Not index.TryGetValue(i.UserIsLockedOut, temp) Then
                        temp = New CUserList()
                        index(i.UserIsLockedOut) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByIsLockedOut = index
            End If
            Return _indexByIsLockedOut
        End Get
    End Property

#End Region

End Class