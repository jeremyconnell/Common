<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Data.aspx.cs" Inherits="pages_apps_Data"
  MasterPageFile="~/masterpages/deploy.master"
  Title="Data"
%>

<%@ Register src="~/pages/self/usercontrols/UCSchemaDiff.ascx" tagname="SchemaDiff" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    
    <asp:Table ID="tbl" runat="server" CssClass="datagrid" />

</asp:Content>
