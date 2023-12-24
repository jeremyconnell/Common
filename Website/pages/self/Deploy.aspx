<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Deploy.aspx.vb" Inherits="pages_self_Deploy" 
  MasterPageFile="~/pages/default.master"
  Title="Self-Deployment"
%>


<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <table class="datagrid">
        <tr>
            <th colspan="4">Self-Deployment Utility</th>
        </tr>
        <tr>
            <th>Remote</th>
            <td colspan="2"><asp:TextBox ID="txtHost" runat="server" Width="100%" /></td>
            <td><asp:Button ID="btnCheck" runat="server" Text="Apply" Font-Size="Smaller" /></td>
        </tr>
        <tr>
            <th>Remote (folder)</th>
            <td colspan="3"><asp:TextBox ID="txtFolder" runat="server" Width="450" /></td>
        </tr>
        <tr>
            <th>Local</th>
            <td colspan="3"><asp:TextBox ID="txtDev" runat="server" Width="450" /></td>
        </tr>
        <tr>
            <th>Exclude</th>
            <td colspan="3"><asp:TextBox ID="txtExclude" runat="server" Width="450" Font-Size ="Smaller"/></td>
        </tr>
        <tr>
            <th>Full Hash</th>
            <td colspan="3"><asp:CheckBox ID="chkFast" runat="server" Text="Fast Hash" AutoPostBack="true" OnCheckedChanged="chkFast_CheckedChanged"/></td>
        </tr>
        <tr>
            <th>Local</th>
            <td><asp:Label ID="lblHash" runat="server"  Font-Bold="true"  /></td>
            <td align="right"><asp:Button ID="btnRefresh" runat="server" Text="Refresh" Font-Size="Smaller" />  </td>
            <td><asp:Label id="lblLocalTotal" runat="server" Font-Bold="true" /></td>
        </tr>
        <tr>
            <th>Remote</th>
            <td><asp:Label ID="lblRemote" runat="server"  Font-Bold="true" /></td>
            <td align="right"><asp:Button ID="btnDeploy" runat="server" Text="Deploy»"  Font-Bold="true" />  </td>
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
