<%@ Page Language="vb" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="pages_Audit_Errors_default" 
  MasterPageFile="~/pages/default.master"
  Title="Audit_Errors"
  EnableViewState="false"
%>

<%@ Register src="usercontrols/UCAudit_Errors.ascx" tagname="UCAudit_Errors" tagprefix="uc" %>

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
        <td nowrap>
          <asp:CheckBox ID="chkUniqueOnly" runat="server" Text="Unique Errors Only" AutoPostBack="true" />
        </td>
        <td width=100% align=right>
          <asp:Button ID="btnDelete" runat="server" Text="Delete All" OnClientClick="return confirm('Clear the Error Log?')" />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCAudit_Errors ID="ctrlAudit_Errors" runat="server" EnableViewState="false" />
</asp:Content>
