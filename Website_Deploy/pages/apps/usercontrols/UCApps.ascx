<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCApps.ascx.cs" Inherits="pages_Apps_usercontrols_UCApps" %>
<%@ Register tagname="UCApp" tagprefix="uc" src="UCApp.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Application" CommandArgument="AppName" id="btnSortByAppName" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Version" CommandArgument="AppMainVersionId" id="LinkButton3" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Deployments" CommandArgument="InstanceCount" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Versions" CommandArgument="VersionCount" id="LinkButton2" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Files" CommandArgument="TotalBytes_" id="LinkButton4" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Releases" CommandArgument="ReleaseCount" id="LinkButton5" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Created" CommandArgument="AppCreated" id="btnSortByAppCreated" runat="server" OnClick="btnResort_Click" /></th>
            <th style="padding:1px"><asp:ImageButton ID="btnAdd" runat="server" ImageAlign="Right" ImageUrl="~/images/add.png" ToolTip="Add new App" OnClick="btnAdd_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCApp.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="20" />
