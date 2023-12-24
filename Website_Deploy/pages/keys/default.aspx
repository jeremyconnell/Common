<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pages_Keys_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Keys"
%>

<%@ Register src="usercontrols/UCKeys.ascx" tagname="UCKeys" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndGroupCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click"/>


</asp:Content>


<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <!-- Search Filters -->
    <table class="filters" width="100%">
      <tr>
        <td>
            <asp:DropDownList ID="ddGroup" Runat="Server" DataTextField="NameAndCount" DataValueField="GroupId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click" />
        </td>
        <td>
          <asp:DropDownList ID="ddFormat" Runat="Server" DataTextField="NameAndCount" DataValueField="FormatId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click" />
        </td>
        <td>
          <asp:Textbox ID="txtSearch" Runat="Server"/>
        </td>
        <td>
          <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"  />
        </td>
        <td align="right" width="100%">
          <asp:Button ID="btnSetDefaults" runat="server" Text="Set Defaults" OnClick="btnSetDefaults_Click"  />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCKeys ID="ctrlKeys" runat="server" OnAddClick="ctrl_AddClick" OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" />
</asp:Content>
