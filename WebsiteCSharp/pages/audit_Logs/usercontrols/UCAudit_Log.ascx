<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCAudit_Log.ascx.vb" Inherits="pages_audit_logs_usercontrols_UCAudit_Log" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><asp:HyperLink ID="lnkLogCreated" Runat="server" /></td>
    <td><asp:literal ID="litLogTypeId" runat="server" /></td>
    <td><asp:literal ID="litLogMessage" runat="server" /></td>
</tr>
