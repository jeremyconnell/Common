Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CUser
    Inherits SchemaAudit.CBaseDynamicAudited
    Implements IComparable(Of CUser)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CUser, target As CDataSrc)
        m_dataSrc = target
        m_userLoginName = original.UserLoginName
        m_userPasswordHashedSha1 = original.UserPasswordHashedSha1
        m_userPasswordSalt = original.UserPasswordSalt
        m_userFirstName = original.UserFirstName
        m_userLastName = original.UserLastName
        m_userEmail = original.UserEmail
        m_userIsDisabled = original.UserIsDisabled
        m_userCreatedDate = original.UserCreatedDate
        m_userLastLoginDate = original.UserLastLoginDate
        m_userLastPasswordChangedDate = original.UserLastPasswordChangedDate
        m_userPasswordQuestion = original.UserPasswordQuestion
        m_userPasswordAnswer = original.UserPasswordAnswer
        m_userFailedPasswordAttemptCount = original.UserFailedPasswordAttemptCount
        m_userFailedPasswordAttemptStartDate = original.UserFailedPasswordAttemptStartDate
        m_userIsLockedOut = original.UserIsLockedOut
        m_userLastLockoutDate = original.UserLastLockoutDate
        m_userComments = original.UserComments
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
        m_userLoginName = String.Empty
        m_userPasswordHashedSha1 = String.Empty
        m_userPasswordSalt = String.Empty
        m_userFirstName = String.Empty
        m_userLastName = String.Empty
        m_userEmail = String.Empty
        m_userIsDisabled = False
        m_userCreatedDate = DateTime.MinValue
        m_userLastLoginDate = DateTime.MinValue
        m_userLastPasswordChangedDate = DateTime.MinValue
        m_userPasswordQuestion = String.Empty
        m_userPasswordAnswer = String.Empty
        m_userFailedPasswordAttemptCount = Integer.MinValue
        m_userFailedPasswordAttemptStartDate = DateTime.MinValue
        m_userIsLockedOut = False
        m_userLastLockoutDate = DateTime.MinValue
        m_userComments = String.Empty
    End Sub
#End Region

