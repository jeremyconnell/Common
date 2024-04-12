Imports System.Runtime.Serialization

<CLSCompliant(True)>
Public Enum ESign
    EqualTo = 1
    NotEqualTo = 2
    GreaterThan = 3
    GreaterThanOrEq = 4
    LessThan = 5
    LessThanOrEq = 6
    [Like] = 7
    [IN] = 8
    NotIn = 9
End Enum

<CLSCompliant(True), DataContract(), Serializable()>
Public Class CCriteria
    'Protobuf: cannot serialise object
    'Shared Sub New()
    '    CProto.Prepare(Of CCriteria)()
    'End Sub
    Private Sub New()

    End Sub

    'Constructors
    Public Sub New(ByVal c As CNameValue)
        Me.New(c.Name, c.Value)
    End Sub
    Public Sub New(ByVal colName As String, ByVal colValue As Object)
        Me.New(colName, ESign.EqualTo, colValue)
    End Sub
    Public Sub New(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object)
        Me.ColumnName = colName
        Me.Sign = sign
        Me.ColumnValue = CDataSrc.NullValue(colValue)

        If Not IsNothing(colName) Then
            Me.MarkerName = colName.ToLower 'Must be unique
        Else
            Me.MarkerName = String.Empty
        End If
    End Sub


    'Members
    <DataMember(Order:=1)> Public ColumnName As String
    <DataMember(Order:=2)> Public Sign As ESign
    <DataMember(Order:=3)> Public ColumnValue As Object
    <DataMember(Order:=4)> Public MarkerName As String
End Class
