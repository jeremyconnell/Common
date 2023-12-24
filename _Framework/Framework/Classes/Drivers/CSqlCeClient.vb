'Imports System.Data.SqlServerCe
'Imports System.Text

''Note for 'money' data tpe: SetParameterType function does not support nulls well, because the default dbtype doesnt work. Use decimal(16,2) or 'money with NOT NULL'

'<Serializable(), CLSCompliant(True)> _
'Public Class CSqlCeClient : Inherits CDataSrcLocal

'#Region "Constants"
'    'Avoids blocking due to transactions
'    'Private Const HINT As String = " WITH (READPAST)" 'Option A: No blocking, no dirty reads
'    Private Const HINT As String = " WITH (NOLOCK)"   'Option B: Dirty reads (returns uncommited records)"
'#End Region

'#Region "Constructors"
'    Public Sub New(ByVal connectionString As String)
'        MyBase.New(connectionString)
'    End Sub
'#End Region

'#Region "MustOverride"
'    Public Overrides Function Connection() As IDbConnection
'        Connection = New SqlCeConnection(m_connectionString)
'        Connection.Open()
'    End Function
'    Protected Friend Overloads Overrides Function CommandFactory(ByVal con As IDbConnection) As IDbCommand
'        Return New SqlCeCommand(String.Empty, CType(con, SqlCeConnection))
'    End Function
'    Public Overrides Function DataAdapter(ByVal cmd As IDbCommand) As IDataAdapter
'        Return New SqlCeDataAdapter(CType(cmd, SqlCeCommand))
'    End Function
'    Protected Overrides Sub DeriveParameters(ByVal cmd As IDbCommand)
'        Throw New Exception("Sql CE does not support DeriveParameters - must supply named parameters")
'        'SqlCeCommandBuilder.DeriveParameters(CType(cmd, SqlCeCommand))
'    End Sub
'    Public Overrides Function Parameter(ByVal name As String, ByVal value As Object) As IDbDataParameter
'        If TypeOf value Is String AndAlso CStr(value).Length > 4000 Then
'            Dim p As New SqlCeParameter(ParameterName(name), SqlDbType.NText)
'            p.Value = NullValue(value)
'            Return p
'        Else
'            Dim p As New SqlCeParameter(ParameterName(name), NullValue(value))
'            SetParameterType(p, value)
'            Return p
'        End If
'    End Function
'    Public Overrides Function Marker(ByVal name As String) As String
'        name = MyBase.Marker(name)
'        If name.StartsWith("@") Then Return name
'        Return "@" & name
'    End Function
'#End Region




'#Region "Optional"
'    Friend Const SEQ_GUID_MARKER As String = "SEQ_GUID_MARKER"
'    Public Overrides Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal txOrNull As IDbTransaction, ByVal sequentialGuidMarker As String) As Object
'        'Normal case: Last parameter shouldn't be used (but might have a generic default value)
'        If String.IsNullOrEmpty(sequentialGuidMarker) OrElse sequentialGuidMarker <> SEQ_GUID_MARKER Then
'            Return MyBase.Insert(tableName, pKeyName, insertPk, data, txOrNull, Nothing)
'        End If

'        'Special case - sequential guid for primary key => use an OUTPUT clause in the sql to get the GUID
'        Dim cmd As IDbCommand = InsertCmd(tableName, pKeyName, insertPk, data, txOrNull) 'Open or Share connection
'        cmd.CommandText = cmd.CommandText.Replace(" VALUES ", String.Concat(" OUTPUT inserted.", pKeyName, " VALUES "))

'        'Single call, should return a guid
'        If LOGGING Then Log(cmd)
'        Dim closeConnection As Boolean = IsNothing(cmd.Transaction)
'        Try
'            Dim guid As Object = ExecuteScalar(cmd)
'            If TypeOf guid Is DBNull Then Throw New Exception("A NewSequenceId was not returned - check that column '" & pKeyName & "' has a default value of (newsequenceid()), otherwise set property CBaseDynamic.PrimaryKeyIsSqlServerSequentialGuid=false")
'            Return guid
'        Catch ex As Exception
'            Rethrow(ex, cmd.CommandText)
'            Return Nothing
'        Finally
'            If closeConnection Then cmd.Connection.Close()
'        End Try
'    End Function
'    Public Overrides ReadOnly Property SqlToListAllTables() As String
'        Get
'            Return "SELECT [Name] AS [Tables] FROM [sysobjects] WHERE xtype='U' ORDER BY [Name]"
'        End Get
'    End Property
'    Protected Overrides Function PagingSql(ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal count As Integer) As String
'        If Not Is2005OrGreater Then Return MyBase.PagingSql(pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, count)

'        'Special case - first page use TOP command
'        If pageIndexZeroBased = 0 Then
'            Dim sql As New StringBuilder("SELECT TOP ")
'            sql.Append(pageSize)
'            sql.Append(" * FROM ")
'            sql.Append(tableName)
'            If Not tableName.ToLower.Contains(" join ") Then sql.Append(HINT)
'            sql.Append(" ORDER BY ")
'            sql.Append(sortByColumn)
'            If descending Then sql.Append(" DESC")
'            Return sql.ToString
'        End If

