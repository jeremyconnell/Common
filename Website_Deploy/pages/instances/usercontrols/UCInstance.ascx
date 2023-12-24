<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCInstance.ascx.cs" Inherits="pages_instances_usercontrols_UCInstance" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td><b><asp:HyperLink ID="lnkInstanceName" Runat="server" /></b></td>
    <td><asp:HyperLink ID="litInstanceClientId" runat="server" Font-Bold="true" /><asp:DropDownList ID="ddClient" runat="server" DataTextField="NameAndInstanceCount" DataValueField="ClientId" AutoPostBack="true" OnSelectedIndexChanged="ddClient_SelectedIndexChanged" /> </td>
    <td><asp:HyperLink Target="_blank" ID="lblLogin" runat="server" /></td>
    <td><asp:Label ID="lblPass" runat="server" Font-Size="Smaller"  /></td>
    <td style="padding:2px 2px" class="bl"><asp:HyperLink ID="lnkTargetVersionId" runat="server" /></td>
    <td style="padding:2px 2px"><asp:HyperLink ID="lnkTargetVersionSchema" runat="server" /></td>
    <td style="padding:2px 2px" class="bl"><asp:HyperLink ID="litLastVersion" runat="server"  /></td>
    <td style="padding:2px 2px"><asp:HyperLink ID="litLastSchema" runat="server"   /></td>
    <td class="bl"><asp:HyperLink ID="lnkValuesCount" runat="server" /></td>
    <td><asp:HyperLink ID="litLastReport" runat="server" Font-Size="Smaller" /></td>
    <td><asp:Label Font-Size="Smaller" ID="litInstanceCreated" runat="server" /></td>
    <td style="padding:1px 1px"><asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Right" ImageUrl="~/images/delete.png" ToolTip="Delete this Instance" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this Instance?')" /></td>
</tr>
