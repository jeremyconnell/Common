<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pages_Backups_default" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Backups"
Async="true"
%>

<%@ Register src="usercontrols/UCBackups.ascx" tagname="UCBackups" tagprefix="uc" %>

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
          <asp:Textbox ID="txtSearch" Runat="Server"/>
        </td>
        <td>
          <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"  />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCBackups ID="ctrlBackups" runat="server"  OnExportClick="ctrl_ExportClick" OnResortClick="ctrl_ResortClick" />
</asp:Content>


<asp:Content ID="b" runat="server" ContentPlaceHolderId="side">
    <div style="margin-left:10px; margin-top:50px">
        <asp:Button ID="btnBackupAll" runat="server" Text="Backup-DB" OnClientClick="return confirm('Take a backup of ALL databases?')" OnClick="btnBackupAll_Click" /><Br />
        <Br />
        <asp:Button ID="btnBackupIns" runat="server" Text="Backup-DB (1)" OnClientClick="return confirm('Take a backup of THIS database?')" OnClick="btnBackupIns_Click" style="margin-bottom:30px"/><br />

        <label>Exclude Schema:</label><br />
        <asp:TextBox ID="txtExclude" runat="server" Font-Size="Smaller" Text="dbo" Width="100" /><br />
        <br />
        <br />
        <asp:Button ID="btnDeleteOld" runat="server" Text="Delete Old" OnClick="btnDeleteOld_Click" />
        <asp:DropDownList ID="ddKeep" runat="server">
            <asp:ListItem Value="0">0</asp:ListItem>
            <asp:ListItem Value="1">1</asp:ListItem>
            <asp:ListItem Value="2">2</asp:ListItem>
            <asp:ListItem Value="3">3</asp:ListItem>
            <asp:ListItem Value="4">4</asp:ListItem>
            <asp:ListItem Value="5" Selected="True">5</asp:ListItem>
            <asp:ListItem Value="6">6</asp:ListItem>
            <asp:ListItem Value="7">7</asp:ListItem>
        </asp:DropDownList>
    </div>

</asp:Content>
