<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="client.aspx.cs" Inherits="pages_Clients_Client"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Add or Change a Client."
    ValidateRequest="False"
%>
<asp:Content ID="a" ContentPlaceHolderID="above" runat="server" >
    <asp:DropDownList ID="ddClients" runat="server" DataTextField="NameAndInstanceCount" DataValueField="ClientId" AutoPostBack="true" OnSelectedIndexChanged="ddClients_SelectedIndexChanged" />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:FormBegin ID="fb" runat="server" />
        <uc:FormLabel ID="FormLabel1" runat="server" Text="Client" />
        <uc:Textbox ID="txtClientName" runat="server" Required="true" Label="Name" />
        <uc:Textbox ID="txtClientEmail" runat="server" Required="false" Label="Email" />
        <uc:Textbox ID="txtClientUniqueCode" runat="server" Required="true" Label="Code" MaxLength="10" Width="100" />



        <uc:FormLabel ID="FormLabel2" runat="server" Text="Status" />
        <uc:Dropdown ID="ddClientStatusId" Runat="server" Required="true" Label="Status" />
        <uc:Textbox  ID="txtClientTrialStarted" Runat="server" Required="false" Label="Trial Start" Mode="Locked" />
        <uc:Textbox  ID="txtClientTrialEnded" Runat="server" Required="false" Label="Trial End" TextMode="Date" />
        <uc:Textbox  ID="txtClientProductionStarted" Runat="server" Required="false" Label="In Prod." TextMode="Date" />
        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True"  OnClick="btnSave_Click"/>
            <asp:button id="btnDelete"  runat="server" text="Delete" causesvalidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this Client?')" />
            <asp:button id="btnCancel"  runat="server" text="Cancel" causesvalidation="False" OnClick="btnCancel_Click" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>