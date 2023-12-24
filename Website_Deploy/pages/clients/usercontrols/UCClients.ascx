<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCClients.ascx.cs" Inherits="pages_Clients_usercontrols_UCClients" %>
<%@ Register tagname="UCClient" tagprefix="uc" src="UCClient.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Client" CommandArgument="ClientName" id="btnSortByClientName" runat="server" OnClick="btnResort_Click" /></th>
            <th class="bl"><asp:LinkButton Text="Status" CommandArgument="ClientStatusId" id="btnSortByClientStatusId" runat="server" OnClick="btnResort_Click" /></th>
            <th class="bl"><asp:LinkButton Text="Code" CommandArgument="ClientSubdomain" id="btnSortByClientSubdomain" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Deployments" CommandArgument="InstanceCount" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Details" CommandArgument="InstanceCount" id="LinkButton3" runat="server" OnClick="btnResort_Click" /></th>
            <th class="bl"><asp:LinkButton Text="Trial-Start" CommandArgument="ClientTrialStarted" id="btnSortByClientTrialStarted" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Trial-End" CommandArgument="ClientTrialEnded" id="btnSortByClientTrialEnded" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="In-Prod" CommandArgument="ClientProductionStarted" id="btnSortByClientProductionStarted" runat="server" OnClick="btnResort_Click" /></th>
            <th style="padding:1px"><asp:ImageButton ID="btnAdd" runat="server" ImageAlign="Right" ImageUrl="~/images/add.png" ToolTip="Add new Client" OnClick="btnAdd_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCClient.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />
