'All methods are shared, but are represented as instance methods, in order to implement an interface. 
'They are accessed like shared methods via the singleton
Imports System.Configuration
Imports System.IO
Imports System.Web
Imports Framework

Public Class CPushUpgradeServer : Implements IPushUpgrade

    Public Sub New()
    End Sub

#Region "Shared - Config"
    Public Shared ReadOnly Property Config_ As CPushUpgradeServer_Config
        Get
            Return CPushUpgradeServer_Config.Shared
        End Get
    End Property
#End Region


#Region "Resolve Enum"
    Public Function ExecuteMethod(methodNameEnum As Integer, params As CReader) As Object
        Try
            Return ExecuteMethod_(CType(methodNameEnum, EPushUpgrade), params)
        Catch ex As Exception
            Return New CException(ex)
        End Try
    End Function
    Private Function ExecuteMethod_(methodName As EPushUpgrade, p As CReader) As Object
        Select Case methodName
            'Functions
            Case EPushUpgrade.RunScripts : Return RunScripts(p)
            Case EPushUpgrade.PushFiles : Return PushFiles(p)
            Case EPushUpgrade.ListDirectory : Return ListDirectory(p)
            Case EPushUpgrade.RequestFile : Return RequestFile(p)
            Case EPushUpgrade.DeleteFile : Return DeleteFile(p)

            Case EPushUpgrade.VerSchConfig : Return VerSchConfig(p)
            Case EPushUpgrade.RequestHash : Return RequestHash(p)
            Case EPushUpgrade.RequestFull : Return RequestFull(p)

            Case EPushUpgrade.RequestLogFile : Return RequestLogFile(p)
            Case EPushUpgrade.RequestWebConfig : Return RequestWebConfig(p)

            Case EPushUpgrade.WriteAppSettings : Return WriteAppSettings(p)
            Case EPushUpgrade.RemoveAppSettings : Return RemoveAppSettings(p)

            Case EPushUpgrade.WriteConnString : Return WriteConnString(p)
            Case EPushUpgrade.RemoveConnString : Return RemoveConnString(p)

            Case EPushUpgrade.ExeIsRunning : Return ExeIsRunning(p)
            Case EPushUpgrade.ExeVersion : Return ExeVersion(p)
            Case EPushUpgrade.ExePushFiles : Return ExePushFiles(p)
            Case EPushUpgrade.ExeStart : Return ExeStart(p)
            Case EPushUpgrade.ExeStop : Return ExeStop(p)
            Case EPushUpgrade.ExeListProcs : Return ExeListProcs(p)
            Case EPushUpgrade.ExeVerConfRunning : Return ExeVerConfRunning(p)
            Case EPushUpgrade.ExeVerConfRunningSet : Return ExeVerConfRunningSet(p)
            Case EPushUpgrade.ExeUninstall : Return ExeUninstall(p)
            Case EPushUpgrade.SetMachineName : Return SetMachineName(p)
            Case EPushUpgrade.RestartMachine : Return RestartMachine()
            Case EPushUpgrade.ReassembleChunks : Return ReassembleChunks(p)

            Case Else : Throw New Exception("Unrecognised EUpgrade method: " & methodName)
        End Select
    End Function
#End Region


    Public Function RequestFileStr(basePath As String) As String
        Return CBinary.BytesToString(RequestFile(basePath))
    End Function

