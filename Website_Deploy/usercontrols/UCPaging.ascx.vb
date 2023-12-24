Imports Framework

Partial Class usercontrols_UCPaging
    Inherits System.Web.UI.UserControl

#Region "Constants"
    Private Const DEFAULT_HASH As String = "" 'Anchor-bookmark for the paging links
    Private Const DEFAULT_QUERYSTRING_PAGING As String = "p"
    Private Const DEFAULT_QUERYSTRING_SORTBY As String = "sortBy"
    Private Const DEFAULT_QUERYSTRING_DESCENDING As String = "desc"
    Private Const DEFAULT_QUERYSTRING_PAGE_SIZE As String = "pageSize"

    Private Const DEFAULT_PAGE_SIZE As Integer = 10
    Private Const DEFAULT_OUTER_BLOCK_SIZE As Integer = 4
    Private Const DEFAULT_CURRENT_PAGE_PADDING As Integer = 2
    Private Const DEFAULT_MAX_PAGES As Integer = 10
    Private Const DEFAULT_SEPARATOR As String = "..."

    Private Const CSS_SELECTED_LINK As String = "page selected"
    Private Const CSS_SEPARATOR As String = "pageSeparator"
#End Region

#Region "Querystring"
    Public ReadOnly Property CurrentPageZeroBased() As Integer
        Get
            Return CurrentPage - 1
        End Get
    End Property
    Public ReadOnly Property CurrentPage() As Integer
        Get
            Return CWeb.RequestInt_RawUrl(QueryString, 1)
        End Get
    End Property
    Public ReadOnly Property SortColumn() As String
        Get
            Dim s As String = CWeb.RequestStr(QueryString_SortBy, Nothing)
            If String.IsNullOrEmpty(s) Then Return s
            If s.Contains(" ") Or s.Contains(";") Then Throw New Exception("Possible attempt at sql injection")
            Return s
        End Get
    End Property
    Public ReadOnly Property IsDescending() As Boolean
        Get
            Return CWeb.RequestBool(QueryString_Descending, False)
        End Get
    End Property
    Public Function BuildQuerystring(ByVal newSort As String) As String
        Dim desc As Boolean = True 'Default direction
        If Not String.IsNullOrEmpty(Info.SortByColumn) AndAlso Not String.IsNullOrEmpty(newSort) AndAlso newSort = Info.SortByColumn Then
            desc = Not Info.Descending   'Toggle direction
        End If
        Return String.Concat("&", QueryString, "=", Info.PageIndex + 1, "&", QueryString_Descending, "=", desc, "&", QueryString_SortBy, "=", CSitemap.Encode(newSort))
    End Function
#End Region

#Region "Event Handlers"
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        DisplayMe(Info.Count)

        Dim lookFor As Integer = Request.RawUrl.IndexOf(QueryString & "=")
        If -1 = lookFor Then lookFor = Request.RawUrl.IndexOf("?") Else lookFor -= 1

        If Info.Offset > Info.Count AndAlso lookFor > 0 Then Response.RedirectPermanent(Request.RawUrl.Substring(0, lookFor), True)
        lnkNext.Visible = Info.Offset + Info.PageSize < Info.Count

        CDropdown.SetText(dd, PageSize)
        For Each i As ListItem In dd.Items
            Dim size As Integer = i.Text
            Dim url As String = Request.RawUrl
            If Not url.Contains(QueryString_PageSize & "=") Then
                If Not url.Contains("?") Then
                    i.Value = String.Concat(url, "?" & QueryString_PageSize & "=", size)
                Else
                    i.Value = String.Concat(url, "&" & QueryString_PageSize & "=", size)
                End If
            Else
                Dim from As Integer = url.IndexOf(QueryString_PageSize & "=") + QueryString_PageSize.Length + 1
                Dim [to] As Integer = url.IndexOf("&", from)
                If [to] = -1 Then [to] = url.Length
                i.Value = String.Concat(url.Substring(0, from), size, url.Substring([to]))
            End If
        Next

        lnkPrev.Visible = CurrentPage > 1 OrElse Not String.IsNullOrEmpty(Request.QueryString(QueryString_PageSize))
    End Sub
#End Region

#Region "Interface #1 of 2 - For Database-Level Paging"
    'Actual paging is performed in the database, but need to read off index/size as inputs, and write back the count as an output
    Private m_info As New CPagingInfo
    Public ReadOnly Property Info() As CPagingInfo
        Get
            With m_info
                .PageIndex = CurrentPageZeroBased
                .PageSize = PageSize
                .SortByColumn = SortColumn
                .Descending = IsDescending
            End With
            Return m_info
        End Get
    End Property
#End Region

