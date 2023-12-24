Imports System.IO
Imports System.Runtime.Serialization
Imports System.Text
Imports System.Xml

<DataContract>
Public Class CVerConfRunning
    'Data
    <DataMember(Order:=1)> Public FolderName As String

    <DataMember(Order:=2)> Public Version As CFolderHash
    <DataMember(Order:=3)> Public ConfigFile As String
    <DataMember(Order:=4)> Public Running As CProcess

    <DataMember(Order:=6)> Public AppSettings As Dictionary(Of String, String)
    <DataMember(Order:=7)> Public ConnectionStrings As Dictionary(Of String, String)

    <DataMember(Order:=8)> Public ListOfFiles As List(Of String)

    'Pre-Cons
    Shared Sub New()
        CProto.Prepare(Of CVerConfRunning)()
    End Sub

    Public Sub New()
    End Sub
    Public Sub New(folderName As String, server As IPushUpgrade, Optional filter As String = CFolderHash.IGNORE_WEB, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False)
        Me.FolderName = folderName

        If Me.FolderName = CFolderHash.WWWROOT Then
            'legacy self-only methods
            Me.Version = server.RequestHash(filter, True, chunkSize, fastHash)
            Me.ConfigFile = server.RequestWebConfig()
            ReadConfig()
        Else
            'data folder
            Me.Version = New CFolderHash() 'dont hash the data  server.ExeVersion(folderName, filter, True, chunkSize, fastHash)
            Try
                Me.ListOfFiles = server.ListDir(folderName)
            Catch ex As Exception
                Me.ListOfFiles = New List(Of String)(1)
                Me.ListOfFiles.Add(ex.Message)
            End Try
            Me.Version = New CFolderHash(folderName, False, Nothing, chunkSize, True)
        End If
    End Sub
    Public Sub New(folderPath As String, exeName As String, server As IPushUpgrade, Optional filter As String = CFolderHash.IGNORE_DLL, Optional chunkSize As Integer = CFolderHash.FOUR_MB, Optional fastHash As Boolean = False)
        Dim exePath As String = String.Concat(folderPath, "\", exeName, ".exe")
        Dim configPath As String = String.Concat(exePath, ".config")


        Me.FolderName = Path.GetFileName(folderPath)
        Me.Version = server.ExeVersion(FolderName, filter, True, chunkSize, fastHash)
        Me.Running = server.ExeIsRunning(exePath)
        Try
            Dim bin As Byte() = server.RequestFile(configPath)
            If IsNothing(bin) Then Exit Sub
            Using ms As New MemoryStream(bin)
                Using sw As New StreamReader(ms)
                    Me.ConfigFile = sw.ReadToEnd()
                End Using
            End Using
        Catch
        End Try

        ReadConfig()
    End Sub

    Private Sub ReadConfig()
        Me.AppSettings = New Dictionary(Of String, String)()
        Me.ConnectionStrings = New Dictionary(Of String, String)()

        If String.IsNullOrEmpty(Me.ConfigFile) Then Exit Sub

        Try
            Dim doc As New XmlDocument()
            doc.PreserveWhitespace = True
            doc.LoadXml(Me.ConfigFile)

            Dim conf As XmlNode = doc.DocumentElement
            Dim appS As XmlNode = CXml.ChildNode(conf, "appSettings")
            Dim connStr As XmlNode = CXml.ChildNode(conf, "connectionStrings")

            If Not IsNothing(appS) Then
                For Each i As XmlNode In CXml.ChildNodes(appS, "add")
                    Me.AppSettings.Add(CXml.AttributeStr(i, "key"), CXml.AttributeStr(i, "value"))
                Next
            End If

            If Not IsNothing(connStr) Then
                For Each i As XmlNode In CXml.ChildNodes(connStr, "add")
                    Me.ConnectionStrings.Add(CXml.AttributeStr(i, "name"), CXml.AttributeStr(i, "connectionString"))
                Next
            End If
        Catch
        End Try
    End Sub



    'Presentation Logic
    Public ReadOnly Property MD5_Base64 As String
        Get
            Return Version.Base64
        End Get
    End Property
    Public ReadOnly Property MD5_Base64Trunc As String
        Get
            Return Version.Base64Trunc
        End Get
    End Property

End Class
