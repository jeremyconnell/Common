Imports System.Text

<CLSCompliant(True), Serializable()> _
Public MustInherit Class CDataSrcLocal : Inherits CDataSrc


#Region "Constructors"
    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
    End Sub
#End Region

#Region "Members"
    Private Shared m_parameterCache As New Dictionary(Of String, IDbDataParameter())
#End Region

#Region "Abstract/Virtual (Driver-Specific)"
    'Connection
    Public MustOverride Function Connection() As IDbConnection
    Protected Friend MustOverride Function CommandFactory(ByVal con As IDbConnection) As IDbCommand
    Protected MustOverride Sub DeriveParameters(ByVal cmd As IDbCommand)
    Public MustOverride Function DataAdapter(ByVal cmd As IDbCommand) As IDataAdapter

    'Parameter
    Public MustOverride Overloads Function Parameter(ByVal name As String, ByVal nativeValue As Object) As IDbDataParameter
    Public Overloads Function Parameter(ByVal i As CNameValue) As IDbDataParameter
        Return Parameter(i.MarkerName, NullValue(i.Value))
    End Function
    Public Overloads Function Parameter(ByVal i As CCriteria) As IDbDataParameter
        Return Parameter(i.MarkerName, i.ColumnValue)
    End Function

    'Parameterized queries
    Public Overridable Function Marker(ByVal name As String) As String    'name, as appears in the query text
        name = name.Replace(".", "_")
        name = name.Replace(" ", String.Empty)
        name = name.Replace("+", String.Empty)
        name = name.Replace("'", String.Empty)
        name = name.Replace("[", String.Empty)
        name = name.Replace("]", String.Empty)
        name = name.Replace("#", String.Empty)
        Return name
    End Function
    Public Overridable Function ParameterName(ByVal name As String) As String
        Return Marker(name) 'name, when parameter added to collection
    End Function
#End Region

#Region "Dataset"
    Public Overrides Function ExecuteDataSet(ByVal cmdProxy As CCommand) As DataSet
        Return ExecuteDataSet(Command(cmdProxy))
    End Function
    Protected Overloads Function ExecuteDataSet(ByVal cmd As IDbCommand) As DataSet
        If LOGGING Then Log(cmd)
        Dim closeConnection As Boolean = IsNothing(cmd.Transaction)

        Dim ds As New DataSet
        Try
            DataAdapter(cmd).Fill(ds)
            If closeConnection Then cmd.Connection.Close()
        Catch ex As Exception
            If closeConnection Then cmd.Connection.Close()
            Rethrow(ex, cmd.CommandText)
        End Try

        Return ds
    End Function
    Protected Function ExecuteAdapter(ByVal cmd As IDbCommand) As IDataAdapter
        Return DataAdapter(cmd)  'WARNING: doesn't close the connection (calling function must do this)
    End Function
    Protected Function FillDataSet(ByVal da As IDataAdapter) As DataSet
        Dim ds As New DataSet
        da.Fill(ds)
        Return ds
    End Function
#End Region

#Region "Execute - DataReader"
    Public Overloads Function ExecuteReader(ByVal sql As String) As IDataReader
        Return ExecuteReader(sql, CType(Nothing, IDbTransaction))
    End Function
    Public Overloads Function ExecuteReader(ByVal sql As String, ByVal txOrNull As IDbTransaction) As IDataReader
        Return ExecuteReader(New CCommand(sql, txOrNull))
    End Function
    Public Overloads Function ExecuteReader(ByVal spName As String, ByVal parameterValues As Object(), ByVal txOrNull As IDbTransaction) As IDataReader
        Return ExecuteReader(New CCommand(spName, parameterValues, txOrNull))
    End Function
    Public Overloads Function ExecuteReader(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, ByVal txOrNull As IDbTransaction) As IDataReader
        Return ExecuteReader(New CCommand(spName, pNamesAndValues, txOrNull))
    End Function
    Public Overloads Function ExecuteReader(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, isStoredProc As Boolean, ByVal txOrNull As IDbTransaction) As IDataReader
        Return ExecuteReader(New CCommand(spName, pNamesAndValues, isStoredProc, txOrNull))
    End Function
    Public Overloads Function ExecuteReader(ByVal cmdProxy As CCommand) As IDataReader
        Return ExecuteReader(Command(cmdProxy))
    End Function
    Protected Overloads Function ExecuteReader(ByVal cmd As IDbCommand) As IDataReader
        If LOGGING Then Log(cmd)
        Dim closeConnection As Boolean = IsNothing(cmd.Transaction)

        Try
            If closeConnection Then
                Return cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Else
                Return cmd.ExecuteReader()
            End If
        Catch ex As Exception
            If closeConnection Then cmd.Connection.Close()
            Rethrow(ex, cmd.CommandText)
            Return Nothing
        End Try
    End Function
#End Region

#Region "Select - DataReader"
    'SelectAll
    Public Function SelectAll_DataReader(ByVal tableName As String) As IDataReader
        Return SelectAll_DataReader(tableName, Nothing)
    End Function
    Public Function SelectAll_DataReader(ByVal tableName As String, ByVal orderBy As String) As IDataReader
        Return SelectAll_DataReader("*", tableName, orderBy)
    End Function
    Public Function SelectAll_DataReader(ByVal selectColumns As String, ByVal tableName As String, ByVal orderBy As String) As IDataReader
        Return CType(SelectAll(selectColumns, tableName, orderBy, EQueryReturnType.DataReader), IDataReader)
    End Function

    'SelectWhere
    Public Function SelectWhere_DataReader(ByVal tableName As String, ByVal criteria As CCriteriaList) As IDataReader
        Return SelectWhere_DataReader(tableName, Nothing, criteria)
    End Function
    Public Function SelectWhere_DataReader(ByVal tableName As String, ByVal orderBy As String, ByVal criteria As CCriteriaList) As IDataReader
        Return SelectWhere_DataReader("*", tableName, orderBy, criteria)
    End Function
    Public Function SelectWhere_DataReader(ByVal selectColumns As String, ByVal tableName As String, ByVal orderBy As String, ByVal criteria As CCriteriaList) As IDataReader
        Return CType(SelectWhere(selectColumns, tableName, criteria, orderBy, EQueryReturnType.DataReader), IDataReader)
    End Function
    Public Function SelectWhere_DataReader(ByVal selectColumns As String, ByVal tableName As String, ByVal orderBy As String, ByVal unsafeWhere As String) As IDataReader
        Return CType(SelectWhere(selectColumns, tableName, unsafeWhere, orderBy, EQueryReturnType.DataReader), IDataReader)
    End Function
