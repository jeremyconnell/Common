Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CAudit_Error : Inherits CBaseDynamic : Implements IComparable(Of CAudit_Error)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original As CAudit_Error, ByVal target As CDataSrc)
        m_dataSrc = target
        m_errorUserID = original.ErrorUserID
        m_errorUserName = original.ErrorUserName
        m_errorWebsite = original.ErrorWebsite
        m_errorUrl = original.ErrorUrl
        m_errorMachineName = original.ErrorMachineName
        m_errorApplicationName = original.ErrorApplicationName
        m_errorApplicationVersion = original.ErrorApplicationVersion
        m_errorType = original.ErrorType
        m_errorTypeHash = original.ErrorTypeHash
        m_errorMessage = original.ErrorMessage
        m_errorMessageHash = original.ErrorMessageHash
        m_errorStacktrace = original.ErrorStacktrace
        m_errorInnerType = original.ErrorInnerType
        m_errorInnerTypeHash = original.ErrorInnerTypeHash
        m_errorInnerMessage = original.ErrorInnerMessage
        m_errorInnerMessageHash = original.ErrorInnerMessageHash
        m_errorInnerStacktrace = original.ErrorInnerStacktrace
        m_errorDateCreated = original.ErrorDateCreated
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
        m_errorID = Integer.MinValue
        m_errorUserID = String.Empty
        m_errorUserName = String.Empty
        m_errorWebsite = String.Empty
        m_errorUrl = String.Empty
        m_errorMachineName = String.Empty
        m_errorApplicationName = String.Empty
        m_errorApplicationVersion = String.Empty
        m_errorType = String.Empty
        m_errorTypeHash = Integer.MinValue
        m_errorMessage = String.Empty
        m_errorMessageHash = Integer.MinValue
        m_errorStacktrace = String.Empty
        m_errorInnerType = String.Empty
        m_errorInnerTypeHash = Integer.MinValue
        m_errorInnerMessage = String.Empty
        m_errorInnerMessageHash = Integer.MinValue
        m_errorInnerStacktrace = String.Empty
        m_errorDateCreated = DateTime.MinValue
    End Sub
#End Region

