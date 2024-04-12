Imports System
Imports System.Web
Imports System.Web.HttpContext
Imports System.Text
Imports System.Configuration
Imports System.Configuration.ConfigurationManager
Imports System.Threading.Tasks

<CLSCompliant(True)>
Public Enum EQueryReturnType
    Optimal = 1
    DataReader = 2
    DataSet = 3
End Enum


<CLSCompliant(True)>
Public MustInherit Class CDataSrc

#Region "Constants"
    Public Shared LOGGING As Boolean = False 'CApplication.IsWebApplication
#End Region


#Region "Factory"
    'Types
    Public Delegate Function DFactoryLoc(cs As String) As CDataSrc
    Public Delegate Function DFactoryWeb(url As String, password As String) As CDataSrc

    'Dict
    Protected Shared m_factoryLoc As Dictionary(Of String, DFactoryLoc)
    Protected Shared m_factoryWeb As Dictionary(Of String, DFactoryWeb)


    Shared Sub New()
        'Webservice Types
        Dim soap As New DFactoryWeb(Function(cs As String, pass As String) New CWebSrcSoap(cs, pass))
        Dim bin As New DFactoryWeb(Function(cs As String, pass As String) New CWebSrcBinary(cs, pass))

        'Local Types
        Dim oledb As New DFactoryLoc(Function(cs As String) New COleDb(cs))
        Dim odbc As New DFactoryLoc(Function(cs As String) New COdbc(cs))
        Dim sqlServ As New DFactoryLoc(Function(cs As String) New CSqlClient(cs))

        '*** Mysql, OracleOdp, Postgres => Seperate Projs


        'Webservice Dict
        m_factoryWeb = New Dictionary(Of String, DFactoryWeb)
        m_factoryWeb.Add("webservice", soap)
        m_factoryWeb.Add("webpage", bin)
        m_factoryWeb.Add("websrc", bin)
        m_factoryWeb.Add("website", bin)

        'Local Dict
        m_factoryLoc = New Dictionary(Of String, DFactoryLoc)
        m_factoryLoc.Add("oledb", oledb)
        m_factoryLoc.Add("odbc", odbc)
        m_factoryLoc.Add("sqlclient", sqlServ)
        m_factoryLoc.Add("sqlserver", sqlServ)
        m_factoryLoc.Add("sql", sqlServ)


    End Sub

    Public Sub ShiftDates(dt As DataTable)
        If Me.Offset.TotalHours = 0 Then Exit Sub

        Dim list As New List(Of DataColumn)
        For Each i As DataColumn In dt.Columns
            If i.DataType Is GetType(DateTime) Then list.Add(i)
        Next

        If list.Count = 0 Then Exit Sub
        For Each i As DataRow In dt.Rows
            For Each j As DataColumn In list
                Dim d As Object = i(j)
                If TypeOf d Is DateTime Then
                    i(j) = CType(d, DateTime).Subtract(Me.Offset)
                End If
            Next
        Next
    End Sub
#End Region

#Region "Constructors"
    Public Sub New(ByVal connectionString As String)
        m_connectionString = connectionString
    End Sub
#End Region

#Region "Members"
    Protected m_connectionString As String
#End Region

