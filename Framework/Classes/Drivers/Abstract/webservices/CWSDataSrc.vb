Imports System.Web.Services

'Implements a webservice that will service a CWebSrcSoap driver
Public MustInherit Class CWSDataSrc : Inherits WebService

#Region "Driver Interface"
    <WebMethod()> _
    Public Function ExecuteDataset(ByVal data As Byte()) As Byte()
        Dim cmd As CCommand = UnpackCmd(data)
        Dim ds As DataSet = DataSrc.ExecuteDataSet(cmd)
        Return Pack(ds)
    End Function
    <WebMethod()> _
    Public Function ExecuteScalar(ByVal data As Byte()) As Byte()
        Dim cmd As CCommand = UnpackCmd(data)
        Dim obj As Object = DataSrc.ExecuteScalar(cmd)
        Return Pack(obj)
    End Function
    <WebMethod()> _
    Public Function ExecuteNonQuery(ByVal data As Byte()) As Byte()
        Dim cmd As CCommand = UnpackCmd(data)
        Dim rowsAffected As Integer = DataSrc.ExecuteNonQuery(cmd)
        Return Pack(rowsAffected)
    End Function

    'Mid-Level
    <WebMethod()> _
    Public Function SqlToListAllTables() As Byte()
        Return Pack(DataSrc.SqlToListAllTables)
    End Function
    <WebMethod()> _
    Public Function AllTableNames() As Byte()
        Return Pack(DataSrc.AllTableNames)
    End Function
#End Region

#Region "Sql Generation"
    <WebMethod()> _
    Public Sub BulkSaveDelete(ByVal s As Byte(), ByVal d As Byte())
        Dim saves As ICollection = UnpackList(s)
        Dim deletes As ICollection = UnpackList(d)
        DataSrc.BulkSaveDelete(saves, deletes)
    End Sub
    <WebMethod()> _
    Public Function Delete(ByVal w As Byte()) As Integer
        Dim where As CWhere = UnpackWhere(w)
        ClearCache(where.TableName)
        Return DataSrc.Delete(where)
    End Function
    <WebMethod()> _
    Public Function Insert(ByVal t As Byte(), ByVal p As Byte(), ByVal insertPk As Boolean, ByVal d As Byte(), ByVal s As Byte()) As Byte()
        Dim tableName As String = UnpackString(t)
        Dim pKeyName As String = UnpackString(p)
        Dim data As CNameValueList = UnpackNV(d)
        Dim oracleSequenceName As String = UnpackString(s)
        Dim obj As Object = DataSrc.Insert(tableName, pKeyName, insertPk, data, Nothing, oracleSequenceName)
        ClearCache(tableName)
        Return Pack(obj)
    End Function
    <WebMethod()> _
    Public Function [Select](ByVal w As Byte()) As Byte()
        Dim where As CSelectWhere = UnpackSelectWhere(w)
        Dim ds As Object = DataSrc.Select(where, EQueryReturnType.DataSet)
        Return Pack(ds)
    End Function
    <WebMethod()> _
    Public Function SelectCount(ByVal w As Byte()) As Integer
        Dim where As CWhere = UnpackWhere(w)
        Return DataSrc.SelectCount(where)
    End Function
    <WebMethod()> _
    Public Function Update(ByVal nv As Byte(), ByVal w As Byte()) As Integer
        Dim data As CNameValueList = UnpackNV(nv)
        Dim where As CWhere = UnpackWhere(w)
        ClearCache(where.TableName)
        Return DataSrc.Update(data, where)
    End Function
    <WebMethod()> _
    Public Function UpdateOrdinals(ByVal n As Byte(), ByVal d As Byte()) As Integer
        Dim names As List(Of String) = CType(Unpack(n), List(Of String))
        Dim tableName As String = names(0)
        Dim primaryKey As String = names(1)
        Dim ordinalName As String = names(2)

        Dim data As CNameValueList = UnpackNV(d)

        ClearCache(tableName)
        Return DataSrc.UpdateOrdinals(tableName, primaryKey, ordinalName, data)
    End Function
    <WebMethod()> _
    Public Function Paging(ByVal data As Byte()) As Byte()
        Dim response As New CWebSrcBinary.CPagingResponse
        With CType(Unpack(data), CWebSrcBinary.CPagingRequest)
            response.DataSet = CType(DataSrc.Paging(response.Count, .PageIndexZeroBased, .PageSize, .TableName, .Descending, .SortByColumn, .SelectColumns, Nothing, EQueryReturnType.DataSet), DataSet)
        End With
        Return Pack(response)
    End Function
    <WebMethod()> _
    Public Function PagingWithFilters(ByVal data As Byte()) As Byte()
        Dim response As New CWebSrcBinary.CPagingResponse
        With CType(Unpack(data), CWebSrcBinary.CPagingWithFiltersRequest)
            response.DataSet = CType(DataSrc.PagingWithFilters(response.Count, .PageIndexZeroBased, .PageSize, .TableName, .Descending, .SortByColumn, .SelectColumns, .Where, Nothing, EQueryReturnType.DataSet), DataSet)
        End With
        Return Pack(response)
    End Function
    <WebMethod()> _
    Public Function BulkSelect(ByVal data As Byte()) As Byte()
        Dim tables As List(Of CCommand) = CType(Unpack(data), List(Of CCommand))
        Dim datasets As New List(Of DataSet)(tables.Count)
        For Each i As CCommand In tables
            datasets.Add(DataSrc.ExecuteDataSet(i))
        Next
        Return Pack(datasets)
    End Function
#End Region

#Region "Utilities"
    Protected Overridable ReadOnly Property DataSrc() As CDataSrc
        Get
            Return CDataSrc.Default
        End Get
    End Property

    Private Function Pack(ByVal obj As Object) As Byte()
        Return CBinary.Pack(obj)
    End Function
    Private Function Unpack(ByVal data As Byte()) As Object
        Return CBinary.Unpack(data)
    End Function
    Private Function UnpackCmd(ByVal data As Byte()) As CCommand
        Return CType(Unpack(data), CCommand)
    End Function
    Private Function UnpackList(ByVal data As Byte()) As ICollection
        Return CType(Unpack(data), ICollection)
    End Function
    Private Function UnpackWhere(ByVal data As Byte()) As CWhere
        Return CType(Unpack(data), CWhere)
    End Function
    Private Function UnpackSelectWhere(ByVal data As Byte()) As CSelectWhere
        Return CType(Unpack(data), CSelectWhere)
    End Function
    Private Function UnpackString(ByVal data As Byte()) As String
        Return CType(Unpack(data), String)
    End Function
    Private Function UnpackNV(ByVal data As Byte()) As CNameValueList
        Return CType(Unpack(data), CNameValueList)
    End Function
#End Region

#Region "Cache Control"
    Public MustOverride Sub ClearCache(ByVal tableName As String)
    Public Overridable Sub ClearCache()
        With System.Web.HttpContext.Current
            For Each i As DictionaryEntry In .Cache
                .Cache.Remove(CStr(i.Key))
            Next
        End With
        Exit Sub
    End Sub
#End Region

End Class
