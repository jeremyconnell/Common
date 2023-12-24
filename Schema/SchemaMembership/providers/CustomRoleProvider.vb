Imports System.Web.Security
Imports Framework

Public Class CustomRoleProvider : Inherits RoleProvider

#Region "Config - Constructor"
    Public Overrides Sub Initialize(ByVal name As String, ByVal config As System.Collections.Specialized.NameValueCollection)
        MyBase.Initialize(name, config)
    End Sub
#End Region

#Region "Interfaces - Trivial"
    Public Overrides Property ApplicationName() As String
        Get
            Return String.Empty
        End Get
        Set(ByVal value As String)
        End Set
    End Property
#End Region

#Region "Interfaces"
    Public Overrides Sub AddUsersToRoles(ByVal usernames() As String, ByVal roleNames() As String)
        Dim inserts As New CUserRoleList
        For Each un As String In usernames
            Dim user As CUser = CUser.GetByLogin(un, False)
            If IsNothing(user) Then Exit Sub
            For Each rn In roleNames
                If Not user.Roles.Contains(rn) Then
                    Dim ur As New CUserRole
                    ur.URRoleName = rn
                    ur.URUserLogin = un
                    inserts.Add(ur)
                End If
            Next
        Next
        CDataSrc.Default.BulkSave(inserts)
    End Sub


    Public Overrides Sub CreateRole(ByVal roleName As String)
        If Not IsNothing(CRole.Cache.GetById(roleName)) Then Exit Sub
        Dim r As New CRole()
        r.RoleName = roleName
        r.Save()
    End Sub

    Public Overrides Function DeleteRole(ByVal roleName As String, ByVal throwOnPopulatedRole As Boolean) As Boolean
        Dim r As CRole = CRole.Cache.GetById(roleName)
        If IsNothing(r) Then Return True
        If throwOnPopulatedRole AndAlso r.UserRoles.Count > 0 Then Throw New Exception("This role is currently in use")
        r.Delete()
    End Function

    Public Overrides Function FindUsersInRole(ByVal roleName As String, ByVal usernameToMatch As String) As String()
        Dim r As CRole = CRole.Cache.GetById(roleName)
        If IsNothing(r) Then Return New String() {}
        Dim list As New List(Of String)(r.UserRoles.Count)
        For Each i As CUserRole In r.UserRoles
            list.Add(i.URUserLogin)
        Next
        Return list.ToArray
    End Function

    Public Overrides Function GetAllRoles() As String()
        Dim list As New List(Of String)(CRole.Cache.Count)
        For Each i As CRole In CRole.Cache
            list.Add(i.RoleName)
        Next
        Return list.ToArray
    End Function

    Public Overrides Function GetRolesForUser(ByVal username As String) As String()
        Dim user As CUser = CUser.GetByLogin(username, False)
        If IsNothing(user) Then Return New String() {}
        Dim list As New List(Of String)(user.UserRoles.Count + 1)
        For Each i As CUserRole In user.UserRoles
            list.Add(i.URRoleName)
        Next
        list.Add(CRole.EVERYONE)
        Return list.ToArray
    End Function

    Public Overrides Function GetUsersInRole(ByVal roleName As String) As String()
        Dim r As CRole = CRole.Cache.GetById(roleName)
        If IsNothing(r) Then Return New String() {}

        Dim list As New List(Of String)(r.UserRoles.Count)
        For Each i As CUserRole In r.UserRoles
            Try
                list.Add(i.URUserLogin)
            Catch ex As Exception
            End Try
        Next
        Return list.ToArray
    End Function

    Public Overrides Function IsUserInRole(ByVal username As String, ByVal roleName As String) As Boolean
        If LCase(roleName) = LCase(CRole.EVERYONE) Then Return True
        Try
            With New CUserRole(username, roleName)
                Return True
            End With
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Overrides Sub RemoveUsersFromRoles(ByVal usernames() As String, ByVal roleNames() As String)
        For Each i In usernames
            For Each j In roleNames
                CUserRole.DeletePair(i, j)
            Next
        Next
    End Sub

    Public Overrides Function RoleExists(ByVal roleName As String) As Boolean
        Return Not IsNothing(CRole.Cache.GetById(roleName))
    End Function
#End Region

End Class
