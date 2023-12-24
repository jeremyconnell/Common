<%@ Page Language="VB" AutoEventWireup="false" CodeFile="group.aspx.vb" Inherits="pages_audit_Errors_group"
  MasterPageFile="~/masterpages/default.master"
  Title="Group of Similar Errors"
  EnableViewState="false"
%>

<%@ Register src="usercontrols/UCAudit_Errors.ascx" tagname="UCAudit_Errors" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
   <!-- Search Results -->
    <uc:UCAudit_Errors ID="ctrlAudit_Errors" runat="server" EnableViewState="false" />
</asp:Content>
