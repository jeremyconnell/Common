Imports Framework
Imports System.Data
Imports System.Collections

'Note #1: Unlike DataSrc.aspx and WSDataSrc.asmx, this service does not provide any encryption or compression. i.e. it completely relys on WCF mechanisms for security
'Note #2. Unlike DataSrc.aspx and WSDataSrc.asmx, this service is loosely coupled (by schema not class) i.e. client is not restricted to .Net apps referencing Framework.dll
'Note #3. Unlike CWebSrcBinary and CWebSrcSoap, this class resides in the application layer, because the burden of WCF configuration does not lend itself towards reuse
Public Class WcfDataSrc : Implements IWcfDataSrc

#Region "Schema-Specific"
    Public Overloads Sub ClearCache(ByVal tableName As String)
        ClearCache() 'Can be more specific when schema project is in scope
    End Sub
    Public Overloads Sub ClearCache()
        Framework.CApplication.ClearAll()
    End Sub
#End Region

#Region "Low-level Methods"
    Public Function ExecuteDataSet(ByVal cmd As CCommand) As DataSet Implements IWcfDataSrc.ExecuteDataSet
        Return CDataSrc.Default.ExecuteDataSet(cmd)
    End Function
    Public Function ExecuteNonQuery(ByVal cmd As CCommand) As Integer Implements IWcfDataSrc.ExecuteNonQuery
        Return CDataSrc.Default.ExecuteNonQuery(cmd)
    End Function
    Public Function ExecuteScalar(ByVal cmd As CCommand) As Object Implements IWcfDataSrc.ExecuteScalar
        Return CDataSrc.Default.ExecuteScalar(cmd)
    End Function
#End Region

#Region "Driver-specific Dynamic sql"
    Public Function [Select](ByVal where As CSelectWhere) As DataSet Implements IWcfDataSrc.Select
        Return CDataSrc.Default.Select(where, EQueryReturnType.DataSet)
    End Function
    Public Function SelectCount(ByVal where As CWhere) As Integer Implements IWcfDataSrc.SelectCount
        Return CDataSrc.Default.SelectCount(where)
    End Function
    Public Function Delete(ByVal where As CWhere) As Integer Implements IWcfDataSrc.Delete
        Return CDataSrc.Default.Delete(where)
    End Function
    Public Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal oracleSequenceName As String) As Object Implements IWcfDataSrc.Insert
        Return CDataSrc.Default.Insert(tableName, pKeyName, insertPk, data, Nothing, oracleSequenceName)
    End Function
    Public Function Update(ByVal data As CNameValueList, ByVal where As CWhere) As Integer Implements IWcfDataSrc.Update
        Return CDataSrc.Default.Update(data, where)
    End Function
    Public Function UpdateOrdinals(ByVal tableName As String, ByVal pKeyName As String, ByVal ordinalName As String, ByVal data As CNameValueList) As Integer Implements IWcfDataSrc.UpdateOrdinals
        Return CDataSrc.Default.UpdateOrdinals(tableName, pKeyName, ordinalName, data)
    End Function
    Public Function Paging(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String) As DataSet Implements IWcfDataSrc.Paging
        Return CDataSrc.Default.Paging(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, Nothing, EQueryReturnType.DataSet)
    End Function
    Public Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList) As DataSet Implements IWcfDataSrc.PagingWithFilters
        Return CDataSrc.Default.PagingWithFilters(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, criteria, Nothing, EQueryReturnType.DataSet)
    End Function
    Public Function BulkSelect(ByVal tables As List(Of Framework.CCommand)) As List(Of DataSet) Implements IWcfDataSrc.BulkSelect
        Dim datasets As New List(Of DataSet)(tables.Count)
        For Each i As CCommand In tables
            datasets.Add(CDataSrc.Default.ExecuteDataSet(i))
        Next
        Return datasets
    End Function
#End Region

#Region "Implicit Transaction"
    Public Sub BulkSaveDelete(ByVal saves As ICollection, ByVal deletes As ICollection, ByVal il As IsolationLevel) Implements IWcfDataSrc.BulkSaveDelete
        CDataSrc.Default.BulkSaveDelete(saves, deletes, il)
    End Sub
#End Region

End Class
