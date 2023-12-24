Imports System.Web

Public Class CUpgradeServer_Rest_Ashx : Inherits CServer_Http_Ashx

    Public Sub New()
        Me.New(HttpContext.Current)
    End Sub
    Public Sub New(context As HttpContext)
        MyBase.New(context)
    End Sub

    Protected Overrides ReadOnly Property Config As IConfig_Server
        Get
            Return CUpgradeServer_Config.Shared
        End Get
    End Property
    Public Overrides Function ExecuteMethod(methodNameEnum_ As Integer, params As CReader) As Object
        Return CUpgradeServer.Shared.ExecuteMethod(methodNameEnum_, params)
    End Function

End Class
