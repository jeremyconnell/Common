Imports System.ServiceModel
Imports System.Data
Imports System.Collections

' NOTE: If you change the class name "IWcfDataSrc" here, you must also update the reference to "IWcfDataSrc" in Web.config.
<ServiceContract()> _
Public Interface IWcfDataSrc
    'Low-level driver methods
    <OperationContract()> Function ExecuteDataSet(ByVal cmd As CCommand) As DataSet
    <OperationContract()> Function ExecuteNonQuery(ByVal cmd As CCommand) As Integer
    <OperationContract()> Function ExecuteScalar(ByVal cmd As CCommand) As Object

    'Driver-specific dynamic sql
    <OperationContract()> Function [Select](ByVal where As CSelectWhere) As DataSet
    <OperationContract()> Function SelectCount(ByVal where As CWhere) As Integer
    <OperationContract()> Function Delete(ByVal where As CWhere) As Integer
    <OperationContract()> Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal oracleSequenceName As String) As Object
    <OperationContract()> Function Update(ByVal data As CNameValueList, ByVal where As CWhere) As Integer
    <OperationContract()> Function UpdateOrdinals(ByVal tableName As String, ByVal pKeyName As String, ByVal ordinalName As String, ByVal data As CNameValueList) As Integer
    <OperationContract()> Function Paging(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String) As DataSet
    <OperationContract()> Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList) As DataSet
    <OperationContract()> Function BulkSelect(ByVal tables As List(Of CCommand)) As List(Of DataSet)

    'Transactional
    <OperationContract()> Sub BulkSaveDelete(ByVal saves As ICollection, ByVal deletes As ICollection, ByVal il As IsolationLevel)
End Interface
