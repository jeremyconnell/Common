<%@ Control Language="vb" AutoEventWireup="true" CodeFile="UCClicks.ascx.vb" Inherits="pages_Clicks_usercontrols_UCClicks" %>
<%@ Register tagname="UCClick" tagprefix="uc" src="UCClick.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" /></th>
            <th><asp:LinkButton Text="Date" CommandArgument="ClickDate" id="btnSortByClick" runat="server" OnClick="btnResort_Click" /></th>
            <th id=colTime runat=server><asp:LinkButton Text="Duration" CommandArgument="ClickTimeSpan" id="LinkButton4" runat="server" OnClick="btnResort_Click" /></th>
            <th id=colUser runat=server><asp:LinkButton Text="User" CommandArgument="SessionUserLoginName" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th id=colHost runat=server><asp:LinkButton Text="Domain" CommandArgument="ClickHost" id="LinkButton3" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Url" CommandArgument="ClickUrl" id="LinkButton2" runat="server" OnClick="btnResort_Click" /></th>
            <th>Querystring</th>
        </tr>
    </thead>
    <tbody>
        <%--UCClick.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />