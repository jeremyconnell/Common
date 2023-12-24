<%@ Page Language="VB" AutoEventWireup="false" CodeFile="schema.aspx.vb" Inherits="pages_self_schema"
  MasterPageFile="~/pages/default.master"
  Title="Schema Sync (Admin)"
%>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <div>        
         <asp:LinkButton ID="btnToggle" runat="server" />
    </div>
    <div>
        <table class="datagrid">
            <tr>
                <th>Source</th>
                <td><asp:DropDownList id="ddSource" runat="server" AutoPostBack="true" Font-Bold="true" DataTextField="NameAndSuffix" DataValueField="InstanceId" /></td>
                <td><asp:TextBox id="txtSource" runat="server" Enabled="false" Font-Size="Smaller" Width="300"  /><asp:Button ID="btnSaveSrc" runat="server" Text="Save" Font-Size="Smaller" /> </td>
            </tr>
            <tr>
                <th>Target</th>
                <td><asp:DropDownList id="ddTarget" runat="server" AutoPostBack="true" DataTextField="NameAndSuffix" DataValueField="InstanceId"  /></td>
                <td><asp:TextBox id="txtTarget" runat="server" Enabled="false" Font-Size="Smaller" Width="300"  /><asp:Button ID="btnSaveTar" runat="server" Text="Save" Font-Size="Smaller" /></td>
            </tr>
        </table>
    </div>

    <asp:Table ID="tbl" runat="server" CssClass="datagrid" style="margin-bottom:40px" Width="900px">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>#</asp:TableHeaderCell>
            <asp:TableHeaderCell>Deploy</asp:TableHeaderCell>
            <asp:TableHeaderCell ID="cellBin" runat="server" Visible="false">Bin</asp:TableHeaderCell>
            <asp:TableHeaderCell>Schema</asp:TableHeaderCell>
            <asp:TableHeaderCell>Tables</asp:TableHeaderCell>
            <asp:TableHeaderCell>Views</asp:TableHeaderCell>
            <asp:TableHeaderCell>Columns</asp:TableHeaderCell>
            <asp:TableHeaderCell>Procs/Fns</asp:TableHeaderCell>
            <asp:TableHeaderCell>Indexs</asp:TableHeaderCell>
            <asp:TableHeaderCell>Keys</asp:TableHeaderCell>
            <asp:TableHeaderCell>Mig</asp:TableHeaderCell>
            <asp:TableHeaderCell>View</asp:TableHeaderCell>
            <asp:TableHeaderCell>Diff</asp:TableHeaderCell>
            <asp:TableHeaderCell>Fix</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>


</asp:Content>
