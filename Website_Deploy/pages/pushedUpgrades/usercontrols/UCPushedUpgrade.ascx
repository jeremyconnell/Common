<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCPushedUpgrade.ascx.cs" Inherits="pages_pushedupgrades_usercontrols_UCPushedUpgrade" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><b><asp:HyperLink ID="lnkPushUserName" Runat="server" /></b></td>
    <td><asp:literal ID="litPushInstanceId" runat="server" /></td>
    <td><asp:literal ID="litPushOldVersionId" runat="server" /></td>
    <td><asp:literal ID="litPushNewVersionId" runat="server" /></td>
    <td><asp:literal ID="litPushOldSchemaMD5" runat="server" /></td>
    <td><asp:literal ID="litPushNewSchemaMD5" runat="server" /></td>
    <td><asp:literal ID="litPushStarted" runat="server" /></td>
    <td><asp:literal ID="litPushCompleted" runat="server" /></td>

    <td style="padding:1px"><asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Right" ImageUrl="~/images/delete.png" ToolTip="Delete this PushedUpgrade" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this PushedUpgrade?')" /></td>
</tr>
