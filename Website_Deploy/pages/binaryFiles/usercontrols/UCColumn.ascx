<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCColumn.ascx.cs" Inherits="pages_binaryFiles_usercontrols_UCColumn" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td>
        <asp:Label ID="lblColumn" runat="server" />
    </td>
    <td nowrap><asp:Label ID="lblType" runat="server" /></td>
    <td nowrap><asp:Label ID="lblNull" runat="server" /> </td>
</tr>
