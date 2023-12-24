Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CAudit_Data
    Inherits CBaseDynamic
    Implements IComparable(Of CAudit_Data)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CAudit_Data, target As CDataSrc)
        m_dataSrc = target
        m_dataTrailId = original.DataTrailId
        m_dataIsBefore = original.DataIsBefore
        m_dataName = original.DataName
        m_dataValue = original.DataValue
    End Sub

    'Protected (Datareader/Dataset)
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As IDataReader)
        MyBase.New(dataSrc, dr)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As DataRow)
        MyBase.New(dataSrc, dr)
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues_Auto()
        'Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
        m_dataId = Integer.MinValue
        m_dataTrailId = Integer.MinValue
        m_dataIsBefore = False
        m_dataName = String.Empty
        m_dataValue = String.Empty
    End Sub
#End Region

#Region "Members"
    Protected m_dataId As Integer
    Protected m_dataTrailId As Integer
    Protected m_dataIsBefore As Boolean
    Protected m_dataName As String
    Protected m_dataValue As String
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property [DataId]() As Integer
        Get
            Return m_dataId
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property [DataTrailId]() As Integer
        Get
            Return m_dataTrailId
        End Get
        Set(ByVal value As Integer)
            m_dataTrailId = value
        End Set
    End Property
    Public Property [DataIsBefore]() As Boolean
        Get
            Return m_dataIsBefore
        End Get
        Set(ByVal value As Boolean)
            m_dataIsBefore = value
        End Set
    End Property
    Public Property [DataName]() As String
        Get
            Return m_dataName
        End Get
        Set(ByVal value As String)
            m_dataName = value
        End Set
    End Property
    Public Property [DataValue]() As String
        Get
            Return m_dataValue
        End Get
        Set(ByVal value As String)
            m_dataValue = value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblAudit_Data"
    Public Const VIEW_NAME As String  = ""          'Used to override Me.ViewName
    Public Const ORDER_BY_COLS As String = "DataName" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
    Public Const SORTING_COLUMN As String = ""
    Public Overrides ReadOnly Property TableName() As String
        Get
            Return TABLE_NAME
        End Get
    End Property
    Protected Overrides ReadOnly Property OrderByColumns() As String
        Get
            Return ORDER_BY_COLS
        End Get
    End Property

    'CompareTo Interface (Default Sort Order)
    Public Function CompareTo(other As CAudit_Data) As Integer Implements IComparable(Of CAudit_Data).CompareTo
        Return Me.DataName.CompareTo(other.DataName) 
    End Function

    'Primary Key Information
    Public Const PRIMARY_KEY_NAME As String = "DataId"
    Protected Overrides ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return False
        End Get
    End Property
    Protected Overrides ReadOnly Property PrimaryKeyName() As String
        Get
            Return PRIMARY_KEY_NAME
        End Get
    End Property
    Protected Overrides Property PrimaryKeyValue() As Object
        Get
            Return m_dataId
        End Get
        Set(ByVal value As Object)
            m_dataId = CType(Value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CAudit_Data(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CAudit_Data(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CAudit_DataList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CAudit_DataList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_dataId = CAdoData.GetInt(dr, "DataId")
        m_dataTrailId = CAdoData.GetInt(dr, "DataTrailId")
        m_dataIsBefore = CAdoData.GetBool(dr, "DataIsBefore")
        m_dataName = CAdoData.GetStr(dr, "DataName")
        m_dataValue = CAdoData.GetStr(dr, "DataValue")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_dataId = CAdoData.GetInt(dr, "DataId")
        m_dataTrailId = CAdoData.GetInt(dr, "DataTrailId")
        m_dataIsBefore = CAdoData.GetBool(dr, "DataIsBefore")
        m_dataName = CAdoData.GetStr(dr, "DataName")
        m_dataValue = CAdoData.GetStr(dr, "DataValue")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("DataId", NullVal(m_dataId))
        data.Add("DataTrailId", NullVal(m_dataTrailId))
        data.Add("DataIsBefore", NullVal(m_dataIsBefore))
        data.Add("DataName", NullVal(m_dataName))
        data.Add("DataValue", NullVal(m_dataValue))
        Return data
    End Function
#End Region

#Region "Queries - SelectAll/SelectWhere (inherited methods, cast only)"
    'Select Queries - Non-Paged
    Public Shadows Function SelectAll() As CAudit_DataList
        Return CType(MyBase.SelectAll(), CAudit_DataList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String) As CAudit_DataList
        Return CType(MyBase.SelectAll(orderBy), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CAudit_DataList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteria) As CAudit_DataList
        Return CType(MyBase.SelectWhere(where), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList) As CAudit_DataList
        Return CType(MyBase.SelectWhere(where), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String) As CAudit_DataList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, Me.OrderByColumns), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String, ByVal orderBy As String) As CAudit_DataList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, orderBy), CAudit_DataList)
    End Function
    <Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")> _
    Public Shadows Function SelectWhere(ByVal unsafeWhereClause As String) As CAudit_DataList
        Return CType(MyBase.SelectWhere(unsafeWhereClause), CAudit_DataList)
    End Function
    Public Shadows Function SelectById(ByVal dataId As Integer) As CAudit_DataList
        Return CType(MyBase.SelectById(dataId), CAudit_DataList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer)) As CAudit_DataList
        Return CType(MyBase.SelectByIds(ids), CAudit_DataList)
    End Function
    
    'Select Queries - Paged
    Public Shadows Function SelectAll(ByVal pi As CPagingInfo) As CAudit_DataList
        Return CType(MyBase.SelectAll(pi), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CAudit_DataList
        Return CType(MyBase.SelectWhere(pi, colName, sign, colValue), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteria) As CAudit_DataList
        Return CType(MyBase.SelectWhere(pi, criteria), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As CAudit_DataList
        Return CType(MyBase.SelectWhere(pi, criteria), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList, ByVal tableViewOrJoinExpr As String) As CAudit_DataList
        Return CType(MyBase.SelectWhere(pi, criteria, tableViewOrJoinExpr), CAudit_DataList)
    End Function
    Public Shadows Function SelectByIds(ByVal pi As CPagingInfo, ByVal ids As List(Of Integer)) As CAudit_DataList
        Return CType(MyBase.SelectByIds(pi, ids), CAudit_DataList)
    End Function

    'Select Queries - Transactional
    Public Shadows Function SelectAll(ByVal tx As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.SelectAll(tx), CAudit_DataList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String, ByVal tx As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.SelectAll(orderBy, tx), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, columnValue As Object, ByVal tx As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.SelectWhere(columnName, columnValue, tx), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, sign As ESign, columnValue As Object, ByVal tx As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.SelectWhere(columnName, sign, columnValue, tx), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteria, ByVal tx As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.SelectWhere(criteria, tx), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tx As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.SelectWhere(criteria, tx), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal tx As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, tx), CAudit_DataList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal tx As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, orderBy, tx), CAudit_DataList)
    End Function
    Public Shadows Function SelectById(ByVal dataId As Integer, ByVal tx As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.SelectById(dataId, tx), CAudit_DataList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer), ByVal tx As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.SelectByIds(ids, tx), CAudit_DataList)
    End Function
    
    'Select Queries - Stored Procedures
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal txOrNull As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.MakeList(storedProcName, txOrNull), CAudit_DataList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As Object(), ByVal txOrNull As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_DataList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As CNameValueList, ByVal txOrNull As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_DataList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As List(Of Object), ByVal txOrNull As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_DataList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As Integer, ByVal txOrNull As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CAudit_DataList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As String, ByVal txOrNull As IDbTransaction) As CAudit_DataList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CAudit_DataList)
    End Function

    'Query Results
    Protected Overloads Function MakeList(ByVal ds As DataSet) As CAudit_DataList
        Return CType(MyBase.MakeList(ds), CAudit_DataList)
    End Function
    Protected Overloads Function MakeList(ByVal dt As DataTable) As CAudit_DataList
        Return CType(MyBase.MakeList(dt), CAudit_DataList)
    End Function
    Protected Overloads Function MakeList(ByVal rows As DataRowCollection) As CAudit_DataList
        Return CType(MyBase.MakeList(rows), CAudit_DataList)
    End Function
    Protected Overloads Function MakeList(ByVal dr As IDataReader) As CAudit_DataList
        Return CType(MyBase.MakeList(dr), CAudit_DataList)
    End Function
    Protected Overloads Function MakeList(ByVal drOrDs As Object) As CAudit_DataList
        Return CType(MyBase.MakeList(drOrDs), CAudit_DataList)
    End Function
    Protected Overloads Function MakeList(ByVal gzip As Byte()) As CAudit_DataList
        Return CType(MyBase.MakeList(gzip), CAudit_DataList)
    End Function
