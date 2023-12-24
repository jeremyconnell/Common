Option Strict Off

Imports System.Runtime.Serialization
Imports System.Security.Cryptography

#Region "Enums"
<DataContract>
Public Enum EEncryption
    None = 0 'Default (will change to xor)
    [Xor] = 1 'Faster
    Rij = 2 'More secure
End Enum
#End Region

'Essentially just a password
Partial Public Class CConfigComms : Inherits Framework.CConfigBase : Implements IConfig

#Region "Config Settings - Overridable"
    'Config-Setting Keys (master prefix)
    Protected Overridable ReadOnly Property KEY_CommonPrefix As String 'Each Comms project that extends this class can distinguish itself with a different prefix (unprefixed settings will apply to all)
        Get
            Return String.Empty
        End Get
    End Property

    'Raw keys (Can be varied by each derived class via a unique prefix)
    Protected Const KEY_Password As String = "Password"

    'Prefixed keys (for runtime efficiency)
    Protected PREFIXED_Password As String = KEY_CommonPrefix & KEY_Password  'Server and Client (fails over to standard webservice password in encrypted section)
#End Region

#Region "Config Settings (and Defaults)"
    Protected ReadOnly Property Password As String Implements IConfig.Password
        Get
            Return Config_(PREFIXED_Password, KEY_Password, WebServicePassword) 'Defaults to old framework setting
        End Get
    End Property
    Public Function WriteConfigSetting_Password(value As String, overwrite As Boolean) As Boolean
        Return WriteConfigSetting(KEY_Password, value, overwrite)
    End Function
    Public Function WriteConfigSetting_Password(value As String) As Boolean
        Return WriteConfigSetting_Password(value, True)
    End Function
#End Region

#Region "Password formatting"
    'Encryption Password (As binary)
    Private m_password_Rij As Byte() = Nothing
    Private m_password_Xor As Byte() = Nothing

    'Separate passwords for Rij (max 2xguid supported) and XOR (max 4xguid is available *unless concatenate repeated hashing)
    Private ReadOnly Property EncryptionPasswordRij() As Byte() 'Made stronger for XOR (via Sha512), and more random for Rij (via MD5)
        Get
            If IsNothing(m_password_Rij) Then
                Dim b As Byte() = CBinary.StringToBytes(Password)
                m_password_Rij = CBinary.Sha256(b) 'longest supported key length for rij
            End If
            Return m_password_Rij
        End Get
    End Property
    Private ReadOnly Property EncryptionPasswordXor() As Byte() 'Made stronger for XOR (via Sha512), and more random for Rij (via MD5)
        Get
            If IsNothing(m_password_Xor) Then
                Dim b As Byte() = CBinary.StringToBytes(Password)
                'm_password_Xor = CBinary.Sha512(b) 'longest readily-available hash

                Dim fortyGuids As New List(Of Byte)(10 * 256)
                For i As Integer = 1 To 10
                    b = CBinary.Sha256(b)
                    fortyGuids.AddRange(b)
                Next
                m_password_Xor = fortyGuids.ToArray() '10x256bits (2.56kB of psudeo-random)
            End If
            Return m_password_Xor
        End Get
    End Property
    Protected Function EncryptionPassword(e As EEncryption) As Byte()
        If e = EEncryption.Rij Then
            Return EncryptionPasswordRij
        Else
            Return EncryptionPasswordXor
        End If
    End Function
#End Region

#Region "Encryption Helpers (interface"
    Public Function Encrypt(data() As Byte, algorithm As EEncryption) As Byte() Implements IConfig.Encrypt
        Return Encrypt(data, algorithm, EncryptionPassword(algorithm))
    End Function
    Public Function Decrypt(data() As Byte, algorithm As EEncryption) As Byte() Implements IConfig.Decrypt
        Try
            Return Decrypt(data, algorithm, EncryptionPassword(algorithm))
        Catch
            Throw New Exception("Decryption failed (" & algorithm.ToString & ") - Check Client/Server passwords")
        End Try
    End Function
#End Region

#Region "Protected - WriteConfigSetting Helpers"
    Protected Function WriteConfigSetting(unprefixedKey As String, value As Boolean, overwrite As Boolean) As Boolean
        Return WriteConfigSetting(unprefixedKey, value.ToString, overwrite)
    End Function
    Protected Function WriteConfigSetting(unprefixedKey As String, value As Integer, overwrite As Boolean) As Boolean
        Return WriteConfigSetting(unprefixedKey, value.ToString, overwrite)
    End Function
    Protected Function WriteConfigSetting(unprefixedKey As String, value As String, overwrite As Boolean) As Boolean
        Try
            CConfigWriter.WriteAppSetting(KEY_CommonPrefix & unprefixedKey, value, overwrite)
        Catch
        End Try
        Return True
    End Function
#End Region

