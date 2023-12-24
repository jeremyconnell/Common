Imports System.Runtime.Serialization
Imports Framework

'List of Methods (Passthru)
Public Enum EPushUpgrade
    RunScripts = 2
    PushFiles = 3
    ListDirectory = 4
    RequestFile = 5
    DeleteFile = 6

    VerSchConfig = 7
    RequestHash = 8
    RequestFull = 9

    RequestLogFile = 11
    RequestWebConfig = 12

    WriteAppSettings = 13
    RemoveAppSettings = 14
    WriteConnString = 15
    RemoveConnString = 16

    ExeStart = 20
    ExeStop = 21
    ExeIsRunning = 22
    ExeVersion = 23
    ExePushFiles = 24
    ExeListProcs = 25
    ExeVerConfRunning = 26
    ExeVerConfRunningSet = 27
    ExeUninstall = 28

    SetMachineName = 30
    RestartMachine = 31

    'TODO: LARGE FILE IN CHUNKS: START/ADD/COMPLETE, ID+DONE+LEFT
    ReassembleChunks = 40
End Enum


'Native Interface (Trivial overloads should go in the Client base class)
Public Interface IPushUpgrade
    'Upgrade
    Function RunScripts(scripts As List(Of String)) As List(Of String)
    Function PushFiles(changes As CFilesList, ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CFolderHash
    Function ListDir(subDir As String) As List(Of String)
    Function RequestFile(name As String) As Byte()
    Function DeleteFile(name As String) As Boolean

    'Monitor/Maintain
    Function VerSchConfig(allData As Boolean, ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CVersionSchemaConfig
    Function RequestHash(ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CFolderHash
    Function RequestFull(ignoreExt As String, Optional recursive As Boolean = True) As CFilesList

    'Log-Files, Config-Files
    Function RequestLogFile(name As String) As String
    Function RequestWebConfig() As String

    'Appsettings
    Function WriteAppSettings(pairs As Dictionary(Of String, String)) As String
    Function RemoveAppSettings(keys As List(Of String)) As String
    Function WriteConnString(name As String, value As String) As String
    Function RemoveConnString(keys As String) As String

    'Exe management
    Function ExeStart(path As String) As CProcess
    Function ExeStop(path As String) As Boolean
    Function ExeIsRunning(path As String) As CProcess
    Function ExeVersion(path As String, ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CFolderHash
    Function ExePushFiles(path As String, changes As CFilesList, ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional noHash As Boolean = False, Optional fastHash As Boolean = False) As CFolderHash
    Function ExeListProcs(mask As String) As List(Of CProcess)
    Function ExeVerConfRunning(folder As String, app As String, ignoreExt As String, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CVerConfRunning
    Function ExeVerConfRunningSet(folders As List(Of String), apps As List(Of String), ignoreExt As String, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CVerConfRunningList
    Function ExeUninstall(path As String) As Boolean

    Function SetMachineName(name As String) As Integer
    Function RestartMachine() As Integer

    Function ReassembleChunks(folder As String, filePath As String, total As Integer, md5OfList As List(Of Guid), chunkSize As Integer, lastWriteTime As DateTime, fastHash As Boolean) As String
End Interface

