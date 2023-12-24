<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCClient.ascx.cs" Inherits="pages_clients_usercontrols_UCClient" %>
<%@ Register tagname="UCClient" tagprefix="uc" src="UCClientInstance.ascx" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><b><asp:HyperLink ID="lnkClientName" Runat="server" /></b></td>
    <td class="bl" align="center"><b><asp:literal ID="litClientStatusId" runat="server" /></b></td>
    <td class="bl"><asp:literal ID="litClientCode" runat="server"  /></td>
    <td><asp:HyperLink ID="lnkInstances" Runat="server" /></td>
    <td><asp:PlaceHolder ID="plhInstances" runat="server" /></td>
    <td class="bl"><asp:literal ID="litClientTrialStarted" runat="server" /></td>
    <td><asp:literal ID="litClientTrialEnded" runat="server" /></td>
    <td><asp:literal ID="litClientProductionStarted" runat="server" /></td>

    <td style="padding:1px"><asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Right" ImageUrl="~/images/delete.png" ToolTip="Delete this Client" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this Client?')" /></td>
</tr>
