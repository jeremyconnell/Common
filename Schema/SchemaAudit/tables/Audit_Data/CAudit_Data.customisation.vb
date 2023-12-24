Imports System
Imports System.Text
Imports System.Data
Imports System.Collections.Generic

Imports Framework

#Region "Enum: Primary Key Values"
'<CLSCompliant(True)> _
'Public Enum EData
'    Huey = 1
'    Duey = 2
'    Louie = 3
'End Enum
#End Region

'Table-Row Class (Customisable half)
Partial Public Class CAudit_Data

#Region "Constants"
    'Join Expressions
    Public Shared JOIN_AUDIT_TRAIL As String = String.Concat(TABLE_NAME, " INNER JOIN ", CAudit_Trail.TABLE_NAME, " ON DataTrailId=AuditId")
#End Region

#Region "Constructors (Public)"
    'Default Connection String
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New([dataId] As Integer)
        MyBase.New([dataId])
    End Sub
    
    'Alternative Connection String
    Public Sub New(ByVal dataSrc As CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    Public Sub New(ByVal dataSrc As CDataSrc, [dataId] As Integer)
        MyBase.New(dataSrc, [dataId])
    End Sub

    'Transactional (shares an open connection)
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, ByVal [dataId] As Integer, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, [dataId], txOrNull)
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues_Custom()
        'Custom default values (e.g. DateCreated column)
        
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
    <NonSerialized()> Private m_audit_Trail As CAudit_Trail

    'Child Collections  

    'Xml Data (as high-level objects)

#End Region

#Region "Properties - Relationships"    
    'Relationships - Foriegn Keys (e.g parent)
    Public Property [Audit_Trail]() As CAudit_Trail
        Get
            If IsNothing(m_audit_Trail) Then
                SyncLock (Me)
                    If IsNothing(m_audit_Trail) Then
                        m_audit_Trail = New CAudit_Trail(Me.DataTrailId)
                    End If
                End SyncLock
            End If
            Return m_audit_Trail
        End Get
        Set(ByVal Value As CAudit_Trail)
            m_audit_Trail = Value
        End Set
    End Property

    'Relationships - Collections (e.g. children)

#End Region

#Region "Properties - Customisation"
    'Derived/ReadOnly (e.g. xml classes, presentation logic)
    
#End Region

#Region "Save/Delete Overrides"
     'Can Override MyBase.Save/Delete (e.g. Cascade deletes, or insert related records)
#End Region

#Region "Custom Database Queries"
    'For Stored Procs can use: MakeListTyped (matching schema), or DataSrc.ExecuteDataset (reports etc)
    'For Dynamic sql, can use: SelectSum, SelectDistinct, SelectCount, SelectWhere (inherited methods)
    '                see also: SelectBy[FK], Search and Count (auto-generated sample queries)
    Public Function SelectByTrailIds(ByVal trailIds As List(Of Integer)) As CAudit_DataList
        Return SelectWhere(New CCriteriaList("DataTrailId", ESign.IN, trailIds))
    End Function
#End Region

#Region "Searching (Optional)"
    'Dynamic search methods: (overload as required for common search patterns, cascade the BuildWhere overloads)
    '   Public  x5 - Simple, Paged, Transactional, Count, and Dataset
    '   Private x1 - BuildWhere
    'See also in-memory search options in list class, such as GetBy[FK] and Search

    'Simple
    Public Function SelectSearch(ByVal nameOrId As String) As CAudit_DataList
        Return SelectWhere(BuildWhere(nameOrId)) ', JOIN_OR_VIEW)
    End Function
'   Public Function SelectSearch(ByVal nameOrId As String, ByVal trailId As Integer, ByVal isBefore As Boolean?) As CAudit_DataList
'       Return SelectWhere(BuildWhere(nameOrId, trailId, isBefore)) ', JOIN_OR_VIEW)
'   End Function

    'Paged
    Public Function SelectSearch(ByVal pi As CPagingInfo, ByVal nameOrId As String) As CAudit_DataList
        'pi.TableName = JOIN_OR_VIEW
        Return SelectWhere(pi, BuildWhere(nameOrId))
    End Function
