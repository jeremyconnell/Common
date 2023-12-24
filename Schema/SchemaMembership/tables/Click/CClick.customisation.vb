Imports System
Imports System.Text
Imports System.Data
Imports System.Collections.Generic
Imports System.Web.HttpContext

Imports Framework

#Region "Enum: Primary Key Values"
'<CLSCompliant(True)> _
'Public Enum EClick
'    Huey = 1
'    Duey = 2
'    Louie = 3
'End Enum
#End Region

'Table-Row Class (Customisable half)
Partial Public Class CClick

#Region "Constants"
    'Join Expressions
    Public Shared JOIN_SESSION As String = String.Concat(TABLE_NAME, " INNER JOIN ", CSession.TABLE_NAME, " ON ClickSessionId=SessionId")

#End Region

#Region "Constructors (Public)"
    'Default Connection String
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New([clickId] As Integer)
        MyBase.New([clickId])
    End Sub
    
    'Alternative Connection String
    Public Sub New(ByVal dataSrc As CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    Public Sub New(ByVal dataSrc As CDataSrc, [clickId] As Integer)
        MyBase.New(dataSrc, [clickId])
    End Sub

    'Transactional (shares an open connection)
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, ByVal [clickId] As Integer, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, [clickId], txOrNull)
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues_Custom()
        'Custom default values (e.g. DateCreated column)
        m_clickDate = DateTime.Now
        If IsNothing(Current) Then Exit Sub
        With Current.Request.Url
            m_clickHost = CUtilities.Truncate(String.Concat(.Scheme, System.Uri.SchemeDelimiter, .Authority), 200)
            m_clickUrl = CUtilities.Truncate(.AbsolutePath, 900)
            m_clickQuerystring = CUtilities.Truncate(.Query, 6000)
        End With
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
    <NonSerialized()> Private m_session As CSession

    'Child Collections  

    'Xml Data (as high-level objects)

#End Region

#Region "Properties - Relationships"    
    'Relationships - Foriegn Keys (e.g parent)
    Public Property [Session]() As CSession
        Get
            If IsNothing(m_session) Then
                SyncLock (Me)
                    If IsNothing(m_session) Then
                        m_session = New CSession(Me.ClickSessionId)
                    End If
                End SyncLock
            End If
            Return m_session
        End Get
        Set(ByVal Value As CSession)
            m_session = Value
        End Set
    End Property

    'Relationships - Collections (e.g. children)

#End Region

#Region "Properties - Customisation"
    'Derived/ReadOnly (e.g. xml classes, presentation logic)
    Public ReadOnly Property FullUrl() As String
        Get
            Return String.Concat(ClickHost, ClickUrl, ClickQuerystring)
        End Get
    End Property
    Public ReadOnly Property NextClick() As CClick
        Get
            Dim list As CClickList = Me.Session.Clicks
            Dim index As Integer = list.IndexOf(Me)
            If index = 0 Then Return Nothing
            Return list(index - 1)
        End Get
    End Property
    Public ReadOnly Property ClickTimeSpan() As TimeSpan
        Get
            If IsNothing(NextClick) Then Return TimeSpan.MinValue
            Return NextClick.ClickDate.Subtract(ClickDate)
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
    Public Function SelectBySessionIds(ByVal sessionIds As List(Of Integer)) As CClickList
        Return SelectWhere(New CCriteriaList("ClickSessionId", ESign.IN, sessionIds))
    End Function
    Public Function UserNamesAndClickCounts() As CNameValueList
        Return DataSrc.MakeNameValueList(String.Concat("SELECT DISTINCT SessionUserLoginName, COUNT(*) FROM ", VIEW_NAME, " GROUP BY SessionUserLoginName"))
    End Function
    Public Function UrlsAndClickCounts() As CNameValueList
        Return DataSrc.MakeNameValueList(String.Concat("SELECT DISTINCT ClickUrl, COUNT(*) FROM ", TABLE_NAME, " GROUP BY ClickUrl ORDER BY ClickUrl"))
    End Function
#End Region

#Region "Searching (Optional)"
    'Dynamic search methods: (overload as required for common search patterns, cascade the BuildWhere overloads)
    '   Public  x5 - Simple, Paged, Transactional, Count, and Dataset
    '   Private x1 - BuildWhere
    'See also in-memory search options in list class, such as GetBy[FK] and Search

    'Simple
    Public Function SelectSearch(ByVal userName As String, ByVal url As String) As CClickList
        Return SelectWhere(BuildWhere(userName, url)) ', JOIN_OR_VIEW)
    End Function
    '   Public Function SelectSearch(ByVal nameOrId As String, ByVal sessionId As Integer) As CClickList
