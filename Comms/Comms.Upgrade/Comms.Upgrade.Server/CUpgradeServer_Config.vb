Imports System.Web
Imports Comms.Upgrade.Client

Public Class CUpgradeServer_Config : Inherits CServer_Config

#Region "Constants"
    Public Const COMMON_PREFIX As String = "Upgrade."

    Public Const KEY_DisableAutoUpgrades As String = "DisableAutoUpgrades"
    Public Const KEY_ExceptExtensions As String = "ExceptExtensions"

    Private Shared PREFIXED_DisableAutoUpgrades As String = COMMON_PREFIX & KEY_DisableAutoUpgrades
    Private Shared PREFIXED_ExceptExtensions As String = COMMON_PREFIX & KEY_ExceptExtensions
#End Region

#Region "Prefix (optional)"
    Protected Overrides ReadOnly Property KEY_CommonPrefix As String
        Get
            Return COMMON_PREFIX
        End Get
    End Property
#End Region

#Region "Config settings (write first time)"
    Public Function WriteConfigSetting_DisableAutoUpgrades(Optional value As Boolean = True) As Boolean
        Return WriteConfigSetting(KEY_DisableAutoUpgrades, value, True)
    End Function

    'Make base ones more usable
    Public Shadows Function WriteConfigSetting_Password(Optional value As String = "somelongpassword", Optional overwrite As Boolean = False) As Boolean
        Return MyBase.WriteConfigSetting_Password(value, overwrite)
    End Function
    Public Shadows Function WriteConfigSetting_RequireEncryption(Optional value As Boolean = False, Optional overwrite As Boolean = False) As Boolean
        Return MyBase.WriteConfigSetting_RequireEncryption(value, overwrite)
    End Function
#End Region

#Region "Config Settings (read)"
    Public ReadOnly Property DisableAutoUpgrades As Boolean
        Get
            Return ConfigBool_(PREFIXED_DisableAutoUpgrades, KEY_DisableAutoUpgrades, False)
        End Get
    End Property
    Public ReadOnly Property ExceptExtensions As String
        Get
            Return Config_(PREFIXED_ExceptExtensions, KEY_ExceptExtensions, CFolderHash.IGNORE_WEB)
        End Get
    End Property
#End Region

#Region "Singleton (Shared access to member variables)"
    Private Shared m_shared As New CUpgradeServer_Config
    Public Shared Shadows ReadOnly Property [Shared] As CUpgradeServer_Config
        Get
            Return m_shared
        End Get
    End Property
#End Region

End Class
