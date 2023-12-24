Imports System.Web
Imports Comms.PushUpgrade.Interface

Public Class CUpgradeClient_Config : Inherits CClient_Config

#Region "Constants"
    Public Const COMMON_PREFIX As String = "Upgrade."

    Public Const DEFAULT_HOSTNAME As String = "crypton.azurewebsites.net"
    Public Const DEFAULT_USE_SSL As Boolean = True
    Public Const DEFAULT_IGNORES As String = CFolderHash.IGNORE_WEB & ",.xml,.pdb,.refresh,.pfx,.cer"


    Private Const KEY_AppId As String = "AppId"
    Private Const KEY_InstanceId As String = "InstanceId"
    Private Const KEY_InstanceName As String = "InstanceName"
    Private Const KEY_IsDevMachine As String = "IsDevMachine"
    Private Const KEY_ExtensionsToIgnore As String = "ExtensionsToIgnore"

    Private Shared PREFIXED_AppId As String = COMMON_PREFIX & KEY_AppId
    Private Shared PREFIXED_InstanceId As String = COMMON_PREFIX & KEY_InstanceId
    Private Shared PREFIXED_InstanceName As String = COMMON_PREFIX & KEY_InstanceName
    Private Shared PREFIXED_IsDevMachine As String = COMMON_PREFIX & KEY_IsDevMachine
    Private Shared PREFIXED_ExtensionsToIgnore As String = COMMON_PREFIX & KEY_ExtensionsToIgnore
#End Region

#Region "Overrides (Prefix, Defaults)"
    Protected Overrides ReadOnly Property KEY_CommonPrefix As String
        Get
            Return COMMON_PREFIX
        End Get
    End Property
    Protected Overrides ReadOnly Property DefaultHostName As String
        Get
            Return DEFAULT_HOSTNAME
        End Get
    End Property
    Protected Overrides ReadOnly Property DefaultUseSsl As String
        Get
            Return DEFAULT_USE_SSL
        End Get
    End Property

    Protected ReadOnly Property DefaultInstanceName As String
        Get
            If IsNothing(HttpContext.Current) Then
                Return My.Computer.Name
            Else
                Return HttpContext.Current.Request.Url.Host
            End If
        End Get
    End Property
    Protected ReadOnly Property DefaultIsDevMachine As Boolean
        Get
            Return My.Computer.Name.ToUpper = "DESKTOP-KJNAST7" OrElse My.Computer.Name.ToLower = "jeremy-pc"
        End Get
    End Property
#End Region

#Region "Config settings (write first time)"
    Public Shadows Function WriteConfigSetting_Password(Optional value As String = "somelongpassword", Optional overwrite As Boolean = False) As Boolean
        Return MyBase.WriteConfigSetting_Password(value, overwrite)
    End Function
    Public Shadows Function WriteConfigSetting_HostName(Optional value As String = DEFAULT_HOSTNAME, Optional overwrite As Boolean = False) As Boolean 'expose the default value via optional param
        Return MyBase.WriteConfigSetting_HostName(value, overwrite)
    End Function
    Public Shadows Function WriteConfigSetting_UseSsl(Optional value As Boolean = DEFAULT_USE_SSL, Optional overwrite As Boolean = False) As Boolean 'expose the default value via optional param
        Return MyBase.WriteConfigSetting_UseSsl(value, overwrite)
    End Function
    Public Function WriteConfigSetting_ExtensionsToIgnore(Optional value As String = DEFAULT_IGNORES, Optional overwrite As Boolean = True) As Boolean
        Return WriteConfigSetting(KEY_ExtensionsToIgnore, value, overwrite)
    End Function
    Public Function WriteConfigSetting_AppId(appId As Integer, Optional overwrite As Boolean = True) As Boolean
        Return WriteConfigSetting(KEY_AppId, appId, overwrite)
    End Function
    Public Function WriteConfigSetting_InstanceName(Optional instanceName As String = Nothing, Optional overwrite As Boolean = True) As Boolean
        Return WriteConfigSetting(KEY_InstanceName, instanceName, overwrite)
    End Function
    Public Function WriteConfigSetting_InstanceId(instanceId As Integer, Optional overwrite As Boolean = True) As Boolean
        Return WriteConfigSetting(KEY_InstanceId, instanceId, overwrite)
    End Function

    'New Config settings
    Public ReadOnly Property IsDevMachine As Boolean
        Get
            Return ConfigBool_(PREFIXED_IsDevMachine, KEY_IsDevMachine, DefaultIsDevMachine)
        End Get
    End Property
    Public ReadOnly Property InstanceName As String
        Get
            Return Config_(PREFIXED_InstanceName, KEY_InstanceName, DefaultInstanceName)
        End Get
    End Property
    Public ReadOnly Property InstanceId As Integer
        Get
            Return ConfigInt_(PREFIXED_InstanceId, KEY_InstanceId, Integer.MinValue)
        End Get
    End Property
    Public ReadOnly Property AppId As Integer
        Get
            Return ConfigInt_(PREFIXED_AppId, KEY_AppId, Integer.MinValue)
        End Get
    End Property
    Public ReadOnly ExtensionsToIgnore_AsArray As String() = CUtilities.StringToListStr(ExtensionsToIgnore).ToArray()
    Public ReadOnly Property ExtensionsToIgnore As String
        Get
            Return Config_(PREFIXED_ExtensionsToIgnore, KEY_ExtensionsToIgnore, DEFAULT_IGNORES)
        End Get
    End Property

    Public ReadOnly Property Identity As CIdentity
        Get
            Dim id As New CIdentity
            id.AppId = AppId
            id.InstanceId = InstanceId
            id.InstanceName = InstanceName
            Return id
        End Get
    End Property
#End Region

#Region "Singleton (Shared access to member variables)"
    Private Shared m_shared As New CUpgradeClient_Config
    Public Shared Shadows ReadOnly Property [Shared] As CUpgradeClient_Config
        Get
            Return m_shared
        End Get
    End Property
#End Region

End Class
