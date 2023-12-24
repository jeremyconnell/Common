<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="key.aspx.cs" Inherits="pages_Keys_Key"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Add or Change a Key."
    ValidateRequest="False"
%>

<asp:Content ID="a" ContentPlaceHolderID="above" runat="server" >
    <asp:DropDownList ID="ddKeys" runat="server" DataTextField="KeyName" DataValueField="KeyName" AutoPostBack="true" OnSelectedIndexChanged="ddKeys_SelectedIndexChanged" />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:FormBegin ID="fb" runat="server" />
        <uc:Dropdown ID="ddKeyGroupId" Runat="server" Required="true" Label="Group" />
        <uc:Dropdown ID="ddKeyFormatId" Runat="server" Required="true" Label="Format" />

        <uc:Textarea ID="txtKeyDefaultString" runat="server" Required="false" Label="Default Value" Width="600" />
        <uc:Dropdown ID="ddKeyDefaultBoolean" runat="server" Required="false" Label="Default Value" />
        <uc:Textbox  ID="txtKeyDefaultInteger" runat="server" Required="false" Label="Default Value" TextMode="Integer" Width="50" />

        <uc:Checkbox ID="chkKeyIsEncrypted" runat="server" Required="false" Label="IsEncrypted" Visible="false" />

        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True"  OnClick="btnSave_Click"/>
            <asp:button id="btnDelete"  runat="server" text="Delete" causesvalidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this Key?')" />
            <asp:button id="btnCancel"  runat="server" text="Cancel" causesvalidation="False" OnClick="btnCancel_Click" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />
    
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
</asp:Content>

<asp:Content ID="b" runat="server" ContentPlaceHolderID="below">
    <div>
        <fieldset>
            <legend>Actions</legend>
            <asp:button id="btnDefault"   runat="server" text="Most-Common Value => Default Value" causesvalidation="False" OnClick="btnDefault_Click" OnClientClick="return confirm('Overwrite the default value, based on current usage?')" /><br />
            <br />
            <asp:button id="btnApplyAll"   runat="server" text="Default Value => Overwrite All Clients" causesvalidation="False" OnClick="btnApplyAll_Click" OnClientClick="return confirm('Overwrite all values for this key?\nThis will set values for ALL CLIENTS to this default value?')" />
        </fieldset>
    </div>
</asp:Content>