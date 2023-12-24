<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCAudit_Errors.ascx.vb" Inherits="pages_Audit_Errors_usercontrols_UCAudit_Errors" %>
<%@ Register tagname="UCAudit_Error" tagprefix="uc" src="UCAudit_Error.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" ImageAlign="Left" /></th>
            <th><asp:LinkButton id="btnSortByErrorId" runat="server" CommandArgument="ErrorId" Text="ErrorId" /></th>
            <th nowrap><asp:LinkButton id="btnSortByDateCreated" runat="server" CommandArgument="ErrorDateCreated" Text="Date Error Logged" /></th>
            <th><asp:LinkButton id="btnSortByUserName" runat="server" CommandArgument="ErrorUserName" Text="User" /></th>
            <th><asp:LinkButton id="btnSortByMessage" runat="server" CommandArgument="ErrorMessage" Text="Message" /></th>
            <th><asp:LinkButton id="btnSortByInnerMessage" runat="server" CommandArgument="ErrorInnerMessage" Text="Message (Inner)" /></th>
            <th><asp:LinkButton id="btnSortByType" runat="server" CommandArgument="ErrorType" Text="Type" />, <asp:LinkButton id="btnSortByInnerType" runat="server" CommandArgument="ErrorInnerType" Text="InnerType" /></th>
            <th><asp:LinkButton id="btnSortByMachineName" runat="server" CommandArgument="ErrorMachineName" Text="Server" />, <asp:LinkButton id="btnSortByWebsite" runat="server" CommandArgument="ErrorWebsite" Text="Domain" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCAudit_Error.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />
