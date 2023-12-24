<%@ Page Language="VB" AutoEventWireup="false" CodeFile="data.aspx.vb" Inherits="pages_self_data"
  MasterPageFile="~/masterpages/deploy.master"
  Title="Data Sync (Admin)"
  EnableViewState="false"
  EnableEventValidation="false"
%>

<%@ Register src="usercontrols/UCDataDiff.ascx" tagname="DataDiff" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    
    <uc:DataDiff id="ctrl" runat="server" Admin="true" />


</asp:Content>
