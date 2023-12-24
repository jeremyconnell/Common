
Imports Microsoft.WindowsAzure.Management.WebSites.Models.WebSiteGetPublishProfileResponse

Partial Class pages_instances_usercontrols_UCPublishProfile
    Inherits System.Web.UI.UserControl
    Public Sub Display(pp As PublishProfile)
        litName.Text = pp.ProfileName
        litTitle.Text = pp.PublishMethod
        litPubUrl.Text = pp.PublishUrl
        litConnStr.Text = pp.SqlServerConnectionString
        litUser.Text = pp.UserName
        litPassword.Text = pp.UserPassword

        lnkDestApp.Text = pp.DestinationAppUri.ToString
        lnkDestApp.NavigateUrl = pp.DestinationAppUri.ToString()

        If Not IsNothing(pp.HostingProviderForumUri) Then
            colForum.Visible = True
            lnkForum.Text = pp.HostingProviderForumUri.ToString()
            lnkForum.NavigateUrl = pp.HostingProviderForumUri.ToString()
        End If

        If pp.PublishMethod = "FTP" Then
            colDest.Visible = False
            colConnStr.Visible = False
            litTitle.ToolTip = String.Concat("PassiveMode=", pp.FtpPassiveMode)
        Else
            colConnStr.Visible = litConnStr.Text.Length > 0
        End If

        For Each i As Database In pp.Databases
            If i.ConnectionString = pp.SqlServerConnectionString Then Continue For
            UCDatabase(plh).Display(i)
        Next
    End Sub



#Region "User Controls"
    Private Shared Function UCDatabase(ByVal target As Control) As pages_instances_usercontrols_UCDatabase
        Dim ctrl As Control = target.Page.LoadControl(CSitemap.UCDatabaseProfile)
        target.Controls.Add(ctrl)
        Return CType(ctrl, pages_instances_usercontrols_UCDatabase)
    End Function
#End Region

End Class
