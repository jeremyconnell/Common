<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCRoles.ascx.vb" Inherits="pages_Roles_usercontrols_UCRoles" %>
<%@ Register tagname="UCRole" tagprefix="uc" src="UCRole.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server" />
            <th>Role</th>
            <th>Usage</th>
            <th><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/add.png" ToolTip="Add new Role" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCRole.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />
