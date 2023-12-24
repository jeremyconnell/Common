<%@ Page Language="vb" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="pages_Roles_default" 
  MasterPageFile="~/pages/default.master"
  Title="Roles"
%>

<%@ Register src="usercontrols/UCRoles.ascx" tagname="UCRoles" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <!-- Filters etc go here -->
    <uc:UCRoles ID="ctrlRoles" runat="server" />        
</asp:Content>
