<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pages_Values_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Values"
%>

<%@ Register src="usercontrols/UCValues.ascx" tagname="UCValues" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndGroupCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click"/>
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <!-- Search Filters -->
    <table class="filters">
      <tr>
        <td>
          <asp:DropDownList ID="ddInstance" Runat="Server" DataTextField="IdAndName" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click" />
        </td>
        <td>
          <asp:DropDownList ID="ddKey" Runat="Server" DataTextField="NameAndCount" DataValueField="KeyName" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click" />
        </td>
        <td>
          <asp:Textbox ID="txtSearch" Runat="Server"/>
        </td>
        <td>
          <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"  />
        </td>
        <td>
          <asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click"  />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCValues ID="ctrlValues" runat="server" OnAddClick="ctrl_AddClick" OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" />
</asp:Content>
