<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VersionDiff.aspx.cs" Inherits="pages_binaryFiles_VersionDiff"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Schema"
    ValidateRequest="False"
    EnableViewState="false"
%>

<%@ Register src="usercontrols/UCBinaryFiles.ascx" tagname="UCBinaryFiles" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <asp:DropDownList ID="ddVersion" Runat="Server" DataTextField="AppNameAndVersion" DataValueField="VersionId" AutoPostBack="true" OnSelectedIndexChanged="ddVersion_SelectedIndexChanged"/>
    <asp:DropDownList ID="ddInstance" Runat="Server" DataTextField="AppAndName" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddInstance_SelectedIndexChanged"/>
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    
    <asp:RadioButtonList ID="rbl" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
        <asp:ListItem Value="0">MD5</asp:ListItem>
        <asp:ListItem Value="1">Name</asp:ListItem>
    </asp:RadioButtonList>


    <uc:UCBinaryFiles ID="ctrlNew" runat="server" Title="New"  PageSize="50" QueryString="p1" />
    
    <uc:UCBinaryFiles ID="ctrlDel" runat="server" Title="Del"   PageSize="50" QueryString="p2" />

    <uc:UCBinaryFiles ID="ctrlSame" runat="server" Title="Same" PageSize="50" />

</asp:Content>