#Region "Interface #1 of 2 - For cached data"
    'Use to perform the paging on in-memory data
    Public Function Display(ByVal list As IList) As IList
        'Store the count
        Info.Count = list.Count

        'Resort if necessary
        If Not String.IsNullOrEmpty(Info.SortByColumn) Then
            Dim copy As New ArrayList(list)
            copy.Sort(New CReflection.GenericSortBy(Info.SortByColumn, Info.Descending, copy))
            list = copy
        End If

        PageSize = CWeb.RequestInt(QueryString_PageSize, PageSize)

        'Perform the paging
        Return CUtilities.Page(list, PageSize, CurrentPage - 1)
    End Function
    Public Function Display(ByVal list As IList, ByRef sorted As IList) As IList
        'Store the count
        Info.Count = list.Count

        'Resort if necessary
        If Not String.IsNullOrEmpty(Info.SortByColumn) Then
            Dim copy As New ArrayList(list)
            copy.Sort(New CReflection.GenericSortBy(Info.SortByColumn, Info.Descending, copy))
            list = copy
        End If
        sorted = list

        PageSize = CWeb.RequestInt(QueryString_PageSize, PageSize)

        'Perform the paging
        Return CUtilities.Page(list, PageSize, CurrentPage - 1)
    End Function
#End Region

#Region "Optional Properties"
    Private m_hash As String = DEFAULT_HASH
    Public Property Hash() As String
        Get
            Return m_hash
        End Get
        Set(ByVal value As String)
            m_hash = value
        End Set
    End Property
    Private m_queryString As String = DEFAULT_QUERYSTRING_PAGING
    Public Property QueryString() As String
        Get
            Return m_queryString
        End Get
        Set(ByVal value As String)
            m_queryString = value
        End Set
    End Property
    Private m_queryString_sortBy As String = DEFAULT_QUERYSTRING_SORTBY
    Public Property QueryString_SortBy() As String
        Get
            Return m_queryString_sortBy
        End Get
        Set(ByVal value As String)
            m_queryString_sortBy = value
        End Set
    End Property
    Private m_queryString_descending As String = DEFAULT_QUERYSTRING_DESCENDING
    Public Property QueryString_Descending() As String
        Get
            Return m_queryString_descending
        End Get
        Set(ByVal value As String)
            m_queryString_descending = value
        End Set
    End Property
    Private m_queryString_pageSize As String = DEFAULT_QUERYSTRING_PAGE_SIZE
    Public Property QueryString_PageSize() As String
        Get
            Return m_queryString_pageSize
        End Get
        Set(ByVal value As String)
            m_queryString_pageSize = value
        End Set
    End Property
    Private m_pageSize As Integer = DEFAULT_PAGE_SIZE
    Public Property PageSize() As Integer
        Get
            Return m_pageSize
        End Get
        Set(ByVal value As Integer)
            m_pageSize = value
        End Set
    End Property
    Private m_outerBlockSize As Integer = DEFAULT_OUTER_BLOCK_SIZE
    Public Property OuterBlockSize() As Integer
        Get
            Return m_outerBlockSize
        End Get
        Set(ByVal value As Integer)
            m_outerBlockSize = value
        End Set
    End Property
    Private m_currentPagePadding As Integer = DEFAULT_CURRENT_PAGE_PADDING
    Public Property CurrentPagePadding() As Integer
        Get
            Return m_currentPagePadding
        End Get
        Set(ByVal value As Integer)
            m_currentPagePadding = value
        End Set
    End Property
    Private m_maxPages As Integer = DEFAULT_MAX_PAGES
    Public Property MaxPages() As Integer
        Get
            Return m_maxPages
        End Get
        Set(ByVal value As Integer)
            m_maxPages = value
        End Set
    End Property
    Private m_separator As String = DEFAULT_SEPARATOR
    Public Property Separator() As String
        Get
            Return m_separator
        End Get
        Set(ByVal value As String)
            m_separator = value
        End Set
    End Property
    Private m_friendlyUrl As String
    Public Property FriendlyUrl() As String
        Get
            Return m_friendlyUrl
        End Get
        Set(ByVal value As String)
            m_friendlyUrl = value
        End Set
    End Property
#End Region

#Region "ReadOnly Properties"
    'Set by display method
    Private m_totalRecords As Integer = 0
    Public ReadOnly Property TotalRecords() As Integer
        Get
            Return m_totalRecords
        End Get
    End Property
    'Derived
    Public ReadOnly Property TotalPages() As Integer
        Get
            Return CInt(Math.Ceiling(TotalRecords / PageSize))
        End Get
    End Property
#End Region

