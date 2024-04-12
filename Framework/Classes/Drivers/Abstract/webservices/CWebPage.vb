'Implements a webpage that will service a CWebSrcBinary driver
Public MustInherit Class CWebPage : Inherits Web.UI.Page

#Region "Event Handler"
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Get the querystring
        Dim int As Integer = CWeb.RequestInt(CWebSrcBinary.QUERYSTRING)
        If Integer.MinValue = int Then Exit Sub

        'Get the payload and decrypt/unzip it
        Dim encrypted As Byte() = Request.BinaryRead(Request.ContentLength)
        Dim data As Object = CBinary.Unpack(encrypted)
        encrypted = Nothing

        'Cast enum, farm out to appropriate method
        Dim cmd As CWebSrcBinary.ECmd = CType(int, CWebSrcBinary.ECmd)
        Select Case cmd
            'Low-Level
            Case CWebSrcBinary.ECmd.ExecuteDataSet : ExecuteDataSet(data)
            Case CWebSrcBinary.ECmd.ExecuteNonQuery : ExecuteNonQuery(data)
            Case CWebSrcBinary.ECmd.ExecuteScalar : ExecuteScalar(data)

                'Mid-Level
            Case CWebSrcBinary.ECmd.AllTableNames : Flush(DataSrc.AllTableNames(CType(data, Boolean)))
            Case CWebSrcBinary.ECmd.SqlToListAllTables : Flush(DataSrc.SqlToListAllTables)

                'High-Level
            Case CWebSrcBinary.ECmd.BulkSaveDelete : BulkSaveDelete(data)
            Case CWebSrcBinary.ECmd.Delete : Delete(data)
            Case CWebSrcBinary.ECmd.Insert : Insert(data)
            Case CWebSrcBinary.ECmd.Select : [Select](data)
            Case CWebSrcBinary.ECmd.SelectCount : SelectCount(data)
            Case CWebSrcBinary.ECmd.Update : Update(data)
            Case CWebSrcBinary.ECmd.UpdateOrdinals : UpdateOrdinals(data)
            Case CWebSrcBinary.ECmd.Paging : Paging(data)
            Case CWebSrcBinary.ECmd.PagingWithFilters : PagingWithFilters(data)
            Case CWebSrcBinary.ECmd.BulkSelect : BulkSelect(data)

            Case CWebSrcBinary.ECmd.InsertId : InsertId(data)
            Case CWebSrcBinary.ECmd.InsertIdBulk : InsertIdBulk(data)

                'SqlServer
            Case CWebSrcBinary.ECmd.BulkInsert : BulkInsert(data)
            Case CWebSrcBinary.ECmd.BulkInsertWithTx : BulkInsertWithTx(data)
        End Select

        Response.End()
    End Sub
#End Region