#Region "Shared - Encrypt/Decrypt Helpers"
    Private Shared EMPTY As Byte() = New Byte() {}
    Protected Shared Function Encrypt(data As Byte(), encryptionType As EEncryption, password As Byte()) As Byte()
        If IsNothing(data) OrElse data.Length = 0 Then Return EMPTY
        Select Case encryptionType
            Case EEncryption.None : Return data
            Case EEncryption.Xor : Return CBinary.EncryptFast(data, password)
            Case EEncryption.Rij : Return EncryptRijndael(data, password)
            Case Else : Throw New Exception("EEncryption value not supported, use {0,1,2}: " & encryptionType)
        End Select
    End Function
    Protected Shared Function Decrypt(data As Byte(), encryptionType As EEncryption, password As Byte()) As Byte()
        If IsNothing(data) OrElse data.Length = 0 Then Return EMPTY
        Select Case encryptionType
            Case EEncryption.None : Return data
            Case EEncryption.Xor : Return CBinary.EncryptFast(data, password) 'encrypt=decrypt
            Case EEncryption.Rij : Return DecryptRijndael(data, password) 'encrypt<>decrypt
            Case Else : Throw New Exception("EEncryption value not supported, use {0,1,2}: " & encryptionType)
        End Select
    End Function

    'Rijndael Helpers
    Private Shared Function EncryptRijndael(data As Byte(), password As Byte()) As Byte()
        Return CBinary.Encrypt(data, Rijndael(password))
    End Function
    Private Shared Function DecryptRijndael(data As Byte(), password As Byte()) As Byte()
        Return CBinary.Decrypt(data, Rijndael(password))
    End Function

    'Rijndael Providers (supports reuse with multiple passwords)
    Private Shared _rijndaels As Dictionary(Of Byte(), RijndaelManaged)
    Private Shared ReadOnly Property Rijndael(password As Byte()) As RijndaelManaged
        Get
            Dim rij As RijndaelManaged = Nothing

            If Not Rijndaels.TryGetValue(password, rij) Then
                rij = New RijndaelManaged()
                rij.Key = CBinary.LongerKey(password, rij.LegalKeySizes(0)) '64-256 bits = 8-32 ascii characters
                rij.IV = CBinary.LongerKey(password, CInt(rij.BlockSize / 8), CInt(rij.BlockSize / 8))

                Rijndaels(password) = rij
            End If

            Return rij
        End Get
    End Property
    Private Shared ReadOnly Property Rijndaels() As Dictionary(Of Byte(), RijndaelManaged)
        Get
            If IsNothing(_rijndaels) Then
                _rijndaels = New Dictionary(Of Byte(), RijndaelManaged)(1) 'Normally would just have one
            End If
            Return _rijndaels
        End Get
    End Property
#End Region

#Region "Config Helper => fail over to unprefixed settings"
    Protected Function Config_(key1 As String, key2 As String, defaultVal As String) As String
        If key1 = key2 Then Return Config(key1, defaultVal)
        Return Config(key1, Config(key2, defaultVal))
    End Function
    Protected Function ConfigInt_(key1 As String, key2 As String, defaultVal As Integer) As Integer
        If key1 = key2 Then Return ConfigInt(key1, defaultVal)
        Return ConfigInt(key1, ConfigInt(key2, defaultVal))
    End Function
    Protected Function ConfigBool_(key1 As String, key2 As String, defaultVal As Boolean) As Boolean
        If key1 = key2 Then Return ConfigBool(key1, defaultVal)
        Return ConfigBool(key1, ConfigBool(key2, defaultVal))
    End Function
#End Region

#Region "Config Helper => fail over to unprefixed settings"
    Protected Function Config_(key1 As String, defaultVal As String) As String
        Dim key2 As String = KEY_CommonPrefix & key1
        Return Config_(key2, key1, defaultVal)
    End Function
    Friend Function ConfigInt_(key1 As String, defaultVal As Integer) As Integer
        Dim key2 As String = KEY_CommonPrefix & key1
        Return ConfigInt_(key2, key1, defaultVal)
    End Function
    Friend Function ConfigBool_(key1 As String, defaultVal As Boolean) As Boolean
        Dim key2 As String = KEY_CommonPrefix & key1
        Return ConfigBool_(key2, key1, defaultVal)
    End Function
#End Region

#Region "Config Helper => fail over to unprefixed settings (or Exception)"
    Protected Function ConfigOrEx_(key1 As String) As String
        Dim key2 As String = KEY_CommonPrefix & key1
        Dim s As String = Config_(key2, key1, String.Empty)
        If s.Length = 0 Then Throw New Exception(String.Concat("Config setting is required: ", key2, " (or ", key1, ")"))
        Return s
    End Function
    Protected Function ConfigIntOrEx_(key1 As String) As Integer
        Dim key2 As String = KEY_CommonPrefix & key1
        Dim i As Integer = ConfigInt_(key2, key1, Integer.MinValue)
        If i = Integer.MinValue Then Throw New Exception(String.Concat("Config setting is required: ", key2, " (or ", key1, ") *Integer"))
        Return i
    End Function
#End Region

End Class
