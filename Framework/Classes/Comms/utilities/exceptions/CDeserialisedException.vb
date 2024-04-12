Public Class CDeserialisedException : Inherits System.Exception

    'Constructor
    Private Sub New()
    End Sub
    Public Sub New(ex As CException)
        Me.New(ex.Message, ex.StackTrace, ex.Inner)
    End Sub
    Private Sub New(msg As String, stackTrace As String, inner As CException)
        MyBase.New(msg, Resolve(inner))
        m_stackTrace = stackTrace
    End Sub

    'Private
    Private Shared Function Resolve(inner As CException) As CDeserialisedException
        If IsNothing(inner) Then Return Nothing
        Return New CDeserialisedException(inner)
    End Function

    'Member
    Private m_stackTrace As String

    'Property
    Public Overrides ReadOnly Property StackTrace As String
        Get
            Return m_stackTrace
        End Get
    End Property
End Class