#Region "Private"
    'Call this method to display the dynamic links
    Private Sub DisplayMe(ByVal total As Integer)
        If total <= PageSize Then
            Me.Visible = False
            Exit Sub
        End If
        Me.Visible = True

        'Avoid displaying twice
        If m_totalRecords = total Then Exit Sub

        m_totalRecords = total
        DisplayButtons()
    End Sub
    Private Sub DisplayButtons()
        litSummary.Text = String.Format(litSummary.Text, (CurrentPage - 1) * PageSize + 1, Math.Min(CurrentPage * PageSize, TotalRecords), TotalRecords)

        'Configure Prev button
        If CurrentPage = 1 Then
            lnkPrev.NavigateUrl = Request.RawUrl
        Else
            lnkPrev.NavigateUrl = AppendParam(CurrentPage - 1)
        End If

        'Configure Next button
        If CurrentPage = TotalPages Then
            lnkNext.NavigateUrl = Request.RawUrl
        Else
            lnkNext.NavigateUrl = AppendParam(CurrentPage + 1)
        End If

        'Easy case - show all links
        If TotalPages <= MaxPages Then
            AddLinks(lbl, 1, TotalPages)
            Exit Sub
        End If

        'Case 1 - inside first block
        If CurrentPage - CurrentPagePadding <= OuterBlockSize Then
            AddLinks(1, Math.Max(OuterBlockSize, CurrentPage + CurrentPagePadding))
            AddSeparator()
            AddLinks(TotalPages - OuterBlockSize + 1, OuterBlockSize)
            Exit Sub
        End If

        'Case 2 - inside last block
        If CurrentPage > TotalPages - OuterBlockSize - CurrentPagePadding - 1 Then
            AddLinks(1, OuterBlockSize)
            AddSeparator()
            Dim startAt As Integer = Math.Min(CurrentPage - CurrentPagePadding, TotalPages - OuterBlockSize + 1)
            AddLinks(startAt, TotalPages - startAt + 1)
            Exit Sub
        End If

        'Case Else - inside central block
        AddLinks(1, OuterBlockSize)
        AddSeparator()
        AddLinks(CurrentPage - CurrentPagePadding, CurrentPagePadding * 2 + 1)
        AddSeparator()
        AddLinks(TotalPages - OuterBlockSize + 1, OuterBlockSize)
    End Sub

    Private Sub AddLinks(ByVal startAt As Integer, ByVal count As Integer)
        AddLinks(lbl, startAt, count)
    End Sub
    Private Sub AddLinks(ByVal ctrl As WebControl, ByVal startAt As Integer, ByVal count As Integer)
        For i As Integer = 0 To count - 1
            AddLink(ctrl, i + startAt)
        Next
    End Sub

    Private Sub AddSeparator()
        AddSeparator(lbl)
    End Sub
    Private Sub AddSeparator(ByVal ctrl As WebControl)
        Dim lbl As New Label()
        lbl.CssClass = CSS_SEPARATOR
        lbl.Text = Separator
        ctrl.Controls.Add(lbl)
    End Sub

    Private Sub AddLink(ByVal page As Integer)
        AddLink(lbl, page)
    End Sub
    Private Sub AddLink(ByVal ctrl As WebControl, ByVal page As Integer)
        Dim lbl As New Label()
        ctrl.Controls.Add(lbl)

        Dim lnk As New HyperLink()
        lbl.Controls.Add(lnk)

        lnk.NavigateUrl = AppendParam(page)
        lnk.Text = page.ToString
        If page = CurrentPage Then lnk.CssClass = CSS_SELECTED_LINK
    End Sub
    Private Function AppendParam(ByVal page As Integer) As String

        If Not String.IsNullOrEmpty(FriendlyUrl) Then
            Return SetHash(String.Format(FriendlyUrl, page))
        End If

        Dim rx As New Regex("([&|?])" + QueryString + "=[^&]*")

        Dim urlPath As String = Request.RawUrl

        If rx.IsMatch(urlPath) Then
            Return SetHash(rx.Replace(urlPath, String.Format("$1{0}={1}", QueryString, page)))
        ElseIf urlPath.IndexOf("?") > -1 Then
            Return SetHash(urlPath & String.Format("&{0}={1}", QueryString, page))
        Else
            Return SetHash(urlPath & String.Format("?{0}={1}", QueryString, page))
        End If
    End Function

    'Add (or replace) the bookmark on the end of the paging links
    Private Function SetHash(ByVal urlPath As String) As String
        Dim rx As New Regex("#([^&]*)")

        If Hash.Length = 0 Then Return urlPath

        If rx.IsMatch(urlPath) Then
            Return rx.Replace(urlPath, String.Format("#{0}", Hash))
        Else
            Return urlPath & String.Format("#{0}", Hash)
        End If

    End Function
#End Region

End Class
