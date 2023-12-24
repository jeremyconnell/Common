<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pages_UpgradeHistorys_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Self-Upgrades"
%>

<%@ Register src="usercontrols/UCUpgradeHistorys.ascx" tagname="UCUpgradeHistorys" tagprefix="uc" %>


<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged"/>
    <asp:DropDownList ID="ddIns" Runat="Server" DataTextField="IdAndName" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click"/>
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
    <uc:UCUpgradeHistorys ID="ctrlUpgradeHistorys" runat="server" OnAddClick="ctrl_AddClick" OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" />
</asp:Content>
