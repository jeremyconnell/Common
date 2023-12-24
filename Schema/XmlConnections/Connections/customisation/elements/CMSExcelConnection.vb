Imports Framework
Imports System.Xml

Partial Public Class CMSExcelConnection : Implements IComparable(Of CMSExcelConnection)

#Region "Equals"
    Public Overrides Function GetHashCode() As Integer
        Return Me.PathLower.GetHashCode()
    End Function
    Public Function CompareTo(ByVal other As CMSExcelConnection) As Integer Implements System.IComparable(Of CMSExcelConnection).CompareTo
        Dim i As Integer = Me.FileName.ToLower.CompareTo(other.FileName.ToLower)
        If i = 0 Then i = Me.Folder.ToLower.CompareTo(other.Folder.ToLower)
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not TypeOf (obj) Is CMSExcelConnection Then Return False
        With CType(obj, CMSExcelConnection)
            Return .PathLower = Me.PathLower
        End With
    End Function
#End Region

#Region "Members"
    Private m_pathLower As String
#End Region

#Region "Derived"
    Public ReadOnly Property PathLower() As String
        Get
            If IsNothing(m_pathLower) Then
                m_pathLower = Path.ToLower
            End If
            Return m_pathLower
        End Get
    End Property
    Public ReadOnly Property FileName() As String
        Get
            Return IO.Path.GetFileName(Me.Path)
        End Get
    End Property
    Public ReadOnly Property Folder() As String
        Get
            Return IO.Path.GetDirectoryName(Me.Path)
        End Get
    End Property
#End Region

End Class
