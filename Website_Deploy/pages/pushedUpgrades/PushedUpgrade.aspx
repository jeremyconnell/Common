<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="pushedUpgrade.aspx.cs" Inherits="pages_PushedUpgrades_PushedUpgrade"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Add or Change a PushedUpgrade."
    ValidateRequest="False"
%>
<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:FormBegin ID="fb" runat="server" />
        <uc:Textbox ID="txtPushUserName" runat="server" Required="true" Label="UserName" />
        <uc:Dropdown ID="ddPushInstanceId" Runat="server" Required="true" Label="Instance" />
        <uc:Dropdown ID="ddPushOldVersionId" Runat="server" Required="true" Label="OldVersion" />
        <uc:Textbox ID="txtPushOldSchemaMD5" runat="server" Required="true" Label="OldSchemaMD5" />
        <uc:Dropdown ID="ddPushNewVersionId" Runat="server" Required="true" Label="NewVersion" />
        <uc:Textbox ID="txtPushNewSchemaMD5" runat="server" Required="true" Label="NewSchemaMD5" />
        <uc:Textbox  ID="txtPushStarted" Runat="server" Required="true" Label="Started" TextMode="Date" />
        <uc:Textbox  ID="txtPushCompleted" Runat="server" Required="true" Label="Completed" TextMode="Date" />
        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True"  OnClick="btnSave_Click"/>
            <asp:button id="btnDelete"  runat="server" text="Delete" causesvalidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this PushedUpgrade?')" />
            <asp:button id="btnCancel"  runat="server" text="Cancel" causesvalidation="False" OnClick="btnCancel_Click" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />
    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>