#Region "Members"
    Protected m_errorID As Integer
    Protected m_errorUserID As String
    Protected m_errorUserName As String
    Protected m_errorWebsite As String
    Protected m_errorUrl As String
    Protected m_errorMachineName As String
    Protected m_errorApplicationName As String
    Protected m_errorApplicationVersion As String
    Protected m_errorType As String
    Protected m_errorTypeHash As Integer
    Protected m_errorMessage As String
    Protected m_errorMessageHash As Integer
    Protected m_errorStacktrace As String
    Protected m_errorInnerType As String
    Protected m_errorInnerTypeHash As Integer
    Protected m_errorInnerMessage As String
    Protected m_errorInnerMessageHash As Integer
    Protected m_errorInnerStacktrace As String
    Protected m_errorDateCreated As DateTime
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property [ErrorID]() As Integer
        Get
            Return m_errorID
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property [ErrorUserID]() As String
        Get
            Return m_errorUserID
        End Get
        Set(ByVal value As String)
            m_errorUserID = value
        End Set
    End Property
    Public Property [ErrorUserName]() As String
        Get
            Return m_errorUserName
        End Get
        Set(ByVal value As String)
            m_errorUserName = value
        End Set
    End Property
    Public Property [ErrorWebsite]() As String
        Get
            Return m_errorWebsite
        End Get
        Set(ByVal value As String)
            m_errorWebsite = value
        End Set
    End Property
    Public Property [ErrorUrl]() As String
        Get
            Return m_errorUrl
        End Get
        Set(ByVal value As String)
            m_errorUrl = value
        End Set
    End Property
    Public Property [ErrorMachineName]() As String
        Get
            Return m_errorMachineName
        End Get
        Set(ByVal value As String)
            m_errorMachineName = value
        End Set
    End Property
    Public Property [ErrorApplicationName]() As String
        Get
            Return m_errorApplicationName
        End Get
        Set(ByVal value As String)
            m_errorApplicationName = value
        End Set
    End Property
    Public Property [ErrorApplicationVersion]() As String
        Get
            Return m_errorApplicationVersion
        End Get
        Set(ByVal value As String)
            m_errorApplicationVersion = value
        End Set
    End Property
    Public Property [ErrorType]() As String
        Get
            Return m_errorType
        End Get
        Set(ByVal value As String)
            m_errorType = value
        End Set
    End Property
    Public Property [ErrorTypeHash]() As Integer
        Get
            Return m_errorTypeHash
        End Get
        Set(ByVal value As Integer)
            m_errorTypeHash = value
        End Set
    End Property
    Public Property [ErrorMessage]() As String
        Get
            Return m_errorMessage
        End Get
        Set(ByVal value As String)
            m_errorMessage = value
        End Set
    End Property
    Public Property [ErrorMessageHash]() As Integer
        Get
            Return m_errorMessageHash
        End Get
        Set(ByVal value As Integer)
            m_errorMessageHash = value
        End Set
    End Property
    Public Property [ErrorStacktrace]() As String
        Get
            Return m_errorStacktrace
        End Get
        Set(ByVal value As String)
            m_errorStacktrace = value
        End Set
    End Property
    Public Property [ErrorInnerType]() As String
        Get
            Return m_errorInnerType
        End Get
        Set(ByVal value As String)
            m_errorInnerType = value
        End Set
    End Property
    Public Property [ErrorInnerTypeHash]() As Integer
        Get
            Return m_errorInnerTypeHash
        End Get
        Set(ByVal value As Integer)
            m_errorInnerTypeHash = value
        End Set
    End Property
    Public Property [ErrorInnerMessage]() As String
        Get
            Return m_errorInnerMessage
        End Get
        Set(ByVal value As String)
            m_errorInnerMessage = value
        End Set
    End Property
    Public Property [ErrorInnerMessageHash]() As Integer
        Get
            Return m_errorInnerMessageHash
        End Get
        Set(ByVal value As Integer)
            m_errorInnerMessageHash = value
        End Set
    End Property
    Public Property [ErrorInnerStacktrace]() As String
        Get
            Return m_errorInnerStacktrace
        End Get
        Set(ByVal value As String)
            m_errorInnerStacktrace = value
        End Set
    End Property
    Public Property [ErrorDateCreated]() As DateTime
        Get
            Return m_errorDateCreated
        End Get
        Set(ByVal value As DateTime)
            m_errorDateCreated = value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblAudit_Error"
    Public Const VIEW_NAME As String = ""          'Used to override Me.ViewName
    Public Const ORDER_BY_COLS As String = "ErrorDateCreated DESC" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
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
    Public Function CompareTo(ByVal other As CAudit_Error) As Integer Implements IComparable(Of CAudit_Error).CompareTo
        Return Me.ErrorDateCreated.CompareTo(other.ErrorDateCreated) * -1
    End Function

    'Primary Key Information
    Public Const PRIMARY_KEY_NAME As String = "ErrorID"
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
            Return m_errorID
        End Get
        Set(ByVal value As Object)
            m_errorID = CType(value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CAudit_Error(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CAudit_Error(Me.DataSrc, dr)
    End Function

    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CAudit_ErrorList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CAudit_ErrorList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_errorID = CAdoData.GetInt(dr, "ErrorID")
        m_errorUserID = CAdoData.GetStr(dr, "ErrorUserID")
        m_errorUserName = CAdoData.GetStr(dr, "ErrorUserName")
        m_errorWebsite = CAdoData.GetStr(dr, "ErrorWebsite")
        m_errorUrl = CAdoData.GetStr(dr, "ErrorUrl")
        m_errorMachineName = CAdoData.GetStr(dr, "ErrorMachineName")
        m_errorApplicationName = CAdoData.GetStr(dr, "ErrorApplicationName")
        m_errorApplicationVersion = CAdoData.GetStr(dr, "ErrorApplicationVersion")
        m_errorType = CAdoData.GetStr(dr, "ErrorType")
        m_errorTypeHash = CAdoData.GetInt(dr, "ErrorTypeHash")
        m_errorMessage = CAdoData.GetStr(dr, "ErrorMessage")
        m_errorMessageHash = CAdoData.GetInt(dr, "ErrorMessageHash")
        m_errorStacktrace = CAdoData.GetStr(dr, "ErrorStacktrace")
        m_errorInnerType = CAdoData.GetStr(dr, "ErrorInnerType")
        m_errorInnerTypeHash = CAdoData.GetInt(dr, "ErrorInnerTypeHash")
        m_errorInnerMessage = CAdoData.GetStr(dr, "ErrorInnerMessage")
        m_errorInnerMessageHash = CAdoData.GetInt(dr, "ErrorInnerMessageHash")
        m_errorInnerStacktrace = CAdoData.GetStr(dr, "ErrorInnerStacktrace")
        m_errorDateCreated = CAdoData.GetDate(dr, "ErrorDateCreated")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_errorID = CAdoData.GetInt(dr, "ErrorID")
        m_errorUserID = CAdoData.GetStr(dr, "ErrorUserID")
        m_errorUserName = CAdoData.GetStr(dr, "ErrorUserName")
        m_errorWebsite = CAdoData.GetStr(dr, "ErrorWebsite")
        m_errorUrl = CAdoData.GetStr(dr, "ErrorUrl")
        m_errorMachineName = CAdoData.GetStr(dr, "ErrorMachineName")
        m_errorApplicationName = CAdoData.GetStr(dr, "ErrorApplicationName")
        m_errorApplicationVersion = CAdoData.GetStr(dr, "ErrorApplicationVersion")
        m_errorType = CAdoData.GetStr(dr, "ErrorType")
        m_errorTypeHash = CAdoData.GetInt(dr, "ErrorTypeHash")
        m_errorMessage = CAdoData.GetStr(dr, "ErrorMessage")
        m_errorMessageHash = CAdoData.GetInt(dr, "ErrorMessageHash")
        m_errorStacktrace = CAdoData.GetStr(dr, "ErrorStacktrace")
        m_errorInnerType = CAdoData.GetStr(dr, "ErrorInnerType")
        m_errorInnerTypeHash = CAdoData.GetInt(dr, "ErrorInnerTypeHash")
        m_errorInnerMessage = CAdoData.GetStr(dr, "ErrorInnerMessage")
        m_errorInnerMessageHash = CAdoData.GetInt(dr, "ErrorInnerMessageHash")
        m_errorInnerStacktrace = CAdoData.GetStr(dr, "ErrorInnerStacktrace")
        m_errorDateCreated = CAdoData.GetDate(dr, "ErrorDateCreated")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("ErrorID", NullVal(m_errorID))
        data.Add("ErrorUserID", NullVal(m_errorUserID))
        data.Add("ErrorUserName", NullVal(m_errorUserName))
        data.Add("ErrorWebsite", NullVal(m_errorWebsite))
        data.Add("ErrorUrl", NullVal(m_errorUrl))
        data.Add("ErrorMachineName", NullVal(m_errorMachineName))
        data.Add("ErrorApplicationName", NullVal(m_errorApplicationName))
        data.Add("ErrorApplicationVersion", NullVal(m_errorApplicationVersion))
        data.Add("ErrorType", NullVal(m_errorType))
        data.Add("ErrorTypeHash", NullVal(m_errorTypeHash))
        data.Add("ErrorMessage", NullVal(m_errorMessage))
        data.Add("ErrorMessageHash", NullVal(m_errorMessageHash))
        data.Add("ErrorStacktrace", NullVal(m_errorStacktrace))
        data.Add("ErrorInnerType", NullVal(m_errorInnerType))
        data.Add("ErrorInnerTypeHash", NullVal(m_errorInnerTypeHash))
        data.Add("ErrorInnerMessage", NullVal(m_errorInnerMessage))
        data.Add("ErrorInnerMessageHash", NullVal(m_errorInnerMessageHash))
        data.Add("ErrorInnerStacktrace", NullVal(m_errorInnerStacktrace))
        data.Add("ErrorDateCreated", NullVal(m_errorDateCreated))
        Return data
    End Function
#End Region

#Region "Queries - SelectAll/SelectWhere (inherited methods, cast only)"
    'Select Queries - Non-Paged
    Public Shadows Function SelectAll() As CAudit_ErrorList
        Return CType(MyBase.SelectAll(), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String) As CAudit_ErrorList
        Return CType(MyBase.SelectAll(orderBy), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteria) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(where), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(where), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, Me.OrderByColumns), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String, ByVal orderBy As String) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, orderBy), CAudit_ErrorList)
    End Function
    <Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")> _
    Public Shadows Function SelectWhere(ByVal unsafeWhereClause As String) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(unsafeWhereClause), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer)) As CAudit_ErrorList
        Return CType(MyBase.SelectByIds(ids), CAudit_ErrorList)
    End Function

    'Select Queries - Paged
    Public Shadows Function SelectAll(ByVal pi As CPagingInfo) As CAudit_ErrorList
        Return CType(MyBase.SelectAll(pi), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(pi, colName, sign, colValue), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteria) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(pi, criteria), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(pi, criteria), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList, ByVal tableViewOrJoinExpr As String) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(pi, criteria, tableViewOrJoinExpr), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectByIds(ByVal pi As CPagingInfo, ByVal ids As List(Of Integer)) As CAudit_ErrorList
        Return CType(MyBase.SelectByIds(pi, ids), CAudit_ErrorList)
    End Function

    'Select Queries - Transactional
    Public Shadows Function SelectAll(ByVal tx As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.SelectAll(tx), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String, ByVal tx As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.SelectAll(orderBy, tx), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, ByVal columnValue As Object, ByVal tx As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(columnName, columnValue, tx), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, ByVal sign As ESign, ByVal columnValue As Object, ByVal tx As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(columnName, sign, columnValue, tx), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteria, ByVal tx As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(criteria, tx), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tx As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(criteria, tx), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal tx As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, tx), CAudit_ErrorList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal tx As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, orderBy, tx), CAudit_ErrorList)
    End Function

    'Select Queries - Stored Procedures
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal txOrNull As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.MakeList(storedProcName, txOrNull), CAudit_ErrorList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As Object(), ByVal txOrNull As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_ErrorList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As CNameValueList, ByVal txOrNull As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_ErrorList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As List(Of Object), ByVal txOrNull As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_ErrorList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As Integer, ByVal txOrNull As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CAudit_ErrorList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As String, ByVal txOrNull As IDbTransaction) As CAudit_ErrorList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CAudit_ErrorList)
    End Function

    'Query Results
    Protected Overloads Function MakeList(ByVal ds As DataSet) As CAudit_ErrorList
        Return CType(MyBase.MakeList(ds), CAudit_ErrorList)
    End Function
    Protected Overloads Function MakeList(ByVal dt As DataTable) As CAudit_ErrorList
        Return CType(MyBase.MakeList(dt), CAudit_ErrorList)
    End Function
    Protected Overloads Function MakeList(ByVal rows As DataRowCollection) As CAudit_ErrorList
        Return CType(MyBase.MakeList(rows), CAudit_ErrorList)
    End Function
    Protected Overloads Function MakeList(ByVal dr As IDataReader) As CAudit_ErrorList
        Return CType(MyBase.MakeList(dr), CAudit_ErrorList)
    End Function
    Protected Overloads Function MakeList(ByVal drOrDs As Object) As CAudit_ErrorList
        Return CType(MyBase.MakeList(drOrDs), CAudit_ErrorList)
    End Function
    Protected Overloads Function MakeList(ByVal gzip As Byte()) As CAudit_ErrorList
        Return CType(MyBase.MakeList(gzip), CAudit_ErrorList)
    End Function
