Imports System
Imports System.Text
Imports System.Data
Imports System.Collections.Generic

Imports Framework
Imports System.Web

#Region "Enum: Primary Key Values"
'<CLSCompliant(True)> _
'Public Enum ESession
'    Huey = 1
'    Duey = 2
'    Louie = 3
'End Enum
#End Region

'Table-Row Class (Customisable half)
Partial Public Class CSession

#Region "Constants"
    'Join Expressions
    'Private Shared JOIN_SAMPLE As String = String.Concat(TABLE_NAME, " LEFT OUTER JOIN ", CSample.TABLE_NAME, " ON SessionSampleId=SampleId")
#End Region

#Region "Constructors (Public)"
    'Default Connection String
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New([sessionId] As Integer)
        MyBase.New([sessionId])
    End Sub
    
    'Alternative Connection String
    Public Sub New(ByVal dataSrc As CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    Public Sub New(ByVal dataSrc As CDataSrc, [sessionId] As Integer)
        MyBase.New(dataSrc, [sessionId])
    End Sub

    'Transactional (shares an open connection)
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, ByVal [sessionId] As Integer, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, [sessionId], txOrNull)
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues_Custom()
        'Custom default values (e.g. DateCreated column)
        m_sessionUserLoginName = CUser.CurrentLogin()

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
    <NonSerialized()> Private m_clicks As CClickList

    'Child Collections  

    'Xml Data (as high-level objects)

#End Region

#Region "Properties - Relationships"    
    'Relationships - Foriegn Keys (e.g parent)
    Public Property [Clicks]() As CClickList
        Get
            If IsNothing(m_clicks) Then
                SyncLock (Me)
                    If IsNothing(m_clicks) Then
                        m_clicks = New CClick(DataSrc).SelectBySessionId(Me.SessionId)
                        m_clicks.Session = Me
                    End If
                End SyncLock
            End If
            Return m_clicks
        End Get
        Set(ByVal Value As CClickList)
            m_clicks = Value
            If Not IsNothing(Value) Then
                m_clicks.Session = Me
            End If
        End Set
    End Property
    Friend Function Clicks_(ByVal tx As IDbTransaction) As CClickList
        With New CClick(DataSrc)
            Return .SelectBySessionId(Me.SessionId, tx)     'Only used for cascade deletes
        End With
    End Function
    'Public Function ClicksCount() As Integer
    '    If Not IsNothing(m_clicks) Then Return m_clicks.Count
    '    With New CClick(DataSrc)
    '        Return .SelectCountBySessionId(Me.SessionId)
    '    End With
    'End Function
    'Public Function ClicksMinDate() As DateTime
    '    If Not IsNothing(m_clicks) Then Return m_clicks(0).ClickDate
    '    With New CClick(DataSrc)
    '        Return CType(.SelectMin("ClickDate", Nothing, New CCriteriaList("ClickSessionId", Me.SessionId)), DateTime)
    '    End With
    'End Function
    'Public Function ClicksMaxDate() As DateTime
    '    If Not IsNothing(m_clicks) Then Return m_clicks(m_clicks.Count - 1).ClickDate
    '    With New CClick(DataSrc)
    '        Return CType(.SelectMax("ClickDate", Nothing, New CCriteriaList("ClickSessionId", Me.SessionId)), DateTime)
    '    End With
    'End Function

    'Relationships - Collections (e.g. children)

#End Region

#Region "Properties - Customisation"
    'Derived/ReadOnly (e.g. xml classes, presentation logic)
    Public ReadOnly Property SessionName() As String
        Get
            Return CUtilities.NameAndCount(String.Concat(CUtilities.LongDateTime(MinDate), " ", CUtilities.Timespan(Timespan)), ClickCount, "click")
        End Get
    End Property
    Public Function Timespan() As TimeSpan
        Return MaxDate.Subtract(MinDate)
    End Function
#End Region

#Region "Save/Delete Overrides"
    Public Overrides Sub Delete(ByVal txOrNull As IDbTransaction)
        'Use a transaction if none supplied
        If txOrNull Is Nothing Then
            BulkDelete(Me)
            Exit Sub
        End If

        'Cascade-Delete (all child collections)
        Me.Clicks_(txOrNull).DeleteAll(txOrNull)

        'Normal Delete
        MyBase.Delete(txOrNull)
    End Sub

#End Region

#Region "Custom Database Queries"
    Public Function UserNamesAndSessionCounts() As CNameValueList
        Return DataSrc.MakeNameValueList(String.Concat("SELECT DISTINCT SessionUserLoginName, COUNT(*) FROM ", VIEW_NAME, " GROUP BY SessionUserLoginName"))
    End Function
    'For Stored Procs can use: MakeListTyped (matching schema), or DataSrc.ExecuteDataset (reports etc)
    'For Dynamic sql, can use: SelectSum, SelectDistinct, SelectCount, SelectWhere (inherited methods)
    '                see also: SelectBy[FK], Search and Count (auto-generated sample queries)
#End Region

