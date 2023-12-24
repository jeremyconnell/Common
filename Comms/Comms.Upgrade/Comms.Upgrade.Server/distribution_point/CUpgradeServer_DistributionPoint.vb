'Provides a distribution point for upgrade requests from local apps, to reduce the load on central
Public Class CUpgradeServer_DistributionPoint : Implements IUpgrade

#Region "Remote Source (already configured due to self-upgrade)"
    Public Shared ReadOnly Property Central As Comms.Upgrade.Client.CUpgradeClient
        Get
            Return Comms.Upgrade.Client.CUpgradeClient.Factory
        End Get
    End Property
#End Region

#Region "Shared - Singleton"
    Private Shared m_singleton As New CUpgradeServer_DistributionPoint
    Public Shared Function [Shared]() As CUpgradeServer_DistributionPoint
        Return m_singleton
    End Function
    'Constructor
    Public Sub New()
    End Sub
#End Region

#Region "Resolve Enum"
    Public Function ExecuteMethod(methodNameEnum As Integer, params As CReader) As Object
        Return ExecuteMethod_(methodNameEnum, params)
    End Function
    Private Function ExecuteMethod_(methodName As EUpgrade, p As CReader) As Object
        Select Case methodName
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

#Region "Logic Override (Interface Methods) - Instead of checking local files, ask central (and cache the results)"
    Public Function GetCurrentVersion(appId As Integer, client As String, exceptExtensions As String(), recursive As Boolean) As Guid Implements IUpgrade.GetCurrentVersion
        Return Cached_LatestVersion(appId, client, exceptExtensions, recursive)
    End Function
    Public Function GetCurrentVersions(folders As List(Of Integer), client As String, exceptExtensions As String(), recursive As Boolean) As List(Of Guid) Implements IUpgrade.GetCurrentVersions
        Return Cached_LatestVersions(folders, client, exceptExtensions, recursive)
    End Function
    Public Function GetFilesToUpgrade(appId As Integer, client As String, local As CFolderHash, exceptExtensions As String(), recursive As Boolean) As CFilesList Implements IUpgrade.GetFilesToUpgrade
        Dim remote As Guid = Cached_LatestVersion(appId, client, exceptExtensions, recursive)
        Dim files As List(Of CFileNameAndContent) = Cached_Upgrade_New(local.Hash, remote) 'new format
        If IsNothing(files) Then
            SyncLock GetType(CUpgradeServer_DistributionPoint)
                files = Cached_Upgrade_New(local.Hash, remote)
                If IsNothing(files) Then
                    files = Central.GetFilesToUpgrade(appId, client, local, exceptExtensions, recursive)

                    Cached_Upgrade_New(local.Hash, remote) = files 'Store (keyed on both guids)
                    Cached_LatestVersionReset(appId, client, exceptExtensions, recursive) 'refresh the current-version info again, just in case its stale (causing looping for up to 30secs)
                End If
            End SyncLock
        End If
        Return files
    End Function
    Public Sub ClearCache() Implements IUpgrade.ClearCache
        CApplication.ClearAll()
    End Sub

#End Region

