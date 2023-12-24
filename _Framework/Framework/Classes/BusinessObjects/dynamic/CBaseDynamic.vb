Imports System.Text
Imports System.Xml

' Supports any CDataSrc including CWebSrc
' Uses data readers internally, unless the driver is CWebSrc (otherwise datasets)
' Calls to datareader functions will throw an error if the driver is CWebSrc (AppCode should use Business Objects or DataSets)
<Serializable(), CLSCompliant(True)> _
Public MustInherit Class CBaseDynamic : Inherits CBase

#Region "Constructors"
    'Main Constructors
    Protected Sub New()    'Used for Insert and Select-Multiple
        MyBase.New()
    End Sub
    Protected Sub New(ByVal primaryKey As Object) 'Used for Update and Select-Single
        MyBase.New(primaryKey, Nothing)
    End Sub
    Protected Sub New(ByVal primaryKey As Object, ByVal txOrNull As IDbTransaction) 'Used for Update and Select-Single within a transaction
        MyBase.New(primaryKey, txOrNull)
    End Sub
    Protected Sub New(ByVal dr As IDataReader) 'Used for Select-Multiple
        MyBase.New(dr)
    End Sub
    Protected Sub New(ByVal dr As DataRow) 'Used for Select-Multiple
        MyBase.new(dr)
    End Sub

    'As above, with CDataSrc
    Protected Sub New(ByVal dataSrc As CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal primaryKey As Object)
        MyBase.New(dataSrc, primaryKey)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal primaryKey As Object, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, primaryKey, txOrNull)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As IDataReader)
        MyBase.New(dataSrc, dr)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal row As DataRow)
        MyBase.New(dataSrc, row)
    End Sub
#End Region

