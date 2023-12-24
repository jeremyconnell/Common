Imports System.Collections

Partial Class pages_statuss_usercontrols_UCStatus : Inherits UserControl

#Region "Members"
    Private m_status As CStatus
    Private m_sortedList As IList
#End Region

#Region "Interface"
    Public Sub Display(ByVal [status] As CStatus, sortedList as IList)
        If Parent.Controls.Count Mod 2 = 0 Then row.Attributes.Add("class", "alt_row")

        m_status = [status]
        m_sortedList = sortedList

        With m_status
            litNumber.Text = CStr(sortedList.IndexOf(m_status) + 1)
            lnkStatusName.Text =  CStr(IIF(.StatusName.Length = 0, "...", .StatusName))
            lnkStatusName.NavigateUrl = CSitemap.StatusEdit(.StatusId)
            litStatusDescription.Text = .StatusDescription
            lnkClients.Text = CUtilities.CountSummary(.Clients, "client", "none")
            lnkClients.NavigateUrl = CSitemap.Clients(String.Empty, .StatusId)
        End With
    End Sub
#End Region

#Region "Event Handlers"
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
        m_status.Delete()
        Refresh()
    End Sub
#End Region

#Region "Private"
    Private Sub Refresh()
        'CCache.ClearCache()

        'If False Then 'Request.RawUrl.ToLower.Contains("myparent.aspx") Then
        '    'Special case: Parent entity owns the list
        '    Response.Redirect(CSitemap.MyParentEdit(m_status.StatusParentId, MyParent.ETab.statuss, m_pageIndex)
        'Else
        '    'Normal case: Search page owns the list
            Response.Redirect(Request.RawUrl) 'includes paging info
        'End If 
    End Sub
#End Region

End Class
