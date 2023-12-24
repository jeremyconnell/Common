Imports System.Data

Partial Public Class pages_Audit_Logs_default : Inherits CPage

#region "Querystring (Filters)"
    Public ReadOnly Property Search As String
        Get
            Return CWeb.RequestStr("search")
        End Get
    End Property
    
    'Rename or Delete:
    Public ReadOnly Property TypeId As Integer
        Get
            Return CWeb.RequestInt("typeId")
        End Get
    End Property
#End Region

#Region "Members"
    Private m_audit_Logs As CAudit_LogList
#End Region

#Region "Data"
    Public ReadOnly Property [Audit_Logs]() As CAudit_LogList
        Get
            If IsNothing(m_audit_Logs) Then
                m_audit_Logs = New CAudit_Log().SelectSearch(ctrlAudit_Logs.PagingInfo, txtSearch.Text, TypeId) 'Sql-based Paging
                'm_audit_Logs.PreloadChildren() 'Loads children for page in one hit (where applicable)
            End If
            Return m_audit_Logs
        End Get
    End Property
    Public Function Audit_LogsAsDataset() As System.Data.DataSet
        Return (New CAudit_Log()).SelectSearch_Dataset(txtSearch.Text, TypeId)
    End Function
#End Region

#Region "Overrides"
    Protected Overrides Sub PageInit
        'Populate Dropdowns
        AddMenuSide("All", CSitemap.Audit_Logs, Integer.MinValue = TypeId)
        For Each i As ELogType In [Enum].GetValues(GetType(ELogType))
            AddMenuSide(CAudit_Log.NameAndCount(i), CSitemap.Audit_Logs(String.Empty, i), i = TypeId)
        Next

        'Search state (from querystring)
        txtSearch.Text = Me.Search
        
        'Display Results
        ctrlAudit_Logs.Display(Me.Audit_Logs)

        'Client-side        
        Me.Form.DefaultFocus  = txtSearch.ClientID  'txtCreate.ClientID
        Me.Form.DefaultButton = btnSearch.UniqueID  'CTextbox.OnReturnPress(txtSearch, btnSearch)
    End Sub
#End Region

#Region "Event Handlers - Form"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Response.Redirect(CSitemap.Audit_Logs(txtSearch.Text, TypeId))
    End Sub
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        With New CAudit_Log
            If TypeId = Integer.MinValue Then
                .DeleteAll()
            Else
                .DeleteWhere(New CCriteriaList("LogTypeId", TypeId))
            End If
        End With
        Response.Redirect(Request.RawUrl)
    End Sub
#End Region

#Region "Event Handlers - UserControl"
    Private Sub ctrl_ExportClick() Handles ctrlAudit_Logs.ExportClick
        CDataSrc.ExportToCsv(Audit_LogsAsDataset(), Response, "Audit_Logs.csv")
    End Sub
    Private Sub ctrl_ResortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) Handles ctrlAudit_Logs.ResortClick
        Response.Redirect(CSitemap.Audit_Logs(txtSearch.Text, TypeId, New CPagingInfo(0, pageNumber - 1, sortBy, descending)))
    End Sub
#End Region

End Class
