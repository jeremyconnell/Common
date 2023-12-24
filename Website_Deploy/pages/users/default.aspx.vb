Partial Public Class pages_Users_default : Inherits CPage

#Region "Querystring"
    Public ReadOnly Property Search() As String
        Get
            Return CWeb.RequestStr("search")
        End Get
    End Property
#End Region

#Region "Members"
    Private m_users As CUserList
#End Region

#Region "Data"
    Public ReadOnly Property [Users]() As CUserList
        Get
            If IsNothing(m_users) Then
                m_users = New CUser().SelectSearch(ctrlUsers.PagingInfo, txtSearch.Text) 'Sql-based Paging
            End If
            Return m_users
        End Get
    End Property
    Public ReadOnly Property UsersAsDataset() As System.Data.DataSet
        Get
            Dim u As New CUser
            Return u.SelectSearch_Dataset(txtSearch.Text)
        End Get
    End Property
#End Region

#Region "Overrides"
    Protected Overrides Sub PageInit()
        AddMenuSide("Search")
        AddMenuSide("New User", CSitemap.UserAdd)

        txtSearch.Text = Search
        CTextbox.OnReturnPress(txtSearch, btnSearch)
        Form.DefaultFocus = txtSearch.ClientID

        ctrlUsers.Display(Me.Users)
    End Sub
#End Region

#Region "Event Handlers - Form "
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Response.Redirect(CSitemap.Users(txtSearch.Text))
    End Sub
#End Region

#Region "Event Handlers - UserControl"
    Private Sub ctrl_AddClick() Handles ctrlUsers.AddClick
        Response.Redirect(CSitemap.UserAdd())
    End Sub
    Private Sub ctrl_ExportClick() Handles ctrlUsers.ExportClick
        CDataSrc.ExportToCsv(UsersAsDataset, Response, "Users.csv")
    End Sub
    Private Sub ctrl_ResortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) Handles ctrlUsers.ResortClick
        Response.Redirect(CSitemap.Users(txtSearch.Text, New CPagingInfo(0, pageNumber - 1, sortBy, descending)))
    End Sub
#End Region

End Class
