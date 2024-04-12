Imports System.Text

Public Class CConflictException : Inherits Exception
    Public Sub New()    'Used for deserialization
        MyBase.New()
    End Sub
    Public Sub New(ByVal conflicts As Dictionary(Of String, Object), ByVal data As Dictionary(Of String, Object))
        MyBase.New(Details(conflicts))
        m_data = data
    End Sub
    Private Shared Function Details(ByVal conflicts As Dictionary(Of String, Object)) As String
        Dim msg As New StringBuilder("Another user has already updated this record, so the following changes will be discarded:")
        msg.Append(vbCrLf)
        Dim i As String
        For Each i In conflicts.Keys
            msg.Append(vbTab)
            msg.Append(i)
            msg.Append(":")
            msg.Append(vbCrLf)
            Dim s As String = conflicts(i).ToString
            If Len(s) > 255 Then s = s.Substring(0, 252) & "..."
            msg.Append(s)
            msg.Append(vbCrLf)
            msg.Append(vbCrLf)
        Next
        Return msg.ToString
    End Function

    Private m_data As Dictionary(Of String, Object)
    Public ReadOnly Property FreshData() As Dictionary(Of String, Object)
        Get
            Return m_data
        End Get
    End Property
End Class

<Serializable()> _
Public Class CConflictExceptionSerializable

    Public Sub New(ByVal ex As CConflictException)
        Me.Message = ex.Message
        Me.FreshData = ex.FreshData
    End Sub

    Public Message As String
    Public FreshData As Dictionary(Of String, Object)
End Class