'        'Hack for very complex joins where colnames are not unique - Change ORDER BY Name to ORDER BY [Table].Name
'        Dim tbl As String = String.Empty
'        If sortByColumn.Contains(".") AndAlso Not sortByColumn.Contains("(") Then
'            Dim index As Integer = sortByColumn.LastIndexOf(".") + 1
'            tbl = sortByColumn.Substring(0, index)
'            sortByColumn = sortByColumn.Substring(index)
'        End If

'        Dim sb As New StringBuilder("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY ")
'        sb.Append(tbl).Append(sortByColumn)
'        If descending Then sb.Append(" DESC")
'        sb.Append(") AS _rownumber_, ")
'        sb.Append(selectColumns)
'        sb.Append(" FROM ")
'        sb.Append(tableName)
'        If Not tableName.ToLower.Contains(" join ") Then sb.Append(HINT)
'        sb.Append(") AS Numbered WHERE _rownumber_ BETWEEN ")
'        sb.Append(pageIndexZeroBased * pageSize + 1)
'        sb.Append(" AND ")
'        sb.Append((1 + pageIndexZeroBased) * pageSize)
'        sb.Append(" ORDER BY ")
'        sb.Append(sortByColumn)
'        If descending Then sb.Append(" DESC")
'        Return sb.ToString
'        'e.g. SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY AuditID DESC) AS _rownum_, * FROM tblAudit_Trail) As Numbered WHERE _rownum_ between 5 and 9 ORDER BY AuditId desc
'    End Function
'    Public Overrides Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As System.Data.IDbTransaction, ByVal type As EQueryReturnType) As Object
'        If Not Is2005OrGreater Then Return MyBase.PagingWithFilters(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, criteria, txOrNull, type)

'        'Count the total records
'        count = SelectCount(tableName, criteria, txOrNull)

'        'Special case - first page use TOP command
'        If pageIndexZeroBased = 0 Then
'            Dim sql As New StringBuilder("SELECT TOP ")
'            sql.Append(pageSize)
'            sql.Append(" ")
'            sql.Append(selectColumns)
'            sql.Append(" FROM ")
'            sql.Append(tableName)
'            If Not tableName.ToLower.Contains(" join ") Then sql.Append(HINT)
'            sql.Append(Me.WhereCriteria(criteria))
'            sql.Append(" ORDER BY ")
'            sql.Append(sortByColumn)
'            If descending Then sql.Append(" DESC")

'            Dim topCmd As IDbCommand = Command(sql.ToString, txOrNull)
'            Parameters(topCmd, criteria)
'            Return ExecuteQuery(topCmd, type)
'        End If

'        'Hack for very complex joins where colnames are not unique - Change ORDER BY Name to ORDER BY [Table].Name
'        Dim tbl As String = String.Empty
'        If sortByColumn.Contains(".") AndAlso Not sortByColumn.Contains("(") Then
'            Dim index As Integer = sortByColumn.LastIndexOf(".") + 1
'            tbl = sortByColumn.Substring(0, index)
'            sortByColumn = sortByColumn.Substring(index)
'        End If

'        Dim sb As New StringBuilder("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY ")
'        sb.Append(tbl).Append(sortByColumn)
'        If descending Then sb.Append(" DESC")
'        sb.Append(") AS _rownumber_, ")
'        sb.Append(selectColumns)
'        sb.Append(" FROM ")
'        sb.Append(tableName)
'        If Not tableName.ToLower.Contains(" join ") Then sb.Append(HINT)
'        sb.Append(WhereCriteria(criteria))
'        sb.Append(") AS Numbered")

'        Dim wherePaging As New CCriteriaList()
'        wherePaging.Add("_rownumber_", ESign.GreaterThan, pageIndexZeroBased * pageSize)
'        wherePaging.Add("_rownumber_", ESign.LessThanOrEq, (1 + pageIndexZeroBased) * pageSize)
'        sb.Append(WhereCriteria(wherePaging))

'        sb.Append(" ORDER BY ")
'        sb.Append(sortByColumn)
'        If descending Then sb.Append(" DESC")

'        'Parameterised Query
'        Dim cmd As IDbCommand = Command(sb.ToString, txOrNull)
'        Parameters(cmd, criteria)
'        Parameters(cmd, wherePaging)
'        Return ExecuteQuery(cmd, type)
'    End Function
'#End Region

'#Region "Version Info"
'    Private m_is2005OrGreater As Boolean? = Nothing
'    Public ReadOnly Property Is2005OrGreater() As Boolean
'        Get
'            If Not m_is2005OrGreater.HasValue Then
'                SyncLock (Me)
'                    If Not m_is2005OrGreater.HasValue Then
'                        'SQL 2008 returns "Microsoft SQL Server 2008 (RTM) - 10.0.1600.22 (Intel X86) Jul  9 2008 14:43:34  ..."
'                        'Sql(2005) : returns("Microsoft SQL Server 2005 - 9.00.3073.00 (Intel X86) Aug  5 2008 12:31:12 ...")
'                        'Sql(2000) : returns("Microsoft SQL Server  2000 - 8.00.2050 (Intel X86) Mar  7 2008 21:29:56 ...")
'                        Dim version As String = CStr(ExecuteScalar("SELECT @@VERSION", Nothing))
'                        m_is2005OrGreater = Not version.Contains("2000")
'                    End If
'                End SyncLock
'            End If
'            Return m_is2005OrGreater.Value
'        End Get
'    End Property
'#End Region

'End Class
