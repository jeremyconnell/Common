<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="value.aspx.cs" Inherits="pages_Values_Value"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Add or Change a Value."
    ValidateRequest="False"
%>
<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:FormBegin ID="fb" runat="server" />
        <uc:Dropdown ID="ddValueInstanceId" Runat="server" Required="false" Label="Deployment" Width="100%" />
        <uc:Dropdown ID="ddKeyName" Runat="server" Required="false" Label="Key" Width="100%" />
        <uc:Textarea ID="txtValueString" runat="server" Required="false" Label="String" />
        <uc:Dropdown ID="ddValueBoolean" runat="server" Required="false" Label="Boolean" />
        <uc:Textbox  ID="txtValueInteger" runat="server" Required="false" Label="Integer" TextMode="Integer" />
        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True"  OnClick="btnSave_Click"/>
            <asp:button id="btnDelete"  runat="server" text="Delete" causesvalidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this Value?')" />
            <asp:button id="btnCancel"  runat="server" text="Cancel" causesvalidation="False" OnClick="btnCancel_Click" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />
    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>
