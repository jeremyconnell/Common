<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="instance.aspx.cs" Inherits="pages_Instances_Instance"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Add or Change a Instance."
    ValidateRequest="False"
%>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged"/>

    <span style="font-weight:bold; font-size:smaller">Deploy:</span> 
    <asp:DropDownList ID="ddIns" Runat="Server" DataTextField="NameAndSuffix" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddIns_SelectedIndexChanged"/>
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:FormBegin ID="fb" runat="server" />
        <uc:FormLabel ID="FormLabel1" runat="server" Text="Deployment Label" />
        <uc:Dropdown ID="ddInstanceClientId" Runat="server" Required="true" Label="Client" AutoPostback="true"   />
        <uc:Dropdown ID="ddInstanceSuffix" Runat="server" Required="false" Label="Suffix"   />
        <uc:Textbox ID="txtMachineName" runat="server" Required="false" Label="Machine"  Mode="Locked" />

    <asp:PlaceHolder ID="plhIsEdit" runat="server">
        <uc:Textbox  ID="txtInstanceCreated" Runat="server" Required="false" Mode="Locked" Label="Created" TextMode="String" />



    
    </asp:PlaceHolder>

        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True"  OnClick="btnSave_Click"/>
            <asp:button id="btnDelete"  runat="server" text="Delete" causesvalidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this Instance?')" />
            <asp:button id="btnCancel"  runat="server" text="Cancel" causesvalidation="False" OnClick="btnCancel_Click" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />
    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>
