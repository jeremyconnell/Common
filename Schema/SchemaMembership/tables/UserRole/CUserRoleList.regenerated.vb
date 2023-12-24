Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CUserRoleList : Inherits List(Of CUserRole)

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
    Public Sub New(ByVal list As CUserRoleList)
        MyBase.New(list)
        _index = list._index 
    End Sub

    'Generic list (eg. from paging control), have to assume type
    Public Sub New(ByVal list As IList)
        MyBase.New(list.Count)
        For Each i As CUserRole In list
            MyBase.Add(i)
        Next
    End Sub
#End Region

#Region "Top/Bottom/Page"
    Public Function Top(ByVal count As Integer) As CUserRoleList
        If count >= Me.Count Then Return Me
        Return New CUserRoleList(CUtilities.Page(Me, count, 0))
    End Function
    Public Function Bottom(ByVal count As Integer) As CUserRoleList
        If count > Me.Count Then count = Me.Count
        Return New CUserRoleList(Me.GetRange(Me.Count - count - 1, count))
    End Function
    Public Function Page(pageSize As Integer, pageIndex As Integer) As CUserRoleList
        Return New CUserRoleList( CUtilities.Page(Me, pageSize, pageIndex) )
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
    Public Function SortBy(ByVal propertyName As String, Optional ByVal descending As Boolean = False) As CUserRoleList
        Dim copy As New CUserRoleList(Me)
        If Me.Count = 0 Then
            Return copy
        End If
        copy.Sort(New CUserRoleList_SortBy(propertyName, descending, Me))
        Return copy
    End Function
    'Private 
    Private Class CUserRoleList_SortBy : Inherits CReflection.GenericSortBy : Implements IComparer(Of CUserRole)
        Public Sub New(ByVal propertyName As String, ByVal descending As Boolean, ByVal list As IList)
            MyBase.New(propertyName, descending, list)
        End Sub
        Public Overloads Function Compare(ByVal x As CUserRole, ByVal y As CUserRole) As Integer Implements IComparer(Of CUserRole).Compare
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
        For Each i As CUserRole In Me
            i.Save(txOrNull)
        Next
    End Sub
    Public Sub DeleteAll(ByVal txOrNull As IDbTransaction)
        For Each i As CUserRole In Me
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
    'Main Logic
    Public Shadows Sub Add(ByVal item As CUserRole)
        If Not IsNothing(_index) Then
            Dim k As String = Key(item.URUserLogin, item.URRoleName)
            If Not _index.ContainsKey(k) Then
                _index(k) = item
            End If
        End If
        MyBase.Add(item)
    End Sub
    Public Shadows Sub Remove(ByVal item As CUserRole)
        If Not IsNothing(_index) Then
            Dim k As String = Key(item.URUserLogin, item.URRoleName)
            If _index.ContainsKey(k) Then
                _index.Remove(k)
            End If
        End If
        MyBase.Remove(item)
    End Sub
    
    'Supplementary List Overloads
    Public Shadows Sub Add(ByVal itemsToAdd As IList(Of CUserRole))
        For Each i As CUserRole In itemsToAdd
            Add(i)
        Next
    End Sub
    Public Shadows Sub Remove(ByVal itemsToRemove As IList(Of CUserRole))
        For Each i As CUserRole In itemsToRemove
            Remove(i)
        Next
    End Sub
#End Region

#Region "Main Index (on URUserLogin, URRoleName)"
    Default Public Overloads ReadOnly Property Item(ByVal uRUserLogin As String, ByVal uRRoleName As String) As CUserRole
        Get
            Return GetById(uRUserLogin, uRRoleName)
        End Get
    End Property
    Public Shared Function Key(ByVal uRUserLogin As String, ByVal uRRoleName As String) As String
        Return uRUserLogin.ToString() + "_" + uRRoleName.ToString()
    End Function
    Public Function GetById(ByVal uRUserLogin As String, ByVal uRRoleName As String) As CUserRole
        Dim c As CUserRole = Nothing
        Dim k As String = Key(uRUserLogin, uRRoleName)
        Index.TryGetValue(k, c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of String, CUserRole)
    Private ReadOnly Property Index() As Dictionary(Of String, CUserRole)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _Index
                End If
            End If

            _index = New Dictionary(Of String, CUserRole)(Me.Count)
            For Each i As CUserRole in Me
                _index(Key(i.URUserLogin, i.URRoleName)) = i
            Next    
            Return _index
        End Get
    End Property
#End Region

#Region "Foreign-Key Indices (Subsets)"
	'Index by URUserLogin
	Public Function GetByUserLogin(ByVal userLogin As String) As CUserRoleList
        Dim temp As CUserRoleList = Nothing
        If Not IndexByUserLogin.TryGetValue(userLogin, temp) Then
            temp = New CUserRoleList()
            IndexByUserLogin(userLogin) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByUserLogin As Dictionary(Of String, CUserRoleList)
    Private ReadOnly Property IndexByUserLogin() As Dictionary(Of String, CUserRoleList)
        Get
            If IsNothing(_indexByUserLogin) Then
                'Instantiate
                Dim index As New Dictionary(Of String, CUserRoleList)()

                'Populate
                Dim temp As CUserRoleList = Nothing
                For Each i As CUserRole in Me
                    If Not index.TryGetValue(i.URUserLogin, temp) Then
                        temp = New CUserRoleList()
                        index(i.URUserLogin) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByUserLogin = index
            End If
            Return _indexByUserLogin
        End Get
    End Property

    'Index by URRoleName
    Public Function GetByRoleName(ByVal roleName As String) As CUserRoleList
        Dim temp As CUserRoleList = Nothing
        If Not IndexByRoleName.TryGetValue(roleName, temp) Then
            temp = New CUserRoleList()
            IndexByRoleName(roleName) = temp
        End If
        Return temp
    End Function
    <NonSerialized()> _
    Private _indexByRoleName As Dictionary(Of String, CUserRoleList)
    Private ReadOnly Property IndexByRoleName() As Dictionary(Of String, CUserRoleList)
        Get
            If IsNothing(_indexByRoleName) Then
                'Instantiate
                Dim index As New Dictionary(Of String, CUserRoleList)()

                'Populate
                Dim temp As CUserRoleList = Nothing
                For Each i As CUserRole in Me
                    If Not index.TryGetValue(i.URRoleName, temp) Then
                        temp = New CUserRoleList()
                        index(i.URRoleName) = temp
                    End If
                    temp.Add(i)
                Next

                'Store
                _indexByRoleName = index
            End If
            Return _indexByRoleName
        End Get
    End Property

#End Region

End Class
