<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCSchemaDiff.ascx.vb" Inherits="pages_self_usercontrols_UCSchemaDiff" %>


<div>
    <span style="font-size:smaller; font-weight:bold">Reference: </span>
    <asp:DropDownList id="ddRefInstance" runat="server" AutoPostBack="true" DataTextField="NameAndSuffix" DataValueField="InstanceId" />
    <asp:Button ID="btnRefreshSchema" runat="server" Text="Refresh" Font-Size="Smaller" />
</div>

<asp:Table ID="tbl" runat="server" CssClass="datagrid" style="margin-bottom:40px" Width="900px">
    <asp:TableHeaderRow>
        <asp:TableHeaderCell>#</asp:TableHeaderCell>
        <asp:TableHeaderCell>Deploy</asp:TableHeaderCell>
        <asp:TableHeaderCell ID="cellBin" runat="server" Visible="false">Bin</asp:TableHeaderCell>
        <asp:TableHeaderCell>Schema</asp:TableHeaderCell>
        <asp:TableHeaderCell>Tables</asp:TableHeaderCell>
        <asp:TableHeaderCell>Views</asp:TableHeaderCell>
        <asp:TableHeaderCell>Columns</asp:TableHeaderCell>
        <asp:TableHeaderCell>Procs/Fns</asp:TableHeaderCell>
        <asp:TableHeaderCell>Indexs</asp:TableHeaderCell>
        <asp:TableHeaderCell>Keys</asp:TableHeaderCell>
        <asp:TableHeaderCell>Defs</asp:TableHeaderCell>
        <asp:TableHeaderCell>Mig</asp:TableHeaderCell>
        <asp:TableHeaderCell>View</asp:TableHeaderCell>
        <asp:TableHeaderCell>Diff</asp:TableHeaderCell>
        <asp:TableHeaderCell>Fix</asp:TableHeaderCell>
    </asp:TableHeaderRow>
</asp:Table>