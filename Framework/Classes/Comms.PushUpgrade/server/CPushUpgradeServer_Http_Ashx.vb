Imports System.Web

Public Class CPushUpgradeServer_Http_Ashx : Inherits CServer_Http_Ashx
    'Constructor
    Public Sub New(context As HttpContext)
        MyBase.New(context)
    End Sub


    'Config
    Protected Overrides ReadOnly Property Config As IConfig_Server
        Get
            Return CPushUpgradeServer_Config.Shared
        End Get
    End Property


    'Action - For Generic Handler (Deserialise params & Exec) 
    Public Overrides Function ExecuteMethod(methodNameEnum_ As Integer, params As CReader) As Object
        Return New CPushUpgradeServer().ExecuteMethod(methodNameEnum_, params)
    End Function
End Class

'Inherit this class in the .ashx file
Public Class CPushUpgradeServer_Http_Ashx_Handler : Implements IHttpHandler

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim s As New CPushUpgradeServer_Http_Ashx(context)
        s.ProcessRequest()
    End Sub

    Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property
End Class

