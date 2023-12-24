Imports Framework

'Note #1: Unlike CWebSrcBinary and CWebSrcSoap, this class does not provide any encryption or compression. i.e. it completely relys on WCF mechanisms for security
'Note #2. Unlike CWebSrcBinary and CWebSrcSoap, this class is loosely coupled (by schema not class) i.e. client is not restricted to .Net apps referencing Framework.dll
'Note #3. Unlike CWebSrcBinary and CWebSrcSoap, this class resides in the application layer, because the burden of WCF configuration does not lend itself towards reuse
Public Class CWebSrcWcf : Inherits CWebSrc


#Region "Constructors"
    Public Sub New()
        MyBase.New(String.Empty, String.Empty)
        m_webServiceRef = New WRWcfDatasrc.WcfDataSrcClient()   'TODO: m_connectionString
    End Sub
    'Public Sub New(ByVal url As String)
    '    MyBase.New(url, CConfigBase.WebServicePassword)

    '    'Web service
    '    m_webServiceRef = New WRWcfDatasrc.WcfDataSrcClient

    '    'TODO: Set url programatically
    'End Sub
#End Region

#Region "Url-Helper"
    Protected Overrides Function DefaultPageName(ByVal url As String) As String
        If Not url.ToLower().Contains(".svc") And Len(url) > 0 Then
            If "/" <> url.Substring(url.Length - 1, 1) Then url &= "/"
            url &= "webservices/wcf/WcfDataSrc.svc"
        End If
        Return url
    End Function
#End Region

#Region "Low-Level Methods"
    Public Overloads Overrides Function ExecuteDataSet(ByVal cmd As CCommand) As DataSet
        Return WS.ExecuteDataSet(Map(cmd))
    End Function
    Public Overloads Overrides Function ExecuteNonQuery(ByVal cmd As CCommand) As Integer
        Return WS.ExecuteNonQuery(cmd)
    End Function
    Public Overloads Overrides Function ExecuteScalar(ByVal cmd As CCommand) As Object
        Return WS.ExecuteScalar(cmd)
    End Function
#End Region

#Region "Driver-Specific Dynamic Sql"
    Public Overrides Function [Select](ByVal where As CSelectWhere, ByVal type As EQueryReturnType) As Object
        Return WS.Select(where) 'ignore type, never use datareader, always dataset
    End Function
    Public Overloads Overrides Function SelectCount(ByVal where As CWhere) As Integer
        Return WS.SelectCount(where)
    End Function
    Public Overrides Function Delete(ByVal where As CWhere) As Integer
        Return WS.Delete(where)
    End Function
    Public Overrides Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal txOrNull As IDbTransaction, ByVal oracleSequenceName As String) As Object
        CDataSrcRemote.CheckTxIsNull(txOrNull)
        Return WS.Insert(tableName, pKeyName, insertPk, data, oracleSequenceName)
    End Function
    Public Overrides Function Update(ByVal data As CNameValueList, ByVal where As CWhere) As Integer
        Return WS.Update(data, where)
    End Function
    Public Overrides Function UpdateOrdinals(ByVal tableName As String, ByVal pKeyName As String, ByVal ordinalName As String, ByVal data As CNameValueList) As Integer
        Return WS.UpdateOrdinals(tableName, pKeyName, ordinalName, data)
    End Function
    Public Overloads Overrides Function Paging(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal txOrNull As IDbTransaction, ByVal type As Framework.EQueryReturnType) As Object
        Return WS.Paging(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns)
    End Function
    Public Overloads Overrides Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As Framework.CCriteriaList, ByVal txOrNull As IDbTransaction, ByVal type As Framework.EQueryReturnType) As Object
        Return WS.PagingWithFilters(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, criteria)
    End Function
    Protected Overloads Overrides Function BulkSelect(ByVal tables As List(Of CCommand)) As List(Of DataSet)
        Return WS.BulkSelect(tables)
    End Function
#End Region

#Region "Transactional"
    Public Overloads Overrides Sub BulkSaveDelete(ByVal saves As ICollection, ByVal deletes As ICollection, ByVal il As IsolationLevel)
        WS.BulkSaveDelete(saves, deletes, il)
    End Sub

    Protected Overrides Function BulkInsertWithTx_(rowsToInsert As List(Of CNameValueList), tableName As String, primaryKey As String, isIdentity As Boolean) As List(Of Object)
        Throw New NotImplementedException()
    End Function

    Public Overrides Sub InsertId(tableName As String, data As CNameValueList, isId As Boolean)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub InsertId(tableName As String, data As List(Of CNameValueList), isId As Boolean)
        Throw New NotImplementedException()
    End Sub
#End Region

#Region "Mapping"
    Private Function Map(cmd As CCommand) As WRWcfDatasrc.CCommand
        Dim wcf As New WRWcfDatasrc.CCommand
        wcf.Text = cmd.Text
        wcf.CommandType = cmd.CommandType
        wcf.ParametersNamed = Map(cmd.ParametersNamed)
        wcf.ParametersUnnamed = New List(Of Object)(cmd.ParametersUnnamed)
        wcf.TimeoutSecs = cmd.TimeoutSecs
        Return wcf
    End Function

    Private Function Map(list As CNameValueList) As List(Of WRWcfDatasrc.CNameValue)

    End Function
#End Region

#Region "Private - WebService Reference"
    Private m_webServiceRef As WRWcfDatasrc.WcfDataSrcClient
    Private ReadOnly Property WS() As WRWcfDatasrc.WcfDataSrcClient
        Get
            Return m_webServiceRef
        End Get
    End Property
#End Region

End Class

