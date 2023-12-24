<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Audit_Errors.aspx.vb" Inherits="pages_instances_Audit_Errors"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Audit_Errors"
    ValidateRequest="False"
%>

<%@ Register src="~/pages/audit_errors/usercontrols/UCAudit_Errors.ascx" tagname="UCAudit_Errors" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged" />

    <span style="font-weight:bold; font-size:smaller">Deploy:</span> 
    <asp:DropDownList ID="ddIns" Runat="Server" DataTextField="NameAndSuffix" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddIns_SelectedIndexChanged" />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
   <!-- Search Filters -->
    <table class="filters">
      <tr>
        <td>
          <asp:Textbox ID="txtSearch" Runat="Server"/>
        </td>
        <td>
          <asp:Button ID="btnSearch" runat="server" Text="Search" />
        </td>
        <td nowrap>
          <asp:CheckBox ID="chkUniqueOnly" runat="server" Text="Unique Errors Only" AutoPostBack="true" />
        </td>
        <td width=100% align=right>
          <asp:Button ID="btnDelete" runat="server" Text="Delete All" OnClientClick="return confirm('Clear the Error Log?')" />
        </td>
      </tr>
    </table>
    
   <!-- Search Results -->
    <uc:UCAudit_Errors ID="ctrlAudit_Errors" runat="server" EnableViewState="false" />
</asp:Content>
