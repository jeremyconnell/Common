<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCValues.ascx.cs" Inherits="pages_Values_usercontrols_UCValues" %>
<%@ Register tagname="UCValue" tagprefix="uc" src="UCValue.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Deployment" CommandArgument="ValueInstanceId" id="btnSortByValueInstanceId" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Group/Key"  CommandArgument="GroupAndKey" id="btnSortByValueKeyName" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Value"      CommandArgument="ValueAsString" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th style="padding:1px"><asp:ImageButton ID="btnAdd" runat="server" ImageAlign="Right" ImageUrl="~/images/add.png" ToolTip="Add new Value" OnClick="btnAdd_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCValue.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />
