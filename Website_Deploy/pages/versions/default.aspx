<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pages_Versions_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Versions"
%>

<%@ Register src="usercontrols/UCVersions.ascx" tagname="UCVersions" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndVersionCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged"/>
</asp:Content>

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
    <uc:UCVersions ID="ctrlVersions" runat="server" OnAddClick="ctrl_AddClick" OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" />
</asp:Content>
