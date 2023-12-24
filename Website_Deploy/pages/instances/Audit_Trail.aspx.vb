
Imports SchemaDeploy

Partial Class pages_instances_Audit_Trail : Inherits CPageDeploy

#Region "Querystring (Filters)"
	Public ReadOnly Property Search As String
		Get
			Return CWeb.RequestStr("search")
		End Get
	End Property

	'Rename or Delete:
	Public ReadOnly Property TypeId As Integer
		Get
			Return CWeb.RequestInt("typeId")
		End Get
	End Property
	Public ReadOnly Property InstanceId As Integer
		Get
			Return CWeb.RequestInt("instanceId")
		End Get
	End Property
#End Region

#Region "Data"
	Public ReadOnly Property Database As CDataSrc
		Get
			Return Instance.DatabaseDirectOrWeb
		End Get
	End Property
	Public ReadOnly Property AppId As Integer
		Get
			Return App.AppId
		End Get
	End Property
	Public ReadOnly Property App As CApp
		Get
			Return Instance.App
		End Get
	End Property
	Public ReadOnly Property Instance As CInstance
		Get
			Return CInstance.Cache.GetById(InstanceId)
		End Get
	End Property
#End Region

#Region "Overrides"
	Protected Overrides Sub PageInit()
		MenuInstanceAudit(AppId, InstanceId)

		ddApp.DataSource = CApp.Cache
		ddApp.DataBind()
		CDropdown.SetValue(ddApp, AppId)


		ddIns.DataSource = App.Instances
		ddIns.DataBind()
		CDropdown.BlankItem(ddIns, "-- Select Instance --")
		CDropdown.SetValue(ddIns, InstanceId)

		With ddType
			.DataTextField = "TypeName"
			.DataValueField = "TypeId"
			.DataSource = CAudit_Type.Cache
			.DataBind()
			CDropdown.BlankItem(ddType)
		End With

		Dim a As New CAudit_Trail(Database)
		If Not IsNothing(a) Then
			With ddTable
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
		End If

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
				.PrimaryKey = Me.PrimaryKey
			Else
				Me.UserLoginName = .Login
				Me.SearchDate = .SearchDate
				Me.TableName = .Table
				Me.AuditTypeId = .TypeId
				Me.PrimaryKey = .PrimaryKey
			End If
		End With

		'Clear paging when filter changes
		If Page.IsPostBack AndAlso ctrlPaging.Info.PageIndex <> 0 Then Response.Redirect(CSitemap.AuditTrail)

		'Perform search, using sql-based paging
		Dim chunk As CAudit_TrailList = New CAudit_Trail(Database).SearchWithPaging(ctrlPaging.Info, CSession.AuditTrailFilters)
		chunk.PreloadDatas(Database)
		For Each i As CAudit_Trail In chunk
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
	Public Sub ddApp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddApp.SelectedIndexChanged
		Dim appId As Integer = CDropdown.GetInt(ddApp)
		Dim a As CApp = CApp.Cache.GetById(appId)
		Response.Redirect(CSitemap.InstanceAuditTrail(App.Instances(0).InstanceId))
	End Sub
	Public Sub ddIns_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddIns.SelectedIndexChanged
		Response.Redirect(CSitemap.InstanceAuditTrail(CDropdown.GetInt(ddIns)))
	End Sub
#End Region

#Region "Usercontrols"
	Private Function UCTrail(ByVal target As Control) As usercontrols_audit_trail_UCTrail
		Dim ctrl As Control = LoadControl("~/pages/audit-trail/usercontrols/UCTrail.ascx")
		target.Controls.Add(ctrl)
		Return CType(ctrl, usercontrols_audit_trail_UCTrail)
	End Function
	Private Function UCFilter(ByVal target As Control) As usercontrols_audit_trail_UCFilter
		Dim ctrl As Control = LoadControl("~/pages/audit-trail/usercontrols/UCFilter.ascx")
		target.Controls.Add(ctrl)
		Return CType(ctrl, usercontrols_audit_trail_UCFilter)
	End Function
#End Region

End Class
