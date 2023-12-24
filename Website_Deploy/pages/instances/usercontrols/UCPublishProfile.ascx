<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCPublishProfile.ascx.vb" Inherits="pages_instances_usercontrols_UCPublishProfile" %>
<%@ Register tagname="UCDatabase" tagprefix="uc" src="UCDatabase.ascx" %>
<table class="datagrid" width="420">
    <tr>
        <th><asp:Label ID="litTitle" runat="server">Publish</asp:Label></th>
        <th>
            <asp:Literal ID="litName" runat="server" />
        </th>
    </tr>
    <tr id="colDest" runat="server">
        <th>Dest.App</th>
        <td><asp:HyperLink ID="lnkDestApp" runat="server" /></td>
    </tr>
    <tr id="colForum" runat="server" visible="false">
        <th>Forum</th>
        <td><asp:HyperLink ID="lnkForum" runat="server" /></td>
    </tr>
    <tr>
        <th>Url</th>
        <td><asp:Label ID="litPubUrl" runat="server" /></td>
    </tr>
    <tr>
        <th>User</th>
        <td><asp:Literal ID="litUser" runat="server" /></td>
    </tr>
    <tr>
        <th>Password</th>
        <td><asp:Literal ID="litPassword" runat="server" /></td>
    </tr>
    <tr id="colConnStr" runat="server">
        <th>Conn.Str</th>
        <td style="max-height:400px; overflow:auto"><asp:Literal ID="litConnStr" runat="server" /></td>
    </tr>
<asp:PlaceHolder ID="plh" runat="server" />
</table>