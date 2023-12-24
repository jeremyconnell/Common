<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeaturesForRoles.aspx.cs" Inherits="pages_instances_FeaturesForRoles"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Deployment Version"
    ValidateRequest="False"
%>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged" />

    <br /><br />
    <span style="font-weight:bold; font-size:smaller">Deployment:</span> 
    <asp:DropDownList ID="ddIns" Runat="Server" DataTextField="NameAndSuffix" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddIns_SelectedIndexChanged" />
</asp:Content>


<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <asp:RadioButtonList ID="rbl" runat="server" OnSelectedIndexChanged="rbl_SelectedIndexChanged" RepeatDirection="Horizontal" AutoPostBack="true">
        <asp:ListItem Value="1" Text="Control Manager" Selected="True" />
        <asp:ListItem Value="2" Text="Risk Manager" />
    </asp:RadioButtonList>

    <asp:Table ID="tbl" runat="server" CssClass="datagrid" />

    <asp:Table ID="tblEdit" runat="server" CssClass="datagrid" Visible="false">
        <asp:TableHeaderRow>
            <asp:TableCell>
                <asp:TextBox ID="txt" runat="server" MaxLength="255" Font-Size="Smaller" Width="180" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="btnRename" runat="server" Text="Rename" Font-Size="Smaller" OnClick="btnRename_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" Font-Size="Smaller" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this Feature?')" />
            </asp:TableCell>
        </asp:TableHeaderRow>
    </asp:Table>

</asp:Content>
