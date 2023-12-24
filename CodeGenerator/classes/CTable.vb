Public Class CTable

#Region "Constructors"
    Public Sub New(ByVal classFilePath As String, ByVal dataSrc As CDataSrcLocal)
        m_dataSrc = dataSrc

        'Concantate files
        Dim classFile As String = IO.File.ReadAllText(classFilePath)

        'Derive information
        m_tableName = GetLiteralForConstant(classFile, "TABLE_NAME")
        m_className = GetClassName(classFile)
        m_folderName = IO.Path.GetFileName(IO.Path.GetDirectoryName(classFilePath))

        m_primaryKeyName = GetLiteralForConstant(classFile, "PRIMARY_KEY_NAME")
        If String.IsNullOrEmpty(m_primaryKeyName) Then
            m_primaryKeyName = GetLiteralForConstant(classFile, "PrimaryKeyName")
        End If
        m_secondaryKeyName = GetLiteralForConstant(classFile, "SecondaryKeyName")
        m_tertiaryKeyName = GetLiteralForConstant(classFile, "TertiaryKeyName")
        m_insertPrimaryKey = GetInsertPK(classFile)

        m_useCaching = classFile.Contains("CACHE_KEY")
        m_auditTrail = classFile.Contains("CBaseDynamicAudited") And classFile.Contains("OriginalState")
        m_orderBy = GetLiteralForConstant(classFile, "ORDER_BY_COLS")
        m_viewName = GetLiteralForConstant(classFile, "VIEW_NAME")
        m_sortingColumn = GetLiteralForConstant(classFile, "SORTING_COLUMN")
    End Sub
#End Region

#Region "Members"
    Private m_tableName As String
    Private m_className As String
    Private m_folderName As String

    Private m_primaryKeyName As String
    Private m_secondaryKeyName As String
    Private m_tertiaryKeyName As String
    Private m_insertPrimaryKey As Boolean

    Private m_useCaching As Boolean
    Private m_auditTrail As Boolean
    Private m_orderBy As String
    Private m_viewName As String
    Private m_sortingColumn As String

    'Column data
    Private m_dataSrc As CDataSrcLocal
    Private m_columnNames As String()
    Private m_columnTypes As Type()
#End Region

#Region "Properties"
    Public ReadOnly Property TableName() As String
        Get
            Return m_tableName
        End Get
    End Property
    Public ReadOnly Property ClassName() As String
        Get
            Return m_className
        End Get
    End Property
    Public ReadOnly Property FolderName() As String
        Get
            Return m_folderName
        End Get
    End Property

    Public ReadOnly Property PrimaryKeyName() As String
        Get
            Return m_primaryKeyName
        End Get
    End Property
    Public ReadOnly Property SecondaryKeyName() As String
        Get
            Return m_secondaryKeyName
        End Get
    End Property
    Public ReadOnly Property TertiaryKeyName() As String
        Get
            Return m_tertiaryKeyName
        End Get
    End Property
    Public ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return m_insertPrimaryKey
        End Get
    End Property

    Public ReadOnly Property UseCaching() As Boolean
        Get
            Return m_useCaching
        End Get
    End Property
    Public ReadOnly Property AuditTrail() As Boolean
        Get
            Return m_auditTrail
        End Get
    End Property
    Public ReadOnly Property OrderBy() As String
        Get
            Return m_orderBy
        End Get
    End Property
    Public ReadOnly Property ViewName() As String
        Get
            Return m_viewName
        End Get
    End Property
    Public ReadOnly Property SortingColumn() As String
        Get
            Return m_sortingColumn
        End Get
    End Property


    'Derived
    Public ReadOnly Property AutoPk() As Boolean
        Get
            Return Not InsertPrimaryKey
        End Get
    End Property
    Public ReadOnly Property Keys() As String
        Get
            Dim prefix As String = String.Empty
            Dim sb As New StringBuilder(PrimaryKeyName)
            If Not String.IsNullOrEmpty(SecondaryKeyName) Then
                sb.Append(", ").Append(SecondaryKeyName)
                prefix = "(M2M) "
            End If
            If Not String.IsNullOrEmpty(TertiaryKeyName) Then
                sb.Append(", ").Append(TertiaryKeyName)
                prefix = "(3Way) "
            End If
            sb.Insert(0, prefix)
            Return sb.ToString()
        End Get
    End Property
    Public ReadOnly Property TableAndClass() As String
        Get
            Return String.Concat(ClassName, " (", TableName, ")")
            'Return String.Concat(TableName, " (", ClassName, ")")
        End Get
    End Property
    Public ReadOnly Property TableAndPK() As String
        Get
            Return String.Concat(TableName, " (", PrimaryKeyName, ")")
        End Get
    End Property
    Public Function ColumnNames() As List(Of String)
        If IsNothing(m_columnNames) Then SaveTableColumns()
        Return New List(Of String)(m_columnNames)
    End Function
    Public Function ColumnTypes() As List(Of Type)
        If IsNothing(m_columnTypes) Then SaveTableColumns()
        Return New List(Of Type)(m_columnTypes)
    End Function
    Public ReadOnly Property Singular() As String
        Get
            If ClassName.StartsWith("C") Then Return ClassName.Substring(1)
            Return ClassName
        End Get
    End Property
    Public ReadOnly Property Plural() As String
        Get
            Return String.Concat(Singular & "s")
        End Get
    End Property
    Public ReadOnly Property PluralCamelCase() As String
        Get
            Return CTableInformation.CamelCase(Plural)
        End Get
    End Property
    Public ReadOnly Property SingularCamelCase() As String
        Get
            Return CTableInformation.CamelCase(Singular)
        End Get
    End Property
    Public ReadOnly Property PrimaryKeyCamelCase() As String
        Get
            Return CTableInformation.CamelCase(PrimaryKeyName)
        End Get
    End Property
    Public ReadOnly Property SecondaryKeyCamelCase() As String
        Get
            Return CTableInformation.CamelCase(SecondaryKeyName)
        End Get
    End Property
    Public ReadOnly Property PrimaryKeyType() As Type
        Get
            Return GetColumnType(PrimaryKeyName)
        End Get
    End Property
    Public ReadOnly Property SecondaryKeyType() As Type
        Get
            Return GetColumnType(SecondaryKeyName)
        End Get
    End Property
    Public Function PrimaryKeyTypeName(ByVal language As ELanguage) As String
        Return CMainLogic.ShortDataType(PrimaryKeyType, language)
    End Function
    Public Function SecondaryKeyTypeName(ByVal language As ELanguage) As String
        Return CMainLogic.ShortDataType(SecondaryKeyType, language)
    End Function
    Public ReadOnly Property IsAssociative() As Boolean
        Get
            Return Not String.IsNullOrEmpty(Me.SecondaryKeyName)
        End Get
    End Property
    Public ReadOnly Property Is3Way() As Boolean
        Get
            Return Not String.IsNullOrEmpty(Me.TertiaryKeyName)
        End Get
    End Property
