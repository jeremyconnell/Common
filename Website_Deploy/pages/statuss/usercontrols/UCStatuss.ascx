<%@ Control Language="vb" AutoEventWireup="true" CodeFile="UCStatuss.ascx.vb" Inherits="pages_Statuss_usercontrols_UCStatuss" %>
<%@ Register tagname="UCStatus" tagprefix="uc" src="UCStatus.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" /></th>
            <th><asp:LinkButton Text="Status" CommandArgument="StatusName" id="btnSortByStatus" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Description" CommandArgument="StatusDescription" id="btnSortByDescription" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Clients" CommandArgument="ClientCount" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th style="padding:1px"><asp:ImageButton ID="btnAdd" runat="server" ImageAlign="Right" ImageUrl="~/images/add.png" ToolTip="Add new Status" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCStatus.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />
