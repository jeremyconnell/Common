Imports System
Imports System.Text
Imports System.Data
Imports System.Collections.Generic

Imports Framework


'Table-Row Class (Customisable half)
Partial Public Class CRole

#Region "Constants"
    'Join Expressions
    Public Shared ROLE_JOIN_USERROLE As String = String.Concat(CRole.TABLE_NAME, " INNER JOIN ", CUserRole.TABLE_NAME, " ON RoleName=URRoleName")
    Public Shared ROLE_OUTER_JOIN_USERROLE As String = String.Concat(CRole.TABLE_NAME, " INNER JOIN ", CUserRole.TABLE_NAME, " ON RoleName=URRoleName")

    'Primary Key Values
    Public Const EVERYONE As String = "EVERYONE"
#End Region

#Region "Constructors (Public)"
    'Default Connection String
    Public Sub New()
        MyBase.New()
    End Sub
    
    'Alternative Connection String
    Public Sub New(ByVal dataSrc as CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    
    'Hidden (UI code should use cache instead)
    Protected Friend Sub New([roleName] As String)
        MyBase.New([roleName])
    End Sub
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, [roleName] As String)
        MyBase.New(dataSrc, [roleName])
    End Sub
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, ByVal [roleName] As String, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, [roleName], txOrNull)
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues_Custom()
        'm_sampleDateCreated = DateTime.Now
        'm_sampleSortOrder   = 0
    End Sub
#End Region

#Region "Default Connection String"
    Protected Overrides Function DefaultDataSrc() As CDataSrc
        Return CDataSrc.Default
    End Function
#End Region

#Region "Properties - Relationships"    
    'Relationships - Foriegn Keys (e.g parent)

    'Relationships - Collections (e.g. children)
    Public Function UserRoles() As CUserRoleList
        Return New CUserRole(DataSrc).SelectByRoleName(Me.RoleName)
    End Function
    Public Function UserRoles(ByVal pi As CPagingInfo) As CUserRoleList
        Return New CUserRole(DataSrc).SelectByRoleName(pi, Me.RoleName)
    End Function
    Public Function UserRolesCount() As Integer
        Return New CUserRole(DataSrc).SelectCountByRoleName(Me.RoleName)
    End Function

    'Relationships - 2-Step Walk (On-Demand)
    Public Function [Users]() As CUserList
        Return New CUser(DataSrc).SelectByRoleName(Me.RoleName)
    End Function
    Public Function [Users](ByVal pi As CPagingInfo) As CUserList
        Return New CUser(DataSrc).SelectByRoleName(pi, Me.RoleName)
    End Function

    Public Function RemainingUsers() As CUserList
        Return New CUser(DataSrc).SelectRemainingRoleName(Me.RoleName)
    End Function
    Public Function RemainingUsers(ByVal pi As CPagingInfo, ByVal search As String) As CUserList
        Return New CUser(DataSrc).SelectRemainingRoleName(pi, Me.RoleName)
    End Function
#End Region

#Region "Properties - Customisation"
    'Derived/ReadOnly (e.g. xml classes, presentation logic)
#End Region

#Region "Save/Delete Overrides"
     'Can Override MyBase.Save/Delete (e.g. Cascade deletes, or insert related records)
#End Region

#Region "Custom Database Queries"
    'Associative Table: 2-Step Walk
    Public Function SelectByUserLogin(ByVal userLogin As String) As CRoleList
        Return SelectByUserLogin(Nothing, userLogin, String.Empty)
    End Function
    Public Function SelectByUserLogin(ByVal userLogin As String, ByVal search As String) As CRoleList
        Return SelectByUserLogin(Nothing, userLogin)
    End Function
    Public Function SelectByUserLogin(ByVal pi As CPagingInfo, ByVal userLogin As String, ByVal search As String) As CRoleList
        Dim where As CCriteriaList = New CCriteriaList("RoleName", String.Concat("%", search, "%"))
        where.Add("URUserLogin", userLogin)
        If IsNothing(pi) Then
            Return SelectWhere(where, ROLE_JOIN_USERROLE)
        Else
            pi.TableName = ROLE_JOIN_USERROLE
            Return SelectWhere(pi, where)
        End If
    End Function

    Public Function SelectRemainingUserLogin(ByVal userLogin As String) As CRoleList
        Return SelectRemainingUserLogin(Nothing, userLogin, String.Empty)
    End Function
    Public Function SelectRemainingUserLogin(ByVal userLogin As String, ByVal search As String) As CRoleList
        Return SelectRemainingUserLogin(Nothing, userLogin, search)
    End Function
    Public Function SelectRemainingUserLogin(ByVal pi As CPagingInfo, ByVal userLogin As String, ByVal search As String) As CRoleList
        Dim join As String = String.Concat(ROLE_OUTER_JOIN_USERROLE, " AND URRoleName=", userLogin)
        Dim where As CCriteriaList = New CCriteriaList("RoleName", String.Concat("%", search, "%"))
        where.Add("URRoleName", Nothing)
        If IsNothing(pi) Then
            Return SelectWhere(where, join)
        Else
            pi.TableName = join
            Return SelectWhere(pi, where)
        End If
    End Function
#End Region

#Region "Searching (Optional)"
    'For cached classes, custom seach logic resides in static methods on the list class
    ' e.g. CRole.Cache.Search("..")

    'See also the auto-generated methods based on indexes
    ' e.g. CRole.Cache.GetBy...
#End Region

#Region "Caching Details"
    'Cache Key
    Friend Shared CACHE_KEY As String = GetType(CRole).ToString 'TABLE_NAME

    'Cache data
    Private Shared Function LoadCache() As CRoleList
        Return New CRole().SelectAll()
    End Function
    
    'Cache Timeout
    Private Shared Sub SetCache(ByVal key As String, ByVal value As CRoleList)
        If Not IsNothing(value) Then value.Sort()
        CCache.Set(key, value) 'Optional parameter can override timeout (otherwise uses config-settings, which default to 3hrs)
    End Sub
    
    'Helper Method
    Private Function CacheGetById(ByVal list As CRoleList) As CRole
        Return list.GetById(Me.RoleName)
    End Function
#End Region

#Region "Cloning"
    Public Function Clone(ByVal target as CDataSrc, ByVal txOrNull as IDbTransaction) As CRole ', parentId As Integer) As CRole
        'Shallow copy: Copies the immediate record, excluding autogenerated Pks
        Dim copy As New CRole(Me, target)
        copy.Save(txOrNull)

        'Deep Copy - Child Entities: Cloned children must reference their cloned parent
        'copy.SampleParentId = parentId

        'Deep Copy - Parent Entities: Cloned parents also clone their child collections
        'Me.Children.Clone(target, txOrNull, copy.RoleName)

        Return copy
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Custom(ByVal w As System.Xml.XmlWriter)
        'Store(w, "Example", Me.Example)
    End Sub
#End Region

End Class
