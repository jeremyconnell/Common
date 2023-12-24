<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCStatus.ascx.vb" Inherits="pages_statuss_usercontrols_UCStatus" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><b><asp:HyperLink ID="lnkStatusName" Runat="server" /></b></td>
    <td><asp:literal ID="litStatusDescription" runat="server" /></td>
    <td><asp:HyperLink ID="lnkClients" runat="server" /></td>

    <td style="padding:1px"><asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Right" ImageUrl="~/images/delete.png" ToolTip="Delete this Status" OnClientClick="return confirm('Are you sure you want to delete this Status?')" /></td>
</tr>
