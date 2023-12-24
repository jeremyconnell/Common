<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCDataDiff.ascx.vb" Inherits="pages_self_usercontrols_UCDataDiff"  EnableViewState="false" ValidateRequestMode="Disabled" %>
<style>
    table.datagrid td {
        padding: 1px 5px;
    }
    table.datagrid th {
        padding: 1px 5px;
    }

    input[type="checkbox" i] {
        margin: 1px;
    }
</style>
<table>
    <tr>
        <td valign="top">
            <table class="datagrid">
                <tr>
                    <th>Source</th>
                    <td><asp:DropDownList id="ddSource" runat="server" AutoPostBack="true" DataTextField="NameAndSuffix" DataValueField="InstanceId" /></td>
                </tr>
                <tr>
                    <th>Target</th>
                    <td><asp:DropDownList id="ddTarget" runat="server" AutoPostBack="true" DataTextField="NameAndSuffix" DataValueField="InstanceId" Enabled="false" /></td>
                </tr>
                <tr>
                    <th>Table</th>
                    <td><asp:DropDownList id="ddTable" runat="server" AutoPostBack="true" /></td>
                </tr>
            </table>
        </td>
        <asp:PlaceHolder ID="plh" runat="server">
        <td valign="top" id="colSummary" runat="server">
            <table class="datagrid" style="margin-left:50px">
                <tr>
                    <th>#</th>
                    <th>Rows</th>
                    <th>Data</th>
                    <th>IDs</th>
                </tr>
                <tr>
                    <th>Source</th>
                    <td align="right"><asp:Label ID="lblSourceTotal" runat="server" /></td>
                    <td align="right"><asp:Label ID="lblSourceOnlyByMd5" runat="server" ForeColor="Red" Font-Bold="true" /></td>
                    <td align="right"><asp:Label ID="lblSourceOnlyById" runat="server" ForeColor="Red" Font-Bold="true" /></td>
                </tr>
                <tr>
                    <th>Target</th>
                    <td align="right"><asp:Label ID="lblTargetTotal" runat="server" /></td>
                    <td align="right"><asp:Label ID="lblTargetOnlyByMd5" runat="server" ForeColor="Red" Font-Bold="true" /></td>
                    <td align="right"><asp:Label ID="lblTargetOnlyById" runat="server" ForeColor="Red" Font-Bold="true" /></td>
                </tr>
                <tr>
                    <th>Both</th>
                    <td align="right"><asp:Label ID="lblExactMatch" runat="server" /></td>
                    <td align="right"><asp:Label ID="lblMatchingByMd5" runat="server" /></td>
                    <td align="right"><asp:Label ID="lblMatchingById" runat="server" /></td>
                </tr>
            </table>
        </td>
        <td valign="top" style="padding-top:10px">
            <asp:Button ID="btnFixIds"  runat="server" Text="Fix IDs*" /> <br />
            <asp:Button ID="btnFixData" runat="server" Text="Fix Data" />
        </td>

        </asp:PlaceHolder>
    </tr>
</table>

<asp:Table ID="tblCounts" runat="server" CssClass="datagrid" />

<asp:PlaceHolder ID="plhSingle" runat="server">

    <div id="pnlSourceOnly" runat="server" style="max-width:1600px; max-height:450px; overflow:auto; border:1px solid #ddd; margin-top:40px; padding-bottom:-20px; background:#dde">
        <div id="pnlSourceOnlyByMd5" runat="server" style="text-align:center"><b>Source-Only (By MD5):</b> <asp:Label ID="lblSourceOnlyByMD5Count" runat="server" /></div>
        <div style="max-height:200px; overflow:auto">
        <asp:DataGrid ID="dgSourceOnlyByHash" runat="server" CssClass="datagrid" AutoGenerateColumns="true">
            <HeaderStyle Font-Bold="true" BackColor="DimGray" ForeColor="white" />
            <AlternatingItemStyle CssClass="alt_row" />
            <ItemStyle Wrap="false" />
        </asp:DataGrid>
        </div>

        <div id="pnlSourceOnlyByPK" runat="server" style="text-align:center; margin-top:10px"><b>Source-Only (By ID):</b> <asp:Label ID="lblSourceOnlyByPkCount" runat="server" /> </div>
        <div style="max-height:200px; overflow:auto">
            <asp:DataGrid ID="dgSourceOnlyById" runat="server" CssClass="datagrid" AutoGenerateColumns="true">
                <HeaderStyle Font-Bold="true" BackColor="DimGray" ForeColor="white" />
                <AlternatingItemStyle CssClass="alt_row" />
                <ItemStyle Wrap="false" />
            </asp:DataGrid>
        </div>
    </div>
    <div id="pnlTargetOnly" runat="server" style="max-width:1600px; max-height:450px; overflow:auto; border:1px solid #ddd; margin-top:40px; background:#edd">
        <div id="pnlTargetOnlyByMd5" runat="server" style="text-align:center"><b>Target-Only (By MD5): </b> <asp:Label ID="lblTargetOnlyByMd5Count" runat="server" /> </div>
        <div style="max-height:200px; overflow:auto">
            <asp:DataGrid ID="dgTargetOnlyByHash" runat="server" CssClass="datagrid" AutoGenerateColumns="true">
                <HeaderStyle Font-Bold="true" BackColor="DimGray" ForeColor="white" />
                <AlternatingItemStyle CssClass="alt_row" />
                <ItemStyle Wrap="false" />
            </asp:DataGrid>
        </div>
    
        <div id="pnlTargetOnlyByPK" runat="server" style="text-align:center; margin-top:10px"><b>Target-Only (By ID): </b> <asp:Label ID="lblTargetOnlyByPkCount" runat="server" /> </div>
        <div style="max-height:200px; overflow:auto">
            <asp:DataGrid ID="dgTargetOnlyById" runat="server" CssClass="datagrid" AutoGenerateColumns="true">
                <HeaderStyle Font-Bold="true" BackColor="DimGray" ForeColor="white" />
                <AlternatingItemStyle CssClass="alt_row" />
                <ItemStyle Wrap="false" />
            </asp:DataGrid>
        </div>
    </div>
    <div id="pnlBoth" runat="server" style="max-width:1600px; max-height:200px; overflow:auto; border:1px solid #ddd; margin-top:40px; background:#ded">
        <div style="text-align:center"><b>Matching (By MD5)</b></div>
        <asp:DataGrid ID="dgBoth" runat="server" CssClass="datagrid" AutoGenerateColumns="true">
            <HeaderStyle Font-Bold="true" BackColor="DimGray" ForeColor="white" />
            <AlternatingItemStyle CssClass="alt_row" />
            <ItemStyle Wrap="false" />
        </asp:DataGrid>
    </div>

</asp:PlaceHolder>