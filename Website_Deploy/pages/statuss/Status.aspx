<%@ Page EnableEventValidation="false" EnableViewState="false" Language="vb" AutoEventWireup="false" CodeFile="status.aspx.vb" Inherits="pages_Statuss_Status"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Add or Change a Status."
    ValidateRequest="False"
%>
<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:FormBegin ID="fb" runat="server" />
        <uc:Textbox  ID="txtStatusName" runat="server" Required="true" Label="Name" />
        <uc:Textarea  ID="txtStatusDescription" runat="server" Required="true" Label="Description" />
        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True"  />
            <asp:button id="btnDelete"  runat="server" text="Delete" causesvalidation="False" OnClientClick="return confirm('Delete this Status?')" />
            <asp:button id="btnCancel"  runat="server" text="Cancel" causesvalidation="False" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />
    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>
