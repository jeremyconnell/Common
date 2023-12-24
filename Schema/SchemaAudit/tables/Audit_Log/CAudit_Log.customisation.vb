Imports System
Imports System.Text
Imports System.Data
Imports System.Collections.Generic

Imports Framework

#Region "Enum: Primary Key Values"
<CLSCompliant(True)> _
Public Enum ELogType
    Website = 1
    Webservice = 2
    Controller = 3
    Worker = 4
    Upgrade = 5
End Enum
#End Region

'Table-Row Class (Customisable half)
Partial Public Class CAudit_Log

#Region "Constants"
    'Join Expressions
    'Private Shared JOIN_SAMPLE As String = String.Concat(TABLE_NAME, " LEFT OUTER JOIN ", CSample.TABLE_NAME, " ON LogSampleId=SampleId")
#End Region

#Region "Constructors (Public)"
    'Default Connection String
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New([logId] As Integer)
        MyBase.New([logId])
    End Sub
    
    'Alternative Connection String
    Public Sub New(ByVal dataSrc As CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    Public Sub New(ByVal dataSrc As CDataSrc, [logId] As Integer)
        MyBase.New(dataSrc, [logId])
    End Sub

    'Transactional (shares an open connection)
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, ByVal [logId] As Integer, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, [logId], txOrNull)
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues_Custom()
        'Custom default values (e.g. DateCreated column)
        m_logCreated = DateTime.Now
        
        'Member variables (e.g. for child collections)
        
    End Sub
#End Region

#Region "Default Connection String"
    Protected Overrides Function DefaultDataSrc() As CDataSrc
        Return CDataSrc.Default
    End Function
#End Region

#Region "Members"
    'Foreign Keys   

    'Child Collections  

    'Xml Data (as high-level objects)

#End Region

#Region "Properties - Relationships"    
    'Relationships - Foriegn Keys (e.g parent)

    'Relationships - Collections (e.g. children)

#End Region

#Region "Properties - Customisation"
    'Derived/ReadOnly (e.g. xml classes, presentation logic)
    Public Property LogTypeId_ As ELogType
        Get
            Return CType(LogTypeId, ELogType)
        End Get
        Set(value As ELogType)
            LogTypeId = value
        End Set
    End Property
    Public ReadOnly Property LogTypeName As String
        Get
            Return LogTypeId_.ToString
        End Get
    End Property
#End Region

#Region "Save/Delete Overrides"
     'Can Override MyBase.Save/Delete (e.g. Cascade deletes, or insert related records)
#End Region

#Region "Custom Database Queries"
    'For Stored Procs can use: MakeListTyped (matching schema), or DataSrc.ExecuteDataset (reports etc)
    'For Dynamic sql, can use: SelectSum, SelectDistinct, SelectCount, SelectWhere (inherited methods)
    '                see also: SelectBy[FK], Search and Count (auto-generated sample queries)
    Public Function SelectByTypeId_(type As ELogType) As CAudit_LogList
        Return SelectByTypeId(type)
    End Function
#End Region

#Region "Searching (Optional)"
    'Dynamic search methods: (overload as required for common search patterns, cascade the BuildWhere overloads)
    '   Public  x5 - Simple, Paged, Transactional, Count, and Dataset
    '   Private x1 - BuildWhere
    'See also in-memory search options in list class, such as GetBy[FK] and Search

    'Simple
    Public Function SelectSearch(ByVal nameOrId As String, ByVal typeId As Integer) As CAudit_LogList
        Return SelectWhere(BuildWhere(nameOrId, typeId)) ', JOIN_OR_VIEW)
    End Function

    'Paged
    Public Function SelectSearch(ByVal pi As CPagingInfo, ByVal nameOrId As String, ByVal typeId As Integer) As CAudit_LogList
        'pi.TableName = JOIN_OR_VIEW
        Return SelectWhere(pi, BuildWhere(nameOrId, typeId))
    End Function

    'Transactional
    Public Function SelectSearch(ByVal nameOrId As String, ByVal typeId As Integer, ByVal tx As IDbTransaction) As CAudit_LogList
        Return SelectWhere(BuildWhere(nameOrId, typeId), tx) ', JOIN_OR_VIEW, tx)
    End Function

    'Count
    Public Overloads Function SelectCount(ByVal nameOrId As String, ByVal typeId As Integer) As Integer
        Return SelectCount(BuildWhere(nameOrId, typeId)) ', JOIN_OR_VIEW)
    End Function

    'Dataset (e.g. ExportToCsv)
    Public Function SelectSearch_Dataset(ByVal nameOrId As String, ByVal typeId As Integer) As DataSet
        Return SelectWhere_Dataset(BuildWhere(nameOrId, typeId)) ', JOIN_OR_VIEW)
    End Function


    'Filter Logic
    'Represents a simple search box to search PK and any string columns
    Private Function BuildWhere(ByVal nameOrId As String) As CCriteriaList
        Dim where As New CCriteriaList 'Defaults to AND logic

        'Simple search box UI
        If Not String.IsNullOrEmpty(nameOrId) Then
            'Interpret search string in various ways using OR sub-expression
            Dim orExpr As New CCriteriaGroup(EBoolOperator.Or)

            'Special case - search by PK (assumes integer PK)
            Dim id As Integer
            If Integer.TryParse(nameOrId, id) Then
                orExpr.Add("LogId", id)
            End If

            'Search a range of string columns
            Dim wildCards As String = String.Concat("%", nameOrId, "%")
            orExpr.Add("LogMessage", ESign.Like, wildCards)

            'Conclude
            If orExpr.Group.Count > 1 Then
                where.Add(orExpr)
            Else
                where.Add("LogMessage", ESign.Like, wildCards)
            End If
        End If

        Return where
    End Function
    'Represents more complex combinations of search filters (suggestion only)
    Private Function BuildWhere(ByVal nameOrId As String, ByVal typeId As Integer) As CCriteriaList
        Dim where As CCriteriaList = BuildWhere(nameOrId) 'Reuses logic above

        'Other search Colums (customise as required)
        If Integer.MinValue <> typeId Then where.Add("LogTypeId", typeId)

        Return where
    End Function
#End Region

#Region "Cloning"
    Public Function Clone(target As CDataSrc, txOrNull As IDbTransaction) As CAudit_Log ', parentId As Integer) As CAudit_Log
        'Shallow copy: Copies the immediate record, excluding autogenerated Pks
        Dim copy As New CAudit_Log(Me, target)

        'Deep Copy - Child Entities: Cloned children must reference their cloned parent
        'copy.SampleParentId = parentId

        copy.Save(txOrNull)

        'Deep Copy - Parent Entities: Cloned parents also clone their child collections
        'Me.Children.Clone(target, txOrNull, copy.LogId)

        Return copy
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Custom(ByVal w As System.Xml.XmlWriter)
        'Store(w, "Example", Me.Example)
    End Sub
#End Region

#Region "Shared"
    Public Shared Function NameAndCount(type As ELogType) As String
        Return CUtilities.NameAndCount(type.ToString, New CAudit_Log().SelectCountByTypeId(type))
    End Function
    Public Shared Function Log(type As ELogType, message As String) As Integer
        With New CAudit_Log
            .LogTypeId = type
            .LogMessage = message
            .Save()
            Return .LogId
        End With
    End Function
#End Region

End Class
