<%@ Control Language="vb" AutoEventWireup="false" CodeFile="UCUserRoles.ascx.vb" Inherits="pages_UserRoles_usercontrols_UCUserRoles" %>
<%@ Register tagname="UCUserRole" tagprefix="uc" src="UCUserRole.ascx" %>
<table class="datagrid" width=450>
  <colgroup>
    <col width=20 style="text-align:right" />
  </colgroup>
  <thead>
    <tr>
      <th colspan="2" id=colMain runat=server>
        <asp:Literal ID="litSelected" runat="server" />
        <asp:Literal ID="litTitle"    runat="server" />
        (<asp:Literal ID=litCount runat=server />)
      </th>
      <th id=colDate runat=server visible=false>Since</th>
      <th>&nbsp;</th>
    </tr>
  </thead>
  <tbody id="plh" runat="server" enableviewstate="false"/>
</table>
<uc:Paging ID="ctrlPaging" runat="server" PageSize="20" />
