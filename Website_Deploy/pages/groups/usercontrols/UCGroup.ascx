<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCGroup.ascx.cs" Inherits="pages_groups_usercontrols_UCGroup" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><b><asp:HyperLink ID="lnkGroupName" Runat="server" /></b></td>
    <td><asp:HyperLink ID="lnkKeys" Runat="server" /></td>

    <td style="padding:1px"><asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Right" ImageUrl="~/images/delete.png" ToolTip="Delete this Group" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this Group?')" /></td>
</tr>
