Imports Ionic.Zip

<Serializable(), CLSCompliant(True)> _
Public Class CSkin : Inherits List(Of CPathAndTemplate) : Implements IComparable(Of CSkin)

#Region "Constants"
    Public Const DEFAULT_FILE_NAME As String = "templates.zip"
    Public Const DEFAULT_FILE_NAME_OLD As String = "Default.gz"
    Public Const URL_MARKER As String = "url: "
    Public Const DEFAULT_NAME As String = "-- Default --"
#End Region

#Region "Members"
    Private m_id As Guid
    Private m_name As String
    Private m_description As String
    Private m_url As String
    Private m_lastUpdated As DateTime = DateTime.MinValue

    Private Shared m_default As CSkin
#End Region

#Region "Constructor"
    Public Sub New()
        Me.new(String.Empty, String.Empty)
    End Sub
    Public Sub New(ByVal name As String, ByVal description As String)
        m_id = Guid.NewGuid
        m_name = name
        m_description = description
    End Sub
    Private Sub New(ByVal copyFrom As CSkin)
        MyBase.New(copyFrom)
        m_id = copyFrom.m_id
        m_name = copyFrom.m_name
        m_description = copyFrom.m_description
    End Sub
    Public Sub New(ByVal gzip As Byte())
        Me.FromGZip(gzip)
    End Sub
    Public Sub New(ByVal xml As String)
        Me.FromXml(xml)
    End Sub
    Public Sub New(ByVal r As Xml.XmlReader)
        Me.FromXml(r)
    End Sub
#End Region

#Region "Properties"
    Public ReadOnly Property Id() As Guid
        Get
            Return m_id
        End Get
    End Property
    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            m_name = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return m_description
        End Get
        Set(ByVal value As String)
            m_description = value
        End Set
    End Property
    Public Property Url() As String
        Get
            Return m_url
        End Get
        Set(ByVal value As String)
            m_url = value
        End Set
    End Property

    Public Function AllTemplates() As CSkin
        Dim temp As New CSkin(Me)
        For Each i As CPathAndTemplate In CSkin.Default
            If Not temp.Contains(i) Then temp.Add(i)
        Next
        Return temp
    End Function
#End Region

