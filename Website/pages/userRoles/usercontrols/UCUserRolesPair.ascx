<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCUserRolesPair.ascx.vb" Inherits="pages_UserRoles_usercontrols_UCUserRolesPair" %>
<%@ Register src="UCUserRoles.ascx" tagname="UCUserRoles" tagprefix="uc" %>
<table cellpadding=0 cellspacing=0>
  <tr>
    <td valign="top">
        <uc:UCUserRoles ID="ctrlSelected"  runat="server" QueryString="p2" />
    </td>
  </tr>
  <tr>
    <td colspan=2>
        <asp:TextBox ID=txtSearch runat=server />
        <asp:Button ID=btnSearch runat=server Text="Search" OnClick="btnSearch_Click" />
    </td>
  </tr>
  <tr>
    <td valign="top">
       <uc:UCUserRoles ID="ctrlRemaining" runat="server" QueryString="p1" />
    </td>
  </tr>
</table>
