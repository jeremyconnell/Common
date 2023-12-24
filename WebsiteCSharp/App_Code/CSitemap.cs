using Microsoft.VisualBasic;
using Framework;
using System.Web;
using System;

public enum ESource
{

    Local = -100,

    Prod = -200,

    Other = -300,
}
public enum EFix
{

    None = int.MinValue,

    All = 0,

    Indexes = 1,

    StoredProcs = 2,

    ForeignKeys = 3,

    Views = 4,

    Tables = 5,

    Cols = 6,

    DefaultVals = 7,

    Migration = 10,

    ScriptOnly = 1000,
}
public class CSitemap
{

    public static string Home()
    {
        return "~/default.aspx";
    }

    public static string SelfDeploy()
    {
        return "~/pages/self/deploy.aspx";
    }

    public static string SelfSchemaSync()
    {
        return "~/pages/self/schema.aspx";
    }

    public static string SelfDataSync()
    {
        return "~/pages/self/data.aspx";
    }

    public static string SelfSql()
    {
        return "~/pages/self/sql.aspx";
    }

    public static string SchemaView(int instanceId)
    {
        return string.Concat("~/pages/self/schemaView.aspx?instanceId=", instanceId);
    }

    public static string SelfSchemaSync_Diff(int instanceId)
    {
        return string.Concat("~/pages/self/schema.aspx?diffInstanceId=", instanceId);
    }

    public static string SelfSchemaSync_Diff(int instanceId, int fix)
    {
        return string.Concat(CSitemap.SelfSchemaSync_Diff(instanceId), "&fix=", CSitemap.Str(fix));
    }

    // UserControls
    public static string UCTableInfo()
    {
        return "~/pages/self/usercontrols/UCTable.ascx";
    }

    public static string UCColumn()
    {
        return "~/pages/self/usercontrols/UCColumn.ascx";
    }

    public static string UCStoredProc()
    {
        return "~/pages/self/usercontrols/UCStoredProc.ascx";
    }

    public static string UCForeignKey()
    {
        return "~/pages/self/usercontrols/UCForeignKey.ascx";
    }

    public static string UCDefaultValue()
    {
        return "~/pages/self/usercontrols/UCDefaultValue.ascx";
    }

    public static string UCIndex()
    {
        return "~/pages/self/usercontrols/UCIndex.ascx";
    }

    public static string UCView()
    {
        return "~/pages/self/usercontrols/UCView.ascx";
    }

    // List/Search
    public static string Audit_Logs()
    {
        return "~/pages/audit_Logs/default.aspx";
    }

    public static string Audit_Logs(string search, int typeId)
    {
        return string.Concat(CSitemap.Audit_Logs(), "?search=", CSitemap.Encode(search), "&typeId=", CSitemap.Str(typeId));
    }

