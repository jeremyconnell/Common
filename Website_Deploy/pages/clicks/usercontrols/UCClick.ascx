<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCClick.ascx.vb" Inherits="pages_clicks_usercontrols_UCClick" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><b><asp:HyperLink ID="lnkClickDate" Runat="server" Target=_blank ToolTip="View the page that was clicked on" /></b></td>
    <td id=colTime runat=server align=right><asp:Label ID=lblTime runat=server /></td>
    <td id=colUser runat=server><asp:HyperLink ID="lnkUsers" runat="server" ToolTip="Filter by User" /></td>
    <td id=colHost runat=server><asp:Literal ID=litHost runat=server /></td>
    <td><asp:HyperLink ID="lnkUrl" runat="server" ToolTip="Filter by Url" /></td>
    <td><table ID=tbl runat=server border=1 /></td>
</tr>
