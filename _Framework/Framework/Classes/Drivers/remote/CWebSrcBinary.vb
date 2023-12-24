Imports System.Data.OleDb
Imports System.Net
Imports System.Text
Imports Framework

<Serializable(), CLSCompliant(True)>
Public Class CWebSrcBinary : Inherits CWebSrc

#Region "Constants"
    Public Const QUERYSTRING As String = "c"
    Public Const DEFAULT_BASE_URL As String = "webservices/DataSrc.ashx"
#End Region

#Region "Enum - ECmd"
    Public Enum ECmd
        'Low-Level
        ExecuteDataSet = 0
        ExecuteScalar = 1
        ExecuteNonQuery = 2

        'High-Level
        BulkSaveDelete = 3
        Delete = 4
        Insert = 5
        Update = 6
        [Select] = 7
        SelectCount = 8
        UpdateOrdinals = 9
        Paging = 10
        PagingWithFilters = 11
        BulkSelect = 12

        'Remote Driver Access
        AllTableNames = 13
        SqlToListAllTables = 14

        'SqlServer only (or via webservice)
        BulkInsert = 15
        BulkInsertWithTx = 16
        BulkInsertWithTx_ = 17


        InsertId = 20
        InsertIdBulk = 21
    End Enum
#End Region

#Region "Transport Classes - (Method parameters if >1)"
    <Serializable()> Friend Class CBulkSaveDelete
        Public Saves As ICollection
        Public Deletes As ICollection
    End Class
    <Serializable()> Friend Class CInsert
        Public TableName As String
        Public PrimaryKeyName As String
        Public InsertPrimaryKey As Boolean
        Public Data As CNameValueList
        Public OracleSequenceName As String
    End Class
    <Serializable()> Friend Class CInsertId
        Public TableName As String
        Public Data As CNameValueList
        Public IsId As Boolean
    End Class
    <Serializable()> Friend Class CInsertIdBulk
        Public TableName As String
        Public Bulk As List(Of CNameValueList)
        Public IsId As Boolean
    End Class
    <Serializable()> Friend Class CUpdate
        Public Data As CNameValueList
        Public Where As CWhere
    End Class
    <Serializable()> Friend Class CUpdateOrdinals
        Public TableName As String
        Public PrimaryKeyName As String
        Public OrdinalName As String
        Public Data As CNameValueList
    End Class
    <Serializable()> Friend Class CPagingRequest
        Public PageIndexZeroBased As Integer
        Public PageSize As Integer
        Public TableName As String
        Public SortByColumn As String
        Public Descending As Boolean
        Public SelectColumns As String
    End Class
    <Serializable()> Friend Class CPagingWithFiltersRequest : Inherits CPagingRequest
        Public Where As CCriteriaList
    End Class
    <Serializable()> Friend Class CPagingResponse
        Public DataSet As DataSet
        Public Count As Integer
    End Class
    <Serializable()> Friend Class CBulkInsert
        Public DataTable As DataTable
        Public TableName As String
        Public Mappings As Dictionary(Of Integer, Integer)
    End Class
    <Serializable()> Friend Class CBulkInsertWithTx_
        Public RowsToInsert As List(Of CNameValueList)
        Public TableName As String
        Public PrimaryKey As String
        Public IsIdentity As Boolean
    End Class
#End Region

#Region "Constructors"
    Public Sub New(ByVal url As String)
        Me.New(url, CConfigBase.WebServicePassword)
    End Sub
    Public Sub New(ByVal url As String, ByVal password As String)
        'Connection String, Encryption Password
        MyBase.New(url, password)
    End Sub
    Protected Overrides Function DefaultPageName(ByVal url As String) As String
        If Not url.ToLower().Contains(".aspx") AndAlso Not url.ToLower().Contains(".ashx") And Len(url) > 0 Then
            If "/" <> url.Substring(url.Length - 1, 1) Then url &= "/"
            url &= DEFAULT_BASE_URL
        End If
        Return url
    End Function
#End Region



