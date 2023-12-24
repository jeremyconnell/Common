<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="ConfigSettings"
MasterPageFile="~/pages/default.master"
Title="Configuration Settings"
 %>
<%@ Register src="usercontrols/UCGroup.ascx" tagname="UCGroup" tagprefix="uc" %>

<asp:Content ID="h" runat="server" ContentPlaceHolderID="head">
  <style type="text/css">
  .control label { width: 300px; }
  </style>
</asp:Content>

<asp:Content ID="c" runat="server" ContentPlaceHolderID="body">
  <table class=datagrid width="600" cellpadding="0" cellspacing="0">
    <colgroup>
        <col />
        <col />
        <col />
        <col />
        <col width="30" />
    </colgroup>
    <asp:PlaceHolder ID="plh" runat="server" />
    <thead>
        <tr>
            <th colspan="4" style="text-align:right"><asp:Button ID="btnSave" runat="server" Text="Save Settings" /></th>
            <th><asp:HyperLink ID="lnkAdd" runat="server" ImageUrl="~/images/add.png" ToolTip="Add New Group" /> </th>
        </tr>
    </thead>
  </table>
  
  

</asp:Content>
