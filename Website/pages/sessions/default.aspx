<%@ Page EnableEventValidation="false" EnableViewState="false" Language="vb" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="pages_Sessions_default" 
  MasterPageFile="~/pages/default.master"
  Title="Sessions"
%>

<%@ Register src="usercontrols/UCSessions.ascx" tagname="UCSessions" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
   <!-- Search Filters -->
    <table class="filters">
      <tr>
        <td>
          <asp:DropDownList id="ddUserName" Runat="Server" AutoPostBack=true />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCSessions ID="ctrlSessions" runat="server" />        
</asp:Content>
