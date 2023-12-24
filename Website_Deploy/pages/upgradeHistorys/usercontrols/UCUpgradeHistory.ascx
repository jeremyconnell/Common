<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCUpgradeHistory.ascx.cs" Inherits="pages_upgradehistorys_usercontrols_UCUpgradeHistory" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><asp:literal ID="litChangeReportId" runat="server" /></td>
    <td><asp:Label ID="litChangeNewVersionId" runat="server" /></td>
    <td><asp:Label ID="litChangeNewSchemaMD5" runat="server" /></td>
    <td><asp:Label ID="litChangeStarted" runat="server" /></td>
    <td><asp:Label ID="litChangeFinished" runat="server" /></td>
</tr>

