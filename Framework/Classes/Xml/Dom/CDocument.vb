Imports System.Xml

<Serializable(), CLSCompliant(True)> _
Public MustInherit Class CDocument : Inherits CElement

#Region "Constructors"
    Public Sub New()
        MyBase.New(Nothing)
    End Sub
    Public Sub New(ByVal xml As String)
        Me.New()
        Me.Import(xml)
    End Sub
#End Region

#Region "Public"
    Public Overrides Function ToString() As String
        Dim doc As New XmlDocument()
        Export(doc)
        Return doc.OuterXml
    End Function
    Public Sub SaveAs(ByVal filePath As String, Optional ByVal indent As Boolean = False)
        If indent Then
            Dim doc As New XmlDocument
            Me.Export(doc)

            Dim settings As New XmlWriterSettings
            settings.Indent = True

            Dim writer As XmlWriter = XmlWriter.Create(filePath, settings)
            doc.Save(writer)
            writer.Close()
        Else
            IO.File.WriteAllText(filePath, Me.ToString)
        End If
    End Sub
    Public Sub LoadFromFile(ByVal filePath As String)
        Me.Import(IO.File.ReadAllText(filePath))
    End Sub
#End Region

#Region "Protected"
    Protected Overrides Function ExportSelf(ByVal parent As XmlNode) As XmlNode
        Return CXml.AddNode(parent, Me.TagName)
    End Function
    Protected Overridable Overloads Sub Import(ByVal xml As String)
        If String.IsNullOrEmpty(xml) Then Exit Sub

        Dim doc As New XmlDocument()
        Try
            doc.LoadXml(xml)
        Catch ex As Exception
            Exit Sub
        End Try
        Import(doc)
    End Sub
#End Region

End Class
