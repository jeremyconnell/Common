<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Website.aspx.cs" Inherits="pages_instances_Website"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Deployment Website (Azure)"
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
    
        <uc:FormLabel ID="lblDatabase" runat="server" Text="Azure: Setup" />
        <uc:Dropdown ID="ddAzure" runat="server" Required="false" Label="Azure Web" />
        <uc:Textbox ID="txtInstanceWebNameAzure" runat="server" Required="false" Label="Web (Azure)" />

        <asp:PlaceHolder ID="plh" runat="server">
            <uc:FormButtonsBegin ID="FormButtonsBegin1" runat="server" />
                <asp:button id="btnCreateWeb"    runat="server" text="Create" OnClick="btnCreateWeb_Click" />
            <uc:FormButtonsEnd   ID="FormButtonsEnd1" runat="server" />
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="plh2" runat="server">
        <uc:FormLabel ID="FormLabel2" runat="server" Text="Azure: Status" />
        <uc:Textbox ID="txtWebsiteName" runat="server"  Label="Website" Mode="Disabled" />
        <uc:Textbox ID="txtHostNames" runat="server"  Label="Host-Names" Mode="Disabled" Visible="false" />
        <uc:Textbox ID="txtWebSpaceName" runat="server"  Label="WebSpace" Mode="Disabled" />
        <uc:Textbox ID="txtGeoRegion" runat="server"  Label="Geo.R./Plan" Mode="Disabled" />
        <uc:Textbox ID="txtServerFarm" runat="server"  Label="Server Farm" Mode="Disabled" />
        <uc:Textbox ID="txtAvailState" runat="server"  Label="Avail." Mode="Locked" Text="" Visible="false" />
        <uc:Textbox ID="txtRuntime" runat="server"  Label="Runtime" Mode="Locked" Text="" Visible="false" />
        <uc:Textbox ID="txtUsageState" runat="server"  Label="Usage" Mode="Locked" Text="" Visible="false" />
        </asp:PlaceHolder>


        <uc:FormLabel ID="lblApi" runat="server" Text="Website" />
        <uc:Checkbox ID="chkInstanceWebUseSsl" runat="server" Required="false" Label="Ssl" />
        <uc:Textbox ID="txtInstanceWebHostName" runat="server" Required="false" Label="Host" />
        <uc:Textbox ID="txtInstanceWebSubDir" runat="server" Required="false" Label="SubDir" />
        <uc:Textbox ID="lnkUrl" runat="server" Required="false" Label="URL" Mode="Locked" />



        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True" onclick="btnSave_Click" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />




        <uc:FormButtonsBegin ID="fbb3" runat="server" />
            <div style="text-align:left">
                <asp:button id="btnRestartWeb" runat="server" Text="Restart"        OnClick="btnRestartWeb_Click"  />
                <asp:button id="btnDeleteWeb"  runat="server" Text="Delete"         OnClick="btnDeleteWeb_Click" OnClientClick="return confirm('Delete this Web-App?')" />
            </div>

        <uc:FormButtonsEnd   ID="fbe3" runat="server" />

    <uc:FormEnd ID="fe" runat="server" />
    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>
