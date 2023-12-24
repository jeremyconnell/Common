<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Database.aspx.cs" Inherits="pages_instances_Database"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Deployment Database (Azure)"
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
        <uc:Dropdown ID="ddAzure" runat="server" Required="false" Label="Azure DB" />
        <uc:Textbox ID="txtInstanceDbNameAzure" runat="server" Required="false" Label="DB Name" />
        <uc:Textbox ID="txtCreated" runat="server"  Label="Created" Mode="Locked" Text="" Visible="false" />


        <asp:PlaceHolder ID="plh" runat="server">
            <uc:FormButtonsBegin ID="FormButtonsBegin1" runat="server" />
                <asp:button id="btnCreate"    runat="server" text="Create Database" OnClick="btnCreate_Click" />
            <uc:FormButtonsEnd   ID="FormButtonsEnd1" runat="server" />
        </asp:PlaceHolder>

    
        <asp:PlaceHolder ID="plh2" runat="server">
            <uc:FormLabel ID="FormLabel1" runat="server" Text="Azure: Status" />
            <uc:Textbox ID="txtDatabaseName" runat="server"  Label="Database" Mode="Disabled" />
            <uc:Textbox ID="txtServerName" runat="server"  Label="Server" Mode="Locked" Text="ControlTrackSaas" />
            <uc:Textbox ID="txtEdition" runat="server"  Label="Edition/Objv" Mode="Locked" Text="" />
            <uc:Textbox ID="txtSize" runat="server"  Label="Size" Mode="Locked" Text="" Visible="false" />
            <uc:Textbox ID="txtState" runat="server"  Label="State" Mode="Locked" Text="" Visible="false" />
        </asp:PlaceHolder>

        <uc:FormLabel ID="lblSec" runat="server" Text="Security" />
        <uc:Textbox ID="txtInstanceDbLogin" runat="server" Required="false" Label="Login" Width="150" />
        <uc:Textbox ID="txtInstanceDbUserName" runat="server" Required="false" Label="UserName" Width="150" />
        <uc:Textbox ID="txtInstanceDbPassword" runat="server" Required="false" Label="Password" Width="150" />
        <uc:Textarea ID="txtInstanceDbConnectionString" runat="server" Required="false" Label="Conn. Str." Width="600" Height="60px" />

    
        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnDeleteDB"  runat="server" Text="Delete"         OnClick="btnDeleteDB_Click" OnClientClick="return confirm('Delete this Database?')" />
            <asp:button id="btnGuessCS"    runat="server" text="Guess Con.Str." causesvalidation="false" OnClick="btnGuessCS_Click"  />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True" onclick="btnSave_Click" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    

    <asp:PlaceHolder ID="plhSetup" runat="server">
        <uc:FormLabel ID="fll" runat="server" Text="Scripting" />
        <uc:Dropdown ID="ddUsers" runat="server" Label="DB Users" Width="150" />
        <uc:Textbox ID="txtSqlUser" runat="server" Label="Sql: User" Mode="Locked" Width="150" />
        <uc:Textbox ID="txtSqlRole" runat="server" Label="Sql: Role" Mode="Locked" Width="150" />
        <uc:FormButtonsBegin ID="FormButtonsBegin2" runat="server" />
            <asp:button id="btnCreateUser"  runat="server" Text="Create User" OnClick="btnCreateUser_Click" />
            &nbsp;
            <asp:button id="btnCreateRole"  runat="server" Text="Create Role" OnClick="btnCreateRole_Click" />
        <uc:FormButtonsEnd   ID="FormButtonsEnd2" runat="server" />
        
        <uc:FormLabel ID="lbl" runat="server" Text="Sql" />
        <uc:RadioButtonList ID="rbl" runat="server" Label="DB" RepeatDirection="Horizontal" />
        <uc:Textarea ID="txtSql" runat="server" Label="Sql (Saas)" Height="100px" Width="600" />
        <uc:FormButtonsBegin ID="FormButtonsBegin3" runat="server" />
            <asp:button id="btnSelect"  runat="server" Text="Select" OnClick="btnSelect_Click" />
                    &nbsp;
            <asp:button id="btnUpdate"  runat="server" Text="update" OnClick="btnUpdate_Click" />
        <uc:FormButtonsEnd   ID="FormButtonsEnd3" runat="server" />

    </asp:PlaceHolder>

    
    <uc:FormEnd ID="fe" runat="server" />
    

    <div id="divIframe" runat="server" visible="false" style="width:100%; height:500px; min-width:1200px">
        <b>Results</b>
		<iframe id="iframe" name="iframe" runat=server width="100%" height="100%"/>
    </div>

    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>