#End Region

#Region "Queries - SelectBy[FK] (user-nominated fk/bool columns)"
    'Non-Paged
    Public Function SelectByUserID(ByVal errorUserID As String) As CAudit_ErrorList
        Return SelectByUserID(Nothing, errorUserID)
    End Function

    'Paged
    Public Function SelectByUserID(ByVal pi As CPagingInfo, ByVal errorUserID As String) As CAudit_ErrorList
        Return SelectWhere(pi, New CCriteria("ErrorUserID", errorUserID))
    End Function

    'Count
    Public Function SelectCountByUserID(ByVal errorUserID As String) As Integer
        Return SelectCount(New CCriteriaList("ErrorUserID", errorUserID))
    End Function

    'Transactional
    Friend Function SelectByUserID(ByVal errorUserID As String, ByVal tx As IDbTransaction) As CAudit_ErrorList
        Return SelectWhere(New CCriteria("ErrorUserID", errorUserID), tx)
    End Function

#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Autogenerated(ByVal w As System.Xml.XmlWriter)
        Store(w, "ErrorID", Me.ErrorID)
        Store(w, "ErrorUserID", Me.ErrorUserID)
        Store(w, "ErrorUserName", Me.ErrorUserName)
        Store(w, "ErrorWebsite", Me.ErrorWebsite)
        Store(w, "ErrorUrl", Me.ErrorUrl)
        Store(w, "ErrorMachineName", Me.ErrorMachineName)
        Store(w, "ErrorApplicationName", Me.ErrorApplicationName)
        Store(w, "ErrorApplicationVersion", Me.ErrorApplicationVersion)
        Store(w, "ErrorType", Me.ErrorType)
        Store(w, "ErrorTypeHash", Me.ErrorTypeHash)
        Store(w, "ErrorMessage", Me.ErrorMessage)
        Store(w, "ErrorMessageHash", Me.ErrorMessageHash)
        Store(w, "ErrorStacktrace", Me.ErrorStacktrace)
        Store(w, "ErrorInnerType", Me.ErrorInnerType)
        Store(w, "ErrorInnerTypeHash", Me.ErrorInnerTypeHash)
        Store(w, "ErrorInnerMessage", Me.ErrorInnerMessage)
        Store(w, "ErrorInnerMessageHash", Me.ErrorInnerMessageHash)
        Store(w, "ErrorInnerStacktrace", Me.ErrorInnerStacktrace)
        Store(w, "ErrorDateCreated", Me.ErrorDateCreated)
    End Sub
#End Region


End Class