<%@ Application Language="VB" %>
<%@ Import Namespace="Comms.Upgrade.Server" %>
<%@ Import Namespace="Comms.Upgrade.Client" %>
<%@ Import Namespace="Comms.PushUpgrade.Client" %>

<script runat="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        AddHandler [Error], AddressOf Application_Error

        'CAudit_Log.Log(ELogType.Website, "Started")


        'Webservice settings
        If True OrElse Not CUpgradeClient.Config_.IsDevMachine Then
            'Debug-Mode (Temp-disable)
            CUpgradeServer.Config_.WriteConfigSetting_DisableAutoUpgrades()

            'Security: Client-to-Server
            Dim password As String = "ControlTrack_27April"
            CUpgradeServer.Config_.WriteConfigSetting_RequireEncryption(True)
            CUpgradeServer.Config_.WriteConfigSetting_Password(password)

            CUpgradeClient.Config_.WriteConfigSetting_Encryption(EEncryption.Rij)
            CUpgradeClient.Config_.WriteConfigSetting_UseSsl(True)

            'Security: Server-to-Client
            CPushUpgradeClient.Config_.WriteConfigSetting_Password(password)
            CPushUpgradeClient.Config_.WriteConfigSetting_Encryption(EEncryption.Rij)
            CPushUpgradeClient.Config_.WriteConfigSetting_UseSsl(True)
        End If
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
        'CAudit_Log.Log(ELogType.Website, "Stopped")
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
        Dim name As String = String.Empty
        If User.Identity.IsAuthenticated Then name = User.Identity.Name
        Application("errorId") = CAudit_Error.Log(Server.GetLastError, name, name)
        Response.Redirect("~/error.aspx")
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub

</script>