#End Region

#Region "Private - String parsing"
    Private Function GetClassName(ByVal content As String) As String
        content = content.Replace(vbCr, " ").Replace(vbLf, " ")
        Dim startAt As Integer = content.ToLower.IndexOf(" class ") + 7
        Dim length As Integer = content.IndexOf(" ", startAt) - startAt
        Return content.Substring(startAt, length)
    End Function
    Private Function GetInsertPK(ByVal content As String) As Boolean
        Dim startAt As Integer = content.IndexOf("InsertPrimaryKey")
        If startAt = -1 Then Return False
        With content.ToLower
            startAt = .IndexOf("return ", startAt) + 7
            If startAt = 6 Then Return False
            Return "true" = .Substring(startAt, 4)
        End With
    End Function
    Private Function GetLiteralForConstant(ByVal content As String, ByVal constant As String) As String
        constant = String.Concat(" ", constant)
        Dim i As Integer = content.IndexOf(constant)
        If i = -1 Then Return String.Empty
        Dim startAt As Integer = content.IndexOf("""", i) + 1
        Dim length As Integer = content.IndexOf("""", startAt) - startAt
        Return content.Substring(startAt, length)
    End Function
#End Region

#Region "Private - SaveTableColumns"
    Private Sub SaveTableColumns()
        Try
            Dim dr As IDataReader = GetDataReader()
            Dim names As New List(Of String)(dr.FieldCount)
            Dim types As New List(Of Type)(dr.FieldCount)
            For i As Integer = 0 To dr.FieldCount - 1
                names.Add(dr.GetName(i))
                types.Add(dr.GetFieldType(i))
            Next
            dr.Close()
            m_columnNames = names.ToArray()
            m_columnTypes = types.ToArray()
        Catch ex As Exception
            m_columnNames = New String() {}
            m_columnTypes = New Type() {}
        End Try
    End Sub
    Protected Function GetDataReader() As IDataReader
        If m_dataSrc.IsMySql Then Return m_dataSrc.ExecuteReader("SELECT * FROM " & TableName)
        Return m_dataSrc.ExecuteReader("SELECT * FROM [" & TableName & "] WHERE 1=0")
    End Function
#End Region

#Region "GetType"
    Private Function GetColumnIndex(ByVal columnName As String) As Integer
        columnName = columnName.ToLower
        For i As Integer = 0 To ColumnNames.Count - 1
            If ColumnNames(i).ToLower.Equals(columnName) Then Return i
        Next
        Return -1
    End Function
    Public Function GetColumnType(ByVal columnName As String) As Type
        Return ColumnTypes(GetColumnIndex(columnName))
    End Function
    Public Function GetShortName(ByVal columnName As String) As String
        Return Shorter(columnName, Me.ColumnNames)
    End Function
#End Region

#Region "Shared"
    Public Shared Function Shorter(ByVal colName As String, ByVal allNames As List(Of String)) As String
        If String.IsNullOrEmpty(colName) Then Return String.Empty
        Dim prefix As String = GetPrefix(allNames)
        If prefix.Length >= colName.Length Then Return colName
        If prefix.ToLower <> colName.Substring(0, prefix.Length).ToLower Then Return colName

        Dim name As String = colName.Substring(prefix.Length)
        If name.ToLower.StartsWith("date") AndAlso name.Length > 4 Then name = name.Substring(4)
        Return name
    End Function
    Public Function GetPrefix() As String
        Return GetPrefix(ColumnNames)
    End Function
    Public Shared Function GetPrefix(ByVal allNames As List(Of String)) As String
        Return GetPrefix(allNames, String.Empty)
    End Function
    Private Shared Function GetPrefix(ByVal allNames As List(Of String), ByVal prefix As String) As String
        Dim temp As String = prefix
        If allNames.Count = 0 Then Return prefix
        If allNames.Count = 1 Then Return prefix
        Dim firstOne As String = allNames(0)
        If temp.Length > firstOne.Length Then Return prefix
        temp = firstOne.Substring(0, temp.Length + 1)
        For Each i As String In allNames
            If Not i.StartsWith(temp) Then Return prefix
        Next
        Return GetPrefix(allNames, temp)
    End Function
#End Region

End Class