#Region "Searching (Optional)"
    'Dynamic search methods: (overload as required for common search patterns, cascade the BuildWhere overloads)
    '   Public  x5 - Simple, Paged, Transactional, Count, and Dataset
    '   Private x1 - BuildWhere
    'See also in-memory search options in list class, such as GetBy[FK] and Search

    'Simple
    Public Function SelectSearch(ByVal nameOrId As String) As CSessionList
        Return SelectWhere(BuildWhere(nameOrId)) ', JOIN_OR_VIEW)
    End Function
'   Public Function SelectSearch(ByVal nameOrId As String, ) As CSessionList
'       Return SelectWhere(BuildWhere(nameOrId)) ', JOIN_OR_VIEW)
'   End Function

    'Paged
    Public Function SelectSearch(ByVal pi As CPagingInfo, ByVal nameOrId As String) As CSessionList
        'pi.TableName = JOIN_OR_VIEW
        Return SelectWhere(pi, BuildWhere(nameOrId))
    End Function
'   Public Function SelectSearch(ByVal pi As CPagingInfo, ByVal nameOrId As String, ) As CSessionList
'       'pi.TableName = JOIN_OR_VIEW
'       Return SelectWhere(pi, BuildWhere(nameOrId))
'   End Function

    'Transactional
    Public Function SelectSearch(ByVal nameOrId As String, ByVal tx As IDbTransaction) As CSessionList
        Return SelectWhere(BuildWhere(nameOrId), tx) ', JOIN_OR_VIEW, tx)
    End Function
'   Public Function SelectSearch(ByVal nameOrId As String, , ByVal tx As IDbTransaction) As CSessionList
'       Return SelectWhere(BuildWhere(nameOrId), tx) ', JOIN_OR_VIEW, tx)
'   End Function

    'Count
    Public Overloads Function SelectCount(ByVal nameOrId As String) As Integer
        Return SelectCount(BuildWhere(nameOrId)) ', JOIN_OR_VIEW)
    End Function
'   Public Overloads Function SelectCount(ByVal nameOrId As String, ) As Integer
'       Return SelectCount(BuildWhere(nameOrId)) ', JOIN_OR_VIEW)
'   End Function

    'Dataset (e.g. ExportToCsv)
    Public Function SelectSearch_Dataset(ByVal nameOrId As String) As DataSet
        Return SelectWhere_Dataset(BuildWhere(nameOrId)) ', JOIN_OR_VIEW)
    End Function
'   Public Function SelectSearch_Dataset(ByVal nameOrId As String, ) As DataSet
'       Return SelectWhere_Dataset(BuildWhere(nameOrId)) ', JOIN_OR_VIEW)
'   End Function
    

    'Filter Logic
    'Represents a simple search box to search PK and any string columns
    Private Function BuildWhere(ByVal userLogin As String) As CCriteriaList
        Dim where As New CCriteriaList 'Defaults to AND logic
        If "*" <> userLogin Then
            If userLogin.Contains("*") Then
                where.Add("SessionUserLoginName", ESign.Like, CAdoData.EscapeWildcardsForLIKE(userLogin))
            Else
                where.Add("SessionUserLoginName", userLogin)
            End If
        End If

        Return where
    End Function
    'Represents more complex combinations of search filters (suggestion only)
'   Private Function BuildWhere(ByVal nameOrId As String, ) As CCriteriaList
'       Dim where As CCriteriaList = BuildWhere(nameOrId) 'Reuses logic above
'
'       'Other search Colums (customise as required)

'       Return where
'   End Function    
#End Region

#Region "Cloning"
    Public Function Clone(target as CDataSrc, txOrNull as IDbTransaction) As CSession ', parentId As Integer) As CSession
        'Shallow copy: Copies the immediate record, excluding autogenerated Pks
        Dim copy As New CSession(Me, target)

        'Deep Copy - Child Entities: Cloned children must reference their cloned parent
        'copy.SampleParentId = parentId

        copy.Save(txOrNull)

        'Deep Copy - Parent Entities: Cloned parents also clone their child collections
        'Me.Children.Clone(target, txOrNull, copy.SessionId)

        Return copy
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Custom(ByVal w As System.Xml.XmlWriter)
        'Store(w, "Example", Me.Example)
    End Sub
#End Region

#Region "Shared"
    Public Shared Property Current() As CSession
        Get
            Dim s As CSession = CType(CSessionBase.Get(TABLE_NAME), CSession)
            If IsNothing(s) Then
                SyncLock (GetType(CSession))
                    s = CType(CSessionBase.Get(TABLE_NAME), CSession)
                    If IsNothing(s) Then
                        s = New CSession
                        CSession.Current = s
                        s.Save()
                    End If
                End SyncLock
            ElseIf s.SessionUserLoginName <> CUser.CurrentLogin Then 'Subsequent login
                s.SessionUserLoginName = CUser.CurrentLogin
                s.Save()
            End If
            Return s
        End Get
        Set(ByVal value As CSession)
            CSessionBase.Set(TABLE_NAME, value)
        End Set
    End Property
#End Region

End Class
