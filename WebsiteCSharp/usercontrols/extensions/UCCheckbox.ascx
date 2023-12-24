<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCCheckbox.ascx.vb" Inherits="usercontrols_extensions_UCCheckbox" %>
<%@ Register src="layout/UCContainerBegin.ascx" tagname="UCContainerBegin"  tagprefix="uc" %>
<%@ Register src="layout/UCContainerEnd.ascx"   tagname="UCContainerEnd"    tagprefix="uc" %>
<%@ Register src="layout/UCSeparator1.ascx"     tagname="UCSeparator1"      tagprefix="uc" %>
<%@ Register src="layout/UCSeparator2.ascx"     tagname="UCSeparator2"      tagprefix="uc" %>
<uc:UCContainerBegin ID="_st" runat="server" />
    <asp:Literal ID="litLabel" runat="server" />
<uc:UCSeparator1 ID="_s1" runat="server" />
  <!-- Table is needed if css layout, to make the required asterisk at the end line up -->
  <table cellpadding="0" cellspacing="0">
    <tr>
      <td>
        <asp:CheckBox ID="ctrl" runat="server" CssClass="checkbox" />
        <asp:Label ID="_l" runat="server" CssClass="locked" Visible="false"/>
        <asp:HiddenField ID="_h" runat="server" Value="0" />
        <asp:Label ID="lblDescription" runat="server" />
      </td>
    </tr>
  </table>
<uc:UCSeparator2 ID="_s2" runat="server" />
  <asp:CustomValidator  ID="rfv" runat="server"  Display="Dynamic" Text="*" Enabled="false"  />
  <asp:CustomValidator ID="cv" runat="server" Display="Dynamic" Text="*" Enabled="false" EnableClientScript="true"    />
  <asp:PlaceHolder ID="plhScript" runat="server" Visible="false">
    <script language="javascript">
      function Validate_<%=ctrl.ClientID%>(oSrc, args) { args.IsValid = document.all["<%=ctrl.ClientID%>"].checked; }
    </script>
  </asp:PlaceHolder>
<uc:UCContainerEnd ID="_et" runat="server" />
