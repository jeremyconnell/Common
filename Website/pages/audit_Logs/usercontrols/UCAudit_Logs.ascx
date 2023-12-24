<%@ Control Language="vb" AutoEventWireup="true" CodeFile="UCAudit_Logs.ascx.vb" Inherits="pages_Audit_Logs_usercontrols_UCAudit_Logs" %>
<%@ Register tagname="UCAudit_Log" tagprefix="uc" src="UCAudit_Log.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server">#</th>
            <th><asp:LinkButton Text="Created" CommandArgument="LogCreated" id="btnSortByCreated" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Type" CommandArgument="LogTypeId" id="btnSortByType" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Message" CommandArgument="LogMessage" id="btnSortByMessage" runat="server" OnClick="btnResort_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCAudit_Log.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />