Partial Public Class pages_Sessions_usercontrols_UCSessions : Inherits UserControl

#Region "Events"
    Public Event ExportClick()
    Public Event ResortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer)
#End Region

#Region "Interface"
    Public Sub Display(ByVal sessions As CSessionList) 'Single page due to sql-based paging (by exposing Me.Info)
        'Show/Hide Columns
        colNumber.Visible = sessions.Count > 0

        'Display
        For Each i As SchemaMembership.CSession In sessions
            UCSession(plh).Display(i, sessions, Me.PagingInfo)
        Next
    End Sub
    Public ReadOnly Property PagingInfo() As CPagingInfo
        Get
            Return ctrlPaging.Info
        End Get
    End Property
#End Region

#Region "User Controls"
    Private Shared Function UCSession(ByVal target As Control) As pages_sessions_usercontrols_UCSession
        Dim ctrl As Control = target.Page.LoadControl(CSitemap.UCSession)
        target.Controls.Add(ctrl)
        Return CType(ctrl, pages_sessions_usercontrols_UCSession)
    End Function
#End Region

#Region "Event Handlers"
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExport.Click
        RaiseEvent ExportClick()
    End Sub
    Protected Sub btnResort_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnSortBySessionUserLoginName.Click
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
