<%@ Application Language="VB" %>
<%@ Import Namespace="Comms.Upgrade.Client"     %>
<%@ Import Namespace="Comms.Upgrade.Interface"  %> 
<%@ Import Namespace="Comms.Compute.Server"     %>
<%@ Import Namespace="Comms.Abstract"           %>

<script runat="server">
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        'Self-Config
        CComputeServer.Config_.WriteConfigSetting_FolderRoot()
        CComputeServer.Config_.WriteConfigSetting_RequireEncryption(True)
        
        CUpgradeClient.Config_.WriteConfigSetting_Encryption(EEncryption.Rij)
        CUpgradeClient.Config_.WriteConfigSetting_HostName("cutplan.fabric-utilization.co.uk")
        
        'Self-upgrade (client)        
        CUpgradeClient.Factory().DoUpgrade_Website(EFolderName.Slave_Webservice)
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
    End Sub
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
    End Sub
    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
    End Sub       
</script>