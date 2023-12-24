<%@ WebHandler Language="VB" Class="Upgrade" %>

Imports System
Imports Comms.Upgrade.Server

Public Class Upgrade : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim s As New CUpgradeServer_Rest_Ashx(context)
        s.ProcessRequest()
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

End Class