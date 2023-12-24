<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCRadioButtonList.ascx.vb" Inherits="usercontrols_extensions_UCRadioButtonList" %>
<%@ Register src="layout/UCContainerBegin.ascx" tagname="UCContainerBegin"  tagprefix="uc" %>
<%@ Register src="layout/UCContainerEnd.ascx"   tagname="UCContainerEnd"    tagprefix="uc" %>
<%@ Register src="layout/UCSeparator1.ascx"     tagname="UCSeparator1"      tagprefix="uc" %>
<%@ Register src="layout/UCSeparator2.ascx"     tagname="UCSeparator2"      tagprefix="uc" %>
<uc:UCContainerBegin ID="_st" runat="server" />
  <asp:Literal ID="litLabel" runat="server" />
<uc:UCSeparator1 ID="_s1" runat="server" />
  <asp:RadioButtonList ID="ctrl" runat="server" EnableViewState="false"  CellPadding="0" CellSpacing="0" />
  <asp:Label ID="_l" runat="server" CssClass="locked" Visible="false"/>
  <asp:HiddenField ID="_h" runat="server" Value="0" />
<uc:UCSeparator2 ID="_s2" runat="server" />
  <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="ctrl" Display="Dynamic" Text="*" Enabled="false"  />
  <asp:CustomValidator ID="cv" runat="server" Display="Dynamic" Text="*" Enabled="false" EnableClientScript="true"    />
<uc:UCContainerEnd ID="_et" runat="server" />