#End Region

#Region "Scalar"
    Public Overloads Overrides Function ExecuteScalar(ByVal cmdProxy As CCommand) As Object
        Return ExecuteScalar(Command(cmdProxy))
    End Function
    Protected Overloads Function ExecuteScalar(ByVal cmd As IDbCommand) As Object
        If LOGGING Then Log(cmd)
        Dim closeConnection As Boolean = IsNothing(cmd.Transaction)

        Try
            ExecuteScalar = cmd.ExecuteScalar()
            If closeConnection Then cmd.Connection.Close()
        Catch ex As Exception
            If closeConnection Then cmd.Connection.Close()
            Rethrow(ex, cmd.CommandText)
            Return Nothing
        End Try
    End Function
#End Region

#Region "NonQuery"
    Public Overloads Overrides Function ExecuteNonQuery(ByVal cmdProxy As CCommand) As Integer
        Return ExecuteNonQuery(Command(cmdProxy))
    End Function
    Public Overloads Function ExecuteNonQuery(ByVal cmd As IDbCommand) As Integer
        If LOGGING Then Log(cmd)
        Dim closeConnection As Boolean = IsNothing(cmd.Transaction)

        Try
            ExecuteNonQuery = cmd.ExecuteNonQuery()
            If closeConnection Then cmd.Connection.Close()
        Catch ex As Exception
            If closeConnection Then cmd.Connection.Close()
            Rethrow(ex, cmd.CommandText)
            Return Nothing
        End Try
    End Function
#End Region

#Region "Insert"
    'Execute Insert (and retrieve the new identity)
    Public Function ExecuteInsert(ByVal sql As String, ByVal txOrNull As IDbTransaction) As Integer
        Return ExecuteInsert(Command(sql, txOrNull))
    End Function
    Public Function ExecuteInsert(ByVal cmd As IDbCommand) As Integer
        If LOGGING Then Log(cmd)
        Dim closeConnection As Boolean = IsNothing(cmd.Transaction)

        Try
            'Execute insert, check rowsAffected
            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
            If 0 = rowsAffected Then
                If closeConnection Then cmd.Connection.Close()
                Throw New Exception("0 rows affected after insert: ")
            End If

            'Get the new identity value
            If TypeOf Me Is COracleClient Then
                cmd.CommandText = "SELECT MAX(PrimaryKeyName) FROM TableName"
            ElseIf Me.IsPostGres Then
                cmd.CommandText = "SELECT lastval()"
            Else
                cmd.CommandText = "SELECT @@identity"
            End If
            cmd.Parameters.Clear()

            'Close connection (unless bulksave op)
            Return Integer.Parse(ExecuteScalar(cmd).ToString)
        Catch ex As Exception
            If closeConnection Then cmd.Connection.Close()
            Rethrow(ex, cmd.CommandText)
            Return Nothing
        End Try
    End Function
#End Region

#Region "Bulk Methods (Transactional)"
    Public Overrides Sub BulkSaveDelete(ByVal saves As ICollection, ByVal deletes As ICollection, ByVal txIsolation As IsolationLevel)
        If IsNothing(saves) Then saves = New List(Of CBase)(0)
        If IsNothing(deletes) Then deletes = New List(Of CBase)(0)
        If saves.Count = 0 And deletes.Count = 0 Then Exit Sub

        'Old code: Start a normal transaction
        'Dim con As IDbConnection = Connection()
        'Dim tx As IDbTransaction
        'If txIsolation = IsolationLevel.Unspecified Then
        '    tx = con.BeginTransaction()
        'Else
        '    tx = con.BeginTransaction(txIsolation)
        'End If

        'New code: 2 different types of transactions are now supported...
        Dim con As IDbConnection = Nothing
        Dim tx As IDbTransaction = Nothing
        If Not IsNothing(CImplicitTransaction.Current) Then
            'Alternative approach to transactions
            tx = CImplicitTransaction.Current.Transaction
        Else
            'Standard approach to transactions
            con = Connection()
            If txIsolation = IsolationLevel.Unspecified Then
                tx = con.BeginTransaction()
            Else
                tx = con.BeginTransaction(txIsolation)
            End If
        End If

        Try
            'Main Loop
            For Each i As CBase In saves
                i.Save(tx)
            Next
            For Each i As CBase In deletes
                i.Delete(tx)
            Next

            'Commit:

            'Old code:
            'tx.Commit()
            'con.Close()

            'New code:
            If IsNothing(CImplicitTransaction.Current) Then
                tx.Commit()
                con.Close()
            End If
        Catch ex As Exception
            'Rollback

            'Old code:
            'tx.Rollback()
            'con.Close()

            'New Code: exceptions, rollback handled further up
            If IsNothing(CImplicitTransaction.Current) Then
                tx.Rollback()
                con.Close()
            End If

            Throw
        End Try
    End Sub
#End Region

#Region "DataReader/DataSet Generalisation"
    Public Overloads Function ExecuteQuery(ByVal cmd As IDbCommand, ByVal type As EQueryReturnType) As Object
        If type = EQueryReturnType.DataSet Then
            Return ExecuteDataSet(cmd)
        Else
            Return ExecuteReader(cmd)
        End If
    End Function
    Public Overrides Function ExecuteQuery(ByVal cmd As CCommand, ByVal type As EQueryReturnType) As Object
        If type = EQueryReturnType.DataSet Then
            Return ExecuteDataSet(cmd)
        Else
            Return ExecuteReader(cmd)
        End If
    End Function
#End Region

