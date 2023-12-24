<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCUsers.ascx.vb" Inherits="pages_Users_usercontrols_UCUsers" %>
<%@ Register tagname="UCUser" tagprefix="uc" src="UCUser.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" /></th>
            <th><asp:LinkButton Text="Login" CommandArgument="UserLogin" id="btnSortByUserLogin" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="FirstName" CommandArgument="UserFirstName" id="btnSortByUserFirstName" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="LastName" CommandArgument="UserLastName" id="btnSortByUserLastName" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Email" CommandArgument="UserEmail" id="btnSortByUserEmail" runat="server" OnClick="btnResort_Click" /></th>
            <th>Roles</th>
            <th><asp:LinkButton Text="Last Login" CommandArgument="UserLastLoginDate" id="btnSortByUserLastLoginDate" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Disabled" CommandArgument="UserIsDisabled" id="btnSortByUserIsDisabled" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Locked Out" CommandArgument="UserIsLockedOut" id="btnSortByUserIsLockedOut" runat="server" OnClick="btnResort_Click" /></th>
            <th style="padding:1px"><asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/add.png" ToolTip="Add new User" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCUser.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />
