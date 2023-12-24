<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCKey.ascx.cs" Inherits="pages_keys_usercontrols_UCKey" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><asp:HyperLink ID="lnkKeyName" runat="server" Font-Bold="true" /></td>
    <td><asp:HyperLink ID="litKeyGroupId" runat="server" /></td>
    <td align="center"><asp:literal ID="litKeyFormatId" runat="server" /></td>
    <td><asp:Label ID="litKeyDefault" runat="server" /></td>
    <td><asp:HyperLink ID="lnkKeyDistinct" runat="server" /></td>
    <td><asp:HyperLink ID="lnkClients" runat="server" /></td>

    <td style="padding:1px"><asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Right" ImageUrl="~/images/delete.png" ToolTip="Delete this Key" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this Key?')" /></td>
</tr>
