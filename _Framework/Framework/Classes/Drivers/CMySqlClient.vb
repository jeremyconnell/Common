Imports MySql.Data.MySqlClient

<Serializable()> _
Public Class CMySqlClient : Inherits CDataSrcLocal


#Region "Constructors"
    Public Sub New(ByVal server As String, ByVal database As String, ByVal user As String, ByVal password As String, Optional ByVal port As Integer = 3306)
        MyBase.New(BuildConnectionString(server, database, user, password, port))
    End Sub
    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
    End Sub
#End Region

#Region "MustOverride"
    Public Overrides Function Connection() As IDbConnection
        Connection = New MySqlConnection(m_connectionString)
        Connection.Open()
    End Function
    Protected Friend Overloads Overrides Function CommandFactory(ByVal con As IDbConnection) As IDbCommand
        Return New MySqlCommand(String.Empty, CType(con, MySqlConnection))
    End Function
    Public Overloads Overrides Function Command(ByVal sql As String) As IDbCommand
        Dim s As String = LCase(Trim(sql))

        If s.StartsWith("select") Then
            Const TOP As String = " top "
            If s.Contains(TOP) Then
                'Replace "SELECT TOP 50 ..." with "SELECT ... LIMIT 50"
                Dim i As Integer = s.IndexOf(TOP)
                Dim before As String = sql.Substring(0, i)
                Dim after As String = sql.Substring(i + TOP.Length)
                i = after.IndexOf(" ")
                Dim limit As Integer = Integer.Parse(Trim(after.Substring(0, i)))
                after = Trim(after.Substring(i))
                If after.EndsWith(";") Then after = after.Substring(0, after.Length - 1)
                sql = String.Concat(before, " ", after, " LIMIT ", limit, ";")
            End If
        End If

        Return MyBase.Command(s)
    End Function
    Public Overrides Function DataAdapter(ByVal cmd As IDbCommand) As IDataAdapter
        Return New MySqlDataAdapter(CType(cmd, MySqlCommand))
    End Function
    Protected Overrides Sub DeriveParameters(ByVal cmd As IDbCommand)
        MySqlCommandBuilder.DeriveParameters(CType(cmd, MySqlCommand))
    End Sub
    Public Overrides Function Parameter(ByVal name As String, ByVal value As Object) As IDbDataParameter
        Dim p As New MySqlParameter(ParameterName(name), NullValue(value))
        SetParameterType(p, value)

        'Mysql hack
        If TypeOf value Is String Then p.Size = CStr(value).Length

        Return p
    End Function
    Public Overrides Function Marker(ByVal name As String) As String
        Return "?" & MyBase.Marker(name)
    End Function
    Public Overrides Function ParameterName(ByVal name As String) As String
        Return Marker(name)
    End Function
#End Region

#Region "Platform-Specific"
    Public Overrides ReadOnly Property SqlToListAllTables() As String
        Get
            Return "SHOW FULL TABLES WHERE Table_type='BASE TABLE'"
        End Get
    End Property
    Protected Overrides Function PagingSql(ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal count As Integer) As String
        Dim pageNum As Integer = pageIndexZeroBased + 1
        If descending Then sortByColumn = String.Concat(sortByColumn, " DESC")

        'Short syntax for first page (TOP)
        If pageIndexZeroBased = 0 Then
            Return String.Concat("SELECT ", selectColumns, " FROM ", tableName, " ORDER BY ", sortByColumn, " LIMIT ", pageSize)
        End If

        'If on the last page then reduce the pagesize
        Dim pageSizeActual As Integer = pageSize
        If pageNum * pageSize > count Then
            pageSizeActual = count - (pageIndexZeroBased * pageSize)
        End If

        Return String.Concat("SELECT ", selectColumns, " FROM ", tableName, " ORDER BY ", sortByColumn, " LIMIT ", pageIndexZeroBased * pageSize, ",", pageSize)
    End Function
    Public Overloads Overrides Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As System.Data.IDbTransaction, ByVal type As EQueryReturnType) As Object
        'Count the total records (used by the paging control)
        count = SelectCount(tableName, criteria, txOrNull)

        If descending Then sortByColumn = String.Concat(sortByColumn, " DESC")
        Return String.Concat("SELECT ", selectColumns, " FROM ", tableName, WhereCriteria(criteria), " ORDER BY ", sortByColumn, " LIMIT ", pageIndexZeroBased * pageSize, ",", pageSize)
    End Function
#End Region

#Region "Shared"
    Public Shared Function BuildConnectionString(ByVal server As String, ByVal database As String, ByVal user As String, ByVal password As String, Optional ByVal port As Integer = 3306) As String
        Dim sb As New System.Text.StringBuilder()
        sb.Append("Server=")
        sb.Append(server)
        sb.Append(";")
        If port <> 3306 Then
            sb.Append("Port=")
            sb.Append(port)
            sb.Append(";")
        End If
        If Not String.IsNullOrEmpty(database) Then
            sb.Append("Database=")
            sb.Append(database)
            sb.Append(";")
        End If
        sb.Append("Uid=")
        sb.Append(user)
        sb.Append(";")
        sb.Append("Pwd=")
        sb.Append(password)
        sb.Append(";")
        sb.Append("Allow Zero Datetime=true")
        Return sb.ToString
    End Function
    Public Shared Function GetDate(ByVal obj As Object, ByVal defaultValue As DateTime) As DateTime
        If TypeOf obj Is MySql.Data.Types.MySqlDateTime Then
            With CType(obj, MySql.Data.Types.MySqlDateTime)
                If Not .IsValidDateTime OrElse .IsNull Then Return defaultValue
                Return New DateTime(.Year, .Month, .Day, .Hour, .Minute, .Second, .Millisecond)
            End With
        End If
        Return CDate(obj)
    End Function
#End Region

End Class
