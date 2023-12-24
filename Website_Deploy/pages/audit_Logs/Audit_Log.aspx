<%@ Page EnableEventValidation="false" EnableViewState="false" Language="vb" AutoEventWireup="false" CodeFile="audit_Log.aspx.vb" Inherits="pages_Audit_Logs_Audit_Log"
    MasterPageFile="~/masterpages/default.master"
    Title="Add or Change a Audit_Log."
    ValidateRequest="False"
%>
<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:FormBegin ID="fb" runat="server" />
        <uc:Textbox  ID="txtLogCreated" Runat="server" Required="true" Label="Created" TextMode="Date" Mode="Locked" />
        <uc:Dropdown ID="ddLogTypeId" Runat="server" Required="true" Label="Type" Mode="Locked" />
        <uc:Textbox  ID="txtLogMessage" runat="server" Required="true" Label="Message" Mode="Locked" />
    <uc:FormEnd ID="fe" runat="server" />
    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>
