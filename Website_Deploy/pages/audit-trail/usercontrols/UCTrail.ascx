<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCTrail.ascx.vb" Inherits="usercontrols_audit_trail_UCTrail" %>
<%@ Register src="UCDiff.ascx" tagname="UCDiff" tagprefix="uc" %>
<tr valign="top" id="row" runat="server">
  <td nowrap>
    <asp:HyperLink ID="lnkDate" runat="server" />
  </td>
  <td nowrap>
    <asp:HyperLink ID="lnkTime" runat="server" />
  </td>
  <td>
    <asp:Label ID="lblTable" runat="server" />
  </td>
  <td>
    <asp:Literal ID="litPrimaryKey" runat="server" />
  </td>
  <td nowrap>
    <asp:HyperLink ID="lnkUser" runat="server" />
  </td>
  <td>
    <asp:HyperLink ID="lnkType" runat="server" Target="_blank" ToolTip="Url where action occurred" />
  </td>
  <td colspan="3">
      <table class=diff cellpadding="0" cellspacing="1" border="1">
        <colgroup>
          <col />
          <col  />
          <col  />
          <col  />
        </colgroup>
        <tbody id="tbody" runat="server" />
      </table>
  </td>
</tr>


