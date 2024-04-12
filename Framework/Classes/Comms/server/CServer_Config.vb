Public Class CServer_Config : Inherits CConfigComms : Implements IConfig_Server

    'Raw Keys
    Private Const KEY_RequireEncryption As String = "RequireEncryption"

    'Prefixed keys (for runtime efficiency)
    Protected PREFIXED_RequireEncryption As String = KEY_CommonPrefix & KEY_RequireEncryption
    Public ReadOnly Property RequireEncryptionConfigKey As String Implements IConfig_Server.RequireEncryptionConfigKey
        Get
            Return PREFIXED_RequireEncryption
        End Get
    End Property

    'Config Settings (and Defaults)
    Public ReadOnly Property RequireEncryption As Boolean Implements IConfig_Server.RequireEncryption 'Enforced in ServerWcf/ServerRest classes
        Get
            Return ConfigBool_(PREFIXED_RequireEncryption, KEY_RequireEncryption, False)
        End Get
    End Property

    'WriteConfigSetting
    Public Function WriteConfigSetting_RequireEncryption(value As Boolean, overwrite As Boolean) As Boolean
        Return WriteConfigSetting(KEY_RequireEncryption, value, overwrite)
    End Function
End Class
