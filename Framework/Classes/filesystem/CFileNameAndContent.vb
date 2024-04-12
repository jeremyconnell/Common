Imports System.IO
Imports System.Runtime.Serialization

<DataContract, Serializable(), CLSCompliant(True)>
Public Class CFileNameAndContent
    'Data
    <DataMember(Order:=1)> Public Name As String
    <DataMember(Order:=2)> Public Content As Byte()
    <DataMember(Order:=3)> Public LastWriteTimeUtc As DateTime?

    'Pre-Constructor
    Shared Sub New()
        Try
            CProto.Prepare(Of CFileNameAndContent)()
        Catch
        End Try
    End Sub

    'Constructors
    Public Sub New()
    End Sub
    Private Sub New(fileName As String)
        Me.Name = fileName
        Me.Content = Nothing
    End Sub
    Public Sub New(folderPath As String, fileName As String)
        Me.Name = fileName
        If Not fileName.Contains(":") Then fileName = folderPath & fileName
        Me.Content = File.ReadAllBytes(fileName)
        Me.LastWriteTimeUtc = New FileInfo(fileName).LastWriteTimeUtc
    End Sub
    Public Sub New(fileName As String, content As Byte(), lastWriteTime As DateTime)
        Me.Name = fileName
        Me.Content = content
        Me.LastWriteTimeUtc = lastWriteTime
    End Sub
    Public Sub New(fileName As String, content As Byte(), lastWriteTime As DateTime, md5 As Guid)
        Me.New(fileName, content, lastWriteTime)
        m_md5 = md5
    End Sub


    Public Sub SaveToFolder(ByVal folderPath As String)
        If Not IO.Directory.Exists(folderPath) Then IO.Directory.CreateDirectory(folderPath)
        Dim filePath As String = folderPath & Name
        Dim bw As New IO.BinaryWriter(IO.File.OpenWrite(filePath))
        bw.Write(Content)
        bw.Close()
    End Sub


    Public Shared Function UniqueName(ByVal name As String) As String
        Dim extension As String = Path.GetExtension(name)
        Dim baseName As String = name.Substring(0, name.LastIndexOf(extension))

        If 0 = baseName.Length Then Return "_" & name 'trivial case

        Dim len As Integer = baseName.Length
        Dim suffix As String = "(" & 1 & ")" & extension
        If ")" <> baseName.Substring(len - 1, 1) Then Return baseName & suffix

        Dim startAt As Integer = baseName.LastIndexOf("(")
        If -1 = startAt Then Return baseName + suffix

        Dim number As String = baseName.Substring(startAt + 1, len - startAt - 2)
        Try
            Dim nextNumber As Integer = Integer.Parse(number) + 1
            Return baseName.Substring(0, startAt) + "(" + nextNumber.ToString() + ")" + extension
        Catch
            Return baseName + suffix
        End Try
    End Function

    Private m_md5 As Guid = Guid.Empty
    Public ReadOnly Property Md5 As Guid
        Get
            If IsNothing(Content) Then Return Guid.Empty
            If m_md5 = Guid.Empty Then
                m_md5 = CBinary.MD5_(Content)
            End If
            Return m_md5
        End Get
    End Property

    Public Overrides Function ToString() As String
        If IsNothing(Content) Then Return Name
        Return String.Concat(Name, " (", CUtilities.FileSize(Content.Length), ")")
    End Function

End Class
