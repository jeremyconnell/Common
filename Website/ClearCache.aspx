<%@ Page Title="Clear Cache" Language="C#" MasterPageFile="~/pages/default.master" AutoEventWireup="true" CodeFile="ClearCache.aspx.cs" Inherits="ClearCache" %>

<asp:Content ID="b" ContentPlaceHolderID="body" Runat="Server">
    <div>
        Cached Cleared: <asp:Literal ID=litDate runat=server />
    </div>
</asp:Content>