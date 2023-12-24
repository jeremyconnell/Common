<%@ Page Language="C#" AutoEventWireup="false" CodeFile="Version.aspx.cs" Inherits="pages_instances_Version"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Deployment Version"
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
        <uc:FormLabel ID="fl" runat="server" Text="Normal Version" />
        <uc:Textbox  ID="txtMainVersion" Runat="server" Required="false" Mode="Locked" Label="Binaries" TextMode="String" />
        <uc:Textbox  ID="txtMainSchema" Runat="server" Required="false" Mode="Locked" Label="Schema" TextMode="String" />

        <uc:FormLabel ID="FormLabel1" runat="server" Text="Client-Specific Branch" />
        <uc:Dropdown ID="ddInstanceSpecialVersionId" Runat="server" Required="false" Label="Special" />
        <uc:Textbox ID="txtInstanceSpecialVersionName" runat="server" Required="false" Label="Reason" />

    
        <uc:FormLabel ID="FormLabel2" runat="server" Text="Last Reported Version" />
        <uc:Textbox ID="txtLastReportedVersion" runat="server" Required="false" Label="Binaries" Mode="Locked" />
        <uc:Textbox ID="txtLastReportedSchema" runat="server" Required="false" Label="Schema" mode="Locked" />


        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True" onclick="btnSave_Click" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />
    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>
