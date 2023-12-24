Imports Npgsql

Public Class CPostgresClient : Inherits CDataSrcLocal


    Shared Sub CPostgresClient()
        Dim postGres As New DFactoryLoc(Function(cs As String) New CPostgresClient(cs))
        m_factoryLoc.Add("postgres", postGres)
        m_factoryLoc.Add("ngpsql", postGres)
    End Sub

#Region "Constructors"
    Public Sub New(ByVal server As String, ByVal database As String, Optional ByVal port As Integer = 3306)
        MyBase.New(BuildConnectionString(server, database, port))
    End Sub
    Public Sub New(ByVal server As String, ByVal database As String, ByVal user As String, ByVal password As String, Optional ByVal port As Integer = 3306, Optional ByVal commandTimeoutSecs As Integer = 20, Optional ByVal connectionTimeoutSecs As Integer = 15, Optional ByVal protocol3 As Boolean = True)
        MyBase.New(BuildConnectionString(server, database, user, password, port, commandTimeoutSecs, connectionTimeoutSecs, protocol3))
    End Sub
    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
    End Sub
#End Region

#Region "MustOverride"
    Public Overrides Function Connection() As IDbConnection
        Connection = New NpgsqlConnection(m_connectionString)
        Connection.Open()
    End Function
    Protected Overloads Overrides Function CommandFactory(ByVal con As IDbConnection) As IDbCommand
        Return New NpgsqlCommand(String.Empty, CType(con, NpgsqlConnection))
    End Function
    Public Overrides Function DataAdapter(ByVal cmd As IDbCommand) As IDataAdapter
        Return New NpgsqlDataAdapter(CType(cmd, NpgsqlCommand))
    End Function
    Protected Overrides Sub DeriveParameters(ByVal cmd As IDbCommand)
        NpgsqlCommandBuilder.DeriveParameters(CType(cmd, NpgsqlCommand))
    End Sub
    Public Overrides Function Parameter(ByVal name As String, ByVal value As Object) As IDbDataParameter
        Dim p As New NpgsqlParameter(ParameterName(name), NullValue(value))
        SetParameterType(p, value)
        Return p
    End Function
    Public Overrides Function ParameterName(ByVal name As String) As String
        Return Marker(name)
    End Function
    Public Overrides Function Marker(ByVal name As String) As String
        Return "@" & MyBase.Marker(name)
    End Function
#End Region



    'Public Overrides Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal txOrNull As System.Data.IDbTransaction, ByVal oracleSequenceName As String) As Object
    '    tableName = tableName.ToLower
    '    pKeyName = pKeyName.ToLower
    '    For Each i As CNameValue In data
    '        i.Name = i.Name.ToLower
    '    Next
    '    Return MyBase.Insert(tableName, pKeyName, insertPk, data, txOrNull, oracleSequenceName)
    'End Function
    'Public Overrides Function Update(ByVal data As CNameValueList, ByVal where As CWhere) As Integer
    '    With where
    '        .TableName = .TableName.ToLower
    '    End With
    '    Return MyBase.Update(data, where)
    'End Function

#Region "Platform-Specific"
    Public Overrides ReadOnly Property SqlToListAllTables() As String
        Get
            Return "select c.relname FROM pg_catalog.pg_class c LEFT JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace WHERE c.relkind IN ('r','') AND n.nspname NOT IN ('pg_catalog', 'pg_toast') AND pg_catalog.pg_table_is_visible(c.oid);"
        End Get
    End Property
    Protected Overrides Function PagingSql(ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal count As Integer) As String
        Dim pageNum As Integer = pageIndexZeroBased + 1
        If descending Then sortByColumn = String.Concat(sortByColumn, " DESC")

        'Short syntax for first page (TOP)
        If pageIndexZeroBased = 0 Then
            Return String.Concat("SELECT ", selectColumns, " FROM ", tableName, " ORDER BY ", sortByColumn, " LIMIT ", pageSize)
        End If

        'If on the last page then reduce the pagesize
        Dim pageSizeActual As Integer = pageSize
        If pageNum * pageSize > count Then
            pageSizeActual = count - (pageIndexZeroBased * pageSize)
        End If

        Return String.Concat("SELECT ", selectColumns, " FROM ", tableName, " ORDER BY ", sortByColumn, " LIMIT ", pageIndexZeroBased * pageSize, ",", pageSize)
    End Function
    Public Overloads Overrides Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As System.Data.IDbTransaction, ByVal type As EQueryReturnType) As Object
        'Count the total records (used by the paging control)
        count = SelectCount(tableName, criteria, txOrNull)

        If descending Then sortByColumn = String.Concat(sortByColumn, " DESC")
        Return String.Concat("SELECT ", selectColumns, " FROM ", tableName, WhereCriteria(criteria), " ORDER BY ", sortByColumn, " LIMIT ", pageIndexZeroBased * pageSize, ",", pageSize)
    End Function
#End Region

#Region "Shared"
    Public Shared Function BuildConnectionString(ByVal server As String, ByVal database As String, Optional ByVal port As Integer = 5432) As String
        'See also: Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;
        Dim sb As New System.Text.StringBuilder("Integrated Security=true;")
        sb.Append("Server=")
        sb.Append(server)
        sb.Append(";")
        sb.Append("Database=")
        sb.Append(database)
        sb.Append(";")
        If port <> 5432 Then
            sb.Append("Port=")
            sb.Append(port)
            sb.Append(";")
        End If
        Return sb.ToString
    End Function
    Public Shared Function BuildConnectionString(ByVal server As String, ByVal database As String, ByVal user As String, ByVal password As String, Optional ByVal port As Integer = 5432, Optional ByVal commandTimeoutSecs As Integer = 20, Optional ByVal connectionTimeoutSecs As Integer = 15, Optional ByVal protocol3 As Boolean = True) As String
        'See also: Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;
        Dim sb As New System.Text.StringBuilder()
        sb.Append("Server=")
        sb.Append(server)
        sb.Append(";")
        sb.Append("Database=")
        sb.Append(database)
        sb.Append(";")
        sb.Append("User Id==")
        sb.Append(user)
        sb.Append(";")
        sb.Append("Password=")
        sb.Append(password)
        sb.Append(";")
        If port <> 5432 Then
            sb.Append("Port=")
            sb.Append(port)
            sb.Append(";")
        End If
        If commandTimeoutSecs <> 20 Then
            sb.Append("CommandTimeout=")
            sb.Append(commandTimeoutSecs)
            sb.Append(";")
        End If
        If connectionTimeoutSecs <> 15 Then
            sb.Append("Timeout=")
            sb.Append(connectionTimeoutSecs)
            sb.Append(";")
        End If
        sb.Append("protocol=")
        sb.Append(IIf(protocol3, 3, 2))
        sb.Append(";")
        Return sb.ToString
    End Function
#End Region

End Class
