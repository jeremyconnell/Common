<%@ Page EnableEventValidation="false" EnableViewState="false" Language="vb" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="pages_Statuss_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Status"
%>

<%@ Register src="usercontrols/UCStatuss.ascx" tagname="UCStatuss" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
   <!-- Search Filters -->
    <table class="filters">
      <tr>
        <td>
          <asp:Textbox ID="txtSearch" Runat="Server"/>
        </td>
        <td>
          <asp:Button ID="btnSearch" runat="server" Text="Search" />
        </td>
        <td>
          <asp:Button ID="btnCreate" runat="server" Text="Create"  />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCStatuss ID="ctrlStatuss" runat="server" />        
</asp:Content>
