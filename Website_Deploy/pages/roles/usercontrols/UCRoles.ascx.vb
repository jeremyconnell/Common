Partial Public Class pages_Roles_usercontrols_UCRoles : Inherits UserControl

#Region "Events"
    Public Event AddClick()
#End Region

#Region "Interface"
    Public Sub Display(ByVal roles As CRoleList) 'Full list, paging done internally
        'Show/Hide Columns
        colNumber.Visible = roles.Count > 0

        'Display
        For Each i As CRole In ctrlPaging.Display(roles) 'In-Memory paging
            UCRole(plh).Display(i, roles, ctrlPaging.Info)
        Next
    End Sub
#End Region

#Region "User Controls"
    Private Shared Function UCRole(ByVal target As Control) As pages_roles_usercontrols_UCRole
        Dim ctrl As Control = target.Page.LoadControl(CSitemap.UCRole)
        target.Controls.Add(ctrl)
        Return CType(ctrl, pages_roles_usercontrols_UCRole)
    End Function
#End Region

#Region "Event Handlers"
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        RaiseEvent AddClick
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