#Region "Members"
    Protected m_userLoginName As String
    Protected m_userPasswordHashedSha1 As String
    Protected m_userPasswordSalt As String
    Protected m_userFirstName As String
    Protected m_userLastName As String
    Protected m_userEmail As String
    Protected m_userIsDisabled As Boolean
    Protected m_userCreatedDate As DateTime
    Protected m_userLastLoginDate As DateTime
    Protected m_userLastPasswordChangedDate As DateTime
    Protected m_userPasswordQuestion As String
    Protected m_userPasswordAnswer As String
    Protected m_userFailedPasswordAttemptCount As Integer
    Protected m_userFailedPasswordAttemptStartDate As DateTime
    Protected m_userIsLockedOut As Boolean
    Protected m_userLastLockoutDate As DateTime
    Protected m_userComments As String
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public Property [UserLoginName]() As String
        Get
            Return m_userLoginName
        End Get
        Set(ByVal value As String)
            If Not m_insertPending Then
                DataSrc.Update(New CNameValueList("UserLoginName", value), New CWhere(TABLE_NAME, New CCriteria("UserLoginName", m_userLoginName), Nothing))
            End If
            m_userLoginName = value
            CacheClear()
        End Set
    End Property

    'Table Columns (Read/Write)
    Public Property [UserPasswordHashedSha1]() As String
        Get
            Return m_userPasswordHashedSha1
        End Get
        Set(ByVal value As String)
            m_userPasswordHashedSha1 = value
        End Set
    End Property
    Public Property [UserPasswordSalt]() As String
        Get
            Return m_userPasswordSalt
        End Get
        Set(ByVal value As String)
            m_userPasswordSalt = value
        End Set
    End Property
    Public Property [UserFirstName]() As String
        Get
            Return m_userFirstName
        End Get
        Set(ByVal value As String)
            m_userFirstName = value
        End Set
    End Property
    Public Property [UserLastName]() As String
        Get
            Return m_userLastName
        End Get
        Set(ByVal value As String)
            m_userLastName = value
        End Set
    End Property
    Public Property [UserEmail]() As String
        Get
            Return m_userEmail
        End Get
        Set(ByVal value As String)
            m_userEmail = value
        End Set
    End Property
    Public Property [UserIsDisabled]() As Boolean
        Get
            Return m_userIsDisabled
        End Get
        Set(ByVal value As Boolean)
            m_userIsDisabled = value
        End Set
    End Property
    Public Property [UserCreatedDate]() As DateTime
        Get
            Return m_userCreatedDate
        End Get
        Set(ByVal value As DateTime)
            m_userCreatedDate = value
        End Set
    End Property
    Public Property [UserLastLoginDate]() As DateTime
        Get
            Return m_userLastLoginDate
        End Get
        Set(ByVal value As DateTime)
            m_userLastLoginDate = value
        End Set
    End Property
    Public Property [UserLastPasswordChangedDate]() As DateTime
        Get
            Return m_userLastPasswordChangedDate
        End Get
        Set(ByVal value As DateTime)
            m_userLastPasswordChangedDate = value
        End Set
    End Property
    Public Property [UserPasswordQuestion]() As String
        Get
            Return m_userPasswordQuestion
        End Get
        Set(ByVal value As String)
            m_userPasswordQuestion = value
        End Set
    End Property
    Public Property [UserPasswordAnswer]() As String
        Get
            Return m_userPasswordAnswer
        End Get
        Set(ByVal value As String)
            m_userPasswordAnswer = value
        End Set
    End Property
    Public Property [UserFailedPasswordAttemptCount]() As Integer
        Get
            Return m_userFailedPasswordAttemptCount
        End Get
        Set(ByVal value As Integer)
            m_userFailedPasswordAttemptCount = value
        End Set
    End Property
    Public Property [UserFailedPasswordAttemptStartDate]() As DateTime
        Get
            Return m_userFailedPasswordAttemptStartDate
        End Get
        Set(ByVal value As DateTime)
            m_userFailedPasswordAttemptStartDate = value
        End Set
    End Property
    Public Property [UserIsLockedOut]() As Boolean
        Get
            Return m_userIsLockedOut
        End Get
        Set(ByVal value As Boolean)
            m_userIsLockedOut = value
        End Set
    End Property
    Public Property [UserLastLockoutDate]() As DateTime
        Get
            Return m_userLastLockoutDate
        End Get
        Set(ByVal value As DateTime)
            m_userLastLockoutDate = value
        End Set
    End Property
    Public Property [UserComments]() As String
        Get
            Return m_userComments
        End Get
        Set(ByVal value As String)
            m_userComments = value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblMembership_User"
    Public Const VIEW_NAME As String  = ""          'Used to override Me.ViewName
    Public Const ORDER_BY_COLS As String = "UserLoginName" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
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
    Public Function CompareTo(other As CUser) As Integer Implements IComparable(Of CUser).CompareTo
        Return Me.UserLoginName.CompareTo(other.UserLoginName) 
    End Function

    'Primary Key Information
    Public Const PRIMARY_KEY_NAME As String = "UserLoginName"
    Protected Overrides ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return True
        End Get
    End Property
    Protected Overrides ReadOnly Property PrimaryKeyName() As String
        Get
            Return PRIMARY_KEY_NAME
        End Get
    End Property
    Protected Overrides Property PrimaryKeyValue() As Object
        Get
            Return m_userLoginName
        End Get
        Set(ByVal value As Object)
            If Not m_insertPending Then 'Note: Use cascade update for relationships
                DataSrc.Update(New CNameValueList("UserLoginName", value), New CWhere(TABLE_NAME, New CCriteria("UserLoginName", Me.UserLoginName), Nothing))
                CacheClear()
            End If
            m_userLoginName = CType(Value, String)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CUser(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CUser(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CUserList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CUserList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_userLoginName = CAdoData.GetStr(dr, "UserLoginName")
        m_userPasswordHashedSha1 = CAdoData.GetStr(dr, "UserPasswordHashedSha1")
        m_userPasswordSalt = CAdoData.GetStr(dr, "UserPasswordSalt")
        m_userFirstName = CAdoData.GetStr(dr, "UserFirstName")
        m_userLastName = CAdoData.GetStr(dr, "UserLastName")
        m_userEmail = CAdoData.GetStr(dr, "UserEmail")
        m_userIsDisabled = CAdoData.GetBool(dr, "UserIsDisabled")
        m_userCreatedDate = CAdoData.GetDate(dr, "UserCreatedDate")
        m_userLastLoginDate = CAdoData.GetDate(dr, "UserLastLoginDate")
        m_userLastPasswordChangedDate = CAdoData.GetDate(dr, "UserLastPasswordChangedDate")
        m_userPasswordQuestion = CAdoData.GetStr(dr, "UserPasswordQuestion")
        m_userPasswordAnswer = CAdoData.GetStr(dr, "UserPasswordAnswer")
        m_userFailedPasswordAttemptCount = CAdoData.GetInt(dr, "UserFailedPasswordAttemptCount")
        m_userFailedPasswordAttemptStartDate = CAdoData.GetDate(dr, "UserFailedPasswordAttemptStartDate")
        m_userIsLockedOut = CAdoData.GetBool(dr, "UserIsLockedOut")
        m_userLastLockoutDate = CAdoData.GetDate(dr, "UserLastLockoutDate")
        m_userComments = CAdoData.GetStr(dr, "UserComments")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_userLoginName = CAdoData.GetStr(dr, "UserLoginName")
        m_userPasswordHashedSha1 = CAdoData.GetStr(dr, "UserPasswordHashedSha1")
        m_userPasswordSalt = CAdoData.GetStr(dr, "UserPasswordSalt")
        m_userFirstName = CAdoData.GetStr(dr, "UserFirstName")
        m_userLastName = CAdoData.GetStr(dr, "UserLastName")
        m_userEmail = CAdoData.GetStr(dr, "UserEmail")
        m_userIsDisabled = CAdoData.GetBool(dr, "UserIsDisabled")
        m_userCreatedDate = CAdoData.GetDate(dr, "UserCreatedDate")
        m_userLastLoginDate = CAdoData.GetDate(dr, "UserLastLoginDate")
        m_userLastPasswordChangedDate = CAdoData.GetDate(dr, "UserLastPasswordChangedDate")
        m_userPasswordQuestion = CAdoData.GetStr(dr, "UserPasswordQuestion")
        m_userPasswordAnswer = CAdoData.GetStr(dr, "UserPasswordAnswer")
        m_userFailedPasswordAttemptCount = CAdoData.GetInt(dr, "UserFailedPasswordAttemptCount")
        m_userFailedPasswordAttemptStartDate = CAdoData.GetDate(dr, "UserFailedPasswordAttemptStartDate")
        m_userIsLockedOut = CAdoData.GetBool(dr, "UserIsLockedOut")
        m_userLastLockoutDate = CAdoData.GetDate(dr, "UserLastLockoutDate")
        m_userComments = CAdoData.GetStr(dr, "UserComments")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("UserLoginName", NullVal(m_userLoginName))
        data.Add("UserPasswordHashedSha1", NullVal(m_userPasswordHashedSha1))
        data.Add("UserPasswordSalt", NullVal(m_userPasswordSalt))
        data.Add("UserFirstName", NullVal(m_userFirstName))
        data.Add("UserLastName", NullVal(m_userLastName))
        data.Add("UserEmail", NullVal(m_userEmail))
        data.Add("UserIsDisabled", NullVal(m_userIsDisabled))
        data.Add("UserCreatedDate", NullVal(m_userCreatedDate))
        data.Add("UserLastLoginDate", NullVal(m_userLastLoginDate))
        data.Add("UserLastPasswordChangedDate", NullVal(m_userLastPasswordChangedDate))
        data.Add("UserPasswordQuestion", NullVal(m_userPasswordQuestion))
        data.Add("UserPasswordAnswer", NullVal(m_userPasswordAnswer))
        data.Add("UserFailedPasswordAttemptCount", NullVal(m_userFailedPasswordAttemptCount))
        data.Add("UserFailedPasswordAttemptStartDate", NullVal(m_userFailedPasswordAttemptStartDate))
        data.Add("UserIsLockedOut", NullVal(m_userIsLockedOut))
        data.Add("UserLastLockoutDate", NullVal(m_userLastLockoutDate))
        data.Add("UserComments", NullVal(m_userComments))
        Return data
    End Function
#End Region

#Region "Queries - SelectAll/SelectWhere (inherited methods, cast only)"
    'Select Queries - Non-Paged
    Public Shadows Function SelectAll() As CUserList
        Return CType(MyBase.SelectAll(), CUserList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String) As CUserList
        Return CType(MyBase.SelectAll(orderBy), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CUserList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteria) As CUserList
        Return CType(MyBase.SelectWhere(where), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList) As CUserList
        Return CType(MyBase.SelectWhere(where), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String) As CUserList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, Me.OrderByColumns), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String, ByVal orderBy As String) As CUserList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, orderBy), CUserList)
    End Function
    <Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")> _
    Public Shadows Function SelectWhere(ByVal unsafeWhereClause As String) As CUserList
        Return CType(MyBase.SelectWhere(unsafeWhereClause), CUserList)
    End Function
    Public Shadows Function SelectById(ByVal userLoginName As String) As CUserList
        Return CType(MyBase.SelectById(userLoginName), CUserList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of String)) As CUserList
        Return CType(MyBase.SelectByIds(ids), CUserList)
    End Function
    
    'Select Queries - Paged
    Public Shadows Function SelectAll(ByVal pi As CPagingInfo) As CUserList
        Return CType(MyBase.SelectAll(pi), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CUserList
        Return CType(MyBase.SelectWhere(pi, colName, sign, colValue), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteria) As CUserList
        Return CType(MyBase.SelectWhere(pi, criteria), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As CUserList
        Return CType(MyBase.SelectWhere(pi, criteria), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList, ByVal tableViewOrJoinExpr As String) As CUserList
        Return CType(MyBase.SelectWhere(pi, criteria, tableViewOrJoinExpr), CUserList)
    End Function
    Public Shadows Function SelectByIds(ByVal pi As CPagingInfo, ByVal ids As List(Of String)) As CUserList
        Return CType(MyBase.SelectByIds(pi, ids), CUserList)
    End Function

    'Select Queries - Transactional
    Public Shadows Function SelectAll(ByVal tx As IDbTransaction) As CUserList
        Return CType(MyBase.SelectAll(tx), CUserList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String, ByVal tx As IDbTransaction) As CUserList
        Return CType(MyBase.SelectAll(orderBy, tx), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, columnValue As Object, ByVal tx As IDbTransaction) As CUserList
        Return CType(MyBase.SelectWhere(columnName, columnValue, tx), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, sign As ESign, columnValue As Object, ByVal tx As IDbTransaction) As CUserList
        Return CType(MyBase.SelectWhere(columnName, sign, columnValue, tx), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteria, ByVal tx As IDbTransaction) As CUserList
        Return CType(MyBase.SelectWhere(criteria, tx), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tx As IDbTransaction) As CUserList
        Return CType(MyBase.SelectWhere(criteria, tx), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal tx As IDbTransaction) As CUserList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, tx), CUserList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal tx As IDbTransaction) As CUserList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, orderBy, tx), CUserList)
    End Function
    Public Shadows Function SelectById(ByVal userLoginName As String, ByVal tx As IDbTransaction) As CUserList
        Return CType(MyBase.SelectById(userLoginName, tx), CUserList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of String), ByVal tx As IDbTransaction) As CUserList
        Return CType(MyBase.SelectByIds(ids, tx), CUserList)
    End Function
    
    'Select Queries - Stored Procedures
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal txOrNull As IDbTransaction) As CUserList
        Return CType(MyBase.MakeList(storedProcName, txOrNull), CUserList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As Object(), ByVal txOrNull As IDbTransaction) As CUserList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CUserList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As CNameValueList, ByVal txOrNull As IDbTransaction) As CUserList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CUserList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As List(Of Object), ByVal txOrNull As IDbTransaction) As CUserList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CUserList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As Integer, ByVal txOrNull As IDbTransaction) As CUserList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CUserList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As String, ByVal txOrNull As IDbTransaction) As CUserList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CUserList)
    End Function

    'Query Results
    Protected Overloads Function MakeList(ByVal ds As DataSet) As CUserList
        Return CType(MyBase.MakeList(ds), CUserList)
    End Function
    Protected Overloads Function MakeList(ByVal dt As DataTable) As CUserList
        Return CType(MyBase.MakeList(dt), CUserList)
    End Function
    Protected Overloads Function MakeList(ByVal rows As DataRowCollection) As CUserList
        Return CType(MyBase.MakeList(rows), CUserList)
    End Function
    Protected Overloads Function MakeList(ByVal dr As IDataReader) As CUserList
        Return CType(MyBase.MakeList(dr), CUserList)
    End Function
    Protected Overloads Function MakeList(ByVal drOrDs As Object) As CUserList
        Return CType(MyBase.MakeList(drOrDs), CUserList)
    End Function
    Protected Overloads Function MakeList(ByVal gzip As Byte()) As CUserList
        Return CType(MyBase.MakeList(gzip), CUserList)
    End Function