#Region "Driver Implementation"
    'Low-Level
    Private Sub ExecuteDataSet(ByVal data As Object)
        Dim cmd As CCommand = CType(data, CCommand)
        Dim ds As DataSet = DataSrc.ExecuteDataSet(cmd)
        Flush(ds)
    End Sub
    Private Sub ExecuteNonQuery(ByVal data As Object)
        Dim cmd As CCommand = CType(data, CCommand)
        Dim rowsAffected As Integer = DataSrc.ExecuteNonQuery(cmd)
        Flush(rowsAffected)
    End Sub
    Private Sub ExecuteScalar(ByVal data As Object)
        Dim cmd As CCommand = CType(data, CCommand)
        Dim obj As Object = DataSrc.ExecuteScalar(cmd)
        Flush(obj)
    End Sub

    'High Level
    Private Sub BulkSaveDelete(ByVal data As Object)
        Dim bsd As CWebSrcBinary.CBulkSaveDelete = CType(data, CWebSrcBinary.CBulkSaveDelete)
        DataSrc.BulkSaveDelete(bsd.Saves, bsd.Deletes)
        'Implements a sub, so no return value
    End Sub
    Private Sub Delete(ByVal data As Object)
        Dim where As CWhere = CType(data, CWhere)
        Dim rowsAffected As Integer = DataSrc.Delete(where)
        ClearCache(where.TableName)
        Flush(rowsAffected)
    End Sub
    Private Sub Insert(ByVal data As Object)
        Dim i As CWebSrcBinary.CInsert = CType(data, CWebSrcBinary.CInsert)
        Dim obj As Object = DataSrc.Insert(i.TableName, i.PrimaryKeyName, i.InsertPrimaryKey, i.Data, Nothing, i.OracleSequenceName)
        ClearCache(i.TableName)
        Flush(obj)
    End Sub
    Private Sub InsertId(ByVal data As Object)
        Dim i As CWebSrcBinary.CInsertId = CType(data, CWebSrcBinary.CInsertId)
        DataSrc.InsertId(i.TableName, i.Data, i.IsId)
        ClearCache(i.TableName)
    End Sub
    Private Sub InsertIdBulk(ByVal data As Object)
        Dim i As CWebSrcBinary.CInsertIdBulk = CType(data, CWebSrcBinary.CInsertIdBulk)
        DataSrc.InsertId(i.TableName, i.Bulk, i.IsId)
        ClearCache(i.TableName)
    End Sub
    Private Sub [Select](ByVal data As Object)
        Dim where As CSelectWhere = CType(data, CSelectWhere)
        Dim ds As DataSet = CType(DataSrc.Select(where, EQueryReturnType.DataSet), DataSet)
        Flush(ds)
    End Sub
    Private Sub SelectCount(ByVal data As Object)
        Dim where As CWhere = CType(data, CWhere)
        Dim count As Integer = DataSrc.SelectCount(where)
        Flush(count)
    End Sub
    Private Sub Update(ByVal data As Object)
        Dim i As CWebSrcBinary.CUpdate = CType(data, CWebSrcBinary.CUpdate)
        Dim obj As Integer = DataSrc.Update(i.Data, i.Where)
        ClearCache(i.Where.TableName)
        Flush(obj)
    End Sub
    Private Sub UpdateOrdinals(ByVal data As Object)
        Dim i As CWebSrcBinary.CUpdateOrdinals = CType(data, CWebSrcBinary.CUpdateOrdinals)
        Dim obj As Integer = DataSrc.UpdateOrdinals(i.TableName, i.PrimaryKeyName, i.OrdinalName, i.Data)
        ClearCache(i.TableName)
        Flush(obj)
    End Sub
    Private Sub Paging(ByVal data As Object)
        Dim response As New CWebSrcBinary.CPagingResponse
        With CType(data, CWebSrcBinary.CPagingRequest)
            response.DataSet = CType(DataSrc.Paging(response.Count, .PageIndexZeroBased, .PageSize, .TableName, .Descending, .SortByColumn, .SelectColumns, Nothing, EQueryReturnType.DataSet), DataSet)
        End With
        Flush(response)
    End Sub
    Private Sub PagingWithFilters(ByVal data As Object)
        Dim response As New CWebSrcBinary.CPagingResponse
        With CType(data, CWebSrcBinary.CPagingWithFiltersRequest)
            response.DataSet = CType(DataSrc.PagingWithFilters(response.Count, .PageIndexZeroBased, .PageSize, .TableName, .Descending, .SortByColumn, .SelectColumns, .Where, Nothing, EQueryReturnType.DataSet), DataSet)
        End With
        Flush(response)
    End Sub
    Private Sub BulkSelect(ByVal data As Object)
        Dim tables As List(Of CCommand) = CType(data, List(Of CCommand))
        Dim datasets As New List(Of DataSet)(tables.Count)
        For Each i As CCommand In tables
            datasets.Add(DataSrc.ExecuteDataSet(i))
        Next
        Flush(datasets)
    End Sub
    Private Sub BulkInsert(ByVal data As Object)
        With CType(data, CWebSrcBinary.CBulkInsert)
            DataSrc.BulkInsert(.DataTable, .TableName, .Mappings) 'SqlClient (or another webservice)
        End With
    End Sub
    Private Sub BulkInsertWithTx(ByVal data As Object)
        With CType(data, CWebSrcBinary.CBulkInsert)
            DataSrc.BulkInsertWithTx(.DataTable, .TableName, .Mappings) 'SqlClient (or another webservice)
        End With
    End Sub
#End Region

#Region "Private"
    Private Function DataSrc() As CDataSrc
        Return CDataSrc.Default
    End Function
    Private Sub Flush(ByVal obj As Object)
        If IsNothing(obj) Then Exit Sub
        Dim bin As Byte() = CBinary.Pack(obj)
        Response.BinaryWrite(bin)
    End Sub
#End Region

#Region "Cache Control (Abstract/Virtual)"
    Public MustOverride Sub ClearCache(ByVal tableName As String)
    Public Overridable Sub ClearCache()
        With System.Web.HttpContext.Current
            For Each i As DictionaryEntry In .Cache
                .Cache.Remove(CStr(i.Key))
            Next
        End With
    End Sub
#End Region

End Class
