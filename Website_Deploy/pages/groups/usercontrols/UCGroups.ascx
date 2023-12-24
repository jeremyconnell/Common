<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCGroups.ascx.cs" Inherits="pages_Groups_usercontrols_UCGroups" %>
<%@ Register tagname="UCGroup" tagprefix="uc" src="UCGroup.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Group" CommandArgument="GroupName" id="btnSortByGroupName" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Keys" CommandArgument="KeyCount" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th style="padding:1px"><asp:ImageButton ID="btnAdd" runat="server" ImageAlign="Right" ImageUrl="~/images/add.png" ToolTip="Add new Group" OnClick="btnAdd_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCGroup.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />
