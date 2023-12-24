<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCBackupItems.ascx.cs" Inherits="pages_BackupItems_usercontrols_UCBackupItems" %>
<%@ Register tagname="UCBackupItem" tagprefix="uc" src="UCBackupItem.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Created" CommandArgument="BackupCreated" id="btnSortByBackup" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Instance" CommandArgument="InstanceClientName" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Table" CommandArgument="ItemTableName" id="btnSortByBackupItem" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Rows" CommandArgument="ItemRowCount" id="LinkButton2" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Sch." CommandArgument="SchemaBytes" id="btnSortBySchemaXmlGz" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Data" CommandArgument="DataBytes" id="btnSortByDatasetXmlGz" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Sch.Md5" CommandArgument="ItemSchemaMD5" id="btnSortBySchemaMD5" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Dat.Md5" CommandArgument="ItemDatasetMD5" id="btnSortByDatasetMD5" runat="server" OnClick="btnResort_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCBackupItem.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="20" />
