Imports System.Web.HttpContext
Imports System.Web

'1. Makes application variables work in any kind of application e.g. winforms
'2. Helps derived classes to provides static/shared properties with the correct casting and key constants
Public Class CApplication

#Region "Public"
    Public Shared Function [Get](ByVal key As String) As Object
        If IsWebApplication Then Return Current.Application(key)

        If IsNothing(m_application) Then m_application = New Dictionary(Of String, Object)
        Dim o As Object = Nothing
        m_application.TryGetValue(key, o)
        Return o
    End Function
    Public Shared Sub [Set](ByVal key As String, ByVal value As Object)
        If IsWebApplication Then
            Current.Application(key) = value
            Exit Sub
        End If

        If IsNothing(m_application) Then m_application = New Dictionary(Of String, Object)
        m_application(key) = value
    End Sub
    Public Shared Sub ClearAll()
        If IsWebApplication Then
            With HttpContext.Current
                For Each i As DictionaryEntry In .Cache
                    .Cache.Remove(CStr(i.Key))
                Next
            End With
            HttpContext.Current.Application.Clear()
        Else
            m_application.Clear()
        End If
    End Sub
#End Region

#Region "Private"
    Private Shared m_application As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
    Public Shared ReadOnly Property IsWebApplication() As Boolean
        Get
            Return Not IsNothing(Current)
        End Get
    End Property
#End Region

End Class
