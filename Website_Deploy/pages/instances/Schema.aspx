<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Schema.aspx.cs" Inherits="pages_instances_Schema"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Deployment Schema"
    ValidateRequest="False"
%>
<%@ Register src="~/pages/self/usercontrols/UCSChemaDiff.ascx" tagname="SchemaDiff" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged" />

    <span style="font-weight:bold; font-size:smaller">Deploy:</span> 
    <asp:DropDownList ID="ddIns" Runat="Server" DataTextField="NameAndSuffix" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddIns_SelectedIndexChanged" />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    
         <uc:SchemaDiff id="ctrlSchema" runat="server" />
</asp:Content>

