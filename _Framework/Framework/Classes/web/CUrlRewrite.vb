Imports System.Web

'Usage:
'1. Inherit from this class
'2. Add a constructor, calling the base constructor with a true/false (false for forms auth)
'3. Implement the required logic in the Rewrite(app) method e.g. app.Context.RewritePath
'4. Add a line into the web.config like so (may need to upgrade to 3.5):
'		<httpModules>
'			<add type="CUrlRewrite" name="CUrlRewrite" />
Public MustInherit Class CUrlRewrite : Implements IHttpModule
    'Abstract
    Protected MustOverride Sub Rewrite(ByVal app As HttpApplication)

    'Members
    Protected m_windowsAuth As Boolean = False

    'Constructor
    Protected Sub New(ByVal windowsAuth As Boolean)
        m_windowsAuth = windowsAuth
    End Sub

    'Other
    Public Overridable Sub Init(ByVal app As HttpApplication) Implements IHttpModule.Init
        If m_windowsAuth Then
            AddHandler app.BeginRequest, AddressOf Me.BaseModuleRewriter_Event
        Else
            AddHandler app.AuthorizeRequest, AddressOf Me.BaseModuleRewriter_Event
        End If
    End Sub
    Public Overridable Sub Dispose() Implements IHttpModule.Dispose
    End Sub
    Protected Overridable Sub BaseModuleRewriter_Event(ByVal sender As Object, ByVal e As EventArgs)
        Rewrite(DirectCast(sender, HttpApplication))
    End Sub
End Class