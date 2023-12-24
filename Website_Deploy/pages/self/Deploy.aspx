<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Deploy.aspx.vb" Inherits="pages_self_Deploy" 
  MasterPageFile="~/masterpages/deploy.master"
  Title="Self-Deployment"
%>


<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <table class="datagrid">
        <tr>
            <th colspan="4">Self-Deployment Utility</th>
        </tr>
        <tr>
            <th>Build</th>
            <td colspan="2"><asp:TextBox ID="txtDev" runat="server" Width="260" Font-Size ="Smaller"/></td>
            <td><asp:CheckBox ID="chkPublish" runat="server" Text="Release" AutoPostBack="true"/></td>
        </tr>
        <tr>
            <th>Exclude</th>
            <td colspan="2"><asp:TextBox ID="txtExclude" runat="server" Width="260" Font-Size ="Smaller"/></td>
            <td></td>
        </tr>
        <tr>
            <th>Deploy</th>
            <td colspan="2"><asp:TextBox ID="txtHost" runat="server" Width="260" Font-Size ="Smaller"/></td>
            <td><asp:Button ID="btnCheck" runat="server" Text="Apply" Font-Size="Smaller" /></td>
        </tr>
        <tr>
            <th>Current</th>
            <td><asp:Label ID="lblHash" runat="server"  Font-Bold="true"  /></td>
            <td><asp:Button ID="btnRefresh" runat="server" Text="Refresh" Font-Size="Smaller" />  </td>
            <td><asp:Label id="lblLocalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
        <tr>
            <th>Remote</th>
            <td><asp:Label ID="lblRemote" runat="server"  Font-Bold="true" /></td>
            <td><asp:Button ID="btnDeploy" runat="server" Text="Deploy»"  Font-Bold="true" />  </td>
            <td><asp:Label id="lblToDeploy" runat="server" Font-Bold="true" /></td>
        </tr>
    </table>




    
    <asp:PlaceHolder ID="plhTbl" runat="server">
        <table class="datagrid">
            <tr>
                <th id="colLoc" runat="server">Local-Only</th>
                <th id="colDif" runat="server">Different</th>
                <th id="colRem" runat="server">Remote-Only</th>
            </tr>
            <tr>
                <td valign="top">
                    <asp:PlaceHolder ID="plhLoc" runat="server" />
                </td>
                <td valign="top">
                    <asp:PlaceHolder ID="plhDif" runat="server" />
                </td>
                <td valign="top">
                    <asp:PlaceHolder ID="plhRem" runat="server" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>

</asp:Content>
