Imports System.IO

<CLSCompliant(True)> _
Public Class CTemplate

#Region "Attributes"
    Public Shared START_TAG As String = "["
    Public Shared END_TAG As String = "]"

    Protected m_original As String
    Protected m_current As String
#End Region

#Region "Constructor"
    Public Sub New()
        m_original = String.Empty
        m_current = String.Empty
    End Sub
    Public Sub New(ByVal fileName As String, ByVal folderPath As String)
        Me.New(folderPath & fileName)
    End Sub
    Public Sub New(ByVal path As String)
        m_original = ReadFile(path)
        m_current = m_original
    End Sub
#End Region

#Region "Public"
    Public Sub Initialise(ByVal s As String)
        m_original = s
        m_current = s
    End Sub
    Public ReadOnly Property Template() As String
        Get
            Return m_current
        End Get
    End Property
    Public Sub Reset()
        m_current = m_original
    End Sub
    Public Sub Replace(ByVal name As String, ByVal value As String)
        If Len(name) = 0 Then Exit Sub
        If IsNothing(value) Then value = String.Empty
        m_current = m_current.Replace(Tags(name), value)
    End Sub
    Public Sub ReplaceRaw(ByVal name As String, ByVal value As String)
        If IsNothing(value) Then value = String.Empty
        If m_current.Contains(name) Then m_current = m_current.Replace(name, value)
    End Sub
    Public Sub Append(ByVal value As String)
        m_current &= value
    End Sub
    Public Sub SaveAs(ByVal filePath As String)
        IO.File.WriteAllText(filePath, Me.Template)
    End Sub
    Public Function Contains(ByVal name As String) As Boolean
        Return -1 <> m_current.IndexOf(Tags(name))
    End Function
    Public Shared Function Tags(ByVal name As String) As String
        Return String.Concat(START_TAG, name, END_TAG)
    End Function
#End Region

#Region "Shared - Read/Write File"
    Protected Shared Sub WriteFile(ByVal filePath As String, ByVal fileContent As String)
        Dim fs As FileStream = File.OpenWrite(filePath)
        Dim sw As New StreamWriter(fs)
        sw.Write(fileContent)
        sw.Close()
    End Sub
    Protected Shared Function ReadFile(ByVal filePath As String) As String
        If Not IO.File.Exists(filePath) Then Throw New Exception("File Not Found: " & filePath)
        Dim fs As FileStream = File.OpenRead(filePath)
        Dim sr As New StreamReader(fs)
        ReadFile = sr.ReadToEnd()
        sr.Close()
    End Function
#End Region

End Class
