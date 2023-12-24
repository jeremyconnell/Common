'Generalisation of a remote data source, such as a webservice or webpage
'Doesnt support datareaders, open connections, or transactions
<CLSCompliant(True), Serializable()> _
Public MustInherit Class CDataSrcRemote : Inherits CDataSrc

#Region "Constructors"
    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
    End Sub
#End Region

#Region "DataReader/DataSet Generalisation"
    Public Overrides Function ExecuteQuery(ByVal cmd As CCommand, ByVal type As EQueryReturnType) As Object
        CheckTxIsNull(cmd.Transaction) 'Throw exception if a transaction is attempted
        Return ExecuteDataSet(cmd) 'Always use dataset (ignore type)
    End Function
    Protected Shared Sub CheckTxIsNull(ByVal txOrNull As IDbTransaction)
        If Not IsNothing(txOrNull) Then Throw New Exception("Warning: Transactions over remote datasource is not supported")
    End Sub
#End Region

#Region "MustOverride"
    'Used to pre-load the cache for a set of tables
    Public Function BulkSelect(ByVal ParamArray tableNames() As String) As List(Of DataSet)
        Return BulkSelect(New List(Of String)(tableNames))
    End Function
    Public Function BulkSelect(ByVal tables As List(Of String)) As List(Of DataSet)
        Dim cmds As New List(Of CCommand)(tables.Count)
        For Each i As String In tables
            cmds.Add(New CCommand(String.Concat("SELECT * FROM ", i)))
        Next
        Return BulkSelect(cmds)
    End Function
    Protected MustOverride Function BulkSelect(ByVal tables As List(Of CCommand)) As List(Of DataSet)

    Public Overrides Function BulkInsertWithTx(ByVal rowsToInsert As List(Of CNameValueList), ByVal tableName As String, primaryKey As String, isIdentity As Boolean) As List(Of Object)
        Return BulkInsertWithTx_(rowsToInsert, tableName, primaryKey, isIdentity)
    End Function
    Protected MustOverride Function BulkInsertWithTx_(ByVal rowsToInsert As List(Of CNameValueList), ByVal tableName As String, primaryKey As String, isIdentity As Boolean) As List(Of Object)
#End Region

End Class
