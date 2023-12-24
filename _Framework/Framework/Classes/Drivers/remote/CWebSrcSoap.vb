Imports System.Net
Imports System.Text
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports Framework

<Serializable(), CLSCompliant(True)>
Public Class CWebSrcSoap : Inherits CWebSrc

#Region "Constructors"
    Public Sub New(ByVal url As String)
        Me.New(url, CConfigBase.WebServicePassword)
    End Sub
    Public Sub New(ByVal url As String, ByVal password As String)
        MyBase.New(url, password)

        'Web service
        m_webServiceRef = New WRDataSrc.WSDataSrc()
        m_webServiceRef.Url = Me.Url
        If Not IsNothing(Me.Proxy) Then m_webServiceRef.Proxy = Me.Proxy

        'Internally-async methods:
        AddHandler m_webServiceRef.BulkSaveDeleteCompleted, AddressOf Completed
        AddHandler m_webServiceRef.DeleteCompleted, AddressOf Completed
        AddHandler m_webServiceRef.UpdateOrdinalsCompleted, AddressOf Completed
    End Sub
    Protected Overrides Function DefaultPageName(ByVal url As String) As String
        If Not url.ToLower().Contains(".asmx") And Len(url) > 0 Then
            If "/" <> url.Substring(url.Length - 1, 1) Then url &= "/"
            url &= "webservices/soap/WSDataSrc.asmx"
        End If
        Return url
    End Function
#End Region

#Region "Public - Driver Methods"
    Public Overrides Function ExecuteDataSet(ByVal cmd As CCommand) As DataSet
        Dim c As Byte() = Pack(cmd)
        Dim ds As Byte() = WS.ExecuteDataset(c)
        Return CType(Unpack(ds), DataSet)
    End Function
    Public Overrides Function ExecuteScalar(ByVal cmd As CCommand) As Object
        Dim c As Byte() = Pack(cmd)
        Dim obj As Byte() = WS.ExecuteScalar(c)
        Return Unpack(obj)
    End Function
    Public Overrides Function ExecuteNonQuery(ByVal cmd As CCommand) As Integer
        Dim c As Byte() = Pack(cmd)
        Dim rowsAffected As Byte() = WS.ExecuteNonQuery(c)
        Return CType(Unpack(rowsAffected), Integer)
    End Function

    Public Overrides ReadOnly Property SqlToListAllTables() As String
        Get
            Return CType(Unpack(WS.SqlToListAllTables()), String)
        End Get
    End Property
    Public Overrides Function AllTableNames(Optional withSchema As Boolean = False) As List(Of String)
        Return CType(Unpack(WS.AllTableNames()), List(Of String))
    End Function
#End Region

