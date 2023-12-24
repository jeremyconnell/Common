<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCKeys.ascx.cs" Inherits="pages_Keys_usercontrols_UCKeys" %>
<%@ Register tagname="UCKey" tagprefix="uc" src="UCKey.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Key" CommandArgument="KeyName" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Group" CommandArgument="KeyGroupId" id="btnSortByKeyGroupId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Type" CommandArgument="KeyFormatId" id="btnSortByKeyFormatId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Default" CommandArgument="DefaultValue" id="LinkButton3" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Distinct" CommandArgument="Distinct" id="LinkButton4" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Usage" CommandArgument="Usage" id="LinkButton2" runat="server" OnClick="btnResort_Click" /></th>
            <th style="padding:1px"><asp:ImageButton ID="btnAdd" runat="server" ImageAlign="Right" ImageUrl="~/images/add.png" ToolTip="Add new Key" OnClick="btnAdd_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCKey.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />
