<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCYearMonth.ascx.vb" Inherits="usercontrols_UCYearMonth" %>
<table cellpadding=0 cellspacing=0>
    <tr>
        <td><asp:Label ID=lbl runat=server Font-Size=Smaller /></td>
        <td><asp:DropDownList ID=ddYear runat=server AutoPostBack=true /></td>
        <td><asp:DropDownList ID=ddMonth runat=server AutoPostBack=true /></td>
    </tr>
</table>