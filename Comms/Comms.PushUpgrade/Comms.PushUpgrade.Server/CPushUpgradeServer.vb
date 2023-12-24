'All methods are shared, but are represented as instance methods, in order to implement an interface. 
'They are accessed like shared methods via the singleton
Imports System.Configuration
Imports System.IO
Imports System.Web
Imports Comms.PushUpgrade.Interface
Imports Framework

Public Class CPushUpgradeServer : Implements IPushUpgrade

    Public Delegate Sub DTriggerUpgrade()
    Private m_triggerUpgrade As DTriggerUpgrade


    Public Delegate Sub DMoreConfigSettings(d As Dictionary(Of String, String))
    Private m_moreConfigSettings As DMoreConfigSettings

    Public Delegate Function DRemoveConfigSetting(name As String) As Boolean
    Private m_removeConfigSetting As DRemoveConfigSetting


    Public Sub New(Optional triggerUpgrade As DTriggerUpgrade = Nothing, Optional moreConfigSettings As DMoreConfigSettings = Nothing, Optional removeConfigSetting As DRemoveConfigSetting = Nothing)
        m_triggerUpgrade = triggerUpgrade
        m_moreConfigSettings = moreConfigSettings
        m_removeConfigSetting = removeConfigSetting
    End Sub

#Region "Shared - Config"
    Public Shared ReadOnly Property Config_ As CPushUpgradeServer_Config
        Get
            Return CPushUpgradeServer_Config.Shared
        End Get
    End Property
    Private Shared Function GetId() As CIdentity
        Dim id As New CIdentity
        Return id
    End Function
#End Region


#Region "Resolve Enum"
    Public Function ExecuteMethod(methodNameEnum As Integer, params As CReader) As Object
        Return ExecuteMethod_(methodNameEnum, params)
    End Function
    Private Function ExecuteMethod_(methodName As EPushUpgrade, p As CReader) As Object
        Select Case methodName
            'Functions
            Case EPushUpgrade.TriggerUpgr : Return TriggerUpgrade(p)
            Case EPushUpgrade.RunScripts : Return RunScripts(p)
            Case EPushUpgrade.PushFiles : Return PushFiles(p)
            Case EPushUpgrade.ListDirectory : Return ListDirectory(p)
            Case EPushUpgrade.RequestFile : Return RequestFile(p)
            Case EPushUpgrade.DeleteFile : Return DeleteFile(p)

            Case EPushUpgrade.PollVersion : Return PollVersion(p)
            Case EPushUpgrade.RequestHash : Return RequestHash(p)
            Case EPushUpgrade.RequestFull : Return RequestFull(p)
            Case EPushUpgrade.SetInstance : Return SetInstance(p)

            Case EPushUpgrade.RequestLogFile : Return RequestLogFile(p)
            Case EPushUpgrade.RequestWebConfig : Return RequestWebConfig(p)

            Case EPushUpgrade.WriteAppSettings : Return WriteAppSettings(p)
            Case EPushUpgrade.RemoveAppSettings : Return RemoveAppSettings(p)

            Case EPushUpgrade.WriteConnString : Return WriteConnString(p)
            Case EPushUpgrade.RemoveConnString : Return RemoveConnString(p)


            Case Else : Throw New Exception("Unrecognised EUpgrade method: " & methodName)
        End Select
    End Function
#End Region

#Region "Parameter Casting"
    Public Function TriggerUpgrade(p As CReader) As CException
        Return TriggerUpgrade()
    End Function
    Public Function RunScripts(p As CReader) As List(Of String)
        Return RunScripts(p.StrList)
    End Function
    Public Function PushFiles(p As CReader) As CException
        Return PushFiles(p.Unpack(Of CFilesList))
    End Function
    Public Function ListDirectory(p As CReader) As List(Of String)
        Return ListDirectory(p.Str)
    End Function
    Public Function RequestFile(p As CReader) As Byte()
        Return RequestFile(p.Str)
    End Function
    Public Function DeleteFile(p As CReader) As Boolean
        Return DeleteFile(p.Str)
    End Function

    Public Function PollVersion(p As CReader) As CMyVersion
        Return PollVersion(p.Unpack(Of CFilter), p.Bool)
    End Function
    Public Function RequestHash(p As CReader) As CFolderHash
        Return RequestHash(p.Unpack(Of CFilter))
    End Function
    Public Function RequestFull(p As CReader) As CFilesList
        Return RequestFull(p.Unpack(Of CFilter))
    End Function
    Public Function SetInstance(p As CReader) As CMyVersion
        Return SetInstance(p.Unpack(Of CInstanceInfo))
    End Function

    Public Function RequestLogFile(p As CReader) As String
        Return RequestLogFile(p.Str)
    End Function
    Public Function RequestWebConfig(p As CReader) As String
        Return RequestWebConfig()
    End Function

    Public Function WriteAppSettings(p As CReader) As String
        Return WriteAppSettings(p.DictStrStr)
    End Function
    Public Function RemoveAppSettings(p As CReader) As String
        Return RemoveAppSettings(p.StrList)
    End Function
    Public Function WriteConnString(p As CReader) As String
        Return WriteConnString(p.Str, p.Str)
    End Function
    Public Function RemoveConnString(p As CReader) As String
        Return RemoveConnString(p.Str)
    End Function

