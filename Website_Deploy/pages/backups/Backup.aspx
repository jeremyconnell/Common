<%@ Page EnableEventValidation="false" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="backup.aspx.cs" Inherits="pages_Backups_Backup"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Database Backup"
    ValidateRequest="False"
%>

<%@ Register src="~/pages/backupItems/usercontrols/UCBackupItems.ascx" tagname="UCBackupItems" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <uc:FormBegin ID="fb" runat="server" />
        <uc:Textbox  ID="txtBackupCreated" runat="server" Mode="Locked" Label="Created" />
        <uc:Dropdown ID="lnkBackupInstanceId" Runat="server" Mode="Locked" Label="Instance" Tooltip="Click to view this Instance" />


        <uc:Textbox  ID="litTables" runat="server" Mode="Locked" Label="Tables" />
        <uc:Textbox  ID="litBinaries" runat="server" Mode="Locked" Label="Stored" />
        <uc:Textbox  ID="litTotalSize" runat="server" Mode="Locked" Label="Size" />

        <uc:Textbox  ID="txtBackupDescription" runat="server" Mode="Locked" Label="Log" />
    <uc:FormEnd ID="fe" runat="server" />
</asp:Content>

<asp:Content ID="b" runat="server" ContentPlaceHolderID="below">
    <uc:UCBackupItems ID="ctrlBackupItems" runat="server" />
</asp:Content>