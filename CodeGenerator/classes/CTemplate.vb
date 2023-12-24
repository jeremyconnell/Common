Imports System.IO


Public Class CTemplate : Inherits Framework.CTemplate

#Region "Constructor"
    Public Sub New(ByVal fileName As String, ByVal folderPath As String)
        Me.New(folderPath & fileName)
    End Sub
    Public Sub New(ByVal path As String)
        m_original = CUser_Templates.GetTemplate(path) 'Instead of ReadFile(path), get file from current set of templates (user-controlled)
        m_current = m_original
    End Sub
#End Region

End Class
