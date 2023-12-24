Imports Microsoft.VisualBasic

'Usage:
'1. Implement the required logic in the Rewrite(app) method e.g. app.Context.RewritePath
'2. Add a line into the web.config like so (may need to upgrade to 3.5):
'		<httpModules>
'			<add type="CUrlRewrite" name="CUrlRewrite" />
Public Class CUrlRewrite : Inherits Framework.CUrlRewrite
    'Constructor
    Public Sub New()
        MyBase.New(False)
    End Sub

    'Logic
    Protected Overloads Overrides Sub Rewrite(ByVal app As HttpApplication)
        'app.Context.RewritePath()
    End Sub
End Class