Option Strict Off

Public Class CClient_Config : Inherits CConfigComms : Implements IConfig_Client

    'Raw Keys
    Private Const KEY_Encryption As String = "Encryption"
    Private Const KEY_HostName As String = "HostName"
    Private Const KEY_UseSsl As String = "UseSsl"


    'Prefixed keys (for runtime efficiency)
    Protected PREFIXED_Encryption As String = KEY_CommonPrefix & KEY_Encryption
    Protected PREFIXED_HostName As String = KEY_CommonPrefix & KEY_HostName
    Protected PREFIXED_UseSsl As String = KEY_CommonPrefix & KEY_UseSsl

    'Optional Defaults
    Protected Overridable ReadOnly Property DefaultHostName As String
        Get
            Return String.Empty
        End Get
    End Property
    Protected Overridable ReadOnly Property DefaultUseSsl As String
        Get
            Return False
        End Get
    End Property

    'WriteConfigSetting
    Public Function WriteConfigSetting_HostName(value As String, overwrite As Boolean) As Boolean
        Return WriteConfigSetting(KEY_HostName, value, overwrite)
    End Function
    Public Function WriteConfigSetting_Encryption(value As EEncryption, overwrite As Boolean) As Boolean
        Return WriteConfigSetting(KEY_Encryption, value.ToString, overwrite) 'using string-based, not integer-based
    End Function
    Public Function WriteConfigSetting_UseSsl(value As Boolean, overwrite As Boolean) As Boolean
        Return WriteConfigSetting(KEY_UseSsl, value, overwrite)
    End Function

    'Overloads (default overwrite settings)
    Public Function WriteConfigSetting_HostName(value As String) As Boolean
        Return WriteConfigSetting_HostName(value, False)
    End Function
    Public Function WriteConfigSetting_Encryption(value As EEncryption) As Boolean
        Return WriteConfigSetting_Encryption(value, True)
    End Function
    Public Function WriteConfigSetting_UseSsl(value As Boolean) As Boolean
        Return WriteConfigSetting_UseSsl(value, True)
    End Function
    'Config Settings
    Public Overridable ReadOnly Property HostName As String Implements IConfig_Client.HostName 'Connection String (equivalent)
        Get
            Return Config_(PREFIXED_HostName, KEY_HostName, DefaultHostName)
        End Get
    End Property
    Public Overridable ReadOnly Property UseSsl As Boolean Implements IConfig_Client.UseSsl
        Get
            Return ConfigBool_(PREFIXED_UseSsl, KEY_UseSsl, DefaultUseSsl)
        End Get
    End Property
    Public ReadOnly Property Encryption As EEncryption Implements IConfig_Client.Encryption
        Get
            Dim s As String = Config_(PREFIXED_Encryption, KEY_Encryption, EEncryption.None).ToLower
            Select Case s
                Case String.Empty, "none" : Return EEncryption.None
                Case "rij" : Return EEncryption.Rij
                Case "xor", "xor_" : Return EEncryption.Xor
                Case Else
                    Dim i As Integer
                    If Integer.TryParse(s, i) Then Return i 'integer represenation?
                    Throw New Exception("Failed to parse encryption type: " & s)
            End Select
        End Get
    End Property

    'Encryption Helpers: Use the configured choice of encryption to select/use the configured password
    Protected ReadOnly Property EncryptionPasswordBytes As Byte()
        Get
            Return MyBase.EncryptionPassword(Me.Encryption)
        End Get
    End Property
    Public Overloads Function Encrypt(data As Byte()) As Byte()
        Return Encrypt(data, Encryption, EncryptionPasswordBytes)
    End Function
    Public Overloads Function Decrypt(data As Byte()) As Byte()
        Return Decrypt(data, Encryption, EncryptionPasswordBytes)
    End Function
End Class
