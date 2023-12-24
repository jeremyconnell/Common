<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCTextbox.ascx.vb" Inherits="usercontrols_extensions_UCTextbox" %>
<%@ Register src="layout/UCContainerBegin.ascx" tagname="UCContainerBegin"  tagprefix="uc" %>
<%@ Register src="layout/UCContainerEnd.ascx"   tagname="UCContainerEnd"    tagprefix="uc" %>
<%@ Register src="layout/UCSeparator1.ascx"     tagname="UCSeparator1"      tagprefix="uc" %>
<%@ Register src="layout/UCSeparator2.ascx"     tagname="UCSeparator2"      tagprefix="uc" %>
<uc:UCContainerBegin ID="_st" runat="server" />
  <asp:Literal ID="litLabel" runat="server" />
<uc:UCSeparator1 ID="_s1" runat="server" />
  <asp:TextBox ID="ctrl" runat="server" CssClass="textbox" />
    <asp:PlaceHolder ID="plhDate" runat="server" Visible="false">
        <input type="hidden" id="DPC_TODAY_TEXT" value="today">
        <input type="hidden" id="DPC_BUTTON_TITLE" value="Open calendar...">
        <input type="hidden" id="DPC_MONTH_NAMES" value="['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']">
        <input type="hidden" id="DPC_DAY_NAMES" value="['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']">
    </asp:PlaceHolder>
  <asp:Label ID="_l" runat="server" CssClass="locked" Visible="false"/>
  <asp:HyperLink ID="lnk" runat="server" CssClass="locked" Visible="false" />
  <asp:HiddenField ID="_h" runat="server" Value="0" />
  <div><asp:Label ID="lblDescription" runat="server" /></div>
<uc:UCSeparator2 ID="_s2" runat="server" />
  <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="ctrl" Display="Dynamic" Text="*" Enabled="false" CssClass="required" />
  <asp:CustomValidator ID="cv" runat="server" Display="Dynamic" Text="*" Enabled="false" EnableClientScript="true"    />
<uc:UCContainerEnd ID="_et" runat="server" />