#Region "MustOverride"
    'Tweaks
    Public Overridable ReadOnly Property IsMySql As Boolean
        Get
            Return False
        End Get
    End Property
    Protected Overridable ReadOnly Property IsPostGres As Boolean
        Get
            Return False
        End Get
    End Property



    'Low-Level: Core Methods
    Public MustOverride Function ExecuteDataSet(ByVal cmd As CCommand) As DataSet
    Public MustOverride Function ExecuteScalar(ByVal cmd As CCommand) As Object
    Public MustOverride Function ExecuteNonQuery(ByVal cmd As CCommand) As Integer

    'DataReader/DataSet Generalisation
    Public MustOverride Function ExecuteQuery(ByVal cmd As CCommand, ByVal type As EQueryReturnType) As Object

    'High-Level: Transactional
    Public MustOverride Sub BulkSaveDelete(ByVal saves As ICollection, ByVal deletes As ICollection, ByVal il As IsolationLevel)

    'Sql-Server specific
    Public Overridable Sub BulkInsert(ByVal dt As DataTable, ByVal tableName As String, ByVal mappings As Dictionary(Of Integer, Integer))
        Throw New Exception("BulkInsert is only supported on SqlServer (or sql via webservice)")
    End Sub
    Public Overridable Sub BulkInsertWithTx(ByVal dt As DataTable, ByVal tableName As String, ByVal mappings As Dictionary(Of Integer, Integer))
        Throw New Exception("BulkInsert is only supported on SqlServer (or sql via webservice)")
    End Sub
    Public Overridable Async Function BulkInsertAsync(ByVal dt As DataTable, ByVal tableName As String, ByVal mappings As Dictionary(Of Integer, Integer), keepIdentity As Boolean) As Task
        Await Task.Delay(1)
        Throw New Exception("BulkInsertAsync is only supported on SqlServer (or sql via webservice)")
    End Function


    Public Overridable Async Function ExecuteNonQueryAsync(ByVal cmd As CCommand) As Task(Of Integer)
        Await Task.Delay(1)
        Throw New Exception("ExecuteNonQueryAsync is only supported on SqlServer")
    End Function
    Public Overridable Async Function ExecuteScalarAsync(ByVal cmd As CCommand) As Task(Of Object)
        Await Task.Delay(1)
        Throw New Exception("ExecuteScalarAsync is only supported on SqlServer")
    End Function

    'Overloads
    Public Sub BulkInsert(ByVal dt As DataTable)
        BulkInsert(dt, dt.TableName, Nothing)
    End Sub
    Public Sub BulkInsert(ByVal dt As DataTable, ByVal tableName As String)
        BulkInsert(dt, tableName, Nothing)
    End Sub
    Public Sub BulkInsertOffsetBy1(ByVal dt As DataTable, ByVal tableName As String)
        BulkInsert(dt, tableName, OffsetBy1(dt))
    End Sub
    Protected Function OffsetBy1(ByVal dt As DataTable) As Dictionary(Of Integer, Integer)
        Dim dict As New Dictionary(Of Integer, Integer)(dt.Columns.Count)
        For i As Integer = 0 To dt.Columns.Count - 1
            dict.Add(i, i + 1)
        Next
        Return dict
    End Function

    'Identity Insert
    Public Sub BulkInsertWithTx(ByVal dt As DataTable, ByVal tableName As String)
        Dim mappings As Dictionary(Of Integer, Integer) = Nothing
        BulkInsertWithTx(dt, tableName, Nothing)
    End Sub
    Public Sub BulkInsertWithTxOffsetBy1(ByVal dt As DataTable, ByVal tableName As String)
        BulkInsertWithTx(dt, tableName, OffsetBy1(dt))
    End Sub
    Public Overridable Function BulkInsertWithTx(ByVal rowsToInsert As List(Of CNameValueList), ByVal tableName As String, primaryKey As String, isIdentity As Boolean) As List(Of Object)
        Dim cn As IDbConnection = CDataSrc.Default.Local.Connection
        Dim tx As IDbTransaction = cn.BeginTransaction()
        Dim list As New List(Of Object)
        Try

            If isIdentity Then Me.SetIdentityInsert(True, tableName, tx)

            For Each i As CNameValueList In rowsToInsert
                list.Add(Me.Insert(tableName, primaryKey, True, i, tx, Nothing))
            Next
            If isIdentity Then Me.SetIdentityInsert(False, tableName, tx)

            tx.Commit()
        Catch ex As Exception
            tx.Rollback()
            Throw ex
        Finally
            cn.Close()
        End Try
        Return list
    End Function


    'High-Level: Parameterised Sql
    Public MustOverride Sub InsertId(ByVal tableName As String, ByVal data As CNameValueList, isId As Boolean)
    Public MustOverride Sub InsertId(ByVal tableName As String, ByVal data As List(Of CNameValueList), isId As Boolean)
    Public MustOverride Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal txOrNull As IDbTransaction, ByVal oracleSequenceName As String) As Object
    Public MustOverride Function Update(ByVal data As CNameValueList, ByVal where As CWhere) As Integer
    Public MustOverride Function UpdateOrdinals(ByVal tableName As String, ByVal pKeyName As String, ByVal ordinalName As String, ByVal data As CNameValueList) As Integer
    Public MustOverride Function Delete(ByVal where As CWhere) As Integer
    Public MustOverride Function [Select](ByVal where As CSelectWhere, ByVal type As EQueryReturnType) As Object
    Public MustOverride Function SelectCount(ByVal where As CWhere) As Integer
    Public MustOverride Function Paging(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
    Public MustOverride Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object

    'Non-transactional overloads
    Public Function Paging(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String) As Object
        Return Paging(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, Nothing, EQueryReturnType.Optimal)
    End Function
    Public Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList) As Object
        Return PagingWithFilters(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, criteria, Nothing, EQueryReturnType.Optimal)
    End Function

    'Dataset
    Public Function Paging_Dataset(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String) As DataSet
        Return CType(Paging(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, Nothing, EQueryReturnType.DataSet), DataSet)
    End Function
    Public Function PagingWithFilters_Dataset(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList) As DataSet
        Return CType(PagingWithFilters(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, criteria, Nothing, EQueryReturnType.DataSet), DataSet)
    End Function

    'DataReader
    Public Function Paging_DataReader(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal txOrNull As IDbTransaction) As IDataReader
        Return CType(Paging(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, txOrNull, EQueryReturnType.DataReader), IDataReader)
    End Function
    Public Function PagingWithFilters_DataReader(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction) As IDataReader
        Return CType(PagingWithFilters(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, criteria, txOrNull, EQueryReturnType.DataReader), IDataReader)
    End Function


    'CPagingInfo overloads
    Public Function Paging(ByVal pi As CPagingInfo, ByVal selectColumns As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return Paging(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, selectColumns, txOrNull, type)
    End Function
    Public Function PagingWithFilters(ByVal pi As CPagingInfo, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return PagingWithFilters(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, selectColumns, criteria, txOrNull, type)
    End Function
    Public Function Paging_Dataset(ByVal pi As CPagingInfo, ByVal selectColumns As String) As DataSet
        Return Paging_Dataset(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, selectColumns)
    End Function
    Public Function PagingWithFilters_Dataset(ByVal pi As CPagingInfo, ByVal selectColumns As String, ByVal criteria As CCriteriaList) As DataSet
        Return PagingWithFilters_Dataset(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, selectColumns, criteria)
    End Function
    Public Function Paging_DataReader(ByVal pi As CPagingInfo, ByVal selectColumns As String, ByVal txOrNull As IDbTransaction) As IDataReader
        Return Paging_DataReader(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, selectColumns, txOrNull)
    End Function
    Public Function PagingWithFilters_DataReader(ByVal pi As CPagingInfo, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction) As IDataReader
        Return PagingWithFilters_DataReader(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, selectColumns, criteria, txOrNull)
    End Function


    'CPagingInfo overloads - select *, no tx
    Public Function Paging(ByVal pi As CPagingInfo, ByVal type As EQueryReturnType) As Object
        Return Paging(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, "*", Nothing, type)
    End Function
    Public Function PagingWithFilters(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList, ByVal type As EQueryReturnType) As Object
        Return PagingWithFilters(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, "*", criteria, Nothing, type)
    End Function
    Public Function Paging_Dataset(ByVal pi As CPagingInfo) As DataSet
        Return Paging_Dataset(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, "*")
    End Function
    Public Function PagingWithFilters_Dataset(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As DataSet
        Return PagingWithFilters_Dataset(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, "*", criteria)
    End Function
    Public Function Paging_DataReader(ByVal pi As CPagingInfo) As IDataReader
        Return Paging_DataReader(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, "*", Nothing)
    End Function
    Public Function PagingWithFilters_DataReader(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As IDataReader
        Return PagingWithFilters_DataReader(pi.Count, pi.PageIndex, pi.PageSize, pi.TableName, pi.Descending, pi.SortByColumn, "*", criteria, Nothing)
    End Function
#End Region

#Region "Null-Equivalent Values"
    Public Shared Function NullValue(ByVal nativeValue As Object) As Object
        If nativeValue Is Nothing Then Return System.DBNull.Value


        If TypeOf (nativeValue) Is IList Then
            If TypeOf nativeValue Is Byte() Then Return nativeValue
            If TypeOf nativeValue Is List(Of Byte) Then Return nativeValue

            Dim list As IList = CType(nativeValue, IList)

            Dim newList As New ArrayList(list.Count) 'Dbnull does not go into a generic list, so need a new one
            For i As Integer = 0 To list.Count - 1
                newList.Add(NullValue(list(i)))
            Next
            Return newList
        End If

        If TypeOf (nativeValue) Is Integer Then Return IIf(Integer.MinValue.Equals(nativeValue), System.DBNull.Value, nativeValue)
        If TypeOf (nativeValue) Is Long Then Return IIf(Long.MinValue.Equals(nativeValue), System.DBNull.Value, nativeValue)
        If TypeOf (nativeValue) Is Short Then Return IIf(Short.MinValue.Equals(nativeValue), System.DBNull.Value, nativeValue)
        If TypeOf (nativeValue) Is Guid Then Return IIf(Guid.Empty.Equals(nativeValue), System.DBNull.Value, nativeValue)
        If TypeOf (nativeValue) Is DateTime Then Return IIf(DateTime.MinValue.Equals(nativeValue), System.DBNull.Value, nativeValue)
        If TypeOf (nativeValue) Is Double Then Return IIf(Double.IsNaN(CDbl(nativeValue)), System.DBNull.Value, nativeValue)
        If TypeOf (nativeValue) Is Single Then Return IIf(Single.IsNaN(CType(nativeValue, Single)), System.DBNull.Value, nativeValue)
        If TypeOf (nativeValue) Is Decimal Then Return IIf(Decimal.MinValue.Equals(CType(nativeValue, Decimal)), System.DBNull.Value, nativeValue)
        Return nativeValue
    End Function
#End Region

#Region "Public"
    'Connection Strong
    Public ReadOnly Property ConnectionString() As String
        Get
            Return m_connectionString
        End Get
    End Property

    'HashCode
    Public Overrides Function GetHashCode() As Integer
        Return m_connectionString.GetHashCode()
    End Function

    'Casting
    Public ReadOnly Property Local() As CDataSrcLocal
        Get
            If TypeOf Me Is CDataSrcRemote Then Throw New Exception("This operation requires a local datasrc")
            Return CType(Me, CDataSrcLocal)
        End Get
    End Property
    Public ReadOnly Property Remote() As CDataSrcRemote
        Get
            If Not TypeOf Me Is CDataSrcRemote Then Throw New Exception("This operation requires a remote datasrc")
            Return CType(Me, CDataSrcRemote)
        End Get
    End Property
    Public ReadOnly Property IsLocal() As Boolean
        Get
            Return TypeOf Me Is CDataSrcLocal
        End Get
    End Property
    Public ReadOnly Property IsRemote() As Boolean
        Get
            Return TypeOf Me Is CDataSrcRemote
        End Get
    End Property


    'ExecuteDataSet
    Public Function ExecuteDataSet(ByVal sql As String, ByVal txOrNull As IDbTransaction) As DataSet
        Return ExecuteDataSet(New CCommand(sql, txOrNull))
    End Function
    Public Function ExecuteDataSet(ByVal spName As String, ByVal parameterValues As Object(), ByVal txOrNull As IDbTransaction) As DataSet
        Return ExecuteDataSet(New CCommand(spName, parameterValues, txOrNull))
    End Function
    Public Function ExecuteDataSet(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, ByVal txOrNull As IDbTransaction) As DataSet
        Return ExecuteDataSet(New CCommand(spName, pNamesAndValues, txOrNull))
    End Function
    Public Function ExecuteDataSet(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, isStoredProc As Boolean, ByVal txOrNull As IDbTransaction) As DataSet
        Return ExecuteDataSet(New CCommand(spName, pNamesAndValues, isStoredProc, txOrNull))
    End Function


    'ExecuteScalar
    Public Function ExecuteScalar(ByVal sql As String, ByVal txOrNull As IDbTransaction) As Object
        Return ExecuteScalar(New CCommand(sql, txOrNull))
    End Function
    Public Function ExecuteScalar(ByVal spName As String, ByVal parameterValues As Object(), ByVal txOrNull As IDbTransaction) As Object
        Return ExecuteScalar(New CCommand(spName, parameterValues, txOrNull))
    End Function
    Public Function ExecuteScalar(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, ByVal txOrNull As IDbTransaction) As Object
        Return ExecuteScalar(New CCommand(spName, pNamesAndValues, txOrNull))
    End Function
    Public Function ExecuteScalar(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, isStoredProc As Boolean, ByVal txOrNull As IDbTransaction) As Object
        Return ExecuteScalar(New CCommand(spName, pNamesAndValues, txOrNull))
    End Function


    'ExecuteNonQuery
    Public Function ExecuteNonQuery(ByVal sql As String, ByVal txOrNull As IDbTransaction) As Integer
        Return ExecuteNonQuery(New CCommand(sql, txOrNull))
    End Function
    Public Function ExecuteNonQuery(ByVal spName As String, ByVal parameterValues As Object(), ByVal txOrNull As IDbTransaction) As Integer
        Return ExecuteNonQuery(New CCommand(spName, parameterValues, txOrNull))
    End Function
    Public Function ExecuteNonQuery(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, ByVal txOrNull As IDbTransaction) As Integer
        Return ExecuteNonQuery(New CCommand(spName, pNamesAndValues, txOrNull))
    End Function
    Public Function ExecuteNonQuery(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, isStoredProc As Boolean, ByVal txOrNull As IDbTransaction) As Integer
        Return ExecuteNonQuery(New CCommand(spName, pNamesAndValues, isStoredProc, txOrNull))
    End Function

    'Non-transactional overloads
    Public Function ExecuteScalar(ByVal sql As String) As Object
        Return ExecuteScalar(New CCommand(sql, CType(Nothing, IDbTransaction)))
    End Function
    Public Function ExecuteDataSet(ByVal sql As String) As DataSet
        Return ExecuteDataSet(sql, CType(Nothing, IDbTransaction))
    End Function
    Public Function ExecuteNonQuery(ByVal sql As String) As Integer
        Return ExecuteNonQuery(sql, CType(Nothing, IDbTransaction))
    End Function
#End Region

#Region "DataReader/DataSet Generalisation (overloads)"
    Public Function ExecuteQuery(ByVal sql As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return ExecuteQuery(New CCommand(sql, txOrNull), type)
    End Function
    Public Function ExecuteQuery(ByVal spName As String, ByVal parameterValues As Object(), ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return ExecuteQuery(New CCommand(spName, parameterValues), type)
    End Function
    Public Function ExecuteQuery(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return ExecuteQuery(New CCommand(spName, pNamesAndValues), type)
    End Function
    Public Function ExecuteQuery(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, ByVal isStoredProcedure As Boolean, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return ExecuteQuery(New CCommand(spName, pNamesAndValues, isStoredProcedure), type)
    End Function
#End Region

#Region "Default DataSrc (Config Settings)"
    Private Shared _defaultDataSource As CDataSrc = Nothing
    Public Shared Property [Default]() As CDataSrc
        Get
            If IsNothing(_defaultDataSource) Then
                _defaultDataSource = GetDefaultDataSrc()
            End If
            Return _defaultDataSource
        End Get
        Set(ByVal value As CDataSrc)
            _defaultDataSource = value
        End Set
    End Property
    Private Shared Function GetDefaultDataSrc() As CDataSrc
        'Named connection string (assume sqlserver or webservice)
        If Len(CConfigBase.ActiveConnectionString) > 0 Then
            Dim cs As ConnectionStringSettings = ConnectionStrings(CConfigBase.ActiveConnectionString)
            If Not IsNothing(cs) AndAlso Not String.IsNullOrEmpty(cs.ConnectionString) Then
                If cs.ConnectionString.ToLower.Contains(".asmx") Then Return New CWebSrcSoap(cs.ConnectionString)
                If cs.ConnectionString.ToLower.Contains(".aspx") Then Return New CWebSrcBinary(cs.ConnectionString)
                Return New CSqlClient(cs.ConnectionString)
            End If
        End If

        'Regular Config: Driver/Conn-Str
        Dim driver As String = LCase(CConfigBase.Driver)
        Dim conStr As String = CConfigBase.ConnectionString
        Dim pass As String = CConfigBase.WebServicePassword

        'List of Factory Methods (static constructor)
        Dim local As DFactoryLoc = Nothing
        If m_factoryLoc.TryGetValue(driver, local) Then Return local(conStr)

        Dim web As DFactoryWeb = Nothing
        If m_factoryWeb.TryGetValue(driver, web) Then Return web(conStr, pass)

        'Explicit case: Driver+ConnectionString
        Select Case LCase(CConfigBase.Driver)
            Case "sqlclient", "sqlserver" : Return New CSqlClient(CConfigBase.ConnectionString)
                'Case "sqlce" : Return New CSqlCeClient(CConfigBase.ConnectionString)
            Case "mysql" : Return New CMySqlClient(CConfigBase.ConnectionString)
            Case "postgres", "ngpsql" : Return New CPostgresClient(CConfigBase.ConnectionString)
            Case "oledb" : Return New COleDb(CConfigBase.ConnectionString)
            Case "oracle", "oracleodp" : Return New COracleClientOdp(CConfigBase.ConnectionString)
            Case "oraclems" : Return New COracleClientMs(CConfigBase.ConnectionString)
            Case "odbc" : Return New COdbc(CConfigBase.ConnectionString)
            Case "webservice" : Return New CWebSrcSoap(CConfigBase.ConnectionString, CConfigBase.WebServicePassword)
            Case "webpage", "websrc", "website" : Return New CWebSrcBinary(CConfigBase.ConnectionString, CConfigBase.WebServicePassword)
        End Select

        'Shorthand case #1: Webservices
        If Len(CConfigBase.WebSite) > 0 Then
            Return New CWebSrcSoap(CConfigBase.WebSite & "webservices/WSDataSrc.asmx", CConfigBase.WebServicePassword)
        End If

        'Shorthand case #2: Access Database
        If Len(CConfigBase.AccessDatabasePath) > 0 Then
            Return OleDbFromAccessPath(CConfigBase.AccessDatabasePath)
        End If

        'Shorthand case #3: mdf file in App_Data
        If Len(CConfigBase.SqlExpressPath) > 0 Then
            Return SqlExpressFromPath(CConfigBase.SqlExpressPath)
        End If

        'Shorthand case #4: Excel Spreadsheet
        If Len(CConfigBase.ExcelDatabasePath) > 0 Then
            Return OleDbFromExcelPath(CConfigBase.ExcelDatabasePath) 'Note: SELECT * FROM [SheetName$]
        End If

        ''Shorthand case #5: SQL CE
        'If Len(CConfigBase.SqlCEPath) > 0 Then
        '    Return SqlCeFromPath(CConfigBase.SqlCEPath)
        'End If

        'Driver misspelt
        Dim msg As String = "\nDriver Options are: [SqlClient,MySql,PostGres,Odbc,OleDb,Oracle,OracleMs,OracleOdp,WebService,WebPage]"
        msg &= "\nAlternatively you can use a single config setting such as 'AccessDatabasePath', 'SqlExpressPath', 'SqlCEPath' or 'ExcelDatabasePath'"
        If Len(CConfigBase.Driver) > 0 Then Throw New Exception("Config File Error:\nUnrecognised driver: " & CConfigBase.Driver & msg)

        'ConnectionString (in appsettings) but no driver - Default to SqlServer
        If Len(CConfigBase.ConnectionString) > 0 Then
            Return New CSqlClient(CConfigBase.ConnectionString)
        ElseIf ConnectionStringCount() > 0 AndAlso GetConnectionString(0).Length > 0 Then
            'Generic connection string (take the first one if any) - Default to SqlServer
            Return New CSqlClient(GetConnectionString(0))
        Else
            'No settings found
            Throw New Exception("Expected a pair of config settings called 'Driver' and 'ConnectionString'" & msg)
        End If
    End Function

    Public Const EXCEL_DEFAULT_TABLE_NAME As String = "[Sheet1$]"
    Public Shared Function OleDbFromAccessPath(ByVal path As String) As COleDb
        If path.ToLower.EndsWith(".accdb") Then
            Return New COleDb(String.Concat("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=", MapPath(path), ";Persist Security Info=False"))
        Else
            Return New COleDb(String.Concat("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=", MapPath(path), ";Persist Security Info=False"))
        End If
    End Function
    Public Shared Function SqlExpressFromPath(ByVal path As String) As CSqlClient
        Return New CSqlClient(String.Concat("Data Source=.; AttachDbFilename=", MapPath(path), "; Integrated Security=True; User Instance=True"))
    End Function
    'Public Shared Function SqlCeFromPath(ByVal path As String) As CSqlCeClient
    '    Return New CSqlCeClient(String.Concat("Data Source=", MapPath(path), ";Persist Security Info=False;"))
    'End Function
    Public Shared Function OleDbFromExcelPath(ByVal path As String) As COleDb
        Return OleDbFromExcelPath(path, True)
    End Function
    Public Shared Function OleDbFromExcelPath(ByVal path As String, ByVal containsHeaderRow As Boolean) As COleDb
        Return OleDbFromExcelPath(path, containsHeaderRow, False) 'Note: header-row is irrelevant to xls, but imex affects ability to change schema
    End Function
    Public Shared ACE_DRIVER_ONLY As Boolean = IsPlatformX64() 'used to read xls from 64-bit
    Public Shared Function OleDbFromExcelPath(ByVal path As String, ByVal containsHeaderRow As Boolean, ByVal alwaysReadIntermixed As Boolean) As COleDb
        'Different providers
        Dim is2007Format As Boolean = path.ToLower.EndsWith(".xlsx") OrElse ACE_DRIVER_ONLY
        Dim sb As New StringBuilder("Provider=")
        If is2007Format Then
            sb.Append("Microsoft.ACE.OLEDB.12.0;")
        Else
            sb.Append("Microsoft.Jet.OLEDB.4.0;")
        End If

        'Same path syntax
        sb.Append("Data Source=")
        sb.Append(MapPath(path))

        'Different extended properties
        If is2007Format Then
            sb.Append(";Extended Properties=""Excel 12.0 Xml;")
        Else
            sb.Append(";Extended Properties=""Excel 8.0;")
        End If
        If containsHeaderRow Then sb.Append("HDR=YES;") Else sb.Append("HDR=NO;")
        If alwaysReadIntermixed Then sb.Append("IMEX=1") Else sb.Append("IMEX=0")
        sb.Append("""") 'Extended properties must be in quotes

        Return New COleDb(sb.ToString) '*Use "select * from [SheetName$]"
    End Function

    Public Shared Function OleDbFromCsvPath(ByVal folderPath As String) As COleDb
        Return OleDbFromCsvPath(folderPath, True)
    End Function
    Public Shared Function OleDbFromCsvPath(ByVal folderPath As String, ByVal containsHeaderRow As Boolean) As COleDb
        'Different providers
        Dim is2007Format As Boolean = ACE_DRIVER_ONLY
        Dim sb As New StringBuilder("Provider=")
        If is2007Format Then
            sb.Append("Microsoft.ACE.OLEDB.12.0;")
        Else
            sb.Append("Microsoft.Jet.OLEDB.4.0;")
        End If


        sb.Append("Data Source=")
        sb.Append(folderPath)
        sb.Append(";Extended Properties=""text;")
        If containsHeaderRow Then sb.Append("HDR=YES;") Else sb.Append("HDR=NO;")
        sb.Append("FMT=Delimited"";")
        Return New COleDb(sb.ToString) '*Use "select * from filename"
    End Function

    Private Shared Function MapPath(ByRef path As String) As String
        If IO.File.Exists(path) AndAlso path.Contains(":") Then Return path
        If IO.Directory.Exists(path) Then Return path

        'Try to interpret non-absolute path
        If Not IsNothing(Current) Then
            'Interpret path relative to website root
            With Current.Server
                If IO.File.Exists(.MapPath(path)) Then
                    path = .MapPath(path)
                ElseIf IO.File.Exists(.MapPath("~/App_Data/" & path)) Then
                    path = .MapPath("~/App_Data/" & path)
                End If
            End With
        Else
            'Interpret path relative to application dir
            Try
                Dim temp As String = String.Concat(My.Application.Info.DirectoryPath, "\", path)
                If IO.File.Exists(temp) Then path = temp
            Catch
            End Try
        End If
        If IO.File.Exists(path) Then Return path

        Throw New Exception("File does not exist: " & path)
    End Function
    'Generic connection strings
    Private Shared Function ConnectionStringCount() As Integer
        Return ConnectionStrings.Count
    End Function
    Private Shared Function GetConnectionString(ByVal key As String) As String
        Dim cs As ConnectionStringSettings = ConnectionStrings(key)
        If Not IsNothing(cs) Then Return cs.ConnectionString
        Return String.Empty
    End Function
    Private Shared Function GetConnectionString(ByVal index As Integer) As String
        Dim cs As ConnectionStringSettings = ConnectionStrings(index)
        If Not IsNothing(cs) Then Return cs.ConnectionString
        Return String.Empty
    End Function
#End Region

#Region "Logging"
    Protected Sub Log(ByVal cmd As IDbCommand)
        Dim sw As New IO.StreamWriter(LogPath, True)
        sw.Write(DateTime.Now.ToString)
        sw.Write(vbTab)
        sw.Write(cmd.CommandText.Replace(vbCrLf, "").Replace(vbLf, ""))
        sw.Write(vbTab)
        For Each i As IDbDataParameter In cmd.Parameters
            sw.Write("{")
            sw.Write(i.ParameterName)
            sw.Write("=")
            sw.Write(i.Value.ToString.Replace(vbCrLf, "").Replace(vbLf, ""))
            sw.Write("}")
            sw.Write(vbTab)
        Next
        sw.Write(vbCrLf)
        sw.Close()
    End Sub
    Private Shared _logPath As String
    Private ReadOnly Property LogPath() As String
        Get
            If IsNothing(_logPath) Then
                _logPath = HttpContext.Current.Server.MapPath("~/App_Data/dblog.txt")

                Dim folder As String = IO.Path.GetDirectoryName(_logPath)
                If Not IO.Directory.Exists(folder) Then IO.Directory.CreateDirectory(folder)
            End If
            Return _logPath
        End Get
    End Property
    Protected Shared Sub Rethrow(ByVal ex As Exception, ByVal sql As String)
        Throw New Exception(sql & vbCrLf & ex.Message)
    End Sub
#End Region

#Region "Select/Delete Overloads"
    Public Function SelectCount(ByVal tableName As String, ByVal txOrNull As IDbTransaction) As Integer
        Return SelectCount(New CWhere(tableName, txOrNull))
    End Function
    Public Function SelectCount(ByVal tableName As String, ByVal criteria As CCriteria, ByVal txOrNull As IDbTransaction) As Integer
        Return SelectCount(New CWhere(tableName, criteria, txOrNull))
    End Function
    Public Function SelectCount(ByVal tableName As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction) As Integer
        Return SelectCount(New CWhere(tableName, criteria, txOrNull))
    End Function
    Public Function SelectCount(ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal txOrNull As IDbTransaction) As Integer
        Return SelectCount(New CWhere(tableName, unsafeWhereClause, txOrNull))
    End Function

    'Non-Transactional, Optimal
    Public Function SelectAll(ByVal selectCols As String, ByVal tableName As String, ByVal orderByCols As String) As Object
        Return Me.SelectAll(selectCols, tableName, orderByCols, Nothing, EQueryReturnType.Optimal)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteria, ByVal orderByCols As String) As Object
        Return Me.SelectWhere(selectCols, tableName, criteria, orderByCols, Nothing, EQueryReturnType.Optimal)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteriaList, ByVal orderByCols As String) As Object
        Return Me.SelectWhere(selectCols, tableName, criteria, orderByCols, Nothing, EQueryReturnType.Optimal)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal orderByCols As String) As Object
        Return Me.SelectWhere(selectCols, tableName, unsafeWhereClause, orderByCols, Nothing, EQueryReturnType.Optimal)
    End Function


    'Non-Transactional, Specific type
    Public Function SelectAll(ByVal selectCols As String, ByVal tableName As String, ByVal orderByCols As String, ByVal type As EQueryReturnType) As Object
        Return Me.SelectAll(selectCols, tableName, orderByCols, Nothing, type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteria, ByVal orderByCols As String, ByVal type As EQueryReturnType) As Object
        Return Me.SelectWhere(selectCols, tableName, criteria, orderByCols, Nothing, type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteriaList, ByVal orderByCols As String, ByVal type As EQueryReturnType) As Object
        Return Me.SelectWhere(selectCols, tableName, criteria, orderByCols, Nothing, type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal orderByCols As String, ByVal type As EQueryReturnType) As Object
        Return Me.SelectWhere(selectCols, tableName, unsafeWhereClause, orderByCols, Nothing, type)
    End Function

    Public Function DeleteWhere(ByVal tableName As String, ByVal c As CCriteria) As Integer
        Return DeleteWhere(tableName, c, Nothing)
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal criteria As CCriteriaList) As Integer
        Return DeleteWhere(tableName, criteria, Nothing)
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal keys As CNameValueList, ByVal txOrNull As IDbTransaction) As Integer
        Dim criteria As New CCriteriaList(keys)
        Return DeleteWhere(tableName, criteria, txOrNull)
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal unsafeWhereClause As String) As Integer
        Return DeleteWhere(tableName, unsafeWhereClause, Nothing)
    End Function

    'Transactional, Specific type
    Public Function SelectAll(ByVal selectCols As String, ByVal tableName As String, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return Me.Select(New CSelectWhere(selectCols, tableName, orderByCols, txOrNull), type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteria, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return Me.Select(New CSelectWhere(selectCols, tableName, criteria, orderByCols, txOrNull), type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteriaList, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return Me.Select(New CSelectWhere(selectCols, tableName, criteria, orderByCols, txOrNull), type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return Me.Select(New CSelectWhere(selectCols, tableName, unsafeWhereClause, orderByCols, txOrNull), type)
    End Function

    Public Function DeleteAll(ByVal tableName As String, ByVal txOrNull As IDbTransaction) As Integer
        Return Me.Delete(New CWhere(tableName, txOrNull))
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal criteria As CCriteria, ByVal txOrNull As IDbTransaction) As Integer
        Return Me.Delete(New CWhere(tableName, criteria, txOrNull))
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction) As Integer
        Return Me.Delete(New CWhere(tableName, criteria, txOrNull))
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal txOrNull As IDbTransaction) As Integer
        Return Me.Delete(New CWhere(tableName, unsafeWhereClause, txOrNull))
    End Function
#End Region

#Region "Select - Dataset"
    'Select
    Public Function Select_Dataset(ByVal where As CSelectWhere) As DataSet
        Return CType(Me.Select(where, EQueryReturnType.DataSet), DataSet)
    End Function
    Public Function Select_Datareader(ByVal where As CSelectWhere) As IDataReader
        Return CType(Me.Select(where, EQueryReturnType.DataReader), IDataReader)
    End Function

    'SelectAll
    Public Function SelectAll_Dataset(ByVal tableName As String) As DataSet
        Return SelectAll_Dataset(tableName, Nothing)
    End Function
    Public Function SelectAll_Dataset(ByVal tableName As String, ByVal orderBy As String) As DataSet
        Return SelectAll_Dataset("*", tableName, orderBy)
    End Function
    Public Function SelectAll_Dataset(ByVal selectColumns As String, ByVal tableName As String, ByVal orderBy As String) As DataSet
        Return CType(SelectAll(selectColumns, tableName, orderBy, EQueryReturnType.DataSet), DataSet)
    End Function

    'SelectWhere
    Public Function SelectWhere_Dataset(ByVal tableName As String, ByVal criteria As CCriteriaList) As DataSet
        Return SelectWhere_Dataset(tableName, Nothing, criteria)
    End Function
    Public Function SelectWhere_Dataset(ByVal tableName As String, ByVal orderBy As String, ByVal criteria As CCriteriaList) As DataSet
        Return SelectWhere_Dataset("*", tableName, orderBy, criteria)
    End Function
    Public Function SelectWhere_Dataset(ByVal selectColumns As String, ByVal tableName As String, ByVal orderBy As String, ByVal criteria As CCriteriaList) As DataSet
        Return CType(SelectWhere(selectColumns, tableName, criteria, orderBy, EQueryReturnType.DataSet), DataSet)
    End Function
    Public Function SelectWhere_Dataset(ByVal selectColumns As String, ByVal tableName As String, ByVal orderBy As String, ByVal unsafeWhere As String) As DataSet
        Return CType(SelectWhere(selectColumns, tableName, unsafeWhere, orderBy, EQueryReturnType.DataSet), DataSet)
    End Function
#End Region

#Region "Bulk Operations"
    'Lists
    Public Sub BulkDelete(ByVal list As ICollection, ByVal txIsolation As IsolationLevel)
        Me.BulkSaveDelete(Nothing, list, txIsolation)
    End Sub
    Public Sub BulkSave(ByVal list As ICollection, ByVal txIsolation As IsolationLevel)
        Me.BulkSaveDelete(list, Nothing, txIsolation)
    End Sub

    'Non-list overloads
    Public Sub BulkDelete(ByVal cascading As CBase, ByVal txIsolation As IsolationLevel)
        Dim list As New ArrayList()
        list.Add(cascading)
        Me.BulkDelete(list, txIsolation)
    End Sub
    Public Sub BulkSave(ByVal cascading As CBase, ByVal txIsolation As IsolationLevel)
        Dim list As New ArrayList()
        list.Add(cascading)
        Me.BulkSave(list, txIsolation)
    End Sub

    'Isolation-level Defaults
    Private Const DEFAULT_TX_ISOLATION As IsolationLevel = IsolationLevel.Unspecified
    Public Sub BulkSave(ByVal saves As ICollection)
        BulkSave(saves, DEFAULT_TX_ISOLATION)
    End Sub
    Public Sub BulkDelete(ByVal deletes As ICollection)
        BulkDelete(deletes, DEFAULT_TX_ISOLATION)
    End Sub
    Public Sub BulkSave(ByVal cascading As CBase)
        BulkSave(cascading, DEFAULT_TX_ISOLATION)
    End Sub
    Public Sub BulkDelete(ByVal cascading As CBase)
        BulkDelete(cascading, DEFAULT_TX_ISOLATION)
    End Sub
    Public Sub BulkSaveDelete(ByVal saves As ICollection, ByVal deletes As ICollection)
        BulkSaveDelete(saves, deletes, DEFAULT_TX_ISOLATION)
    End Sub
#End Region

#Region "Distinct"
    'Non-Transactional
    Public Function SelectDistinctAsInt(ByVal tableName As String, ByVal colName As String) As List(Of Integer)
        Return MakeListInteger(SelectDistinctSql(tableName, colName), Nothing)
    End Function
    Public Function SelectDistinctAsStr(ByVal tableName As String, ByVal colName As String) As List(Of String)
        Return MakeListString(SelectDistinctSql(tableName, colName), Nothing)
    End Function
    Public Function SelectDistinctAsGuid(ByVal tableName As String, ByVal colName As String) As List(Of Guid)
        Return MakeListGuid(SelectDistinctSql(tableName, colName), Nothing)
    End Function
    Public Function SelectDistinctAsDate(ByVal tableName As String, ByVal colName As String) As List(Of DateTime)
        Return MakeListDate(SelectDistinctSql(tableName, colName), Nothing)
    End Function

    'Transactional
    Public Function SelectDistinctAsInt(ByVal tableName As String, ByVal colName As String, ByVal tx As IDbTransaction) As List(Of Integer)
        Return MakeListInteger(SelectDistinctSql(tableName, colName), tx)
    End Function
    Public Function SelectDistinctAsStr(ByVal tableName As String, ByVal colName As String, ByVal tx As IDbTransaction) As List(Of String)
        Return MakeListString(SelectDistinctSql(tableName, colName), tx)
    End Function
    Public Function SelectDistinctAsGuid(ByVal tableName As String, ByVal colName As String, ByVal tx As IDbTransaction) As List(Of Guid)
        Return MakeListGuid(SelectDistinctSql(tableName, colName), tx)
    End Function
    Public Function SelectDistinctAsDate(ByVal tableName As String, ByVal colName As String, ByVal tx As IDbTransaction) As List(Of DateTime)
        Return MakeListDate(SelectDistinctSql(tableName, colName), tx)
    End Function

    'Private
    Private Function SelectDistinctSql(ByVal tableName As String, ByVal colName As String) As String
        Return String.Concat("SELECT DISTINCT ", colName, " FROM ", tableName, " ORDER BY ", colName)
    End Function
#End Region

#Region "Single-Column Data"
    'Overloads: Sql-only
    Public Function MakeListString(ByVal sql As String) As List(Of String)
        Return MakeListString(sql, Nothing)
    End Function
    Public Function MakeListInteger(ByVal sql As String) As List(Of Integer)
        Return MakeListInteger(sql, Nothing)
    End Function
    Public Function MakeListLong(ByVal sql As String) As List(Of Long)
        Return MakeListLong(sql, Nothing)
    End Function
    Public Function MakeListGuid(ByVal sql As String) As List(Of Guid)
        Return MakeListGuid(sql, Nothing)
    End Function
    Public Function MakeListDate(ByVal sql As String) As List(Of DateTime)
        Return MakeListDate(sql, Nothing)
    End Function
    Public Function MakeListDouble(ByVal sql As String) As List(Of Double)
        Return MakeListDouble(sql, Nothing)
    End Function

    'Overloads: Sql+transaction
    Public Function MakeListString(ByVal sql As String, ByVal txOrNull As IDbTransaction) As List(Of String)
        If Me.IsLocal Then
            Return MakeListString(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeListString(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeListLong(ByVal sql As String, ByVal txOrNull As IDbTransaction) As List(Of Long)
        If Me.IsLocal Then
            Return MakeListLong(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeListLong(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeListInteger(ByVal sql As String, ByVal txOrNull As IDbTransaction) As List(Of Integer)
        If Me.IsLocal Then
            Return MakeListInteger(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeListInteger(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeListGuid(ByVal sql As String, ByVal txOrNull As IDbTransaction) As List(Of Guid)
        If Me.IsLocal Then
            Return MakeListGuid(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeListGuid(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeListDate(ByVal sql As String, ByVal txOrNull As IDbTransaction) As List(Of DateTime)
        If Me.IsLocal Then
            Return MakeListDate(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeListDate(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeListDouble(ByVal sql As String, ByVal txOrNull As IDbTransaction) As List(Of Double)
        If Me.IsLocal Then
            Return MakeListDouble(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeListDouble(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function

    'Overloads: Command object
    Public Function MakeListString(ByVal cmd As CCommand) As List(Of String)
        If IsLocal Then Return MakeListString(Local.ExecuteReader(cmd))
        Return MakeListString(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeListInteger(ByVal cmd As CCommand) As List(Of Integer)
        If IsLocal Then Return MakeListInteger(Local.ExecuteReader(cmd))
        Return MakeListInteger(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeListGuid(ByVal cmd As CCommand) As List(Of Guid)
        If IsLocal Then Return MakeListGuid(Local.ExecuteReader(cmd))
        Return MakeListGuid(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeListDate(ByVal cmd As CCommand) As List(Of DateTime)
        If IsLocal Then Return MakeListDate(Local.ExecuteReader(cmd))
        Return MakeListDate(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeListDouble(ByVal cmd As CCommand) As List(Of Double)
        If IsLocal Then Return MakeListDouble(Local.ExecuteReader(cmd))
        Return MakeListDouble(ExecuteDataSet(cmd).Tables(0))
    End Function


    'Overloads: Command object
    Public Function MakeListString(ByVal where As CSelectWhere) As List(Of String)
        If IsLocal Then Return MakeListString(Me.Select_Datareader(where))
        Return MakeListString(Me.Select_Dataset(where).Tables(0))
    End Function
    Public Function MakeListInteger(ByVal where As CSelectWhere) As List(Of Integer)
        If IsLocal Then Return MakeListInteger(Me.Select_Datareader(where))
        Return MakeListInteger(Me.Select_Dataset(where).Tables(0))
    End Function
    Public Function MakeListGuid(ByVal where As CSelectWhere) As List(Of Guid)
        If IsLocal Then Return MakeListGuid(Me.Select_Datareader(where))
        Return MakeListGuid(Me.Select_Dataset(where).Tables(0))
    End Function
    Public Function MakeListDate(ByVal where As CSelectWhere) As List(Of DateTime)
        If IsLocal Then Return MakeListDate(Me.Select_Datareader(where))
        Return MakeListDate(Me.Select_Dataset(where).Tables(0))
    End Function
    Public Function MakeListDouble(ByVal where As CSelectWhere) As List(Of Double)
        If IsLocal Then Return MakeListDouble(Me.Select_Datareader(where))
        Return MakeListDouble(Me.Select_Dataset(where).Tables(0))
    End Function

    'Overloads: No column specified (assume first column
    Public Function MakeListString(ByVal dr As IDataReader) As List(Of String)
        Return MakeListString(dr, Nothing)
    End Function
    Public Function MakeListInteger(ByVal dr As IDataReader) As List(Of Integer)
        Return MakeListInteger(dr, Nothing)
    End Function
    Public Function MakeListLong(ByVal dr As IDataReader) As List(Of Long)
        Return MakeListLong(dr, Nothing)
    End Function
    Public Function MakeListGuid(ByVal dr As IDataReader) As List(Of Guid)
        Return MakeListGuid(dr, Nothing)
    End Function
    Public Function MakeListDate(ByVal dr As IDataReader) As List(Of DateTime)
        Return MakeListDate(dr, Nothing)
    End Function
    Public Function MakeListDouble(ByVal dr As IDataReader) As List(Of Double)
        Return MakeListDouble(dr, Nothing)
    End Function

    Public Function MakeListString(ByVal dt As DataTable) As List(Of String)
        Return MakeListString(dt, Nothing)
    End Function
    Public Function MakeListLong(ByVal dt As DataTable) As List(Of Long)
        Return MakeListLong(dt, Nothing)
    End Function
    Public Function MakeListInteger(ByVal dt As DataTable) As List(Of Integer)
        Return MakeListInteger(dt, Nothing)
    End Function
    Public Function MakeListGuid(ByVal dt As DataTable) As List(Of Guid)
        Return MakeListGuid(dt, Nothing)
    End Function
    Public Function MakeListDate(ByVal dt As DataTable) As List(Of DateTime)
        Return MakeListDate(dt, Nothing)
    End Function
    Public Function MakeListDouble(ByVal dt As DataTable) As List(Of Double)
        Return MakeListDouble(dt, Nothing)
    End Function

    'Datareaders
    Public Function MakeListString(ByVal dr As IDataReader, ByVal colName As String) As List(Of String)
        Dim list As New List(Of String)()
        Try
            If IsNothing(colName) Then
                While dr.Read()
                    If dr.IsDBNull(0) Then list.Add(Nothing) Else list.Add(dr(0).ToString)
                End While
            Else
                While dr.Read()
                    list.Add(CAdoData.GetStr(dr, colName))
                End While
            End If
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return list
    End Function
    Public Function MakeListLong(ByVal dr As IDataReader, ByVal colName As String) As List(Of Long)
        Dim list As New List(Of Long)()
        Try
            If IsNothing(colName) Then
                While dr.Read()
                    If dr.IsDBNull(0) Then list.Add(Long.MinValue) Else list.Add(CLng(dr(0)))
                End While
            Else
                While dr.Read()
                    list.Add(CAdoData.GetLong(dr, colName))
                End While
            End If
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return list
    End Function
    Public Function MakeListInteger(ByVal dr As IDataReader, ByVal colName As String) As List(Of Integer)
        Dim list As New List(Of Integer)()
        Try
            If IsNothing(colName) Then
                While dr.Read()
                    If dr.IsDBNull(0) Then list.Add(Integer.MinValue) Else list.Add(CInt(dr(0)))
                End While
            Else
                While dr.Read()
                    list.Add(CAdoData.GetInt(dr, colName))
                End While
            End If
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return list
    End Function
    Public Function MakeListGuid(ByVal dr As IDataReader, ByVal colName As String) As List(Of Guid)
        Dim list As New List(Of Guid)()
        Try
            If IsNothing(colName) Then
                While dr.Read()
                    list.Add(CAdoData.GetGuid(dr, 0))
                End While
            Else
                While dr.Read()
                    list.Add(CAdoData.GetGuid(dr, colName))
                End While
            End If
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return list
    End Function
    Public Function MakeListDate(ByVal dr As IDataReader, ByVal colName As String) As List(Of DateTime)
        Dim list As New List(Of DateTime)()
        Try
            If IsNothing(colName) Then
                While dr.Read()
                    If dr.IsDBNull(0) Then list.Add(DateTime.MinValue) Else list.Add(CDate(dr(0)))
                End While
            Else
                While dr.Read()
                    list.Add(CAdoData.GetDate(dr, colName))
                End While
            End If
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return list
    End Function
    Public Function MakeListDouble(ByVal dr As IDataReader, ByVal colName As String) As List(Of Double)
        Dim list As New List(Of Double)()
        Try
            If IsNothing(colName) Then
                While dr.Read()
                    If dr.IsDBNull(0) Then list.Add(Double.NaN) Else list.Add(CDbl(dr(0)))
                End While
            Else
                While dr.Read()
                    list.Add(CAdoData.GetDbl(dr, colName))
                End While
            End If
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return list
    End Function

    'DataTables
    Public Function MakeListString(ByVal dt As DataTable, ByVal colName As String) As List(Of String)
        Dim list As New List(Of String)(dt.Rows.Count)
        If IsNothing(colName) Then
            For Each i As DataRow In dt.Rows
                If i.IsNull(0) Then list.Add(Nothing) Else list.Add(CStr(i(0)))
            Next
        Else
            For Each i As DataRow In dt.Rows
                list.Add(CAdoData.GetStr(i, colName))
            Next
        End If
        Return list
    End Function
    Public Function MakeListLong(ByVal dt As DataTable, ByVal colName As String) As List(Of Long)
        Dim list As New List(Of Long)(dt.Rows.Count)
        If IsNothing(colName) Then
            For Each i As DataRow In dt.Rows
                If i.IsNull(0) Then list.Add(Long.MinValue) Else list.Add(CLng(i(0)))
            Next
        Else
            For Each i As DataRow In dt.Rows
                list.Add(CAdoData.GetLong(i, colName))
            Next
        End If
        Return list
    End Function
    Public Function MakeListInteger(ByVal dt As DataTable, ByVal colName As String) As List(Of Integer)
        Dim list As New List(Of Integer)(dt.Rows.Count)
        If IsNothing(colName) Then
            For Each i As DataRow In dt.Rows
                If i.IsNull(0) Then list.Add(Integer.MinValue) Else list.Add(CInt(i(0)))
            Next
        Else
            For Each i As DataRow In dt.Rows
                list.Add(CAdoData.GetInt(i, colName))
            Next
        End If
        Return list
    End Function
    Public Function MakeListGuid(ByVal dt As DataTable, ByVal colName As String) As List(Of Guid)
        Dim list As New List(Of Guid)(dt.Rows.Count)
        If IsNothing(colName) Then
            For Each dr As DataRow In dt.Rows
                If dr.IsNull(0) Then list.Add(Guid.Empty) Else list.Add(CType(dr(0), Guid))
            Next
        Else
            For Each dr As DataRow In dt.Rows
                list.Add(CAdoData.GetGuid(dr, colName))
            Next
        End If
        Return list
    End Function
    Public Function MakeListDate(ByVal dt As DataTable, ByVal colName As String) As List(Of DateTime)
        Dim list As New List(Of DateTime)(dt.Rows.Count)
        If IsNothing(colName) Then
            For Each dr As DataRow In dt.Rows
                If dr.IsNull(0) Then list.Add(DateTime.MinValue) Else list.Add(CDate(dr(0)))
            Next
        Else
            For Each dr As DataRow In dt.Rows
                list.Add(CAdoData.GetDate(dr, colName))
            Next
        End If
        Return list
    End Function
    Public Function MakeListDouble(ByVal dt As DataTable, ByVal colName As String) As List(Of Double)
        Dim list As New List(Of Double)(dt.Rows.Count)
        If IsNothing(colName) Then
            For Each dr As DataRow In dt.Rows
                If dr.IsNull(0) Then list.Add(Double.NaN) Else list.Add(CDbl(dr(0)))
            Next
        Else
            For Each dr As DataRow In dt.Rows
                list.Add(CAdoData.GetDbl(dr, colName))
            Next
        End If
        Return list
    End Function
#End Region

#Region "Double-Column Data"
    'Overloads: Sql-only
    Public Function MakeDictIntInt(ByVal sql As String) As Dictionary(Of Integer, Integer)
        Return MakeDictIntInt(sql, CType(Nothing, IDbTransaction))
    End Function
    Public Function MakeDictGuidGuid(ByVal sql As String) As Dictionary(Of Guid, Guid)
        Return MakeDictGuidGuid(sql, CType(Nothing, IDbTransaction))
    End Function
    Public Function MakeDictStrInt(ByVal sql As String) As Dictionary(Of String, Integer)
        Return MakeDictStrInt(sql, CType(Nothing, IDbTransaction))
    End Function
    Public Function MakeDictStrStr(ByVal sql As String) As Dictionary(Of String, String)
        Return MakeDictStrStr(sql, CType(Nothing, IDbTransaction))
    End Function
    Public Function MakeDictStrListStr(ByVal sql As String) As Dictionary(Of String, List(Of String))
        Return MakeDictStrListStr(sql, CType(Nothing, IDbTransaction))
    End Function
    Public Function MakeDictDictListStr(ByVal sql As String) As Dictionary(Of String, Dictionary(Of String, List(Of String)))
        Return MakeDictDictListStr(sql, CType(Nothing, IDbTransaction))
    End Function
    Public Function MakeNameValueList(ByVal sql As String) As CNameValueList
        Return MakeNameValueList(sql, CType(Nothing, IDbTransaction))
    End Function


    'Overloads: Sql+params+transaction
    Public Function MakeDictStrListStr(ByVal spName As String, params As CNameValueList, isStoredProc As Boolean) As Dictionary(Of String, List(Of String))
        Return MakeDictStrListStr(spName, params, isStoredProc, CType(Nothing, IDbTransaction))
    End Function
    Public Function MakeDictDictListStr(ByVal spName As String, params As CNameValueList, isStoredProc As Boolean) As Dictionary(Of String, Dictionary(Of String, List(Of String)))
        Return MakeDictDictListStr(spName, params, isStoredProc, CType(Nothing, IDbTransaction))
    End Function

    'Overloads: Sql+transaction
    Public Function MakeDictIntInt(ByVal sql As String, ByVal txOrNull As IDbTransaction) As Dictionary(Of Integer, Integer)
        If Me.IsLocal Then
            Return MakeDictIntInt(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeDictIntInt(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeDictGuidGuid(ByVal sql As String, ByVal txOrNull As IDbTransaction) As Dictionary(Of Guid, Guid)
        If Me.IsLocal Then
            Return MakeDictGuidGuid(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeDictGuidGuid(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeDictStrInt(ByVal sql As String, ByVal txOrNull As IDbTransaction) As Dictionary(Of String, Integer)
        If Me.IsLocal Then
            Return MakeDictStrInt(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeDictStrInt(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeDictStrStr(ByVal sql As String, ByVal txOrNull As IDbTransaction) As Dictionary(Of String, String)
        If Me.IsLocal Then
            Return MakeDictStrStr(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeDictStrStr(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeDictStrListStr(ByVal sql As String, ByVal txOrNull As IDbTransaction) As Dictionary(Of String, List(Of String))
        If Me.IsLocal Then
            Return MakeDictStrListStr(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeDictStrListStr(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeDictDictListStr(ByVal sql As String, ByVal txOrNull As IDbTransaction) As Dictionary(Of String, Dictionary(Of String, List(Of String)))
        If Me.IsLocal Then
            Return MakeDictDictListStr(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeDictDictListStr(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeNameValueList(ByVal sql As String, ByVal txOrNull As IDbTransaction) As CNameValueList
        If Me.IsLocal Then
            Return MakeNameValueList(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeNameValueList(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function


    Public Function MakeDictStrListStr(ByVal spName As String, params As CNameValueList, isStoredProc As Boolean, ByVal txOrNull As IDbTransaction) As Dictionary(Of String, List(Of String))
        If Me.IsLocal Then
            Return MakeDictStrListStr(Local.ExecuteReader(spName, params, isStoredProc, txOrNull))
        Else
            Return MakeDictStrListStr(ExecuteDataSet(spName, params, isStoredProc, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeDictDictListStr(ByVal spName As String, params As CNameValueList, isStoredProc As Boolean, ByVal txOrNull As IDbTransaction) As Dictionary(Of String, Dictionary(Of String, List(Of String)))
        If Me.IsLocal Then
            Return MakeDictDictListStr(Local.ExecuteReader(spName, params, isStoredProc, txOrNull))
        Else
            Return MakeDictDictListStr(ExecuteDataSet(spName, params, isStoredProc, txOrNull).Tables(0))
        End If
    End Function

    'Overloads: Command object
    Public Function MakeDictIntInt(ByVal cmd As CCommand) As Dictionary(Of Integer, Integer)
        If IsLocal Then Return MakeDictIntInt(Local.ExecuteReader(cmd))
        Return MakeDictIntInt(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeDictGuidGuid(ByVal cmd As CCommand) As Dictionary(Of Guid, Guid)
        If IsLocal Then Return MakeDictGuidGuid(Local.ExecuteReader(cmd))
        Return MakeDictGuidGuid(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeDictStrInt(ByVal cmd As CCommand) As Dictionary(Of String, Integer)
        If IsLocal Then Return MakeDictStrInt(Local.ExecuteReader(cmd))
        Return MakeDictStrInt(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeDictStrStr(ByVal cmd As CCommand) As Dictionary(Of String, String)
        If IsLocal Then Return MakeDictStrStr(Local.ExecuteReader(cmd))
        Return MakeDictStrStr(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeNameValueList(ByVal cmd As CCommand) As CNameValueList
        If IsLocal Then Return MakeNameValueList(Local.ExecuteReader(cmd))
        Return MakeNameValueList(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeDictStrListStr(ByVal cmd As CCommand) As Dictionary(Of String, List(Of String))
        If Me.IsLocal Then
            Return MakeDictStrListStr(Local.ExecuteReader(cmd))
        Else
            Return MakeDictStrListStr(ExecuteDataSet(cmd).Tables(0))
        End If
    End Function
    Public Function MakeDictDictListStr(ByVal cmd As CCommand) As Dictionary(Of String, Dictionary(Of String, List(Of String)))
        If Me.IsLocal Then
            Return MakeDictDictListStr(Local.ExecuteReader(cmd))
        Else
            Return MakeDictDictListStr(ExecuteDataSet(cmd).Tables(0))
        End If
    End Function


    'Datareaders
    Public Function MakeDictIntInt(ByVal dr As IDataReader) As Dictionary(Of Integer, Integer)
        Dim dict As New Dictionary(Of Integer, Integer)
        Try
            While dr.Read()
                dict(CAdoData.GetInt(dr, 0)) = CAdoData.GetInt(dr, 1)
            End While
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return dict
    End Function
    Public Function MakeDictGuidGuid(ByVal dr As IDataReader) As Dictionary(Of Guid, Guid)
        Dim dict As New Dictionary(Of Guid, Guid)
        Try
            While dr.Read()
                dict(CAdoData.GetGuid(dr, 0)) = CAdoData.GetGuid(dr, 1)
            End While
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return dict
    End Function
    Public Function MakeDictStrInt(ByVal dr As IDataReader) As Dictionary(Of String, Integer)
        Dim dict As New Dictionary(Of String, Integer)
        Try
            While dr.Read()
                dict(CAdoData.GetStr(dr, 0)) = CAdoData.GetInt(dr, 1)
            End While
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return dict
    End Function
    Public Function MakeDictStrStr(ByVal dr As IDataReader) As Dictionary(Of String, String)
        Dim dict As New Dictionary(Of String, String)
        Try
            While dr.Read()
                dict(CAdoData.GetStr(dr, 0)) = CAdoData.GetStr(dr, 1)
            End While
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return dict
    End Function
    Public Function MakeDictStrListStr(ByVal dr As IDataReader) As Dictionary(Of String, List(Of String))
        Dim dict As New Dictionary(Of String, List(Of String))
        Try
            While dr.Read()
                Add(dict, CAdoData.GetStr(dr, 0), CAdoData.GetStr(dr, 1))
            End While
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return dict
    End Function
    Public Function MakeDictDictListStr(ByVal dr As IDataReader) As Dictionary(Of String, Dictionary(Of String, List(Of String)))
        Dim dict As New Dictionary(Of String, Dictionary(Of String, List(Of String)))()
        Try
            While dr.Read()
                Add(dict, CAdoData.GetStr(dr, 0), CAdoData.GetStr(dr, 1), CAdoData.GetStr(dr, 2))
            End While
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return dict
    End Function

    Public Function MakeNameValueList(ByVal dr As IDataReader) As CNameValueList
        Dim list As New CNameValueList
        Try
            While dr.Read()
                list.Add(CAdoData.GetStr(dr, 0), IIf(dr.IsDBNull(1), Nothing, dr(1)))
            End While
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return list
    End Function

    'DataTables
    Public Function MakeDictIntInt(ByVal dt As DataTable) As Dictionary(Of Integer, Integer)
        Dim dict As New Dictionary(Of Integer, Integer)(dt.Rows.Count)
        For Each dr As DataRow In dt.Rows
            dict(CAdoData.GetInt(dr, 0)) = CAdoData.GetInt(dr, 1)
        Next
        Return dict
    End Function
    Public Function MakeDictGuidGuid(ByVal dt As DataTable) As Dictionary(Of Guid, Guid)
        Dim dict As New Dictionary(Of Guid, Guid)(dt.Rows.Count)
        For Each dr As DataRow In dt.Rows
            dict(CAdoData.GetGuid(dr, 0)) = CAdoData.GetGuid(dr, 1)
        Next
        Return dict
    End Function
    Public Function MakeDictStrInt(ByVal dt As DataTable) As Dictionary(Of String, Integer)
        Dim dict As New Dictionary(Of String, Integer)(dt.Rows.Count)
        For Each dr As DataRow In dt.Rows
            dict(CAdoData.GetStr(dr, 0)) = CAdoData.GetInt(dr, 1)
        Next
        Return dict
    End Function
    Public Function MakeDictStrStr(ByVal dt As DataTable) As Dictionary(Of String, String)
        Dim dict As New Dictionary(Of String, String)(dt.Rows.Count)
        For Each dr As DataRow In dt.Rows
            dict(CAdoData.GetStr(dr, 0)) = CAdoData.GetStr(dr, 1)
        Next
        Return dict
    End Function
    Public Function MakeDictStrListStr(ByVal dt As DataTable) As Dictionary(Of String, List(Of String))
        Dim dict As New Dictionary(Of String, List(Of String))(dt.Rows.Count)
        For Each dr As DataRow In dt.Rows
            Add(dict, CAdoData.GetStr(dr, 0), CAdoData.GetStr(dr, 1))
        Next
        Return dict
    End Function
    Public Function MakeDictDictListStr(ByVal dt As DataTable) As Dictionary(Of String, Dictionary(Of String, List(Of String)))
        Dim dict As New Dictionary(Of String, Dictionary(Of String, List(Of String)))()
        For Each dr As DataRow In dt.Rows
            Add(dict, CAdoData.GetStr(dr, 0), CAdoData.GetStr(dr, 1), CAdoData.GetStr(dr, 2))
        Next
        Return dict
    End Function


    Public Function MakeNameValueList(ByVal dt As DataTable) As CNameValueList
        Dim list As New CNameValueList(dt.Rows.Count)
        For Each dr As DataRow In dt.Rows
            list.Add(CAdoData.GetStr(dr, 0), IIf(dr.IsNull(1), Nothing, dr(1)))
        Next
        Return list
    End Function

    Private Sub Add(d As Dictionary(Of String, List(Of String)), key As String, val As String)
        Dim list As List(Of String) = Nothing
        If Not d.TryGetValue(key, list) Then
            list = New List(Of String)
            d.Add(key, list)
        End If
        list.Add(val)
    End Sub
    Private Sub Add(d As Dictionary(Of String, Dictionary(Of String, List(Of String))), key1 As String, key2 As String, val As String)
        Dim list As Dictionary(Of String, List(Of String)) = Nothing
        If Not d.TryGetValue(key1, list) Then
            list = New Dictionary(Of String, List(Of String))
            d.Add(key1, list)
        End If
        Add(list, key2, val)
    End Sub
#End Region

#Region "List tables"
    Public Overridable ReadOnly Property SqlToListAllTables() As String
        Get
            Return SQL_TO_LIST_ALL_TABLES
        End Get
    End Property
    Public Overridable ReadOnly Property SqlToListAllTables_WithSchema() As String
        Get
            Return SQL_TO_LIST_ALL_TABLES_WITH_SCHEMA
        End Get
    End Property
    Public Overridable ReadOnly Property SqlToListAllViews() As String
        Get
            Return SQL_TO_LIST_ALL_VIEWS
        End Get
    End Property
    Public Overridable ReadOnly Property SqlToListAllViews_WithSchema() As String
        Get
            Return SQL_TO_LIST_ALL_VIEWS_WITH_SCHEMA
        End Get
    End Property
    Public Overridable ReadOnly Property SqlToListTableColumnsAndTypes() As String
        Get
            Return SQL_TO_LIST_TABL_COLUMNS_TYPES
        End Get
    End Property
    Public Overridable ReadOnly Property SqlToListViewColumnsAndTypes() As String
        Get
            Return SQL_TO_LIST_VIEW_COLUMNS_TYPES
        End Get
    End Property
    Public Overridable Function AllTableNames(Optional withSchema As Boolean = False) As List(Of String)
        If TypeOf Me Is COleDb Then
            'Folder path as connection string
            If m_connectionString.ToLower.Contains("fmt=delimited") Then Return AllFileNames()

            'Excel/Access etc: use connection.GetSchema
            Dim cn As System.Data.OleDb.OleDbConnection = CType(Local.Connection(), System.Data.OleDb.OleDbConnection)
            Try
                Dim list As List(Of String) = MakeListString(cn.GetSchema("Tables"), "TABLE_NAME")
                Dim shortList As New List(Of String)(list.Count)
                For Each i As String In list
                    If Not i.StartsWith("MSys") Then shortList.Add(i)
                Next
                Return shortList
            Catch
                Return Nothing
            Finally
                cn.Close()
            End Try
        End If

        Dim s As String = SqlToListAllTables
        If withSchema Then s = SqlToListAllTables_WithSchema
        If String.IsNullOrEmpty(s) Then Return Nothing
        Return MakeListString(s)
    End Function
    Public Overridable Function AllViewNames(Optional withSchema As Boolean = False) As List(Of String)
        Dim s As String = SqlToListAllViews
        If withSchema Then s = SqlToListAllViews_WithSchema
        If String.IsNullOrEmpty(s) Then Return Nothing
        Return MakeListString(s)
    End Function
    Public Overridable Function AllTableColumnsAndTypesAsDict(Optional removeSysAndDbo As Boolean = True) As Dictionary(Of String, List(Of String))
        If String.IsNullOrEmpty(SqlToListTableColumnsAndTypes) Then Return Nothing

        Dim d As Dictionary(Of String, List(Of String)) = MakeDictStrListStr(SqlToListTableColumnsAndTypes)

        If removeSysAndDbo Then
            For Each i As String In New List(Of String)(d.Keys)
                If i.StartsWith("sys.") OrElse i.StartsWith("dbo.sys") Then d.Remove(i)
            Next
        End If

        Return d
    End Function
    Public Overridable Function AllViewColumnsAndTypesAsDict(Optional removeSysAndDbo As Boolean = True) As Dictionary(Of String, List(Of String))
        If String.IsNullOrEmpty(SqlToListViewColumnsAndTypes) Then Return Nothing

        Dim d As Dictionary(Of String, List(Of String)) = MakeDictStrListStr(SqlToListViewColumnsAndTypes)

        If removeSysAndDbo Then
            For Each i As String In New List(Of String)(d.Keys)
                If i.StartsWith("sys.") OrElse i.StartsWith("dbo.sys") Then d.Remove(i)
            Next
        End If

        Return d
    End Function
    Private Function AllFileNames() As List(Of String)
        Dim list As New List(Of String)
        Dim i As Integer = m_connectionString.ToLower.IndexOf("data source=")
        If -1 = i Then Return list

        Dim s As String = m_connectionString.Substring(i + 12)
        i = s.IndexOf(";")
        If -1 <> i Then s = s.Substring(0, i)
        If Not IO.Directory.Exists(s) Then Return list

        For Each path As String In IO.Directory.GetFiles(s)
            Select Case IO.Path.GetExtension(path).ToLower
                Case ".csv", ".tab", ".asc" : list.Add(IO.Path.GetFileName(path))
            End Select
        Next
        Return list
    End Function
#End Region

#Region "Identity Insert"
    Public Overridable Sub SetIdentityInsert(ByVal allow As Boolean, ByVal tableName As String, ByVal txOrNull As IDbTransaction)
        ExecuteNonQuery(String.Concat("SET IDENTITY_INSERT ", tableName, IIf(allow, " ON", " OFF")), txOrNull)
    End Sub

    Public Shared Sub InsertDataset(ByVal ds As DataSet, ByVal target As CDataSrc)
        InsertDataset(ds, target, EXCEL_DEFAULT_TABLE_NAME)
    End Sub
    Public Shared Sub InsertDataset(ByVal ds As DataSet, ByVal target As CDataSrc, ByVal tableName As String)
        InsertDataset(ds, target, tableName, Nothing, False)
    End Sub
    Public Shared Sub InsertDataset(ByVal ds As DataSet, ByVal target As CDataSrc, ByVal tableName As String, ByVal pkName As String, ByVal insertPk As Boolean)
        Dim dt As DataTable = ds.Tables(0)

        Dim data As New CNameValueList(dt.Columns.Count)
        For Each i As DataColumn In dt.Columns
            data.Add(i.ColumnName, Nothing)
            'If i.ColumnName = "NoName" Then Continue For
        Next

        For Each dr As DataRow In dt.Rows
            For i As Integer = 0 To dt.Columns.Count - 1
                If i >= data.Count Then Continue For
                data(i).Value = dr.Item(i)
            Next
            target.Insert(tableName, pkName, insertPk, data, Nothing, Nothing)
        Next
    End Sub
#End Region




#Region "Shared - Csv Export"
    'Export Constants
    Private Shared COMMA As Char = (",").ToCharArray()(0)
    Private Shared DOUBLE_QUOTE As Char = ("""").ToCharArray()(0) ' Convert.ToChar("""") 'CChar("""")
    Private Const DOUBLE_QUOTEx2 As String = """"""
    Private Const DEFAULT_NAME As String = "Export"

    'Http overloads (Dataset/Datatable)
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal response As System.Web.HttpResponse)
        ExportToCsv(ds, response, DEFAULT_NAME)
    End Sub
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal response As System.Web.HttpResponse, ByVal fileName As String)
        ExportToCsv(ds, response, fileName, 0)
    End Sub
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal startAtColumn As Integer)
        ExportToCsv(ds, response, fileName, startAtColumn, Nothing)
    End Sub
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal startAtColumn As Integer, ByVal chopColumnNamePrefix As String)
        ExportToCsv(ds.Tables(0), response, fileName, startAtColumn, chopColumnNamePrefix)
    End Sub
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal chopColumnNamePrefix As String) 'For backwards compat
        ExportToCsv(ds.Tables(0), response, fileName, 0, chopColumnNamePrefix)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse)
        ExportToCsv(dt, response, DEFAULT_NAME)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse, ByVal fileName As String)
        ExportToCsv(dt, response, fileName, 0)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal startAtColumn As Integer)
        ExportToCsv(dt, response, fileName, startAtColumn, Nothing)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal startAtColumn As Integer, ByVal chopColumnNamePrefix As String)
        ExportToCsv(dt, response, fileName, startAtColumn, chopColumnNamePrefix, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal chopColumnNamePrefix As String) 'For backwards compat
        ExportToCsv(dt, response, fileName, 0, chopColumnNamePrefix, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal startAtColumn As Integer, ByVal chopColumnNamePrefix As String, ByVal delimiter As Char)
        ExportToCsv(response, fileName)
        ExportToCsv(dt, response.OutputStream, startAtColumn, chopColumnNamePrefix, delimiter)
        response.End()
    End Sub

    'Dataset to Csv
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal filePath As String)
        ExportToCsv(ds.Tables(0), filePath, 0)
    End Sub
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal stream As IO.Stream)
        ExportToCsv(ds.Tables(0), stream, 0)
    End Sub

    'Main overloads - file/stream
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal filePath As String)
        ExportToCsv(dt, filePath, 0)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal stream As IO.Stream)
        ExportToCsv(dt, stream, 0)
    End Sub

    'Overloads - StartAt Column
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal filePath As String, ByVal startAtColumn As Integer)
        ExportToCsv(dt, filePath, startAtColumn, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal stream As IO.Stream, ByVal startAtColumn As Integer)
        ExportToCsv(dt, stream, startAtColumn, COMMA)
    End Sub

    'Overloads - name Prefix Char
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal s As IO.Stream, ByVal startAtColumn As Integer, ByVal chopNamePrefix As String)
        ExportToCsv(dt, New IO.StreamWriter(s), startAtColumn, chopNamePrefix, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal filePath As String, ByVal startAtColumn As Integer, ByVal chopNamePrefix As String)
        ExportToCsv(dt, filePath, startAtColumn, chopNamePrefix, COMMA)
    End Sub

    'Overloads - Sep Char
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal s As IO.Stream, ByVal startAtColumn As Integer, ByVal chopNamePrefix As String, ByVal sepChar As Char)
        ExportToCsv(dt, New IO.StreamWriter(s), startAtColumn, chopNamePrefix, sepChar)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal filePath As String, ByVal startAtColumn As Integer, ByVal chopNamePrefix As String, ByVal sepChar As Char)
        Dim writer As New System.IO.StreamWriter(filePath)
        ExportToCsv(dt, writer, startAtColumn, chopNamePrefix, sepChar)
    End Sub

    'Non-Dataset overloads (e.g. direct from high-level objects)
    Public Shared Sub ExportToCsv(ByVal response As HttpResponse, ByVal fileName As String)
        If Not fileName.ToLower.EndsWith(".csv") Then fileName &= ".csv"
        With response
            .ContentType = "text/csv"
            .AddHeader("content-disposition", String.Concat("attachment; filename=", fileName))
        End With
    End Sub
    Public Shared Sub ExportToCsv(ByVal response As HttpResponse, ByVal ParamArray headings As String())
        Dim sw As New IO.StreamWriter(response.OutputStream)
        ExportToCsv(sw, headings)
        sw.Flush()
    End Sub
    Public Shared Sub ExportToCsv(ByVal response As HttpResponse, ByVal ParamArray dataRow As Object())
        Dim sw As New IO.StreamWriter(response.OutputStream)
        ExportToCsv(sw, dataRow)
        sw.Flush()
    End Sub
    Public Shared Sub ExportToCsv(ByVal writer As IO.StreamWriter, ByVal ParamArray headings As String())
        ExportToCsv(headings, writer)
    End Sub
    Public Shared Sub ExportToCsv(ByVal writer As IO.StreamWriter, ByVal ParamArray dataRow As Object())
        ExportToCsv(dataRow, writer)
    End Sub
    Public Shared Sub ExportToCsv(ByVal headings As String(), ByVal writer As IO.StreamWriter)
        ExportToCsv(headings, writer, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal data As Object(), ByVal writer As IO.StreamWriter)
        ExportToCsv(data, writer, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal data As Object(), ByVal writer As IO.StreamWriter, ByVal sepChar As Char)
        Dim sb As StringBuilder = Nothing
        For Each i As Object In data
            If IsNothing(sb) Then sb = New StringBuilder() Else sb.Append(sepChar)
            sb.Append(StripNewLines(i, sepChar))
        Next
        writer.WriteLine(sb.ToString())
    End Sub
    Public Shared Sub ExportToCsv(ByVal data As String(), ByVal writer As IO.StreamWriter, ByVal sepChar As Char)
        Dim sb As StringBuilder = Nothing
        For Each i As String In data
            If IsNothing(sb) Then sb = New StringBuilder() Else sb.Append(sepChar)
            sb.Append(StripNewLines(i, sepChar))
        Next
        writer.WriteLine(sb.ToString())
    End Sub


    'Private CSV logic
    Private Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal writer As IO.StreamWriter, ByVal startAtColumn As Integer, ByVal chopNamePrefix As String, ByVal sepChar As Char)
        '1. write a line with the column names
        Dim headings As New List(Of String)(dt.Columns.Count)
        For Each col As DataColumn In dt.Columns
            If col.Ordinal < startAtColumn Then Continue For
            If String.IsNullOrEmpty(chopNamePrefix) OrElse Not col.ColumnName.ToLower.StartsWith(chopNamePrefix.ToLower) Then
                headings.Add(col.ColumnName)
            Else
                headings.Add(col.ColumnName.Substring(chopNamePrefix.Length))
            End If
        Next
        ExportToCsv(headings.ToArray, writer, sepChar)

        '2. write all the data rows
        Dim data As New List(Of Object)(dt.Columns.Count)
        For Each row As DataRow In dt.Rows
            data.Clear()
            For Each col As DataColumn In dt.Columns
                If col.Ordinal < startAtColumn Then Continue For
                data.Add(row(col))
            Next
            ExportToCsv(data.ToArray, writer, sepChar)
        Next

        writer.Close()
    End Sub
    Private Shared Function StripNewLines(ByVal obj As Object, ByVal sepChar As Char) As String
        'Null Values
        If IsNothing(obj) Then Return String.Empty
        If TypeOf obj Is Integer AndAlso Integer.MinValue = Convert.ToInt32(obj) Then Return String.Empty
        If TypeOf obj Is Decimal AndAlso Decimal.MinValue = Convert.ToDecimal(obj) Then Return String.Empty
        If TypeOf obj Is DateTime AndAlso DateTime.MinValue = Convert.ToDateTime(obj) Then Return String.Empty
        If TypeOf obj Is Double AndAlso Double.IsNaN(Convert.ToDouble(obj)) Then Return String.Empty

        'Binary Data
        If TypeOf obj Is Byte() Then Return String.Concat("0x", CBinary.BytesToHex(CType(obj, Byte())))

        'Special Encoding
        Return StripNewLines(obj.ToString(), sepChar)
    End Function
    Private Shared Function StripNewLines(ByVal s As String, ByVal sepChar As Char) As String
        If String.IsNullOrEmpty(s) Then Return String.Empty

        'These 3 delimiter chars need special encoding, so encase the whole string in doublequotes, and replace any " with ""
        If s.Contains(sepChar) OrElse s.Contains(vbCr) OrElse s.Contains(vbLf) OrElse s.Contains(DOUBLE_QUOTE) Then
            Return String.Concat(DOUBLE_QUOTE, s.Replace(DOUBLE_QUOTE, DOUBLE_QUOTEx2), DOUBLE_QUOTE)
        End If
        Return s
    End Function
