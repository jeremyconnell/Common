Imports System.Web

Public Class CGenericHandler

    Public Delegate Sub DClearCache(tableName As String)

#Region "Interface"
    Public Shared Sub ProcessRequest(c As HttpContext, clearcache As DClearCache)
        Dim Request As HttpRequest = c.Request
        Dim Response As HttpResponse = c.Response
        Dim Cache As Caching.Cache = c.Cache

        'Get the querystring
        Dim int As Integer = Integer.MinValue 'CWeb.RequestInt(CWebSrcBinary.QUERYSTRING)
        Dim s As String = Request(CWebSrcBinary.QUERYSTRING)
        If Not Integer.TryParse(s, int) OrElse int = Integer.MinValue Then Exit Sub

        'Get the payload and decrypt/unzip it
        Dim encrypted As Byte() = Request.BinaryRead(Request.ContentLength)
        Dim data As Object = CBinary.Unpack(encrypted)

        'Cast enum, farm out to appropriate method
        Dim cmd As CWebSrcBinary.ECmd = CType(int, CWebSrcBinary.ECmd)
        Select Case cmd
            'Low-Level
            Case CWebSrcBinary.ECmd.ExecuteDataSet : ExecuteDataSet(data, Response)
            Case CWebSrcBinary.ECmd.ExecuteNonQuery : ExecuteNonQuery(data, Response)
            Case CWebSrcBinary.ECmd.ExecuteScalar : ExecuteScalar(data, Response)

                'Mid-Level
            Case CWebSrcBinary.ECmd.AllTableNames : Flush(DataSrc.AllTableNames(CType(data, Boolean)), Response)
            Case CWebSrcBinary.ECmd.SqlToListAllTables : Flush(DataSrc.SqlToListAllTables, Response)

                'High-Level
            Case CWebSrcBinary.ECmd.BulkSaveDelete : BulkSaveDelete(data)
            Case CWebSrcBinary.ECmd.Delete : Delete(data, Response, clearcache)
            Case CWebSrcBinary.ECmd.Insert : Insert(data, Response, clearcache)
            Case CWebSrcBinary.ECmd.Select : [Select](data, Response)
            Case CWebSrcBinary.ECmd.SelectCount : SelectCount(data, Response)
            Case CWebSrcBinary.ECmd.Update : Update(data, Response, clearcache)
            Case CWebSrcBinary.ECmd.UpdateOrdinals : UpdateOrdinals(data, Response, clearcache)
            Case CWebSrcBinary.ECmd.Paging : Paging(data, Response)
            Case CWebSrcBinary.ECmd.PagingWithFilters : PagingWithFilters(data, Response)
            Case CWebSrcBinary.ECmd.BulkSelect : BulkSelect(data, Response)


            Case CWebSrcBinary.ECmd.InsertId : InsertId(data, Response, clearcache)
            Case CWebSrcBinary.ECmd.InsertIdBulk : InsertIdBulk(data, Response, clearcache)


                'SqlServer
            Case CWebSrcBinary.ECmd.BulkInsert : BulkInsert(data)
            Case CWebSrcBinary.ECmd.BulkInsertWithTx : BulkInsertWithTx(data)
        End Select

        Response.ContentType = "application/octet-stream"
        Response.End()
    End Sub
#End Region


