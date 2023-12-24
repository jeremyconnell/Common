<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCTextArea.ascx.vb" Inherits="usercontrols_extensions_UCTextArea" %>
<%@ Register src="layout/UCContainerBegin.ascx" tagname="UCContainerBegin"  tagprefix="uc" %>
<%@ Register src="layout/UCContainerEnd.ascx"   tagname="UCContainerEnd"    tagprefix="uc" %>
<%@ Register src="layout/UCSeparator1.ascx"     tagname="UCSeparator1"      tagprefix="uc" %>
<%@ Register src="layout/UCSeparator2.ascx"     tagname="UCSeparator2"      tagprefix="uc" %>
<uc:UCContainerBegin ID="_st" runat="server" />
  <asp:Literal ID="litLabel" runat="server" />
<uc:UCSeparator1 ID="_s1" runat="server" />
  <asp:TextBox ID="ctrl" runat="server" TextMode="MultiLine"  Wrap="true" />
  <asp:Label ID="_l" runat="server" CssClass="locked" Visible="false"/>
  <asp:HiddenField ID="_h" runat="server" Value="0" />
  <div><asp:Label ID="lblDescription" runat="server" /></div>
<uc:UCSeparator2 ID="_s2" runat="server" />
  <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="ctrl" Display="Dynamic" Text="*" Enabled="false" CssClass="required" EnableClientScript="true" />
  <asp:CustomValidator ID="cv" runat="server" Display="Dynamic" Text="*" Enabled="false" EnableClientScript="true"    />
  <asp:CustomValidator ID="valMaxLength" runat="server" Enabled="false" ErrorMessage="Maxlength has been exceeded" ValidateEmptyText="false" Display=Dynamic Text="*" />
<uc:UCContainerEnd ID="_et" runat="server" />


<asp:PlaceHolder ID=plhScript runat=server>
    <script>
        //<![CDATA[
        // Allows tabbing in a textarea (if wrap=false)
        // Textarea : onkeydown="return UCTextarea_AllowTab(this, event)"
        var TAB = "    "; //"\t"
        function UCTextarea_AllowTab(sender, e) {
            if (e.keyCode != 9)
                return true;

            if (e.srcElement) {
                sender.selection = document.selection.createRange();
                sender.selection.text = TAB;
            }
            else if (e.target) {
                var start = sender.value.substring(0, sender.selectionStart);
                var end = sender.value.substring(sender.selectionEnd, sender.value.length);
                var newRange = sender.selectionEnd + TAB.length;
                var scrollPos = sender.scrollTop;
                sender.value = String.concat(start, TAB, end);
                sender.setSelectionRange(newRange, newRange);
                sender.scrollTop = scrollPos;
            }
            return false;
        } 
        //]]>
    </script>
</asp:PlaceHolder>
    