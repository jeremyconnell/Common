Imports System.Xml

<Serializable(), CLSCompliant(True)> _
Public MustInherit Class CRoot : Inherits CNode

#Region "Constructors"
    Public Sub New()
        MyBase.New(Nothing)
    End Sub
    Public Sub New(ByVal xml As String)
        Me.New()
        Me.Parse(xml)
    End Sub
    Public Sub New(ByVal xml As IO.TextReader)
        Me.New()
        Me.Read(xml)
    End Sub
#End Region

#Region "Public"
    'To/From String
    Public Overloads Sub Parse(ByVal xml As String)
        If String.IsNullOrEmpty(xml) Then Exit Sub
        Dim sr As New IO.StringReader(xml)
        Read(sr)
    End Sub
    Public Overrides Function ToString() As String
        Dim sw As New IO.StringWriter()
        Write(sw)
        Return sw.ToString()
    End Function

    'To/From Stream
    Public Overloads Sub Read(ByVal xml As IO.TextReader)
        Dim reader As New XmlTextReader(xml)
        Read(reader)
        reader.Close()
    End Sub
    Public Overloads Sub Write(ByVal xml As IO.TextWriter)
        Dim writer As New XmlTextWriter(xml)
        Write(writer)
        writer.Close()
    End Sub

    'To/From File
    Public Sub SaveAs(ByVal filePath As String)
        IO.File.WriteAllText(filePath, Me.ToString)
    End Sub
    Public Sub LoadFromFile(ByVal filePath As String)
        Me.Parse(IO.File.ReadAllText(filePath))
    End Sub
#End Region

End Class
