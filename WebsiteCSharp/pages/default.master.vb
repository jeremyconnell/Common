
Partial Class MasterPage : Inherits CMasterPage


    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If Request.RawUrl.ToLower.Contains("login.aspx") Then Exit Sub
        If CSession.IsLoggedIn Then
            ctrlMenu.ShowLogout = True
        Else
            AddMenuRight("Login", CSitemap.Login, False, "Login to the Application")
        End If
    End Sub

    Public Overrides Property PageHeading() As String
        Get
            Return hgcHeading.InnerText
        End Get
        Set(ByVal value As String)
            hgcHeading.InnerText = value
        End Set
    End Property


    Public Overrides Sub AddButton(ByVal url As String, ByVal tooltip As String)
        lnk.Visible = True
        lnk.NavigateUrl = url
        lnk.ToolTip = tooltip
    End Sub
    Public Overrides Sub AddMenuLeft(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
        ctrlMenu.AddLeft(name, url, selected, tooltip)
    End Sub
    Public Overrides Sub AddMenuRight(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
        ctrlMenu.AddRight(name, url, selected, tooltip)
    End Sub
    Public Overrides Sub AddMenuSide(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
        ctrlSide.Add(name, url, selected, tooltip)
    End Sub

    Public Overrides Sub AddLinkSide(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
        Dim li As New HtmlGenericControl("li")
        plhSideLinks.Controls.Add(li)
        li.Style.Add("margin-bottom", "5px")

        Dim lnk As New HyperLink
        lnk.Font.Size = New FontUnit(FontSize.Smaller)
        lnk.Text = name
        lnk.NavigateUrl = url
        li.Controls.Add(lnk)

        If selected Then lnk.ForeColor = Drawing.Color.Black
    End Sub

    Public Overrides Property MenuDataSourceId() As String
        Get
            Return ctrlMenu.DataSourceID
        End Get
        Set(ByVal value As String)
            ctrlMenu.DataSourceID = value
        End Set
    End Property
    Public Overrides Property MenuSideDataSourceId() As String
        Get
            Return ctrlSide.DataSourceID
        End Get
        Set(ByVal value As String)
            ctrlSide.DataSourceID = value
        End Set
    End Property
    Public Overrides Property MenuTitle() As String
        Get
            Return ctrlMenu.Title
        End Get
        Set(ByVal value As String)
            ctrlMenu.Title = value
        End Set
    End Property
    Public Overrides Property MenuSelected() As String
        Get
            Return ctrlMenu.Selected
        End Get
        Set(ByVal value As String)
            ctrlMenu.Selected = value
        End Set
    End Property
    Public Overrides ReadOnly Property Fieldset() As System.Web.UI.HtmlControls.HtmlGenericControl
        Get
            Return fs
        End Get
    End Property
    Public Overrides ReadOnly Property Main() As System.Web.UI.WebControls.ContentPlaceHolder
        Get
            Return body
        End Get
    End Property

End Class