'   Public Function SelectSearch(ByVal pi As CPagingInfo, ByVal nameOrId As String, ByVal trailId As Integer, ByVal isBefore As Boolean?) As CAudit_DataList
'       'pi.TableName = JOIN_OR_VIEW
'       Return SelectWhere(pi, BuildWhere(nameOrId, trailId, isBefore))
'   End Function

    'Transactional
    Public Function SelectSearch(ByVal nameOrId As String, ByVal tx As IDbTransaction) As CAudit_DataList
        Return SelectWhere(BuildWhere(nameOrId), tx) ', JOIN_OR_VIEW, tx)
    End Function
'   Public Function SelectSearch(ByVal nameOrId As String, ByVal trailId As Integer, ByVal isBefore As Boolean?, ByVal tx As IDbTransaction) As CAudit_DataList
'       Return SelectWhere(BuildWhere(nameOrId, trailId, isBefore), tx) ', JOIN_OR_VIEW, tx)
'   End Function

    'Count
    Public Overloads Function SelectCount(ByVal nameOrId As String) As Integer
        Return SelectCount(BuildWhere(nameOrId)) ', JOIN_OR_VIEW)
    End Function
'   Public Overloads Function SelectCount(ByVal nameOrId As String, ByVal trailId As Integer, ByVal isBefore As Boolean?) As Integer
'       Return SelectCount(BuildWhere(nameOrId, trailId, isBefore)) ', JOIN_OR_VIEW)
'   End Function

    'Dataset (e.g. ExportToCsv)
    Public Function SelectSearch_Dataset(ByVal nameOrId As String) As DataSet
        Return SelectWhere_Dataset(BuildWhere(nameOrId)) ', JOIN_OR_VIEW)
    End Function
'   Public Function SelectSearch_Dataset(ByVal nameOrId As String, ByVal trailId As Integer, ByVal isBefore As Boolean?) As DataSet
'       Return SelectWhere_Dataset(BuildWhere(nameOrId, trailId, isBefore)) ', JOIN_OR_VIEW)
'   End Function
    

    'Filter Logic
    'Represents a simple search box to search PK and any string columns
    Private Function BuildWhere(ByVal nameOrId As String) As CCriteriaList
        Dim where As New CCriteriaList 'Defaults to AND logic

        'Simple search box UI
        If Not String.IsNullOrEmpty(nameOrId) Then
            'Interpret search string in various ways using OR sub-expression
            Dim orExpr As New CCriteriaGroup(EBoolOperator.Or)

            'Special case - search by PK (assumes integer PK)
            'Dim id As Integer
            'If Integer.TryParse(nameOrId, id)
            '    orExpr.Add("DataId", id)
            'End If

            'Search a range of string columns
            Dim wildCards As String = String.Concat("%", nameOrId, "%")
            orExpr.Add("DataName", ESign.Like, wildCards)

            'Conclude
            If orExpr.Group.Count > 0 Then
                where.Add(orExpr)
            End If
        End If

        Return where
    End Function
    'Represents more complex combinations of search filters (suggestion only)
'   Private Function BuildWhere(ByVal nameOrId As String, ByVal trailId As Integer, ByVal isBefore As Boolean?) As CCriteriaList
'       Dim where As CCriteriaList = BuildWhere(nameOrId) 'Reuses logic above
'
'       'Other search Colums (customise as required)
        'If Integer.MinValue <> trailId Then where.Add("DataTrailId", trailId)
        'If isBefore.HasValue Then where.Add("DataIsBefore", isBefore) 'Customise bool filters according to UI (e.g. for checkbox, use simple bool and bias in one direction)

'       Return where
'   End Function    
#End Region

#Region "Cloning"
    Public Function Clone(target as CDataSrc, txOrNull as IDbTransaction) As CAudit_Data ', parentId As Integer) As CAudit_Data
        'Shallow copy: Copies the immediate record, excluding autogenerated Pks
        Dim copy As New CAudit_Data(Me, target)

        'Deep Copy - Child Entities: Cloned children must reference their cloned parent
        'copy.SampleParentId = parentId

        copy.Save(txOrNull)

        'Deep Copy - Parent Entities: Cloned parents also clone their child collections
        'Me.Children.Clone(target, txOrNull, copy.DataId)

        Return copy
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Custom(ByVal w As System.Xml.XmlWriter)
        'Store(w, "Example", Me.Example)
    End Sub
#End Region

End Class