#Region "Public - Driver Methods"
    Public Overrides Function ExecuteDataSet(ByVal cmd As CCommand) As System.Data.DataSet
        If LOGGING Then Log(cmd)
        Return CType(SyncRequest(ECmd.ExecuteDataSet, cmd), DataSet)
    End Function
    Public Overrides Function ExecuteScalar(ByVal cmd As CCommand) As Object
        If LOGGING Then Log(cmd)
        Return SyncRequest(ECmd.ExecuteScalar, cmd)
    End Function
    Public Overrides Function ExecuteNonQuery(ByVal cmd As CCommand) As Integer
        If LOGGING Then Log(cmd)
        Return Convert.ToInt32(SyncRequest(ECmd.ExecuteNonQuery, cmd))
    End Function

    'Remote Driver methods
    Public Overrides Function AllTableNames(Optional withSchema As Boolean = False) As List(Of String)
        If LOGGING Then Log("AllTableNames")
        Return CType(SyncRequest(ECmd.AllTableNames, withSchema), List(Of String))
    End Function
    Private m_sqlToListTables As String
    Public Overrides ReadOnly Property SqlToListAllTables() As String 'Todo: make protected, only publicly implement alltablenames
        Get
            If IsNothing(m_sqlToListTables) Then
                If LOGGING Then Log("SqlToListAllTables")
                m_sqlToListTables = CType(SyncRequest(ECmd.SqlToListAllTables, Nothing), String)
            End If
            Return m_sqlToListTables
        End Get
    End Property

    Public Overrides Sub BulkInsert(ByVal dt As DataTable, ByVal tableName As String, ByVal mappings As Dictionary(Of Integer, Integer))
        If 0 = dt.Rows.Count Then Exit Sub

        If LOGGING Then Log(String.Concat("SqlServer_BulkInsert: ", tableName, " ", CUtilities.CountSummary(dt.Rows.Count, "row")))

        Dim p As New CBulkInsert
        p.DataTable = dt
        p.TableName = tableName
        p.Mappings = mappings

        If NO_ASYNC Then
            SyncRequest(ECmd.BulkInsert, p)
        Else
            AsyncRequest(ECmd.BulkInsert, p)
        End If
    End Sub
    Public Overrides Sub BulkInsertWithTx(ByVal dt As DataTable, ByVal tableName As String, ByVal mappings As Dictionary(Of Integer, Integer))
        If 0 = dt.Rows.Count Then Exit Sub

        If LOGGING Then Log(String.Concat("SqlServer_BulkInsertWithTx: ", tableName, " ", CUtilities.CountSummary(dt.Rows.Count, "row")))

        Dim p As New CBulkInsert
        p.DataTable = dt
        p.TableName = tableName
        p.Mappings = mappings

        If NO_ASYNC Then
            SyncRequest(ECmd.BulkInsertWithTx, p)
        Else
            AsyncRequest(ECmd.BulkInsertWithTx, p)
        End If
    End Sub
    Protected Overrides Function BulkInsertWithTx_(rowsToInsert As List(Of CNameValueList), tableName As String, primaryKey As String, isIdentity As Boolean) As List(Of Object)
        Dim bsd As New CBulkInsertWithTx_()
        bsd.RowsToInsert = rowsToInsert
        bsd.TableName = tableName
        bsd.PrimaryKey = primaryKey
        bsd.IsIdentity = isIdentity

        Return DirectCast(SyncRequest(ECmd.BulkInsertWithTx_, bsd), List(Of Object))
    End Function
#End Region

    Public NO_ASYNC As Boolean = False