#End Region

#Region "Detect 64-bit"
    Public Shared Function IsPlatformX64() As Boolean
        Return 8 = IntPtr.Size
    End Function
#End Region

#Region "Timezone Offset Correction"
    Private m_offset As TimeSpan = TimeSpan.MinValue
    Public Overridable ReadOnly Property Offset As TimeSpan
        Get
            If m_offset = TimeSpan.MinValue Then
                Try
                    Dim d As DateTime = DirectCast(Me.ExecuteScalar("SELECT GETDATE()"), DateTime)
                    Dim dd As DateTime = DateTime.Now
                    Dim hrs As Double = Math.Round(dd.Subtract(d).TotalHours)
                    m_offset = TimeSpan.FromHours(hrs)
                Catch ex As Exception
                    m_offset = TimeSpan.Zero
                End Try
            End If
            Return m_offset
        End Get
    End Property
#End Region


    'Schema Info
    Public Overridable Function SchemaInfo() As CSchemaInfo
        Return New CSchemaInfo(Me)
        'Throw New Exception("SchemaInfo not implemented on " & Me.GetType().ToString)
    End Function

#Region "Constants - SqlClient"
    'Avoids blocking due to transactions
    'Private Const HINT As String = " WITH (READPAST)" 'Option A: No blocking, no dirty reads
    Private Const HINT As String = " WITH (NOLOCK)"   'Option B: Dirty reads (returns uncommited records)"

    Public Const SQL_TO_LIST_ALL_TABLES As String = "SELECT [Name] AS [Tables] FROM [sysobjects] WHERE xtype='U' ORDER BY [Name]"
    Public Const SQL_TO_LIST_ALL_VIEWS As String = "SELECT [Name] AS [Tables] FROM [sysobjects] WHERE xtype='V' ORDER BY [Name]"

    Public Const SQL_TO_LIST_ALL_TABLES_WITH_SCHEMA As String = "SELECT TABLE_SCHEMA + '.' + TABLE_NAME FROM information_schema.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_SCHEMA,TABLE_NAME"
    Public Const SQL_TO_LIST_ALL_VIEWS_WITH_SCHEMA As String = "SELECT TABLE_SCHEMA + '.' + TABLE_NAME FROM information_schema.VIEWS  ORDER BY TABLE_SCHEMA,TABLE_NAME"       ',VIEW_DEFINITION 

    Public Const SQL_TO_LIST_TABL_COLUMNS_TYPES As String = "SELECT t.TABLE_SCHEMA + '.' + t.TABLE_NAME As TableName,
	c.COLUMN_NAME+' '+
	UPPER(c.DATA_TYPE COLLATE SQL_Latin1_General_CP1_CI_AS+ 
	CASE 
		WHEN c.DATA_TYPE = 'DECIMAL' 
			THEN '(' + CONVERT(varchar(10), c.NUMERIC_PRECISION) + ',' +  + CONVERT(varchar(10), c.NUMERIC_SCALE) + ')' 
			ELSE ISNULL('(' + REPLACE(CONVERT(varchar(10), c.CHARACTER_MAXIMUM_LENGTH) + ')', '-1', 'MAX'),'')
	END		
	+ REPLACE(REPLACE(IS_NULLABLE, 'NO', ' NOT NULL'), 'YES', ' NULL')) AS ColumnNameAndType

		
		FROM		information_schema.columns	c 
		INNER JOIN	information_schema.TABLES	t on c.TABLE_NAME = t.TABLE_NAME 
		ORDER BY c.table_name, ordinal_position"

    Public Shared ReadOnly SQL_TO_LIST_VIEW_COLUMNS_TYPES As String = SQL_TO_LIST_TABL_COLUMNS_TYPES.Replace("information_schema.TABLES", "information_schema.VIEWS")

    Public Const SQL_TO_LIST_STORED_PROCS_INCLUD_DBO As String = "SELECT '[' + SPECIFIC_SCHEMA + '].[' + SPECIFIC_NAME+ ']', ROUTINE_DEFINITION  FROM information_schema.routines WHERE routine_type = 'PROCEDURE' "
    Public Const SQL_TO_LIST_STORED_PROCS_EXCEPT_DBO As String = "SELECT '[' + SPECIFIC_SCHEMA + '].[' + SPECIFIC_NAME+ ']', ROUTINE_DEFINITION  FROM information_schema.routines WHERE routine_type = 'PROCEDURE' AND SPECIFIC_SCHEMA <> 'dbo'"

    Public Const SQL_TO_LIST_FUNCTIONS_INCLUD_DBO As String = "SELECT '[' + SPECIFIC_SCHEMA + '].[' + SPECIFIC_NAME+ ']', ROUTINE_DEFINITION  FROM information_schema.routines WHERE routine_type = 'FUNCTION' "
    Public Const SQL_TO_LIST_FUNCTIONS_EXCEPT_DBO As String = "SELECT '[' + SPECIFIC_SCHEMA + '].[' + SPECIFIC_NAME+ ']', ROUTINE_DEFINITION  FROM information_schema.routines WHERE routine_type = 'FUNCTION' AND SPECIFIC_SCHEMA <> 'dbo'"

    Public Const SQL_TO_LIST_PKS As String = "SELECT
