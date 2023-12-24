Partial Public Class pages_Audit_Logs_usercontrols_UCAudit_Logs : Inherits UserControl

#Region "Events"
    Public Event ExportClick()
    Public Event ResortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer)
#End Region

#Region "Interface"
    Public Sub Display(ByVal audit_Logs As CAudit_LogList) 'Single page due to sql-based paging (by exposing Me.Info)
        'Show/Hide Columns
        colNumber.Visible = audit_Logs.Count > 0

        'Display
        For Each i As CAudit_Log In audit_Logs
            UCAudit_Log(plh).Display(i, audit_Logs, Me.PagingInfo)
        Next
    End Sub
    Public ReadOnly Property PagingInfo() As CPagingInfo
        Get
            Return ctrlPaging.Info
        End Get
    End Property
#End Region

#Region "User Controls"
    Private Shared Function UCAudit_Log(ByVal target As Control) As pages_audit_logs_usercontrols_UCAudit_Log
        Dim ctrl As Control = target.Page.LoadControl(CSitemap.UCAudit_Log)
        target.Controls.Add(ctrl)
        Return CType(ctrl, pages_audit_logs_usercontrols_UCAudit_Log)
    End Function
#End Region

#Region "Event Handlers"
    Protected Sub btnResort_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnSortByType.Click, btnSortByLogMessage.Click, btnSortByLogCreated.Click
        'Toggle descending if necessary
        Dim sortBy As String = CType(sender, LinkButton).CommandArgument
        Dim descending As Boolean = ctrlPaging.IsDescending
        Dim currentSort As String = ctrlPaging.SortColumn
        If Not String.IsNullOrEmpty(currentSort) Then
            If currentSort = sortBy Then descending = Not descending
        End If

        'Bubble up as event, search page will add filter info and redirect
        RaiseEvent ResortClick(sortBy, descending, ctrlPaging.Info.PageIndex + 1)
    End Sub
#End Region

#Region "Paging"
    Public ReadOnly Property Info() As CPagingInfo
        Get
            Return ctrlPaging.Info
        End Get
    End Property
    Public Property QueryString() As String
        Get
            Return ctrlPaging.QueryString
        End Get
        Set(ByVal value As String)
            ctrlPaging.QueryString = value
        End Set
    End Property
    Public Property PageSize() As Integer
        Get
            Return ctrlPaging.PageSize
        End Get
        Set(ByVal value As Integer)
            ctrlPaging.PageSize = value
        End Set
    End Property
#End Region

End Class
