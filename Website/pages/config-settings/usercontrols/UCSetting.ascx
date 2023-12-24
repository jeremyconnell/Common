<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCSetting.ascx.vb" Inherits="usercontrols_config_settings_UCSetting" %>

<tr id="row" runat="server">
  <td nowrap>
    <asp:Label ID="lblName" runat="server" />
  </td>
  <td style="width:100%">
    <asp:Label ID="lblReadOnly" runat="server" Visible="false" />
    <asp:TextBox ID="txt" runat="server" Visible="false" Width="100%"   style="padding:1px;margin:0px;"/>
    <asp:CheckBox ID="chk" runat="server" Visible="false" />
    <asp:DropDownList ID="ddList" runat="server" Width="100%" Visible="false" EnableViewState="false" />
  </td>
  <td>
    <asp:HyperLink ForeColor="#666" ID="lnk" runat="server" ToolTip="Edit this Group">Edit</asp:HyperLink>      
  </td>
    <td id="colOrder" runat="server" nowrap>
        <asp:ImageButton ID="btnMoveDn" runat="server" ToolTip="Move Down" ImageUrl="~/images/down.jpg"  />
        <asp:ImageButton ID="btnMoveUp" runat="server" ToolTip="Move Up"   ImageUrl="~/images/up.jpg"   />
    </td>
  <td>
    <asp:ImageButton ID="btnDelete" runat="server" ToolTip="Delete this Setting"  ImageUrl="~/images/delete.png" OnClientClick="return confirm('Delete this Setting?')" />
  </td>
</tr>
