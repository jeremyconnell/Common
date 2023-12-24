Imports System.Web
Imports Framework

Public Class CPushUpgradeServer_Config : Inherits CServer_Config

#Region "Constants"
    Public Const COMMON_PREFIX As String = "PushUpgrade."
    Public Const DEFAULT_PASSWORD As String = "somelongpassword"
#End Region

#Region "Prefix (optional)"
    Protected Overrides ReadOnly Property KEY_CommonPrefix As String
        Get
            Return COMMON_PREFIX
        End Get
    End Property
#End Region

#Region "Config settings (write first time)"
    'Overloads
    Public Shadows Function WriteConfigSetting_Password(Optional value As String = DEFAULT_PASSWORD, Optional overwrite As Boolean = False) As Boolean
        Return MyBase.WriteConfigSetting_Password(value, overwrite)
    End Function
    Public Shadows Function WriteConfigSetting_RequireEncryption(Optional value As Boolean = False, Optional overwrite As Boolean = False) As Boolean
        Return MyBase.WriteConfigSetting_RequireEncryption(value, overwrite)
    End Function
#End Region

#Region "Config Settings (read)"

#End Region

#Region "Singleton (Shared access to member variables)"
    Private Shared m_shared As New CPushUpgradeServer_Config
    Public Shared Shadows ReadOnly Property [Shared] As CPushUpgradeServer_Config
        Get
            Return m_shared
        End Get
    End Property
#End Region

End Class
