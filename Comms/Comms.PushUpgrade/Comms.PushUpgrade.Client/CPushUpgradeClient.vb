Imports System.Web
Imports Comms.PushUpgrade.Interface

Public MustInherit Class CPushUpgradeClient : Inherits CClient_RestOrWcf : Implements IPushUpgrade


#Region "Constructors"
    Protected Sub New(hostName As String)
        Me.New(hostName, Config_.UseSsl, Config_)
    End Sub
    Protected Sub New(hostName As String, useSsl As Boolean)
        Me.New(hostName, useSsl, Config_)
    End Sub
    Protected Sub New(hostName As String, useSsl As Boolean, password As IConfig_Client)
        MyBase.New(hostName, useSsl, password, EGzip.None, ESerialisation.Protobuf) 'Protobuf serialisation, and no compression (unless otherwise specified)
    End Sub
#End Region

#Region "Shared - Factory (Config-based Rest vs WCF)"
    Private Const INPROC_ERROR As String = "For InProc option, add reference to Server-side, call Factory method on the Server class (add if necessary, using interface as return type)"
    Private Const CMD_LINE_ERR As String = "No implementation for CmdLine exe"
    Private Const WCF_ERR As String = "No wcf implementation"
    Public Shared Function Factory() As CPushUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CPushUpgradeClient_Http

            Case ETransport.WCF : Throw New Exception(WCF_ERR)
            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
    Public Shared Function Factory(hostName As String, useSsl As Boolean) As CPushUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CPushUpgradeClient_Http(hostName, useSsl)

            Case ETransport.WCF : Throw New Exception(WCF_ERR)
            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
    Public Shared Function Factory(hostName As String, useSsl As Boolean, restTimeoutMs As Integer) As CPushUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CPushUpgradeClient_Http(hostName, useSsl, restTimeoutMs)

            Case ETransport.WCF : Throw New Exception(WCF_ERR)
            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
    Public Shared Function Factory(hostName As String, useSsl As Boolean, wcfEndpoint As String) As CPushUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CPushUpgradeClient_Http(hostName, useSsl)

            Case ETransport.WCF : Throw New Exception(WCF_ERR)
            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
    Public Shared Function Factory(hostName As String, useSsl As Boolean, password As IConfig) As CPushUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CPushUpgradeClient_Http(hostName, useSsl, password)

            Case ETransport.WCF : Throw New Exception(WCF_ERR)
            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
    Public Shared Function Factory(hostAndPassword As IConfig_Client) As CPushUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CPushUpgradeClient_Http(hostAndPassword)

            Case ETransport.WCF : Throw New Exception(WCF_ERR)
            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
#End Region

#Region "Shared - Config"
    Public Shared ReadOnly Property Config_ As CPushUpgradeClient_Config
        Get
            Return CPushUpgradeClient_Config.Shared
        End Get
    End Property
#End Region


#Region "Overloads"
    Public Function PollVersion(allData As Boolean) As CMyVersion
        Return PollVersion(Config_.Filter, allData)
    End Function
    Public Function RequestHash() As CFolderHash
        Return RequestHash(Config_.Filter)
    End Function
    Public Function RequestFiles() As CFilesList
        Return RequestFiles(Config_.Filter)
    End Function

    'Trivial
    Public Function PollVersion_Base64() As String
        Return PollVersion(False).MD5_Base64
    End Function
    Public Function PollVersion_Base64Trunc() As String
        Return PollVersion(False).MD5_Base64Trunc
    End Function
#End Region

