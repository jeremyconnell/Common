
Imports Microsoft.WindowsAzure.Management.WebSites.Models.WebSiteGetPublishProfileResponse

Partial Class pages_instances_usercontrols_UCDatabase
    Inherits System.Web.UI.UserControl


    Public Sub Display(db As Database)
        litName.Text = db.Name
        litType.Text = db.Type
        litType.ToolTip = db.ProviderName
        litConnStr.Text = db.ConnectionString
    End Sub

End Class
