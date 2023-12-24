<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCVersions.ascx.cs" Inherits="pages_Versions_usercontrols_UCVersions" %>
<%@ Register tagname="UCVersion" tagprefix="uc" src="UCVersion.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Version" CommandArgument="VersionName" id="btnSortByVersionName" runat="server" OnClick="btnResort_Click" /></th>
            <th class="bl">Diff</th>
            <th class="br">Size</th>
            <th><asp:LinkButton Text="All Files" CommandArgument="VersionFileCount" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Size" CommandArgument="VersionTotalBytes" id="btnSortByVersionTotalBytes" runat="server" OnClick="btnResort_Click" /></th>
            <th class="bl"><asp:LinkButton Text="Folder" CommandArgument="VersionFilesMD5" id="btnSortByVersionSchemaMD5" runat="server" OnClick="btnResort_Click" /></th>
            <th class="br"><asp:LinkButton Text="Schema" CommandArgument="VersionSchemaMD5" id="LinkButton4" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Assign" CommandArgument="UsageCount" id="LinkButton2" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Active" CommandArgument="ActiveCount" id="LinkButton3" runat="server" OnClick="btnResort_Click" /></th>
            <th class="bl"><asp:LinkButton Text="Created" CommandArgument="VersionCreated" id="btnSortByVersionCreated" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Expires" CommandArgument="VersionExpires" id="btnSortByVersionExpires" runat="server" OnClick="btnResort_Click" /></th>
            <th style="padding:1px"><asp:ImageButton ID="btnAdd" runat="server" ImageAlign="Right" ImageUrl="~/images/add.png" ToolTip="Add new Version" OnClick="btnAdd_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCVersion.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />
