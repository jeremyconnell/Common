<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="upgradeHistory.aspx.cs" Inherits="pages_UpgradeHistorys_UpgradeHistory"
  MasterPageFile="~/masterpages/deploy.master"
  Title="UpgradeHistory"
%>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <uc:FormBegin ID="fb" runat="server" />
        <uc:Textbox ID="txtChangeReportId" Runat="server" Mode="Locked" Label="ReportId" Tooltip="Click to view this Report" />
        <uc:Textbox ID="txtChangeNewVersionId" Runat="server" Mode="Locked" Label="NewVersionId" Tooltip="Click to view this NewVersion" />
        <uc:Textbox  ID="txtChangeNewSchemaMD5" runat="server" Mode="Locked" Label="NewSchemaMD5" />
        <uc:Textbox  ID="txtChangeStarted" runat="server" Mode="Locked" Label="Started" />
        <uc:Textbox  ID="txtChangeFinished" runat="server" Mode="Locked" Label="Finished" />
    <uc:FormEnd ID="fe" runat="server" />

</asp:Content>