#Region "Command Overloads"
    'Command proxy object
    Protected Function Command(ByVal cmd As CCommand) As IDbCommand
        With cmd
            Dim dbCmd As IDbCommand

            If .CommandType = ECmdType.Sql Then
                'No parameters
                dbCmd = Command(.Text, .Transaction)
            Else
                'Parameters
                If IsNothing(.ParametersUnnamed) Then
                    dbCmd = Command(.Text, .ParametersNamed, .Transaction)
                Else
                    dbCmd = Command(.Text, .ParametersUnnamed, .Transaction)
                End If

                'Parameterised sql vs. stored proc
                If .CommandType = ECmdType.ParameterisedSql Then
                    dbCmd.CommandType = CommandType.Text
                End If
            End If

            'Timeout
            If .TimeoutSecs <> dbCmd.CommandTimeout Then dbCmd.CommandTimeout = .TimeoutSecs

            Return dbCmd
        End With
    End Function


    'Supplied Transaction (optional)
    Protected Function Command(ByVal sql As String, ByVal txOrNull As IDbTransaction) As IDbCommand
        'Alternative transactions
        If IsNothing(txOrNull) AndAlso Not IsNothing(CImplicitTransaction.Current) Then txOrNull = CImplicitTransaction.Current.Transaction

        'Standard code
        If IsNothing(txOrNull) Then
            Return Command(sql)
        Else
            Dim cmd As IDbCommand = Command(txOrNull, sql)
            cmd.Transaction = txOrNull
            Return cmd
        End If
    End Function
    Protected Function Command(ByVal spName As String, ByVal parameterValues As Object(), ByVal txOrNull As IDbTransaction) As IDbCommand
        'Alternative transactions
        If IsNothing(txOrNull) AndAlso Not IsNothing(CImplicitTransaction.Current) Then txOrNull = CImplicitTransaction.Current.Transaction

        'Standard code
        If IsNothing(txOrNull) Then
            Return Command(spName, parameterValues)
        Else
            Dim cmd As IDbCommand = Command(txOrNull, spName, parameterValues)
            cmd.Transaction = txOrNull
            Return cmd
        End If
    End Function
    Protected Function Command(ByVal spName As String, ByVal parameterValues As CNameValueList, ByVal txOrNull As IDbTransaction) As IDbCommand
        'Alternative transactions
        If IsNothing(txOrNull) AndAlso Not IsNothing(CImplicitTransaction.Current) Then txOrNull = CImplicitTransaction.Current.Transaction

        'Standard code
        If IsNothing(txOrNull) Then
            Return Command(spName, parameterValues)
        Else
            Dim cmd As IDbCommand = Command(txOrNull, spName, parameterValues)
            cmd.Transaction = txOrNull
            Return cmd
        End If
    End Function

    'Supplied Transaction (Must be from the same connection)
    Protected Function Command(ByVal tx As IDbTransaction, ByVal spName As String, ByVal parameterValues As Object()) As IDbCommand
        Dim cmd As IDbCommand = Command(tx)
        cmd.CommandText = spName
        cmd.CommandType = CommandType.StoredProcedure
        ApplyParameters(cmd, parameterValues)
        Return cmd
    End Function
    Protected Function Command(ByVal tx As IDbTransaction, ByVal spName As String, ByVal parameterValues As CNameValueList) As IDbCommand
        Dim cmd As IDbCommand = Command(tx)
        cmd.CommandText = spName
        cmd.CommandType = CommandType.StoredProcedure
        ApplyParameters(cmd, parameterValues)
        Return cmd
    End Function
    Protected Function Command(ByVal tx As IDbTransaction, ByVal sql As String) As IDbCommand
        Dim cmd As IDbCommand = Command(tx)
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        Return cmd
    End Function
    Private Function Command(ByVal tx As IDbTransaction) As IDbCommand
        Dim cmd As IDbCommand = Command(tx.Connection)
        cmd.Transaction = tx
        Return cmd
    End Function

    'Low-level (previously abstract)
    Protected Function Command(ByVal con As IDbConnection) As IDbCommand
        Dim cmd As IDbCommand = CommandFactory(con)
        If Integer.MinValue <> CConfigBase.CommandTimeoutSecs Then
            cmd.CommandTimeout = CConfigBase.CommandTimeoutSecs
        End If
        Return cmd
    End Function
    Public Overridable Function Command(ByVal sql As String) As IDbCommand
        Dim cmd As IDbCommand = Me.Command(Connection())
        cmd.CommandText = sql
        cmd.CommandType = CommandType.Text
        Return cmd
    End Function
    Public Function Command(ByVal spName As String, ByVal parameterValues As Object()) As IDbCommand
        Dim cmd As IDbCommand = Me.Command(Connection())
        cmd.CommandText = spName
        cmd.CommandType = CommandType.StoredProcedure
        ApplyParameters(cmd, parameterValues)
        Return cmd
    End Function
    Public Function Command(ByVal spName As String, ByVal parameterNameValues As CNameValueList) As IDbCommand
        Dim cmd As IDbCommand = Me.Command(Connection())
        cmd.CommandText = spName
        cmd.CommandType = CommandType.StoredProcedure
        ApplyParameters(cmd, parameterNameValues)
        Return cmd
    End Function
#End Region