#Region "Public - Sql Methods"
    Public Overrides Sub BulkSaveDelete(ByVal saves As ICollection, ByVal deletes As ICollection, ByVal txIsolation As IsolationLevel)
        If LOGGING Then Log(String.Concat("BulkSaveDelete(", saves.Count, ",", deletes.Count))
        Dim bsd As New CBulkSaveDelete()
        bsd.Saves = saves
        bsd.Deletes = deletes

        If NO_ASYNC Then
            SyncRequest(ECmd.BulkSaveDelete, bsd)
        Else
            AsyncRequest(ECmd.BulkSaveDelete, bsd)
        End If
    End Sub
    Public Overrides Function Delete(ByVal where As CWhere) As Integer
        CheckTxIsNull(where.TxOrNull)

        If LOGGING Then
            Dim nv As New CNameValueList
            If Not IsNothing(where.Criteria) Then
                nv.Add(where.Criteria.ColumnName, where.Criteria.ColumnValue)
            ElseIf Not IsNothing(where.CriteriaList) Then
                For Each i As CCriteria In where.CriteriaList
                    nv.Add(i.ColumnName, i.ColumnValue)
                Next
            End If
            Log(New CCommand(String.Concat("DeleteWhere(", where.TableName, ",", where.UnsafeWhereClause), nv))
        End If

        If NO_ASYNC Then
            Return CInt(SyncRequest(ECmd.Delete, where))
        Else
            AsyncRequest(ECmd.Delete, where)
            Return 1
        End If
    End Function
    Public Overrides Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal txOrNull As IDbTransaction, ByVal oracleSequenceName As String) As Object
        CheckTxIsNull(txOrNull)
        If LOGGING Then Log(New CCommand(String.Concat("INSERT INTO ", tableName, ", ", pKeyName, ", ", insertPk), data))

        Dim i As New CInsert
        i.TableName = tableName
        i.PrimaryKeyName = pKeyName
        i.InsertPrimaryKey = insertPk
        i.Data = data
        i.OracleSequenceName = oracleSequenceName

        Return SyncRequest(ECmd.Insert, i)
    End Function
    Public Overrides Sub InsertId(ByVal tableName As String, ByVal data As CNameValueList, isId As Boolean)
        If LOGGING Then Log(New CCommand(String.Concat("INSERT INTO ", tableName), data))

        Dim i As New CInsertId
        i.TableName = tableName
        i.Data = data
        i.IsId = isId

        SyncRequest(ECmd.InsertId, i)
    End Sub
    Public Overrides Sub InsertId(ByVal tableName As String, ByVal bulk As List(Of CNameValueList), isId As Boolean)
        If LOGGING Then Log(New CCommand(String.Concat("INSERT INTO ", tableName), bulk.Count))

        Dim i As New CInsertIdBulk
        i.TableName = tableName
        i.Bulk = bulk
        i.IsId = isId

        SyncRequest(ECmd.InsertIdBulk, i)
    End Sub
    Public Overrides Function [Select](ByVal where As CSelectWhere, ByVal type As EQueryReturnType) As Object
        CheckTxIsNull(where.TxOrNull)

        If LOGGING Then
            Dim nv As New CNameValueList
            If Not IsNothing(where.Criteria) Then
                nv.Add(where.Criteria.ColumnName, where.Criteria.ColumnValue)
            ElseIf Not IsNothing(where.CriteriaList) Then
                For Each i As CCriteria In where.CriteriaList
                    nv.Add(i.ColumnName, i.ColumnValue)
                Next
            End If
            Log(New CCommand(String.Concat("Select(", where.TableName, ",", where.UnsafeWhereClause), nv))
        End If


        'Ignores type param, always go with dataset, never datareader
        Return SyncRequest(ECmd.Select, where)
    End Function
    Public Overrides Function SelectCount(ByVal where As CWhere) As Integer
        CheckTxIsNull(where.TxOrNull)

        If LOGGING Then
            Dim nv As New CNameValueList
            If Not IsNothing(where.Criteria) Then
                nv.Add(where.Criteria.ColumnName, where.Criteria.ColumnValue)
            ElseIf Not IsNothing(where.CriteriaList) Then
                For Each i As CCriteria In where.CriteriaList
                    nv.Add(i.ColumnName, i.ColumnValue)
                Next
            End If
            Log(New CCommand(String.Concat("SelectCount(", where.TableName, ",", where.UnsafeWhereClause), nv))
        End If

        Return Convert.ToInt32(SyncRequest(ECmd.SelectCount, where))
    End Function
    Public Overrides Function Update(ByVal data As CNameValueList, ByVal where As CWhere) As Integer
        CheckTxIsNull(where.TxOrNull)

        If LOGGING Then
            Dim nv As New CNameValueList
            If Not IsNothing(where.Criteria) Then
                nv.Add(where.Criteria.ColumnName, where.Criteria.ColumnValue)
            ElseIf Not IsNothing(where.CriteriaList) Then
                For Each i As CCriteria In where.CriteriaList
                    nv.Add(i.ColumnName, i.ColumnValue)
                Next
            End If
            nv.AddRange(data)
            Log(New CCommand(String.Concat("Update(", where.TableName, ",", where.UnsafeWhereClause), nv))
        End If

        Dim u As New CUpdate
        u.Data = data
        u.Where = where

        Return Convert.ToInt32(SyncRequest(ECmd.Update, u))
    End Function
    Public Overrides Function UpdateOrdinals(ByVal tableName As String, ByVal pKeyName As String, ByVal ordinalName As String, ByVal data As CNameValueList) As Integer
        Dim uo As New CUpdateOrdinals
        uo.TableName = tableName
        uo.PrimaryKeyName = pKeyName
        uo.OrdinalName = ordinalName
        uo.Data = data

        If NO_ASYNC Then
            Return CInt(SyncRequest(ECmd.UpdateOrdinals, uo))
        Else
            AsyncRequest(ECmd.UpdateOrdinals, uo)
            Return data.Count
        End If
    End Function
    Public Overrides Function Paging(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal txOrNull As System.Data.IDbTransaction, ByVal type As EQueryReturnType) As Object
        CheckTxIsNull(txOrNull)

        If LOGGING Then
            Log(New CCommand(String.Concat("Paging(", count, ",", pageIndexZeroBased, ",", pageSize, ",", tableName, ",", descending, ",", sortByColumn, ",", selectColumns)))
        End If

        Dim request As New CPagingWithFiltersRequest
        request.PageIndexZeroBased = pageIndexZeroBased
        request.PageSize = pageSize
        request.Descending = descending
        request.SortByColumn = sortByColumn
        request.TableName = tableName
        request.SelectColumns = selectColumns

        Dim response As CPagingResponse = CType(SyncRequest(ECmd.Paging, request), CPagingResponse)
        count = response.Count
        Return response.DataSet
    End Function
    Public Overrides Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        CheckTxIsNull(txOrNull)

        If LOGGING Then
            Dim nv As New CNameValueList
            For Each i As CCriteria In criteria
                nv.Add(i.ColumnName, i.ColumnValue)
            Next
            Log(New CCommand(String.Concat("Paging(", count, ",", pageIndexZeroBased, ",", pageSize, ",", tableName, ",", descending, ",", sortByColumn, ",", selectColumns), nv))
        End If


        Dim request As New CPagingWithFiltersRequest
        request.PageIndexZeroBased = pageIndexZeroBased
        request.PageSize = pageSize
        request.Descending = descending
        request.SortByColumn = sortByColumn
        request.TableName = tableName
        request.SelectColumns = selectColumns
        request.Where = criteria

        Dim response As CPagingResponse = CType(SyncRequest(ECmd.PagingWithFilters, request), CPagingResponse)
        count = response.Count
        Return response.DataSet
    End Function
    Protected Overloads Overrides Function BulkSelect(ByVal tables As List(Of CCommand)) As List(Of DataSet)
        Return CType(SyncRequest(ECmd.BulkSelect, tables), List(Of DataSet))
    End Function