#Region "Methods"
    'Template
    Public Function GetTemplate(ByVal relativePath As String) As String
        Dim t As CPathAndTemplate = Item(relativePath)
        If IsNothing(t) Then
            If Me Is [Default] OrElse IsNothing([Default](relativePath)) Then Return String.Concat("Template not found: ", relativePath)
            Return [Default](relativePath).Template
        End If
        Return t.Template
    End Function

    'Add
    Public Function AddFromFile(ByVal relativePath As String, ByVal rootFolder As String) As CPathAndTemplate
        Dim pt As New CPathAndTemplate(relativePath)
        pt.LoadFromFile(rootFolder)
        Add(pt)
        Return pt
    End Function
    Public Shadows Function Add(ByVal relativePath As String, ByVal template As String) As CPathAndTemplate
        Dim pt As New CPathAndTemplate(relativePath, template)
        Add(pt)
        Return pt
    End Function
    Public Shadows Sub Add(ByVal item As CPathAndTemplate)
        Dim existing As CPathAndTemplate = Me(item.RelativePath)
        If Not IsNothing(existing) Then MyBase.Remove(existing)
        MyBase.Add(item)
        Index(item.RelativePath) = item
    End Sub

    'Load From Dir (Recurse files)
    Private Sub LoadFromDir(ByVal rootFolder As String)
        If Not rootFolder.EndsWith("/") AndAlso Not rootFolder.EndsWith("\") Then rootFolder &= "\"
        LoadFromDir(rootFolder, rootFolder)
    End Sub
    Private Sub LoadFromDir(ByVal rootFolder As String, ByVal startAtFolder As String)
        If Not IO.Directory.Exists(startAtFolder) Then Exit Sub
        For Each i As String In IO.Directory.GetDirectories(startAtFolder)
            LoadFromDir(rootFolder, i)
        Next
        For Each i As String In IO.Directory.GetFiles(startAtFolder)
            If i.EndsWith(".gz") Then Continue For
            Dim relativePath As String = i.Substring(rootFolder.Length - 1)
            Me.AddFromFile(relativePath, rootFolder)
        Next
    End Sub

    'Import/Export
    'Public Shared Function ImportDefaultFromConfig() As CSkin
    '    Dim s As New CSkin
    '    s.m_id = Guid.Empty
    '    s.Name = DEFAULT_NAME
    '    s.Url = CConfig.DefaultTemplatesUrl
    '    If s.UpdateFromNetwork() Then
    '        s.ExportToDefaultFile()
    '        Return s
    '    End If
    '    Return Nothing
    'End Function
    Public Shared Function ImportDefaultFromFile() As CSkin
        Dim path As String = CPathAndTemplate.DownloadsPath & DEFAULT_FILE_NAME
        If Not IO.File.Exists(path) Then
            path = CPathAndTemplate.DownloadsPath & DEFAULT_FILE_NAME_OLD
            If Not IO.File.Exists(path) Then Return Nothing

            'Old binary format
            Dim skin As Object = CBinary.DeserialiseFromBytesAndUnzip(path)
            If TypeOf skin Is CSkin Then Return CType(skin, CSkin)

            'Old xml format
            Dim xml As String = CType(skin, String)
            Return New CSkin(xml)
        Else
            Dim skin As New CSkin()
            skin.m_id = Guid.Empty
            skin.Name = DEFAULT_NAME
            skin.FromZip(path)
            skin.Url = CConfig.DefaultTemplatesUrl
            skin.m_lastUpdated = IO.File.GetLastWriteTime(path)
            Return skin
        End If
    End Function
    Public Sub ExportToDefaultFile()
        ExportToDefaultFile(CPathAndTemplate.DownloadsPath & DEFAULT_FILE_NAME)
    End Sub

    'Methods for helper app
    Public Sub ExportToDefaultFile(ByVal path As String)
        If IO.File.Exists(path) Then IO.File.Delete(path)

        'Old binary format
        'CBinary.SerialiseToBytesAndZip(Me, path)

        'Old xml format
        'CBinary.SerialiseToBytesAndZip(Me.ToXml, path)

        'New Zip format
        ToZip(path)
    End Sub
    Public Function UpdateFromNetwork() As Boolean
        If String.IsNullOrEmpty(Url) Then Return False
        If m_lastUpdated.AddMinutes(10) > DateTime.Now Then Return True 'Less than 10mins old, dont worry about it

        Dim wc As New Net.WebClient()
        Dim data As Byte() = Nothing
        Dim uniqueUrl As String = Url
        If Url.ToLower.Contains("http") Then uniqueUrl = String.Concat(Url, IIf(Url.Contains("?"), "&", "?"), DateTime.Now.Ticks)
        Try
            data = wc.DownloadData(uniqueUrl)
        Catch ex As Exception
            MsgBox("Refresh templates from network failed", MsgBoxStyle.OkOnly, String.Concat("Template: '", Name, "'", vbCrLf, "Url: '", Url, "'", vbCrLf, ex.Message))
            Return False
        End Try
        Try
            If String.IsNullOrEmpty(Name) Then Name = IO.Path.GetFileNameWithoutExtension(Url)
            FromZip(data, Name)
        Catch ex As Exception
            MsgBox("Create templates from network failed", MsgBoxStyle.OkOnly, String.Concat("Template: '", Name, "'", vbCrLf, "Url: '", Url, "'", vbCrLf, ex.Message))
            Return False
        End Try
        m_lastUpdated = DateTime.Now

        Return True
    End Function
#End Region

#Region "ToXml"
    Public Function ToZip() As Byte()
        Dim z As ZipFile = ToZipFile()
        Dim ms As New IO.MemoryStream()
        z.Save(ms)
        Return ms.ToArray
    End Function
    Public Sub ToZip(ByVal filePath As String)
        ToZipFile().Save(filePath)
    End Sub
    Private Function ToZipFile() As ZipFile
        Dim z As New ZipFile()
        If String.IsNullOrEmpty(Me.Url) Then
            z.Comment = Me.Description
        Else
            z.Comment = String.Concat(URL_MARKER, Me.Url, vbCrLf, Me.Description)
        End If

        z.UseUnicodeAsNecessary = True
        For Each i As CPathAndTemplate In Me
            z.AddEntry(IO.Path.GetFileName(i.RelativePath), IO.Path.GetDirectoryName(i.RelativePath), i.Template)
        Next
        z.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression
        Return z
    End Function
    Public Sub FromZip(ByVal zip As Byte(), ByVal name As String)
        Me.Name = name
        FromZip(ZipFile.Read(zip))
    End Sub
    Public Sub FromZip(ByVal filePath As String)
        If String.IsNullOrEmpty(Me.Name) Then Me.Name = IO.Path.GetFileNameWithoutExtension(filePath)
        FromZip(ZipFile.Read(filePath))
    End Sub
    Private Sub FromZip(ByVal z As ZipFile)
        If String.IsNullOrEmpty(Me.Description) Then Me.Description = z.Comment
        For Each i As ZipEntry In z.Entries
            If Len(IO.Path.GetFileName(i.FileName)) = 0 Then Continue For
            'If i.UncompressedSize = 0 Then Continue For
            Dim ms As New IO.MemoryStream(CInt(i.UncompressedSize))
            i.Extract(ms)
            Dim s As String = i.FileName.ToLower()
            If s.StartsWith("templates") Then s = s.Substring(9)
            Dim template As String = CBinary.BytesToString(ms.ToArray)
            template = template.Replace("???", String.Empty)
            Add(s, template)
        Next
        If Not String.IsNullOrEmpty(Me.Description) AndAlso Me.Description.StartsWith(URL_MARKER) AndAlso Me.Description.Contains(vbCrLf) Then
            Dim index1 As Integer = URL_MARKER.Length
            Dim index2 As Integer = Me.Description.IndexOf(vbCrLf)
            Me.Url = Me.Description.Substring(index1, index2 - index1)
            Me.Description = Me.Description.Substring(index2)
        End If
    End Sub
    Public Function ToGZip() As Byte()
        Return CBinary.SerialiseToBytesAndZip(ToXml)
    End Function
    Public Sub FromGZip(ByVal zip As Byte())
        FromXml(CStr(CBinary.DeserialiseFromBytesAndUnzip(zip)))
    End Sub
    Public Function ToXml() As String
        Dim ms As New IO.MemoryStream()
        Dim tw As New Xml.XmlTextWriter(ms, Encoding.Default)
        ToXml(tw)
        tw.Close()
        Return Encoding.Default.GetString(ms.ToArray)
    End Function
    Public Sub ToXml(ByVal w As Xml.XmlWriter)
        w.WriteStartElement("Skin")
        w.WriteAttributeString("Id", Me.Id.ToString)
        w.WriteAttributeString("Name", Me.Name)
        w.WriteAttributeString("Description", Me.Description)
        For Each i As CPathAndTemplate In Me
            i.ToXml(w)
        Next
        w.WriteEndElement()
    End Sub
    Private Sub FromXml(ByVal xml As String)
        Dim r As New Xml.XmlTextReader(xml, System.Xml.XmlNodeType.Document, Nothing)
        r.Read()
        FromXml(r)
        r.Close()
    End Sub
    Private Sub FromXml(ByVal r As Xml.XmlReader)
        If r.MoveToAttribute("Id") Then m_id = New Guid(r.Value)
        If r.MoveToAttribute("Name") Then m_name = r.Value
        If r.MoveToAttribute("Description") Then m_description = r.Value
        While r.Read()
            Add(New CPathAndTemplate(r))
        End While
    End Sub
#End Region

#Region "Compare/Equals"
    Public ReadOnly Property IsDefault() As Boolean
        Get
            Return Equals(CSkin.Default)
        End Get
    End Property
    Public Function CompareTo(ByVal other As CSkin) As Integer Implements System.IComparable(Of CSkin).CompareTo
        Return Me.Name.CompareTo(other.Name)
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not TypeOf obj Is CSkin Then Return False
        Return CType(obj, CSkin).Id.Equals(Me.Id)
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return Id.GetHashCode()
    End Function
#End Region

#Region "Shared"
    Public Shared ReadOnly Property [Default]() As CSkin
        Get
            If IsNothing(m_default) Then
                'A zip file that is packaged with the installed code-gen, and updated when a new one is downloaded
                m_default = CSkin.ImportDefaultFromFile()
            End If
            Return m_default
        End Get
    End Property
#End Region

#Region "Index"
    'Only uncomment if type is NOT an Integer (e.g. string/guid)
    Default Public Overloads ReadOnly Property Item(ByVal id As String) As CPathAndTemplate
        Get
            Return GetById(id)
        End Get
    End Property
    Public Function GetById(ByVal relativePath As String) As CPathAndTemplate
        relativePath = New CPathAndTemplate(relativePath).RelativePath

        If String.IsNullOrEmpty(relativePath) Then Return Nothing
        Dim c As CPathAndTemplate = Nothing
        Index.TryGetValue(relativePath, c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of String, CPathAndTemplate)
    Private ReadOnly Property Index() As Dictionary(Of String, CPathAndTemplate)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _index
                End If
            End If
            _index = New Dictionary(Of String, CPathAndTemplate)(Me.Count)
            For Each i As CPathAndTemplate In Me
                _index(i.RelativePath) = i
            Next
            Return _index
        End Get
    End Property
#End Region

End Class
