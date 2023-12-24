<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pages_BackupItems_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Datasets"
%>

<%@ Register src="usercontrols/UCBackupItems.ascx" tagname="UCBackupItems" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
   <!-- Search Filters -->
    <table class="filters">
      <tr>
        <td>
            <asp:DropDownList id="ddApp" runat="server" DataTextField="AppName" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click" />
        </td>
        <td>
            <asp:DropDownList id="ddInstance" runat="server" DataTextField="NameAndSuffix" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click" />
        </td>
        <td>
            <asp:DropDownList id="ddTable" runat="server" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click" />
        </td>
        <td>
          <asp:Textbox ID="txtSearch" Runat="Server"/>
        </td>
        <td>
          <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCBackupItems ID="ctrlBackupItems" runat="server" OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" />
</asp:Content>

