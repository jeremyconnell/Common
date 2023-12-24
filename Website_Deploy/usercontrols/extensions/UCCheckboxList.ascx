<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCCheckboxList.ascx.vb" Inherits="usercontrols_extensions_UCCheckboxList" %>
<%@ Register src="layout/UCContainerBegin.ascx" tagname="UCContainerBegin"  tagprefix="uc" %>
<%@ Register src="layout/UCContainerEnd.ascx"   tagname="UCContainerEnd"    tagprefix="uc" %>
<%@ Register src="layout/UCSeparator1.ascx"     tagname="UCSeparator1"      tagprefix="uc" %>
<%@ Register src="layout/UCSeparator2.ascx"     tagname="UCSeparator2"      tagprefix="uc" %>
<uc:UCContainerBegin ID="_st" runat="server" />
  <asp:Literal ID="litLabel" runat="server" />
<uc:UCSeparator1 ID="_s1" runat="server" />
  <div style="max-height:150px; overflow:auto; width:450px" >
    <asp:CheckBoxList ID="ctrl" runat="server" EnableViewState="false" CellPadding="0" CellSpacing="0" CssClass=table />
  </div>
  <asp:Label ID="_l" runat="server" CssClass="locked" Visible="false"/>
  <asp:HiddenField ID="_h" runat="server" Value="0" />
<uc:UCSeparator2 ID="_s2" runat="server" />
  <asp:CustomValidator  ID="rfv" runat="server"  Display="Dynamic" Text="*" Enabled="false"  />
  <asp:CustomValidator ID="cv" runat="server" Display="Dynamic" Text="*" Enabled="false" EnableClientScript="true"    />
  <asp:PlaceHolder ID="plhScript" runat="server" Visible="false">
    <script language="javascript">
      function Validate_<%=ctrl.ClientID%>(oSrc, args) 
      {
        var cbl =document.all["<%=ctrl.ClientID%>"]; 
        args.IsValid = RecurseChildrenForACheckedCheckbox(cbl);
      }
      function RecurseChildrenForACheckedCheckbox(obj)
      {
        if (null!=obj.tagName && obj.tagName.toLowerCase()=="input" && obj.checked == true){ return true; }
        if (null == obj.childNodes) { return false; }
        for (i = 0; i<obj.childNodes.length; i++){if ( RecurseChildrenForACheckedCheckbox(obj.childNodes[i]) ){ return true; } }
        return false;
      }
    </script>
  </asp:PlaceHolder>
<uc:UCContainerEnd ID="_et" runat="server" />