<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="reportHistory.aspx.cs" Inherits="pages_ReportHistorys_ReportHistory"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Add or Change a ReportHistory."
    ValidateRequest="False"
%>
<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:FormBegin ID="fb" runat="server" />
        <uc:Dropdown ID="ddReportInstanceId" Runat="server" Required="true" Label="Instance" />
        <uc:Dropdown ID="ddReportInitialVersionId" Runat="server" Required="true" Label="InitialVersion" />
        <uc:Textbox ID="txtReportInitialSchemaMD5" runat="server" Required="true" Label="InitialSchemaMD5" />
        <uc:Textbox  ID="txtReportAppStarted" Runat="server" Required="true" Label="AppStarted" TextMode="Date" />
        <uc:Textbox  ID="txtReportAppStopped" Runat="server" Required="true" Label="AppStopped" TextMode="Date" />
        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True"  OnClick="btnSave_Click"/>
            <asp:button id="btnDelete"  runat="server" text="Delete" causesvalidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this ReportHistory?')" />
            <asp:button id="btnCancel"  runat="server" text="Cancel" causesvalidation="False" OnClick="btnCancel_Click" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />
    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>
