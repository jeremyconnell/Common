Imports System.Runtime.Serialization
Imports Framework

'List of Methods (Passthru)
Public Enum EPushUpgrade
    TriggerUpgr = 1
    RunScripts = 2
    PushFiles = 3
    ListDirectory = 4
    RequestFile = 5
    DeleteFile = 6

    PollVersion = 7
    RequestHash = 8
    RequestFull = 9
    SetInstance = 10

    RequestLogFile = 11
    RequestWebConfig = 12

    WriteAppSettings = 13
    RemoveAppSettings = 14

    WriteConnString = 15
    RemoveConnString = 16
End Enum


'Native Interface (Trivial overloads should go in the Client base class)
Public Interface IPushUpgrade
    'Upgrades/Ftp
    Function TriggerUpgrade() As CException
    Function RunScripts(scripts As List(Of String)) As List(Of String)
    Function PushFiles(changes As CFilesList) As CException
    Function ListDirectory(subDir As String) As List(Of String)
    Function RequestFile(name As String) As Byte()
    Function DeleteFile(name As String) As Boolean

    'Monitor/Maintain
    Function PollVersion(filter As CFilter, allData As Boolean) As CMyVersion
    Function RequestHash(filter As CFilter) As CFolderHash
    Function RequestFull(filter As CFilter) As CFilesList
    Function SetInstance(info As CInstanceInfo) As CMyVersion

    'Log-Files, Config-Files
    Function RequestLogFile(name As String) As String
    Function RequestWebConfig() As String

    'Appsettings
    Function WriteAppSettings(pairs As Dictionary(Of String, String)) As String
    Function RemoveAppSettings(keys As List(Of String)) As String
    Function WriteConnString(name As String, value As String) As String
    Function RemoveConnString(keys As String) As String
End Interface

