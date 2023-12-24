<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pages_Clients_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Clients"
  Async="true"
    ValidateRequest="false"
%>

<%@ Register src="usercontrols/UCClients.ascx" tagname="UCClients" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <!-- Search Filters -->
    <table class="filters" width="100%">
      <tr>
        <td>
          <asp:DropDownList ID="ddStatus" Runat="Server" DataTextField="StatusNameAndCount" DataValueField="StatusId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click" />
        </td>
        <td>
          <asp:Textbox ID="txtSearch" Runat="Server"/>
        </td>
        <td>
          <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"  />
        </td>
        <td width="100%" align="right">
          <asp:Button ID="btnTouch"  runat="server" Text="Touch-All"  OnClick="btnTouch_Click"  OnClientClick="return confirm('Tap API for each client?\nThis will start the application')" />
          <asp:Button ID="btnImport" runat="server" Text="Import-All" OnClick="btnImport_Click" OnClientClick="return confirm('Read settings from all clients?\nThis will overwrite the copy on Admin')" />
          <asp:Button ID="btnExport" runat="server" Text="Export-All" OnClick="btnExport_Click" OnClientClick="return confirm('Write settings to all clients?\nThis will overwrite actual Client settings')" />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCClients ID="ctrlClients" runat="server" OnAddClick="ctrl_AddClick" OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" />

    <pre style="border:2px solid #aaa; max-height:400px; overflow:auto" id="pre" runat="server" visible="false"/>
</asp:Content>