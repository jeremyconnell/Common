Imports SchemaMembership

Partial Public Class pages_Clicks_usercontrols_UCClicks : Inherits UserControl

#Region "Events"
    Public Event AddClick()
    Public Event ExportClick()
    Public Event ResortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer)
#End Region

#Region "Interface"
    Public Sub Display(ByVal session As SchemaMembership.CSession)
        Dim clicks As New CClickList(ctrlPaging.Display(session.Clicks)) 'Unpaged, allows for timespan to be computed
        Display(clicks, False, False)
    End Sub
    Public Sub Display(ByVal clicks As CClickList, ByVal showHost As Boolean) 'Single page due to sql-based paging (by exposing Me.Info)
        Display(clicks, True, showHost)
    End Sub
    Private Sub Display(ByVal clicks As CClickList, ByVal showUser As Boolean, ByVal showHost As Boolean) 'Single page due to sql-based paging (by exposing Me.Info)
        'Show/Hide Columns
        colNumber.Visible = clicks.Count > 0
        colUser.Visible = showUser
        colHost.Visible = showHost
        colTime.Visible = Not showUser

        'Display
        For Each i As CClick In clicks
            UCClick(plh).Display(i, clicks, Me.PagingInfo, showUser, showHost)
        Next
    End Sub
    Public ReadOnly Property PagingInfo() As CPagingInfo
        Get
            Return ctrlPaging.Info
        End Get
    End Property
#End Region

#Region "User Controls"
    Private Shared Function UCClick(ByVal target As Control) As pages_clicks_usercontrols_UCClick
        Dim ctrl As Control = target.Page.LoadControl(CSitemap.UCClick)
        target.Controls.Add(ctrl)
        Return CType(ctrl, pages_clicks_usercontrols_UCClick)
    End Function
#End Region

#Region "Event Handlers"
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExport.Click
        RaiseEvent ExportClick()
    End Sub
    Protected Sub btnResort_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnSortByClickDate.Click, btnSortBySession.Click, btnSortByClickHost.Click, btnSortByClickUrl.Click, btnSortByClickQuerystring.Click
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
