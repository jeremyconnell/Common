<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dataImport.aspx.cs" Inherits="pages_instances_dataImport"
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
    <div style="margin-bottom:10px">
        <asp:DropDownList ID="ddRiskRatingScheme" Runat="Server" DataTextField="Name" DataValueField="RiskRatingSchemeID" />
        <asp:DropDownList ID="ddRiskRegister"     Runat="Server" DataTextField="Name" DataValueField="RiskRegisterID" />
    </div>

    <div>
        <asp:FileUpload ID="fi" runat="server" />
        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
        <asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click" />
    </div>

    <div id="divUpl" runat="server" visible="false">
        <h3>Imported</h3>
        <asp:DataGrid ID="dg" runat="server" AutoGenerateColumns="true" CssClass="datagrid" HeaderStyle-CssClass="thead" />
    </div>
    
    <div id="divExisting" runat="server" visible="false">
        <h3>Existing</h3>
        <asp:DataGrid ID="ddCat" runat="server" AutoGenerateColumns="true" CssClass="datagrid" HeaderStyle-CssClass="thead" />
        <asp:DataGrid ID="ddCon" runat="server" AutoGenerateColumns="true" CssClass="datagrid" HeaderStyle-CssClass="thead" />
        <asp:DataGrid ID="dgExisting" runat="server" AutoGenerateColumns="true" CssClass="datagrid" HeaderStyle-CssClass="thead" />
    </div>
    
</asp:Content>

<asp:Content ID="s" runat="server" ContentPlaceHolderID="side">
    <div style="margin:10px">
        <asp:Button ID="btnDeleteInactive" runat="server" Text="Delete Inactive Records" OnClick="btnDeleteInactive_Click" />
    </div>
</asp:Content>