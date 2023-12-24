<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCUserRole.ascx.vb" Inherits="pages_UserRoles_usercontrols_UCUserRole" %>
<tr id="row" runat="server">
  <td><strong><asp:Literal ID=litNum runat=server />.</strong></td>
  <td>
    <asp:HyperLink ID="lnkTarget" runat="server" />
  </td>
  <td id=colDateCreated runat=server visible=false>
    <asp:Label ID=lblDateCreated runat=server />
  </td>
  <td style="text-align:right">
    <asp:CheckBox ID="chk" runat="server" AutoPostBack="true" />
  </td>
</tr>
