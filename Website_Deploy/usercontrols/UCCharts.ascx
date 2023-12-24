<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCCharts.ascx.cs" Inherits="Dashboard_Controls_UCCharts" %>
<asp:PlaceHolder ID=plhFirstOneOnly runat=server>
    <script type="text/javascript" src="<%=Request.Url.Scheme %>://www.google.com/jsapi"></script>   
    <script>google.load("visualization", "1", { packages: ["corechart"] });</script>
</asp:PlaceHolder>

<asp:Literal ID=lit runat=server />
