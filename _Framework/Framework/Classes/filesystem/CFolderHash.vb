Imports System.IO
Imports System.Linq
Imports System.Runtime.Serialization
Imports System.Text
Imports System.Threading.Tasks

<CLSCompliant(True), Serializable(), DataContract>
Public Class CFolderHash : Inherits List(Of CFileHash)

#Region "Constants"
    Public Const FOUR_MB As Integer = 3500000
    Public Const REASSEMBLY As String = ".reassembly"
    Public Const CHUNKFILE As String = ".chunkfile"
    Public Const WWWROOT As String = "inetpub\wwwroot"

    'Common exclude-file patterns for folder-hashing
    Public Const IGNORE_PDB As String = ".xml,.config,.txt,.vssscc,.scc,.pubxml,.refresh,.publishproj,.resources.dll,.dat,.pfx,.cer,.gz,.mdf,.ldf"
    Public Const IGNORE_DLL As String = ".pdb," + IGNORE_PDB
    Public Const IGNORE_WEB As String = "app_data,.txt,.refresh,.xml,.pubxml,.dll.config,.debug.config,.publishproj,packages.config" 'first one is a folder-name, also supported (todo: separate param, or split by .prefix)

    'Array version
    Public Shared ReadOnly IGNORE_DLL_ As String() = CUtilities.StringToListStr(IGNORE_DLL).ToArray()
    Public Shared ReadOnly IGNORE_PDB_ As String() = CUtilities.StringToListStr(IGNORE_PDB).ToArray()
    Public Shared ReadOnly IGNORE_WEB_ As String() = CUtilities.StringToListStr(IGNORE_WEB).ToArray()
#End Region


