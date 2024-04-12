Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.HtmlControls

'Used for postbacks involving url-rewriting

'1. Register the namespace in web.config
'<pages>
'   <controls>
'       <add tagPrefix="uc" namespace="Framework" assembly="Framework"/>

'2. Replace the <asp:Form tags with <uc:CActionlessForm
Public Class CActionlessForm : Inherits HtmlForm
    Protected Overloads Overrides Sub RenderAttributes(ByVal w As HtmlTextWriter)
        w.WriteAttribute("name", Me.Name)
        MyBase.Attributes.Remove("name")

        w.WriteAttribute("method", Me.Method)
        MyBase.Attributes.Remove("method")

        Me.Attributes.Render(w)

        MyBase.Attributes.Remove("action")

        If MyBase.ID IsNot Nothing Then
            w.WriteAttribute("id", MyBase.ClientID)
        End If
    End Sub
End Class