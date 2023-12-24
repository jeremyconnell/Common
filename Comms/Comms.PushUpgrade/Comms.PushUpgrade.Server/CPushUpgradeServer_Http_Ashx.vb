Imports System.Web
Imports Framework

Public Class CPushUpgradeServer_Http_Ashx : Inherits CServer_Http_Ashx

    Public Sub New(context As HttpContext,
                   Optional upgradeSelf As CPushUpgradeServer.DTriggerUpgrade = Nothing,
                   Optional moreConfigSett As CPushUpgradeServer.DMoreConfigSettings = Nothing,
                   Optional removeConfigSett As CPushUpgradeServer.DRemoveConfigSetting = Nothing)

        MyBase.New(context)

        m_server = New CPushUpgradeServer(upgradeSelf, moreConfigSett, removeConfigSett)
    End Sub

    Private m_server As CPushUpgradeServer

    Protected Overrides ReadOnly Property Config As IConfig_Server
        Get
            Return CPushUpgradeServer_Config.Shared
        End Get
    End Property
    Public Overrides Function ExecuteMethod(methodNameEnum_ As Integer, params As CReader) As Object

        Return m_server.ExecuteMethod(methodNameEnum_, params)
    End Function


End Class
