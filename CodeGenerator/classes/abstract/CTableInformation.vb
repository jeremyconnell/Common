Imports System.IO
Imports System.Collections.Generic
Imports System.Configuration.ConfigurationManager

Public MustInherit Class CTableInformation

#Region "Constants"
    Public Const CLASS_PREFIX As String = "C"
#End Region

#Region "Constructors"
    Public Sub New()
    End Sub
#End Region

#Region "Attributes"
    'Database Connection
    Public Database As CDataSrcLocal

    'Compulsory
    Public ClassName As String
    Public TableName As String
    Public PrimaryKeyType As EPrimaryKeyType
    Public PrimaryKeyName As String
    Public SecondaryKeyName As String
    Public TertiaryKeyName As String
    Public OptionalFilters As String()
    Public Architecture As EArchitecture
    Public Language As ELanguage
    Public Platform As EPlatform
    Public UseCaching As Boolean
    Public UseAuditTrail As Boolean
    Public CSharpNamespace As String
    Public TableNamePrefix As String
    Public StoredProcNamePrefix As String
    Public TemplateFolder As String
    Public TargetFolder As String

    'Optional
    Public ViewName As String
    Public OrderByColumns As String
    Public SortingColumn As String

    'Derived
    Public Function TemplateFolderFragments() As String
        Return TemplateFolderClasses(True) & "fragments/"
    End Function
    Public Function TemplateFolderClasses(ByVal auto As Boolean) As String
        Dim s As String = TemplateFolder & "classes/"
        If auto Then Return s & "auto/"
        s &= "custom/"
        If Me.UseCaching Then s &= "caching/"
        Return s
    End Function
    Public ReadOnly Property TemplateFolderWeb() As String
        Get
            Return TemplateFolder & "web/"
        End Get
    End Property

    Public ReadOnly Property TemplateFolderWebDetails() As String
        Get
            Return TemplateFolderWeb & "details/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderWebDetailsEditable() As String
        Get
            Return TemplateFolderWebDetails & "editable/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderWebDetailsReadOnly() As String
        Get
            Return TemplateFolderWebDetails & "readonly/"
        End Get
    End Property

    Private ReadOnly Property TemplateFolderWebList() As String
        Get
            Return TemplateFolderWeb & "list/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderWebListEditable() As String
        Get
            Return TemplateFolderWebList & "editable/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderWebListReadOnly() As String
        Get
            Return TemplateFolderWebList & "readonly/"
        End Get
    End Property

    Private ReadOnly Property TemplateFolderWebListItem() As String
        Get
            Return TemplateFolderWeb & "listitem/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderWebListItemEditable() As String
        Get
            Return TemplateFolderWebListItem & "editable/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderWebListItemReadOnly() As String
        Get
            Return TemplateFolderWebListItem & "readonly/"
        End Get
    End Property


    Private ReadOnly Property TemplateFolderWebContainer() As String
        Get
            Return TemplateFolderWeb & "container/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderWebContainerEditable() As String
        Get
            Return TemplateFolderWebContainer & "editable/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderWebContainerReadOnly() As String
        Get
            Return TemplateFolderWebContainer & "readonly/"
        End Get
    End Property

    Public ReadOnly Property TemplateFolderWebUrls() As String
        Get
            Return TemplateFolderWeb & "urls/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderWebUrlsEditable() As String
        Get
            Return TemplateFolderWebUrls & "editable/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderWebUrlsReadOnly() As String
        Get
            Return TemplateFolderWebUrls & "readonly/"
        End Get
    End Property

    Public ReadOnly Property TemplateFolderRelationships() As String
        Get
            Return TemplateFolder & "relationships/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderSorting() As String
        Get
            Return TemplateFolder & "sorting/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderStoredProcs() As String
        Get
            Return TemplateFolder & "../../storedprocs/" & Me.Platform.ToString & "/"
        End Get
    End Property
    Public ReadOnly Property TemplateFolderStoredProcFrags() As String
        Get
            Return TemplateFolderStoredProcs & "fragments/"
        End Get
    End Property
    Public ReadOnly Property IsManyToMany() As Boolean
        Get
            Return Me.PrimaryKeyType = EPrimaryKeyType.Many2Many
        End Get
    End Property
    Public ReadOnly Property Is3Way() As Boolean
        Get
            Return Me.PrimaryKeyType = EPrimaryKeyType.ThreeWay
        End Get
    End Property
    Public ReadOnly Property IsAutoNumber() As Boolean
        Get
            Return Me.PrimaryKeyType = EPrimaryKeyType.AutoNumber
        End Get
    End Property
#End Region

