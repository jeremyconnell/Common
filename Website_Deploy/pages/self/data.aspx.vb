
Imports SchemaDeploy

Partial Class pages_self_data
	Inherits CPage

	Protected Overrides Sub PageInit()
		UnbindSideMenu()
		If CConfig.IsDev Then
			AddMenuSide("Deploy", CSitemap.SelfDeploy())
			AddMenuSide("Schema", CSitemap.SelfSchemaSync)
		End If
		AddMenuSide("Data", CSitemap.SelfDataSync, True)
		AddMenuSide("Sql", CSitemap.SelfSql)

		'By App
		For Each i As CApp In CApp.Cache
			If i.AppId <> EApp.ControlTrack Then Continue For
			AddMenuSide(i.NameAndInstanceCount, CSitemap.AppData(i.AppId))
		Next

		ctrl.Display()
	End Sub

End Class