#Region "Parameters"
    'Named
    Protected Sub ApplyParameters(ByVal cmd As IDbCommand, ByVal pNamesAndValues As CNameValueList)
        If IsNothing(pNamesAndValues) Then Exit Sub
        For Each i As CNameValue In pNamesAndValues
            cmd.Parameters.Add(Parameter(i))
        Next
    End Sub
    Protected Sub ApplyParameters(ByVal cmd As IDbCommand, ByVal names As String(), ByVal values As Object())
        If IsNothing(names) Or IsNothing(values) Then Exit Sub
        Dim i As Integer
        For i = 0 To names.Length - 1
            cmd.Parameters.Add(Parameter(names(i), values(i)))
        Next
    End Sub
    'Unnamed
    Protected Sub ApplyParameters(ByVal cmd As IDbCommand, ByVal values As Object(), Optional ByVal includeReturnValueParameter As Boolean = False)
        If IsNothing(values) Then Exit Sub
        If values.Length = 0 Then Exit Sub

        Dim parameters As IDbDataParameter() = GetSpParameters(cmd, includeReturnValueParameter)
        If parameters.Length = 0 Then Exit Sub

        Dim i As IDbDataParameter
        For Each i In parameters
            i = CType(CType(i, ICloneable).Clone, IDbDataParameter)
            'If TypeOf i Is MySql.Data.MySqlClient.MySqlParameter Then
            '    If i.ParameterName.Substring(0, 1) <> "?" Then i.ParameterName = "?" & i.ParameterName
            'Else
            '    If i.ParameterName.Substring(0, 1) <> "@" Then i.ParameterName = "@" & i.ParameterName
            'End If

            cmd.Parameters.Add(i)
        Next

        Dim j As Integer
        For j = 0 To values.Length - 1
            CType(cmd.Parameters(j), IDbDataParameter).Value = NullValue(values(j))
        Next
    End Sub
    Protected Function GetSpParameters(ByVal cmd As IDbCommand, ByVal includeReturnValueParameter As Boolean) As IDbDataParameter()
        Dim hashKey As String
        With New StringBuilder(cmd.Connection.ConnectionString)
            .Append(":")
            .Append(cmd.CommandText)
            If includeReturnValueParameter Then .Append(":include ReturnValue Parameter")
            hashKey = .ToString
        End With

        Dim p As IDbDataParameter() = Nothing
        If m_parameterCache.TryGetValue(hashKey, p) Then Return p
        p = DiscoverSpParameterSet(cmd.CommandText, includeReturnValueParameter)
        m_parameterCache(hashKey) = p
        Return p
    End Function
    Private Function DiscoverSpParameterSet(ByVal spName As String, ByVal includeReturnValueParameter As Boolean) As IDbDataParameter()
        If Len(spName) = 0 Then Throw New ArgumentNullException("spName")

        Dim cmd As IDbCommand = Command(spName)
        cmd.CommandType = CommandType.StoredProcedure
        DeriveParameters(cmd)
        cmd.Connection.Close()

        If Not includeReturnValueParameter AndAlso Not IsMySql AndAlso cmd.Parameters.Count > 0 Then cmd.Parameters.RemoveAt(0)

        Dim discoveredParameters As IDbDataParameter() = {}
        ReDim discoveredParameters(cmd.Parameters.Count - 1)

        cmd.Parameters.CopyTo(discoveredParameters, 0)

        Dim i As IDbDataParameter
        For Each i In discoveredParameters
            i.Value = DBNull.Value
        Next
        Return discoveredParameters
    End Function
    Protected Sub SetParameterType(ByVal p As IDbDataParameter, ByVal value As Object)
        If IsNothing(value) Then Exit Sub
        If TypeOf value Is System.DBNull Then Exit Sub

        If TypeOf value Is Int32 Then
            p.DbType = DbType.Int32
        ElseIf TypeOf value Is String Then
            p.Size = Len(value)
            p.DbType = DbType.String
        ElseIf TypeOf value Is Boolean Then
            p.DbType = DbType.Boolean
        ElseIf TypeOf value Is DateTime Then
            p.DbType = DbType.DateTime
        ElseIf TypeOf value Is Double Then
            p.DbType = DbType.Double
        ElseIf TypeOf value Is Decimal Then
            p.DbType = DbType.Decimal
        ElseIf TypeOf value Is Guid Then
            p.DbType = DbType.Guid
        ElseIf TypeOf value Is Byte() Then
            p.DbType = DbType.Binary
        ElseIf TypeOf value Is Byte Then
            p.DbType = DbType.Byte
        ElseIf TypeOf value Is Int16 Then
            p.DbType = DbType.Int32
        ElseIf TypeOf value Is Int64 Then
            p.DbType = DbType.Int64
        End If
    End Sub
#End Region

