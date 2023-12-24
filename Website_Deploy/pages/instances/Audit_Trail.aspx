<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Audit_Trail.aspx.vb" Inherits="pages_instances_Audit_Trail"
  MasterPageFile="~/masterpages/deploy.master"
  Title="Audit Trail"
%>

<%@ Register src="~/pages/audit-trail/usercontrols/UCFilter.ascx" tagname="UCFilter" tagprefix="uc" %>
<%@ Register src="~/pages/audit-trail/usercontrols/UCTrail.ascx"  tagname="UCTrail"  tagprefix="uc" %>

<asp:Content ID="h" runat="server" ContentPlaceHolderID="head">
    <link rel="Stylesheet" type="text/css" href="../../audit-trail/audit-trail.css" />
</asp:Content>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged" />

    <span style="font-weight:bold; font-size:smaller">Deploy:</span> 
    <asp:DropDownList ID="ddIns" Runat="Server" DataTextField="NameAndSuffix" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddIns_SelectedIndexChanged" />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
  <table cellpadding=0 cellspacing=0>
    <tr>
      <td valign=top>
        <table class="filters" cellpadding="3" style="margin-top:2px">
          <tr>
            <td><strong>Table*</strong></td>
            <td><asp:DropDownList ID="ddTable" runat="server" AutoPostBack="true" /></td>
            <td><strong>PK*</strong></td>
            <td><asp:TextBox ID="txtPrimaryKey" runat="server" Width="75px" /></td>
            <td><strong>Type*</strong></td>
            <td><asp:DropDownList ID="ddType" runat="server" AutoPostBack="true" /></td>
            <td><strong>User</strong></td>
            <td><asp:DropDownList ID="ddUser" runat="server" AutoPostBack="true" /></td>
            <td><strong>Date</strong></td>
            <td><asp:TextBox ID="txtDate" runat="server" Width="75" /></td>
            <td><asp:Button ID="btnSearch" runat="server" Text="Search" /></td>
          </tr>
        </table>
      </td>
      <td width=20>&nbsp;</td>
      <td valign=top>
        <table class="filters" cellpadding="1" style="margin-top:2px">
          <asp:PlaceHolder ID=plhFilters runat=server />
          <tr>
            <td><asp:TextBox ID=txtColumnName runat=server Text="" Width="120" onfocus="select()" ToolTip="Enter a column name to search" /></td>
            <td><asp:TextBox ID=txtColumnValue runat=server Text="" Width="125" onfocus="select()" ToolTip="Enter a value to match, using * for wildcards" /></td>
            <td><asp:ImageButton ID=btnAdd runat=server ImageUrl="~/images/add.png" ToolTip="Add a Custom Search Filter" />
          </tr>
        </table>
      </td>
    </tr>
  </table>
  
  <uc:Paging ID="ctrlPaging" runat="server" PageSize="50" />
  
  <br />
  <strong>Differences</strong>
  <table class="datagrid" style="margin-top:2px">
    <thead>
      <tr align="left">
        <th>Date</th>
        <th>Time</th>
        <th>Table</th>
        <th>PK</th>
        <th>User</th>
        <th>Type/Url</th>
        <th align="left" width="100%">
          Affected Columns (Before/After)
          <asp:CheckBox ID="chkShowUnchanged" runat="server" AutoPostBack="true" Text="Show All Columns" />
        </th>
      </tr>
    </thead>
    <tbody id="tbody" runat="server" />
  </table>      
</asp:Content>
