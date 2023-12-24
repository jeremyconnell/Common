<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCBackup.ascx.cs" Inherits="pages_backups_usercontrols_UCBackup" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><asp:HyperLink ID="lnkBackupCreated" runat="server" Font-Bold="true" /></td>
    <td><asp:HyperLink ID="lnkBackupInstanceId" runat="server" /></td>
    <td align="right"><asp:HyperLink ID="lnkTables" runat="server" /></td>
    <td align="right"><asp:Label ID="lblStored" runat="server" /></td>
    <td align="right"><asp:Label ID="lblTotalSize" runat="server"/></td>
    <td><asp:Label ID="litBackupDescription" runat="server" Font-Size="Smaller" /></td>
    <td><asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Right" ImageUrl="~/images/delete.png" ToolTip="Delete this Backup" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this Backup?')" /></td>
</tr>
