Imports System.Data.OleDb

<Serializable(), CLSCompliant(True)> _
Public Class COleDb : Inherits CDataSrcLocal

#Region "Constructors"
    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
    End Sub
#End Region

#Region "MustOverride"
    Public Overrides Function Connection() As IDbConnection
        Connection = New OleDbConnection(m_connectionString)
        Connection.Open()
    End Function
    Protected Friend Overloads Overrides Function CommandFactory(ByVal con As IDbConnection) As IDbCommand
        Return New OleDbCommand(String.Empty, CType(con, OleDbConnection))
    End Function
    Public Overrides Function DataAdapter(ByVal cmd As IDbCommand) As IDataAdapter
        Return New OleDbDataAdapter(CType(cmd, OleDbCommand))
    End Function
    Protected Overrides Sub DeriveParameters(ByVal cmd As IDbCommand)
        OleDbCommandBuilder.DeriveParameters(CType(cmd, OleDbCommand))
    End Sub
    Public Overrides Function Parameter(ByVal name As String, ByVal value As Object) As IDbDataParameter
        Dim p As New OleDbParameter(ParameterName(name), NullValue(value))
        SetParameterType(p, value)
        If TypeOf value Is DateTime Then p.OleDbType = OleDbType.Date
        Return p
    End Function
    'Note: "?" used to work, can try that or base method if there are any problems with @
    Public Overrides Function Marker(ByVal name As String) As String
        Return "@" & MyBase.Marker(name)
    End Function
    Public Overrides Function ParameterName(ByVal name As String) As String
        Return name
    End Function
#End Region

End Class