#End Region

#Region "Private - Request Sync/Async"
    Private Sub AsyncRequest(ByVal cmd As ECmd, ByVal data As Object)
        Try
            If IsNothing(data) Then
                GetWebClient.DownloadDataAsync(Uri(cmd))
            Else
                GetWebClient.UploadDataAsync(Uri(cmd), "post", Pack(data))
            End If
        Catch
            SyncRequest(cmd, data)
        End Try
    End Sub
    Private Function SyncRequest(ByVal cmd As ECmd, ByVal data As Object) As Object
        Dim response As Byte() = Nothing
        Try
            If IsNothing(data) Then
                response = GetWebClient.DownloadData(Uri(cmd))
            Else
                response = GetWebClient.UploadData(Uri(cmd), "post", Pack(data))
            End If
        Catch ex As WebException
            If Not IsNothing(ex.Response) Then
                Throw New Exception(CBinary.BytesToString(CBinary.StreamToBytes(ex.Response.GetResponseStream())), ex)
            Else
                Throw ex
            End If
        End Try

        Try
            Return Unpack(response)
        Catch ex As Exception
            Throw New Exception("Failed to unpack data - check password")
        End Try
    End Function
    Private Function Uri(ByVal cmd As ECmd) As Uri
        Return New Uri(String.Concat(Url, "?", QUERYSTRING, "=", CInt(cmd)))
    End Function
#End Region

    Public Shared TimeOutMs As Integer = 1000 * CInt(IIf(Integer.MinValue = CConfigBase.CommandTimeoutSecs, 100, CConfigBase.CommandTimeoutSecs))
    Private Function GetWebClient() As WebClient
        'Web client
        Dim wc As New CWebClient(TimeOutMs) '100secs is the normal default
        If Not IsNothing(Me.Proxy) Then wc.Proxy = Me.Proxy


        'Internally-async methods:
        'AddHandler wc.DownloadDataCompleted, AddressOf Completed
        AddHandler wc.UploadDataCompleted, AddressOf Completed
        Return wc
    End Function

End Class

