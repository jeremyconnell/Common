<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCAudit_Error.ascx.vb" Inherits="pages_audit_errors_usercontrols_UCAudit_Error" %>
<tr id="row" runat="server">
    <td nowrap valign="top"><b><asp:Literal ID="litNumber" runat="server" /></b></td>
    <td align="right"><asp:literal ID="litErrorId" runat="server" /></td>
    <td nowrap><asp:HyperLink ID="lnkErrorDateCreated" Runat="server" /></td>
    <td nowrap><asp:literal ID="litErrorUserName" runat="server" /></td>
    <td><asp:literal ID="litErrorMessage" runat="server" /></td>
    <td><asp:literal ID="litErrorInnerMessage" runat="server" /></td>
    <td>
        <div></dib><asp:literal ID="litErrorType" runat="server" /></div>
        <div><asp:literal ID="litErrorInnerType" runat="server" /></div>
    </td>
    <td>
        <div style="float:left"><asp:HyperLink ID="lnkErrorWebsite" runat="server" ToolTip="Original Url - Click to launch" /></div>
        <div style="float:left;padding-left:10px;"><asp:literal ID="litErrorMachineName" runat="server" /></div>
    </td>
</tr>
