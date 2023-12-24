<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCMenu.ascx.vb" Inherits="usercontrols_UCMenu" %>
<div class="menu">
    <table width=100% cellpadding=0 cellspacing=0>
        <tr>
            <td align=left class=left>
                <asp:PlaceHolder ID=plhLeft runat=server />
            </td>
            <td align=center width=100%>
                <asp:Label ID=lblTitle runat=server />
            </td>
            <td align=right nowrap>
                <asp:PlaceHolder ID=plhRight runat=server />
                <asp:LinkButton ID=btnLogout runat=server Text="Logout" OnClientClick="return confirm('Do you wish to logout from the Application?\nThis will end your session.')" />
            </td>
        </tr>
    </table>
</div>