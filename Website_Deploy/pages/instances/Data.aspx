<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Data.aspx.cs" Inherits="pages_instances_Data"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Deployment Data"
    ValidateRequest="False"
%>
<%@ Register src="~/pages/self/usercontrols/UCDataDiff.ascx" tagname="DataDiff" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged" />

    <span style="font-weight:bold; font-size:smaller">Deploy:</span> 
    <asp:DropDownList ID="ddIns" Runat="Server" DataTextField="NameAndSuffix" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddIns_SelectedIndexChanged" />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
           
         <uc:DataDiff id="ctrlData" runat="server" />

</asp:Content>

