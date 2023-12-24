Partial Public Class pages_Users_usercontrols_UCUsers : Inherits UserControl

#Region "Events"
    Public Event AddClick()
    Public Event ExportClick()
    Public Event ResortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer)
#End Region

#Region "Interface"
    Public Sub Display(ByVal users As CUserList) 'Single page due to sql-based paging (by exposing Me.Info)
        'Show/Hide Columns
        colNumber.Visible = users.Count > 0

        'Display
        For Each i As CUser In users
            UCUser(plh).Display(i, users, Me.PagingInfo)
        Next
    End Sub
    Public ReadOnly Property PagingInfo() As CPagingInfo
        Get
            Return ctrlPaging.Info
        End Get
    End Property
#End Region

#Region "User Controls"
    Private Shared Function UCUser(ByVal target As Control) As pages_users_usercontrols_UCUser
        Dim ctrl As Control = target.Page.LoadControl(CSitemap.UCUser)
        target.Controls.Add(ctrl)
        Return CType(ctrl, pages_users_usercontrols_UCUser)
    End Function
#End Region

#Region "Event Handlers"
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        RaiseEvent AddClick
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnExport.Click
        RaiseEvent ExportClick()
    End Sub
    Protected Sub btnResort_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnSortByUserPasswordHashedSha1.Click, btnSortByUserPasswordSalt.Click, btnSortByUserFirstName.Click, btnSortByUserLastName.Click, btnSortByUserEmail.Click, btnSortByUserIsDisabled.Click, btnSortByUserCreatedDate.Click, btnSortByUserLastLoginDate.Click, btnSortByUserLastPasswordChangedDate.Click, btnSortByUserPasswordQuestion.Click, btnSortByUserPasswordAnswer.Click, btnSortByUserFailedPasswordAttemptCount.Click, btnSortByUserFailedPasswordAttemptStartDate.Click, btnSortByUserIsLockedOut.Click, btnSortByUserLastLockoutDate.Click, btnSortByUserComments.Click
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
