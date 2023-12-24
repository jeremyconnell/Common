<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCView.ascx.cs" Inherits="pages_binaryFiles_usercontrols_UCView" %>
<%@ Register src="UCColumn.ascx" tagname="UCColumn" tagprefix="uc" %>

<tr id="row" runat="server">
    <td><b><asp:Literal ID="litNumber" runat="server" />.</b></td>
    <td>
        <asp:Label ID="lblProc" runat="server" Font-Bold="true" />
        <div style="margin-top:5px; text-align:right">
            <asp:Label ID="lblHash" runat="server" />
        </div>
    </td>
    <td style="padding:3px 0px">
        <div  style="max-height:150px; overflow:auto">
            <table>
                 <asp:PlaceHolder ID="plhCols" runat="server" />
            </table>
        </div>
    </td>
    <td style="padding:3px 3px">
        <pre id="lblScript" runat="server" style="margin-top:0px; height:100%; max-height:150px; overflow:auto"/>
    </td>
</tr>