#End Region

#Region "Queries - SelectBy[FK] (user-nominated fk/bool columns)"
    'Non-Paged
    Public Function SelectByIsDisabled(ByVal userIsDisabled As Boolean) As CUserList
        Return SelectWhere(new CCriteriaList("UserIsDisabled", userIsDisabled))
    End Function
    Public Function SelectByIsLockedOut(ByVal userIsLockedOut As Boolean) As CUserList
        Return SelectWhere(new CCriteriaList("UserIsLockedOut", userIsLockedOut))
    End Function

    'Paged
    Public Function SelectByIsDisabled(pi as CPagingInfo, ByVal userIsDisabled As Boolean) As CUserList
        Return SelectWhere(pi, New CCriteriaList("UserIsDisabled", userIsDisabled))
    End Function
    Public Function SelectByIsLockedOut(pi as CPagingInfo, ByVal userIsLockedOut As Boolean) As CUserList
        Return SelectWhere(pi, New CCriteriaList("UserIsLockedOut", userIsLockedOut))
    End Function

    'Count
    Public Function SelectCountByIsDisabled(ByVal userIsDisabled As Boolean) As Integer
        Return SelectCount(New CCriteriaList("UserIsDisabled", userIsDisabled))
    End Function
    Public Function SelectCountByIsLockedOut(ByVal userIsLockedOut As Boolean) As Integer
        Return SelectCount(New CCriteriaList("UserIsLockedOut", userIsLockedOut))
    End Function

    'Transactional
    Public Function SelectByIsDisabled(ByVal userIsDisabled As Boolean, tx As IDbTransaction) As CUserList
        Return SelectWhere(New CCriteriaList("UserIsDisabled", userIsDisabled), tx)
    End Function
    Public Function SelectByIsLockedOut(ByVal userIsLockedOut As Boolean, tx As IDbTransaction) As CUserList
        Return SelectWhere(New CCriteriaList("UserIsLockedOut", userIsLockedOut), tx)
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Autogenerated(ByVal w As System.Xml.XmlWriter)
        Store(w, "UserLoginName", Me.UserLoginName)
        Store(w, "UserPasswordHashedSha1", Me.UserPasswordHashedSha1)
        Store(w, "UserPasswordSalt", Me.UserPasswordSalt)
        Store(w, "UserFirstName", Me.UserFirstName)
        Store(w, "UserLastName", Me.UserLastName)
        Store(w, "UserEmail", Me.UserEmail)
        Store(w, "UserIsDisabled", Me.UserIsDisabled)
        Store(w, "UserCreatedDate", Me.UserCreatedDate)
        Store(w, "UserLastLoginDate", Me.UserLastLoginDate)
        Store(w, "UserLastPasswordChangedDate", Me.UserLastPasswordChangedDate)
        Store(w, "UserPasswordQuestion", Me.UserPasswordQuestion)
        Store(w, "UserPasswordAnswer", Me.UserPasswordAnswer)
        Store(w, "UserFailedPasswordAttemptCount", Me.UserFailedPasswordAttemptCount)
        Store(w, "UserFailedPasswordAttemptStartDate", Me.UserFailedPasswordAttemptStartDate)
        Store(w, "UserIsLockedOut", Me.UserIsLockedOut)
        Store(w, "UserLastLockoutDate", Me.UserLastLockoutDate)
        Store(w, "UserComments", Me.UserComments)
    End Sub
#End Region

#Region "Audit Trail"
    Protected Overrides Function OriginalState(ByVal txOrNull As IDbTransaction) As SchemaAudit.CBaseDynamicAudited
        Return New CUser(Me.DataSrc, Me.UserLoginName, txOrNull)
    End Function
#End Region


End Class