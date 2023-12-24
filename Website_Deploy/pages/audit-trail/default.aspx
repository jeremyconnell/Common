<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="AuditTrail"
MasterPageFile="~/masterpages/default.master"
Title="Audit Trail"
%>
<%@ Register src="usercontrols/UCFilter.ascx" tagname="UCFilter" tagprefix="uc" %>
<%@ Register src="usercontrols/UCTrail.ascx"  tagname="UCTrail"  tagprefix="uc" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <link rel="Stylesheet" type="text/css" href="audit-trail.css" />
</asp:Content>

<asp:Content ID="c" runat="server" ContentPlaceHolderID="body">
  
  <table cellpadding=0 cellspacing=0>
    <tr>
      <td valign=top>
        <strong>General Search Filters (* = Indexed field)</strong>
        <table class="filters" cellpadding="3" style="margin-top:2px">
          <tr>
            <td><strong>Table*</strong></td>
            <td><asp:DropDownList ID="ddTable" runat="server" AutoPostBack="true" /></td>
            <td><strong>Type*</strong></td>
            <td><asp:DropDownList ID="ddType" runat="server" AutoPostBack="true" /></td>
            <td><strong>User</strong></td>
            <td><asp:DropDownList ID="ddUser" runat="server" AutoPostBack="true" /></td>
            <td><strong>Date</strong></td>
            <td><asp:TextBox ID="txtDate" runat="server" Width="75" /></td>
          </tr>
          <tr>
            <td><strong>PK*</strong></td>
            <td  style="padding-right:10px"><asp:TextBox ID="txtPrimaryKey" runat="server" Width="100%" /></td>
            <td><strong>Url</strong></td>
            <td colspan="4"><asp:DropDownList ID="ddUrl" runat="server" AutoPostBack="true"  Width="100%"/></td>
            <td align="right"><asp:Button ID="btnSearch" runat="server" Text="Search" Width="100%" /></td>
          </tr>
        </table>
      </td>
      <td width=20>&nbsp;</td>
      <td valign=top>
        <strong>Custom Search Filters</strong>
        <table class="filters" cellpadding="1" style="margin-top:2px">
          <thead>
            <tr>
              <th align=left>Column Name</th>
              <th align=left nowrap colspan=2>Value <span style="font-weight:normal">(Use * for wildcard)</span></th>
            </tr>
          </thead>
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