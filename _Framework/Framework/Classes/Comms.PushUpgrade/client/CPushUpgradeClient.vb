Imports System.IO
Imports System.Threading.Tasks
Imports System.Web
Imports Framework

Public MustInherit Class CPushUpgradeClient : Inherits CClient_RestOrWcf : Implements IPushUpgrade

#Region "Constructors"
    Protected Sub New(hostName As String)
        Me.New(hostName, Config_.UseSsl, Config_)
    End Sub
    Protected Sub New(hostName As String, useSsl As Boolean)
        Me.New(hostName, useSsl, Config_)
    End Sub
    'All methods use: Protobuf serialisation, no compression (unless otherwise specified)
    Protected Sub New(hostName As String, useSsl As Boolean, password As IConfig_Client)
        MyBase.New(hostName, useSsl, password, EGzip.None, ESerialisation.Protobuf)
    End Sub
#End Region


#Region "Shared - Config"
    Public Shared ReadOnly Property Config_ As IConfig_PushUpgradeClient
        Get
            Return CPushUpgradeClient_Config.Shared
        End Get
    End Property
#End Region


#Region "Overloads"
    Public Function VerSchConfig(allData As Boolean) As CVersionSchemaConfig
        Return VerSchConfig(allData, Config_.ExtensionsToIgnore, True)
    End Function
    Public Function RequestHash() As CFolderHash
        Return RequestHash(Config_.ExtensionsToIgnore, True)
    End Function
    Public Function RequestFiles() As CFilesList
        Return RequestFiles(Config_.ExtensionsToIgnore, True)
    End Function

    'Trivial
    Public Function VerSchConfig_Base64() As String
        Return VerSchConfig(False).MD5_Base64
    End Function
    Public Function VerSchConfig_Base64Trunc() As String
        Return VerSchConfig(False).MD5_Base64Trunc
    End Function
    Public Function RequestHash_Base64() As String
        Return RequestHash.Base64
    End Function
    Public Function RequestHash_Base64Trunc() As String
        Return RequestHash.Base64Trunc
    End Function


    Public Function RequestFileStr(name As String) As String
        Return CBinary.BytesToString(RequestFile(name))
    End Function

    Public Function ExeListProcs() As List(Of CProcess)
        Return ExeListProcs(String.Empty)
    End Function

    Public Function ExeVerConfRunningSet(foldersAndExeNames As Dictionary(Of String, String), ignoreExt As String, fastHash As Boolean, Optional chunkSize As Integer = CFolderHash.FOUR_MB) As CVerConfRunningList
        Dim folderNames As New List(Of String)(foldersAndExeNames.Keys)
        Dim appNames As New List(Of String)(foldersAndExeNames.Values)
        Return ExeVerConfRunningSet(folderNames, appNames, ignoreExt, chunkSize, fastHash)
    End Function
#End Region

