<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCRole.ascx.vb" Inherits="pages_roles_usercontrols_UCRole" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber"      runat="server" />.</b></td>
    <td><asp:HyperLink  ID="lnkRoleName"    Runat="server" /></td>
    <td><asp:Literal    ID="litUserCount"   runat="server" /></td>

    <td id="colAddDelete" runat="server">
        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete this Role" OnClientClick="return confirm('Are you sure you want to delete this Role?')" />
    </td>
</tr>
