Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Web.HttpContext
Imports System.Security

Imports Framework


'Table-Row Class (Customisable half)
Partial Public Class CAudit_Trail

#Region "Constants"
#End Region

#Region "Constructors (Public)"
    'Default DataSrc
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal auditId As Integer)
        MyBase.New(auditId)
    End Sub

    'Explicit DataSrc (Overloads)
    Public Sub New(ByVal dataSrc As CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    Public Sub New(ByVal dataSrc As CDataSrc, ByVal auditId As Integer)
        MyBase.New(dataSrc, auditId)
    End Sub

    'Transactional (shares an open connection)
    Public Sub New(ByVal dataSrc As CDataSrc, ByVal auditId As Integer, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, auditId, txOrNull)
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues()
        'Null values
        m_auditId = Integer.MinValue
        m_auditTypeId = Integer.MinValue
        m_auditDataTableName = String.Empty

        'Custom values
        m_auditDate = DateTime.Now
        m_auditUserLoginName = UserLoginName
        m_auditUrl = Url
        m_auditUrlNoQuerystring = UrlNoQuerystring

        'Members
        m_datas = New CAudit_DataList
    End Sub
#End Region

#Region "Default DataSrc"
    Protected Overrides Function DefaultDataSrc() As CDataSrc
        Return CDataSrc.Default
    End Function
#End Region

#Region "Members"
    <NonSerialized()> Private m_differencesWith As CDifferences
    <NonSerialized()> Private m_differencesWithout As CDifferences
    <NonSerialized()> Private m_datas As CAudit_DataList

#End Region

#Region "Properties"
    Public Property [Datas]() As CAudit_DataList
        Get
            If IsNothing(m_datas) Then
                SyncLock (Me)
                    If IsNothing(m_datas) Then
                        m_datas = New CAudit_Data(DataSrc).SelectByTrailId(Me.AuditId)
                        m_datas.Audit_Trail = Me
                    End If
                End SyncLock
            End If
            Return m_datas
        End Get
        Set(ByVal Value As CAudit_DataList)
            m_datas = Value
            If Not IsNothing(Value) Then
                m_datas.Audit_Trail = Me
            End If
        End Set
    End Property
    Friend Function Datas_(ByVal tx As IDbTransaction) As CAudit_DataList
        With New CAudit_Data(DataSrc)
            Return .SelectByTrailId(Me.AuditId, tx)     'Only used for cascade deletes
        End With
    End Function
    Public Function DatasCount() As Integer
        If Not IsNothing(m_datas) Then Return m_datas.Count
        With New CAudit_Data(DataSrc)
            Return .SelectCountByTrailId(Me.AuditId)
        End With
    End Function


    'Derived/ReadOnly (e.g. xml classes, presentation logic)
    Public Function Differences(ByVal includeCurrentState As Boolean) As CDifferences
        If includeCurrentState Then
            If m_differencesWith Is Nothing Then
                m_differencesWith = New CDifferences(Me, True)
            End If
            Return m_differencesWith
        Else
            If m_differencesWithout Is Nothing Then
                m_differencesWithout = New CDifferences(Me, False)
            End If
            Return m_differencesWithout
        End If
    End Function
    Public Shared Function ShortenTableName(ByVal s As String) As String
        Dim i As Integer = s.IndexOf("_")
        If -1 = i Then Return s
        Return s.Substring(i + 1)
    End Function
    Public ReadOnly Property ShorterTableName() As String
        Get
            Return ShortenTableName(AuditDataTableName)
        End Get
    End Property

    'Modified
    Public Property AuditTypeId() As EAuditType
        Get
            Return CType(m_auditTypeId, EAuditType)
        End Get
        Set(ByVal value As EAuditType)
            m_auditTypeId = value
        End Set
    End Property

    'Relationships - Foriegn Keys (e.g parent)

    'Relationships - Collections (e.g. children)
#End Region

#Region "Methods - Database Queries"
    'Non-Paged
    Public Function SelectByAuditTypeId(ByVal auditTypeId As Integer) As CAudit_TrailList
        Return SelectByAuditTypeId(Nothing, auditTypeId)
    End Function

    'Paged
    Public Function SelectByAuditTypeId(ByVal pi As CPagingInfo, ByVal auditTypeId As Integer) As CAudit_TrailList
        Return SelectWhere(pi, "AuditTypeId", ESign.EqualTo, auditTypeId)
    End Function
    Public Function SearchWithPaging(ByVal pi As CPagingInfo, ByVal filters As CAudit_SearchFilters) As IList
        With filters
            Return SearchWithPaging(pi, .TypeId, .Table, .Url, .Login, .SearchDate, .Custom, .PrimaryKey)
        End With
    End Function
    Public Function SearchWithPaging(ByVal pi As CPagingInfo, Optional ByVal typeId As Integer = Integer.MinValue, Optional ByVal table As String = "", Optional ByVal url As String = "", Optional ByVal login As String = "", Optional ByVal searchDate As DateTime = Nothing, Optional ByVal custom As Dictionary(Of String, String) = Nothing, Optional ByVal primaryKey As String = "") As CAudit_TrailList
        Dim criteria As New CCriteriaList
        With criteria
            'Collection of filters with AND logic
            If typeId <> Integer.MinValue Then .Add("AuditTypeId", typeId)
            If table <> String.Empty Then .Add("AuditDataTableName", table)
            If url <> String.Empty Then .Add("AuditUrlNoQuerystring", url)
            If login <> String.Empty Then .Add("AuditUserLoginName", login)
            If primaryKey <> String.Empty Then .Add("AuditDataPrimaryKey", primaryKey)
            If searchDate <> DateTime.MinValue Then
                .Add("AuditDate", ESign.GreaterThan, searchDate.Date.AddMilliseconds(-1))
                .Add("AuditDate", ESign.LessThan, searchDate.Date.AddDays(1).AddMilliseconds(-1))
            End If

            'More complex filter to search xml fields
            If Not IsNothing(custom) AndAlso custom.Count > 0 Then
                Dim sb As New System.Text.StringBuilder("(SELECT DataTrailId FROM ")
                sb.Append(CAudit_Data.TABLE_NAME)
                sb.Append(" WHERE ")
                Dim keys As New List(Of String)(custom.Keys)
                For Each i As String In keys
                    If keys.IndexOf(i) > 0 Then sb.Append(" AND ")
                    sb.Append(" DataName='").Append(i).Append("' AND DataValue='")
                    sb.Append(CAdoData.EscapeWildcardsForLIKE(custom(i), CAdoData.EWildCards.None)).Append("'")
                Next
                sb.Append(")")
                .Add("AuditId", ESign.IN, sb.ToString())
            End If
        End With

        SearchWithPaging = SelectWhere(pi, criteria)
        SearchWithPaging.PreloadDatas()
    End Function

    'Select Distinct
    Public Function SelectDistinctUrls() As List(Of String)
        Return SelectDistinct("AuditUrlNoQuerystring")
    End Function
    Public Function SelectDistinctTables() As List(Of String)
        Return SelectDistinct("AuditDataTableName")
    End Function
    Public Function SelectDistinctUserLoginNames() As List(Of String)
        Return SelectDistinct("AuditUserLoginName")
    End Function
#End Region


    Public Overrides Sub Delete(ByVal txOrNull As IDbTransaction)
        'Use a transaction if none supplied
        If txOrNull Is Nothing Then
            BulkDelete(Me)
            Exit Sub
        End If

        'Cascade-Delete (all child collections)
        Me.Datas_(txOrNull).DeleteAll(txOrNull)

        'Normal Delete
        MyBase.Delete(txOrNull)
    End Sub

#Region "Key Environment Variables"
    Public Shared ReadOnly Property Url() As String
        Get
            If IsNothing(Current) Then Return My.Application.Info.AssemblyName
            Return Current.Request.Url.AbsoluteUri
        End Get
    End Property
    Public Shared ReadOnly Property UrlNoQuerystring() As String
        Get
            If IsNothing(Current) Then Return My.Application.Info.ProductName
            Dim s As String = Url.ToLower
            Dim i As Integer = s.IndexOf("?")
            If i > 0 Then Return s.Substring(0, i)
            Return s
        End Get
    End Property
    Public ReadOnly Property UserLoginName() As String
        Get
            If Current Is Nothing Then
                With My.User
                    If Not .IsAuthenticated Then .InitializeWithWindowsUser()
                    If .IsAuthenticated Then Return .Name Else Return My.Application.Info.ProductName
                End With
            Else
                With Current.User.Identity
                    If .IsAuthenticated Then Return .Name Else Return String.Empty
                End With
            End If
        End Get
    End Property
#End Region

End Class