#Region "Public Interface (Serialisation etc)"
    Public Function TriggerUpgrade() As CException Implements IPushUpgrade.TriggerUpgrade
        Return Deserialiser.AsEx(Invoke(EPushUpgrade.TriggerUpgr))
    End Function
    Public Function RunScripts(sql As List(Of String)) As List(Of String) Implements IPushUpgrade.RunScripts
        Return Deserialiser.AsListStr(Invoke(EPushUpgrade.RunScripts, EGzip.Both, sql))
    End Function
    Public Function PushFiles(changes As CFilesList) As CException Implements IPushUpgrade.PushFiles
        Return Deserialiser.AsEx(Invoke(EPushUpgrade.PushFiles, EGzip.Input, changes))
    End Function
    Public Function ListDirectory(subDir As String) As List(Of String) Implements IPushUpgrade.ListDirectory
        Throw New NotImplementedException()
    End Function

    Public Function RequestFile(name As String) As Byte() Implements IPushUpgrade.RequestFile
        Throw New NotImplementedException()
    End Function

    Public Function DeleteFile(name As String) As Boolean Implements IPushUpgrade.DeleteFile
        Throw New NotImplementedException()
    End Function




    Public Function PollVersion(filter As CFilter, allData As Boolean) As CMyVersion Implements IPushUpgrade.PollVersion
        Return AsMyVersion(Invoke(EPushUpgrade.PollVersion, EGzip.Output, filter, allData))
    End Function

    Public Function RequestHash(filter As CFilter) As CFolderHash Implements IPushUpgrade.RequestHash
        Return AsFolderHash(Invoke(EPushUpgrade.RequestHash, EGzip.Output, filter))
    End Function

    Public Function RequestFiles(filter As CFilter) As CFilesList Implements IPushUpgrade.RequestFull
        Return AsFiles(Invoke(EPushUpgrade.RequestFull, EGzip.Output, filter))
    End Function
    Public Function SetInstance(info As CInstanceInfo) As CMyVersion Implements IPushUpgrade.SetInstance
        Return AsMyVersion(Invoke(EPushUpgrade.SetInstance, info))
    End Function

    Public Function RequestLogFile(name As String) As String Implements IPushUpgrade.RequestLogFile
        Return Deserialiser.AsStr(Invoke(EPushUpgrade.RequestLogFile, EGzip.Output, name))
    End Function

    Public Function RequestWebConfig() As String Implements IPushUpgrade.RequestWebConfig
        Return Deserialiser.AsStr(Invoke(EPushUpgrade.RequestWebConfig, EGzip.Output))
    End Function


    Public Function WriteAppSettings(pairs As Dictionary(Of String, String)) As String Implements IPushUpgrade.WriteAppSettings
        Return Deserialiser.AsStr(Invoke(EPushUpgrade.WriteAppSettings, EGzip.Input, pairs))
    End Function
    Public Function RemoveAppSettings(pairs As List(Of String)) As String Implements IPushUpgrade.RemoveAppSettings
        Return Deserialiser.AsStr(Invoke(EPushUpgrade.WriteAppSettings, EGzip.Input, pairs))
    End Function

    Public Function WriteConnString(name As String, value As String) As String Implements IPushUpgrade.WriteConnString
        Return Deserialiser.AsInt(Invoke(EPushUpgrade.WriteConnString, EGzip.Input, name, value))
    End Function

    Public Function RemoveConnString(name As String) As String Implements IPushUpgrade.RemoveConnString
        Return Deserialiser.AsInt(Invoke(EPushUpgrade.RemoveConnString, EGzip.Input, name))
    End Function
#End Region

#Region "Deserialisation"
    Private Function AsMyVersion(bin As Byte()) As CMyVersion
        Return Me.Deserialiser.Deserialise_(Of CMyVersion)(bin)
    End Function
    Private Function AsFiles(bin As Byte()) As CFilesList
        Return Me.Deserialiser.Deserialise_(Of CFilesList)(bin)
    End Function
    Private Function AsFolderHash(bin As Byte()) As CFolderHash
        Return Me.Deserialiser.Deserialise_(Of CFolderHash)(bin)
    End Function

#End Region

#Region "Shadows: Integer-enum => Custom Enum (EPushUpgrade)"
    Protected Shadows Function Invoke(enum_ As EPushUpgrade, ParamArray params As Object()) As Byte() 'most commonly-used overload
        Return MyBase.Invoke(enum_, params)
    End Function
    Protected Shadows Function Invoke(enum_ As EPushUpgrade, gzip As EGzip, ParamArray params As Object()) As Byte() 'occasional control of compression at the method-level
        Return MyBase.Invoke(enum_, gzip, params)
    End Function
    Protected Shadows Function Invoke(enum_ As EPushUpgrade, gzip As EGzip, formatIn As ESerialisation, ParamArray params As Object()) As Byte() 'rarely used (encryption control)
        Return MyBase.Invoke(enum_, gzip, formatIn, params)
    End Function
    Protected Shadows Function Invoke(enum_ As EPushUpgrade, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, ParamArray params As Object()) As Byte() 'rarely used (encryption control)
        Return MyBase.Invoke(enum_, gzip, formatIn, formatOut, params)
    End Function
    Protected Shadows Function Invoke(enum_ As EPushUpgrade, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, encryption As EEncryption, ParamArray params As Object()) As Byte()
        Return MyBase.Invoke(enum_, gzip, formatIn, formatOut, encryption, params) 'lowest-level (serialisation option is rarely used)
    End Function

#End Region

End Class

