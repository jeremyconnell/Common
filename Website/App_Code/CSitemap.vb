Imports Microsoft.VisualBasic
Imports System.Web.HttpContext
Imports Framework


Public Enum ESource
    Local = -100
    Prod = -200
    Other = -300
End Enum
Public Enum EFix
    None = Integer.MinValue
    All = 0
    Indexes = 1
    StoredProcs = 2
    ForeignKeys = 3
    Views = 4
    Tables = 5
    Cols = 6
    DefaultVals = 7
    Migration = 10
    ScriptOnly = 1000
End Enum


Public Class CSitemap
    Public Shared Function Home() As String
        Return "~/default.aspx"
    End Function


#Region "Self"
    Public Shared Function SelfDeploy() As String
        Return "~/pages/self/deploy.aspx"
    End Function
    Public Shared Function SelfSchemaSync() As String
        Return "~/pages/self/schema.aspx"
    End Function
    Public Shared Function SelfDataSync() As String
        Return "~/pages/self/data.aspx"
    End Function
    Public Shared Function SelfSql() As String
        Return "~/pages/self/sql.aspx"
    End Function
    Public Shared Function SchemaView(instanceId As Integer) As String
        Return String.Concat("~/pages/self/schemaView.aspx?instanceId=", instanceId)
    End Function


    Public Shared Function SelfSchemaSync_Diff(instanceId As Integer) As String
        Return String.Concat("~/pages/self/schema.aspx?diffInstanceId=", instanceId)
    End Function
    Public Shared Function SelfSchemaSync_Diff(instanceId As Integer, fix As Integer) As String
        Return String.Concat(SelfSchemaSync_Diff(instanceId), "&fix=", Str(fix))
    End Function

    'UserControls
    Public Shared Function UCTableInfo() As String
        Return "~/pages/self/usercontrols/UCTable.ascx"
    End Function
    Public Shared Function UCColumn() As String
        Return "~/pages/self/usercontrols/UCColumn.ascx"
    End Function
    Public Shared Function UCStoredProc() As String
        Return "~/pages/self/usercontrols/UCStoredProc.ascx"
    End Function
    Public Shared Function UCForeignKey() As String
        Return "~/pages/self/usercontrols/UCForeignKey.ascx"
    End Function
    Public Shared Function UCDefaultValue() As String
        Return "~/pages/self/usercontrols/UCDefaultValue.ascx"
    End Function
    Public Shared Function UCIndex() As String
        Return "~/pages/self/usercontrols/UCIndex.ascx"
    End Function
    Public Shared Function UCView() As String
        Return "~/pages/self/usercontrols/UCView.ascx"
    End Function
#End Region

