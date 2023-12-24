<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCBinaryFile.ascx.cs" Inherits="pages_binaryfiles_usercontrols_UCBinaryFile" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td>
        <asp:Label ID="litPath" runat="server" Font-Names="Arial" />
        <asp:HyperLink ID="lnkSchema" runat="server" />
        <asp:PlaceHolder ID="plhAlso" runat="server" />
    </td>
    <td align="right"><asp:LinkButton ID="litSize" runat="server"  OnClick="litPath_Click" /></td>
    <td id="colUsg" runat="server"><asp:PlaceHolder ID="plh" runat="server" /></td>
    <td nowrap><asp:Label ID="litCreated" runat="server" Font-Size="Smaller" /></td>
    <td id="colDel" runat="server"><asp:Label ID="litDeleted" runat="server" /></td>
    <td ><asp:Label ID="lblMd5" runat="server" Font-Size="Smaller" /></td>
</tr>
