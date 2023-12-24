Imports System.Web
Imports Comms.PushUpgrade.Interface
Imports SchemaAudit

Public MustInherit Class CUpgradeClient : Inherits CClient_RestOrWcf : Implements IUpgrade

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
    Public Shared Function Factory() As CUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CUpgradeClient_Rest
            Case ETransport.WCF : Return New CUpgradeClient_Wcf

            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
    Public Shared Function Factory(hostName As String, useSsl As Boolean) As CUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CUpgradeClient_Rest(hostName, useSsl)
            Case ETransport.WCF : Return New CUpgradeClient_Wcf(Config_.Endpoint, hostName, useSsl)

            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
    Public Shared Function Factory(hostName As String, useSsl As Boolean, restTimeoutMs As Integer) As CUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CUpgradeClient_Rest(hostName, useSsl, restTimeoutMs)
            Case ETransport.WCF : Return New CUpgradeClient_Wcf(Config_.Endpoint, hostName, useSsl) 'todo: timeout?

            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
    Public Shared Function Factory(hostName As String, useSsl As Boolean, wcfEndpoint As String) As CUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CUpgradeClient_Rest(hostName, useSsl)
            Case ETransport.WCF : Return New CUpgradeClient_Wcf(wcfEndpoint, hostName, useSsl)

            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
    Public Shared Function Factory(hostName As String, useSsl As Boolean, password As IConfig) As CUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CUpgradeClient_Rest(hostName, useSsl, password)
            Case ETransport.WCF : Return New CUpgradeClient_Wcf(Config_.Endpoint, hostName, useSsl, password)

            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
    Public Shared Function Factory(hostAndPassword As IConfig_Client) As CUpgradeClient
        Select Case Config_.Transport
            Case ETransport.Rest : Return New CUpgradeClient_Rest(hostAndPassword)
            Case ETransport.WCF : Return New CUpgradeClient_Wcf(hostAndPassword.Endpoint, hostAndPassword.HostName, hostAndPassword.UseSsl, hostAndPassword)

            Case ETransport.CmdLineExe : Throw New Exception(CMD_LINE_ERR)
            Case ETransport.InProcess : Throw New Exception(INPROC_ERROR)
        End Select
        Throw New Exception("Not implemented: " & Config_.Transport.ToString)
    End Function
#End Region

#Region "Shared - Config"
    Public Shared ReadOnly Property Config_ As CUpgradeClient_Config
        Get
            Return CUpgradeClient_Config.Shared
        End Get
    End Property


#End Region

