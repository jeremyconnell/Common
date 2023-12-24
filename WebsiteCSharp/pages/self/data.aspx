<%@ Page Language="VB" AutoEventWireup="false" CodeFile="data.aspx.vb" Inherits="pages_self_data"
  MasterPageFile="~/pages/default.master"
  Title="Data Sync (Admin)"
  EnableViewState="false"
  EnableEventValidation="false"
%>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <style>
        table.datagrid td {
            padding: 1px 5px; white-space:nowrap;
        }
        table.datagrid th {
            padding: 1px 5px; white-space:nowrap;
        }

        input[type="checkbox" i] {
            margin: 1px;
        }
    </style>
    <table>
        <tr>
            <td valign="top">
                <asp:LinkButton ID="btnToggle" runat="server" />
                <table class="datagrid">
                    <tr>
                        <th>Source</th>
                        <td><asp:DropDownList id="ddSource" runat="server" AutoPostBack="true" Font-Bold="true" DataTextField="NameAndSuffix" DataValueField="InstanceId" /></td>
                        <td><asp:TextBox id="txtSource" runat="server" Enabled="false" Font-Size="Smaller" Width="300"  /><asp:Button ID="btnSaveSrc" runat="server" Text="Save" Font-Size="Smaller" /> </td>
                    </tr>
                    <tr>
                        <th>Target</th>
                        <td><asp:DropDownList id="ddTarget" runat="server" AutoPostBack="true" DataTextField="NameAndSuffix" DataValueField="InstanceId"  /></td>
                        <td><asp:TextBox id="txtTarget" runat="server" Enabled="false" Font-Size="Smaller" Width="300"  /><asp:Button ID="btnSaveTar" runat="server" Text="Save" Font-Size="Smaller" /></td>
                    </tr>
                </table>

                <table>
                    <tr>
                        <td ><asp:DropDownList id="ddTable" runat="server" AutoPostBack="true" /></td>
                <asp:PlaceHolder ID="plh" runat="server">
                            <td>
                                <asp:RadioButtonList ID="rblFullScan" runat="server" AutoPostBack="true" Font-Bold="true" RepeatDirection="Horizontal">
                                    <asp:ListItem>PKs Only</asp:ListItem>
                                    <asp:ListItem>All Data</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td valign="top" style="padding-left:10px">
                                <asp:Button ID="btnFixIds"  runat="server" Text="Fix Ids (PKs Only)" /> 
                                <asp:Button ID="btnFixData" runat="server" Text="Fix Data (Full Scan)" />
                            </td>
                </asp:PlaceHolder>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <asp:Table ID="tblCounts" runat="server" CssClass="datagrid" />
    <asp:PlaceHolder ID="plhSingle" runat="server">
    

        <div id="pnlSourceOnly" runat="server" style="max-width:1600px; max-height:200px; overflow:auto; border:1px solid #ddd; margin-top:40px; padding-bottom:-20px; background:#dde">
  
                <div id="divSource" runat="server" style="text-align:center; margin-top:10px">
                    <b>Source-Only:</b> <asp:Label ID="lblSourceOnly" runat="server" />
                </div>
                <asp:Table ID="tblSourceOnly" runat="server" CssClass="datagrid" />            
        </div>

        <div id="pnlTargetOnly" runat="server" style="max-width:1600px; max-height:200px; overflow:auto; border:1px solid #ddd; margin-top:40px; background:#edd">
                <div id="divTarget" runat="server" style="text-align:center; margin-top:10px">
                    <b>Target-Only:</b> <asp:Label ID="lblTargetOnly" runat="server" />
                </div>
                <asp:Table ID="tblTargetOnly" runat="server" CssClass="datagrid" />
        </div>
        
        <div id="pnlDifferent" runat="server" style="max-width:1600px; max-height:200px; overflow:auto; border:1px solid #ddd; margin-top:40px; background:#ded">
            <div id="div2" runat="server" style="text-align:center; margin-top:10px">
                <b>Different:</b> <asp:Label ID="lblDifferent" runat="server" />
            </div>
            <asp:Table ID="tblDifferent" runat="server" CssClass="datagrid" />
        </div>

        <div id="pnlBoth" runat="server" style="max-width:1600px; max-height:200px; overflow:auto; border:1px solid #ddd; margin-top:40px; background:#ded">
            <div id="divMatching" runat="server" style="text-align:center; margin-top:10px">
                <b>Matching:</b> <asp:Label ID="lblMatching" runat="server" />
            </div>
            <asp:Table ID="tblMatching" runat="server" CssClass="datagrid" />
        </div>

    </asp:PlaceHolder>
</asp:Content>
