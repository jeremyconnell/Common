Imports Oracle.DataAccess.Client

<Serializable()> _
Public Class COracleClientOdp : Inherits COracleClient

#Region "Constructors"
    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
    End Sub
#End Region

#Region "MustOverride"
    Public Overrides Function Connection() As IDbConnection
        Connection = New OracleConnection(m_connectionString)
        Connection.Open()
    End Function
    Protected Friend Overloads Overrides Function CommandFactory(ByVal con As IDbConnection) As IDbCommand
        Return New OracleCommand(String.Empty, CType(con, OracleConnection))
    End Function
    Public Overrides Function DataAdapter(ByVal cmd As IDbCommand) As IDataAdapter
        Return New OracleDataAdapter(CType(cmd, OracleCommand))
    End Function
    Protected Overrides Sub DeriveParameters(ByVal cmd As IDbCommand)
        Throw New Exception("ODP Oracle client does not support DeriveParameters. EITHER: (1) Use the Microsoft client by specifying 'oraclems' in the config file OR (2) name all stored proc parameters in your code using a Framework.CNameValueList")
        'OracleCommandBuilder.DeriveParameters(cmd)
    End Sub
    Public Overrides Function Parameter(ByVal name As String, ByVal value As Object) As IDbDataParameter
        Dim p As New OracleParameter(ParameterName(name), NullValue(value))
        SetParameterType(p, value)
        Return p
    End Function
#End Region

End Class
