Partial Public Class pages_sessions_usercontrols_UCSession : Inherits UserControl

#Region "Members"
    Private m_session As SchemaMembership.CSession
    Private m_sortedList As CSessionList
    Private m_pageIndex As Integer
#End Region

#Region "Interface"
    Public Sub Display(ByVal [session] As SchemaMembership.CSession, ByVal sortedList As CSessionList, ByVal pi As CPagingInfo)
        If Parent.Controls.Count Mod 2 = 0 Then row.Attributes.Add("class", "alt_row")

        m_session = [session]
        m_sortedList = sortedList
        m_pageIndex = pi.PageIndex

        With m_session
            litNumber.Text = CStr(sortedList.IndexOf(m_session) + 1 + pi.PageIndex * pi.PageSize)
            lnkUserLoginName.Text = CStr(IIf(.SessionUserLoginName.Length = 0, "Anonymous", .SessionUserLoginName))
            lnkUserLoginName.NavigateUrl = CSitemap.Sessions(.SessionUserLoginName)
            lnkMinDate.NavigateUrl = CSitemap.SessionEdit(.SessionId)
            lnkMinDate.Text = CUtilities.LongDateTime(.MinDate)
            lnkMinDate.Text = CUtilities.Timespan(.MinDate)
            lnkTimespan.NavigateUrl = lnkMinDate.NavigateUrl
            lnkClicks.NavigateUrl = lnkMinDate.NavigateUrl
            lnkClicks.Text = CUtilities.CountSummary(.ClickCount, "click")
        End With
    End Sub
#End Region

#Region "Private"
    Private Sub Refresh()
        'CCache.ClearCache()

        'If False Then 'Request.RawUrl.ToLower.Contains("myparent.aspx") Then
        '    'Special case: Parent entity owns the list
        '    Response.Redirect(CSitemap.MyParentEdit(m_session.SessionParentId, MyParent.ETab.sessions, m_pageIndex)
        'Else
        '    'Normal case: Search page owns the list
            Response.Redirect(Request.RawUrl) 'includes paging info
        'End If 
    End Sub
#End Region

End Class
