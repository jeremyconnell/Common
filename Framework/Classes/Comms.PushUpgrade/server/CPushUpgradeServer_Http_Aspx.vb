'Derived Instance-wrapper on base-class shared methods (wcf version is nicer)

Public MustInherit Class CPushUpgradeServer_Http_Aspx : Inherits CServer_Http_Aspx

    'Config
    Protected Overrides ReadOnly Property Config As IConfig_Server
        Get
            Return CPushUpgradeServer_Config.Shared
        End Get
    End Property


    'Action - For Web Page  (Deserialise params & Exec)
    Public Overrides Function ExecuteMethod(methodNameEnum As Integer, params As CReader) As Object
        Return New CPushUpgradeServer().ExecuteMethod(methodNameEnum, params)
    End Function

End Class
