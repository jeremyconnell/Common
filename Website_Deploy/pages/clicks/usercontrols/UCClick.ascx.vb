Imports SchemaMembership

Partial Public Class pages_clicks_usercontrols_UCClick : Inherits UserControl

#Region "Members"
    Private m_click As CClick
    Private m_sortedList As CClickList
    Private m_pageIndex As Integer
#End Region

#Region "Interface"
    Public Sub Display(ByVal [click] As CClick, ByVal sortedList As CClickList, ByVal pi As CPagingInfo, ByVal showUser As Boolean, ByVal showHost As Boolean)
        If Parent.Controls.Count Mod 2 = 0 Then row.Attributes.Add("class", "alt_row")

        m_click = [click]
        m_sortedList = sortedList
        m_pageIndex = pi.PageIndex

        colUser.Visible = showUser
        colHost.Visible = showHost
        colTime.Visible = Not showUser

        With m_click
            litNumber.Text = CStr(sortedList.IndexOf(m_click) + 1 + pi.PageIndex * pi.PageSize)
            litHost.Text = .ClickHost
            lnkUrl.Text = .ClickUrl
            lnkUrl.NavigateUrl = CSitemap.Clicks("*", .ClickUrl)
            lnkClickDate.ToolTip = CUtilities.LongDateTime(.ClickDate)
            lnkClickDate.Text = CUtilities.Timespan(.ClickDate)
            lnkClickDate.NavigateUrl = .FullUrl
            If showUser Then
                lnkUsers.Text = IIf(Len(.SessionUserLoginName) > 0, .SessionUserLoginName, "Anonymous")
                lnkUsers.NavigateUrl = CSitemap.Clicks(.SessionUserLoginName)
            Else
                lblTime.Text = CUtilities.TimespanShort(.ClickTimeSpan)
                lblTime.ToolTip = CUtilities.Timespan(.ClickTimeSpan)
            End If
            If Not String.IsNullOrEmpty(.ClickQuerystring) Then
                Dim pairs As String() = .ClickQuerystring.Substring(1).Split("&")
                For Each i As String In pairs
                    Dim row As New HtmlTableRow()
                    tbl.Rows.Add(row)
                    Dim ss As String() = i.Split(CChar("="))

                    Dim cell As New HtmlTableCell()
                    cell.InnerText = ss(0)
                    cell.Style.Add("font-weight", "bold")
                    row.Cells.Add(cell)

                    cell = New HtmlTableCell
                    cell.InnerHtml = "&nbsp; = &nbsp;"
                    row.Cells.Add(cell)

                    cell = New HtmlTableCell
                    cell.InnerText = Server.UrlDecode(ss(1))
                    row.Cells.Add(cell)
                Next
            End If
        End With
    End Sub
#End Region

#Region "Private"
    Private Sub Refresh()
        'CCache.ClearCache()

        'If False Then 'Request.RawUrl.ToLower.Contains("myparent.aspx") Then
        '    'Special case: Parent entity owns the list
        '    Response.Redirect(CSitemap.MyParentEdit(m_click.ClickParentId, MyParent.ETab.clicks, m_pageIndex)
        'Else
        '    'Normal case: Search page owns the list
        Response.Redirect(Request.RawUrl) 'includes paging info
        'End If 
    End Sub
#End Region

End Class
