
'1. This Generic WCF Passthru fully resolves (and hides) any interface
'2. Note: Explicitly implementing the (pre-defined) interface will replace Protogen serialisation (and custom encryption) with DataContract serialisation
Public Class CUpgradeServer_Wcf : Inherits CServer_Wcf ': Implements IUpgrade, IUpgrade_SingleParam

#Region "Config Settings"
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

#Region "WCF super-method (full implementation)"
    Protected Overrides Function ExecuteMethod(methodNameEnum As Integer, params As CReader) As Object
        If Me.IsDistributionPoint Then
            Return CUpgradeServer_DistributionPoint.Shared.ExecuteMethod(methodNameEnum, params)
        Else
            Return CUpgradeServer.Shared.ExecuteMethod(methodNameEnum, params)
        End If
    End Function
#End Region

End Class
