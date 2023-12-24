<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pages_BinaryFiles_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Binaries"
%>

<%@ Register src="usercontrols/UCBinaryFiles.ascx" tagname="UCBinaryFiles" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndFileCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click"/>
    <span style="font-weight:bold; font-size:smaller">Ver:</span> 
    <asp:DropDownList ID="ddVer" Runat="Server" DataTextField="NameAndFileCount" DataValueField="VersionId" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click"/>
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <style>
        table.datagrid td { padding:2px 10px; }
    </style>
    <!-- Search Filters -->
    <table class="filters">
      <tr>
        <td>
          <asp:Textbox ID="txtSearch" Runat="Server"/>
        </td>
        <td>
          <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"  />
        </td>
          <td>
              <asp:RadioButtonList ID="rbl"       runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click">
                  <asp:ListItem Value="0" Selected="True">Files</asp:ListItem>
                  <asp:ListItem Value="1">Schema</asp:ListItem>
              </asp:RadioButtonList>
          </td>
      </tr>
    </table>

    <asp:PlaceHolder ID="plhNormallyFiltered" runat="server" Visible="false">
        <br />
        <span style="font-weight:bold">One-Time Files:</span> <asp:HyperLink ID="lnkOneTime" runat="server" />
        <uc:UCBinaryFiles ID="ctrlFiltered" runat="server"  PageSize="50" />
    </asp:PlaceHolder>
    
   <!-- Search Results -->
    <uc:UCBinaryFiles ID="ctrlBinaryFiles" runat="server" OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" PageSize="50" />
</asp:Content>

<asp:Content ID="s" runat="server" ContentPlaceHolderID="side">
    <div style="padding-left:5px">        
        <asp:Button ID="btnDeleteUnused" runat="server" Text="Delete Unused Files" OnClientClick="return confirm('Permanently Delete unused files?')" OnClick="btnDeleteUnused_Click" /><br />
        <br />
        <asp:Label ID="lblTotal" runat="server" />
    </div>
</asp:Content>