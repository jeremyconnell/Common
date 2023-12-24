<%@ WebHandler Language="VB" Class="Datasrc" %>

Imports System
Imports System.Web

Public Class Datasrc : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        CGenericHandler.ProcessRequest(context, AddressOf ClearCache)
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    Private Shared Sub ClearCache(tableName As String)

    End Sub

End Class