#Region "Logic"
    'Dataconversions hack - For backward compat (and forward when Framework objects are serialisable)

    'Simple Upgrade Wrapper e.g. email_templates, upgrader_exe
    Public Function DoUpgrade_Website(Optional exceptExtensions As String() = Nothing, Optional recursive As Boolean = True) As CSummary
        Return DoUpgrade_OtherExe(Config_.AppId, Config_.InstanceName, HttpContext.Current.Server.MapPath("~/"), exceptExtensions, recursive) 'Web case defaults to true
    End Function
    Public Function DoUpgrade_OtherExe(appId As Integer, instanceName As String, folderPath As String, Optional exceptExtensions As String() = Nothing, Optional recursive As Boolean = False) As CSummary
        If Config_.IsDevMachine Then Return New CSummary(appId, instanceName)


        If Not IO.Directory.Exists(folderPath) Then IO.Directory.CreateDirectory(folderPath)

        'Lookup current version (assumes no other process will change the local folder)
        Dim local As CFolderHash = LocalVersion(folderPath, exceptExtensions, recursive)

        'Get hash of remote version
        Dim remoteHash As Guid = Me.GetCurrentVersion(appId, instanceName, exceptExtensions, recursive)
        If remoteHash = local.Hash Then Return New CSummary(appId, instanceName) 'No changes
        If remoteHash = Guid.Empty Then Return New CSummary(appId, instanceName) 'Switched off at server

        'Download changes
        Dim diff As List(Of CFileNameAndContent) = Me.GetFilesToUpgrade(appId, instanceName, local, exceptExtensions, recursive)
        'RestoreExeNumber(diff, exeName, processNumber)
        CFolderHash.ApplyChanges(diff, folderPath, 4000000, True)

        'Reset knowledge of local version
        CApplication.Set(folderPath, Nothing)

        Return New CSummary(diff, appId, instanceName)
    End Function
    Public Function DoUpgrade_Repeated(appId As Integer, serverName As String, folderPath As String, serverVersion As Guid, localVer As CFolderHash, Optional exceptExtensions As String() = Nothing, Optional recursive As Boolean = False) As List(Of String)
        Dim diff As List(Of CFileNameAndContent) = GetFilesToUpgrade(appId, serverName, localVer, exceptExtensions, recursive)
        CFolderHash.ApplyChanges(diff, folderPath, 4000000, True)
        Dim names As New List(Of String)(diff.Count)
        For Each i As CFileNameAndContent In diff
            names.Add(i.Name)
        Next
        Return names
    End Function



    'Self-Upgrade - more complex, need to copy to a temp folder, then call an upgrader exe
    Public Function LocalVersion(folderPath As String, Optional exceptExtensions As String() = Nothing, Optional recursive As Boolean = True) As CFolderHash
        'If String.IsNullOrEmpty(folderPath) Then folderPath = CUpgradeClient_Config.SELF_FOLDER
        If IsNothing(exceptExtensions) Then exceptExtensions = Config_.ExtensionsToIgnore_AsArray

        Dim local As CFolderHash = CApplication.Get(folderPath) 'For folders changed by other processes, use: CUpgradeServer.GetFromCache(folderPath)
        If IsNothing(local) Then
            local = New CFolderHash(folderPath, exceptExtensions, recursive)
            CApplication.Set(folderPath, local)
        End If
        Return local
    End Function
    Public Function DoUpgrade_ThisExe(appId As Integer, instanceName As String, Optional ByRef folderPath As String = Nothing, Optional ByRef tempFolder As String = Nothing, Optional exceptExtensions As String() = Nothing, Optional recursive As Boolean = False) As CSummary
        If Config_.IsDevMachine Then Return New CSummary(appId, instanceName)

        'Defaults
        If String.IsNullOrEmpty(folderPath) Then folderPath = My.Application.Info.DirectoryPath + "\\"
        If String.IsNullOrEmpty(tempFolder) Then tempFolder = folderPath & "upgrade_files\"

        'Lookup current version (assumes no other process will change the local folder)
        Dim local As CFolderHash = LocalVersion(folderPath, exceptExtensions, recursive)
        CAudit_Log.Log(ELogType.Upgrade, instanceName + ": Local version=" + local.Base64Trunc)

        'Get hash of remote version
        Dim central As CUpgradeClient = CUpgradeClient.Factory
        Dim remoteHash As Guid = central.GetCurrentVersion(appId, instanceName, exceptExtensions, recursive)
        If remoteHash = local.Hash OrElse Guid.Empty.Equals(remoteHash) Then Return New CSummary(appId, instanceName)
        CAudit_Log.Log(ELogType.Upgrade, instanceName + ": Remote version=" + local.Base64Trunc)

        'Download changes, write to temp folder
        Dim diff As List(Of CFileNameAndContent) = central.GetFilesToUpgrade(appId, instanceName, local, exceptExtensions, recursive)
        CAudit_Log.Log(ELogType.Upgrade, instanceName + ": got diff=" + diff.Count.ToString("n0"))
        For Each i As CFileNameAndContent In diff
            CAudit_Log.Log(ELogType.Upgrade, instanceName & ": " & i.Name & " (" & CUtilities.FileSize(i.Content) & ")")
        Next

        If Not IO.Directory.Exists(tempFolder) Then IO.Directory.CreateDirectory(tempFolder)
        local.ApplyDeletesOnly(diff, folderPath) 'Attempt to delete directly (if this fails, can write *.delete files for upgrader)
        local.ApplyAddOrUpdatesOnly(diff, tempFolder) 'Write to holding location only
        CAudit_Log.Log(ELogType.Upgrade, instanceName + ": applied diff")

        Return New CSummary(diff, appId, instanceName)
    End Function
#End Region


