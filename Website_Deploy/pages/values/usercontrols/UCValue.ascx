<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCValue.ascx.cs" Inherits="pages_values_usercontrols_UCValue" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><asp:HyperLink ID="litValueInstanceId" runat="server" /></td>
    <td><asp:HyperLink ID="lnkValueKeyName" Runat="server" /></td>
    <td><asp:Label ID="litValueAsString" runat="server" /></td>

    <td style="padding:1px"><asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Right" ImageUrl="~/images/delete.png" ToolTip="Delete this Value" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this Value?')" /></td>
</tr>
