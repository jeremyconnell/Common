<%@ Page Language="VB" AutoEventWireup="false" CodeFile="group.aspx.vb" Inherits="pages_group" 
MasterPageFile="~/pages/default.master"
Title="Group"
%>

<asp:Content id="c" runat="server" ContentPlaceHolderID="body">
    <uc:FormBegin ID="fb" runat="server" />
        <uc:Textbox ID="txtGroupName" runat="server" Label="Group Name" Required="true" />
        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:Button ID="btnCancel"  runat="server" Text="Cancel"  CausesValidation="false" />
            <asp:Button ID="btnDelete"  runat="server" Text="Delete"  CausesValidation="false" OnClientClick="return confirm('Delete this Group?')" />
            <asp:Button ID="btnSave"    runat="server" Text="Create Group" />
        <uc:FormButtonsEnd ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />
</asp:Content>
