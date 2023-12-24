<Serializable(), CLSCompliant(True)> _
Public Class CPathAndTemplate : Implements IComparable(Of CPathAndTemplate)

#Region "Members"
    Private m_relativePath As String
    Private m_template As String
#End Region

#Region "Properties"
    Public ReadOnly Property RelativePath() As String
        Get
            Return m_relativePath
        End Get
    End Property
    Public Property Template() As String
        Get
            Return m_template
        End Get
        Set(ByVal value As String)
            m_template = value
        End Set
    End Property
#End Region

#Region "Constructor"
    Public Sub New(ByVal r As Xml.XmlReader)
        If r.MoveToAttribute("RelativePath") Then m_relativePath = r.ReadContentAsString

        r.Read()
        m_template = r.Value
    End Sub
    Public Sub New(ByVal relativePath As String)
        m_relativePath = relativePath.ToLower.Replace("\", "/")
        Me.Template = String.Empty
    End Sub
    Public Sub New(ByVal relativePath As String, ByVal template As String)
        Me.New(relativePath)
        Me.Template = template
    End Sub

    'Was a constructor, but cant distinguish between string path and string body
    Public Sub LoadFromFile(ByVal rootFolder As String)
        Dim fullPath As String = rootFolder & RelativePath
        If Not IO.File.Exists(fullPath) Then
            Me.Template = String.Concat("File does not exist: ", RelativePath)
        Else
            Me.Template = IO.File.ReadAllText(fullPath)
        End If
    End Sub
#End Region

#Region "ToXml"
    Public Sub ToXml(ByVal w As Xml.XmlWriter)
        w.WriteStartElement("Template")
        w.WriteAttributeString("RelativePath", Me.RelativePath)
        w.WriteValue(Me.Template)
        w.WriteEndElement()
    End Sub
#End Region

#Region "Equals"
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not TypeOf obj Is CPathAndTemplate Then Return False
        Return RelativePath.Equals(CType(obj, CPathAndTemplate).RelativePath)
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return RelativePath.GetHashCode()
    End Function
#End Region

#Region "Sort"
    Public Function CompareTo(ByVal other As CPathAndTemplate) As Integer Implements System.IComparable(Of CPathAndTemplate).CompareTo
        Return RelativePath.CompareTo(other.RelativePath)
    End Function
#End Region

#Region "Shared"
    Public Shared ReadOnly Property DownloadsPath() As String
        Get
            Return String.Concat(My.Application.Info.DirectoryPath, "\\downloads\\")
        End Get
    End Property
#End Region

End Class
