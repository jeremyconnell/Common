<%@ WebHandler Language="VB" Class="PushUpgrade" %>

Imports System
Imports Comms.PushUpgrade.Server

Public Class PushUpgrade : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim s As New CPushUpgradeServer_Rest_Ashx(context, AddressOf UpgradeSelf)
        s.ProcessRequest()
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    Private Sub UpgradeSelf()
        Throw New Exception("No Self-Upgrade Trigger")
    End Sub

End Class