SCHEMA_NAME(t.schema_id) +'.'+  object_name(i.object_id) as  SchemaAndTableName, 
i.name AS PKName, 
c.name AS ColumnName, 
c.is_identity AS IsIdentity, 
idc.last_value as LastValue

FROM sys.indexes i
INNER JOIN sys.index_columns ic on ic.object_id = i.object_id and ic.index_id = i.index_id
INNER JOIN sys.columns c on c.object_id = ic.object_id and c.column_id = ic.column_id
LEFT OUTER JOIN sys.identity_columns  idc on idc.object_id = c.object_id and idc.column_id = c.column_id
INNER JOIN sys.tables t on c.object_id=t.object_id
WHERE i.is_primary_key = 1
ORDER BY SchemaAndTableName, ColumnName"

    Public Const SQL_TO_LIST_FKS As String = "SELECT 
OBJECT_SCHEMA_NAME(f.parent_object_id,DB_ID()) + '.' +  OBJECT_NAME(f.parent_object_id)	AS TableName,
f.name                                                                                  AS ForeignKeyName,
f.update_referential_action																As CascadeUpdate,
f.delete_referential_action																As CascadeDelete,
COL_NAME(fc.parent_object_id,     fc.parent_column_id) + '/' +
OBJECT_SCHEMA_NAME(f.referenced_object_id,DB_ID()) + '.' +  
OBJECT_NAME (f.referenced_object_id) + '/' + 
COL_NAME(fc.referenced_object_id, fc.referenced_column_id)								AS ColumnName_RefTable_RefColumn

