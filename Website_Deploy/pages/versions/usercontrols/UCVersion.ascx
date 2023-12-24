<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCVersion.ascx.cs" Inherits="pages_versions_usercontrols_UCVersion" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><b><asp:HyperLink ID="lnkVersionName" Runat="server" /></b></td>
    <td class="bl"><asp:HyperLink ID="litDiff" runat="server" /></td>
    <td class="br"><asp:literal ID="litDiffSize" runat="server" /></td>
    <td><asp:HyperLink ID="litVersionFileCount" runat="server" /></td>
    <td><asp:literal ID="litVersionTotalBytes" runat="server" /></td>
    <td class="bl"><asp:Label ID="litVersionFilesMD5" runat="server" /></td>
    <td class="br"><asp:Label ID="litVersionSchemaMD5" runat="server" /></td>
    <td><asp:HyperLink ID="lnkUsage" runat="server" /></td>
    <td><asp:HyperLink ID="lnkActive" runat="server" /></td>
    <td class="bl"><asp:Label ID="litVersionCreated" runat="server" Font-Size="Smaller" /></td>
    <td><asp:Label ID="litVersionExpires" runat="server" Font-Size="Smaller" /></td>

    <td style="padding:1px"><asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Right" ImageUrl="~/images/delete.png" ToolTip="Delete this Version" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this Version?')" /></td>
</tr>
