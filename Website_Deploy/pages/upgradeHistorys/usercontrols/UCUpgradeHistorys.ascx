<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCUpgradeHistorys.ascx.cs" Inherits="pages_UpgradeHistorys_usercontrols_UCUpgradeHistorys" %>
<%@ Register tagname="UCUpgradeHistory" tagprefix="uc" src="UCUpgradeHistory.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Report" CommandArgument="ChangeReportId" id="btnSortByReport" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="NewVersion" CommandArgument="ChangeNewVersionId" id="btnSortByNewVersion" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="NewSchemaMD5" CommandArgument="ChangeNewSchemaMD5" id="btnSortByNewSchemaMD5" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Started" CommandArgument="ChangeStarted" id="btnSortByStarted" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Finished" CommandArgument="ChangeFinished" id="btnSortByFinished" runat="server" OnClick="btnResort_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCUpgradeHistory.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="20" />
