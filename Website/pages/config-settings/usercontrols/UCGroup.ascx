<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCGroup.ascx.vb" Inherits="usercontrols_config_settings_UCGroup" %>

<%@ Register src="UCSetting.ascx" tagname="UCSetting" tagprefix="uc" %>

<thead>
  <tr>
    <th width="100%" colspan="2">
     <asp:Literal ID="litGroupName" runat="server" />
    </th>
    <th>
      <asp:HyperLink ForeColor="White" ID="lnk" runat="server" ToolTip="Edit this Group">Edit</asp:HyperLink>      
    </th>
    <th id="colOrder" runat="server" nowrap>
        <asp:ImageButton ID="btnMoveDn" runat="server" ToolTip="Move Down" ImageUrl="~/images/downInverse.jpg"  />
        <asp:ImageButton ID="btnMoveUp" runat="server" ToolTip="Move Up"   ImageUrl="~/images/upInverse.jpg"    />
    </th>
    <th>
      <asp:HyperLink ID="lnkAdd" runat="server" ToolTip="Add a new Setting" ImageUrl="~/images/add.png" />
    </th>
  </tr>
</thead>
<tbody>
      <asp:PlaceHolder ID="plh" runat="server" />
</tbody>
