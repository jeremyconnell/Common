<%@ Page EnableEventValidation="false" EnableViewState="false" Language="vb" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="pages_Clicks_default" 
  MasterPageFile="~/masterpages/default.master"
  Title="Clicks"
%>

<%@ Register src="usercontrols/UCClicks.ascx" tagname="UCClicks" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
   <!-- Search Filters -->
    <table class="filters">
      <tr>
        <td>
          <asp:DropDownList id="ddUserName" Runat="Server" AutoPostBack=true />
        </td>
        <td>
          <asp:DropDownList id="ddUrl" Runat="Server" AutoPostBack=true />
        </td>
        <td>
          <asp:CheckBox ID=chkHost runat=server AutoPostBack=true Text="Domain" Font-Size=Smaller />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCClicks ID="ctrlClicks" runat="server" />        
</asp:Content>
