<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCFileUpload.ascx.vb" Inherits="usercontrols_extensions_UCFileUpload" %>
<%@ Register src="layout/UCContainerBegin.ascx" tagname="UCContainerBegin"  tagprefix="uc" %>
<%@ Register src="layout/UCContainerEnd.ascx"   tagname="UCContainerEnd"    tagprefix="uc" %>
<%@ Register src="layout/UCSeparator1.ascx"     tagname="UCSeparator1"      tagprefix="uc" %>
<%@ Register src="layout/UCSeparator2.ascx"     tagname="UCSeparator2"      tagprefix="uc" %>
<uc:UCContainerBegin ID="_st" runat="server" />
  <asp:Literal ID="litLabel" runat="server" />
<uc:UCSeparator1 ID="_s1" runat="server" />
    <div>
      <asp:FileUpload ID="ctrl"      runat="server" CssClass="upload" />
      <asp:Label      ID="_l" runat="server" CssClass="locked">-- No File Uploaded --</asp:Label>
      <asp:HyperLink  ID="lnkLocked" runat="server" CssClass="locked" ToolTip="Click to Preview" Target="_blank"/>
      <asp:ImageButton ID="btnDelete" runat="server" AlternateText="Delete this File" ImageUrl="~/images/delete.png" ToolTip="Delete this file" OnClientClick="return confirm('Delete this file?')"  />
      <asp:HiddenField ID="_h" runat="server" Value="0" />
      <asp:HiddenField ID="hiddenFileName" runat="server" />
    </div>
<uc:UCSeparator2 ID="_s2" runat="server" />
  <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="ctrl" Display="Dynamic" Text="*" Enabled="false" CssClass="required" />
  <asp:CustomValidator ID="cv" runat="server" Display="Dynamic" Text="*" Enabled="false" EnableClientScript="true"    />
<uc:UCContainerEnd ID="_et" runat="server" />