#End Region

#Region "Queries - SelectBy[FK] (user-nominated fk/bool columns)"
    'Non-Paged
    Public Function SelectByTrailId(ByVal dataTrailId As Integer) As CAudit_DataList
        Return SelectWhere(new CCriteriaList("DataTrailId", dataTrailId))
    End Function
    Public Function SelectByIsBefore(ByVal dataIsBefore As Boolean) As CAudit_DataList
        Return SelectWhere(new CCriteriaList("DataIsBefore", dataIsBefore))
    End Function

    'Paged
    Public Function SelectByTrailId(pi as CPagingInfo, ByVal dataTrailId As Integer) As CAudit_DataList
        Return SelectWhere(pi, New CCriteriaList("DataTrailId", dataTrailId))
    End Function
    Public Function SelectByIsBefore(pi as CPagingInfo, ByVal dataIsBefore As Boolean) As CAudit_DataList
        Return SelectWhere(pi, New CCriteriaList("DataIsBefore", dataIsBefore))
    End Function

    'Count
    Public Function SelectCountByTrailId(ByVal dataTrailId As Integer) As Integer
        Return SelectCount(New CCriteriaList("DataTrailId", dataTrailId))
    End Function
    Public Function SelectCountByIsBefore(ByVal dataIsBefore As Boolean) As Integer
        Return SelectCount(New CCriteriaList("DataIsBefore", dataIsBefore))
    End Function

    'Transactional
    Public Function SelectByTrailId(ByVal dataTrailId As Integer, tx As IDbTransaction) As CAudit_DataList
        Return SelectWhere(New CCriteriaList("DataTrailId", dataTrailId), tx)
    End Function
    Public Function SelectByIsBefore(ByVal dataIsBefore As Boolean, tx As IDbTransaction) As CAudit_DataList
        Return SelectWhere(New CCriteriaList("DataIsBefore", dataIsBefore), tx)
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Autogenerated(ByVal w As System.Xml.XmlWriter)
        Store(w, "DataId", Me.DataId)
        Store(w, "DataTrailId", Me.DataTrailId)
        Store(w, "DataIsBefore", Me.DataIsBefore)
        Store(w, "DataName", Me.DataName)
        Store(w, "DataValue", Me.DataValue)
    End Sub
#End Region



End Class