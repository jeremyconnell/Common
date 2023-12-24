Imports System.Collections

Partial Public Class pages_Statuss_usercontrols_UCStatuss : Inherits UserControl

#Region "Events"
    Public Event AddClick()
    Public Event ResortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer)
#End Region

#Region "Members"
    Private m_list As CStatusList
#End Region

#Region "Interface"
    Public Sub Display(ByVal statuss As CStatusList) 'Full list, paging done internally
        m_list = statuss
        
        'Show/Hide Columns
        colNumber.Visible = statuss.Count > 0

        'Display
        plh.Controls.Clear() 'Only needed for ajax postbacks
        Dim sorted As IList = Nothing 'Fixes the numbering to reflect a user-sorted list (querystring sortBy)
        Dim page As IList = ctrlPaging.Display(statuss, sorted) 'In-Memory paging, also outputs the sorted list
        For Each i As CStatus In page
            UCStatus(plh).Display(i, sorted)
        Next
    End Sub
#End Region

#Region "User Controls"
    Private Shared Function UCStatus(ByVal target As Control) As pages_statuss_usercontrols_UCStatus
        Dim ctrl As Control = target.Page.LoadControl(CSitemap.UCStatus)
        target.Controls.Add(ctrl)
        Return CType(ctrl, pages_statuss_usercontrols_UCStatus)
    End Function
#End Region

#Region "Event Handlers"
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        RaiseEvent AddClick
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExport.Click
        m_list.ExportToCsv(Response)
    End Sub
    Protected Sub btnResort_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnSortByStatusName.Click, btnSortByStatusDescription.Click
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
