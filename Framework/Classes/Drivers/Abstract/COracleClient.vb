Public MustInherit Class COracleClient : Inherits CDataSrcLocal

#Region "Constructors"
    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
    End Sub
#End Region

#Region "Common Methods - Parameter formatting"
    Public Overrides Function Marker(ByVal name As String) As String
        Return ":" & MyBase.Marker(name)
    End Function
#End Region

#Region "List All Tables"
    Public Overrides ReadOnly Property SqlToListAllTables() As String
        Get
            Return "select tname from tab"
            'SELECT table_name FROM user_tables
            'SELECT table_name FROM all_tables
            'SELECT table_name, comments FROM dictionary WHERE table_name LIKE 'user_%' ORDER BY table_name;
        End Get
    End Property
#End Region

#Region "Oracle Inserts"
    Public Overrides Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal txOrNull As System.Data.IDbTransaction, ByVal oracleSequenceName As String) As Object
        'Manual insert of PK has no special behaviour
        If insertPk Then Return MyBase.Insert(tableName, pKeyName, insertPk, data, txOrNull, Nothing)

        'DB-generated PKs: Obtain identity value prior to insert...
        Dim sql As String = "SELECT " & oracleSequenceName & ".NEXTVAL FROM dual"
        Dim newId As Object = ExecuteScalar(sql, txOrNull)

        '...then treat as a manually inserted PK
        data.Add(pKeyName, newId)
        MyBase.Insert(tableName, pKeyName, True, data, txOrNull, Nothing)

        Return newId
    End Function
#End Region

#Region "Oracle Paging"
    'Used by MyBase.Paging (No-Filters version)
    Protected Overrides Function PagingSql(ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal count As Integer) As String
        Dim pageNum As Integer = pageIndexZeroBased + 1

        If descending Then sortByColumn = String.Concat(sortByColumn, " DESC")

        'Short syntax for first page (TOP)
        If pageIndexZeroBased = 0 Then
            Return String.Concat("SELECT * FROM (SELECT ", selectColumns, " FROM ", tableName, " ORDER BY ", sortByColumn, ") WHERE ROWNUM <= ", pageSize)
        End If

        'If on the last page then reduce the pagesize
        Dim pageSizeActual As Integer = pageSize
        If pageNum * pageSize > count Then
            pageSizeActual = count - (pageIndexZeroBased * pageSize)
        End If

        Return String.Concat("SELECT * FROM (SELECT ", selectColumns, " FROM ", tableName, " ORDER BY ", sortByColumn, ") WHERE ROWNUM BETWEEN ", 1 + pageIndexZeroBased * pageSize, " AND ", (1 + pageIndexZeroBased) * pageSize)
    End Function
    Public Overloads Overrides Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As System.Data.IDbTransaction, ByVal type As EQueryReturnType) As Object
        'Count the total records (used by the paging control)
        count = SelectCount(tableName, criteria, txOrNull)

        If descending Then sortByColumn = String.Concat(sortByColumn, " DESC")
        Return String.Concat("SELECT * FROM (SELECT ", selectColumns, " FROM ", tableName, WhereCriteria(criteria), " ORDER BY ", sortByColumn, ") WHERE ROWNUM BETWEEN ", 1 + pageIndexZeroBased * pageSize, " AND ", (1 + pageIndexZeroBased) * pageSize)
    End Function
#End Region

End Class
