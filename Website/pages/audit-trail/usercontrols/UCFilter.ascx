<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCFilter.ascx.vb" Inherits="usercontrols_audit_trail_UCFilter" %>
<tr>
  <td class=filterValue>
    <asp:Literal ID=litName runat=server />
  </td>
  <td  class=filterValue>
    <asp:TextBox ID=txtValue runat=server AutoPostBack="true" Width="125" onfocus="select()" />
  </td>
  <td style="padding:1px">
    <asp:ImageButton ID=btnDelete runat=server ImageUrl="~/images/delete.jpg" ToolTip="Remove this Custom Search Filter" />
  </td>
</tr>