#Region "Overloads"
    Public Function GetCurrentVersion(appId As Integer, Optional instanceName As String = Nothing, Optional exceptExtensions As String() = Nothing) As Guid
        If IsNothing(exceptExtensions) Then exceptExtensions = Config_.ExtensionsToIgnore_AsArray
        If IsNothing(instanceName) Then instanceName = Config_.InstanceName
        Return GetCurrentVersion(appId, instanceName, exceptExtensions, True)
    End Function
    Public Function GetCurrentVersion_AsBase64(appId As Integer, Optional instanceName As String = Nothing, Optional exceptExtensions As String() = Nothing) As String
        Return CBinary.ToBase64(GetCurrentVersion(appId, instanceName, exceptExtensions))
    End Function

#End Region

#Region "Public Interface (combines all input parameters into a single object)"
    Public Sub ClearCache() Implements IUpgrade.ClearCache
        Invoke(EUpgrade.ClearCache)
    End Sub
    Public Function GetCurrentVersion(appId As Integer, instanceName As String, exceptExtensions As String(), recursive As Boolean) As Guid Implements IUpgrade.GetCurrentVersion
        Return CSerialise.Proto.AsGuid(Invoke(EUpgrade.GetCurrentVersion, appId, instanceName, exceptExtensions, recursive))
    End Function
    Public Function GetFilesToUpgrade(appId As Integer, instanceName As String, localVersion As CFolderHash, exceptExtensions As String(), recursive As Boolean) As CFilesList Implements IUpgrade.GetFilesToUpgrade
        Dim z As EGzip = EGzip.Both
        Dim f As ESerialisation = ESerialisation.BinarySer 'Use old serialisation (for framework objects)
        Return CBinary.DeserialiseFromBytes(Invoke(EUpgrade.GetFilesToUpgrade, z, f, f, appId, instanceName, localVersion, exceptExtensions, recursive))
    End Function
    Public Function GetCurrentVersions(folders As List(Of Integer), instanceName As String, exceptExtensions As String(), recursive As Boolean) As List(Of Guid) Implements IUpgrade.GetCurrentVersions
        Return CSerialise.Proto.AsListOfGuid(Invoke(EUpgrade.GetCurrentVersions, folders, instanceName, exceptExtensions, recursive))
    End Function

#End Region

#Region "Async sample"
    ' Same gzip settings, return type serialisation, but no interface implementation
    Public Async Function GetCurrentVersionsAsync(folders As List(Of Integer), instanceName As String, exceptExtensions As String(), recursive As Boolean) As Threading.Tasks.Task(Of List(Of Guid))
        Return CSerialise.Proto.AsListOfGuid(Await InvokeAsync(EUpgrade.GetCurrentVersions, folders, instanceName, exceptExtensions, recursive))
    End Function
#End Region

#Region "Shadows: Integer-enum => Custom Enum (EUpgrade)"
    Protected Shadows Function Invoke(enum_ As EUpgrade, ParamArray params As Object()) As Byte() 'most commonly-used overload
        Return MyBase.Invoke(enum_, params)
    End Function
    Protected Shadows Function Invoke(enum_ As EUpgrade, gzip As EGzip, ParamArray params As Object()) As Byte() 'occasional control of compression at the method-level
        Return MyBase.Invoke(enum_, gzip, params)
    End Function
    Protected Shadows Function Invoke(enum_ As EUpgrade, gzip As EGzip, formatIn As ESerialisation, ParamArray params As Object()) As Byte() 'rarely used (encryption control)
        Return MyBase.Invoke(enum_, gzip, formatIn, params)
    End Function
    Protected Shadows Function Invoke(enum_ As EUpgrade, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, ParamArray params As Object()) As Byte() 'rarely used (encryption control)
        Return MyBase.Invoke(enum_, gzip, formatIn, formatOut, params)
    End Function
    Protected Shadows Function Invoke(enum_ As EUpgrade, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, encryption As EEncryption, ParamArray params As Object()) As Byte()
        Return MyBase.Invoke(enum_, gzip, formatIn, formatOut, encryption, params) 'lowest-level (serialisation option is rarely used)
    End Function


#End Region

End Class