'       Return SelectWhere(BuildWhere(nameOrId, sessionId)) ', JOIN_OR_VIEW)
'   End Function

    'Paged
    Public Function SelectSearch(ByVal pi As CPagingInfo, ByVal userName As String, ByVal url As String) As CClickList
        'pi.TableName = JOIN_OR_VIEW
        Return SelectWhere(pi, BuildWhere(userName, url))
    End Function
'   Public Function SelectSearch(ByVal pi As CPagingInfo, ByVal nameOrId As String, ByVal sessionId As Integer) As CClickList
'       'pi.TableName = JOIN_OR_VIEW
'       Return SelectWhere(pi, BuildWhere(nameOrId, sessionId))
'   End Function

    'Transactional
    Public Function SelectSearch(ByVal userName As String, ByVal url As String, ByVal tx As IDbTransaction) As CClickList
        Return SelectWhere(BuildWhere(userName, url), tx) ', JOIN_OR_VIEW, tx)
    End Function
'   Public Function SelectSearch(ByVal nameOrId As String, ByVal sessionId As Integer, ByVal tx As IDbTransaction) As CClickList
'       Return SelectWhere(BuildWhere(nameOrId, sessionId), tx) ', JOIN_OR_VIEW, tx)
'   End Function

    'Count
    Public Overloads Function SelectCount(ByVal userName As String, ByVal url As String) As Integer
        Return SelectCount(BuildWhere(userName, url)) ', JOIN_OR_VIEW)
    End Function
    '   Public Overloads Function SelectCount(ByVal userName As String, ByVal url As String, ByVal sessionId As Integer) As Integer
'       Return SelectCount(BuildWhere(nameOrId, sessionId)) ', JOIN_OR_VIEW)
'   End Function

    'Dataset (e.g. ExportToCsv)
    Public Function SelectSearch_Dataset(ByVal userName As String, ByVal url As String) As DataSet
        Return SelectWhere_Dataset(BuildWhere(userName, url)) ', JOIN_OR_VIEW)
    End Function
    '   Public Function SelectSearch_Dataset(ByVal userName As String, ByVal url As String, ByVal sessionId As Integer) As DataSet
'       Return SelectWhere_Dataset(BuildWhere(nameOrId, sessionId)) ', JOIN_OR_VIEW)
'   End Function
    

    'Filter Logic
    'Represents a simple search box to search PK and any string columns
    Private Function BuildWhere(ByVal userLogin As String, ByVal url As String) As CCriteriaList
        Dim where As New CCriteriaList 'Defaults to AND logic

        If "*" <> userLogin Then
            If userLogin.Contains("*") Then
                where.Add("SessionUserLoginName", ESign.Like, CAdoData.EscapeWildcardsForLIKE(userLogin))
            Else
                where.Add("SessionUserLoginName", userLogin)
            End If
        End If

        If Not String.IsNullOrEmpty(url) Then
            where.Add("ClickUrl", url)
        End If

        Return where
    End Function
    'Represents more complex combinations of search filters (suggestion only)
    '   Private Function BuildWhere(ByVal userName As String, ByVal url As String, ByVal sessionId As Integer) As CCriteriaList
'       Dim where As CCriteriaList = BuildWhere(nameOrId) 'Reuses logic above
'
'       'Other search Colums (customise as required)
        'If Integer.MinValue <> sessionId Then where.Add("ClickSessionId", sessionId)

'       Return where
'   End Function    
#End Region

#Region "Cloning"
    Public Function Clone(target as CDataSrc, txOrNull as IDbTransaction) As CClick ', parentId As Integer) As CClick
        'Shallow copy: Copies the immediate record, excluding autogenerated Pks
        Dim copy As New CClick(Me, target)

        'Deep Copy - Child Entities: Cloned children must reference their cloned parent
        'copy.SampleParentId = parentId

        copy.Save(txOrNull)

        'Deep Copy - Parent Entities: Cloned parents also clone their child collections
        'Me.Children.Clone(target, txOrNull, copy.ClickId)

        Return copy
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Custom(ByVal w As System.Xml.XmlWriter)
        'Store(w, "Example", Me.Example)
    End Sub
#End Region

#Region "Shared"
    Public Shared Function Log() As Exception
        Try
            With New CClick
                .ClickSessionId = CSession.Current.SessionId
                .Save()
            End With
        Catch ex As Exception
            CSession.Current = Nothing 'sometimes get the error when you delete stuff from the db, or change db
            Return ex
        End Try
        Return Nothing
    End Function
#End Region

End Class
