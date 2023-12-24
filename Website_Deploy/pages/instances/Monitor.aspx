<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Monitor.aspx.cs" Inherits="pages_clients_Database"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Monitor"
    ValidateRequest="False"
%>

<%@ Register src="usercontrols/UCPublishProfile.ascx" tagname="UCPublishProfile" tagprefix="uc" %>
<%@ Register src="~/pages/instances/usercontrols/UCMonitor.ascx" tagname="UCMonitor" tagprefix="uc" %>

<asp:Content ID="a" ContentPlaceHolderID="above" runat="server" >
    
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged"/>

    <span style="font-weight:bold; font-size:smaller">Deploy:</span> 
    <asp:DropDownList ID="ddInstance" runat="server" DataTextField="NameAndSuffix" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddInstance_SelectedIndexChanged" /><br />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <uc:FormBegin ID="FormBegin1" runat="server" />

        <uc:UCMonitor id="ctrlMonitor" runat="server" Visible="false" />
    
    <uc:FormEnd ID="fe" runat="server" />



</asp:Content>