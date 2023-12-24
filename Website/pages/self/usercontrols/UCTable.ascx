<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCTable.ascx.cs" Inherits="pages_self_usercontrols_UCTable" %>
<%@ Register src="UCColumn.ascx" tagname="UCColumn" tagprefix="uc" %>
<%@ Register src="UCIndex.ascx" tagname="UCIndex" tagprefix="uc" %>
<%@ Register src="UCForeignKey.ascx" tagname="UCForeignKey" tagprefix="uc" %>
<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td>
        <asp:Label ID="lblTable" runat="server" Font-Bold="true" />
        <div style="margin-top:5px; text-align:right">
            <asp:Label ID="lblHash" runat="server" />
        </div>
    </td>
    <td style="padding:3px 0px">
        <div  style="max-height:150px; overflow:auto">
            <table style="    border-collapse: separate; border:none; ">
                 <asp:PlaceHolder ID="plhCols" runat="server" />
            </table>
        </div>
    </td>
    <td style="padding:3px 3px">
        <div  style="max-height:150px; overflow:auto">
            <table>
                 <asp:PlaceHolder ID="plhFks" runat="server" />
            </table>
        </div>
    </td>
    <td style="padding:3px 3px">
        <div  style="max-height:150px; overflow:auto">
            <table>
                 <asp:PlaceHolder ID="plhIdx" runat="server" />
            </table>
        </div>
    </td>
    <td style="padding:3px 3px">
            <pre id="txtScript" runat="server" style="margin-top:0px; height:100%; max-height:150px; overflow:auto"/>
    </td>
</tr>