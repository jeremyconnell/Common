<%@ Page Language="VB" AutoEventWireup="false" CodeFile="setting.aspx.vb" Inherits="pages_setting"
MasterPageFile="~/pages/default.master"
Title="Setting"
%>
<asp:Content id="c" runat="server" ContentPlaceHolderID="body">
    <uc:FormBegin id="f1" runat="server" />    
        <uc:Dropdown ID="ddGroup"       runat="server"  Label="Group"       Required="true" />
        <uc:Textbox  ID="txtName"       runat="server"  Label="Name/Key"    Required="true" />
        <uc:Dropdown ID="ddType"        runat="server"  Label="Data Type"   Required="true" AutoPostback="true" />
        <uc:Dropdown ID="ddList"        runat="server"  Label="List"        Required="true" Visible="false" />
        <uc:Checkbox ID="chkCanEdit"    runat="server"  Label="Client Can Edit" Required=false />
        
        <uc:FormButtonsBegin ID="fb1" runat="server" />
            <asp:Button ID="btnCancel"  runat="server" Text="Cancel"  CausesValidation="false" />
            <asp:Button ID="btnDelete"  runat="server" Text="Delete"  CausesValidation="false" OnClientClick="return confirm('Delete this Setting?')" />
            <asp:Button ID="btnSave"    runat="server" Text="Create Setting" />
        <uc:FormButtonsEnd   id="fb2" runat="server" />
    <uc:FormEnd   id="f2" runat="server" />
</asp:Content>
