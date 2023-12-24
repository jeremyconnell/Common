<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="version.aspx.cs" Inherits="pages_Versions_Version"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Add or Change a Version."
    ValidateRequest="False"
%>
<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndVersionCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged"/>
    <span style="font-weight:bold; font-size:smaller">Ver:</span> 
    <asp:DropDownList ID="ddVer" Runat="Server" DataTextField="VersionName" DataValueField="VersionId" AutoPostBack="true" OnSelectedIndexChanged="ddVer_SelectedIndexChanged"/>

</asp:Content>


<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">





    <uc:FormBegin ID="fb" runat="server" />

    
        <asp:PlaceHolder ID="plhSchema" runat="server">
            <uc:FormLabel ID="FormLabel2" runat="server" Text="Schema Version"  />
            <uc:Textarea ID="txtDatabase" runat="server" Required="false" Mode="Editable"  Label="Database"    Width="400" Text="data source=.;initial catalog=ControlTrackNew;user id=sa;password=password!;MultipleActiveResultSets=True" />

            <uc:FormButtonsBegin ID="FormButtonsBegin2" runat="server" />
            <asp:button id="btnImportSch"    runat="server" text="Import Schema" causesvalidation="false" OnClick="btnImportSch_Click" />
            <uc:FormButtonsEnd   ID="FormButtonsEnd2" runat="server" /> 
        </asp:PlaceHolder>

    
            <uc:FormLabel ID="FormLabel3" runat="server" Text="Version Label"  />
        <uc:Textbox ID="txtVersionName" runat="server" Required="false" Mode="Editable" Label="Version"   Width="300" />
        <uc:Textbox ID="txtExceptions" runat="server" Required="false" Mode="Editable" Label="Ignoring"   Width="300" />


        <uc:FormButtonsBegin ID="FormButtonsBegin1" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Save" causesvalidation="True"  OnClick="btnSave_Click"/>
            <asp:button id="btnDelete"  runat="server" text="Delete" causesvalidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this Version?')" />
            <asp:button id="btnCancel"  runat="server" text="Cancel" causesvalidation="False" OnClick="btnCancel_Click" />
        <uc:FormButtonsEnd   ID="FormButtonsEnd1" runat="server" />    

    


        <asp:PlaceHolder ID="plhUpload" runat="server">
    
            <uc:FormLabel ID="FormLabel1" runat="server" Text="Upload New Version"  />
            <uc:FileUpload ID="fuUpload" runat="server" Required="false"   Label="Zip File" Width="300" />
            <uc:FormButtonsBegin ID="fbb" runat="server" />
                <asp:button id="btnUpload"    runat="server" text="Upload New Version" causesvalidation="True"  OnClick="btnUpload_Click"/>
            <uc:FormButtonsEnd   ID="fbe" runat="server" />


            <uc:FormLabel ID="fl4" runat="server" Text="(or) Publish Folder"  />
            <uc:Textbox ID="txtLocalPath" runat="server" Required="false" Label="Folder" Width="300" />
            <uc:FormButtonsBegin ID="fbb2" runat="server" />
                <asp:button id="btnLocal"   runat="server" text="Local Folder" causesvalidation="True" OnClick="btnLocal_Click"  />
            <uc:FormButtonsEnd ID="fbe2" runat="server" />
        </asp:PlaceHolder>
    
        <asp:PlaceHolder ID="plhView" runat="server">
            <uc:FormLabel ID="FormLabel5" runat="server" Text="Contents"  />
            <uc:Textbox  ID="txtVersionTotalBytes" runat="server" Required="false" Mode="Locked" Label="Binaries" />
            <uc:Textbox ID="txtVersionSchemaMD5" runat="server" Required="false" Mode="Locked" Label="Schema" />

            <uc:FormLabel ID="FormLabel6" runat="server" Text="Source"  />
            <uc:Textbox ID="txtLocation" runat="server" Required="false" Mode="Locked" Label="Location"   Width="300" />
            <uc:Textbox ID="txtIgnored" runat="server" Required="false" Mode="Locked" Label="Ignored"   Width="300" />
            
            <uc:FormLabel ID="FormLabel4" runat="server" Text="Changes"  />
            <uc:Textbox  ID="txtVersionDeltaSchemaMd5" runat="server" Required="false" Mode="Locked" Label="ΔSch" TextMode="String" />
            <uc:Textbox  ID="txtVersionCreated" Runat="server" Required="false" Mode="Locked" Label="Created" TextMode="String" />
            <uc:Textbox  ID="txtVersionExpires" Runat="server" Required="false" Mode="Locked" Label="Expired" TextMode="String" />
        </asp:PlaceHolder>

    <uc:FormEnd ID="fe" runat="server" />
    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>