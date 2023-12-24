<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCBackups.ascx.cs" Inherits="pages_Backups_usercontrols_UCBackups" %>
<%@ Register tagname="UCBackup" tagprefix="uc" src="UCBackup.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="" style="min-width:600px">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Created" CommandArgument="BackupCreated" id="btnSortByBackupCreated" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Instance" CommandArgument="IdAndName" id="btnSortByBackupInstanceId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Tables" CommandArgument="BackupTableCount" id="LinkButton2" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Stored" CommandArgument="CountTables" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th style="text-align:right"><asp:LinkButton Text="Size:" CommandArgument="TotalSize" id="btnSize" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Description" CommandArgument="BackupDescription" id="btnSortByBackupDescription" runat="server" OnClick="btnResort_Click" /></th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        <%--UCBackup.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="20" />
