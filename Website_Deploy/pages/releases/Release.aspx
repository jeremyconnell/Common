<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="Release.aspx.cs" Inherits="pages_Releases_Release"
  MasterPageFile="~/masterpages/deploy.master"
  Title="Release"
%>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <uc:FormBegin ID="fb" runat="server" />
        <uc:Textbox  ID="txtReleaseCreated" runat="server" Mode="Locked" Label="Created" />
        <uc:Textbox ID="txtReleaseAppId" Runat="server" Mode="Locked" Label="App" Tooltip="Click to view this App" />
        <uc:Textbox ID="txtReleaseInstanceId" Runat="server" Mode="Locked" Label="InstanceId" Tooltip="Click to view this Instance" />
        <uc:Textbox ID="txtReleaseVersionId" Runat="server" Mode="Locked" Label="Version" Tooltip="Click to view this Version" />
        <uc:Textbox ID="txtPrevious" Runat="server" Mode="Locked" Label="Previous"  />
        <uc:Textbox  ID="txtReleaseBranchName" runat="server" Mode="Locked" Label="BranchName" />
        <uc:Textbox  ID="txtReleaseExpired" runat="server" Mode="Locked" Label="Expired" />
    <uc:FormEnd ID="fe" runat="server" />

</asp:Content>