#End Region

#Region "Native Interface (Implementation)"
    Public Function TriggerUpgrade() As CException Implements IPushUpgrade.TriggerUpgrade
        Try
            m_triggerUpgrade()
            Return Nothing
        Catch ex As Exception
            Return New CException(ex)
        End Try
    End Function

    Public Function SetInstance(info As CInstanceInfo) As CMyVersion Implements IPushUpgrade.SetInstance
        Dim id As CIdentity = info.Instance
        Return PollVersion(info.Filter, True)
    End Function
    Public Function PushFiles(changes As CFilesList) As CException Implements IPushUpgrade.PushFiles
        'CFolderHash.ApplyChanges(changes, SelfFolderPath)
        'Dim c As CUpgradeClient = CUpgradeClient.Factory()
        Try
            Dim folderPath As String = SelfFolderPath()
            Dim local As New CFolderHash(folderPath, Nothing, True)

            local.ApplyDeletesOnly(changes, folderPath)
            local.ApplyAddOrUpdatesOnly(changes, folderPath)

            CApplication.ClearAll()
            Return Nothing
        Catch ex As Exception
            Return New CException(ex)
        End Try
    End Function
    Public Function ListDirectory(subDir As String) As List(Of String) Implements IPushUpgrade.ListDirectory
        Dim dir As String = HttpContext.Current.Server.MapPath(subDir)
        If Not Directory.Exists(dir) Then
            dir = subDir
            If Not Directory.Exists(dir) Then Return Nothing
        End If

        Return New List(Of String)(Directory.GetFiles(dir))
    End Function
    Public Function RequestFile(basePath As String) As Byte() Implements IPushUpgrade.RequestFile
        Dim path As String = HttpContext.Current.Server.MapPath(basePath)
        If Not File.Exists(path) Then
            path = basePath
            If Not File.Exists(path) Then Return Nothing
        End If

        Return File.ReadAllBytes(path)
    End Function
    Public Function DeleteFile(basePath As String) As Boolean Implements IPushUpgrade.DeleteFile
        Dim path As String = HttpContext.Current.Server.MapPath(basePath)
        If Not File.Exists(path) Then
            path = basePath
            If Not File.Exists(path) Then Return False
        End If

        Try
            File.Delete(path)
            Return True
        Catch
            Return False
        End Try

    End Function

    Public Function PollVersion(f As CFilter, allData As Boolean) As CMyVersion Implements IPushUpgrade.PollVersion
        Dim folder As CFolderHash = RequestHash(f)

        Dim ver As New CMyVersion
        ver.MD5 = folder.Hash
        ver.Id = GetId()

        Try
            ver.Schema = CDataSrc.Default.SchemaInfo()
            ver.SchemaMD5 = ver.Schema.MD5
            If Not allData Then ver.Schema = Nothing
        Catch
        End Try

        If allData Then
            ver.AppSettings = GetAppSettings()
            ver.ConnectionStrings = GetConnectionStrings()
            ver.LogFiles = ListLogFiles()

            If Not IsNothing(m_moreConfigSettings) Then
                Try
                    m_moreConfigSettings(ver.AppSettings)
                Catch
                End Try
            End If
        End If

        Return ver
    End Function

    Public Function RequestHash(f As CFilter) As CFolderHash Implements IPushUpgrade.RequestHash
        Return New CFolderHash(SelfFolderPath, f.Ignore, f.Recursive)
    End Function

    Public Function RequestFull(f As CFilter) As CFilesList Implements IPushUpgrade.RequestFull
        Return New CFilesList(New CFolderHash(SelfFolderPath, f.Ignore, f.Recursive))
    End Function

    Public Function RunScripts(sql As List(Of String)) As List(Of String) Implements IPushUpgrade.RunScripts
        Dim result As New List(Of String)
        For Each i As String In sql
            Try
                result.Add(CDataSrc.Default.ExecuteNonQuery(i))
            Catch ex As Exception
                result.Add(ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
            End Try
        Next
        Return result
    End Function



    Public Function RequestLogFile(name As String) As String Implements IPushUpgrade.RequestLogFile
        Dim path As String = HttpContext.Current.Server.MapPath("~/logs/")
        If Not IO.Directory.Exists(path) Then Return "Dir not found: ~/logs/"
        path &= name
        If Not IO.File.Exists(path) Then Return "File not found: ~/logs/" & name
        Return IO.File.ReadAllText(path)
    End Function


    Public Function RequestWebConfig() As String Implements IPushUpgrade.RequestWebConfig
        Dim path As String = HttpContext.Current.Server.MapPath("~/web.config")
        If Not IO.File.Exists(path) Then Return Nothing
        Return IO.File.ReadAllText(path)
    End Function
    Public Function WriteAppSettings(pairs As Dictionary(Of String, String)) As String Implements IPushUpgrade.WriteAppSettings
        For Each i In pairs
            CConfigBase.WriteAppSetting(i.Key, i.Value)
        Next
        Return RequestWebConfig()
    End Function
    Public Function RemoveAppSettings(pairs As List(Of String)) As String Implements IPushUpgrade.RemoveAppSettings
        For Each i In pairs
            CConfigBase.RemoveAppSetting(i)
            If Not IsNothing(m_removeConfigSetting) Then
                Try
                    m_removeConfigSetting(i)
                Catch
                End Try
            End If
        Next
        Return RequestWebConfig()
    End Function

    Public Function WriteConnString(name As String, value As String) As String Implements IPushUpgrade.WriteConnString
        CConfigBase.WriteConnectionString(name, value)
        Return RequestWebConfig()
    End Function
    Public Function RemoveConnString(name As String) As String Implements IPushUpgrade.RemoveConnString
        CConfigBase.RemoveConnectionString(name)
        Return RequestWebConfig()
    End Function
#End Region

    Private Shared Function SelfFolderPath() As String
        Dim c = HttpContext.Current
        If Not IsNothing(c) Then
            If Not IsNothing(c.Server) Then Return c.Server.MapPath("~/")
        End If
        Return My.Application.Info.DirectoryPath & "\"
    End Function

    Private Shared Function GetAppSettings() As Dictionary(Of String, String)
        Try
            Dim keys As String() = ConfigurationManager.AppSettings.AllKeys
            Dim d As New Dictionary(Of String, String)(keys.Length)
            For Each i As String In keys
                d.Add(i, ConfigurationManager.AppSettings(i))
            Next


            Return d
        Catch ex As Exception
            Dim dd As New Dictionary(Of String, String)()
            dd.Add(ex.Message, ex.StackTrace)
            If Not IsNothing(ex.InnerException) Then
                ex = ex.InnerException
                dd.Add(ex.Message, ex.StackTrace)
            End If
            Return dd
        End Try
    End Function
    Private Shared Function GetConnectionStrings() As Dictionary(Of String, String)
        Try
            Dim keys As ConnectionStringSettingsCollection = ConfigurationManager.ConnectionStrings
            Dim d As New Dictionary(Of String, String)(keys.Count)
            For Each i As ConnectionStringSettings In keys
                d.Add(i.Name, i.ConnectionString)
            Next
            Return d
        Catch ex As Exception
            Dim dd As New Dictionary(Of String, String)()
            dd.Add(ex.Message, ex.StackTrace)
            If Not IsNothing(ex.InnerException) Then
                ex = ex.InnerException
                dd.Add(ex.Message, ex.StackTrace)
            End If
            Return dd
        End Try
    End Function

    Private Shared Function ListLogFiles() As Dictionary(Of String, Long)
        Try
            Dim path As String = HttpContext.Current.Server.MapPath("~/logs/")
            Dim files As String() = IO.Directory.GetFiles(path)
            Dim names As New Dictionary(Of String, Long)(files.Count)
            For Each i As String In files
                Dim name As String = IO.Path.GetFileName(i)
                Dim size As Long = New IO.FileInfo(i).Length
                names.Add(name, size)
            Next
            Return names
        Catch ex As Exception
            Dim dd As New Dictionary(Of String, Long)
            dd.Add(ex.Message, ex.Message.Length)
            dd.Add(ex.StackTrace, ex.StackTrace.Length)
            If Not IsNothing(ex.InnerException) Then
                ex = ex.InnerException
                dd.Add(ex.Message, ex.Message.Length)
                dd.Add(ex.StackTrace, ex.StackTrace.Length)
            End If
            Return dd
        End Try
    End Function
End Class