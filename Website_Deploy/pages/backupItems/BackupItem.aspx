<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="backupItem.aspx.cs" Inherits="pages_BackupItems_BackupItem"
  MasterPageFile="~/masterpages/deploy.master"
  Title="Dataset"
%>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <uc:FormBegin ID="fb" runat="server" />
        <uc:Textbox  ID="txtDeploy" runat="server" Mode="Locked" Label="Client" />
        <uc:Dropdown ID="ddTable" runat="server" AutoPostback="true" Label="Table" OnSelectedIndexChanged="ddTable_SelectedIndexChanged" />

        <uc:Textbox  ID="txtItemSchemaXmlGz" runat="server" Mode="Locked" Label="Schema" />
        <uc:Textbox  ID="txtItemDatasetXmlGz" runat="server" Mode="Locked" Label="Data" />

        <uc:Textbox  ID="txtCreated" runat="server" Mode="Locked" Label="Created" />
    <uc:FormEnd ID="fe" runat="server" />

</asp:Content>

<asp:Content ID="b" runat="server" ContentPlaceHolderID="below">

    <asp:DataGrid ID="dg" runat="server" CssClass="datagrid" AutoGenerateColumns="true" EnableViewState="false" style="margin-top:50px">
        <HeaderStyle CssClass="thead" />
        <AlternatingItemStyle CssClass="alt_row" />
    </asp:DataGrid>
</asp:Content>