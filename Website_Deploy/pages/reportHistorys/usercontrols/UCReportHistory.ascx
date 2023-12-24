<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCReportHistory.ascx.cs" Inherits="pages_reporthistorys_usercontrols_UCReportHistory" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><asp:HyperLink ID="litReportInstanceId" runat="server" /></td>
    <td><asp:HyperLink ID="litReportInitialVersionId" runat="server" /></td>
    <td><asp:HyperLink ID="litReportInitialSchemaMD5" runat="server" /></td>
    <td><asp:Label ID="litReportAppStarted" runat="server" /></td>
    <td><asp:Label ID="litReportAppStopped" runat="server" /></td>

    <td style="padding:1px"><asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Right" ImageUrl="~/images/delete.png" ToolTip="Delete this ReportHistory" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this ReportHistory?')" /></td>
</tr>
