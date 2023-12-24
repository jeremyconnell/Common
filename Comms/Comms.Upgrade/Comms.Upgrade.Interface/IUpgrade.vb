Imports System.Runtime.Serialization
Imports Comms.PushUpgrade.Interface

'List of Methods (Passthru)
Public Enum EUpgrade
    GetCurrentVersion = 1
    GetFilesToUpgrade = 3
    GetCurrentVersions = 4
    ClearCache = 5

    FirstReport = 11
    OnAppStarted = 12
    OnAppStopped = 13
End Enum


'Native Interface (Trivial overloads should go in the Client base class)
Public Interface IUpgrade
    'Older, file-system based approach (obsolete)
    '*If Content=NULL => Delete the file; else add/update the file
    '*If client is supplied, then check for folder_client (then folder)
    Function GetCurrentVersion(appId As Integer, instanceName As String, exceptExtensions As String(), recursive As Boolean) As Guid
    Function GetFilesToUpgrade(appId As Integer, instanceName As String, localVersion As CFolderHash, exceptExtensions As String(), recursive As Boolean) As CFilesList
    Function GetCurrentVersions(appIds As List(Of Integer), instanceName As String, exceptExtensions As String(), recursive As Boolean) As List(Of Guid)
    Sub ClearCache()
End Interface





