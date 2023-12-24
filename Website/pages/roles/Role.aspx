<%@ Page Language="vb" AutoEventWireup="false" CodeFile="role.aspx.vb" Inherits="pages_Roles_Role"
    MasterPageFile="~/pages/default.master"
    Title="Add or Change a Role."
    ValidateRequest="False"
%>
<%@ Register tagprefix="uc" tagname="UCUserRolesPair" src="../userRoles/usercontrols/UCUserRolesPair.ascx" %>
<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <uc:FormBegin ID="fb" runat="server" />
        <uc:Textbox  ID="txtRoleName" runat="server" Required="true" Label="Role Name" />
        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True"  />
            <asp:button id="btnDelete"  runat="server" text="Delete" causesvalidation="False" OnClientClick="return confirm('Delete this Role?')" />
            <asp:button id="btnCancel"  runat="server" text="Cancel" causesvalidation="False" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />        
    <uc:FormEnd ID="fe" runat="server" />
    <br />
    <br />
    <strong>Users associated with this Role</strong>
    <uc:UCUserRolesPair ID="ctrlUsers" runat="server" Title="Users" />
</asp:Content>
