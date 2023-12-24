Imports System.Data.Odbc

<Serializable()> _
Public Class COdbc : Inherits CDataSrcLocal

#Region "Constructors"
    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
    End Sub
#End Region

#Region "MustOverride"
    Public Overrides Function Connection() As IDbConnection
        Connection = New OdbcConnection(m_connectionString)
        Connection.Open()
    End Function
    Protected Friend Overloads Overrides Function CommandFactory(ByVal con As IDbConnection) As IDbCommand
        Return New OdbcCommand(String.Empty, CType(con, OdbcConnection))
    End Function
    Public Overrides Function DataAdapter(ByVal cmd As IDbCommand) As IDataAdapter
        Return New OdbcDataAdapter(CType(cmd, OdbcCommand))
    End Function
    Protected Overrides Sub DeriveParameters(ByVal cmd As IDbCommand)
        OdbcCommandBuilder.DeriveParameters(CType(cmd, OdbcCommand))
    End Sub
    Public Overrides Function Parameter(ByVal name As String, ByVal value As Object) As IDbDataParameter
        Dim p As New OdbcParameter(ParameterName(name), NullValue(value))
        SetParameterType(p, value)
        Return p
    End Function
    Public Overrides Function Marker(ByVal name As String) As String
        Return "?" & MyBase.Marker(name)
    End Function
    Public Overrides Function ParameterName(ByVal name As String) As String
        Return String.Empty
    End Function
#End Region

End Class
