
Partial Class AuditTrail : Inherits CPage

#Region "Overrides"
    Protected Overrides Sub PageInit()
        With ddType
            .DataTextField = "TypeName"
            .DataValueField = "TypeId"
            .DataSource = CAudit_Type.Cache
            .DataBind()
            CDropdown.BlankItem(ddType)
        End With

        Dim a As New CAudit_Trail
        With ddTable
            '.DataSource = a.SelectDistinctTables
            '.DataBind()
            For Each s As String In a.SelectDistinctTables
                CDropdown.Add(ddTable, CAudit_Trail.ShortenTableName(s), s)
            Next
            CDropdown.BlankItem(ddTable)
        End With

        With ddUser
            .DataSource = a.SelectDistinctUserLoginNames
            .DataBind()
            CDropdown.BlankItem(ddUser)
        End With

        With ddUrl
            .DataSource = a.SelectDistinctUrls
            .DataBind()
            CDropdown.BlankItem(ddUrl)
        End With

        'Usercontrols
        With CSession.AuditTrailFilters.Custom
            For Each i As String In .Keys
                UCFilter(plhFilters).Display(i, .Item(i))
            Next
        End With

        CTextbox.OnReturnPress(txtColumnName, btnAdd)
        CTextbox.OnReturnPress(txtColumnValue, btnAdd)
        CTextbox.OnReturnPress(txtDate, btnSearch)
        CTextbox.OnReturnPress(txtPrimaryKey, btnSearch)
    End Sub
    Protected Overrides Sub PagePreRender()
        'Persist filters in session
        With CSession.AuditTrailFilters
            If Me.Page.IsPostBack Then
                .Login = Me.UserLoginName
                .SearchDate = Me.SearchDate
                .Table = Me.TableName
                .TypeId = Me.AuditTypeId
                .Url = Me.Url
                .PrimaryKey = Me.PrimaryKey
            Else
                Me.UserLoginName = .Login
                Me.SearchDate = .SearchDate
                Me.TableName = .Table
                Me.AuditTypeId = .TypeId
                Me.Url = .Url
                Me.PrimaryKey = .PrimaryKey
            End If
        End With

        'Clear paging when filter changes
        If Page.IsPostBack AndAlso ctrlPaging.Info.PageIndex <> 0 Then Response.Redirect(CSitemap.AuditTrail)

        'Perform search, using sql-based paging
        For Each i As CAudit_Trail In New CAudit_Trail().SearchWithPaging(ctrlPaging.Info, CSession.AuditTrailFilters)
            UCTrail(tbody).Display(i, chkShowUnchanged.Checked)
        Next

        'Reformat the date
        If SearchDate = DateTime.MinValue Then
            txtDate.Text = String.Empty
        Else
            txtDate.Text = SearchDate.ToString("dd-MMM-yyyy")
        End If

    End Sub
#End Region

#Region "Form"
    Public Property TableName() As String
        Get
            Return ddTable.SelectedValue
        End Get
        Set(ByVal value As String)
            CDropdown.SetValue(ddTable, value)
        End Set
    End Property
    Public Property PrimaryKey() As String
        Get
            Return txtPrimaryKey.Text
        End Get
        Set(ByVal value As String)
            txtPrimaryKey.Text = value
        End Set
    End Property
    Public Property Url() As String
        Get
            Return ddUrl.SelectedValue
        End Get
        Set(ByVal value As String)
            CDropdown.SetValue(ddUrl, value)
        End Set
    End Property
    Public Property UserLoginName() As String
        Get
            Return ddUser.SelectedValue
        End Get
        Set(ByVal value As String)
            CDropdown.SetValue(ddUser, value)
        End Set
    End Property
    Public Property AuditTypeId() As Integer
        Get
            Return CDropdown.GetInt(ddType)
        End Get
        Set(ByVal value As Integer)
            CDropdown.SetValue(ddType, value)
        End Set
    End Property
    Public Property SearchDate() As DateTime
        Get
            Return CTextbox.GetDate(txtDate)
        End Get
        Set(ByVal value As DateTime)
            CTextbox.SetDate(txtDate, value)
        End Set
    End Property
#End Region

#Region "Event Handlers"
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        If String.IsNullOrEmpty(txtColumnName.Text) Then Exit Sub

        With CSession.AuditTrailFilters.Custom
            .Item(txtColumnName.Text) = txtColumnValue.Text
            Response.Redirect(CSitemap.AuditTrail)
        End With
    End Sub
#End Region

#Region "Usercontrols"
    Private Function UCTrail(ByVal target As Control) As usercontrols_audit_trail_UCTrail
        Dim ctrl As Control = LoadControl("usercontrols/UCTrail.ascx")
        target.Controls.Add(ctrl)
        Return CType(ctrl, usercontrols_audit_trail_UCTrail)
    End Function
    Private Function UCFilter(ByVal target As Control) As usercontrols_audit_trail_UCFilter
        Dim ctrl As Control = LoadControl("usercontrols/UCFilter.ascx")
        target.Controls.Add(ctrl)
        Return CType(ctrl, usercontrols_audit_trail_UCFilter)
    End Function
#End Region

End Class
