Partial Public Class pages_Audit_Errors_usercontrols_UCAudit_Errors : Inherits UserControl

#Region "Events"
    Public Event ExportClick()
    Public Event SortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer)
#End Region

#Region "Interface"
    Public Sub Display(ByVal audit_Errors As CAudit_ErrorList, ByVal isUnique As Boolean) 'Short list due to sql-based paging (by exposing Me.Info)
        colNumber.Visible = audit_Errors.Count > 0
        For Each i As CAudit_Error In audit_Errors
            UCAudit_Error(plh).Display(i, audit_Errors, Me.PagingInfo, isUnique)
        Next
        If isUnique Then
            btnSortByDateCreated.Text = "Last Error Date"
            btnSortByErrorId.Text = "Count"
        End If
    End Sub
    Public ReadOnly Property PagingInfo() As CPagingInfo
        Get
            Return ctrlPaging.Info
        End Get
    End Property
#End Region

#Region "User Controls"
    Private Shared Function UCAudit_Error(ByVal target As Control) As pages_audit_errors_usercontrols_UCAudit_Error
        Dim ctrl As Control = target.Page.LoadControl(CSitemap.UCError)
        target.Controls.Add(ctrl)
        Return CType(ctrl, pages_audit_errors_usercontrols_UCAudit_Error)
    End Function
#End Region

#Region "Event Handlers"
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExport.Click
        RaiseEvent ExportClick()
    End Sub
    Protected Sub btnSort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSortByErrorId.Click, btnSortByUserName.Click, btnSortByUserName.Click, btnSortByWebsite.Click, btnSortByMachineName.Click, btnSortByType.Click, btnSortByMessage.Click, btnSortByInnerType.Click, btnSortByInnerMessage.Click, btnSortByDateCreated.Click
        'Toggle descending if necessary
        Dim sortBy As String = CType(sender, LinkButton).CommandArgument
        Dim descending As Boolean = ctrlPaging.IsDescending
        Dim currentSort As String = ctrlPaging.SortColumn
        If Not String.IsNullOrEmpty(currentSort) Then
            If currentSort = sortBy Then descending = Not descending
        End If

        'Bubble up as event, search page will add filter info and redirect
        RaiseEvent SortClick(sortBy, descending, ctrlPaging.Info.PageIndex + 1)
    End Sub
#End Region

End Class
