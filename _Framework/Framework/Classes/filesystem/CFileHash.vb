Imports System.IO
Imports System.Runtime.Serialization
Imports System.Security.Cryptography

<CLSCompliant(True), Serializable(), DataContract>
Public Class CFileHash : Implements IComparable(Of CFileHash)

    <DataMember(Order:=1)> Public Name As String
    <DataMember(Order:=2)> Public MD5 As Guid
    <DataMember(Order:=3)> Public Size As Long
    <DataMember(Order:=4)> Public Chunks As List(Of CFileHash)

    'Serialisation
    Shared Sub New()
        Try
            CProto.Prepare(Of CFileHash)()
        Catch
        End Try
    End Sub
    Protected Sub New()
    End Sub

    'Constructors
    Public Sub New(name As String, md5 As Guid, Optional fileSize As Long = -1, Optional chunks As List(Of CFileHash) = Nothing)
        Me.Name = name.ToLower.Replace("\", "/")
        Me.MD5 = md5
        Me.Size = fileSize
        Me.Chunks = chunks
    End Sub
    Public Sub New(filePath As String, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False)
        Me.Name = filePath

        'Empty
        Me.Size = 0
        Me.MD5 = Guid.Empty

        'Cheating
        If fastHash Then
            If Not File.Exists(filePath) Then Exit Sub

            Dim fi As New FileInfo(filePath)
            Me.Size = fi.Length
            Me.MD5 = CBinary.MD5_(Path.GetFileName(filePath), fi.LastWriteTimeUtc.ToString(), fi.Length.ToString())
            If Me.Size > chunkSize Then
                Me.Chunks = CFileHash.HashAsChunks(fi.FullName, fi.Length, fi.LastWriteTimeUtc, chunkSize, fastHash)
            End If
            Exit Sub
        End If


        'Cached (10k+)
        If Size > 10000 Then
            'Created by cache
            Dim fh As CFileHash = CBinary.MD5_Cached(filePath, chunkSize)

            'Copy from cache
            If Not IsNothing(fh) Then
                Me.Size = fh.Size
                Me.MD5 = fh.MD5
                If Not IsNothing(fh.Chunks) Then
                    Me.Chunks = New List(Of CFileHash)(fh.Chunks)
                End If
            End If
        Else
            'Non-cached (under 10k)
            If Not File.Exists(filePath) Then Exit Sub

            'Read  size, Exit if empty
            Me.Size = New FileInfo(filePath).Length
            If 0 = Size Then Exit Sub

            'Stream it
            MD5 = CBinary.MD5_FromFile(filePath)
        End If
    End Sub


    Friend Shared Function HashAsChunks(fi As FileInfo, chunkSize As Integer, fastHash As Boolean) As List(Of CFileHash)
        Return HashAsChunks(fi.FullName, fi.Length, fi.LastWriteTimeUtc, chunkSize, fastHash)
    End Function

    Friend Shared Function HashAsChunks(filePath As String, size As Long, lastWrite As DateTime, chunkSize As Integer, fastHash As Boolean) As List(Of CFileHash)
        'Chunk params
        Dim remain As Integer = CInt(size Mod chunkSize)
        Dim total As Integer = CInt(size / chunkSize)
        If remain > 0 Then total += 1

        'Chunks, as md5s
        Dim chunks As New List(Of CFileHash)(total)
        Dim originalMd5s As New List(Of Guid)(total)
        Dim mergedMd5s As New List(Of Guid)(total)

        'Hash the individual chunks
        Dim sp As New MD5CryptoServiceProvider()
        Using fs As FileStream = File.OpenRead(filePath)
            Using sr As New BinaryReader(fs)
                For i As Integer = 0 To total - 1
                    'Chunk gets hashed
                    Dim bb As Byte() = sr.ReadBytes(CInt(chunkSize))

                    Dim md5 As Guid = Guid.Empty
                    If fastHash Then
                        md5 = CFolderHash.ChunkFastHash(filePath, size, lastWrite, i, total)
                    Else
                        md5 = New Guid(sp.ComputeHash(bb))
                    End If
                    originalMd5s.Add(md5)

                    'Named (virtual file)
                    Dim chunkPath As String = CFolderHash.ChunkPath(filePath, i, total)
                    chunks.Add(New CFileHash(chunkPath, md5, bb.Length))

                    'Merged set (target-side)
                    If File.Exists(chunkPath) Then
                        If fastHash Then
                            md5 = CFolderHash.ChunkFastHash(New FileInfo(chunkPath), i, total)
                        Else
                            md5 = CBinary.MD5_Cached(chunkPath, chunkSize).MD5
                        End If
                    End If
                Next
                sr.Close()
            End Using
        End Using

        'Final chunk: manifest of original
        Dim manifestPath As String = CFolderHash.ChunkPath(filePath, total, total)
        Dim manifestBin As Byte() = CProto.Serialise(originalMd5s)
        Dim manifestMd5 As Guid = CBinary.MD5_(manifestBin)
        Dim manifestLen As Integer = manifestBin.Length
        chunks.Add(New CFileHash(manifestPath, manifestMd5, manifestLen))

        Return chunks
    End Function

    'Display
    Public Overrides Function ToString() As String
        Dim base64 As String = CBinary.ToBase64(MD5, 8)

        Dim s As String = CUtilities.FileNameAndSize(Name, Size)
        If -1 <> Size Then s = Name

        Return String.Concat(s, " [", base64, "]")
    End Function
    Public Function CompareTo(other As CFileHash) As Integer Implements IComparable(Of CFileHash).CompareTo
        Return Me.Name.CompareTo(other.Name)
    End Function
End Class