#Region "Cached Data - LatestVersionHash=5mins, From/ToHash=1-hour (or recycle)"
    'Cache logic
    Private Const MAX_SECS_VERSION As Integer = 5 * 60 '5mins, same as central
    Private Const MAX_MINS_UPGRADE As Integer = 60 'effectively until recycled

    Private Shared Function Cached_LatestVersionReset(appId As Integer, client As String, exceptExtensions As String(), recursive As Boolean) As Guid
        Dim key As String = GetKey(appId, client, exceptExtensions, recursive)
        CApplication.Set(key, Nothing)
    End Function
    Private Shared Function Cached_LatestVersionsReset(folders As List(Of Integer), client As String, exceptExtensions As String(), recursive As Boolean) As Guid
        Dim key As String = GetKey(folders, client, exceptExtensions, recursive)
        CApplication.Set(key, Nothing)
    End Function
    Private Shared Function Cached_LatestVersion(appId As Integer, client As String, exceptExtensions As String(), recursive As Boolean) As Guid
        Dim key As String = GetKey(appId, client, exceptExtensions, recursive)
        Dim cached As CVersion = CApplication.Get(key)
        If IsNothing(cached) OrElse cached.Age.TotalSeconds > MAX_SECS_VERSION Then
            SyncLock System.Web.HttpContext.Current.Application
                cached = CApplication.Get(key)
                If IsNothing(cached) OrElse cached.Age.TotalSeconds > MAX_SECS_VERSION Then
                    Dim v As New CVersion
                    v.Hash = Central.GetCurrentVersion(appId, client, exceptExtensions, recursive)
                    CApplication.Set(key, v)
                    cached = v
                End If
            End SyncLock
        End If
        Return cached.Hash
    End Function
    Private Shared Function Cached_LatestVersions(folders As List(Of Integer), client As String, exceptExtensions As String(), recursive As Boolean) As List(Of Guid)
        Dim key As String = GetKey(folders, client, exceptExtensions, recursive)
        Dim cached As CVersion = CApplication.Get(key)
        If IsNothing(cached) OrElse cached.Age.TotalSeconds > MAX_SECS_VERSION Then
            SyncLock System.Web.HttpContext.Current.Application
                cached = CApplication.Get(key)
                If IsNothing(cached) OrElse cached.Age.TotalSeconds > MAX_SECS_VERSION Then
                    Dim v As New CVersion
                    v.Hashs = Central.GetCurrentVersions(folders, client, exceptExtensions, recursive)
                    CApplication.Set(key, v)
                    cached = v
                End If
            End SyncLock
        End If
        Return cached.Hashs
    End Function
    Private Shared Property Cached_Upgrade_Old(localVersion As Guid, remoteVersion As Guid) As Dictionary(Of String, Byte())
        Get
            Dim key As String = GetKey(localVersion, remoteVersion) 'might be stale, compromising subsequent data (unless its shifted to a file-level cache)
            Dim cached As CUpgrade = CApplication.Get(key)
            If IsNothing(cached) Then Return Nothing
            If cached.Age.TotalMinutes > MAX_MINS_UPGRADE Then Return Nothing
            Return cached.Files
        End Get
        Set(value As Dictionary(Of String, Byte()))
            Dim key As String = GetKey(localVersion, remoteVersion)
            Dim val As New CUpgrade()
            val.Files = value
            CApplication.Set(key, val)
        End Set
    End Property
    Private Shared Property Cached_Upgrade_New(localVersion As Guid, remoteVersion As Guid) As List(Of CFileNameAndContent)
        Get
            Dim key As String = GetKey(localVersion, remoteVersion) & "New"
            Dim cached As CUpgrade = CApplication.Get(key)
            If IsNothing(cached) Then Return Nothing
            If cached.Age.TotalMinutes > MAX_MINS_UPGRADE Then Return Nothing
            Return cached.Folder
        End Get
        Set(value As List(Of CFileNameAndContent))
            Dim key As String = GetKey(localVersion, remoteVersion) & "New"
            Dim val As New CUpgrade()
            val.Folder = value
            CApplication.Set(key, val)
        End Set
    End Property

    'Utility
    Private Shared EMPTY As String() = {}
    Private Shared Function GetKey(a As Guid, b As Guid) As String
        Return String.Concat(a, b)
    End Function
    Private Shared Function GetKey(appId As Integer, client As String, exceptExtensions As String(), recursive As Boolean) As String
        If IsNothing(exceptExtensions) Then exceptExtensions = EMPTY
        Return String.Concat(appId, vbTab, client, vbTab, CUtilities.ListToString(exceptExtensions), vbTab, recursive)
    End Function
    Private Shared Function GetKey(folders As List(Of Integer), client As String, exceptExtensions As String(), recursive As Boolean) As String
        If IsNothing(exceptExtensions) Then exceptExtensions = EMPTY
        Return String.Concat(CUtilities.ListToString(folders), vbTab, client, vbTab, CUtilities.ListToString(exceptExtensions), vbTab, recursive)
    End Function



    'Storage - latest version
    Private Class CVersion
        Public Hash As Guid
        Public Hashs As List(Of Guid)

        Public Created As DateTime = DateTime.Now
        Public ReadOnly Property Age As TimeSpan
            Get
                Return DateTime.Now.Subtract(Created)
            End Get
        End Property
    End Class
    Private Class CUpgrade
        Public Files As Dictionary(Of String, Byte())
        Public Folder As List(Of CFileNameAndContent)

        Public Created As DateTime = DateTime.Now
        Public ReadOnly Property Age As TimeSpan
            Get
                Return DateTime.Now.Subtract(Created)
            End Get
        End Property
    End Class
#End Region


End Class