#Region "Audit_Logs"
    'List/Search
    Public Shared Function [Audit_Logs]() As String
        Return "~/pages/audit_Logs/default.aspx"
    End Function
    Public Shared Function [Audit_Logs](ByVal search As String, typeId As Integer) As String
        Return String.Concat(CSitemap.Audit_Logs(), "?search=", Encode(search), "&typeId=", Str(typeId))
    End Function
    Public Shared Function [Audit_Logs](ByVal search As String, typeId As Integer, ByVal pi As CPagingInfo) As String
        Return String.Concat(CSitemap.Audit_Logs(search, typeId), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
    End Function

    'Add/Edit
    Private Shared Function Audit_LogAddEdit() As String
        Return "~/pages/audit_Logs/audit_Log.aspx"
    End Function
    Public Shared Function Audit_LogAdd() As String 'May require a parentId (follow pattern below)
        Return Audit_LogAddEdit
    End Function
    Public Shared Function Audit_LogEdit(ByVal logId As Integer) As String
        Return String.Concat(Audit_LogAddEdit, "?logId=", logId)
    End Function

    'UserControls
    Public Shared Function UCAudit_Log() As String
        Return "~/pages//audit_Logs/usercontrols/UCAudit_Log.ascx"
    End Function

    'Folders (relative to /web project)
    Public Shared Function Audit_LogUploads() As String
        Return "~/uploads/audit_Logs/"
    End Function
#End Region

#Region "Clicks"
    'List/Search
    Public Shared Function [Clicks]() As String
        Return "~/pages/clicks/default.aspx"
    End Function
    Public Shared Function [Clicks](ByVal userName As String) As String
        Return String.Concat(CSitemap.Clicks(), "?userName=", Encode(userName))
    End Function
    Public Shared Function [Clicks](ByVal userName As String, ByVal url As String) As String
        Return String.Concat(CSitemap.Clicks(userName), "&url=", Encode(url))
    End Function
    Public Shared Function [Clicks](ByVal search As String, ByVal url As String, ByVal pi As CPagingInfo) As String
        Return String.Concat(CSitemap.Clicks(search, url), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
    End Function

    'UserControls
    Public Shared Function UCClick() As String
        Return "~/pages//clicks/usercontrols/UCClick.ascx"
    End Function
#End Region

#Region "Sessions"
    'List/Search
    Public Shared Function [Sessions]() As String
        Return "~/pages/sessions/default.aspx"
    End Function
    Public Shared Function [Sessions](ByVal search As String) As String
        Return String.Concat(CSitemap.Sessions(), "?search=", Encode(search))
    End Function
    Public Shared Function [Sessions](ByVal search As String, ByVal pi As CPagingInfo) As String
        Return String.Concat(CSitemap.Sessions(search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
    End Function
    
    'Add/Edit
    Private Shared Function SessionAddEdit() As String
        Return "~/pages/sessions/session.aspx"
    End Function
    Public Shared Function SessionAdd() As String 'May require a parentId (follow pattern below)
        Return SessionAddEdit
    End Function
    Public Shared Function SessionEdit(ByVal sessionId As Integer) As String
        Return String.Concat(SessionAddEdit, "?sessionId=", sessionId)
    End Function

    'UserControls
    Public Shared Function UCSession() As String
        Return "~/pages//sessions/usercontrols/UCSession.ascx"
    End Function
#End Region

#Region "Login"
    Public Shared Function Login() As String
        Return "~/login.aspx"
    End Function
#End Region

#Region "Audit_Errors"
    'List/Search
    Public Shared Function [Audit_Errors]() As String
        Return "~/pages/audit_Errors/default.aspx"
    End Function
    Public Shared Function [Audit_Errors](ByVal search As String, ByVal uniqueOnly As Boolean) As String
        Return String.Concat(CSitemap.Audit_Errors(), "?search=", Encode(search), "&uniqueOnly=", uniqueOnly)
    End Function
    Public Shared Function [Audit_Errors](ByVal search As String, ByVal uniqueOnly As Boolean, ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) As String
        Return String.Concat(CSitemap.Audit_Errors(search, uniqueOnly), "&sortBy=", sortBy, "&desc=", descending, "&p=", pageNumber)
    End Function

    'USercontrols
    Public Shared Function UCError() As String
        Return "~/pages/audit_Errors/usercontrols/UCAudit_Error.ascx"
    End Function
    
    'View Details
    Public Shared Function [Audit_Error](ByVal errorID As Integer) As String
        Return String.Concat("~/pages/audit_Errors/audit_Error.aspx?errorID=", errorID)
    End Function
    Public Shared Function Audit_Error(ByVal type1 As Integer, ByVal message1 As Integer, ByVal type2 As Integer, ByVal message2 As Integer) As String
        Return String.Concat("~/pages/audit_Errors/audit_Error.aspx?type1=", type1, "&message1=", message1, "&type2=", type2, "&message2=", message2)
    End Function
    Public Shared Function Audit_ErrorGroup(ByVal type1 As Integer, ByVal message1 As Integer, ByVal type2 As Integer, ByVal message2 As Integer) As String
        Return String.Concat("~/pages/audit_Errors/group.aspx?type1=", type1, "&message1=", message1, "&type2=", type2, "&message2=", message2)
    End Function
    Public Shared Function Audit_ErrorGroup(ByVal type1 As Integer, ByVal message1 As Integer, ByVal type2 As Integer, ByVal message2 As Integer, ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) As String
        Return String.Concat(CSitemap.Audit_ErrorGroup(type1, message1, type2, message2), "&sortBy=", sortBy, "&desc=", descending, "&p=", pageNumber)
    End Function
#End Region

#Region "Roles"
    'List/Search
    Public Shared Function [Roles]() As String
        Return "~/pages/roles/default.aspx"
    End Function
    Public Shared Function [Roles](ByVal pageIndex As Integer) As String
        Return String.Concat(CSitemap.Roles, "?p=", pageIndex)
    End Function

    'Add/Edit
    Private Shared Function RoleAddEdit() As String
        Return "~/pages/roles/role.aspx"
    End Function
    Public Shared Function RoleAdd() As String 'May require a parentId (follow pattern below)
        Return RoleAddEdit()
    End Function
    Public Shared Function RoleEdit(ByVal roleName As String) As String
        Return String.Concat(RoleAddEdit, "?roleName=", roleName)
    End Function
    Public Shared Function RoleEdit(ByVal roleName As String, ByVal search As String) As String
        Return String.Concat(RoleEdit(roleName), "&search=", Encode(search))
    End Function

    'Usercontrols
    Public Shared Function UCRole() As String
        Return "~/pages/roles/usercontrols/UCRole.ascx"
    End Function


    'Folders (relative to /web project)
    Public Shared Function RoleUploads() As String
        Return "/uploads/roles/"
    End Function
#End Region

#Region "Users"
    'List/Search
    Public Shared Function [Users]() As String
        Return "~/pages/users/default.aspx"
    End Function
    Public Shared Function [Users](ByVal search As String) As String
        Return String.Concat(CSitemap.Users, "?search=", Encode(search))
    End Function
    Public Shared Function [Users](ByVal search As String, ByVal pi As CPagingInfo) As String
        Return String.Concat(CSitemap.Users(search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
    End Function

    'Usercontrols
    Public Shared Function UCUser() As String
        Return "~/pages/users/usercontrols/UCUser.ascx"
    End Function

    'Add/Edit
    Private Shared Function UserAddEdit() As String
        Return "~/pages/users/user.aspx"
    End Function
    Public Shared Function UserAdd() As String 'May require a parentId (follow pattern below)
        Return UserAddEdit()
    End Function
    Public Shared Function UserEdit(ByVal userLoginName As String) As String
        Return String.Concat(UserAddEdit, "?userLoginName=", userLoginName)
    End Function
    Public Shared Function UserEdit(ByVal roleName As String, ByVal search As String) As String
        Return String.Concat(UserEdit(roleName), "&search=", Encode(search))
    End Function

    'Folders (relative to /web project)
    Public Shared Function UserUploads() As String
        Return "/uploads/users/"
    End Function
#End Region

#Region "UserRoles"
    'List/Search
    'Public Shared Function UserRolesForUser(ByVal userLoginName As String) As String
    '    Return String.Concat("~/pages/userRoles/ForUser.aspx?userLoginName=", userLoginName)
    'End Function
    'Public Shared Function UserRolesForRole(ByVal roleName As String) As String
    '    Return String.Concat("~/pages/userRoles/ForRole.aspx?roleName=", roleName)
    'End Function
    'Public Shared Function UserRolesForUser(ByVal userLoginName As String, ByVal search As String) As String
    '    Return String.Concat(UserRolesForUser(userLoginName), "&search=", Encode(search))
    'End Function
    'Public Shared Function UserRolesForRole(ByVal roleName As String, ByVal search As String) As String
    '    Return String.Concat(UserRolesForRole(roleName), "&search=", Encode(search))
    'End Function

    'Add/Edit
    Private Shared Function UserRoleAddEdit() As String
        Return "~/pages/userRoles/userRole.aspx"
    End Function
    Public Shared Function UserRoleAdd() As String 'May require a parentId (follow pattern below)
        Return UserRoleAddEdit()
    End Function
    Public Shared Function UserRoleEdit(ByVal uRUserLogin As String, ByVal uRRoleName As String) As String
        Return String.Concat(UserRoleAddEdit, "?uRUserLogin=", uRUserLogin)
    End Function

    'UserControls
    Public Shared Function UCUserRole() As String
        Return "~/pages/userRoles/usercontrols/UCUserRole.ascx"
    End Function
#End Region


#Region "Config Settings"
    Public Shared Function ConfigSettings() As String
        Return "~/pages/config-settings/default.aspx"
    End Function

    Public Shared Function GroupAdd() As String
        Return "~/pages/config-settings/group.aspx"
    End Function
    Public Shared Function GroupEdit(ByVal groupId As Integer) As String
        Return String.Concat(GroupAdd, "?groupId=", groupId)
    End Function

    'Usercontrols
    Public Shared Function UCSetting() As String
        Return "~/pages/config-settings/usercontrols/UCSetting.ascx"
    End Function
    Public Shared Function UCGroup() As String
        Return "~/pages/config-settings/usercontrols/UCGroup.ascx"
    End Function


    Private Shared Function SettingAddEdit() As String
        Return "~/pages/config-settings/setting.aspx"
    End Function
    Public Shared Function SettingAdd(ByVal groupId As Integer) As String
        Return String.Concat(SettingAddEdit, "?groupId=", groupId)
    End Function
    Public Shared Function SettingEdit(ByVal settingId As Integer) As String
        Return String.Concat(SettingAddEdit, "?settingId=", settingId)
    End Function
#End Region

#Region "Audit Trail"
    Public Shared Function AuditTrail() As String
        Return "~/pages/audit-trail/"
    End Function
#End Region

#Region "Standard"
    'Defaults
    Public Shared Function DefaultUploadsPath() As String
        Return "~/uploads/"
    End Function

    'Utilities
    Public Shared Function Encode(ByVal s As String) As String
        If String.IsNullOrEmpty(s) Then Return String.Empty
        Return Current.Server.UrlEncode(s)
    End Function
    Public Shared Function Str(ByVal i As Integer) As String
        If Integer.MinValue = i Then Return String.Empty
        Return i.ToString
    End Function
    Public Shared Function Str(ByVal i As Guid) As String
        If Guid.Empty.Equals(i) Then Return String.Empty
        Return i.ToString
    End Function
    Public Shared Function Str(ByVal i As DateTime) As String
        If DateTime.MinValue.Equals(i) Then Return String.Empty
        Return Encode(i.ToString)
    End Function
    Public Shared Function Str(ByVal i As Boolean?) As String
        If Not i.HasValue Then Return String.Empty
        Return i.Value
    End Function

    'Invalid ids
    Public Shared Sub RecordNotFound(ByVal entity As String)
        With HttpContext.Current.Request.QueryString
            If .Count > 0 Then RecordNotFound(entity, .Item(0)) Else RecordNotFound(entity, String.Empty)
        End With
    End Sub
    Public Shared Sub RecordNotFound(ByVal entity As String, ByVal value As Integer)
        RecordNotFound(entity, value.ToString)
    End Sub
    Public Shared Sub RecordNotFound(ByVal entity As String, ByVal pk1 As Object, ByVal pk2 As Object)
        RecordNotFound(entity, String.Concat(pk1, "/", pk2))
    End Sub
    Public Shared Sub RecordNotFound(ByVal entity As String, ByVal value As String)
        If String.IsNullOrEmpty(value) Then
            Throw New Exception(String.Concat("Invalid ", entity, "Id"))
        Else
            Throw New Exception(String.Concat("Invalid ", entity, "Id: ", value))
        End If
    End Sub
#End Region

End Class