#Region "Driver Implementation"
    'Low-Level
    Private Shared Sub ExecuteDataSet(ByVal data As Object, r As HttpResponse)
        Dim cmd As CCommand = CType(data, CCommand)
        Dim ds As DataSet = DataSrc.ExecuteDataSet(cmd)
        Flush(ds, r)
    End Sub
    Private Shared Sub ExecuteNonQuery(ByVal data As Object, r As HttpResponse)
        Dim cmd As CCommand = CType(data, CCommand)
        Dim rowsAffected As Integer = DataSrc.ExecuteNonQuery(cmd)
        Flush(rowsAffected, r)
    End Sub
    Private Shared Sub ExecuteScalar(ByVal data As Object, r As HttpResponse)
        Dim cmd As CCommand = CType(data, CCommand)
        Dim obj As Object = DataSrc.ExecuteScalar(cmd)
        Flush(obj, r)
    End Sub

    'High Level
    Private Shared Sub BulkSaveDelete(ByVal data As Object)
        Dim bsd As CWebSrcBinary.CBulkSaveDelete = CType(data, CWebSrcBinary.CBulkSaveDelete)
        DataSrc.BulkSaveDelete(bsd.Saves, bsd.Deletes)
        'Implements a sub, so no return value
    End Sub
    Private Shared Sub Delete(ByVal data As Object, r As HttpResponse, clearcache As DClearCache)
        Dim where As CWhere = CType(data, CWhere)
        Dim rowsAffected As Integer = DataSrc.Delete(where)
        If IsNothing(clearcache) Then clearcache(where.TableName)
        Flush(rowsAffected, r)
    End Sub
    Private Shared Sub Insert(ByVal data As Object, r As HttpResponse, clearcache As DClearCache)
        Dim i As CWebSrcBinary.CInsert = CType(data, CWebSrcBinary.CInsert)
        Dim obj As Object = DataSrc.Insert(i.TableName, i.PrimaryKeyName, i.InsertPrimaryKey, i.Data, Nothing, i.OracleSequenceName)
        If IsNothing(clearcache) Then clearcache(i.TableName)
        Flush(obj, r)
    End Sub
    Private Shared Sub InsertId(ByVal data As Object, r As HttpResponse, clearcache As DClearCache)
        Dim i As CWebSrcBinary.CInsertId = CType(data, CWebSrcBinary.CInsertId)
        DataSrc.InsertId(i.TableName, i.Data, i.IsId)
        If IsNothing(clearcache) Then clearcache(i.TableName)
    End Sub
    Private Shared Sub InsertIdBulk(ByVal data As Object, r As HttpResponse, clearcache As DClearCache)
        Dim i As CWebSrcBinary.CInsertIdBulk = CType(data, CWebSrcBinary.CInsertIdBulk)
        DataSrc.InsertId(i.TableName, i.Bulk, i.IsId)
        If IsNothing(clearcache) Then clearcache(i.TableName)
    End Sub
    Private Shared Sub [Select](ByVal data As Object, r As HttpResponse)
        Dim where As CSelectWhere = CType(data, CSelectWhere)
        Dim ds As DataSet = CType(DataSrc.Select(where, EQueryReturnType.DataSet), DataSet)
        Flush(ds, r)
    End Sub
    Private Shared Sub SelectCount(ByVal data As Object, r As HttpResponse)
        Dim where As CWhere = CType(data, CWhere)
        Dim count As Integer = DataSrc.SelectCount(where)
        Flush(count, r)
    End Sub
    Private Shared Sub Update(ByVal data As Object, r As HttpResponse, clearcache As DClearCache)
        Dim i As CWebSrcBinary.CUpdate = CType(data, CWebSrcBinary.CUpdate)
        Dim obj As Integer = DataSrc.Update(i.Data, i.Where)
        If IsNothing(clearcache) Then clearcache(i.Where.TableName)
        Flush(obj, r)
    End Sub
    Private Shared Sub UpdateOrdinals(ByVal data As Object, r As HttpResponse, clearcache As DClearCache)
        Dim i As CWebSrcBinary.CUpdateOrdinals = CType(data, CWebSrcBinary.CUpdateOrdinals)
        Dim obj As Integer = DataSrc.UpdateOrdinals(i.TableName, i.PrimaryKeyName, i.OrdinalName, i.Data)
        If IsNothing(clearcache) Then clearcache(i.TableName)
        Flush(obj, r)
    End Sub
    Private Shared Sub Paging(ByVal data As Object, r As HttpResponse)
        Dim response As New CWebSrcBinary.CPagingResponse
        With CType(data, CWebSrcBinary.CPagingRequest)
            response.DataSet = CType(DataSrc.Paging(response.Count, .PageIndexZeroBased, .PageSize, .TableName, .Descending, .SortByColumn, .SelectColumns, Nothing, EQueryReturnType.DataSet), DataSet)
        End With
        Flush(response, r)
    End Sub
    Private Shared Sub PagingWithFilters(ByVal data As Object, r As HttpResponse)
        Dim response As New CWebSrcBinary.CPagingResponse
        With CType(data, CWebSrcBinary.CPagingWithFiltersRequest)
            response.DataSet = CType(DataSrc.PagingWithFilters(response.Count, .PageIndexZeroBased, .PageSize, .TableName, .Descending, .SortByColumn, .SelectColumns, .Where, Nothing, EQueryReturnType.DataSet), DataSet)
        End With
        Flush(response, r)
    End Sub
    Private Shared Sub BulkSelect(ByVal data As Object, r As HttpResponse)
        Dim tables As List(Of CCommand) = CType(data, List(Of CCommand))
        Dim datasets As New List(Of DataSet)(tables.Count)
        For Each i As CCommand In tables
            datasets.Add(DataSrc.ExecuteDataSet(i))
        Next
        Flush(datasets, r)
    End Sub
    Private Shared Sub BulkInsert(ByVal data As Object)
        With CType(data, CWebSrcBinary.CBulkInsert)
            DataSrc.BulkInsert(.DataTable, .TableName, .Mappings) 'SqlClient (or another webservice)
        End With
    End Sub
    Private Shared Sub BulkInsertWithTx(ByVal data As Object)
        With CType(data, CWebSrcBinary.CBulkInsert)
            DataSrc.BulkInsertWithTx(.DataTable, .TableName, .Mappings) 'SqlClient (or another webservice)
        End With
    End Sub
#End Region

#Region "Private"
    Private Shared Function DataSrc() As CDataSrc
        Return CDataSrc.Default
    End Function
    Private Shared Sub Flush(ByVal obj As Object, response As HttpResponse)
        If IsNothing(obj) Then Exit Sub
        Dim bin As Byte() = CBinary.Pack(obj)
        response.BinaryWrite(bin)
    End Sub
#End Region

#Region "Cache Control (Abstract/Virtual)"
    Public Sub ClearCache(context As HttpContext)
        With context
            For Each i As DictionaryEntry In .Cache
                .Cache.Remove(CStr(i.Key))
            Next
        End With
    End Sub
#End Region

End Class
