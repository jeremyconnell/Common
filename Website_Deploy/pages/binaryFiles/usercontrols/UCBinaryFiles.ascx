<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCBinaryFiles.ascx.cs" Inherits="pages_BinaryFiles_usercontrols_UCBinaryFiles" %>
<%@ Register tagname="UCBinaryFile" tagprefix="uc" src="UCBinaryFile.ascx" %>
<h3 id="h3" runat="server" visible="false" />
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server">#</th>
            <th><asp:LinkButton Text="Path" CommandArgument="Path" id="btnSortByPath" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Size" CommandArgument="Size" id="btnSortBySize" runat="server" OnClick="btnResort_Click" /></th>
            <th id="colUsg" runat="server"><asp:LinkButton Text="N" CommandArgument="Usage" id="LinkButton1" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Created" CommandArgument="Created" id="btnSortByCreated" runat="server" OnClick="btnResort_Click" /></th>
            <th id="colDel" runat="server"><asp:LinkButton Text="Deleted" CommandArgument="Deleted" id="btnSortByDeleted" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Hash" CommandArgument="MD5" id="LinkButton2" runat="server" OnClick="btnResort_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCBinaryFile.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="20" />
