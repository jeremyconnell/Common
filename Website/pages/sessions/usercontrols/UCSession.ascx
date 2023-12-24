<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCSession.ascx.vb" Inherits="pages_sessions_usercontrols_UCSession" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><b><asp:HyperLink ID="lnkMinDate" Runat="server" /></b></td>
    <td><asp:HyperLink ID="lnkUserLoginName" Runat="server" /></td>
    <td><asp:HyperLink ID="lnkClicks" Runat="server" /></td>
    <td align=right><asp:HyperLink ID="lnkTimespan" Runat="server" /></td>
</tr>
