
Partial Class usercontrols_UCMenu : Inherits UserControl

    Public Shared RIGHT_ALIGN As New List(Of String)(New String() {"Self", "CC", "SQL", "Logs", "Error Log", "Audit Trail", "Audit", "Errors", "Sessions", "Roles", "Users"})
    Public Shared PAD_LEFT As New List(Of String)(New String() {"Keys", "Clients", "Apps"})

#Region "Members"
    Private m_showLogout As Boolean = True
    Private m_selected As String = String.Empty
#End Region

#Region "Appearance"
    'Title (text in the middle)
    Public Property Title() As String
        Get
            Return lblTitle.Text
        End Get
        Set(ByVal value As String)
            lblTitle.Text = CUtilities.Truncate(value)
            lblTitle.ToolTip = value
        End Set
    End Property
    Public Property ShowLogout() As Boolean
        Get
            Return m_showLogout
        End Get
        Set(ByVal value As Boolean)
            m_showLogout = value
        End Set
    End Property
    Public Property Selected() As String
        Get
            Return m_selected
        End Get
        Set(ByVal value As String)
            m_selected = value
        End Set
    End Property
#End Region

#Region "Low-Level Hacks"
    Public ReadOnly Property RHS() As PlaceHolder
        Get
            Return plhRight
        End Get
    End Property
    Public ReadOnly Property LHS() As PlaceHolder
        Get
            Return plhLeft
        End Get
    End Property
#End Region

#Region "Add Manually"
    Public Sub AddLeft()
        AddLeft(Page.Title)
    End Sub
    Public Sub AddLeft(ByVal name As String)
        AddLeft(name, Request.RawUrl, True, Page.Title)
    End Sub
    Public Sub AddLeft(ByVal name As String, ByVal url As String, ByVal selected As Boolean)
        AddLeft(name, url, selected, name)
    End Sub
    Public Sub AddLeft(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
        Add(plhLeft, name, url, selected, tooltip, Nothing)
    End Sub
    Public Sub AddLeft(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String, ByVal roles As IList)
        If RIGHT_ALIGN.Contains(name) Then
            Add(plhRight, name, url, selected, tooltip, roles)
        Else
            Add(plhLeft, name, url, selected, tooltip, roles)
        End If
    End Sub
    Public Sub AddRight(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
        Add(plhRight, name, url, selected, tooltip, Nothing)
    End Sub
    Public Sub AddRight(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String, ByVal roles As IList)
        Add(plhRight, name, url, selected, tooltip, roles)
    End Sub
    Private Sub Add(ByVal plh As PlaceHolder, ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String, ByVal roles As IList)
        If Not CUser.CanSee(roles) Then Exit Sub

        If String.IsNullOrEmpty(tooltip) Then tooltip = name

        Dim lnk As New HyperLink
        lnk.Text = CUtilities.Truncate(name)
        lnk.NavigateUrl = url
        lnk.ToolTip = tooltip
        If m_selected.Length > 0 Then
            If name = m_selected Then selected = True Else selected = False
        End If
        If selected Then lnk.CssClass = "selected"

        plh.Controls.Add(lnk)

        If PAD_LEFT.Contains(name) Then lnk.Style.Add("margin-left", "20px")
    End Sub
#End Region

#Region "Binding"
    Private m_DataSourceID As String
    Public Property DataSourceID() As String
        Get
            Return m_DataSourceID
        End Get
        Set(ByVal value As String)
            m_DataSourceID = value
        End Set
    End Property

    Public WriteOnly Property Provider() As SiteMapProvider
        Set(ByVal value As SiteMapProvider)
            'Store current data
            Dim left As New List(Of Control)(plhLeft.Controls.Count)
            Dim right As New List(Of Control)(plhRight.Controls.Count)
            For Each i As Control In plhLeft.Controls
                left.Add(i)
            Next
            For Each i As Control In plhRight.Controls
                right.Add(i)
            Next

            'Clear current data
            plhLeft.Controls.Clear()
            plhRight.Controls.Clear()

            'Bind...
            Dim current As SiteMapNode = value.CurrentNode
            Dim root As SiteMapNode = value.RootNode

            'Get current (ignoring querystring)
            If IsNothing(current) Then
                current = value.FindSiteMapNode(Request.Url.AbsolutePath)
            End If

            If IsNothing(current) Then
                'Error case - nothing matching, show top-level only
                For Each i As SiteMapNode In root.ChildNodes
                    AddLeft(i.Title, i.Url, False, i.Description, i.Roles)
                Next
            ElseIf IsNothing(current.ParentNode.ParentNode) Then
                'Genuine top-level match, show top-level only
                For Each i As SiteMapNode In root.ChildNodes
                    AddLeft(i.Title, i.Url, i.Key = current.Key, i.Description, i.Roles)
                Next
            Else
                Dim orig As SiteMapNode = current
                While Not IsNothing(current.ParentNode.ParentNode)
                    current = current.ParentNode
                End While

                'Show a trail
                Dim q As String = Request.Url.Query
                If -1 <> q.IndexOf("&sortBy=") Then q = q.Substring(0, q.IndexOf("&sortBy="))
                For Each i As SiteMapNode In current.ParentNode.ChildNodes
                    Dim match As Boolean = i.Url.ToLower() = Request.Url.AbsolutePath.ToLower
                    If match Then
                        '=Current node (ignoring querystring)
                        For Each j As SiteMapNode In i.ParentNode.ChildNodes
                            match = j.Url.ToLower() = Request.Url.AbsolutePath.ToLower
                            If match Then
                                AddLeft(Me.Page.Title, Request.RawUrl, match, j.Description, i.Roles)
                            ElseIf Not Me.Page.Title.ToLower.StartsWith("create") Then
                                AddLeft(j.Title, j.Url & q, match, j.Description, j.Roles)
                            End If
                        Next
                    Else
                        'for all parents, preseerve the current querystring (Note: requirement that all urls include all relevant parentIds)
                        AddLeft(i.Title, i.Url & q, orig.IsDescendantOf(i), i.Description, i.Roles)
                    End If
                Next
            End If


            'Restore original data
            For Each i As Control In left
                plhLeft.Controls.Add(i)
            Next
            For Each i As Control In right
                plhRight.Controls.Add(i)
            Next

        End Set
    End Property
#End Region

#Region "Event Handlers - Page"
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'Binding
        If Len(DataSourceID) > 0 Then
            Dim ds As SiteMapDataSource = Me.Parent.FindControl(DataSourceID)
            If IsNothing(ds) Then Return
            Me.Provider = ds.Provider
        End If

        'Auto-hide
        Me.Visible = plhLeft.Controls.Count + plhRight.Controls.Count > 0 OrElse Title.Length > 0

        btnLogout.Visible = ShowLogout AndAlso CUser.IsLoggedIn
        If btnLogout.Visible Then btnLogout.ToolTip = "Logout " & CUser.Current.UserFullName
    End Sub
#End Region

#Region "Event Handlers - Form"
    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        Session.Abandon()
        FormsAuthentication.SignOut()
        Response.Redirect(CSitemap.Login)
    End Sub
#End Region

End Class
