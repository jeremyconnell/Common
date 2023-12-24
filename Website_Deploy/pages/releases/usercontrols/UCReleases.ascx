<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCReleases.ascx.cs" Inherits="pages_Releases_usercontrols_UCReleases" %>
<%@ Register tagname="UCRelease" tagprefix="uc" src="UCRelease.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Released" CommandArgument="ReleaseCreated" id="btnSortByReleaseCreated" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="App" CommandArgument="ReleaseAppId" id="btnSortByReleaseAppId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Version" CommandArgument="ReleaseVersionId" id="btnSortByReleaseVersionId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Only For" CommandArgument="ReleaseInstanceId" id="btnSortByReleaseInstanceId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Reason" CommandArgument="ReleaseBranchName" id="btnSortByReleaseBranchName" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Expired" CommandArgument="ReleaseExpired" id="btnSortByReleaseExpired" runat="server" OnClick="btnResort_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCRelease.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="20" />
