<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCUser.ascx.vb" Inherits="pages_users_usercontrols_UCUser" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><asp:HyperLink ID="lnkUserLoginName" Runat="server" ToolTip="Edit this User" Font-Bold=true /></td>
    <td><asp:literal ID="litUserFirstName" runat="server" /></td>
    <td><asp:literal ID="litUserLastName" runat="server" /></td>
    <td><asp:HyperLink ID="lnkUserEmail" runat="server" ToolTip="Send an Email" /></td>
    <td><asp:literal ID="litRoles" runat="server" /></td>
    <td><asp:Literal ID="litLastLogin" runat=server /></td>
    <td style="text-align:center"><asp:CheckBox ID="chkUserIsDisabled" Runat="server" AutoPostBack="True" /></td>
    <td style="text-align:center"><asp:CheckBox ID="chkUserIsLockedOut" Runat="server" AutoPostBack="True" Enabled=false /></td>

    <td id="colAddDelete" runat="server">
        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete this User" OnClientClick="return confirm('Are you sure you want to delete this User?')" />
    </td>
</tr>
