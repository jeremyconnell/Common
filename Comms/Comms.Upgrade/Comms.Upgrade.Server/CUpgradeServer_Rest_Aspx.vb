'Derived Instance-wrapper on base-class shared methods (wcf version is nicer)
Public Class CUpgradeServer_Rest_Aspx : Inherits CServer_Http_Aspx

#Region "Config Settings (project-specific)"
    Protected Overrides ReadOnly Property Config As IConfig_Server
        Get
            Return CUpgradeServer_Config.Shared
        End Get
    End Property
    Protected Overridable ReadOnly Property IsDistributionPoint As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "REST Implementation (Deserialise, then call a shared method, according to enum)"
    Public Overrides Function ExecuteMethod(methodNameEnum As Integer, params As CReader) As Object
        If Me.IsDistributionPoint Then
            Return CUpgradeServer_DistributionPoint.Shared.ExecuteMethod(methodNameEnum, params)
        Else
            Return CUpgradeServer.Shared.ExecuteMethod(methodNameEnum, params)
        End If
    End Function
#End Region


End Class
