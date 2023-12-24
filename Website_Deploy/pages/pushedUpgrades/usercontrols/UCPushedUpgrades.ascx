<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCPushedUpgrades.ascx.cs" Inherits="pages_PushedUpgrades_usercontrols_UCPushedUpgrades" %>
<%@ Register tagname="UCPushedUpgrade" tagprefix="uc" src="UCPushedUpgrade.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Admin" CommandArgument="PushUserName" id="btnSortByPushUserName" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Instance" CommandArgument="PushInstanceId" id="btnSortByPushInstanceId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Old-Ver" CommandArgument="PushOldVersionId" id="btnSortByPushOldVersionId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="New-Ver" CommandArgument="PushNewVersionId" id="btnSortByPushNewVersionId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Old-Sch" CommandArgument="PushOldSchemaMD5" id="btnSortByPushOldSchemaMD5" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="New-Sch" CommandArgument="PushNewSchemaMD5" id="btnSortByPushNewSchemaMD5" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Started" CommandArgument="PushStarted" id="btnSortByPushStarted" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Completed" CommandArgument="PushCompleted" id="btnSortByPushCompleted" runat="server" OnClick="btnResort_Click" /></th>
            <th style="padding:1px"><asp:ImageButton ID="btnAdd" runat="server" ImageAlign="Right" ImageUrl="~/images/add.png" ToolTip="Add new PushedUpgrade" OnClick="btnAdd_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCPushedUpgrade.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="20" />
