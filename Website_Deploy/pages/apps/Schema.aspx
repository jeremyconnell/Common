<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Schema.aspx.cs" Inherits="pages_instances_Schema"
  MasterPageFile="~/masterpages/deploy.master"
  Title="Schema"
%>

<%@ Register src="~/pages/self/usercontrols/UCSchemaDiff.ascx" tagname="SchemaDiff" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    

    <uc:SchemaDiff id="ctrl" runat="server" />


</asp:Content>
