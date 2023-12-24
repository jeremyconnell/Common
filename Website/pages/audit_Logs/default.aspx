<%@ Page EnableEventValidation="false" EnableViewState="false" Language="vb" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="pages_Audit_Logs_default" 
  MasterPageFile="~/pages/default.master"
  Title="Control Logs"
%>

<%@ Register src="usercontrols/UCAudit_Logs.ascx" tagname="UCAudit_Logs" tagprefix="uc" %>

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
          <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick="return confirm('Delete Logs?')" />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCAudit_Logs ID="ctrlAudit_Logs" runat="server" />        
</asp:Content>