#Region "Public - Sql Methods"
    Public Overrides Sub BulkInsert(ByVal dt As DataTable, ByVal tableName As String, ByVal mappings As Dictionary(Of Integer, Integer))
        Throw New Exception("TODO: Implement Soap-based BulkInsert (can use REST-based CWebSrcBinary/CWebpage)")
    End Sub
    Public Overrides Sub BulkInsertWithTx(ByVal dt As DataTable, ByVal tableName As String, ByVal mappings As Dictionary(Of Integer, Integer))
        Throw New Exception("TODO: Implement Soap-based BulkInsertWithTx (can use REST-based CWebSrcBinary/CWebpage)")
    End Sub
    Public Overrides Sub BulkSaveDelete(ByVal saves As ICollection, ByVal deletes As ICollection, ByVal txIsolation As IsolationLevel)
        If Not IsNothing(System.Web.HttpContext.Current) Then
            WS.BulkSaveDelete(Pack(saves), Pack(deletes)) 'Web apps get pissy about async requests, 
        Else
            WS.BulkSaveDeleteAsync(Pack(saves), Pack(deletes)) '... but other apps can be more responsive
        End If
    End Sub
    Public Overrides Function Delete(ByVal where As CWhere) As Integer
        If Not IsNothing(System.Web.HttpContext.Current) Then
            Return WS.Delete(Pack(where)) 'Web apps get pissy about async requests, 
        Else
            WS.DeleteAsync(Pack(where)) '... but other apps can be more responsive
            Return 1
        End If
    End Function
    Public Overrides Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal txOrNull As IDbTransaction, ByVal oracleSequenceName As String) As Object
        Dim t As Byte() = Pack(tableName)
        Dim p As Byte() = Pack(pKeyName)
        Dim d As Byte() = Pack(data)
        Dim s As Byte() = Pack(oracleSequenceName)
        Dim obj As Byte() = WS.Insert(t, p, insertPk, d, s)
        Return Unpack(obj)
    End Function

    Public Overrides Sub InsertId(ByVal tableName As String, ByVal data As CNameValueList, isId As Boolean)
        Throw New Exception("Not Implemented")
    End Sub
    Public Overrides Sub InsertId(ByVal tableName As String, ByVal bulk As List(Of CNameValueList), isId As Boolean)
        Throw New Exception("Not Implemented")
    End Sub
    Public Overrides Function [Select](ByVal where As CSelectWhere, ByVal type As EQueryReturnType) As Object
        Dim w As Byte() = Pack(where)
        Dim ds As Byte() = WS.Select(w)
        Return Unpack(ds)
    End Function
    Public Overrides Function SelectCount(ByVal where As CWhere) As Integer
        Dim w As Byte() = Pack(where)
        Return WS.SelectCount(w)
    End Function
    Public Overrides Function Update(ByVal data As CNameValueList, ByVal where As CWhere) As Integer
        Dim d As Byte() = Pack(data)
        Dim w As Byte() = Pack(where)
        Return WS.Update(d, w)
    End Function
    Public Overrides Function UpdateOrdinals(ByVal tableName As String, ByVal pKeyNames As String, ByVal ordinalName As String, ByVal data As CNameValueList) As Integer
        Dim names As New List(Of String)(3)
        names.Add(tableName)
        names.Add(pKeyNames)
        names.Add(ordinalName)

        If Not IsNothing(System.Web.HttpContext.Current) Then
            Return WS.UpdateOrdinals(Pack(names), Pack(data)) 'Web apps get pissy about async requests, 
        Else
            WS.UpdateOrdinalsAsync(Pack(names), Pack(data)) '... but other apps can be more responsive
            Return data.Count
        End If
    End Function
    Public Overloads Overrides Function Paging(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        CheckTxIsNull(txOrNull)

        Dim request As New CWebSrcBinary.CPagingRequest
        request.PageIndexZeroBased = pageIndexZeroBased
        request.PageSize = pageSize
        request.Descending = descending
        request.SortByColumn = sortByColumn
        request.TableName = tableName
        request.SelectColumns = selectColumns

        Dim response As CWebSrcBinary.CPagingResponse = CType(Unpack(WS.Paging(Pack(request))), CWebSrcBinary.CPagingResponse)
        count = response.Count
        Return response.DataSet
    End Function
    Public Overloads Overrides Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        CheckTxIsNull(txOrNull)

        Dim request As New CWebSrcBinary.CPagingWithFiltersRequest
        request.PageIndexZeroBased = pageIndexZeroBased
        request.PageSize = pageSize
        request.Descending = descending
        request.SortByColumn = sortByColumn
        request.TableName = tableName
        request.SelectColumns = selectColumns
        request.Where = criteria

        Dim response As CWebSrcBinary.CPagingResponse = CType(Unpack(WS.PagingWithFilters(Pack(request))), CWebSrcBinary.CPagingResponse)
        count = response.Count
        Return response.DataSet
    End Function
    Protected Overloads Overrides Function BulkSelect(ByVal tables As List(Of CCommand)) As List(Of DataSet)
        Return CType(Unpack(WS.BulkSelect(Pack(tables))), List(Of DataSet))
    End Function

    Protected Overrides Function BulkInsertWithTx_(rowsToInsert As List(Of CNameValueList), tableName As String, primaryKey As String, isIdentity As Boolean) As List(Of Object)
        Throw New NotImplementedException()
    End Function
#End Region

#Region "Private - WebService Reference"
    Private m_webServiceRef As WRDataSrc.WSDataSrc
    Private ReadOnly Property WS() As WRDataSrc.WSDataSrc
        Get
            Return m_webServiceRef
        End Get
    End Property
#End Region

End Class