#Region "Parameter Casting"
    Public Function RunScripts(p As CReader) As List(Of String)
        Return RunScripts(p.StrList)
    End Function
    Public Function PushFiles(p As CReader) As CFolderHash
        Return PushFiles(p.Unpack(Of CFilesList), p.Str, p.Bool, p.Int, p.Bool)
    End Function
    Public Function ListDirectory(p As CReader) As List(Of String)
        Return ListDir(p.Str)
    End Function
    Public Function RequestFile(p As CReader) As Byte()
        Return RequestFile(p.Str)
    End Function
    Public Function DeleteFile(p As CReader) As Boolean
        Return DeleteFile(p.Str)
    End Function

    Public Function VerSchConfig(p As CReader) As CVersionSchemaConfig
        Return VerSchConfig(p.Bool, p.Str, p.Bool, p.Int, p.Bool)
    End Function
    Public Function RequestHash(p As CReader) As CFolderHash
        Return RequestHash(p.Str, p.Bool, p.Int, p.Bool)
    End Function
    Public Function RequestFull(p As CReader) As CFilesList
        Return RequestFull(p.Str, p.Bool)
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



    Public Function ExeStart(p As CReader) As CProcess
        Return ExeStart(p.Str)
    End Function
    Public Function ExeStop(p As CReader) As Boolean
        Return ExeStop(p.Str)
    End Function
    Public Function ExeVersion(p As CReader) As CFolderHash
        Return ExeVersion(p.Str, p.Str, p.Bool, p.Int, p.Bool)
    End Function
    Public Function ExeIsRunning(p As CReader) As CProcess
        Return ExeIsRunning(p.Str)
    End Function
    Public Function ExePushFiles(p As CReader) As CFolderHash
        Try
            Return ExePushFiles(p.Str, p.Unpack(Of CFilesList), p.Str, p.Bool, p.Int, p.Bool, p.Bool)
        Catch ex As Exception
            Return ExePushFiles(p.Str, p.Unpack(Of CFilesList), p.Str, p.Bool, CFolderHash.FOUR_MB, p.Bool, p.Bool) 'backwards compat
        End Try
    End Function
    Public Function ExeListProcs(p As CReader) As List(Of CProcess)
        Return ExeListProcs(p.Str)
    End Function

    Public Function ExeVerConfRunning(p As CReader) As CVerConfRunning
        Return ExeVerConfRunning(p.Str, p.Str, p.Str, p.Int, p.Bool)
    End Function
    Public Function ExeVerConfRunningSet(p As CReader) As CVerConfRunningList
        Return ExeVerConfRunningSet(p.StrList, p.StrList, p.Str, p.Int, p.Bool)
    End Function
    Public Function ExeUninstall(p As CReader) As Boolean
        Return ExeUninstall(p.Str)
    End Function

    Public Function SetMachineName(p As CReader) As Integer
        Return SetMachineName(p.Str)
    End Function

    Public Function ReassembleChunks(p As CReader) As String
        Return ReassembleChunks(p.Str, p.Str, p.Int, p.GuidList, p.Int, p.Date, p.Bool)
    End Function

#End Region