#Region "Hashing"
    Private m_hash As Guid = Guid.Empty
    Public ReadOnly Property Hash As Guid
        Get
            If m_hash = Guid.Empty AndAlso Me.Count > 0 Then
                SyncLock (Me)
                    If m_hash = Guid.Empty Then
                        Me.Sort()

                        'Include filename in hash (e.g. may be repeated/moved files)
                        Dim sb As New Text.StringBuilder
                        For Each i As CFileHash In Me
                            sb.Append(i.Name.Replace("\\", "\").Replace("\", "/")).Append(i.MD5)
                        Next

                        m_hash = CBinary.MD5_(sb.ToString())
                    End If
                End SyncLock
            End If
            Return m_hash
        End Get
    End Property
    Public ReadOnly Property Base64 As String
        Get
            Return CBinary.ToBase64(Hash)
        End Get
    End Property
    Public ReadOnly Property Base64Trunc As String
        Get
            Return CBinary.ToBase64(Hash, 8)
        End Get
    End Property
#End Region

#Region "Ageing"
    Private m_created As DateTime = DateTime.Now
    Public ReadOnly Property Created As DateTime
        Get
            Return m_created
        End Get
    End Property
    Public ReadOnly Property Age As TimeSpan
        Get
            Return DateTime.Now.Subtract(Created)
        End Get
    End Property
#End Region

#Region "Export/Import"
    Public Function Export() As Dictionary(Of String, Guid)
        Dim d As New Dictionary(Of String, Guid)(Me.Count)
        For Each i As CFileHash In Me
            d.Add(i.Name, i.MD5)
        Next
        Return d
    End Function
    Public Function FileSizes() As Dictionary(Of String, Long)
        Dim d As New Dictionary(Of String, Long)(Me.Count)
        For Each i As CFileHash In Me
            d.Add(i.Name, i.Size)
        Next
        Return d
    End Function
    Private Sub Import(d As Dictionary(Of String, Guid))
        Me.Clear()
        For Each i As String In d.Keys
            Me.Add(New CFileHash(i, d(i)))
        Next
        m_hash = Guid.Empty
    End Sub
#End Region

#Region "Constructor"
    'STATIC
    Shared Sub New()
        Try
            CProto.Prepare(Of CFolderHash)()
        Catch
        End Try
    End Sub

    Public Sub New()
        MyBase.New
    End Sub

    'Convert params

    Public Sub New(diff As CFilesList)
        MyBase.New(diff.Count)

        For Each i As CFileNameAndContent In diff
            If IsNothing(i.Content) Then
                Me.Add(New CFileHash(i.Name, i.Md5, 0))
            Else
                Me.Add(New CFileHash(i.Name, i.Md5, i.Content.Length))
            End If
        Next
    End Sub
    'Folder

    Public Sub New(ByVal folderPath As String, ByVal recursive As Boolean, ByVal exceptExtensionsAsCsv As String, Optional maxSize As Integer = FOUR_MB, Optional fastHash As Boolean = False)
        Me.New(folderPath, CUtilities.StringToListStr(exceptExtensionsAsCsv).ToArray(), recursive, maxSize, fastHash)
    End Sub
    Public Sub New(ByVal folderPath As String, Optional ByVal exceptExtensions As String() = Nothing, Optional ByVal recursive As Boolean = True, Optional maxSize As Integer = FOUR_MB, Optional fastHash As Boolean = False)
        Me.New(GetFiles(folderPath, recursive, exceptExtensions), exceptExtensions, folderPath, maxSize, fastHash)
    End Sub

    'Files
    Public Sub New(ByVal files As String(), Optional ByVal exceptExtensions As String() = Nothing, Optional ByVal baseFolder As String = Nothing, Optional maxSize As Integer = FOUR_MB, Optional fastHash As Boolean = False)
        MyBase.New(files.Length)

        'Base path
        If Not IsNothing(baseFolder) Then
            If Not baseFolder.EndsWith("/") AndAlso Not baseFolder.EndsWith("\") Then baseFolder &= "\"
            baseFolder = baseFolder.Replace("\\", "\")
        End If

        'Exclusions
        If IsNothing(exceptExtensions) Then exceptExtensions = IGNORE_PDB_
        Dim excludeFolderNames As New List(Of String)
        Dim excludeExt As New List(Of String)(exceptExtensions)
        For Each i As String In exceptExtensions
            If Not i.StartsWith(".") AndAlso Not i.Contains(".") Then
                excludeFolderNames.Add(i)
                excludeExt.Remove(i)
            End If
        Next
        exceptExtensions = excludeExt.ToArray()
        excludeExt.Add(CHUNKFILE)
        excludeExt.Add(REASSEMBLY)


        'Hash the files
        For Each i As String In files
            i = i.ToLower

            'Exclusions
            If ContainsFolder(TrimName(i, i, baseFolder), excludeFolderNames) Then Continue For
            If i.Contains("vshost.") Then Continue For
            If EndsWith(i, exceptExtensions) Then Continue For

            'Single file, as list
            Dim fh As New CFileHash(i, maxSize, fastHash)
            Dim ff As New List(Of CFileHash)({fh})


            'Trim/add
            For Each f As CFileHash In ff
                TrimName(f, i, baseFolder)
                Add(f)
            Next
        Next
    End Sub

    Public Sub New(importFrom As Dictionary(Of String, Guid))
        MyBase.New(importFrom.Count)
        Me.Import(importFrom)
    End Sub
    Public Sub New(importFrom As List(Of CFileHash))
        MyBase.New(importFrom)
    End Sub


    Public Function TotalFor(list As List(Of String)) As Long
        Dim t As Long = 0
        For Each i As CFileHash In Me
            If list.Contains(i.Name) Then
                t += i.Size
                If Not IsNothing(i.Chunks) Then
                    t += i.Chunks.Sum(
                        Function(f As CFileHash) As Long
                            Return f.Size
                        End Function
                        )
                End If
            End If
        Next
        Return t
    End Function
    Public ReadOnly Property Total As Long
        Get
            Dim t As Long = 0
            For Each i As CFileHash In Me
                t += i.Size
                If Not IsNothing(i.Chunks) Then
                    t += i.Chunks.Sum(
                        Function(f As CFileHash) As Long
                            Return f.Size
                        End Function
                        )
                End If
            Next
            Return t
        End Get
    End Property


    Private Shared Sub ExpandChunks(fileName As String, folderPath As String, ref As CFileHash, old As CFileHash, diff As CFilesList, chunkSize As Long, maxSize As Long, fastHash As Boolean)
        If fileName.EndsWith(CHUNKFILE) OrElse fileName.EndsWith(REASSEMBLY) Then
            Dim ss As New List(Of String)(fileName.Split(CChar(".")))
            ss.RemoveAt(ss.Count - 1)  'ext
            ss.RemoveAt(ss.Count - 1) 'total
            ss.RemoveAt(ss.Count - 1) 'of
            ss.RemoveAt(ss.Count - 1) 'index
            fileName = CUtilities.ListToString(ss, ".")
        End If

        Dim filePath As String = folderPath & fileName
        If Not File.Exists(filePath) Then Exit Sub

        'Splitting params
        Dim fi As New FileInfo(filePath)
        Dim size As Long = fi.Length
        Dim remain As Integer = CInt(size Mod chunkSize)
        Dim total As Integer = CInt(size / chunkSize)
        If remain > 0 Then total += 1

        'Diff on the chunks
        Dim takeChunks As New List(Of Integer)(total)
        For i As Integer = 0 To total
            If IsNothing(old) OrElse IsNothing(old.Chunks) OrElse i >= old.Chunks.Count Then
                takeChunks.Add(i)
            ElseIf old.Chunks(i).MD5 <> ref.Chunks(i).MD5 Then
                takeChunks.Add(i)
            End If
        Next

        Dim gg As New List(Of Guid)
        Using fs As FileStream = File.OpenRead(filePath)
            Using sr As New BinaryReader(fs)
                For i As Integer = 0 To total
                    'Read chunk
                    Dim bb As Byte() = sr.ReadBytes(CInt(chunkSize))

                    'Take ones that differ
                    If Not takeChunks.Contains(i) Then Continue For

                    'Hash it
                    Dim md5 As Guid = Guid.Empty
                    If fastHash Then md5 = ChunkFastHash(fi, i, total) Else md5 = CBinary.MD5_(bb)
                    gg.Add(md5)


                    'Memory limit
                    If diff.Total > maxSize Then
                        sr.Close()
                        Exit Sub
                    End If
                    diff.Add(New CFileNameAndContent(ChunkPath(fileName, i, total), bb, fi.LastWriteTimeUtc, md5))
                Next
                sr.Close()
            End Using
        End Using

        If takeChunks.Contains(total) Then
            Dim manifest As Byte() = CProto.Serialise(gg)
            Dim manifMd5 As Guid = CBinary.MD5_(manifest)
            diff.Add(New CFileNameAndContent(ChunkPath(fileName, total, total), manifest, fi.LastWriteTimeUtc, manifMd5))
        End If
    End Sub

    Friend Shared Function ChunkFastHash(fi As FileInfo, i As Integer, total As Integer) As Guid
        Return ChunkFastHash(fi.FullName, fi.Length, fi.LastWriteTimeUtc, i, total)
    End Function
    Friend Shared Function ChunkFastHash(filePath As String, size As Long, lastWriteUtc As Date, i As Integer, total As Integer) As Guid
        Return CBinary.MD5_(filePath, size.ToString(), lastWriteUtc.ToString(), i.ToString(), total.ToString())
    End Function
    Public Shared Function Reassemble(filePath As String, total As Integer, md5List As List(Of Guid), chunkSize As Integer, lastWriteTime As DateTime, fastHash As Boolean) As String
        Dim bits As New List(Of Guid)(total)
        Dim tempFilePath As String = filePath & ".tmp"
        Dim chunksToDelete As New List(Of String)


        Dim sb As New StringBuilder
        Dim dir As String = Path.GetDirectoryName(filePath)
        If Not Directory.Exists(dir) Then
            Try
                Directory.CreateDirectory(dir)
                sb.Append("Created Dir: ").AppendLine(dir)
            Catch ex As Exception
                Throw New Exception("Failed to create dir: " & dir, ex)
            End Try
        End If

        If File.Exists(tempFilePath) Then
            File.Delete(tempFilePath)
            sb.Append("Deleted Tmp File: ").AppendLine(tempFilePath)
        End If

        Dim size As Long = 0
        Dim lastWrite As DateTime = DateTime.Now

        Using writeTo As FileStream = File.OpenWrite(tempFilePath)
            sb.Append("Creating: ").AppendLine(tempFilePath)

            If File.Exists(filePath) Then
                Dim fi As New FileInfo(filePath)
                size = fi.Length
                lastWrite = fi.LastWriteTimeUtc

                sb.Append("Merging with: ").AppendLine(filePath)
                'Merge uploaded chunks with existing
                Using fs As FileStream = File.OpenRead(filePath)
                    Using sr As New BinaryReader(fs)
                        For i As Integer = 0 To total - 1
                            'Chunk 
                            Dim bb As Byte() = sr.ReadBytes(chunkSize)

                            'Hash
                            Dim md5 As Guid = ChunkFastHash(filePath, size, lastWrite, i, total)

                            'uploaded chunk (takes precidence)
                            Dim p As String = ChunkPath(filePath, i, total)
                            If File.Exists(p) Then
                                bb = File.ReadAllBytes(p) '(actual file)
                                chunksToDelete.Add(p)
                                If fastHash Then md5 = ChunkFastHash(filePath, size, lastWriteTime, i, total)
                                sb.Append("Read from Chunk File: ").AppendLine(CUtilities.FileNameAndSize(p, bb.Length))
                            Else
                                sb.Append("Read from Old File: ").AppendLine(CUtilities.FileNameAndSize(p, bb.Length))
                            End If

                            writeTo.Write(bb, 0, bb.Length)

                            If Not fastHash Then md5 = CBinary.MD5_(bb)
                            bits.Add(md5)

                            If md5List.Count <= i Then
                                sb.Append("#").Append(i).Append(": no md5 in list, only has ").Append(md5List.Count).Append(", total=").Append(total)
                            Else
                                If md5List(i) = md5 Then sb.Append("Guid OK: #").AppendLine(i.ToString) Else sb.Append("GUID FAILED: ").Append(CBinary.ToBase64(md5, 6)).Append(" <> ").AppendLine(CBinary.ToBase64(md5List(i), 6))
                            End If
                        Next
                        sr.Close()
                    End Using
                End Using

            Else
                sb.Append("No file to Merge with: ").AppendLine(filePath)
                For i As Integer = 0 To total - 1
                    Dim p As String = ChunkPath(filePath, i, total)
                    If Not File.Exists(p) Then
                        chunksToDelete.Clear()
                        sb.Append("Chunk Not Found: ").AppendLine(p)
                        Exit For
                    End If
                    chunksToDelete.Add(p)

                    Dim ff As Byte() = File.ReadAllBytes(p) '(actual file)
                    writeTo.Write(ff, 0, ff.Length)

                    Dim md5 As Guid = ChunkFastHash(filePath, size, lastWrite, i, total)
                    If Not fastHash Then md5 = CBinary.MD5_(ff)
                    bits.Add(md5)
                    sb.Append("Read from Chunk File: ").AppendLine(CUtilities.FileNameAndSize(p, ff.Length))
                    sb.Append("Wrote to: ").AppendLine(CUtilities.FileNameAndSize(tempFilePath, total))

                    If md5List.Count <= i Then
                        sb.Append("#").Append(i).Append(": no md5 in list, only has ").Append(md5List.Count).Append(", total=").Append(total)
                    Else
                        If md5List(i) = md5 Then
                            sb.Append("Guid OK: #").AppendLine(i.ToString)
                        Else
                            sb.Append("GUID FAILED: ").Append(CBinary.ToBase64(md5, 6)).Append(" <> ").AppendLine(CBinary.ToBase64(md5List(i), 6))
                        End If
                    End If
                Next
            End If
            writeTo.Close()
        End Using

        'Delete temp file if not a match
        If File.Exists(filePath) Then 'TODO: send filesize, so can reassemble
            If CBinary.MD5_(md5List) <> CBinary.MD5_(bits) Then
                If md5List.Count = bits.Count + 1 Then
                    md5List.RemoveAt(md5List.Count - 1)
                    If CBinary.MD5_(md5List) <> CBinary.MD5_(bits) Then
                        File.Delete(tempFilePath)
                        sb.Append("MD5 doesnt match: ") _
                    .Append(bits.Count).Append(" bits, ") _
                    .Append(md5List).Append(" ref bits, ") _
                    .Append(CBinary.ToBase64(CBinary.MD5_(bits), 6)) _
                    .Append(", should be: ") _
                    .AppendLine(CBinary.ToBase64(CBinary.MD5_(md5List), 6))
                        Return sb.ToString
                    End If
                End If
            End If

            File.Delete(filePath)
        End If

        'Replace file with temp file, clean up chunks
        File.Move(tempFilePath, filePath)
        File.SetLastWriteTimeUtc(filePath, lastWriteTime)
        For Each i As String In chunksToDelete
            File.Delete(i)
        Next
        Return String.Empty
    End Function

    Public Shared Function ChunkPath(filePath As String, index As Integer, total As Integer) As String
        If filePath.Contains(CHUNKFILE) OrElse filePath.Contains(REASSEMBLY) Then Throw New Exception(filePath)
        If index < total Then
            Return String.Concat(filePath, ".", index, ".of.", total, CHUNKFILE)
        Else
            Return String.Concat(filePath, ".", index, ".of.", total, REASSEMBLY)
        End If
    End Function

    Private Shared Sub TrimName(f As CFileHash, filePath As String, root As String)
        f.Name = TrimName(f.Name, filePath, root)
        If Not IsNothing(f.Chunks) Then
            For Each i As CFileHash In f.Chunks
                i.Name = TrimName(i.Name, filePath, root)
            Next
        End If
    End Sub
    Private Shared Function TrimName(i As String, filePath As String, root As String) As String
        If IsNothing(root) Then Return i
        Dim dir As String = String.Concat(Path.GetDirectoryName(filePath), "\")
        If dir.Length > root.Length Then
            Dim basePath As String = dir.Substring(root.Length)
            If basePath.StartsWith("\") Then basePath.Substring(1)
            Return String.Concat(basePath, Path.GetFileName(i))
        ElseIf dir.Length = root.Length Then
            Return Path.GetFileName(i)
        Else
            Return i
        End If
    End Function
    Private Shared Function EndsWith(s As String, ss As String()) As Boolean
        For Each i As String In ss
            If s.EndsWith(i) Then Return True
        Next
        Return False
    End Function
    Private Shared Function ContainsFolder(s As String, ss As List(Of String)) As Boolean
        For Each i As String In ss
            If s.Contains(String.Concat(i, "\")) Then Return True
        Next
        Return False
    End Function
    Private Shared Function GetFiles(ByVal folderPath As String, ByVal recursive As Boolean, excludes As String()) As String()
        If Not IO.Directory.Exists(folderPath) Then Return New String() {} 'Suppress exceptions

        Dim list As New List(Of String)
        list.AddRange(IO.Directory.GetFiles(folderPath))
        If Not recursive Then Return list.ToArray

        For Each i As String In IO.Directory.GetDirectories(folderPath)
            If Not IsNothing(excludes) Then
                Dim folderName As String = i.Substring(folderPath.Length).Replace("\", "")
                For Each j As String In excludes
                    If j.ToLower = folderName.ToLower() Then Continue For
                Next
            End If

            list.AddRange(GetFiles(i, True, excludes))
        Next
        Return list.ToArray
    End Function
#End Region

#Region "Index On Name"
    Public Function Has(key As String) As Boolean
        Return Dict.ContainsKey(key.ToLower)
    End Function
    Public Function GetFile(key As String) As CFileHash
        Dim c As CFileHash = Nothing
        Dict.TryGetValue(key.ToLower, c)
        Return c
    End Function
    <NonSerialized()> Private m_dict As Dictionary(Of String, CFileHash)
    Public ReadOnly Property Dict As Dictionary(Of String, CFileHash)
        Get
            If IsNothing(m_dict) Then ' OrElse m_dict.Count <> Me.Count Then
                m_dict = New Dictionary(Of String, CFileHash)(Me.Count)
                For Each i As CFileHash In Me
                    m_dict(i.Name.ToLower) = i
                    If Not IsNothing(i.Chunks) Then
                        For Each j As CFileHash In i.Chunks
                            m_dict(j.Name.ToLower) = j
                        Next
                    End If
                Next
            End If
            Return m_dict
        End Get
    End Property


    <NonSerialized()> Private m_dictByHash As Dictionary(Of Guid, CFileHash)
    Public ReadOnly Property DictByHash As Dictionary(Of Guid, CFileHash)
        Get
            If IsNothing(m_dictByHash) OrElse m_dictByHash.Count <> Me.Count Then
                m_dictByHash = New Dictionary(Of Guid, CFileHash)(Me.Count)
                For Each i As CFileHash In Me
                    m_dictByHash(i.MD5) = i
                Next
            End If
            Return m_dictByHash
        End Get
    End Property


    Public ReadOnly Property Names As List(Of String)
        Get
            Dim list As New List(Of String)(Dict.Keys)
            list.Sort()
            Return list
        End Get
    End Property
#End Region

#Region "Diff Logic"
    Public ReadOnly Property Hashes As List(Of Guid)
        Get
            Dim list As New List(Of Guid)(Me.Count)
            For Each i As CFileHash In Me
                list.Add(i.MD5)
            Next
            Return list
        End Get
    End Property

    Public Function DiffOnHash(other As CFolderHash) As CDiff_Guid
        Return CDiffLogic.Diff(Me.Hashes, other.Hashes)
    End Function
    Public Function DiffOnName(other As CFolderHash) As CDiff_String
        Return CDiffLogic.Diff(Me.Names, other.Names)
    End Function


    Public Shared Function FromPath(folderPath As String, Optional exceptExtensions As String() = Nothing, Optional recursive As Boolean = True) As CFilesList
        Dim temp As New CFolderHash(folderPath, exceptExtensions, recursive)
        Return temp.ResolveDifferences(New CFolderHash, folderPath)
    End Function

    Public Function ResolveDifferences(old As CFolderHash, folderPath As String, Optional maxSize As Long = 5 * FOUR_MB, Optional chunkSize As Integer = FOUR_MB, Optional fastHash As Boolean = False) As CFilesList
        If Not folderPath.EndsWith("/") AndAlso Not folderPath.EndsWith("\") Then folderPath &= "\"

        'Compute diff on hash
        Dim deletes As CFolderHash = Me.DetectNew(old)
        Dim different As CFolderHash = Me.DetectDifferent(old)
        Dim adds As CFolderHash = Me.DetectMissing(old)

        'Sort by soze
        adds.Sort(New Comparison(Of CFileHash)(
                 Function(x As CFileHash, y As CFileHash) As Integer
                     Return x.Size.CompareTo(y.Size)
                 End Function)
                 )

        different.Sort(New Comparison(Of CFileHash)(
                 Function(x As CFileHash, y As CFileHash) As Integer
                     Return x.Size.CompareTo(y.Size)
                 End Function)
                 )


        'Missing 
        Dim diff As New CFilesList
        For Each i As CFileHash In deletes
            If i.Name.EndsWith(CHUNKFILE) Then Continue For
            If i.Name.EndsWith(REASSEMBLY) Then Continue For
            diff.Add(Delete(i.Name))
        Next


        Dim dictOfChunks As New Dictionary(Of String, CFileNameAndContent)


        'Differences
        For Each i As CFileHash In different
            Dim j As CFileHash = old.GetFile(i.Name)
            If i.Name.EndsWith(CHUNKFILE) Or i.Name.EndsWith(REASSEMBLY) Then
                Dim chunk As CFileNameAndContent = Nothing
                If Not dictOfChunks.TryGetValue(i.Name, chunk) Then
                    Dim chunks As New CFilesList
                    ExpandChunks(i.Name, folderPath, i, j, chunks, chunkSize, maxSize, fastHash)
                    For Each k As CFileNameAndContent In chunks
                        dictOfChunks(k.Name) = k
                    Next
                End If
                diff.Add(dictOfChunks(i.Name))
                dictOfChunks.Remove(i.Name)
            ElseIf Not IsNothing(i.Chunks) Then
                Dim ii As New CFolderHash(i.Chunks)
                Dim jj As New CFolderHash()
                If Not IsNothing(j.Chunks) Then
                    jj = New CFolderHash(j.Chunks)
                End If
                diff.AddRange(ii.ResolveDifferences(jj, folderPath, maxSize, chunkSize, fastHash))
            ElseIf i.Size > chunkSize Then
                CFolderHash.ExpandChunks(i.Name, folderPath, i, j, diff, chunkSize, maxSize, fastHash)
            Else
                Try
                    diff.Add(AddOrUpdate(i.Name, folderPath))
                Catch
                End Try
            End If
            If diff.Total > maxSize Then Return diff
        Next

        'New
        For Each i As CFileHash In adds
            If i.Name.EndsWith(CHUNKFILE) Or i.Name.EndsWith(REASSEMBLY) Then
                Dim chunk As CFileNameAndContent = Nothing
                If Not dictOfChunks.TryGetValue(i.Name, chunk) Then
                    Dim chunks As New CFilesList
                    ExpandChunks(i.Name, folderPath, i, Nothing, chunks, chunkSize, maxSize, fastHash)
                    For Each k As CFileNameAndContent In chunks
                        dictOfChunks(k.Name) = k
                    Next
                End If
                diff.Add(dictOfChunks(i.Name))
                dictOfChunks.Remove(i.Name)
            ElseIf Not IsNothing(i.Chunks) Then
                Dim ii As New CFolderHash(i.Chunks)
                diff.AddRange(ii.ResolveDifferences(New CFolderHash(), folderPath, maxSize, chunkSize, fastHash))
            ElseIf i.Size > chunkSize Then
                CFolderHash.ExpandChunks(i.Name, folderPath, i, Nothing, diff, chunkSize, maxSize, fastHash)
            Else
                Try
                    diff.Add(AddOrUpdate(i.Name, folderPath))
                Catch
                End Try
            End If
            If diff.Total > maxSize Then Return diff
        Next



        'Dim byHash As CDiff_Guid = DiffOnHash(old)

        ''By Hash - new/changed
        'For Each i As Guid In byHash.SourceOnly
        '    If Not diff.Has(i) Then
        '        Dim j As CFileHash = Nothing
        '        If Me.DictByHash.TryGetValue(i, j) Then
        '            diff.Add(AddOrUpdate(j.Name, folderPath))
        '        End If
        '    End If
        'Next

        'By Hash - Missing 
        'For Each i As Guid In byHash.TargetOnly
        '    If Not diff.Has(i) Then
        '        If old.DictByHash.ContainsKey(i) Then
        '            diff.Add(Delete(old.DictByHash(i).Name))
        '        End If
        '    End If
        'Next


        Return diff
    End Function
    Private Function AddOrUpdate(name As String, folderPath As String, Optional attempts As Integer = 3) As CFileNameAndContent
        If attempts <= 1 Then
            Return New CFileNameAndContent(folderPath, name)
        End If

        Try
            Return New CFileNameAndContent(folderPath, name)
        Catch
            Threading.Thread.Sleep(New Random().Next(0, 2000))
            Return AddOrUpdate(name, folderPath, attempts - 1)
        End Try
    End Function
    Private Function Delete(name As String) As CFileNameAndContent
        Dim fnc As New CFileNameAndContent
        fnc.Name = name
        fnc.Content = Nothing
        Return fnc
    End Function
#End Region

#Region "Use Diff results to apply an upgrade"
    Public Shared Sub ApplyChanges(differences As List(Of CFileNameAndContent), folderPath As String, Optional chunkSize As Integer = FOUR_MB, Optional fastHash As Boolean = True)
        If Not folderPath.EndsWith("/") AndAlso Not folderPath.EndsWith("\") Then folderPath &= "\"

        For Each i As CFileNameAndContent In differences
            Dim filePath As String = folderPath & i.Name
            If i.Name.Contains(":") Then filePath = i.Name
            If i.Name.EndsWith(REASSEMBLY) Then Continue For

            'Cautious delete
            Dim r As New Random
            Try
                If IO.File.Exists(filePath) Then IO.File.Delete(filePath)
            Catch
                Threading.Thread.Sleep(r.Next(500, 2000))

                If IO.File.Exists(filePath) Then
                    Try
                        IO.File.SetAttributes(filePath, IO.FileAttributes.Normal)
                        IO.File.Delete(filePath)
                    Catch
                        Threading.Thread.Sleep(r.Next(500, 2000))
                        Try
                            IO.File.SetAttributes(filePath, IO.FileAttributes.Normal)
                            IO.File.Delete(filePath)
                        Catch
                        End Try
                    End Try
                End If
            End Try

            'Create dir
            Dim subFolder As String = IO.Path.GetDirectoryName(filePath)
            If Not IO.Directory.Exists(subFolder) Then IO.Directory.CreateDirectory(subFolder)

            'Write
            If Not IsNothing(i.Content) Then
                Try
                    File.WriteAllBytes(filePath, i.Content)
                    If i.LastWriteTimeUtc.HasValue Then
                        File.SetLastWriteTimeUtc(filePath, i.LastWriteTimeUtc.Value)
                    End If
                Catch
                    Threading.Thread.Sleep(r.Next(500, 2000))
                    File.WriteAllBytes(filePath, i.Content)
                    If i.LastWriteTimeUtc.HasValue Then
                        File.SetLastWriteTimeUtc(filePath, i.LastWriteTimeUtc.Value)
                    End If
                End Try
            End If
        Next

        For Each i As CFileNameAndContent In differences
            If i.Name.EndsWith(REASSEMBLY) Then
                If IsNothing(i.Content) OrElse 0 = i.Content.Length Then Continue For

                Dim md5s As List(Of Guid) = CProto.Deserialise(Of List(Of Guid))(i.Content)

                Dim ss As New List(Of String)(i.Name.Split(CChar(".")))
                ss.RemoveAt(ss.Count - 1)
                Dim total As Integer = Integer.Parse(ss(ss.Count - 1))
                ss.RemoveAt(ss.Count - 1) 'total
                ss.RemoveAt(ss.Count - 1) 'of
                ss.RemoveAt(ss.Count - 1) 'index

                Dim filePath As String = folderPath & CUtilities.ListToString(ss, ".")
                Reassemble(filePath, total, md5s, chunkSize, i.LastWriteTimeUtc.Value, fastHash)
            End If
        Next

    End Sub

    'Special case: self-upgrading an exe
    Public Function ApplyDeletesOnly(differences As List(Of CFileNameAndContent), folderPath As String) As Integer
        Return ApplyDeletesOnly(differences, folderPath, False)
    End Function
    Public Function ApplyDeletesOnly(differences As List(Of CFileNameAndContent), folderPath As String, deletesAsAnEmptyFileWithExtension As Boolean, Optional extensionToMarkForDelete As String = ".delete") As Integer
        If Not folderPath.EndsWith("/") AndAlso Not folderPath.EndsWith("\") Then folderPath &= "\"

        Dim count As Integer = 0
        For Each i As CFileNameAndContent In differences
            If Not IsNothing(i.Content) Then Continue For
            count += 1

            Dim filePath As String = folderPath & i.Name

            If i.Name.EndsWith(CHUNKFILE) OrElse i.Name.EndsWith(REASSEMBLY) Then Continue For

            If deletesAsAnEmptyFileWithExtension Then
                'Defer the delete to upgrade.exe (so current exe can exit first)
                IO.File.WriteAllText(filePath & extensionToMarkForDelete, String.Empty)
            Else
                Try
                    'Immediate delete (default behaviour)
                    If IO.File.Exists(filePath) Then IO.File.Delete(filePath)
                Catch
                    'Deferred delete
                    i.Name &= ".delete"
                    i.Content = New Byte() {}
                End Try
            End If
        Next
        Return count
    End Function
    Public Function ApplyAddOrUpdatesOnly(differences As List(Of CFileNameAndContent), folderPath As String) As Integer
        If Not folderPath.EndsWith("/") AndAlso Not folderPath.EndsWith("\") Then folderPath &= "\"

        Dim count As Integer = 0
        For Each i As CFileNameAndContent In differences
            If IsNothing(i.Content) Then Continue For 'delete
            count += 1

            'Add or Upate
            Dim filePath As String = folderPath & i.Name
            If IO.File.Exists(filePath) Then IO.File.Delete(filePath)
            Dim actualFolder As String = IO.Path.GetDirectoryName(filePath)
            If Not IO.Directory.Exists(actualFolder) Then IO.Directory.CreateDirectory(actualFolder)
            If Not IsNothing(i.Content) Then
                File.WriteAllBytes(filePath, i.Content)
                If i.LastWriteTimeUtc.HasValue Then
                    File.SetLastWriteTimeUtc(filePath, i.LastWriteTimeUtc.Value)
                End If
            End If
        Next
        Return count
    End Function
#End Region


#Region "Checks"
    Public Function DetectNew(list As CFolderHash) As CFolderHash
        Dim temp As New CFolderHash
        For Each i As CFileHash In list
            If Not Me.Has(i.Name) Then temp.Add(i)
        Next
        Return temp
    End Function
    Public Function DetectMissing(list As CFolderHash) As CFolderHash
        Return list.DetectNew(Me)
    End Function
    Public Function DetectCommon(list As CFolderHash) As CFolderHash
        Dim temp As New CFolderHash
        For Each i As CFileHash In list
            If Me.Has(i.Name) Then temp.Add(i)
        Next
        Return temp
    End Function
    Public Function DetectDifferent(list As CFolderHash) As CFolderHash
        Dim temp As New CFolderHash
        For Each i As CFileHash In list
            Dim j As CFileHash = Me.GetFile(i.Name)
            If IsNothing(j) Then Continue For
            If i.MD5 <> j.MD5 Then temp.Add(i)
        Next
        Return temp
    End Function
#End Region


    Public Overrides Function GetHashCode() As Integer
        Return ToString.GetHashCode()
    End Function
    Public Overrides Function ToString() As String
        Dim sb As New StringBuilder()
        For Each j As CFileHash In Me
            If Guid.Empty.Equals(j.MD5) Then
                sb.AppendLine("Delete: " + j.Name)
            Else
                sb.AppendLine("Update: " + CUtilities.FileNameAndSize(j.Name, j.Size))
            End If
        Next
        Return sb.ToString()
    End Function


End Class
