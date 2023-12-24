<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppLogin.aspx.cs" Inherits="pages_instances_AppLogin"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Deployment AppLogin"
    ValidateRequest="False"
%>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged" />

    <span style="font-weight:bold; font-size:smaller">Deploy:</span> 
    <asp:DropDownList ID="ddIns" Runat="Server" DataTextField="NameAndSuffix" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddIns_SelectedIndexChanged" />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:FormBegin ID="fb" runat="server" />
        <uc:FormLabel ID="FormLabel2" runat="server" Text="Admin Login" />
        <uc:Textbox ID="txtInstanceAppLogin" runat="server" Required="false" Label="Login" Width="150"  />
        <uc:Textbox ID="txtInstanceAppPassword" runat="server" Required="false" Label="Password" Width="150"  />


        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True" onclick="btnSave_Click" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />
    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />


    <asp:PlaceHolder ID="plh" runat="server" Visible="false">


        <div style="margin-top:50px">
            <asp:Label ID="lbl" runat="server" Text="Live Data: Logins for Demo2 " Font-Size="Smaller" Font-Bold="true" />
            <asp:DropDownList ID="ddLogin" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddLogin_SelectedIndexChanged" />
        </div>
        <asp:Table ID="tbl" runat="server" CssClass="datagrid">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                <asp:TableHeaderCell>Value</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </asp:PlaceHolder>
</asp:Content>
