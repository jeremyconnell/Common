<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Settings.aspx.vb" Inherits="pages_keys_Settings"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Global Values: "
    ValidateRequest="False"
%>

<asp:Content ID="a" ContentPlaceHolderID="above" runat="server" >
    <asp:DropDownList ID="ddKeys" runat="server" DataTextField="GroupAndKey" DataValueField="KeyName" AutoPostBack="true" OnSelectedIndexChanged="ddKeys_SelectedIndexChanged" />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">


    <asp:PlaceHolder ID="plh" runat="server" />
</asp:Content>