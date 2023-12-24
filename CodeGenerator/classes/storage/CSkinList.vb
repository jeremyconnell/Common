<CLSCompliant(True), Serializable()> _
Public Class CSkinList : Inherits List(Of CSkin)

#Region "Constructors"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal size As Integer)
        MyBase.New(size)
    End Sub
    Public Sub New(ByVal xml As String)
        Me.FromXml(xml)
    End Sub
    Public Sub New(ByVal r As Xml.XmlReader)
        Me.FromXml(r)
    End Sub
    Public Sub New(ByVal zip As Byte())
        Me.FromZip(zip)
    End Sub
#End Region

#Region "Methods"
    Public Overloads Function Add(ByVal name As String, ByVal description As String) As CSkin
        Dim s As New CSkin(name, description)
        MyBase.Add(s)
        Return s
    End Function
    Public Overloads Function Add(ByVal url As String) As CSkin
        Dim s As New CSkin()
        s.Url = url
        If Not s.UpdateFromNetwork() Then Return Nothing
        MyBase.Add(s)
        Return s
    End Function
    Public Function AndDefault() As CSkinList
        Dim temp As New CSkinList(Me.Count + 1)
        temp.Add(CSkin.Default)
        temp.AddRange(Me)
        Return temp
    End Function
#End Region

#Region "ToXml"
    Public Function ToZip() As Byte()
        Return CBinary.SerialiseToBytesAndZip(ToXml)
    End Function
    Public Sub FromZip(ByVal zip As Byte())
        FromXml(CStr(CBinary.DeserialiseFromBytesAndUnzip(zip)))
    End Sub
    Public Function ToXml() As String
        Dim ms As New IO.MemoryStream()
        Dim tw As New Xml.XmlTextWriter(ms, Encoding.Default)
        ToXml(tw)
        tw.Close()
        Return Encoding.Default.GetString(ms.ToArray)
    End Function
    Private Sub ToXml(ByVal w As Xml.XmlWriter)
        w.WriteStartElement("Package")
        For Each i As CSkin In Me
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
        While r.Read()
            Add(New CSkin(r))
        End While
    End Sub
#End Region

#Region "Index"
    'Only uncomment if type is NOT an Integer (e.g. string/guid)
    Default Public Overloads ReadOnly Property Item(ByVal id As Guid) As CSkin
        Get
            Return GetById(id)
        End Get
    End Property
    Public Function GetById(ByVal id As Guid) As CSkin
        Dim c As CSkin = Nothing
        Index.TryGetValue(id, c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of Guid, CSkin)
    Private ReadOnly Property Index() As Dictionary(Of Guid, CSkin)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _index
                End If
            End If
            _index = New Dictionary(Of Guid, CSkin)(Me.Count)
            For Each i As CSkin In Me
                _index(i.Id) = i
            Next
            Return _index
        End Get
    End Property
#End Region

#Region "Update"
    Public Function UpdateFromNetwork() As Boolean
        Dim updated As Boolean = False
        For Each i As CSkin In Me
            updated = updated Or i.UpdateFromNetwork()
        Next
        Return updated
    End Function
#End Region

End Class
