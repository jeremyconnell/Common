'Server-side setttings: validate requested encryption
Public Interface IConfig_Server : Inherits IConfig
    ReadOnly Property RequireEncryption As Boolean 'Optional, defaults to false
    ReadOnly Property RequireEncryptionConfigKey As String
End Interface

'Client-side setttings: preconfigure choice of encryption
Public Interface IConfig_Client : Inherits IConfig
    ReadOnly Property Encryption As EEncryption 'Optional, defaults to none
    ReadOnly Property HostName As String 'Optional, can provide to constructor
    ReadOnly Property UseSsl As Boolean
End Interface

'Common settings: Password is stored in config file, ideally in the encrypted section (defaults to Framework.WebservicePassword), and used to provide encryption
Public Interface IConfig
    ReadOnly Property Password As String

    Function Encrypt(data As Byte(), algorithm As EEncryption) As Byte() 'Password involved (from config)
    Function Decrypt(data As Byte(), algorithm As EEncryption) As Byte() 'Password involved (from config)
End Interface
