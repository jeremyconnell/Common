Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

'Provider for the remote driver CWebSrcSoap
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WSDataSrc : Inherits CWSDataSrc

    Public Overloads Overrides Sub ClearCache(ByVal tableName As String)
        ClearCache() 'Can be more specific when schema project is in scope
    End Sub

End Class
