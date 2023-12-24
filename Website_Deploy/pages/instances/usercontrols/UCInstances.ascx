<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCInstances.ascx.cs" Inherits="pages_Instances_usercontrols_UCInstances" %>
<%@ Register tagname="UCInstance" tagprefix="uc" src="UCInstance.ascx" %>
<table class="datagrid" cellpadding="2" cellspacing="0" summary="">
    <colgroup />
    <thead>
        <tr>
            <th id="colNumber" runat="server"><asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Export To Excel" OnClick="btnExport_Click" /></th>
            <th><asp:LinkButton Text="Instance" CommandArgument="NameAndSuffix" id="btnSortByInstanceName" runat="server" OnClick="btnResort_Click" /></th>
            <th><asp:LinkButton Text="Client" CommandArgument="InstanceSuffix" id="btnSortByInstanceClientId" runat="server" OnClick="btnResort_Click" /></th>
            <th>Login</th>
            <th>Pass</th>
            <th class="bl">
                <asp:LinkButton Text="Target" CommandArgument="TargetVersionId"   id="btnSortByInstanceSpecialVersionId"   runat="server" OnClick="btnResort_Click" />
            </th>
            <th>
                <asp:LinkButton Text="Schema" CommandArgument="TargetVersionId"   id="LinkButton2"   runat="server" OnClick="btnResort_Click" />
            </th>
            <th class="bl" colspan="2">Actual</th>
            <th class="bl"><asp:LinkButton Text="Settings" CommandArgument="ValuesCount" id="btnSortByClientSettingsImported" runat="server" OnClick="btnResort_Click" /></th>
            <th>Inspected</th>
            <th><asp:LinkButton Text="Created" CommandArgument="InstanceCreated" id="btnSortByInstanceCreated" runat="server" OnClick="btnResort_Click" /></th>
            <th style="padding:1px"><asp:ImageButton ID="btnAdd" runat="server" ImageAlign="Right" ImageUrl="~/images/add.png" ToolTip="Add new Instance" OnClick="btnAdd_Click" /></th>
        </tr>
    </thead>
    <tbody>
        <%--UCInstance.ascx--%>
        <asp:PlaceHolder ID="plh" runat="server" EnableViewState="False" />
    </tbody>
</table>

<uc:Paging ID="ctrlPaging" runat="server" PageSize="100" />
