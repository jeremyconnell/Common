<%@ Page EnableEventValidation="false" EnableViewState="false" Language="vb" AutoEventWireup="false" CodeFile="session.aspx.vb" Inherits="pages_Sessions_Session"
    MasterPageFile="~/masterpages/default.master"
    Title="Add or Change a Session."
    ValidateRequest="False"
%>
<%@ Register src="../clicks/usercontrols/UCClicks.ascx" tagname="UCClicks" tagprefix="uc" %>
<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:UCClicks ID="ctrlClicks" runat="server" />  
    
</asp:Content>
