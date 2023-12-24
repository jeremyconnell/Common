<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Databases.aspx.vb" Inherits="pages_azure_Databases"
MasterPageFile="~/masterpages/deploy.master"
Title="Databases (Azure)"
EnableViewState="false"
%>

<asp:Content ID="c" runat="server" ContentPlaceHolderID="body">
    
    <asp:Table ID="tblDatabases" runat="server" CssClass="datagrid">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>ID</asp:TableHeaderCell>
            <asp:TableHeaderCell>Databases (<asp:Label ID="lblServerName" runat="server" />) </asp:TableHeaderCell>
            <asp:TableHeaderCell>Code</asp:TableHeaderCell>
            <asp:TableHeaderCell>Deploy</asp:TableHeaderCell>
            <asp:TableHeaderCell>Size</asp:TableHeaderCell>
            <asp:TableHeaderCell>Age</asp:TableHeaderCell>
            <asp:TableHeaderCell>&nbsp;</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>

</asp:Content>
    