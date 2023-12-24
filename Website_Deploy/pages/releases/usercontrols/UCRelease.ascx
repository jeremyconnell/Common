<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCRelease.ascx.cs" Inherits="pages_targetversionhistorys_usercontrols_UCRelease" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><b><asp:HyperLink ID="litReleaseCreated" Runat="server" /></b></td>
    <td><asp:literal ID="litReleaseAppId" runat="server" /></td>
    <td><asp:Label ID="litReleaseVersionId" runat="server" /></td>
    <td><asp:literal ID="litReleaseInstanceId" runat="server" /></td>
    <td><asp:literal ID="litReleaseBranchName" runat="server" /></td>
    <td><asp:literal ID="litReleaseExpired" runat="server" /></td>
</tr>