#Region "Native Interface (Implementation)"
    Public Function PushFiles(changes As CFilesList, ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CFolderHash Implements IPushUpgrade.PushFiles
        changes.Unpack()
        Dim folderPath As String = SelfFolderPath()

        Dim local As New CFolderHash(folderPath, recursive, ignoreExt, chunkSize, fastHash)
        Try
            local.ApplyDeletesOnly(changes, folderPath)
            local.ApplyAddOrUpdatesOnly(changes, folderPath)
        Catch
            Threading.Thread.Sleep(2000)
            local.ApplyDeletesOnly(changes, folderPath)
            local.ApplyAddOrUpdatesOnly(changes, folderPath)
        End Try

        Return New CFolderHash(folderPath, recursive, ignoreExt, chunkSize, fastHash)
    End Function
    Public Function ListDir(subDir As String) As List(Of String) Implements IPushUpgrade.ListDir
        subDir = TrimPath(subDir)

        Dim dataPath As String = MapPath(subDir)
        If Not Directory.Exists(dataPath) Then
            Try
                Dim webPath As String = HttpContext.Current.Server.MapPath(subDir)
                If Not Directory.Exists(webPath) Then Return New List(Of String)({"Dir Not Found:", dataPath, webPath, subDir})
                subDir = webPath
            Catch
            End Try
        Else
            subDir = dataPath
        End If


        Dim files As String() = Directory.GetFiles(subDir)

        Dim clean As New List(Of String)(files.Length)
        Dim total As Long = 0
        For Each i As String In files
            Dim name As String = Path.GetFileName(i)
            Dim size As Long = New FileInfo(i).Length
            total += size
            clean.Add(CUtilities.FileNameAndSize(name, size))
        Next
        clean.Insert(0, CUtilities.FileNameAndSize(CUtilities.CountSummary(clean, "file"), total))
        Return clean
    End Function
    Public Function RequestFile(basePath As String) As Byte() Implements IPushUpgrade.RequestFile
        If File.Exists(basePath) Then Return File.ReadAllBytes(basePath)
        If basePath.Contains(":\") Then Return Nothing

        Dim path As String = HttpContext.Current.Server.MapPath(basePath)
        If Not File.Exists(path) Then Return Nothing
        Return File.ReadAllBytes(path)
    End Function
    Public Function DeleteFile(basePath As String) As Boolean Implements IPushUpgrade.DeleteFile

        If Not File.Exists(basePath) Then
            basePath = HttpContext.Current.Server.MapPath(basePath)
            If Not File.Exists(basePath) Then Return False
        End If

        Try
            File.Delete(basePath)
            Return True
        Catch
            Return False
        End Try

    End Function

    Public Function VerSchConfig(all As Boolean, ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CVersionSchemaConfig Implements IPushUpgrade.VerSchConfig

        Dim ver As New CVersionSchemaConfig
        ver.VersionMD5 = RequestHash(ignoreExt, recursive, chunkSize, fastHash).Hash
        ver.MachineName = My.Computer.Name

        Try
            ver.Schema = CDataSrc.Default.SchemaInfo()
            ver.SchemaMD5 = ver.Schema.MD5
        Catch
        End Try

        If all Then
            ver.AppSettings = GetAppSettings()
            ver.ConnectionStrings = GetConnectionStrings()
            ver.LogFiles = ListLogFiles()
        Else
            ver.Schema = Nothing
        End If

        Return ver
    End Function

    Public Function RequestHash(ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CFolderHash Implements IPushUpgrade.RequestHash
        Try
            Return New CFolderHash(SelfFolderPath, recursive, ignoreExt, chunkSize, fastHash)
        Catch
            Threading.Thread.Sleep(2000)
            Return New CFolderHash(SelfFolderPath, recursive, ignoreExt, chunkSize, fastHash)
        End Try
    End Function

    Public Function RequestFull(ignoreExt As String, Optional recursive As Boolean = True) As CFilesList Implements IPushUpgrade.RequestFull
        Return New CFilesList(SelfFolderPath, recursive, ignoreExt)
    End Function

    Public Function RunScripts(sql As List(Of String)) As List(Of String) Implements IPushUpgrade.RunScripts
        Dim result As New List(Of String)
        For Each i As String In sql
            Try
                result.Add(CDataSrc.Default.ExecuteNonQuery(i).ToString())
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
        If Not File.Exists(path) Then Return Nothing
        Return File.ReadAllText(path)
    End Function
    Public Function WriteAppSettings(pairs As Dictionary(Of String, String)) As String Implements IPushUpgrade.WriteAppSettings
        For Each i As KeyValuePair(Of String, String) In pairs
            CConfigWriter.WriteAppSetting(i.Key, i.Value)
        Next
        Return RequestWebConfig()
    End Function
    Public Function RemoveAppSettings(pairs As List(Of String)) As String Implements IPushUpgrade.RemoveAppSettings
        For Each i As String In pairs
            CConfigWriter.RemoveAppSetting(i)
        Next
        Return RequestWebConfig()
    End Function

    Public Function WriteConnString(name As String, value As String) As String Implements IPushUpgrade.WriteConnString
        CConfigWriter.WriteConnectionString(name, value)
        Return RequestWebConfig()
    End Function
    Public Function RemoveConnString(name As String) As String Implements IPushUpgrade.RemoveConnString
        CConfigWriter.RemoveConnectionString(name)
        Return RequestWebConfig()
    End Function



    Public Function ExeStart(path As String) As CProcess Implements IPushUpgrade.ExeStart
        path = MapPath(path)

        Dim p As CProcess = GetRunningProcess(path)
        If Not IsNothing(p) Then Return p

        Return StartProcess(path)
    End Function

    Public Function ExeStop(path As String) As Boolean Implements IPushUpgrade.ExeStop
        path = MapPath(path)

        Dim p As Process = GetProcess(path)
        If IsNothing(p) Then Return False

        p.Kill()
        Return True
    End Function

    Public Function ExeIsRunning(path As String) As CProcess Implements IPushUpgrade.ExeIsRunning
        path = MapPath(path)

        Return GetRunningProcess(path)
    End Function

    Public Function ExeVersion(folderName As String, ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CFolderHash Implements IPushUpgrade.ExeVersion
        folderName = TrimPath(folderName)
        If folderName = CFolderHash.WWWROOT Then Return RequestHash(ignoreExt, recursive, chunkSize, fastHash)

        Dim folderPath As String = MapPath(folderName)
        Try
            Return New CFolderHash(folderPath, recursive, ignoreExt, chunkSize, fastHash)
        Catch ex As Exception
            Threading.Thread.Sleep(2000)
            Return New CFolderHash(folderPath, recursive, ignoreExt, chunkSize, fastHash)
        End Try
    End Function

    Public Function ExePushFiles(folderName As String, changes As CFilesList, ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional noHash As Boolean = False, Optional fastHash As Boolean = False) As CFolderHash Implements IPushUpgrade.ExePushFiles
        changes.Unpack()

        folderName = TrimPath(folderName)
        If folderName = CFolderHash.WWWROOT Then Return PushFiles(changes, ignoreExt, recursive, chunkSize, fastHash)

        Dim folderPath As String = MapPath(folderName)
        If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

        Dim local As New CFolderHash(folderPath, recursive, ignoreExt, chunkSize, fastHash)
        Try
            local.ApplyDeletesOnly(changes, folderPath)
            local.ApplyAddOrUpdatesOnly(changes, folderPath)
        Catch ex As Exception
            Threading.Thread.Sleep(3000)
            local.ApplyDeletesOnly(changes, folderPath)
            local.ApplyAddOrUpdatesOnly(changes, folderPath)
        End Try

        If noHash Then Return New CFolderHash()
        Return New CFolderHash(folderPath, recursive, ignoreExt, chunkSize, fastHash)
    End Function



    Public Function ExeListProcs(mask As String) As List(Of CProcess) Implements IPushUpgrade.ExeListProcs
        Dim list As New List(Of CProcess)
        For Each i As Process In Process.GetProcesses()
            If String.IsNullOrEmpty(mask) OrElse i.ProcessName.ToLower.Contains(mask.ToLower) Then
                list.Add(New CProcess(i))
            End If
        Next
        Return list
    End Function




    Public Function ExeVerConfRunning(folder As String, app As String, ignoreExt As String, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CVerConfRunning Implements IPushUpgrade.ExeVerConfRunning
        folder = TrimPath(folder)
        If String.IsNullOrEmpty(app) Then
            If folder = CFolderHash.WWWROOT Then
                Return New CVerConfRunning(folder, Me, CFolderHash.IGNORE_WEB, chunkSize, fastHash) 'web
            Else
                Return New CVerConfRunning(MapPath(folder), Me, String.Empty, chunkSize, fastHash) 'data
            End If
        Else
            Return New CVerConfRunning(MapPath(folder), app, Me, ignoreExt, chunkSize, fastHash)
        End If
    End Function

    Public Function ExeVerConfRunningSet(folders As List(Of String), apps As List(Of String), ignoreExt As String, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CVerConfRunningList Implements IPushUpgrade.ExeVerConfRunningSet
        Dim list As New CVerConfRunningList
        Try
            list.MachineName = Environment.MachineName
            list.AllProcesses = ExeListProcs(String.Empty)
        Catch ex As Exception
            Threading.Thread.Sleep(1000)
            list.MachineName = Environment.MachineName
            list.AllProcesses = ExeListProcs(String.Empty)
        End Try

        list.VersionsAndConfigs = New List(Of CVerConfRunning)(folders.Count)
        For i As Integer = 0 To folders.Count - 1
            Try
                list.VersionsAndConfigs.Add(ExeVerConfRunning(folders(i), apps(i), ignoreExt, chunkSize, fastHash))
            Catch
                list.VersionsAndConfigs.Add(ExeVerConfRunning(folders(i), apps(i), ignoreExt, chunkSize, fastHash))
            End Try
        Next
        Return list
    End Function


    Public Function ExeUninstall(path As String) As Boolean Implements IPushUpgrade.ExeUninstall
        path = MapPath(path)
        If Not Directory.Exists(path) Then Return False
        For Each i As String In Directory.GetFiles(path)
            File.Delete(i)
        Next
        Directory.Delete(path, True)
        Return True
    End Function

    Public Function SetMachineName(name As String) As Integer Implements IPushUpgrade.SetMachineName
        Dim pi As New ProcessStartInfo()
        pi.FileName = "WMIC.exe"
        pi.Arguments = "computersystem where caption='" & System.Environment.MachineName & "' rename " + name
        pi.LoadUserProfile = True

        Using p As Process = Process.Start(pi)
            p.WaitForExit()
            Return p.ExitCode
        End Using
    End Function

    Public Function RestartMachine() As Integer Implements IPushUpgrade.RestartMachine
        Dim pi As New ProcessStartInfo()
        pi.FileName = "shutdown"
        pi.Arguments = " -r -f"
        pi.LoadUserProfile = True

        Using p As Process = Process.Start(pi)
            p.WaitForExit()
            Return p.ExitCode
        End Using
    End Function

    Public Function ReassembleChunks(folderName As String, filePath As String, total As Integer, md5OfList As List(Of Guid), chunkSize As Integer, lastWriteTime As Date, fastHash As Boolean) As String Implements IPushUpgrade.ReassembleChunks
        folderName = TrimPath(folderName)
        If folderName = CFolderHash.WWWROOT Then
            filePath = String.Concat(SelfFolderPath, filePath)
        Else
            filePath = MapPath(String.Concat(folderName, "\", filePath))
        End If
        Return CFolderHash.Reassemble(filePath, total, md5OfList, chunkSize, lastWriteTime, fastHash)
    End Function
#End Region

#Region "Private - appsettings etc"

    Private Shared Function SelfFolderPath() As String
        Dim c As HttpContext = HttpContext.Current
        If Not IsNothing(c) Then
            If Not IsNothing(c.Server) Then Return c.Server.MapPath("~/") & "\"
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
            Dim names As New Dictionary(Of String, Long)(files.Length)
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


    'Path virtualisation
    Private Shared Function TrimPath(pathFromClient As String) As String
        If IsNothing(pathFromClient) Then pathFromClient = CFolderHash.WWWROOT
        If pathFromClient.StartsWith("c:\") Then pathFromClient = pathFromClient.Substring(3)
        Return pathFromClient
    End Function
    Private Shared Function MapPath(pathFromClient As String) As String
        Return String.Concat("c:\", TrimPath(pathFromClient))
    End Function

#End Region


#Region "Process control"
    Public Shared Function StartProcess(pathToExe As String) As CProcess
        Dim ps As New ProcessStartInfo(pathToExe)
        ps.LoadUserProfile = True
        Return StartProcess(pathToExe, ps)
    End Function
    Public Shared Function StartProcess(pathToExe As String, pi As ProcessStartInfo) As CProcess
        Dim p As Process = Process.Start(pi)
        StoreProcessId(pathToExe, p.Id) 'pi.FileName
        Return New CProcess(p)
    End Function
    Public Shared Function GetRunningProcess(pathToExe As String) As CProcess
        Dim p As Process = GetProcess(pathToExe)
        If IsNothing(p) Then Return Nothing

        If p.HasExited Then
            CleaerProcessId(pathToExe)
            Return Nothing
        End If
        Return New CProcess(p)
    End Function
    Private Shared Function GetProcess(pathToExe As String) As Process
        Dim id As Integer = GetProcessId(pathToExe)
        If Integer.MinValue = id Then Return Nothing

        Try
            Return Process.GetProcessById(id)
        Catch
            CleaerProcessId(pathToExe)
            Return Nothing
        End Try
    End Function


    Private Shared Sub CleaerProcessId(pathToExe As String)
        File.Delete(StoreProcessIdPath(pathToExe))
    End Sub
    Private Shared Sub StoreProcessId(pathToExe As String, id As Integer)
        Dim pathToTxt As String = StoreProcessIdPath(pathToExe)
        File.WriteAllText(pathToTxt, id.ToString())
    End Sub
    Private Shared Function GetProcessId(pathToExe As String) As Integer
        Dim pathToTxt As String = StoreProcessIdPath(pathToExe)
        If Not File.Exists(pathToTxt) Then Return Integer.MinValue
        Return Integer.Parse(File.ReadAllText(pathToTxt))
    End Function
    Private Shared Function StoreProcessIdPath(pathToExe As String) As String
        'key => filename to store 
        Dim md5 As Guid = CBinary.MD5_(pathToExe.ToLower().Replace("\\", "\"))

        'store in app-data
        Dim dir As String = HttpContext.Current.Server.MapPath("~/app_data/")
        If Not Directory.Exists(dir) Then Directory.CreateDirectory(dir)

        Return String.Concat(dir, md5, ".txt")
    End Function
#End Region

End Class