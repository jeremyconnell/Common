<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCBackupItem.ascx.cs" Inherits="pages_backupitems_usercontrols_UCBackupItem" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><asp:HyperLink ID="lnkCreated" runat="server" Font-Bold="true" /></td>
    <td><asp:HyperLink ID="lnkInstance" Runat="server" /></td>
    <td><asp:HyperLink ID="lnkItemTableName" Runat="server" /></td>
    <td align=right><asp:Label ID="lblItemRowCount" Runat="server" /></td>
    <td align="right"><asp:literal ID="litItemSchema" runat="server" /></td>
    <td align="right"><asp:literal ID="litItemDataset" runat="server" /></td>
    <td><asp:Label ID="litItemSchemaMD5" runat="server" Font-Size="Smaller" /></td>
    <td><asp:Label ID="litItemDatasetMD5" runat="server" Font-Size="Smaller" /></td>
</tr>

