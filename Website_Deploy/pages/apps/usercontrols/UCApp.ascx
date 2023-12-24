<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCApp.ascx.cs" Inherits="pages_apps_usercontrols_UCApp" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td>
        <b><asp:HyperLink ID="lnkAppName" Runat="server" Font-Size="Larger" /></b>
        <div style="margin-top:5px"><asp:literal ID="litMainVerFiles" runat="server" /></div>

    </td>
    <td>
        <div style="margin-top:2px"><asp:HyperLink ID="litAppMainVersionId" runat="server" Font-Bold="true" /></div>
        <div style="margin-top:3px"><asp:HyperLink ID="lnkBranches" Runat="server" /></div>
    </td>
    <td align="left">
        <asp:HyperLink ID="lnkInstances" Runat="server" Font-Bold="true" /> 
        <ul ID="pnlInstances" runat="server" style="max-height:150px; overflow:auto" />
    </td>
    <td align="left">
        <asp:HyperLink ID="lnkVersions" Runat="server" Font-Bold="true" /> 
        <ul ID="pnlVersions" runat="server" style="max-height:150px; overflow:auto;" />
    </td>
    <td>
        <asp:HyperLink ID="lnkFiles" Runat="server" Font-Bold="true" />
        <ul ID="pnlFiles" runat="server" style="max-height:150px; overflow:auto; margin-bottom:5px" />
        Keep: <asp:literal ID="litAppKeepOldFilesForDays" runat="server" />
    </td>
    <td>
        <asp:HyperLink ID="lnkReleases" Runat="server" Font-Bold="true" />
        <ul ID="pnlReleases" runat="server" style="max-height:150px; overflow:auto; margin-bottom:5px" />
    </td>

    <td><asp:literal ID="litAppCreated" runat="server" /></td>

    <td style="padding:1px"><asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Right" ImageUrl="~/images/delete.png" ToolTip="Delete this App" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this App?')" /></td>
</tr>
