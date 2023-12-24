<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pages_Releases_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Releases"
%>

<%@ Register src="usercontrols/UCReleases.ascx" tagname="UCReleases" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndReleaseCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged"/>
    <asp:DropDownList ID="ddIns" Runat="Server" DataTextField="NameAndReleaseCount" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click"/>
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
    <uc:UCReleases ID="ctrlReleases" runat="server"  OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" />
</asp:Content>
