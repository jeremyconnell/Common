<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login"
MasterPageFile="~/pages/default.master"
Title="Login Page"
 %>
<asp:Content ID=c runat=server ContentPlaceHolderID=body>
    <uc:FormBegin ID=fb runat=server />
        <uc:TextBox ID=txtLogin runat=server Label="Username"      Width=100 />
        <uc:TextBox ID=txtPassword runat=server Label="Password"   Width=100 IsPassword=true />
        <uc:FormButtonsBegin ID=fbb runat=server />
            <asp:Button ID=btnLogin runat=server Text="Login" OnClick="btnLogin_Click" />
        <uc:FormButtonsEnd   ID=fbe runat=server />
    <uc:FormEnd ID=fe runat=server />
    <asp:Label ID=lblError runat=server  ForeColor=Red EnableViewState=false />
</asp:Content>