    public static string Audit_Logs(string search, int typeId, CPagingInfo pi)
    {
        return string.Concat(CSitemap.Audit_Logs(search, typeId), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", (pi.PageIndex + 1));
    }

    // Add/Edit
    private static string Audit_LogAddEdit()
    {
        return "~/pages/audit_Logs/audit_Log.aspx";
    }

    public static string Audit_LogAdd()
    {
        // May require a parentId (follow pattern below)
        return Audit_LogAddEdit();
    }

    public static string Audit_LogEdit(int logId)
    {
        return string.Concat(Audit_LogAddEdit(), "?logId=", logId);
    }

    // UserControls
    public static string UCAudit_Log()
    {
        return "~/pages//audit_Logs/usercontrols/UCAudit_Log.ascx";
    }

    // Folders (relative to /web project)
    public static string Audit_LogUploads()
    {
        return "~/uploads/audit_Logs/";
    }

    public static string Login()
    {
        return "~/login.aspx";
    }

    // List/Search
    public static string Audit_Errors()
    {
        return "~/pages/audit_Errors/default.aspx";
    }

    public static string Audit_Errors(string search, bool uniqueOnly)
    {
        return string.Concat(CSitemap.Audit_Errors(), "?search=", CSitemap.Encode(search), "&uniqueOnly=", uniqueOnly);
    }

    public static string Audit_Errors(string search, bool uniqueOnly, string sortBy, bool descending, int pageNumber)
    {
        return string.Concat(CSitemap.Audit_Errors(search, uniqueOnly), "&sortBy=", sortBy, "&desc=", descending, "&p=", pageNumber);
    }

    // USercontrols
    public static string UCError()
    {
        return "~/pages/audit_Errors/usercontrols/UCAudit_Error.ascx";
    }

    // View Details
    public static string Audit_Error(int errorID)
    {
        return string.Concat("~/pages/audit_Errors/audit_Error.aspx?errorID=", errorID);
    }

    public static string Audit_Error(int type1, int message1, int type2, int message2)
    {
        return string.Concat("~/pages/audit_Errors/audit_Error.aspx?type1=", type1, "&message1=", message1, "&type2=", type2, "&message2=", message2);
    }

    public static string Audit_ErrorGroup(int type1, int message1, int type2, int message2)
    {
        return string.Concat("~/pages/audit_Errors/group.aspx?type1=", type1, "&message1=", message1, "&type2=", type2, "&message2=", message2);
    }

    public static string Audit_ErrorGroup(int type1, int message1, int type2, int message2, string sortBy, bool descending, int pageNumber)
    {
        return string.Concat(CSitemap.Audit_ErrorGroup(type1, message1, type2, message2), "&sortBy=", sortBy, "&desc=", descending, "&p=", pageNumber);
    }

    // List/Search
    public static string Roles()
    {
        return "~/pages/roles/default.aspx";
    }

    public static string Roles(int pageIndex)
    {
        return string.Concat(CSitemap.Roles(), "?p=", pageIndex);
    }

    // Add/Edit
    private static string RoleAddEdit()
    {
        return "~/pages/roles/role.aspx";
    }

    public static string RoleAdd()
    {
        // May require a parentId (follow pattern below)
        return CSitemap.RoleAddEdit();
    }

    public static string RoleEdit(string roleName)
    {
        return string.Concat(RoleAddEdit(), "?roleName=", roleName);
    }

    public static string RoleEdit(string roleName, string search)
    {
        return string.Concat(CSitemap.RoleEdit(roleName), "&search=", CSitemap.Encode(search));
    }

    // Usercontrols
    public static string UCRole()
    {
        return "~/pages/roles/usercontrols/UCRole.ascx";
    }

    // Folders (relative to /web project)
    public static string RoleUploads()
    {
        return "/uploads/roles/";
    }

    // List/Search
    public static string Users()
    {
        return "~/pages/users/default.aspx";
    }

    public static string Users(string search)
    {
        return string.Concat(CSitemap.Users(), "?search=", CSitemap.Encode(search));
    }

    public static string Users(string search, CPagingInfo pi)
    {
        return string.Concat(CSitemap.Users(search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", (pi.PageIndex + 1));
    }

    // Usercontrols
    public static string UCUser()
    {
        return "~/pages/users/usercontrols/UCUser.ascx";
    }

    // Add/Edit
    private static string UserAddEdit()
    {
        return "~/pages/users/user.aspx";
    }

    public static string UserAdd()
    {
        // May require a parentId (follow pattern below)
        return CSitemap.UserAddEdit();
    }

    public static string UserEdit(string userLoginName)
    {
        return string.Concat(UserAddEdit(), "?userLoginName=", userLoginName);
    }

    public static string UserEdit(string roleName, string search)
    {
        return string.Concat(CSitemap.UserEdit(roleName), "&search=", CSitemap.Encode(search));
    }

    // Folders (relative to /web project)
    public static string UserUploads()
    {
        return "/uploads/users/";
    }

    // List/Search
    // Public Shared Function UserRolesForUser(ByVal userLoginName As String) As String
    //     Return string.Concat("~/pages/userRoles/ForUser.aspx?userLoginName=", userLoginName)
    // End Function
    // Public Shared Function UserRolesForRole(ByVal roleName As String) As String
    //     Return string.Concat("~/pages/userRoles/ForRole.aspx?roleName=", roleName)
    // End Function
    // Public Shared Function UserRolesForUser(ByVal userLoginName As String, ByVal search As String) As String
    //     Return string.Concat(UserRolesForUser(userLoginName), "&search=", Encode(search))
    // End Function
    // Public Shared Function UserRolesForRole(ByVal roleName As String, ByVal search As String) As String
    //     Return string.Concat(UserRolesForRole(roleName), "&search=", Encode(search))
    // End Function
    // Add/Edit
    private static string UserRoleAddEdit()
    {
        return "~/pages/userRoles/userRole.aspx";
    }

    public static string UserRoleAdd()
    {
        // May require a parentId (follow pattern below)
        return CSitemap.UserRoleAddEdit();
    }

    public static string UserRoleEdit(string uRUserLogin, string uRRoleName)
    {
        return string.Concat(UserRoleAddEdit(), "?uRUserLogin=", uRUserLogin);
    }

    // UserControls
    public static string UCUserRole()
    {
        return "~/pages/userRoles/usercontrols/UCUserRole.ascx";
    }

    public static string AuditTrail()
    {
        return "~/pages/audit-trail/";
    }

    // Defaults
    public static string DefaultUploadsPath()
    {
        return "~/uploads/";
    }

    // Utilities
    public static string Encode(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }

        return HttpContext.Current.Server.UrlEncode(s);
    }

    public static string Str(int i)
    {
        if ((int.MinValue == i))
        {
            return string.Empty;
        }

        return i.ToString();
    }

    public static string Str(Guid i)
    {
        if (Guid.Empty.Equals(i))
        {
            return string.Empty;
        }

        return i.ToString();
    }

    public static string Str(DateTime i)
    {
        if (DateTime.MinValue.Equals(i))
        {
            return string.Empty;
        }

        return CSitemap.Encode(i.ToString());
    }

    public static string Str(bool? i)
    {
        if (!i.HasValue)
        {
            return string.Empty;
        }

        return i.Value.ToString();
    }

    // Invalid ids
    public static void RecordNotFound(string entity)
    {
        // With...
        if ((HttpContext.Current.Request.QueryString.Count > 0))

            CSitemap.RecordNotFound(entity, HttpContext.Current.Request.QueryString[0]);
        else
        {
            RecordNotFound(entity, string.Empty);
        }

    }

    public static void RecordNotFound(string entity, int value)
    {
        CSitemap.RecordNotFound(entity, value.ToString());
    }

    public static void RecordNotFound(string entity, object pk1, object pk2)
    {
        CSitemap.RecordNotFound(entity, string.Concat(pk1, "/", pk2));
    }

    public static void RecordNotFound(string entity, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new Exception(string.Concat("Invalid ", entity, "Id"));
        }
        else
        {
            throw new Exception(string.Concat("Invalid ", entity, "Id: ", value));
        }

    }
}