'All methods are shared, but are represented as instance methods, in order to implement an interface. 
'They are accessed like shared methods via the singleton
Imports System.Web
Imports SchemaAudit


Public Class CUpgradeServer : Implements IUpgrade

#Region "Constants"
    Public Shared MAX_HASH_AGE As New TimeSpan(0, 5, 0) 'Check files every 5-mins
#End Region


#Region "Client Factory (e.g. for testing Inproc)"
    Public Shared Function Factory() As Comms.Upgrade.Interface.IUpgrade
        Select Case Comms.Upgrade.Client.CUpgradeClient_Config.Shared.Transport
            Case ETransport.InProcess : Return New CUpgradeServer()
            Case Else : Return Comms.Upgrade.Client.CUpgradeClient.Factory
        End Select
    End Function
    Public Shared Function Factory(hostName As String, useSsl As Boolean) As Comms.Upgrade.Interface.IUpgrade
        Select Case Comms.Upgrade.Client.CUpgradeClient_Config.Shared.Transport
            Case ETransport.InProcess : Return New CUpgradeServer()
            Case Else : Return Comms.Upgrade.Client.CUpgradeClient.Factory(hostName, useSsl)
        End Select
    End Function
#End Region

#Region "Shared - Singleton"
    Private Shared m_singleton As New CUpgradeServer
    Public Shared Function [Shared]() As CUpgradeServer
        Return m_singleton
    End Function
    'Constructor
    Private Sub New()
    End Sub
#End Region

#Region "Shared - Config"
    Public Shared ReadOnly Property Config_ As CUpgradeServer_Config
        Get
            Return CUpgradeServer_Config.Shared
        End Get
    End Property
#End Region

#Region "Resolve Enum"
    Public Function ExecuteMethod(methodNameEnum As Integer, params As CReader) As Object
        Return ExecuteMethod_(methodNameEnum, params)
    End Function
    Private Function ExecuteMethod_(methodName As EUpgrade, p As CReader) As Object
        Select Case methodName
            'Old Functions/Subs
            Case EUpgrade.GetCurrentVersion : Return GetCurrentVersion(p)
            Case EUpgrade.GetCurrentVersions : Return GetCurrentVersions(p)
            Case EUpgrade.GetFilesToUpgrade : Return GetFilesToUpgrade(p)
            Case EUpgrade.ClearCache
                ClearCache()
                Return Nothing

            Case Else : Throw New Exception("Unrecognised EUpgrade method: " & methodName)
        End Select
    End Function
#End Region

#Region "Parameter Casting"
    Public Function GetCurrentVersion(p As CReader) As Guid
        Dim appId As Integer = p.Unpack(Of Integer)()
        Dim clientName As String = p.Str
        Dim exceptExtensions As String() = p.StrArray
        Dim recursive As Boolean = p.Bool
        Return GetCurrentVersion(appId, clientName, exceptExtensions, recursive)
    End Function
    Public Function GetCurrentVersions(p As CReader) As List(Of Guid)
        Dim folders As List(Of Integer) = p.Unpack(Of List(Of Integer))()
        Dim clientName As String = p.Str
        Dim exceptExtensions As String() = p.StrArray
        Dim recursive As Boolean = p.Bool
        Return GetCurrentVersions(folders, clientName, exceptExtensions, recursive)
    End Function
    Public Function GetFilesToUpgrade(p As CReader) As List(Of CFileNameAndContent)
        Dim appId As Integer = p.Unpack(Of Integer)()
        Dim clientName As String = p.Str
        Dim localVersion As CFolderHash = p.Unpack(Of CFolderHash)()
        Dim exceptExtensions As String() = p.StrArray
        Dim recursive As Boolean = p.Bool

        Return GetFilesToUpgrade(appId, clientName, localVersion, exceptExtensions, recursive)
    End Function

#End Region

#Region "Native Interface (Implementation)"
    Public Sub ClearCache() Implements IUpgrade.ClearCache
        CApplication.ClearAll()
    End Sub
    Public Function GetCurrentVersion(appId As Integer, instanceName As String, exceptExtensions As String(), recursive As Boolean) As Guid Implements IUpgrade.GetCurrentVersion
        If Config_.DisableAutoUpgrades Then Return Guid.Empty
        Dim g As Guid = GetFolderHash(appId, instanceName, exceptExtensions, recursive).Hash

        'Record it
        Try
            Dim instanceId As Integer = -1
            Try
                instanceId = Integer.Parse(instanceName)
            Catch
                instanceName = instanceName.Substring(instanceName.IndexOf("(") + 1)
                instanceId = Integer.Parse(instanceName)
                instanceName = instanceName.Substring(0, instanceName.IndexOf(")"))

            End Try

            'Dim r As CReport = CReport.Cache.GetById(instanceId)
            'If IsNothing(r) Then
            '    r = New CReport
            '    r.InstanceId = instanceId
            'End If
            'r.Version = g
            'r.LastReport = DateTime.Now
            'r.Save()
        Catch ex As Exception
            CAudit_Error.Log(ex)
        End Try

        Return g
    End Function
    Public Function GetCurrentVersions(folders As List(Of Integer), instanceName As String, exceptExtensions() As String, recursive As Boolean) As List(Of Guid) Implements IUpgrade.GetCurrentVersions
        Dim list As New List(Of Guid)(folders.Count)
        For Each i As Integer In folders
            list.Add(GetCurrentVersion(i, instanceName, exceptExtensions, recursive))
        Next
        Return list
    End Function
    Public Function GetFilesToUpgrade(appId As Integer, instanceName As String, localVersion As CFolderHash, exceptExtensions() As String, recursive As Boolean) As CFilesList Implements IUpgrade.GetFilesToUpgrade
        'Lookup from Cache, do a diff
        Dim path As String = Nothing
        Dim hash As CFolderHash = GetFolderHash(appId, instanceName, exceptExtensions, recursive, path)
        If hash.Hash = Guid.Empty Then Throw New Exception("Upgrade Folder not found: " & path)

        Return hash.ResolveDifferences(localVersion, path)
    End Function



#End Region

#Region "Caching Logic (Implementation) - Private"
    Private Shared Function GetFolderHash(appId As Integer, instanceName As String, Optional exceptExtensions As String() = Nothing, Optional recursive As Boolean = False, Optional ByRef pathOut As String = Nothing) As CFolderHash
        Dim folderPath As String = HttpContext.Current.Server.MapPath("~/App_Data/worker/")
        Dim key = String.Concat("Hash_", folderPath, "/", recursive, "/", CUtilities.ListToString(exceptExtensions))
        Dim obj As Object = CApplication.Get(key)
        If IsNothing(obj) Then
            SyncLock GetType(CApplication)
                obj = CApplication.Get(key)
                If IsNothing(obj) Then
                    obj = New CFolderHash(folderPath, exceptExtensions, recursive)
                    CApplication.Set(key, obj)
                End If
            End SyncLock
        End If
        Dim fh As CFolderHash = obj
        If fh.Age.TotalMinutes > 5 Then CApplication.Set(key, Nothing)
        pathOut = folderPath
        Return obj
    End Function

#End Region




End Class