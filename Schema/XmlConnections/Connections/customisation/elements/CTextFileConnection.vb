Imports Framework
Imports System.Xml

Partial Public Class CTextFileConnection : Implements IComparable(Of CTextFileConnection)

#Region "Equals"
    Public Overrides Function GetHashCode() As Integer
        Return Me.PathLower.GetHashCode()
    End Function
    Public Function CompareTo(ByVal other As CTextFileConnection) As Integer Implements System.IComparable(Of CTextFileConnection).CompareTo
        Return Me.PathLower.CompareTo(other.PathLower)
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not TypeOf (obj) Is CTextFileConnection Then Return False
        With CType(obj, CTextFileConnection)
            Return .PathLower = Me.PathLower
        End With
    End Function
#End Region

#Region "Members"
    Private m_pathLower As String
#End Region

#Region "Properties"
    Public ReadOnly Property PathLower() As String
        Get
            If IsNothing(m_pathLower) Then
                m_pathLower = Path.ToLower
            End If
            Return m_pathLower
        End Get
    End Property
#End Region

End Class
