<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pages_Apps_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Applications"
%>

<%@ Register src="usercontrols/UCApps.ascx" tagname="UCApps" tagprefix="uc" %>


<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <!-- Search Filters -->
    <table class="filters">
      <tr>
        <td>
          <asp:Textbox ID="txtSearch" Runat="Server"/>
        </td>
        <td>
          <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"  />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCApps ID="ctrlApps" runat="server" OnAddClick="ctrl_AddClick" OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" />
</asp:Content>
