
Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueNull : Inherits CValue
    'Data
    Public Sub New(name As String)
        MyBase.New(name)
    End Sub
    Private Sub New()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.DbNull
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return System.DBNull.Value
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        Select Case other.Type
            Case EValueType.DbNull
                If Me.Type = EValueType.DbNull Then Return 0
                Return 1
            Case Else : Return -1
        End Select
    End Function
    Public Overrides Function Serialise() As Byte()
        Return New Byte() {}
    End Function
End Class
