'Imports System.Web
Public Interface IConfig_PushUpgradeClient : Inherits IConfig_Client
    ReadOnly Property ExtensionsToIgnore As String
End Interface

Public Class CPushUpgradeClient_Config : Inherits CClient_Config : Implements IConfig_PushUpgradeClient

#Region "Constants"
    'Project Prefix
    Public Const COMMON_PREFIX As String = "PushUpgrade."

    'Defaults
    Public Const DEFAULT_USE_SSL As Boolean = False
    Public Const DEFAULT_HOSTNAME As String = "crypton.azurewebsites.net"
    Public Const DEFAULT_IGNORES As String = CFolderHash.IGNORE_PDB

    'Keys
    Private Const KEY_ExtensionsToIgnore As String = "ExtensionsToIgnore"

    'Full Key
    Private Shared PREFIXED_ExtensionsToIgnore As String = COMMON_PREFIX & KEY_ExtensionsToIgnore


    Public Shared ReadOnly SELF_FOLDER As String = GetSelfFolder()
    Private Shared Function GetSelfFolder() As String
        Return IO.Directory.GetCurrentDirectory()

        'Dim c As HttpContext = HttpContext.Current
        'If Not IsNothing(c) Then
        '    If Not IsNothing(c.Server) Then Return c.Server.MapPath("~/")
        'End If
        'Return My.Application.Info.DirectoryPath & "\"
    End Function

    Public Shared ReadOnly Property DefaultProdUrl As String
        Get
            If DEFAULT_USE_SSL Then
                Return "https://" + DEFAULT_HOSTNAME
            Else
                Return "http://" + DEFAULT_HOSTNAME
            End If
        End Get
    End Property
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
            Return DEFAULT_USE_SSL.ToString()
        End Get
    End Property
#End Region

#Region "Config settings (write first time)"
    Public Function WriteConfigSetting_ExtensionsToIgnore(Optional value As String = DEFAULT_IGNORES, Optional overwrite As Boolean = False) As Boolean
        Return WriteConfigSetting(KEY_ExtensionsToIgnore, value, overwrite)
    End Function
    Public Shadows Function WriteConfigSetting_HostName(Optional value As String = DEFAULT_HOSTNAME, Optional overwrite As Boolean = False) As Boolean 'expose the default value via optional param
        Return MyBase.WriteConfigSetting_HostName(value, overwrite)
    End Function
    Public Shadows Function WriteConfigSetting_UseSsl(Optional value As Boolean = DEFAULT_USE_SSL, Optional overwrite As Boolean = False) As Boolean 'expose the default value via optional param
        Return MyBase.WriteConfigSetting_UseSsl(value, overwrite)
    End Function
#End Region


    'New Config settings
    Public ReadOnly ExtensionsToIgnore_AsArray As String() = CUtilities.StringToListStr(ExtensionsToIgnore).ToArray()
    Public ReadOnly Property ExtensionsToIgnore As String Implements IConfig_PushUpgradeClient.ExtensionsToIgnore
        Get
            Return Config_(PREFIXED_ExtensionsToIgnore, KEY_ExtensionsToIgnore, DEFAULT_IGNORES)
        End Get
    End Property

#Region "Singleton (Shared access to member variables)"
    Private Shared m_shared As New CPushUpgradeClient_Config
    Public Shared Shadows ReadOnly Property [Shared] As CPushUpgradeClient_Config
        Get
            Return m_shared
        End Get
    End Property
#End Region

End Class
