<%@ Control Language="vb" AutoEventWireup="true" CodeFile="UCSessions.ascx.vb" Inherits="pages_Sessions_usercontrols_UCSessions" %>
<%@ Register tagname="UCSession" tagprefix="uc" src="UCSession.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" /></th>
            <th><asp:LinkButton Text="Session" CommandArgument="MinDate" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="User" CommandArgument="SessionUserLoginName" id="btnSortBySession" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Clicks" CommandArgument="ClickCount" id="LinkButton3" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Duration" CommandArgument="MaxDate-MinDate" id="LinkButton2" runat="server" OnClick="btnResort_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCSession.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />
