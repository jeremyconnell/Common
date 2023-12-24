<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCForeignKey.ascx.cs" Inherits="pages_binaryFiles_usercontrols_UCForeignKey" %>
<div style="margin-bottom:5px" id="div" runat="server">
    <asp:Label ID="lblRefTable" runat="server" Font-Bold="true" /><br />
    <asp:Label ID="lblColumns" runat="server" /><br />
    <asp:Label ID="lblKeyName" runat="server" />
</div>
<tr id="row" runat="server" visible="false">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td>
        <asp:Label ID="lblName" runat="server" Font-Bold="true" />
    </td>
    <td>
        <asp:Label ID="lblHash" runat="server"  />
    </td>
    <td>
        <asp:Label ID="lblTable" runat="server"/>
    </td>
    <td>
        <asp:Label ID="lblRef" runat="server" />
    </td>
    <td>
        <asp:Label ID="lblCols" runat="server" />
    </td>
    <td>
        <asp:Label ID="lblRefCols" runat="server" />
    </td>
    <td>
        <asp:Label ID="lblCascadeUpdate" runat="server" />
    </td>
    <td>
        <asp:Label ID="lblCascadeDelete" runat="server" />
    </td>
    <td style="padding:3px 3px">
            <pre id="litScript" runat="server" style="margin-top:0px; margin-bottom:0px; height:100%; max-height:150px; max-width=300px; overflow:auto"/>
    </td>
</tr>