FROM       sys.foreign_keys        AS f 
INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id"

    Public Const SQL_TO_LIST_INDICES_ALLTABLES As String = "SELECT 
OBJECT_SCHEMA_NAME(T.[object_id],DB_ID())
+ '.' +
T.[name]
+ '.' +
I.[name]  AS SchemaTableIndex,
AC.[name] AS ColumnName

FROM       sys.[tables] AS     T  
INNER JOIN sys.[indexes]       I  ON T.[object_id] = I.[object_id]  
INNER JOIN sys.[index_columns] IC ON I.[object_id] = IC.[object_id] AND I.index_id=IC.index_id
INNER JOIN sys.[all_columns]   AC ON T.[object_id] = AC.[object_id] AND IC.[column_id] = AC.[column_id] 

WHERE T.[is_ms_shipped] = 0
AND I.[type_desc] <> 'HEAP'
AND is_primary_key = @isPrimary
AND is_unique = @isUnique

ORDER BY SchemaTableIndex, ColumnName"

    Public Const SQL_TO_LIST_INDICES_BY_TABLE As String = "SELECT 
OBJECT_SCHEMA_NAME(T.[object_id],DB_ID())+ '.' +T.[name]  AS SchemaTable,
I.[name]  AS IndexName,
AC.[name] AS ColumnName