#Region "Public - Names of things"
    Public ReadOnly Property EntityName() As String
        Get
            If CLASS_PREFIX.Length > 0 AndAlso ClassName.Length > 0 Then
                Return ClassName.Substring(CLASS_PREFIX.Length)
            End If
            Return ClassName
        End Get
    End Property
    Public ReadOnly Property ClassFileName() As String
        Get
            Select Case Me.PrimaryKeyType
                Case EPrimaryKeyType.AutoNumber, EPrimaryKeyType.Manual
                    Return "Class.txt"
                Case EPrimaryKeyType.Many2Many
                    Return "ClassM2M.txt"
                Case EPrimaryKeyType.ThreeWay
                    Return "Class3Way.txt"
                Case Else
                    Throw New Exception("Unhandled PK type: " & Me.PrimaryKeyType.ToString)
            End Select
        End Get
    End Property
    Public ReadOnly Property ListFileName() As String
        Get
            Select Case Me.PrimaryKeyType
                Case EPrimaryKeyType.AutoNumber, EPrimaryKeyType.Manual
                    Return "List.txt"
                Case EPrimaryKeyType.Many2Many
                    Return "ListM2M.txt"
                Case EPrimaryKeyType.ThreeWay
                    Return "List3Way.txt"
                Case Else
                    Throw New Exception("Unhandled PK type: " & Me.PrimaryKeyType.ToString)
            End Select
        End Get
    End Property
    Public ReadOnly Property FileExtension() As String
        Get
            If Me.Language = ELanguage.VbNet Then
                Return ".vb"
            Else
                Return ".cs"
            End If
        End Get
    End Property
    Public ReadOnly Property FileName(ByVal isRegerated As Boolean, ByVal isList As Boolean) As String
        Get
            Dim sb As New StringBuilder(ClassName)
            If isList Then sb.Append("List")
            If isRegerated Then sb.Append(".regenerated") Else sb.Append(".customisation")
            sb.Append(FileExtension)
            Return sb.ToString
        End Get
    End Property
#End Region

#Region "MustOverride"
    Public MustOverride Function Generate(ByVal overwrite As COverwriteFiles) As Boolean
    Public Function Generate() As Boolean
        Return Generate(GetOverwriteFileSwitches())
    End Function
#End Region

#Region "Private - Main Flow (Overwrite Checks)"
    Public Function GetOverwriteFileSwitches(Optional ByVal safe As Boolean = False) As COverwriteFiles
        Dim overwriteFileSwitches As New COverwriteFiles
        If ClassFileExists() Or safe Then
            'Prompt user for files to over-write
            Dim form As New FormOverwriteFiles(overwriteFileSwitches, Me.Architecture = EArchitecture.StoredProcs)
            Dim result As DialogResult = form.ShowDialog()
            If result = DialogResult.Cancel Then Return Nothing
        Else
            'Create all files
            With overwriteFileSwitches
                .Customizable = True
                .SpDelete = True
            End With
        End If
        Return overwriteFileSwitches
    End Function
    Private Function ClassFileExists() As Boolean
        Dim filePath As String = String.Concat(TargetFolder, "/tables/", EntityName, "/", FileName(True, False))
        Return IO.File.Exists(filePath)
    End Function
#End Region

#Region "Shared"
    Public Function TableTrimmed(ByVal tableName As String) As String
        Return tableName.Replace("[", "").Replace("]", "").Replace("$", "").Replace(".", "_").Replace(" ", "")
    End Function
    Public Function Singular() As String
        Return Singular(TableName)
    End Function
    Public Function Plural() As String
        Return Plural(TableName)
    End Function
    Public Function Singular(ByVal tableName As String) As String
        Dim s As String = TableTrimmed(tableName)

        'Remove prefix
        If TableNamePrefix.Length > 0 Then
            If s.ToLower.StartsWith(TableNamePrefix.ToLower) Then
                s = s.Substring(TableNamePrefix.Length)
            End If
        End If

        'Capatilise first letter
        s = ProperCase(s)

        'Plural=>Singular
        If "ies" = LCase(s.Substring(s.Length - 3)) Then Return s.Substring(0, Len(s) - 3) & "y"
        If "s" = LCase(s.Substring(s.Length - 1)) Then Return s.Substring(0, Len(s) - 1)
        Return s
    End Function
    Public Function Plural(ByVal tableName As String) As String
        Plural = Singular(tableName)
        If "y" = LCase(Plural.Substring(Len(Plural) - 1)) Then Return Plural.Substring(0, Len(Plural) - 1) & "ies"
        If "s" <> LCase(Plural.Substring(Len(Plural) - 1)) Then Return Plural & "s"
    End Function
    Public Shared Function CamelCase(ByVal variable As String) As String
        variable = ProperCase(variable)
        If Len(variable) < 2 Then Return LCase(variable)
        Return LCase(variable.Substring(0, 1)) & variable.Substring(1)
    End Function
    Public Function ShortName(ByVal i As String) As String
        Return CTable.Shorter(i, New List(Of String)(TableColumnNames)).Replace(" ", "")
    End Function
    Public Shared Function ProperCase(ByVal s As String) As String
        If String.IsNullOrEmpty(s) Then Return String.Empty
        s = s.Replace(" ", "")
        Return s.Substring(0, 1).ToUpper & s.Substring(1)
    End Function
    Public Shared Sub WriteFile(ByVal filePath As String, ByVal fileContent As String)
        If File.Exists(filePath) Then File.Delete(filePath)
        If Not Directory.Exists(Path.GetDirectoryName(filePath)) Then Directory.CreateDirectory(Path.GetDirectoryName(filePath))
        Dim fs As FileStream = File.OpenWrite(filePath)
        Dim sw As New StreamWriter(fs)
        sw.Write(fileContent)
        sw.Close()
    End Sub