#Region "Sql - Parameterised Queries"
    'Select Count
    Public Overrides Function SelectCount(ByVal where As CWhere) As Integer
        Dim selectWhere As New CSelectWhere("COUNT(*)", where, String.Empty)
        Dim obj As Object = ExecuteScalar(SelectWhereCmd(selectWhere))
        If TypeOf obj Is System.DBNull Then Return 0
        If TypeOf obj Is Integer Then Return CInt(obj)
        Return CInt(obj.ToString)
    End Function

    'Select
    Public Overrides Function [Select](ByVal where As CSelectWhere, ByVal type As EQueryReturnType) As Object
        Return ExecuteQuery(SelectWhereCmd(where), type)
    End Function
    Public Function SelectDataset(ByVal where As CSelectWhere) As DataSet
        Return CType(ExecuteQuery(SelectWhereCmd(where), EQueryReturnType.DataSet), DataSet)
    End Function
    Public Function SelectDataReader(ByVal where As CSelectWhere) As IDataReader
        Return CType(ExecuteQuery(SelectWhereCmd(where), EQueryReturnType.DataReader), IDataReader)
    End Function

    'Delete
    Public Overrides Function Delete(ByVal where As CWhere) As Integer
        With where
            Select Case .Type
                Case EWhereType.All
                    Return ExecuteNonQuery(DeleteAllSql(.TableName), .TxOrNull)

                Case EWhereType.Unsafe
                    Return ExecuteNonQuery(DeleteWhereSql(.TableName, .UnsafeWhereClause))

                Case EWhereType.Column
                    Return ExecuteNonQuery(DeleteWhereCmd(.TableName, .Criteria, .TxOrNull))
                Case EWhereType.Columns
                    Return ExecuteNonQuery(DeleteWhereCmd(.TableName, .CriteriaList, .TxOrNull))
                Case Else : Throw New Exception("WhereType not handled: " & .Type.ToString())
            End Select
        End With
    End Function


    'Insert/Update
    Public Overrides Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal txOrNull As IDbTransaction, ByVal oracleSequenceName As String) As Object
        'Note: parameter oracleSequenceName is only used by COracleClient (when insertPk=false)
        Dim cmd As IDbCommand = InsertCmd(tableName, pKeyName, insertPk, data, txOrNull) 'Open or Share connection
        If insertPk Then
            Return ExecuteNonQuery(cmd)
        Else
            Return ExecuteInsert(cmd)
        End If
    End Function
    Public Overrides Function Update(ByVal data As CNameValueList, ByVal where As CWhere) As Integer
        If data.Count = 0 Then Return 0
        Dim cmd As IDbCommand = UpdateCmd(where, data)
        Return ExecuteNonQuery(cmd)
    End Function

    Public Overrides Sub InsertId(ByVal tableName As String, ByVal data As CNameValueList, isId As Boolean)
        If Not isId Then
            'Simple case: no transaction
            ExecuteNonQuery(InsertCmd(tableName, String.Empty, True, data, Nothing))
        Else
            'Complex case: set identity on/off in tx
            Dim cn As IDbConnection = Connection()
            Dim tx As IDbTransaction = cn.BeginTransaction()
            Try
                SetIdentityInsert(True, tableName, tx)
                ExecuteNonQuery(InsertCmd(tableName, String.Empty, True, data, tx))
                SetIdentityInsert(False, tableName, tx)

                tx.Commit()
            Catch
                Throw
            Finally
                cn.Close()
            End Try
        End If

    End Sub

    Public Overrides Sub InsertId(ByVal tableName As String, ByVal bulk As List(Of CNameValueList), isId As Boolean)
        Dim cn As IDbConnection = Connection()
        Dim tx As IDbTransaction = cn.BeginTransaction()
        Try
            If isId Then SetIdentityInsert(True, tableName, tx)
            For Each data As CNameValueList In bulk
                ExecuteNonQuery(InsertCmd(tableName, String.Empty, True, data, tx))
            Next
            If isId Then SetIdentityInsert(False, tableName, tx)

            tx.Commit()
        Catch
            tx.Rollback()

            Try
                tx = cn.BeginTransaction()
                If Not isId Then SetIdentityInsert(True, tableName, tx)
                For Each data As CNameValueList In bulk
                    ExecuteNonQuery(InsertCmd(tableName, String.Empty, True, data, tx))
                Next
                If Not isId Then SetIdentityInsert(False, tableName, tx)
                tx.Commit()
            Catch
                tx.Rollback()
            End Try

            Throw
        Finally
            cn.Close()
        End Try
    End Sub


    'Ordinals
    Public Overrides Function UpdateOrdinals(ByVal tableName As String, ByVal primaryKeyName As String, ByVal ordinalName As String, ByVal ordinals As CNameValueList) As Integer
        If ordinals.Count = 0 Then Return 0

        Dim sql As New StringBuilder("UPDATE ")
        sql.Append(tableName)
        sql.Append(" SET ")
        sql.Append(ordinalName)
        sql.Append("=")
        sql.Append(Marker(ordinalName))
        sql.Append(Where(primaryKeyName))
        sql.Append("=")
        sql.Append(Marker(primaryKeyName))

        Dim rowsAffected As Integer = 0

        Dim cmd As IDbCommand = Command(sql.ToString())
        With cmd
            .Transaction = .Connection.BeginTransaction(IsolationLevel.ReadUncommitted)
            Try
                For Each i As CNameValue In ordinals
                    With .Parameters
                        .Clear()
                        .Add(Parameter(ordinalName, i.Value))
                        .Add(Parameter(primaryKeyName, i.Name))
                    End With
                    rowsAffected += .ExecuteNonQuery()
                Next
                .Transaction.Commit()
                .Connection.Close()
            Catch ex As Exception
                .Transaction.Rollback()
                .Connection.Close()
                Throw ex
            End Try
        End With
        Return rowsAffected
    End Function

    'Paging without filters (no parameters), returns datareader if local or dataset if remote
    'Don't need to override again, as can just override the sql statement (PagingSql)
    Public Overrides Function Paging(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        'Count the total records
        count = SelectCount(tableName, txOrNull)

        'Driver-specific paging sql for a paged select-all
        Dim sql As String = PagingSql(pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, count)

        'Validate page index (return simple query with no records if max page exceeded)
        If pageIndexZeroBased > (count / pageSize) Then sql = PagingSql(0, 0, tableName, descending, sortByColumn, selectColumns, count)

        Return ExecuteQuery(sql, txOrNull, type)
    End Function
    'Driver-specific paging sql (e.g. mysql uses "... LIMIT (a,b)", and sqlserver is faster using "...SELECT ROW_NUMBER() OVER...")
    Protected Overridable Function PagingSql(ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal count As Integer) As String
        Dim pageNum As Integer = pageIndexZeroBased + 1

        Dim sort1 As String = " DESC"
        Dim sort2 As String = " ASC"
        If Not descending Then
            Dim temp As String = sort1
            sort1 = sort2
            sort2 = temp
        End If

        If pageIndexZeroBased = 0 Then 'Simple query for the first page
            Return String.Concat("SELECT TOP ", pageSize, " * FROM ", tableName, " ORDER BY ", sortByColumn, sort1)
        End If

        'If on the last page then reduce the pagesize
        Dim pageSizeActual As Integer = pageSize
        If pageNum * pageSize > count Then
            pageSizeActual = count - (pageIndexZeroBased * pageSize)
        End If

        Return String.Concat("SELECT b.* FROM (SELECT TOP ", pageSizeActual, " a.* FROM (SELECT TOP ", pageNum * pageSize, " * FROM ", tableName, " ORDER BY ", sortByColumn, sort1, ") a	ORDER BY ", sortByColumn, sort2, ") b ORDER BY ", sortByColumn, sort1)
    End Function

    'Paging with filters (parameterised), returns datareader if local or dataset if remote
    Public Overrides Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        'Count the total records
        count = SelectCount(tableName, criteria, txOrNull)

        'Validate page index
        If pageIndexZeroBased > (count / pageSize) Then pageIndexZeroBased = 0

        'Same logic as CDataSrc.Paging
        Dim pageNum As Integer = pageIndexZeroBased + 1
        Dim sort1 As String = " DESC"
        Dim sort2 As String = " ASC"
        If Not descending Then
            Dim temp As String = sort1
            sort1 = sort2
            sort2 = temp
        End If

        Dim startBit As String
        Dim endBit As String
        If pageIndexZeroBased = 0 Then
            startBit = String.Concat("SELECT TOP ", pageSize, " ", selectColumns, " FROM ", tableName)
            endBit = String.Concat(" ORDER BY ", sortByColumn, sort1)
        Else
            'If on the last page then reduce the pagesize
            Dim pageSizeActual As Integer = pageSize
            If (pageIndexZeroBased + 1) * pageSize > count Then
                pageSizeActual = count - (pageIndexZeroBased * pageSize)
            End If

            startBit = String.Concat("SELECT b.* FROM (SELECT TOP ", pageSizeActual, " a.* FROM (SELECT TOP ", pageNum * pageSize, " ", selectColumns, " FROM ", tableName)
            endBit = String.Concat(" ORDER BY ", sortByColumn, sort1, ") a ORDER BY ", sortByColumn, sort2, ") b ORDER BY ", sortByColumn, sort1)
        End If

        'Parameterised Query
        Dim sql As New StringBuilder(startBit)
        sql.Append(Me.WhereCriteria(criteria).ToString)
        sql.Append(endBit)

        Dim cmd As IDbCommand = Command(sql.ToString, txOrNull)
        Parameters(cmd, criteria)
        Return ExecuteQuery(cmd, type)
    End Function
#End Region

#Region "Sql - Private"
    Friend Function InsertCmd(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal txOrNull As IDbTransaction) As IDbCommand
		Dim sql As New StringBuilder("INSERT INTO ")
		sql.Append(tableName)
		sql.Append(" ")
		Dim names As New StringBuilder, values As New StringBuilder
		For Each i As CNameValue In data
			If 0 = String.Compare(i.Name, pKeyName, True) Then
				If Not insertPk Then Continue For
			End If
			If names.Length > 0 Then names.Append(", ")
			If values.Length > 0 Then values.Append(", ")
            names.Append("[").Append(i.Name).Append("]")
            values.Append(Marker(i.Name))
		Next
		sql.Append("(")
		sql.Append(names)
		sql.Append(") VALUES (")
		sql.Append(values)
		sql.Append(")")

		Dim cmd As IDbCommand
		If IsNothing(txOrNull) Then cmd = Command(sql.ToString) Else cmd = Command(txOrNull, sql.ToString)
		For Each i As CNameValue In data
			cmd.Parameters.Add(Parameter(i))
		Next
		'cmd.Prepare()
		Return cmd
	End Function
	Private Function UpdateCmd(ByVal where As CWhere, ByVal data As CNameValueList) As IDbCommand
        With where
            'Make sure the markers are unique, since there are 2 separate collections
            If .Type = EWhereType.Columns Then
                UniqueMarkerNames(.CriteriaList, data)
            ElseIf .Type = EWhereType.Column Then
                UniqueMarkerNames(.Criteria, data)
            End If

            'Data expression
            Dim sql As New StringBuilder("UPDATE ")
            sql.Append(.TableName)
            sql.Append(" SET ")

            Dim pairs As New StringBuilder
            For Each i As CNameValue In data
                If pairs.Length > 0 Then pairs.Append(", ")
                pairs.Append("[").Append(i.Name).Append("]")
                pairs.Append("=")
                pairs.Append(Marker(i.MarkerName))
            Next

            sql.Append(pairs)

            'Where expression
            Select Case .Type
                Case EWhereType.Column
                    sql.Append(WhereSign(.Criteria))
                Case EWhereType.Columns
                    sql.Append(WhereCriteria(.CriteriaList))
                Case EWhereType.Unsafe
                    sql.Append(Me.Where(.UnsafeWhereClause))
            End Select

            'Command
            Dim cmd As IDbCommand
            If IsNothing(.TxOrNull) Then cmd = Command(sql.ToString) Else cmd = Command(.TxOrNull, sql.ToString)

            'Data parameters
            For Each i As CNameValue In data
                cmd.Parameters.Add(Parameter(i))
            Next

            'Where parameters
            If Not IsNothing(.Criteria) Then
                Parameter(cmd, .Criteria)
            End If
            If Not IsNothing(.CriteriaList) Then
                Parameters(cmd, .CriteriaList)
            End If

            'cmd.Prepare()
            Return cmd
        End With

    End Function
    Private Function SelectWhereCmd(ByVal where As CSelectWhere) As IDbCommand
        With where
            Select Case .Type
                Case EWhereType.All
                    Return Command(SelectAllSql(.SelectCols, .TableName, .OrderBy), .TxOrNull)

                Case EWhereType.Column
                    Return SelectWhereCmd(.SelectCols, .TableName, .Criteria, .OrderBy, .TxOrNull)

                Case EWhereType.Columns
                    Return SelectWhereCmd(.SelectCols, .TableName, .CriteriaList, .OrderBy, .TxOrNull)

                Case EWhereType.Unsafe
                    Return SelectWhereCmd(.SelectCols, .TableName, .UnsafeWhereClause, .OrderBy, .TxOrNull)
                Case Else
                    Throw New Exception("WhereType not handled: " & .Type.ToString())
            End Select
        End With
    End Function
    Private Function SelectAllSql(ByVal selectCols As String, ByVal viewName As String, ByVal orderByCols As String) As String
        Dim sql As New StringBuilder(Me.SelectSql(selectCols, viewName))
        sql.Append(OrderBy(orderByCols))
        Return sql.ToString
    End Function
    Private Function SelectWhereCmd(ByVal selectCols As String, ByVal viewName As String, ByVal c As CCriteria, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction) As IDbCommand
        Dim sql As New StringBuilder(Me.SelectSql(selectCols, viewName))
        sql.Append(WhereSign(c))
        sql.Append(OrderBy(orderByCols))

        Dim cmd As IDbCommand = Command(sql.ToString, txOrNull)
        Parameter(cmd, c)
        'cmd.Prepare()
        Return cmd
    End Function
    Private Function SelectWhereCmd(ByVal selectCols As String, ByVal viewName As String, ByVal criteria As CCriteriaList, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction) As IDbCommand
        Dim sql As New StringBuilder(Me.SelectSql(selectCols, viewName))
        sql.Append(WhereCriteria(criteria))
        sql.Append(OrderBy(orderByCols))

        Dim cmd As IDbCommand = Command(sql.ToString, txOrNull)
        Parameters(cmd, criteria)
        'cmd.Prepare()
        Return cmd
    End Function
    Private Function SelectWhereCmd(ByVal selectCols As String, ByVal viewName As String, ByVal unsafeWhereClause As String, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction) As IDbCommand
        Dim sql As New StringBuilder(Me.SelectSql(selectCols, viewName))
        sql.Append(Where(unsafeWhereClause))
        sql.Append(OrderBy(orderByCols))
        Return Command(sql.ToString, txOrNull)
    End Function
    Private Function SelectSql(ByVal selectCols As String, ByVal viewName As String) As String
        Dim s As New StringBuilder("SELECT ")
        s.Append(selectCols)
        s.Append(" FROM ")
        s.Append(viewName)
        Return s.ToString
    End Function


    Private Function DeleteAllSql(ByVal tableName As String) As String
        Return DeleteSql(tableName).ToString
    End Function
    Private Function DeleteWhereSql(ByVal tableName As String, ByVal unsafeWhereClause As String) As String
        Dim sql As StringBuilder = Me.DeleteSql(tableName)
        sql.Append(Where(unsafeWhereClause))
        Return sql.ToString
    End Function
    Private Function DeleteWhereCmd(ByVal tableName As String, ByVal c As CCriteria, ByVal txOrNull As IDbTransaction) As IDbCommand
        Dim sql As StringBuilder = Me.DeleteSql(tableName)
        sql.Append(WhereSign(c))

        Dim cmd As IDbCommand = Command(sql.ToString, txOrNull)
        If c.Sign <> ESign.IN AndAlso c.Sign <> ESign.NotIn Then cmd.Parameters.Add(Parameter(c.ColumnName, c.ColumnValue))
        Return cmd
    End Function
    Private Function DeleteWhereCmd(ByVal tableName As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction) As IDbCommand
        Dim sql As StringBuilder = Me.DeleteSql(tableName)
        sql.Append(WhereCriteria(criteria))

        Dim cmd As IDbCommand = Command(sql.ToString, txOrNull)
        Parameters(cmd, criteria)
        Return cmd
    End Function
    Private Function DeleteSql(ByVal tableName As String) As StringBuilder
        Dim s As New StringBuilder("DELETE FROM ")
        s.Append(tableName)
        Return s
    End Function


    Private Function WhereEq(ByVal colName As String, ByVal colValue As Object) As StringBuilder
        Return WhereSign(colName, ESign.EqualTo, colValue)
    End Function
    Private Function WhereSign(ByVal c As CCriteria) As StringBuilder
        Return WhereSign(c.ColumnName, c.Sign, c.ColumnValue)
    End Function
    Private Function WhereSign(ByVal colName As String, ByVal sqlSign As ESign, ByVal colValue As Object) As StringBuilder
        Dim sb As New StringBuilder(" WHERE ")
        Dim marker As String = colName
        AppendWhere(sb, colName, sqlSign, colValue, marker)
        Return sb
    End Function
    Private Function WhereCriteria(ByVal filters As CNameValueList) As StringBuilder
        Return WhereCriteria(New CCriteriaList(filters))
    End Function
    Private Function WhereCriteria(ByVal filters As CNameValueList, ByVal boolOperator As EBoolOperator) As StringBuilder
        Return WhereCriteria(New CCriteriaList(filters, boolOperator))
    End Function
    Public Function WhereCriteria(ByVal c As CCriteriaList) As StringBuilder
        If c.Count = 0 Then Return New StringBuilder

        UniqueMarkerNames(c)

        Dim s As New StringBuilder(" WHERE ")
        For Each i As CCriteria In c
            If TypeOf i Is CCriteriaGroup AndAlso CType(i, CCriteriaGroup).Group.Count = 0 Then Continue For

            If s.Length > 7 Then
                If c.BoolOperator = EBoolOperator.And Then
                    s.Append(" AND ")
                Else
                    s.Append(" OR ")
                End If
            End If

            AppendWhere(s, i)
        Next
        If s.Length = 7 Then Return New StringBuilder
        Return s
    End Function

    Private Sub AppendWhere(ByVal s As StringBuilder, ByVal colName As String, ByVal sqlSign As ESign, ByVal colValue As Object, ByVal markerName As String)
        'Null values
        colValue = CDataSrc.NullValue(colValue)

        'Name/Sign/Value
        If sqlSign = ESign.IN Then
            Dim inExpr As String = ProcessInExpression(colValue)
            If String.IsNullOrEmpty(inExpr) Then
                s.Append("1=0")
            Else
                s.Append(colName)
                s.Append(Sign(sqlSign, colValue))
                s.Append(inExpr)
            End If
        ElseIf sqlSign = ESign.NotIn Then
            Dim inExpr As String = ProcessInExpression(colValue)
            If String.IsNullOrEmpty(inExpr) Then
                s.Append("1=1")
            Else
                s.Append(colName)
                s.Append(Sign(sqlSign, colValue))
                s.Append(inExpr)
            End If
        Else
            s.Append(colName)
            s.Append(Sign(sqlSign, colValue))
            If TypeOf colValue Is DBNull Then
                s.Append("NULL")
            Else
                s.Append(Marker(markerName))
            End If
        End If
    End Sub
    Private Sub AppendWhere(ByVal s As StringBuilder, ByVal i As CCriteria)
        'Expression in parenthesis
        If TypeOf i Is CCriteriaGroup Then
            AppendWhereGroup(s, CType(i, CCriteriaGroup))
            Exit Sub
        End If

        'Name/Sign/Value
        AppendWhere(s, i.ColumnName, i.Sign, i.ColumnValue, i.MarkerName)
    End Sub
    Private Sub AppendWhereGroup(ByVal s As StringBuilder, ByVal group As CCriteriaGroup)
        With group
            If .Group.Count = 0 Then Exit Sub

            s.Append("(")
            For Each i As CCriteria In .Group
                If .Group.IndexOf(i) > 0 Then
                    If .Logic = EBoolOperator.And Then
                        s.Append(" AND ")
                    Else
                        s.Append(" OR ")
                    End If
                End If

                AppendWhere(s, i)
            Next
            s.Append(")")
        End With
    End Sub
    Private Function Sign(ByVal s As ESign, ByVal nulledValue As Object) As String
        Select Case s
            Case ESign.EqualTo : If TypeOf nulledValue Is DBNull Then Return " IS " Else Return "="
            Case ESign.GreaterThan : Return ">"
            Case ESign.GreaterThanOrEq : Return ">="
            Case ESign.LessThan : Return "<"
            Case ESign.LessThanOrEq : Return "<="
            Case ESign.Like : Return " LIKE "
            Case ESign.NotEqualTo : If TypeOf nulledValue Is DBNull Then Return " IS NOT " Else Return "<>"
            Case ESign.IN : Return " IN "
            Case ESign.NotIn : Return " NOT IN "
            Case Else
                Throw New Exception("Unsupported sign: " & s.ToString())
        End Select
    End Function
    Private Function Where(ByVal unsafeWhereClause As String) As StringBuilder
        Dim s As New StringBuilder(unsafeWhereClause)
        If s.Length > 0 Then
            s.Insert(0, " WHERE ")
        End If
        Return s
    End Function
    Private Function OrderBy(ByVal columnNames As String) As StringBuilder
        Dim s As New StringBuilder(columnNames)
        If s.Length > 0 Then s.Insert(0, " ORDER BY ")
        Return s
    End Function
    Protected Sub Parameters(ByVal cmd As IDbCommand, ByVal criteria As CCriteriaList)
        For Each i As CCriteria In criteria
            Parameter(cmd, i)
        Next
        For Each i As CNameValue In criteria.Parameters
            cmd.Parameters.Add(Parameter(i.MarkerName, i.Value))
        Next
    End Sub
    Protected Overloads Sub Parameter(ByVal cmd As IDbCommand, ByVal i As CCriteria)
        If TypeOf i Is CCriteriaGroup Then
            Parameters(cmd, CType(i, CCriteriaGroup).Group)
        Else
            If i.Sign <> ESign.IN AndAlso i.Sign <> ESign.NotIn Then 'Not parameterised
                cmd.Parameters.Add(Parameter(i.MarkerName, i.ColumnValue))
            End If
        End If
    End Sub

    Private Sub UniqueMarkerNames(ByVal criteria As CCriteria, ByVal data As CNameValueList)
        Dim list As New List(Of String)(1 + data.Count)
        UniqueMarkerNames(list, criteria)
        UniqueMarkerNames(list, data)
    End Sub
    Private Sub UniqueMarkerNames(ByVal criteria As CCriteriaList, ByVal data As CNameValueList)
        Dim list As New List(Of String)(criteria.Count + data.Count + criteria.Parameters.Count)
        UniqueMarkerNames(list, criteria)
        UniqueMarkerNames(list, data)
        UniqueMarkerNames(list, criteria.Parameters)
    End Sub
    Private Sub UniqueMarkerNames(ByVal criteria As CCriteriaList)
        UniqueMarkerNames(New List(Of String)(criteria.Count), criteria)
    End Sub
    Private Sub UniqueMarkerNames(ByVal unique As List(Of String), ByVal criteria As CCriteriaList)
        For Each i As CCriteria In criteria
            If TypeOf i Is CCriteriaGroup Then
                UniqueMarkerNames(unique, CType(i, CCriteriaGroup).Group)
                Continue For
            End If

            UniqueMarkerNames(unique, i)
        Next
    End Sub
    Private Sub UniqueMarkerNames(ByVal unique As List(Of String), ByVal data As CNameValueList)
        For Each i As CNameValue In data
            UniqueMarkerNames(unique, i)
        Next
    End Sub
    Private Sub UniqueMarkerNames(ByVal unique As List(Of String), ByVal c As CCriteria)
        Dim temp As String = c.ColumnName.ToLower
        If Not String.IsNullOrEmpty(c.MarkerName) Then temp = c.MarkerName.ToLower
        temp = EscapeForSqlParam(temp)
        While unique.Contains(temp)
            temp &= "_"
        End While
        unique.Add(temp)
        c.MarkerName = temp
    End Sub
    Private Sub UniqueMarkerNames(ByVal unique As List(Of String), ByVal nv As CNameValue)
        Dim temp As String = nv.Name.ToLower
        If Not String.IsNullOrEmpty(nv.MarkerName) Then temp = nv.MarkerName.ToLower
        temp = EscapeForSqlParam(temp)
        While unique.Contains(temp)
            temp &= "_"
        End While
        unique.Add(temp)
        nv.MarkerName = temp
    End Sub

    Private Function EscapeForSqlParam(ByVal name As String) As String
        Return name.ToLower.Replace(",", "").Replace("(", "").Replace(")", "").Replace(".", "")
    End Function

    Private Function ProcessInExpression(ByVal value As Object) As String
        If TypeOf value Is String Then Return CStr(value)
        Dim sb As New StringBuilder()
        If TypeOf (value) Is Integer() OrElse TypeOf value Is List(Of Integer) Then
            'Most common case
            For Each i As Object In CType(value, IList)
                If sb.Length > 0 Then sb.Append(",")
                sb.Append(i)
            Next
        ElseIf TypeOf value Is List(Of Guid) OrElse TypeOf value Is List(Of String) Or TypeOf (value) Is String() Or TypeOf value Is Guid() Then
            'Things that need single quotes array or generics
            For Each i As Object In CType(value, IList)
                If sb.Length > 0 Then sb.Append(",")
                If i Is System.DBNull.Value Then
                    sb.Append("NULL")
                Else
                    sb.Append("'").Append(i.ToString().Replace("'", "''")).Append("'")
                End If
            Next
        ElseIf TypeOf value Is IList Then
            'Hopefully dont need quotes, e.g. double, real, short
            For Each i As Object In CType(value, IList)
                If sb.Length > 0 Then sb.Append(",")
                If i Is System.DBNull.Value Then
                    sb.Append("NULL")
                ElseIf TypeOf i Is Guid OrElse TypeOf i Is String Then
                    sb.Append("'").Append(i.ToString().Replace("'", "''")).Append("'")
                Else
                    sb.Append(i)
                End If
            Next
        Else
            Return value.ToString
        End If

        If sb.Length = 0 Then Return String.Empty
        sb.Insert(0, "(")
        sb.Append(")")
        Return sb.ToString
    End Function
#End Region

End Class
