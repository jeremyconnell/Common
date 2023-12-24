<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pages_Instances_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Instances"
%>

<%@ Register src="usercontrols/UCInstances.ascx" tagname="UCInstances" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged"/>
    <span style="font-weight:bold; font-size:smaller">Client:</span> 
    <asp:DropDownList ID="ddClient" runat="server" DataTextField="NameAndInstanceCount" DataValueField="ClientId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click"/>
</asp:Content>
<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <!-- Search Filters -->
    <table class="filters" width="100%">
      <tr>
        <td>
          <asp:Textbox ID="txtSearch" Runat="Server"/>
        </td>
        <td>
          <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"  />
        </td>
        <td align="right" width="100%">
            <asp:Button ID="btnTapAll" runat="server" Text="Tap All (inspect)" OnClientClick="return confirm('Inspect all deployments?')" OnClick="btnTapAll_Click" />
            <asp:Button ID="btnFixAll" runat="server" Text="Fix All (Bin+Sch)" OnClientClick="return confirm('Manually fix all versions (Binaries+Schema)?')" OnClick="btnFixAll_Click"  />
        </td>
      </tr>
    </table>
    
    <uc:UCInstances ID="ctrlNewInstances" runat="server" OnAddClick="ctrl_AddClick" OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" />
   <!-- Search Results -->
    <uc:UCInstances ID="ctrlInstances" runat="server" OnAddClick="ctrl_AddClick" OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" />
</asp:Content>