FROM       sys.[tables] AS     T  
INNER JOIN sys.[indexes]       I  ON T.[object_id] = I.[object_id]  
INNER JOIN sys.[index_columns] IC ON I.[object_id] = IC.[object_id] AND I.index_id=IC.index_id
INNER JOIN sys.[all_columns]   AC ON T.[object_id] = AC.[object_id] AND IC.[column_id] = AC.[column_id] 

WHERE T.[is_ms_shipped] = 0
AND I.[type_desc] <> 'HEAP'
AND is_primary_key = @isPrimary
AND is_unique = @isUnique

ORDER BY SchemaTable, IndexName, ColumnName"
    ' --, I.[type_desc], I.[is_unique], I.[data_space_id], I.[ignore_dup_key], I.[is_primary_key],I.[is_unique_constraint], I.[fill_factor],    I.[is_padded], I.[is_disabled], I.[is_hypothetical], I.[allow_row_locks], I.[allow_page_locks], IC.[is_descending_key], IC.[is_included_column]

#End Region



#Region "Schema Info"

    Public Function PrimaryKeys_() As Dictionary(Of String, List(Of String))
        Return Dict2ToDict1(PrimaryKeys)
    End Function
    Public Function PrimaryKeys__() As List(Of String)
        Return DictToList(PrimaryKeys_)
    End Function
    Public Function PrimaryKeyHashes() As List(Of String)
        Return HashDict(PrimaryKeys_)
    End Function
    Public Function PrimaryKeyHash() As Guid
        Return HashList(HashDict(PrimaryKeys_))
    End Function




    Public Function IndexesByTable_(isUnique As Boolean, Optional isPrimary As Boolean = False) As Dictionary(Of String, List(Of String))
        Return Dict2ToDict1(IndexesByTable(isUnique, isPrimary))
    End Function
    Public Function Indexes_(isUnique As Boolean, Optional isPrimary As Boolean = False) As List(Of String)
        Return DictToList(Indexes(isUnique, isPrimary))
    End Function
    Public Function IndexHashes(isUnique As Boolean, Optional isPrimary As Boolean = False) As List(Of String)
        Return HashDict(Indexes(isUnique, isPrimary))
    End Function
    Public Function IndexesByTable_() As Dictionary(Of String, List(Of String))
        Dim uniq As Dictionary(Of String, List(Of String)) = IndexesByTable_(True)
        Dim clus As Dictionary(Of String, List(Of String)) = IndexesByTable_(False)
        Return Join(uniq, clus)
    End Function
    Public Function IndexesByTable() As Dictionary(Of String, Dictionary(Of String, List(Of String)))
        Dim uniq As Dictionary(Of String, Dictionary(Of String, List(Of String))) = IndexesByTable(True)
        Dim clus As Dictionary(Of String, Dictionary(Of String, List(Of String))) = IndexesByTable(False)
        Return Join(uniq, clus)
    End Function
    Public Function Indexes_() As List(Of String)
        Dim uniq As List(Of String) = Indexes_(True)
        Dim clus As List(Of String) = Indexes_(False)
        Return Join(uniq, clus)
    End Function
    Public Function IndexHashes() As List(Of String)
        Dim uniq As List(Of String) = IndexHashes(True)
        Dim clus As List(Of String) = IndexHashes(False)
        Return Join(uniq, clus)
    End Function


    Public Overridable Function LatestMigration() As CMigration
        Return New CMigration(Me)
    End Function
    Public Overridable Function MigrationHistory() As CMigrationHistory
        Return New CMigrationHistory(Me)
    End Function

    Public Overridable Function PrimaryKeys() As Dictionary(Of String, Dictionary(Of String, List(Of String)))
        Return MakeDictDictListStr(SQL_TO_LIST_PKS)
    End Function
    Public Overridable Function ForeignKeys() As DataSet
        Return ExecuteDataSet(SQL_TO_LIST_FKS)
    End Function
    Public Overridable Function IndexesByTable(isUnique As Boolean, Optional isPrimary As Boolean = False) As Dictionary(Of String, Dictionary(Of String, List(Of String)))
        Return MakeDictDictListStr(SQL_TO_LIST_INDICES_BY_TABLE, New CNameValueList("isUnique", isUnique, "isPrimary", isPrimary), False)
    End Function
    Public Overridable Function Indexes(isUnique As Boolean, Optional isPrimary As Boolean = False) As Dictionary(Of String, List(Of String))
        Return MakeDictStrListStr(SQL_TO_LIST_INDICES_ALLTABLES, New CNameValueList("isUnique", isUnique, "isPrimary", isPrimary), False)
    End Function
    Public Overridable Function FunctionNames(Optional includeDbo As Boolean = True) As List(Of String)
        Dim suffix As String = String.Empty
        Return MakeListString(SqlToListProcs(True, includeDbo))
    End Function
    Public Overridable Function FunctionNamesAndScriptsTrunc(Optional includeDbo As Boolean = True) As Dictionary(Of String, String)
        Return MakeDictStrStr(SqlToListProcs(True, includeDbo))
    End Function
    Public Overridable Function StoredProcNames(Optional includeDbo As Boolean = True) As List(Of String)
        Dim suffix As String = String.Empty
        Return MakeListString(SqlToListProcs(False, includeDbo))
    End Function
    Public Overridable Function StoredProcNamesAndScriptsTrunc(Optional includeDbo As Boolean = True) As Dictionary(Of String, String)
        Return MakeDictStrStr(SqlToListProcs(False, includeDbo))
    End Function

    'Stored Proc helpers
    Public Overridable Function SqlToListProcs(functions As Boolean, includeDbo As Boolean) As String
        If functions Then
            If includeDbo Then
                Return SQL_TO_LIST_FUNCTIONS_INCLUD_DBO
            Else
                Return SQL_TO_LIST_FUNCTIONS_EXCEPT_DBO
            End If
        Else
            If includeDbo Then
                Return SQL_TO_LIST_STORED_PROCS_INCLUD_DBO
            Else
                Return SQL_TO_LIST_STORED_PROCS_EXCEPT_DBO
            End If
        End If
    End Function
    Public Overridable Function StoredProcs(Optional fastButTrunc As Boolean = False) As Dictionary(Of String, String)
        If fastButTrunc Then Return FunctionNamesAndScriptsTrunc()

        Return StoredProcText(StoredProcNames)
    End Function
    Public Overridable Function Functions(Optional fastButTrunc As Boolean = False) As Dictionary(Of String, String)
        If fastButTrunc Then Return FunctionNamesAndScriptsTrunc()

        Return StoredProcText(FunctionNames())
    End Function
    Private Function NamesToText(list As List(Of String)) As Dictionary(Of String, String)
        Dim dict As New Dictionary(Of String, String)(list.Count)
        For Each i As String In list
            dict.Add(i, StoredProcText(i))
        Next
        Return dict
    End Function
    Public Overridable Function StoredProcText(spNames As List(Of String)) As Dictionary(Of String, String)
        Dim dict As New Dictionary(Of String, String)(spNames.Count)
        Parallel.ForEach(Of String)(spNames,
            Sub(i)
                Dim sql As String = StoredProcText(i)
                SyncLock (dict)
                    dict.Add(i, sql)
                End SyncLock
            End Sub)
        Return dict
    End Function
    Public Overridable Function StoredProcText(spName As String) As String
        Try
            Return CUtilities.ListToString(MakeListString("EXEC sp_helptext '" & spName & "'"), String.Empty)
        Catch ex As Exception
            If spName.StartsWith("sys.") Then Return spName
            Return ex.Message
        End Try
    End Function





    Public Overridable Function StoredProcHashes(Optional fastButTrunc As Boolean = False) As List(Of String)
        Return HashDict(StoredProcs(fastButTrunc))
    End Function
    Public Overridable Function FunctionHashes(Optional fastButTrunc As Boolean = False) As List(Of String)
        Return HashDict(Functions(fastButTrunc))
    End Function
    Public Overridable Function StoredProcAndFunctionHashes(Optional fastButTrunc As Boolean = False) As List(Of String)
        Dim procs As List(Of String) = StoredProcHashes(fastButTrunc)
        Dim funcs As List(Of String) = FunctionHashes(fastButTrunc)
        Return Join(procs, funcs)
    End Function
