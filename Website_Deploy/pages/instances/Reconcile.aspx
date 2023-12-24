<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Reconcile.aspx.vb" Inherits="pages_clients_Reconcile"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Config-Settings (Client copy)"
    ValidateRequest="False"
%>
<asp:Content ID="a" ContentPlaceHolderID="above" runat="server" >
    
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged"/>

    <span style="font-weight:bold; font-size:smaller">Deploy:</span> 
    <asp:DropDownList ID="ddInstance" runat="server" DataTextField="IdAndName" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddInstance_SelectedIndexChanged" />

    <asp:Button ID="btnRefreshCache" runat="server" Text="Refresh Cache"/>
</asp:Content>

<asp:Content ID="s" ContentPlaceHolderID="side" runat="server">
    <asp:Button ID="btnFileToDb" runat="server" Text="Files => Database" /><br />
    <br />
    <br />
    
    <asp:Button ID="btnExport" runat="server"   Text="« Client" />
    <asp:Button ID="btnImport" runat="server"   Text="Admin »"  style="margin-bottom:5px" />
    <br />
    <br />
    <asp:Button ID="btnKeyGen" runat="server" Text="Create Default-Vals" /><br />
    <br />
    <br />
    <div>
        <asp:TextBox ID="txtJson" runat="server" TextMode="MultiLine" Rows="3" Width="100%" />
    </div>
    <asp:Button ID="btnImportJson"     runat="server" Text="Import JSON" />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <asp:PlaceHolder ID="plh" runat="server" />
</asp:Content>