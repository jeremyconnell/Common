
Imports System.Data
Imports SchemaDeploy

Partial Class pages_instances_Audit_Errors : Inherits CPageDeploy

#Region "Querystring (Filters)"
	Public ReadOnly Property Search As String
		Get
			Return CWeb.RequestStr("search")
		End Get
	End Property

	'Rename or Delete:

	Public ReadOnly Property InstanceId As Integer
		Get
			Return CWeb.RequestInt("instanceId")
		End Get
	End Property
#End Region

#Region "Members"
	Private m_audit_Errors As CAudit_ErrorList
#End Region

#Region "Data"
	Public ReadOnly Property [Audit_Errors]() As CAudit_ErrorList
		Get
			If IsNothing(m_audit_Errors) Then
				m_audit_Errors = New CAudit_Error(Database).SelectSearch(ctrlAudit_Errors.PagingInfo, txtSearch.Text, chkUniqueOnly.Checked) 'Sql-based Paging
			End If
			Return m_audit_Errors
		End Get
	End Property
	Public ReadOnly Property Audit_ErrorsAsDataSet() As DataSet
		Get
			Return New CAudit_Error().SelectSearch_Dataset(txtSearch.Text, chkUniqueOnly.Checked)
		End Get
	End Property


	Public ReadOnly Property Database As CDataSrc
		Get
			If Not IsNothing(DbDirect) Then Return DbDirect Else Return DbViaWeb
		End Get
	End Property

	Public ReadOnly Property DbDirect As CDataSrc
		Get
			Return Instance.Database
		End Get
	End Property
	Public ReadOnly Property DbViaWeb As CDataSrc
		Get
			Return Instance.DatabaseViaWeb
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
		MenuInstanceErrors(AppId, InstanceId)

		ddApp.DataSource = CApp.Cache
		ddApp.DataBind()
		CDropdown.SetValue(ddApp, AppId)


		ddIns.DataSource = App.Instances
		ddIns.DataBind()
		CDropdown.BlankItem(ddIns, "-- Select Instance --")
		CDropdown.SetValue(ddIns, InstanceId)


		txtSearch.Text = CWeb.RequestStr("search")
		chkUniqueOnly.Checked = CWeb.RequestBool("uniqueOnly", True)

		'Display 
		ctrlAudit_Errors.Display(Me.Audit_Errors, chkUniqueOnly.Checked)
	End Sub
	Protected Overrides Sub PagePreRender()
		If Page.IsPostBack Then Response.Redirect(CSitemap.InstanceErrorLog(InstanceId, txtSearch.Text, chkUniqueOnly.Checked))
	End Sub
#End Region


#Region "Event Handlers"
	Public Sub ddApp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddApp.SelectedIndexChanged
		Dim appId As Integer = CDropdown.GetInt(ddApp)
		Dim a As CApp = CApp.Cache.GetById(appId)
		Response.Redirect(CSitemap.InstanceErrorLog(App.Instances(0).InstanceId))
	End Sub
	Public Sub ddIns_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddIns.SelectedIndexChanged
		Response.Redirect(CSitemap.InstanceErrorLog(CDropdown.GetInt(ddIns)))
	End Sub
	Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
		With New CAudit_Error(Instance.DatabaseDirectOrWeb)
			.DeleteAll()
		End With
		Response.Redirect(CSitemap.InstanceErrorLog(InstanceId))
	End Sub
#End Region




End Class
