
Imports SchemaDeploy

Partial Class pages_self_schema
	Inherits CPage


#Region "Querystring"
	Public ReadOnly Property DiffInstanceId As Integer
		Get
			Return CWeb.RequestInt("diffInstanceId")
		End Get
	End Property
	Public ReadOnly Property FixId As EFix
		Get
			Return CWeb.RequestInt("fix")
		End Get
	End Property
#End Region

	Protected Overrides Sub PageInit()
		'Menu
		UnbindSideMenu()
		AddMenuSide("Deploy", CSitemap.SelfDeploy())
		AddMenuSide("Schema", CSitemap.SelfSchemaSync, True)
		If CConfig.IsDev Then
			AddMenuSide("Data", CSitemap.SelfDataSync)
		End If
		AddMenuSide("Sql", CSitemap.SelfSql)

		'By App
		For Each i As CApp In CApp.Cache
			If i.AppId <> EApp.ControlTrack Then Continue For
			AddMenuSide(i.NameAndInstanceCount, CSitemap.AppSchema(i.AppId))
		Next


		'Display
		ctrl.Display_Admin(DiffInstanceId, FixId)
	End Sub



End Class
