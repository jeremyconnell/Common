<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="app.aspx.cs" Inherits="pages_Apps_App"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Add or Change a App."
    ValidateRequest="False"
%>
<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="AppName" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged"/>
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:FormBegin ID="fb" runat="server" />
        <uc:Textbox ID="txtAppName" runat="server" Required="true" Label="App Name" Width="200" />
        <uc:Dropdown ID="ddAppMainVersionId" Runat="server" Required="false" Label="Main Branch" Width="100%" />
        <uc:Textbox  ID="txtAppKeepOldFilesForDays" runat="server" Required="true" Label="Keep Files (Days)" TextMode="Integer" Width="50" />
        <uc:Textbox  ID="txtAppCreated" Runat="server" Label="Created" TextMode="String" Mode="Locked" Visible="false" />
        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True"  OnClick="btnSave_Click"/>
            <asp:button id="btnDelete"  runat="server" text="Delete" causesvalidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this App?')" />
            <asp:button id="btnCancel"  runat="server" text="Cancel" causesvalidation="False" OnClick="btnCancel_Click" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />

    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>

