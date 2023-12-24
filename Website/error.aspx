<%@ Page Title="Error Page" Language="C#" MasterPageFile="~/pages/default.master" AutoEventWireup="true" CodeFile="error.aspx.cs" Inherits="pages_globalError_ContactAdmin" %>

<asp:Content ID="b" ContentPlaceHolderID="body" Runat="Server"> 
    An unexpected error has occured (#<asp:Literal ID=lit runat=server />).<br />
    Administrators have been notified<br /> 
    <br />
    <asp:HyperLink ID=lnkTryAgain runat=server Text="Try Again..." Visible=false /><br />
    <br />
    For urgent enquiries contact the administrator: <asp:HyperLink id="lnk" Runat="Server" NavigateUrl="mailto:picassonz@gmail.com?subject=Error Page">Picasso</asp:HyperLink><br />
    <br />
    <br />
    <asp:HyperLink ID=lnkAdmin runat=server Text="View the Error Log" Visible=false /><br />
    <br />
    <pre id=litM1 runat=server style="font-size:14px; font-weight:bold; font-family:Arial" />
    <pre id=litS1 runat=server style="font-size:10px" />
    <pre id=litM2 runat=server style="font-size:14px; font-weight:bold; font-family:Arial" />
    <pre id=litS2 runat=server style="font-size:12px" />
</asp:Content>