#End Region

#Region "Events"
    Public ExecuteScripts As Boolean = False
    Public Event RaiseError(ByVal spName As String, ByVal ex As Exception)
    Protected Sub ThrowError(ByVal spName As String, ByVal ex As Exception)
        RaiseEvent RaiseError(spName, ex)
    End Sub
#End Region

#Region "Public  - database"
    Public ReadOnly Property TableColumnNames() As String()
        Get
            If IsNothing(_tableColumnNames) Then SaveTableColumns()
            Return _tableColumnNames
        End Get
    End Property
    Public ReadOnly Property TableColumnTypes() As System.Type()
        Get
            If IsNothing(_tableColumnTypes) Then SaveTableColumns()
            Return _tableColumnTypes
        End Get
    End Property
    Public ReadOnly Property ViewColumnNames() As String()
        Get
            If IsNothing(_viewColumnNames) Then SaveViewColumns()
            Return _viewColumnNames
        End Get
    End Property
    Public ReadOnly Property ViewColumnTypes() As System.Type()
        Get
            If IsNothing(_viewColumnTypes) Then SaveViewColumns()
            Return _viewColumnTypes
        End Get
    End Property
    Public Function GetTypeName(ByVal type As Type) As String
        Return CMainLogic.ShortDataType(type, Me.Language)
    End Function
    Public Function GetSchemaTable() As DataTable
        Dim dr As IDataReader = Nothing
        Try
            dr = GetTable()
            GetSchemaTable = dr.GetSchemaTable()
            dr.Close()
            Exit Function
        Catch ex As Exception
            If Not IsNothing(dr) Then dr.Close()
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected"
    'Obsolete - use TableColumnNames, etc
    Protected Function GetTable() As IDataReader
        Return Database.ExecuteReader("SELECT * FROM " & TableName & " WHERE 1=0")

    End Function
    Protected Function GetView() As IDataReader
        If Len(ViewName) = 0 Then ViewName = TableName
        Return Database.ExecuteReader("SELECT * FROM " & ViewName & " WHERE 1=0")
    End Function
#End Region

#Region "Private  - database"
    Private _tableColumnNames As String()
    Private _tableColumnTypes As System.Type()
    Private _viewColumnNames As String()
    Private _viewColumnTypes As System.Type()
    Private Sub SaveTableColumns()
        Dim dr As IDataReader = GetTable()
        Dim names As New List(Of String)(dr.FieldCount)
        Dim types As New List(Of Type)(dr.FieldCount)
        For i As Integer = 0 To dr.FieldCount - 1
            names.Add(dr.GetName(i))
            types.Add(dr.GetFieldType(i))
        Next
        dr.Close()
        _tableColumnNames = names.ToArray()
        _tableColumnTypes = types.ToArray()
    End Sub
    Private Sub SaveViewColumns()
        Dim dr As IDataReader = GetTable()
        Dim names As New List(Of String)(dr.FieldCount)
        Dim types As New List(Of Type)(dr.FieldCount)
        Dim i As Integer
        For i = 0 To dr.FieldCount - 1
            names.Add(dr.GetName(i))
            types.Add(dr.GetFieldType(i))
        Next
        dr.Close()
        _viewColumnNames = names.ToArray()
        _viewColumnTypes = types.ToArray()
    End Sub
#End Region

End Class

Public Enum ELanguage
    VbNet
    CSharp
End Enum
Public Enum EPlatform 'Used to build path to stored proc templates
    MySql
    SqlServer
    Oracle
    Other 'Stored procs not supported
End Enum
Public Enum EArchitecture
    Dynamic
    Smart
    StoredProcs
End Enum
Public Enum EPrimaryKeyType
    AutoNumber
    Manual
    Many2Many
    ThreeWay
End Enum
