<%@ Page Language="VB" AutoEventWireup="false" CodeFile="schema.aspx.vb" Inherits="pages_self_schema"
  MasterPageFile="~/masterpages/deploy.master"
  Title="Schema Sync (Admin)"
%>

<%@ Register src="usercontrols/UCSchemaDiff.ascx" tagname="SchemaDiff" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    

    <uc:SchemaDiff id="ctrl" runat="server" />


</asp:Content>
