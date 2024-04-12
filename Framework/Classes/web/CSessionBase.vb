Imports System.Web.HttpContext

'1. Makes application variables work in any kind of application e.g. winforms
'2. Helps derived classes to provides static/shared properties with the correct casting and key constants
Public Class CSessionBase

#Region "Public"
    'Typed Getters - Standard Defaults
    Public Shared Function GetStr(ByVal key As String) As String
        Return GetStr(key, String.Empty)
    End Function
    Public Shared Function GetBool(ByVal key As String) As Boolean
        Return GetBool(key, False)
    End Function
    Public Shared Function GetInt(ByVal key As String) As Integer
        Return GetInt(key, Integer.MinValue)
    End Function
    Public Shared Function GetDate(ByVal key As String) As DateTime
        Return GetDate(key, DateTime.MinValue)
    End Function
	Public Shared Function GetObj(ByVal key As String) As Object
		Return [Get](key)
	End Function
	Public Shared Function GetStrList(ByVal key As String) As List(Of String)
		Return GetStrList(key, Nothing)
	End Function

	'Typed Getters - Custom defaults
	Public Shared Function GetStr(ByVal key As String, ByVal defaultValue As String) As String
        Dim s As String = CStr([Get](key))
        If String.IsNullOrEmpty(s) Then s = defaultValue
        Return s
    End Function
    Public Shared Function GetBool(ByVal key As String, ByVal defaultValue As Boolean) As Boolean
        Dim obj As Object = [Get](key)
        If obj Is Nothing Then Return defaultValue
        Return CBool(obj)
    End Function
    Public Shared Function GetInt(ByVal key As String, ByVal defaultValue As Integer) As Integer
        Dim obj As Object = [Get](key)
        If obj Is Nothing Then Return defaultValue
        Return CInt(obj)
    End Function
	Public Shared Function GetDate(ByVal key As String, ByVal defaultValue As DateTime) As DateTime
		Dim obj As Object = [Get](key)
		If obj Is Nothing Then Return defaultValue
		Return CType(obj, DateTime)
	End Function
	Public Shared Function GetStrList(ByVal key As String, ByVal defaultValue As List(Of String)) As List(Of String)
		Dim obj As Object = [Get](key)
		If obj Is Nothing Then Return defaultValue
		Return CType(obj, List(Of String))
	End Function

	'Typed Setters
	Public Shared Sub SetStr(ByVal key As String, ByVal value As String)
        [Set](key, value)
    End Sub
    Public Shared Sub SetInt(ByVal key As String, ByVal value As Integer)
        [Set](key, value)
    End Sub
    Public Shared Sub SetBool(ByVal key As String, ByVal value As Boolean)
        [Set](key, value)
    End Sub
    Public Shared Sub SetDate(ByVal key As String, ByVal value As DateTime)
        [Set](key, value)
    End Sub
    Public Shared Sub SetObj(ByVal key As String, ByVal value As Object)
        [Set](key, value)
    End Sub

    'Generic Get/Set (use reserved keywords)
    Public Shared Function [Get](ByVal key As String) As Object
        If IsWebApplication Then Return Current.Session(key)

        If IsNothing(m_session) Then m_session = New Dictionary(Of String, Object)
        Dim o As Object = Nothing
        m_session.TryGetValue(key, o)
        Return o
    End Function
    Public Shared Sub [Set](ByVal key As String, ByVal value As Object)
        If IsWebApplication Then
            Current.Session(key) = value
            Exit Sub
        End If

        If IsNothing(m_session) Then m_session = New Dictionary(Of String, Object)
        m_session(key) = value
    End Sub
#End Region

#Region "Private"
    Private Shared m_session As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
    Private Shared ReadOnly Property IsWebApplication() As Boolean
        Get
            Return Not IsNothing(Current) AndAlso Not IsNothing(Current.Session)
        End Get
    End Property
#End Region

End Class