#Region "MustOverride/Overridable"
    'Dynamic Sql Properties - Compulsory
    Public MustOverride ReadOnly Property TableName() As String

    'Dynamic Sql Properties - Optional (TODO - make first 3 writable)
    Protected Overridable ReadOnly Property SelectColumns() As String
        Get
            Return "*"
        End Get
    End Property
    Protected Overridable ReadOnly Property ViewName() As String
        Get
            Return TableName
        End Get
    End Property
    Protected Overridable ReadOnly Property OrderByColumns() As String
        Get
            Return String.Empty
        End Get
    End Property
    Protected Overridable ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return False
        End Get
    End Property
    Protected Overridable ReadOnly Property OrdinalName() As String
        Get
            Dim s As String = TableName
            s = s.Replace("tbl", String.Empty)
            If LCase(s.Substring(s.Length - 1, 1)) = "s" Then s = s.Substring(0, s.Length - 1)
            If LCase(s.Substring(s.Length - 2, 2)) = "ie" Then s = s.Substring(0, s.Length - 2) & "y"
            Return s & "Ordinal"
        End Get
    End Property
    Protected Overridable ReadOnly Property OracleSequenceName() As String
        Get
            Return "SEQ_" & TableName
        End Get
    End Property
    Protected Overridable ReadOnly Property PrimaryKeyIsSqlServerSequentialGuid() As Boolean
        Get
            Return False
        End Get
    End Property



    'Internal commands
    Protected Overrides Function SelectIdAsDr(ByVal txOrNull As System.Data.IDbTransaction) As IDataReader
        Return CType(SelectIdAsType(txOrNull, EQueryReturnType.DataReader), IDataReader)
    End Function
    Protected Overrides Function SelectIdAsDs(ByVal txOrNull As System.Data.IDbTransaction) As DataSet
        Return CType(SelectIdAsType(txOrNull, EQueryReturnType.DataSet), DataSet)
    End Function
    Private Function SelectIdAsType(ByVal txOrNull As System.Data.IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return DataSrc.SelectWhere(Me.SelectColumns, Me.ViewName, New CCriteriaList(PrimaryKeys), Me.OrderByColumns, txOrNull, type)
    End Function
#End Region

#Region "Protected - Insert/Update"
    Protected Overrides Sub Insert(ByVal txOrNull As IDbTransaction)
        'Special case - sqlserver sequential guid as PK: Can either generate in database or set manually, both are supported
        Dim insertPk As Boolean = Me.InsertPrimaryKey
        Dim oracleSeq As String = Me.OracleSequenceName
        If PrimaryKeyIsSqlServerSequentialGuid Then
            oracleSeq = CSqlClient.SEQ_GUID_MARKER
            insertPk = Not Guid.Empty.Equals(CType(PrimaryKeyValue, Guid)) 'If a guid has been supplied, then use it, otherwise get from db
        End If

        Dim saveFields As CNameValueList = Me.SaveParameters(insertPk)
        Dim obj As Object = DataSrc.Insert(Me.TableName, Me.PrimaryKeyName, insertPk, saveFields, txOrNull, oracleSeq)
        If Not insertPk Then Me.PrimaryKeyValue = obj
    End Sub
    Protected Overrides Function Update(ByVal txOrNull As System.Data.IDbTransaction) As Integer
        Dim where As New CWhere(Me.TableName, Me.PrimaryKeys, txOrNull)
        Return DataSrc.Update(Me.SaveParameters(False), where)
    End Function
#End Region

#Region "Dynamic Sql"

#Region "Bulk Save/Delete"
    Public Sub BulkSave(ByVal base As CBase)
        Dim al As New ArrayList(1)
        al.Add(base)
        BulkSave(al)
    End Sub
    Public Sub BulkDelete(ByVal base As CBase)
        Dim al As New ArrayList(1)
        al.Add(base)
        BulkDelete(al)
    End Sub
    Public Sub BulkSave(ByVal base1 As CBase, ByVal base2 As CBase)
        Dim al As New ArrayList(2)
        al.Add(base1)
        al.Add(base2)
        BulkSave(al)
    End Sub
    Public Sub BulkDelete(ByVal base1 As CBase, ByVal base2 As CBase)
        Dim al As New ArrayList(2)
        al.Add(base2)
        al.Add(base1)
        BulkDelete(al)
    End Sub
    Public Sub BulkSave(ByVal base As CBase, ByVal list As IList)
        Dim al As New ArrayList(list.Count + 1)
        al.Add(base)
        al.Add(list)
        BulkSave(al)
    End Sub
    Public Sub BulkDelete(ByVal base As CBase, ByVal list As IList)
        Dim al As New ArrayList(list.Count + 1)
        al.Add(list)
        al.Add(base)
        BulkDelete(al)
    End Sub

    Public Sub BulkSave(ByVal array As ICollection)
        If array.Count > 1 Then CacheClear() 'Avoids maintaining the cache for each insert/update
        DataSrc.BulkSave(array)
    End Sub
    Public Sub BulkDelete(ByVal array As ICollection)
        If array.Count > 1 Then CacheClear() 'Avoids maintaining the cache for each delete
        DataSrc.BulkDelete(array)
    End Sub
#End Region

#Region "Delete"
    'Transactional
    Public Function DeleteAll(ByVal txOrNull As IDbTransaction) As Integer
        Try
            DataSrc.ExecuteNonQuery("TRUNCATE TABLE [" + Me.TableName + "]")
        Catch
            DeleteAll = DataSrc.DeleteAll(Me.TableName, txOrNull)
        End Try
        CacheClear()
    End Function
    Public Function DeleteWhere(ByVal columnName As String, ByVal sign As ESign, ByVal columnValue As Object, ByVal txOrNull As IDbTransaction) As Integer
        DeleteWhere = DataSrc.DeleteWhere(Me.TableName, New CCriteria(columnName, sign, columnValue), txOrNull)
        CacheClear()
    End Function
    Public Function DeleteWhere(ByVal where As CCriteriaList, ByVal txOrNull As IDbTransaction) As Integer
        Return DeleteWhere(where, txOrNull, True)
    End Function
    Public Function DeleteWhere(ByVal where As CCriteriaList, ByVal txOrNull As IDbTransaction, ByVal clearCache As Boolean) As Integer
        DeleteWhere = DataSrc.DeleteWhere(Me.TableName, where, txOrNull)
        If clearCache Then CacheClear()
    End Function
    Public Function DeleteWhere(ByVal unsafeWhereClause As String, ByVal txOrNull As IDbTransaction) As Integer
        DeleteWhere = DataSrc.DeleteWhere(Me.TableName, unsafeWhereClause, txOrNull)
        CacheClear()
    End Function

    'Non-Transactional (Overloads)
    Public Function DeleteAll() As Integer
        Return DeleteAll(Nothing)
    End Function
    Public Function DeleteWhere(ByVal columnName As String, ByVal sign As ESign, ByVal columnValue As Object) As Integer
        Return DeleteWhere(columnName, sign, columnValue, Nothing)
    End Function
    Public Function DeleteWhere(ByVal where As CCriteriaList) As Integer
        Return DeleteWhere(where, Nothing)
    End Function
    Public Function DeleteWhere(ByVal unsafeWhereClause As String) As Integer
        Return DeleteWhere(unsafeWhereClause, Nothing)
    End Function
#End Region

#Region "Increment,UpdateOrdinals"
    Public Function UpdateOrdinals(ByVal ordinals As CNameValueList) As Integer
        If IsNothing(ordinals) Then Return -1
        If ordinals.Count = 0 Then Return 0
        Return DataSrc.UpdateOrdinals(TableName, PrimaryKeyName, OrdinalName, ordinals)
    End Function
    Public Sub Increment(ByVal colName As String)
        Dim sql As New StringBuilder("UPDATE ")
        sql.Append(TableName)
        sql.Append(" SET ")
        sql.Append(colName)
        sql.Append("=")
        sql.Append(colName)
        sql.Append("+1 WHERE ")
        sql.Append(PrimaryKeyName)
        sql.Append("=")
        sql.Append(PrimaryKeyValue)
        sql.Append(" AND NOT ")
        sql.Append(colName)
        sql.Append(" IS NULL")

        Dim rowsAffected As Integer = DataSrc.ExecuteNonQuery(sql.ToString(), Nothing)
        If rowsAffected > 0 Then Exit Sub

        sql = New StringBuilder("UPDATE ")
        sql.Append(TableName)
        sql.Append(" SET ")
        sql.Append(colName)
        sql.Append("=1 WHERE ")
        sql.Append(PrimaryKeyName)
        sql.Append("=")
        sql.Append(PrimaryKeyValue)
        sql.Append(" AND ")
        sql.Append(colName)
        sql.Append(" IS NULL")
    End Sub
#End Region

#Region "Select IList"
    'Select - No Paging
    Protected Function SelectAll() As IList
        Return MakeList(DataSrc.SelectAll(Me.SelectColumns, Me.ViewName, Me.OrderByColumns, Nothing, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectAll(ByVal orderBy As String) As IList
        Return MakeList(DataSrc.SelectAll(Me.SelectColumns, Me.ViewName, orderBy, Nothing, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectWhere(ByVal columnName As String, ByVal sign As ESign, ByVal columnValue As Object) As IList
        Return MakeList(DataSrc.SelectWhere(Me.SelectColumns, Me.ViewName, New CCriteria(columnName, sign, columnValue), Me.OrderByColumns, Nothing, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectWhere(ByVal where As CCriteria) As IList
        Return MakeList(DataSrc.SelectWhere(Me.SelectColumns, Me.ViewName, New CCriteriaList(where), Me.OrderByColumns, Nothing, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectWhere(ByVal where As CCriteriaList) As IList
        Return MakeList(DataSrc.SelectWhere(Me.SelectColumns, Me.ViewName, where, Me.OrderByColumns, Nothing, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectWhere(ByVal where As CCriteriaList, ByVal tableOrJoin As String) As IList
        Return MakeList(DataSrc.SelectWhere(Me.SelectColumns, tableOrJoin, where, OrderByColumns, Nothing, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectWhere(ByVal where As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String) As IList
        Return MakeList(DataSrc.SelectWhere(SelectCols(tableOrJoin), tableOrJoin, where, orderBy, Nothing, EQueryReturnType.Optimal))
    End Function
    ' <Obsolete()> _
    Protected Function SelectWhere(ByVal where As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal columns As String) As IList
        'BAckwards compat only, columns ignored
        Return MakeList(DataSrc.SelectWhere(SelectCols(tableOrJoin), tableOrJoin, where, orderBy, Nothing, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectWhere(ByVal unsafeWhereClause As String) As IList
        Return MakeList(DataSrc.SelectWhere(Me.SelectColumns, Me.ViewName, unsafeWhereClause, Me.OrderByColumns, Nothing, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectByIds(ByVal ids As IList) As IList
        Return SelectByIds(Nothing, ids)
    End Function
    Protected Function SelectById(ByVal ParamArray ids As Object()) As IList
        Dim tx As IDbTransaction = Nothing
        Return SelectById(tx, ids) 'Overridable method e.g. stored proc
    End Function
    Private Function BuildWhere(ByVal ids As Object()) As CCriteriaList
        Dim where As New CCriteriaList
        Dim pks As String() = PrimaryKeyNames()
        For i As Integer = 0 To pks.Length - 1
            where.Add(pks(i), ids(i))
        Next
        Return where
    End Function

    'Select - With Paging (Note - Derived class will make public and cast as a typed list)
    Protected Function SelectAll(ByVal pi As CPagingInfo) As IList
        If IsNothing(pi) Then
            Return SelectAll()
        Else
            Return Paging(pi)
        End If
    End Function
    Protected Function SelectWhere(ByVal pi As CPagingInfo, ByVal columnName As String, ByVal sign As ESign, ByVal columnValue As Object) As IList
        If IsNothing(pi) Then
            Return SelectWhere(columnName, sign, columnValue)
        Else
            Return PagingWithFilters(pi, columnName, sign, columnValue)
        End If
    End Function
    Protected Function SelectWhere(ByVal pi As CPagingInfo, ByVal where As CCriteria) As IList
        If IsNothing(pi) Then
            Return SelectWhere(where)
        Else
            Return PagingWithFilters(pi, where)
        End If
    End Function
    Protected Function SelectWhere(ByVal pi As CPagingInfo, ByVal where As CCriteriaList) As IList
        If IsNothing(pi) Then
            Return SelectWhere(where)
        Else
            Return PagingWithFilters(pi, where)
        End If
    End Function
    Protected Function SelectWhere(ByVal pi As CPagingInfo, ByVal where As CCriteriaList, ByVal tableOrJoin As String) As IList
        If IsNothing(pi) Then
            Return SelectWhere(where, tableOrJoin, Me.OrderByColumns)
        Else
            pi.TableName = tableOrJoin
            Return PagingWithFilters(pi, where, SelectCols(tableOrJoin))
        End If
    End Function
    Protected Overridable Function SelectByIds(ByVal pi As CPagingInfo, ByVal ids As IList) As IList
        Return SelectWhere(pi, New CCriteriaList(PrimaryKeyName, ESign.IN, ids))
    End Function

    'Select - With Transactions (For backward compat)
    Protected Function SelectAll(ByVal txOrNull As IDbTransaction) As IList
        Return MakeList(DataSrc.SelectAll(Me.SelectColumns, Me.ViewName, Me.OrderByColumns, txOrNull, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectAll(ByVal orderBy As String, ByVal txOrNull As IDbTransaction) As IList
        Return MakeList(DataSrc.SelectAll(Me.SelectColumns, Me.ViewName, orderBy, txOrNull, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectWhere(ByVal columnName As String, ByVal columnValue As Object, ByVal txOrNull As IDbTransaction) As IList
        Return SelectWhere(columnName, ESign.EqualTo, columnValue, txOrNull)
    End Function
    Protected Function SelectWhere(ByVal columnName As String, ByVal sign As ESign, ByVal columnValue As Object, ByVal txOrNull As IDbTransaction) As IList
        Return SelectWhere(New CCriteria(columnName, sign, columnValue), txOrNull)
    End Function
    Protected Function SelectWhere(ByVal where As CCriteria, ByVal txOrNull As IDbTransaction) As IList
        Return MakeList(DataSrc.SelectWhere(Me.SelectColumns, Me.ViewName, where, Me.OrderByColumns, txOrNull, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectWhere(ByVal where As CCriteriaList, ByVal txOrNull As IDbTransaction) As IList
        Return MakeList(DataSrc.SelectWhere(Me.SelectColumns, Me.ViewName, where, Me.OrderByColumns, txOrNull, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectWhere(ByVal where As CCriteriaList, ByVal tableOrJoin As String, ByVal txOrNull As IDbTransaction) As IList
        Return MakeList(DataSrc.SelectWhere(SelectCols(tableOrJoin), tableOrJoin, where, Me.OrderByColumns, txOrNull, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectWhere(ByVal where As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal txOrNull As IDbTransaction) As IList
        Return MakeList(DataSrc.SelectWhere(SelectCols(tableOrJoin), tableOrJoin, where, orderBy, txOrNull, EQueryReturnType.Optimal))
    End Function
    Protected Function SelectWhere(ByVal unsafeWhereClause As String, ByVal txOrNull As IDbTransaction) As IList
        Return MakeList(DataSrc.SelectWhere(Me.SelectColumns, Me.ViewName, unsafeWhereClause, Me.OrderByColumns, txOrNull, EQueryReturnType.Optimal))
    End Function
    Protected Overridable Function SelectByIds(ByVal ids As IList, ByVal txOrNull As IDbTransaction) As IList
        Return SelectWhere(New CCriteriaList(PrimaryKeyName, ESign.IN, ids), txOrNull)
    End Function
    Protected Overridable Function SelectById(ByVal txOrNull As IDbTransaction, ByVal ParamArray ids As Object()) As IList
        Return SelectWhere(BuildWhere(ids), txOrNull)
    End Function
    '<Obsolete()> _
    Protected Function SelectWhere(ByVal selectColumns As String, ByVal where As CCriteriaList, ByVal tableOrJoin As String, ByVal txOrNull As IDbTransaction) As IList
        Return MakeList(DataSrc.SelectWhere(SelectCols(tableOrJoin), tableOrJoin, where, Me.OrderByColumns, txOrNull, EQueryReturnType.Optimal))
    End Function
    '<Obsolete()> _
    Protected Function SelectWhere(ByVal selectColumns As String, ByVal where As CCriteriaList, ByVal tableOrJoin As String, ByVal orderByColumns As String, ByVal txOrNull As IDbTransaction) As IList
        Return MakeList(DataSrc.SelectWhere(SelectCols(tableOrJoin), tableOrJoin, where, orderByColumns, txOrNull, EQueryReturnType.Optimal))
    End Function

    'Select - LowLevel
    Protected Function SelectWhere(ByVal where As CSelectWhere) As IList
        Return MakeList(DataSrc.Select(where, EQueryReturnType.Optimal))
    End Function

    'Select - Utility to control select-columns list
    Private Function SelectCols(ByVal tableOrJoin As String) As String
        'Normal case - table or view, just return the default list of columns, usually *
        If Not tableOrJoin.ToLower.Contains(" join ") Then Return Me.SelectColumns
        'Join Expression - only return the columns needed, to avoid possible duplicate names
        If "*" = Me.SelectColumns Then
            If tableOrJoin.ToLower.Contains(Me.ViewName.ToLower) Then
                Return String.Concat(Me.ViewName, ".*")
            Else
                Return String.Concat(Me.TableName, ".*")
            End If
        End If
        'List of column names - need to prefix each one
        Dim sb As New StringBuilder
        For Each i As String In Me.SelectColumns.Split(CChar(","))
            If sb.Length > 0 Then sb.Append(",")
            If i.Contains(".") OrElse i.Contains("(") Then sb.Append(i) Else sb.Append(Me.ViewName).Append(".").Append(i.Trim())
        Next
        Return sb.ToString
    End Function

    'Overloads (Internal use)
    Protected Overrides Function DeleteId(ByVal txOrNull As IDbTransaction) As Integer
        Return DeleteWhere(PrimaryKeysAsCriteria, txOrNull, False)
    End Function
    Protected Overridable Function SelectId(ByVal txOrNull As IDbTransaction) As CBase
        Dim data As IList = SelectWhere(PrimaryKeysAsCriteria, txOrNull)
        If data.Count <> 1 Then Throw New Exception(data.Count & " rows return from " & Me.GetType.ToString & ".SelectById(" & PrimaryKeyValue.ToString & ")")
        Return CType(data(0), CBase)
    End Function
    Protected Overridable Function DeleteWhere(ByVal where As CCriteria, ByVal txOrNull As IDbTransaction) As Integer
        DeleteWhere = DataSrc.DeleteWhere(Me.TableName, where, txOrNull)
        CacheClear()
    End Function
#End Region

#Region "Select Support - Paging/PagingWithFilters"
    'Select-All with Paging
    Private Function Paging(ByVal pi As CPagingInfo) As IList
        With pi
            If String.IsNullOrEmpty(.TableName) Then .TableName = Me.ViewName
            If String.IsNullOrEmpty(.SortByColumn) Then .SortByColumn = RestrictSortByColumn(Me.OrderByColumns, .Descending, .TableName) Else SeparateDesc(.SortByColumn, .Descending, .TableName)
            Return MakeList(DataSrc.Paging(.Count, .PageIndex, .PageSize, .TableName, .Descending, .SortByColumn, SelectCols(.TableName)))
        End With
    End Function

    'Select-Where with Paging (Criteria vs CriteriaList)
    Private Function PagingWithFilters(ByVal pi As CPagingInfo, ByVal columnName As String, ByVal sign As ESign, ByVal columnValue As Object) As IList
        Return PagingWithFilters(pi, New CCriteria(columnName, sign, columnValue))
    End Function
    Private Function PagingWithFilters(ByVal pi As CPagingInfo, ByVal where As CCriteria) As IList
        Return PagingWithFilters(pi, New CCriteriaList(where))
    End Function
    Private Function PagingWithFilters(ByVal pi As CPagingInfo, ByVal where As CCriteriaList) As IList
        Return PagingWithFilters(pi, where, SelectColumns)
    End Function
    Private Function PagingWithFilters(ByVal pi As CPagingInfo, ByVal where As CCriteriaList, ByVal selectCols As String) As IList
        If where.Count = 0 Then Return Paging(pi)

        With pi
            If String.IsNullOrEmpty(.TableName) Then .TableName = Me.ViewName
            If String.IsNullOrEmpty(.SortByColumn) Then .SortByColumn = RestrictSortByColumn(Me.OrderByColumns, .Descending, .TableName) Else SeparateDesc(.SortByColumn, .Descending, .TableName)
            selectCols = Me.SelectCols(.TableName)
            Return MakeList(DataSrc.PagingWithFilters(.Count, .PageIndex, .PageSize, .TableName, .Descending, .SortByColumn, selectCols, where))
        End With
    End Function

    'Utility - Can only sort by one column at most
    Private Function RestrictSortByColumn(ByVal sortByColumn As String, ByRef descending As Boolean, ByVal viewName As String) As String
        If String.IsNullOrEmpty(sortByColumn) Then sortByColumn = Me.PrimaryKeyName
        If sortByColumn.Contains(",") Then sortByColumn = sortByColumn.Split(CChar(","))(0)
        SeparateDesc(sortByColumn, descending, viewName)
        Return sortByColumn
    End Function
    Private Sub SeparateDesc(ByRef sortByColumn As String, ByRef descending As Boolean, ByVal viewName As String)
        If sortByColumn.ToLower.Trim.EndsWith(" desc") Then
            sortByColumn = sortByColumn.Substring(0, sortByColumn.ToLower.IndexOf(" desc"))
            descending = True
        End If

        'Hack - complex queries on non-unique columns with paging needs a more specific order-by
        If Not sortByColumn.Contains(".") AndAlso Not sortByColumn.Contains(viewName) AndAlso Not viewName.ToLower().Contains(" join ") Then
            sortByColumn = String.Concat(viewName, ".", sortByColumn)
        End If
    End Sub
#End Region

#Region "Select Datasets (Rarely used, normally return IList)"
    'SelectAll
    Public Function SelectAll_Dataset() As DataSet
        Return DataSrc.SelectAll_Dataset(Me.SelectColumns, Me.ViewName, Me.OrderByColumns)
    End Function
    Public Function SelectAll_Dataset(ByVal tableOrJoin As String) As DataSet
        Return DataSrc.SelectAll_Dataset(Me.SelectColumns, tableOrJoin, Me.OrderByColumns)
    End Function
    Public Function SelectAll_Dataset(ByVal columns As String, ByVal tableOrJoin As String) As DataSet
        Return DataSrc.SelectAll_Dataset(columns, tableOrJoin, Me.OrderByColumns)
    End Function
    Public Function SelectAll_Dataset(ByVal columns As String, ByVal tableOrJoin As String, ByVal orderByExpression As String) As DataSet
        Return DataSrc.SelectAll_Dataset(columns, tableOrJoin, orderByExpression)
    End Function

    'SelectWhere - Trivial shortcuts to DataSrc methods
    Public Function SelectWhere_Dataset(ByVal where As CCriteriaList) As DataSet
        Return DataSrc.SelectWhere_Dataset(Me.SelectColumns, Me.ViewName, Me.OrderByColumns, where)
    End Function
    Public Function SelectWhere_Dataset(ByVal where As CCriteriaList, ByVal tableOrJoinExpression As String) As DataSet
        Return DataSrc.SelectWhere_Dataset(tableOrJoinExpression, Nothing, where)
    End Function
    Public Function SelectWhere_Dataset(ByVal where As CCriteriaList, ByVal tableOrJoinExpression As String, ByVal orderBy As String) As DataSet
        Return DataSrc.SelectWhere_Dataset(tableOrJoinExpression, orderBy, where)
    End Function
    Public Function SelectWhere_Dataset(ByVal where As CCriteriaList, ByVal viewname As String, ByVal orderBy As String, ByVal selectColumns As String) As DataSet
        Return DataSrc.SelectWhere_Dataset(selectColumns, viewname, orderBy, where)
    End Function
    Public Function SelectWhere_Dataset(ByVal columnName As String, ByVal sign As ESign, ByVal columnValue As Object) As DataSet
        Return SelectWhere_Dataset(New CCriteriaList(columnName, sign, columnValue))
    End Function

    'Depreciated (wrong order)
    Public Function SelectWhere_Dataset(ByVal tableOrJoinExpression As String, ByVal where As CCriteriaList) As DataSet
        Return DataSrc.SelectWhere_Dataset(tableOrJoinExpression, Nothing, where)
    End Function
    Public Function SelectWhere_Dataset(ByVal tableOrJoinExpression As String, ByVal orderBy As String, ByVal where As CCriteriaList) As DataSet
        Return DataSrc.SelectWhere_Dataset(tableOrJoinExpression, orderBy, where)
    End Function
    Public Function SelectWhere_Dataset(ByVal selectColumns As String, ByVal viewname As String, ByVal orderBy As String, ByVal where As CCriteriaList) As DataSet
        Return DataSrc.SelectWhere_Dataset(selectColumns, viewname, orderBy, where)
    End Function

    'Unsafe
    Public Function SelectWhere_Dataset(ByVal unsafeWhereClause As String) As DataSet
        Return DataSrc.SelectWhere_Dataset(Me.SelectColumns, Me.ViewName, Me.OrderByColumns, unsafeWhereClause)
    End Function
#End Region

#Region "Select Distinct"
    'Simple
    Public Function SelectDistinct(ByVal colName As String) As List(Of String)
        Return DataSrc.SelectDistinctAsStr(Me.TableName, colName)
    End Function
    Public Function SelectDistinctInt(ByVal colName As String) As List(Of Integer)
        Return DataSrc.SelectDistinctAsInt(Me.TableName, colName)
    End Function
    Public Function SelectDistinctGuid(ByVal colName As String) As List(Of Guid)
        Return DataSrc.SelectDistinctAsGuid(Me.TableName, colName)
    End Function
    Public Function SelectDistinctDate(ByVal colName As String) As List(Of DateTime)
        Return DataSrc.SelectDistinctAsDate(Me.TableName, colName)
    End Function

    'Transactional
    Public Function SelectDistinct(ByVal colName As String, ByVal tx As IDbTransaction) As List(Of String)
        Return DataSrc.SelectDistinctAsStr(Me.TableName, colName, tx)
    End Function
    Public Function SelectDistinctInt(ByVal colName As String, ByVal tx As IDbTransaction) As List(Of Integer)
        Return DataSrc.SelectDistinctAsInt(Me.TableName, colName, tx)
    End Function
    Public Function SelectDistinctGuid(ByVal colName As String, ByVal tx As IDbTransaction) As List(Of Guid)
        Return DataSrc.SelectDistinctAsGuid(Me.TableName, colName, tx)
    End Function
    Public Function SelectDistinctDate(ByVal colName As String, ByVal tx As IDbTransaction) As List(Of DateTime)
        Return DataSrc.SelectDistinctAsDate(Me.TableName, colName, tx)
    End Function


    'Flexible (CSelectWhere-based, with 2 overloads)
    Public Function SelectDistinct(ByVal colName As String, ByVal filters As CCriteriaList) As List(Of String)
        Return SelectDistinct(colName, Me.ViewName, filters)
    End Function
    Public Function SelectDistinctInt(ByVal colName As String, ByVal filters As CCriteriaList) As List(Of Integer)
        Return SelectDistinctInt(colName, Me.ViewName, filters)
    End Function
    Public Function SelectDistinctGuid(ByVal colName As String, ByVal filters As CCriteriaList) As List(Of Guid)
        Return SelectDistinctGuid(colName, Me.ViewName, filters)
    End Function
    Public Function SelectDistinctDate(ByVal colName As String, ByVal filters As CCriteriaList) As List(Of DateTime)
        Return SelectDistinctDate(colName, Me.ViewName, filters)
    End Function
    Public Function SelectDistinctDouble(ByVal colName As String, ByVal filters As CCriteriaList) As List(Of Double)
        Return SelectDistinctDouble(colName, Me.ViewName, filters)
    End Function

    Public Function SelectDistinct(ByVal colName As String, ByVal viewName As String, ByVal filters As CCriteriaList) As List(Of String)
        Return SelectDistinct(New CSelectWhere("DISTINCT " & colName, viewName, filters, colName, Nothing))
    End Function
    Public Function SelectDistinctInt(ByVal colName As String, ByVal viewName As String, ByVal filters As CCriteriaList) As List(Of Integer)
        Return SelectDistinctInt(New CSelectWhere("DISTINCT " & colName, viewName, filters, colName, Nothing))
    End Function
    Public Function SelectDistinctGuid(ByVal colName As String, ByVal viewName As String, ByVal filters As CCriteriaList) As List(Of Guid)
        Return SelectDistinctGuid(New CSelectWhere("DISTINCT " & colName, viewName, filters, colName, Nothing))
    End Function
    Public Function SelectDistinctDate(ByVal colName As String, ByVal viewName As String, ByVal filters As CCriteriaList) As List(Of DateTime)
        Return SelectDistinctDate(New CSelectWhere("DISTINCT " & colName, viewName, filters, colName, Nothing))
    End Function
    Public Function SelectDistinctDouble(ByVal colName As String, ByVal viewName As String, ByVal filters As CCriteriaList) As List(Of Double)
        Return SelectDistinctDouble(New CSelectWhere("DISTINCT " & colName, viewName, filters, colName, Nothing))
    End Function

    Public Function SelectDistinct(ByVal w As CSelectWhere) As List(Of String)
        Return DataSrc.MakeListString(w)
    End Function
    Public Function SelectDistinctInt(ByVal w As CSelectWhere) As List(Of Integer)
        Return DataSrc.MakeListInteger(w)
    End Function
    Public Function SelectDistinctGuid(ByVal w As CSelectWhere) As List(Of Guid)
        Return DataSrc.MakeListGuid(w)
    End Function
    Public Function SelectDistinctDate(ByVal w As CSelectWhere) As List(Of DateTime)
        Return DataSrc.MakeListDate(w)
    End Function
    Public Function SelectDistinctDouble(ByVal w As CSelectWhere) As List(Of Double)
        Return DataSrc.MakeListDouble(w)
    End Function
#End Region

#Region "Select Sum"
    Public Function SelectSum(ByVal colName As String) As Decimal
        Return SelectSum(colName, Nothing)
    End Function
    Public Function SelectSum(ByVal colName As String, ByVal join As String) As Decimal
        Return SelectSum(colName, join, Nothing)
    End Function
    Public Function SelectSum(ByVal colName As String, ByVal join As String, ByVal where As CCriteriaList) As Decimal
        Return SelectSum(colName, join, where, Nothing)
    End Function
    Public Function SelectSum(ByVal colName As String, ByVal join As String, ByVal where As CCriteriaList, ByVal txOrNull As IDbTransaction) As Decimal
        colName = String.Concat("SUM(", colName, ")")
        If join Is Nothing Then join = Me.ViewName
        Dim ds As DataSet = CType(DataSrc.SelectWhere(colName, join, where, String.Empty, txOrNull, EQueryReturnType.DataSet), DataSet)
        Dim obj As Object = ds.Tables(0).Rows(0)(0)
        If TypeOf obj Is DBNull Then Return 0
        Return CDec(obj)
    End Function
#End Region

#Region "Select Count"
    Public Function SelectCount() As Integer
        Return SelectCount(Nothing)
    End Function
    Public Function SelectCount(ByVal filters As CCriteriaList) As Integer
        Return SelectCount(filters, Me.ViewName, Nothing)
    End Function
    Public Function SelectCount(ByVal filters As CCriteriaList, ByVal tableNameOrJoin As String) As Integer
        Return SelectCount(filters, tableNameOrJoin, Nothing)
    End Function
    Public Function SelectCount(ByVal filters As CCriteriaList, ByVal txOrNull As IDbTransaction) As Integer
        Return SelectCount(filters, Me.ViewName, txOrNull)
    End Function
    Public Function SelectCount(ByVal filters As CCriteriaList, ByVal tableNameOrJoin As String, ByVal txOrNull As IDbTransaction) As Integer
        If String.IsNullOrEmpty(tableNameOrJoin) Then tableNameOrJoin = Me.ViewName
        Dim where As New CWhere(tableNameOrJoin, filters, txOrNull)
        Return DataSrc.SelectCount(where)
    End Function
#End Region

#Region "Select Max"
    Public Function SelectMax(ByVal colName As String) As Object
        Return SelectMax(colName, Nothing)
    End Function
    Public Function SelectMax(ByVal colName As String, ByVal join As String) As Object
        Return SelectMax(colName, join, Nothing)
    End Function
    Public Function SelectMax(ByVal colName As String, ByVal join As String, ByVal where As CCriteriaList) As Object
        Return SelectMax(colName, join, where, Nothing)
    End Function
    Public Function SelectMax(ByVal colName As String, ByVal join As String, ByVal where As CCriteriaList, ByVal txOrNull As IDbTransaction) As Object
        colName = String.Concat("MAX(", colName, ")")
        If join Is Nothing Then join = Me.ViewName
        Dim ds As DataSet = CType(DataSrc.SelectWhere(colName, join, where, String.Empty, txOrNull, EQueryReturnType.DataSet), DataSet)
        Dim obj As Object = ds.Tables(0).Rows(0)(0)
        If TypeOf obj Is DBNull Then Return Nothing
        Return obj
    End Function
#End Region

#Region "Select Min"
    Public Function SelectMin(ByVal colName As String) As Object
        Return SelectMin(colName, Nothing)
    End Function
    Public Function SelectMin(ByVal colName As String, ByVal join As String) As Object
        Return SelectMin(colName, join, Nothing)
    End Function
    Public Function SelectMin(ByVal colName As String, ByVal join As String, ByVal where As CCriteriaList) As Object
        Return SelectMin(colName, join, where, Nothing)
    End Function
    Public Function SelectMin(ByVal colName As String, ByVal join As String, ByVal where As CCriteriaList, ByVal txOrNull As IDbTransaction) As Object
        colName = String.Concat("MIN(", colName, ")")
        If join Is Nothing Then join = Me.ViewName
        Dim ds As DataSet = CType(DataSrc.SelectWhere(colName, join, where, String.Empty, txOrNull, EQueryReturnType.DataSet), DataSet)
        Dim obj As Object = ds.Tables(0).Rows(0)(0)
        If TypeOf obj Is DBNull Then Return Nothing
        Return obj
    End Function
#End Region

#Region "Select Max/Min PK"
    Public Function SelectMaxId() As Integer
        Return SelectMaxId(Nothing)
    End Function
    Public Function SelectMinId() As Integer
        Return SelectMinId(Nothing)
    End Function
    Public Function SelectMaxId(ByVal txOrNull As IDbTransaction) As Integer
        Dim sql As String = String.Concat("SELECT MAX(", PrimaryKeyName, ") FROM ", TableName)
        Dim max As Object = DataSrc.ExecuteScalar(sql, txOrNull)
        If TypeOf max Is DBNull Then Return 0
        Return CInt(max)
    End Function
    Public Function SelectMinId(ByVal txOrNull As IDbTransaction) As Integer
        Dim sql As String = String.Concat("SELECT MIN(", PrimaryKeyName, ") FROM ", TableName)
        Dim max As Object = DataSrc.ExecuteScalar(sql, txOrNull)
        If TypeOf max Is DBNull Then Return 0
        Return CInt(max)
    End Function
#End Region

#Region "PK Exists"
    Public Function Exists(ByVal primaryKeyValue As Object) As Boolean
        Return 0 <> DataSrc.SelectWhere_Dataset(TableName, New CCriteriaList(PrimaryKeyName, primaryKeyValue)).Tables(0).Rows.Count
    End Function
#End Region

#End Region

#Region "Load Logic"
    'Slightly more specific error msg
    Public Overrides Sub Reload(ByVal txOrNull As IDbTransaction)
        If DataSrc.IsRemote Then
            Load(SelectIdAsDs(Nothing).Tables(0).Rows(0))
        Else
            Dim dr As IDataReader = SelectIdAsDr(txOrNull)
            Try
                dr.Read()
                Load(dr)
                dr.Close()
            Catch ex As Exception
                dr.Close()
                Throw New Exception(String.Concat("Table ", Me.TableName, ": Failed to load record with ", Me.PrimaryKeyName, "=", Me.PrimaryKeyValue, vbCrLf, vbCrLf, ex))
            End Try
        End If
    End Sub
#End Region

#Region "ToXml"
    Public Overloads Overrides Sub ToXml(ByVal w As XmlWriter)
        w.WriteStartElement(Me.TableName.Replace("[", "").Replace("]", "").Replace(".", "_")) 'Uses tablename instead of class name
        ToXml_Autogenerated(w)
        ToXml_Custom(w)
        w.WriteEndElement()
    End Sub
#End Region

End Class

