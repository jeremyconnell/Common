'Derived Instance-wrapper on base-class shared methods (wcf version is nicer)
Imports Framework

Public MustInherit Class CPushUpgradeServer_Http_Aspx : Inherits CServer_Http_Aspx

#Region "Config Settings (project-specific)"
    Protected Overrides ReadOnly Property Config As IConfig_Server
        Get
            Return CPushUpgradeServer_Config.Shared
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
        Return New CPushUpgradeServer(AddressOf TriggerUpgrade).ExecuteMethod(methodNameEnum, params)
    End Function
#End Region

    Public MustOverride Sub TriggerUpgrade()

End Class