#Region "Public Interface (MethodName-to-enum, output Deserialisation, optional gzip)"
    Public Function RunScripts(sql As List(Of String)) As List(Of String) Implements IPushUpgrade.RunScripts
        Return Deserialiser.AsListStr(Invoke(EPushUpgrade.RunScripts, EGzip.Both, sql))
    End Function
    Public Function PushFiles(changes As CFilesList, exceptExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CFolderHash Implements IPushUpgrade.PushFiles
        Return ExePushFiles(CFolderHash.WWWROOT, changes, exceptExt, recursive, chunkSize, fastHash)
    End Function
    Public Function ListDir(subDir As String) As List(Of String) Implements IPushUpgrade.ListDir
        Return Deserialiser.AsListStr(Invoke(EPushUpgrade.ListDirectory, EGzip.Output, subDir))
    End Function
    Public Function RequestFile(name As String) As Byte() Implements IPushUpgrade.RequestFile
        Return Invoke(EPushUpgrade.RequestFile, EGzip.Output, name)
    End Function
    Public Function DeleteFile(name As String) As Boolean Implements IPushUpgrade.DeleteFile
        Return Deserialiser.AsBool(Invoke(EPushUpgrade.DeleteFile, name))
    End Function




    Public Function VerSchConfig(allData As Boolean, ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CVersionSchemaConfig Implements IPushUpgrade.VerSchConfig
        Return AsMyVersion(Invoke(EPushUpgrade.VerSchConfig, EGzip.Output, allData, ignoreExt, recursive, chunkSize, fastHash))
    End Function

    Public Function RequestHash(ignoreExt As String, Optional recursive As Boolean = True, Optional maxSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CFolderHash Implements IPushUpgrade.RequestHash
        Return AsFolderHash(Invoke(EPushUpgrade.RequestHash, EGzip.Output, ignoreExt, recursive, maxSize, fastHash))
    End Function

    Public Function RequestFiles(ignoreExt As String, Optional recursive As Boolean = True) As CFilesList Implements IPushUpgrade.RequestFull
        Return AsFiles(Invoke(EPushUpgrade.RequestFull, EGzip.Output, ignoreExt, recursive))
    End Function

    Public Function RequestLogFile(name As String) As String Implements IPushUpgrade.RequestLogFile
        Return Deserialiser.AsStr(Invoke(EPushUpgrade.RequestLogFile, EGzip.Output, name))
    End Function

    Public Function RequestWebConfig() As String Implements IPushUpgrade.RequestWebConfig
        Return Deserialiser.AsStr(Invoke(EPushUpgrade.RequestWebConfig, EGzip.Output))
    End Function


    Public Function WriteAppSettings(pairs As Dictionary(Of String, String)) As String Implements IPushUpgrade.WriteAppSettings
        Return Deserialiser.AsStr(Invoke(EPushUpgrade.WriteAppSettings, EGzip.Both, pairs))
    End Function
    Public Function RemoveAppSettings(pairs As List(Of String)) As String Implements IPushUpgrade.RemoveAppSettings
        Return Deserialiser.AsStr(Invoke(EPushUpgrade.WriteAppSettings, EGzip.Both, pairs))
    End Function

    Public Function WriteConnString(name As String, value As String) As String Implements IPushUpgrade.WriteConnString
        Return Deserialiser.AsStr(Invoke(EPushUpgrade.WriteConnString, EGzip.Both, name, value))
    End Function

    Public Function RemoveConnString(name As String) As String Implements IPushUpgrade.RemoveConnString
        Return Deserialiser.AsStr(Invoke(EPushUpgrade.RemoveConnString, EGzip.Input, name))
    End Function







    Public Function ExeStart(path As String) As CProcess Implements IPushUpgrade.ExeStart
        Return Deserialiser.Deserialise_(Of CProcess)(Invoke(EPushUpgrade.ExeStart, path))
    End Function
    Public Function ExeStop(path As String) As Boolean Implements IPushUpgrade.ExeStop
        Deserialiser.AsBool(Invoke(EPushUpgrade.ExeStop, path))
    End Function
    Public Function ExeIsRunning(path As String) As CProcess Implements IPushUpgrade.ExeIsRunning
        Return Deserialiser.Deserialise_(Of CProcess)(Invoke(EPushUpgrade.ExeIsRunning, path))
    End Function
    Public Function ExeVersion(remotePath As String, exceptExt As String, Optional recursive As Boolean = True, Optional maxSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CFolderHash Implements IPushUpgrade.ExeVersion
        Return AsFolderHash(Invoke(EPushUpgrade.ExeVersion, EGzip.Output, remotePath, exceptExt, recursive, maxSize, fastHash))
    End Function
    Public Function ExePushFiles(remotePath As String, changes As CFilesList, exceptExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional dontHash As Boolean = False, Optional fastHash As Boolean = False) As CFolderHash Implements IPushUpgrade.ExePushFiles
        'Shuffle the list
        'changes = New CFilesList(CShuffle.Shuffle(Of CFileNameAndContent)(changes))
        changes.Sort(New Comparison(Of CFileNameAndContent)(
                               Function(x As CFileNameAndContent, y As CFileNameAndContent) As Integer
                                   Dim xx As Integer = 0
                                   Dim yy As Integer = 0
                                   If Not IsNothing(x.Content) Then xx = x.Content.Length
                                   If Not IsNothing(y.Content) Then yy = y.Content.Length
                                   Return xx.CompareTo(yy)
                               End Function))


        Dim doMiddle As New CFilesList() 'chunks
        Dim doLast As New CFilesList() 'reassembly
        Dim ignore As New CFilesList
        Dim defer As New CFilesList

        Dim lastHash As CFolderHash = Nothing
        Dim doingMiddle As Boolean = False
        While changes.Count > 0
            Dim temp As New CFilesList()
            While temp.Total < chunkSize And changes.Count > 0
                'Remove one file
                Dim file As CFileNameAndContent = changes(0)
                changes.RemoveAt(0)

                If IsNothing(file.Content) Then
                    defer.Add(file)
                    'temp.Add(file)  'delete command
                Else
                    If file.Content.Length > chunkSize Then
                        ignore.Add(file)    'Large file
                        'temp.Add(file)
                    ElseIf file.Name.EndsWith(CFolderHash.CHUNKFILE) Then
                        If doingMiddle Then
                            temp.Add(file)
                        Else
                            doMiddle.Add(file)  'large file as chunks
                        End If
                    ElseIf file.Name.EndsWith(CFolderHash.REASSEMBLY) Then
                        doLast.Add(file)    'large file reassembly
                    Else
                        temp.Add(file)  'regular files
                    End If
                End If

                If temp.Total > chunkSize Then temp.Pack()
            End While

            'Last file pushed it over the limit => see if it zips down, otherwise remove
            Dim zipLen As Long = CBinary.Zip(CProto.Serialise(temp)).Length
            If zipLen > chunkSize AndAlso temp.Count > 1 Then
                Dim last As CFileNameAndContent = temp(temp.Count - 1)
                temp.Remove(last)
                changes.Add(last)
            Else
                'accounting for zip creates more room
                Dim left As Integer = CInt(chunkSize - zipLen)
                While changes.Count > 0
                    Dim nxt As CFileNameAndContent = changes(0)
                    If Not IsNothing(nxt.Content) Then
                        Dim bb As Byte() = nxt.Content
                        If bb.Length > 4 * left Then Exit While
                        ''bb = CBinary.Zip(bb)
                        If bb.Length > left Then Exit While
                        left = left - bb.Length
                    End If

                    changes.RemoveAt(0)
                    temp.Add(nxt)
                    temp.Pack()
                End While
            End If


            'Send groups of small files
            If temp.Count > 0 Then
                Try
                    If remotePath = CFolderHash.WWWROOT Then
                        Deserialiser.AsFolderHash(Invoke(EPushUpgrade.PushFiles, EGzip.Both, temp, exceptExt, recursive, chunkSize, fastHash))
                    Else
                        Deserialiser.AsFolderHash(Invoke(EPushUpgrade.ExePushFiles, EGzip.Both, remotePath, temp, exceptExt, recursive, chunkSize, False, fastHash))
                    End If
                Catch
                    If remotePath = CFolderHash.WWWROOT Then
                        Deserialiser.AsFolderHash(Invoke(EPushUpgrade.PushFiles, EGzip.Both, temp, exceptExt, recursive, chunkSize, fastHash))
                    Else
                        Deserialiser.AsFolderHash(Invoke(EPushUpgrade.ExePushFiles, EGzip.Both, remotePath, temp, exceptExt, recursive, chunkSize, False, fastHash))
                    End If
                End Try
                temp.Clear()
            End If

            'Do chunks
            If changes.Count = 0 AndAlso doMiddle.Count > 0 Then
                changes.AddRange(doMiddle)
                doMiddle.Clear()
                doingMiddle = True
            End If
        End While


        'Reassemble big files
        For Each i As CFileNameAndContent In doLast
            'Get number and path
            Dim ss As New List(Of String)(i.Name.Split(CChar(".")))
            ss.RemoveAt(ss.Count - 1) 'reassemble
            Dim n As Integer = Integer.Parse(ss(ss.Count - 1))
            ss.RemoveAt(ss.Count - 1) 'total
            ss.RemoveAt(ss.Count - 1) 'of
            ss.RemoveAt(ss.Count - 1) 'index
            Dim filePath As String = CUtilities.ListToString(ss, ".")

            'Reassemble, clean up chunks at both ends
            Dim md5List As List(Of Guid) = CProto.Deserialise(Of List(Of Guid))(i.Content)
            Dim msg As String = ReassembleChunks(remotePath, filePath, n, md5List, chunkSize, i.LastWriteTimeUtc.Value, fastHash)
            If Not String.IsNullOrEmpty(msg) Then
                Throw New Exception("Reassemble Failed: " & vbCrLf & msg)
            End If
        Next

        'deletes done last
        For Each i As CFileNameAndContent In defer
            If remotePath = CFolderHash.WWWROOT Then
                Deserialiser.AsFolderHash(Invoke(EPushUpgrade.PushFiles, EGzip.Output, defer, exceptExt, recursive, chunkSize, False, fastHash))
            Else
                Deserialiser.AsFolderHash(Invoke(EPushUpgrade.ExePushFiles, EGzip.Output, remotePath, defer, exceptExt, recursive, chunkSize, False, fastHash))
            End If
        Next

        'Final hash
        Return ExeVersion(remotePath, exceptExt, recursive)
    End Function
    Public Function ExeListProcs(mask As String) As List(Of CProcess) Implements IPushUpgrade.ExeListProcs
        Return Deserialiser.Deserialise_(Of List(Of CProcess))(Invoke(EPushUpgrade.ExeListProcs, EGzip.Output, mask))
    End Function
    Public Function ExeVerConfRunning(folderName As String, appName As String, ignoreExt As String, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CVerConfRunning Implements IPushUpgrade.ExeVerConfRunning
        Return Deserialiser.Deserialise_(Of CVerConfRunning)(Invoke(EPushUpgrade.ExeVerConfRunning, EGzip.Output, folderName, appName, ignoreExt, chunkSize, fastHash))
    End Function
    Public Function ExeVerConfRunningSet(folderNames As List(Of String), appNames As List(Of String), ignoreExt As String, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As CVerConfRunningList Implements IPushUpgrade.ExeVerConfRunningSet
        Return Deserialiser.Deserialise_(Of CVerConfRunningList)(Invoke(EPushUpgrade.ExeVerConfRunningSet, EGzip.Output, folderNames, appNames, ignoreExt, chunkSize, fastHash))
    End Function
    Public Function ExeUninstall(path As String) As Boolean Implements IPushUpgrade.ExeUninstall
        Return Deserialiser.AsBool(Invoke(EPushUpgrade.ExeUninstall, path))
    End Function


    Public Function SetMachineName(name As String) As Integer Implements IPushUpgrade.SetMachineName
        Return Deserialiser.AsInt(Invoke(EPushUpgrade.SetMachineName, name))
    End Function
    Public Function RestartMachine() As Integer Implements IPushUpgrade.RestartMachine
        Return Deserialiser.AsInt(Invoke(EPushUpgrade.RestartMachine))
    End Function

    Public Function ReassembleChunks(folder As String, filePath As String, total As Integer, md5OfList As List(Of Guid), chunkSize As Integer, lastWriteTime As DateTime, fastHash As Boolean) As String Implements IPushUpgrade.ReassembleChunks
        Return Deserialiser.AsStr(Invoke(EPushUpgrade.ReassembleChunks, EGzip.None, folder, filePath, total, md5OfList, chunkSize, lastWriteTime, fastHash))
    End Function

#End Region

#Region "Public Interface (Async Versions)"
    'Method-to-enum, output deserialisation, and optional gzip settings (same as above)

    'Pattern 1: Common deserialiser, gzip input/output
    Public Async Function RunScriptsAsync(sql As List(Of String)) As Task(Of List(Of String))
        Return Deserialiser.AsListStr(Await InvokeAsync(EPushUpgrade.RunScripts, EGzip.Both, sql))
    End Function

    'Pattern 2: Custom deserialiser, gzip output
    Public Async Function VerSchConfigAsync(allData As Boolean, ignoreExt As String, Optional recursive As Boolean = True, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False) As Task(Of CVersionSchemaConfig)
        Return AsMyVersion(Await InvokeAsync(EPushUpgrade.VerSchConfig, EGzip.Output, allData, ignoreExt, recursive, chunkSize, fastHash))
    End Function

    'Pattern 3: Common deserialiser, no gzip
    Public Async Function DeleteFileAsync(name As String) As Task(Of Boolean)
        Return Deserialiser.AsBool(Await InvokeAsync(EPushUpgrade.DeleteFile, name))
    End Function

    'Unused: Non-protobuf formats for inputs or outputs
#End Region

#Region "Deserialisation"
    Private Function AsMyVersion(bin As Byte()) As CVersionSchemaConfig
        Return Me.Deserialiser.Deserialise_(Of CVersionSchemaConfig)(bin)
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
    'Protected Shadows Function Invoke(enum_ As EPushUpgrade, gzip As EGzip, formatIn As ESerialisation, ParamArray params As Object()) As Byte() 'rarely used (encryption control)
    '    Return MyBase.Invoke(enum_, gzip, formatIn, params)
    'End Function
    'Protected Shadows Function Invoke(enum_ As EPushUpgrade, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, ParamArray params As Object()) As Byte() 'rarely used (encryption control)
    '    Return MyBase.Invoke(enum_, gzip, formatIn, formatOut, params)
    'End Function
    'Protected Shadows Function Invoke(enum_ As EPushUpgrade, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, encryption As EEncryption, ParamArray params As Object()) As Byte()
    '    Return MyBase.Invoke(enum_, gzip, formatIn, formatOut, encryption, params) 'lowest-level (serialisation option is rarely used)
    'End Function

    'InvokeAsync
    Protected Shadows Async Function InvokeAsync(enum_ As EPushUpgrade, ParamArray params As Object()) As Task(Of Byte()) 'most commonly-used overload
        Return Await MyBase.InvokeAsync(enum_, params)
    End Function
    Protected Shadows Async Function InvokeAsync(enum_ As EPushUpgrade, gzip As EGzip, ParamArray params As Object()) As Task(Of Byte()) 'occasional control of compression at the method-level
        Return Await MyBase.InvokeAsync(enum_, gzip, params)
    End Function
#End Region

End Class