#End Region

#Region "SchemaInfo utilities"
    Private Shared Function DictToList(dict As Dictionary(Of String, List(Of String))) As List(Of String)
        Dim list As New List(Of String)(dict.Count)
        For Each i As String In dict.Keys
            Dim vals As List(Of String) = dict(i)
            list.Add(String.Concat(i, " {", CUtilities.ListToString(vals), "}"))
        Next
        Return list
    End Function
    Private Shared Function Dict2ToDict1(dict As Dictionary(Of String, Dictionary(Of String, List(Of String)))) As Dictionary(Of String, List(Of String))
        Dim dict1 As New Dictionary(Of String, List(Of String))(dict.Count)
        For Each i As String In dict.Keys
            Dim vals As Dictionary(Of String, List(Of String)) = dict(i)
            dict1.Add(i, DictToList(vals))
        Next
        Return dict1
    End Function

    Private Shared Function HashDict(dict As Dictionary(Of String, String)) As List(Of String)
        Dim list As New List(Of String)(dict.Count)
        For Each i As String In dict.Keys
            Dim sql As String = dict(i)
            Dim md5 As String = CBinary.MD5(sql)
            list.Add(String.Concat(i, vbTab, md5))
        Next
        Return list
    End Function
    Private Shared Function HashList(list As List(Of String)) As Guid
        Return CBinary.MD5_(CUtilities.ListToString(list))
    End Function
    Private Shared Function HashDict(dict As Dictionary(Of String, List(Of String))) As List(Of String)
        Dim list As New List(Of String)(dict.Count)
        For Each i As String In dict.Keys
            Dim sql As String = CUtilities.ListToString(dict(i))
            Dim md5 As String = CBinary.MD5(sql)
            list.Add(String.Concat(i, vbTab, md5))
        Next
        Return list
    End Function


    Private Shared Function Join(list1 As List(Of String), list2 As List(Of String)) As List(Of String)
        Dim both As New List(Of String)(list1.Count + list2.Count)
        both.AddRange(list1)
        both.AddRange(list2)
        both.Sort()
        Return both
    End Function
    Private Shared Function Join(dict1 As Dictionary(Of String, List(Of String)), dict2 As Dictionary(Of String, List(Of String))) As Dictionary(Of String, List(Of String))
        Dim both As New Dictionary(Of String, List(Of String))(dict1)
        For Each i As KeyValuePair(Of String, List(Of String)) In dict2
            Dim existing As List(Of String) = Nothing
            If both.TryGetValue(i.Key, existing) Then
                existing.AddRange(i.Value)
            Else
                both.Add(i.Key, i.Value)
            End If
        Next
        Return both
    End Function
    Private Shared Function Join(dict1 As Dictionary(Of String, Dictionary(Of String, List(Of String))), dict2 As Dictionary(Of String, Dictionary(Of String, List(Of String)))) As Dictionary(Of String, Dictionary(Of String, List(Of String)))
        Dim both As New Dictionary(Of String, Dictionary(Of String, List(Of String)))(dict1)
        For Each i As KeyValuePair(Of String, Dictionary(Of String, List(Of String))) In dict2
            Dim existing As Dictionary(Of String, List(Of String)) = Nothing
            If both.TryGetValue(i.Key, existing) Then
                both(i.Key) = Join(existing, i.Value)
            Else
                both.Add(i.Key, i.Value)
            End If
        Next
        Return both
    End Function




    'Public Function StoredProcs(dbName As String) As Dictionary(Of String, String)
    '    Dim procs As StoredProcedureCollection = GetStoredProcs(dbName)
    '    Dim dict As New Dictionary(Of String, String)(procs.Count)
    '    'Dim opt As New ScriptingOptions()
    '    For Each i As StoredProcedure In procs
    '        Dim s As Specialized.StringCollection = i.Script
    '        Dim ss As String = CUtilities.ListToString(s, vbCrLf)
    '        dict.Add(i.Name, ss)
    '    Next
    '    Return dict
    'End Function
    'Private Function DatabaseNames() As List(Of String)
    '    Dim dbs As DatabaseCollection = GetDatabases()
    '    Dim list As New List(Of String)(dbs.Count)
    '    For Each i As Database In dbs
    '        list.Add(i.Name)
    '    Next
    '    Return list
    'End Function
    ''Internal
    'Private Function GetStoredProcs(dbName As String) As StoredProcedureCollection
    '    Return GetDatabase(dbName).StoredProcedures
    'End Function
    'Private Function GetDatabases() As DatabaseCollection
    '    Return GetServer.Databases
    'End Function
    ''Helper
    'Private Function GetServer() As Server
    '    Return New Server(New ServerConnection(ConnectionString))
    'End Function
    'Private Function GetDatabase(dbName As String) As Database
    '    Return New Database(GetServer, dbName)
    'End Function
#End Region

End Class


