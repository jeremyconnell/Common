<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Websites.aspx.vb" Inherits="pages_azure_Websites"
MasterPageFile="~/masterpages/deploy.master"
Title="Websites (Azure)"
EnableViewState="false"
%>

<asp:Content ID="c" runat="server" ContentPlaceHolderID="body">

    <asp:Button ID="btnRestartAll" runat="server" Text="Restart All" OnClientClick="return confirm('Restart all Web-Apps?')"  />

    <asp:Table ID="tblWebApps" runat="server" CssClass="datagrid">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>#</asp:TableHeaderCell>
            <asp:TableHeaderCell>WebApps (<asp:Label ID="lblWebSpaceName" runat="server" />)</asp:TableHeaderCell>
            <asp:TableHeaderCell>Code</asp:TableHeaderCell>
            <asp:TableHeaderCell>Suffix</asp:TableHeaderCell>
            <asp:TableHeaderCell>Deploy</asp:TableHeaderCell>
            <asp:TableHeaderCell>State</asp:TableHeaderCell>
            <asp:TableHeaderCell>Avail</asp:TableHeaderCell>
            <asp:TableHeaderCell>Usage</asp:TableHeaderCell>
            <asp:TableHeaderCell>&nbsp;</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>

</asp:Content>
  