



Partial Class _Default : Inherits CPageWithTableHelpers



	Protected Overrides Sub PageInit()
		Response.Redirect(CSitemap.Clients, True)
	End Sub



End Class
