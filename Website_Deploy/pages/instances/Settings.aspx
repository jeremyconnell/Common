<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Settings.aspx.vb" Inherits="pages_clients_Settings"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Config Settings (Server Copy)"
    ValidateRequest="False"
%>
<asp:Content ID="a" ContentPlaceHolderID="above" runat="server" >
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" />

    <span style="font-weight:bold; font-size:smaller">Deploy:</span> 
    <asp:DropDownList ID="ddInstance" runat="server" DataTextField="NameAndValueCount" DataValueField="InstanceId" AutoPostBack="true" /><br />
    <br /><br />
    <span style="font-weight:bold; font-size:smaller">Group:</span> 
    <asp:DropDownList ID="ddGroups" runat="server" DataTextField="NameAndCount" DataValueField="GroupId" AutoPostBack="true" />
    <asp:RadioButtonList ID="rbl" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="0" Selected="True">View</asp:ListItem>
        <asp:ListItem Value="1">Edit</asp:ListItem>
    </asp:RadioButtonList>
    <br /><br />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <style type="text/css">
        .datagrid td { padding:3px 10px; !important }
        .datagrid th { padding:3px 10px; !important }
    </style>




    <uc:FormBegin ID="fb" runat="server" />
        <uc:Textbox  ID="txtInstanceSettingsImported" Runat="server" Mode="Locked" Required="false" Label="Imported" TextMode="Date" />
        <uc:Textbox  ID="txtInstanceSettingsExported" Runat="server" Mode="Locked" Required="false" Label="Exported" TextMode="Date" />
    <uc:FormEnd ID="fe" runat="server" />


    <table>
        <tr>
            <td valign="top">
                <asp:PlaceHolder ID="plh1" runat="server" />
            </td>
            <td valign="top" width="40">
                &nbsp;
            </td>
            <td valign="top">
                <asp:PlaceHolder ID="plh2" runat="server" />
            </td>
            <td valign="top" width="40">
                &nbsp;
            </td>
            <td valign="top">
                <asp:PlaceHolder ID="plh3" runat="server" />
            </td>
        </tr>
    </table>

</asp:Content>