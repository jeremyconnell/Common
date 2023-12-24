<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCDateTimePicker.ascx.vb" Inherits="usercontrols_extensions_UCDateTimePicker" %>
<%@ Register src="layout/UCContainerBegin.ascx" tagname="UCContainerBegin"  tagprefix="uc" %>
<%@ Register src="layout/UCContainerEnd.ascx"   tagname="UCContainerEnd"    tagprefix="uc" %>
<%@ Register src="layout/UCSeparator1.ascx"     tagname="UCSeparator1"      tagprefix="uc" %>
<%@ Register src="layout/UCSeparator2.ascx"     tagname="UCSeparator2"      tagprefix="uc" %>
<uc:UCContainerBegin ID="_st" runat="server" />
  <asp:Literal ID="litLabel" runat="server" />
<uc:UCSeparator1 ID="_s1" runat="server" />
  <asp:PlaceHolder ID="ctrl" runat="server">
      <table cellpadding="0" cellspacing="0" id="tbl" runat="server">
        <tr>
          <td>          
            <fieldset style="margin:0px; padding-top:0px">
              <legend style="margin:0px;">Date</legend>
                <asp:TextBox ID="ctrlDate" runat="server" CssClass="textbox" Width="100" />
            </fieldset>
          </td>
          <td><uc:Timepicker ID="ctrlTime" runat="server" /> </td>
        </tr>
      </table>
  </asp:PlaceHolder>
  <asp:Label ID="_l" runat="server" CssClass="locked" Visible="false"/>
  <asp:HyperLink ID="lnk" runat="server" CssClass="locked" Visible="false" />
  <asp:HiddenField ID="_h" runat="server" Value="0" />
  <div><asp:Label ID="lblDescription" runat="server" /></div>
<uc:UCSeparator2 ID="_s2" runat="server" />
  <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="ctrlDate" Display="Dynamic" Text="*" Enabled="false" CssClass="required" />
  <asp:CustomValidator ID="cv" runat="server" Display="Dynamic" Text="*" Enabled="false" EnableClientScript="true"    />
<uc:UCContainerEnd ID="_et" runat="server" />
