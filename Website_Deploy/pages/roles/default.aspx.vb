Partial Public Class pages_Roles_default : Inherits CPage

#Region "Data"
    Public ReadOnly Property [Roles]() As CRoleList
        Get
            Return CRole.Cache
        End Get
    End Property
#End Region

#Region "Abstract/Virtual"
    Protected Overrides Sub PageInit()
        ctrlRoles.Display(Me.Roles)
    End Sub
#End Region

#Region "Event Handlers"
    Private Sub ctrl_AddClick() Handles ctrlRoles.AddClick
        Response.Redirect(CSitemap.RoleAdd())
    End Sub
#End Region

End Class
