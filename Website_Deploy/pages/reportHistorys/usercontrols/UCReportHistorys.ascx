<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCReportHistorys.ascx.cs" Inherits="pages_ReportHistorys_usercontrols_UCReportHistorys" %>
<%@ Register tagname="UCReportHistory" tagprefix="uc" src="UCReportHistory.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Instance" CommandArgument="ReportInstanceId" id="btnSortByReportInstanceId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Version" CommandArgument="ReportInitialVersionId" id="btnSortByReportInitialVersionId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Schema" CommandArgument="ReportInitialSchemaMD5" id="btnSortByReportInitialSchemaMD5" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="App-Start" CommandArgument="ReportAppStarted" id="btnSortByReportAppStarted" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="App-Stop" CommandArgument="ReportAppStopped" id="btnSortByReportAppStopped" runat="server" OnClick="btnResort_Click" /></th>
            <th style="padding:1px"><asp:ImageButton ID="btnAdd" runat="server" ImageAlign="Right" ImageUrl="~/images/add.png" ToolTip="Add new ReportHistory" OnClick="btnAdd_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCReportHistory.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="20" />
