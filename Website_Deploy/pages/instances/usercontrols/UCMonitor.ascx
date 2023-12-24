<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCMonitor.ascx.cs" Inherits="pages_instances_usercontrols_UCMonitor" %>
<script runat="server">

    protected void btnRefresh_Click(object sender, EventArgs e)
    {

    }
</script>


<asp:RadioButtonList ID="rbl" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbl_SelectedIndexChanged">
    <asp:ListItem Selected="True">Summary</asp:ListItem>
    <asp:ListItem>Log-file Viewer</asp:ListItem>
    <asp:ListItem>App-Set/Con.Str.</asp:ListItem>
</asp:RadioButtonList>

<asp:PlaceHolder ID="plhSummary" runat="server">
    <uc:FormBegin ID="fb" runat="server" />
        <uc:FormLabel ID="lblMonitoring" runat="server" Text="Remote Monitoring" />
        <uc:Textbox ID="txtInstance" runat="server"  Label="Instance" Mode="Locked" />
        <uc:Textbox ID="txtRemote" runat="server"  Label="Remote" Mode="Locked" />
        <uc:Textbox ID="txtMachine" runat="server"  Label="Machine" Mode="Locked" />
        <uc:Textbox ID="txtHost" runat="server"  Label="Host" Mode="Locked" />
        <uc:Textbox ID="txtVersion" runat="server"  Label="Actual Ver." Mode="Locked" />
        <uc:Textbox ID="txtSchema" runat="server"  Label="Actual Sch." Mode="Locked" />
        <uc:Textbox ID="txtTargetVer" runat="server"  Label="Target Ver." Mode="Locked" />
        <uc:Textbox ID="txtTargetSch" runat="server"  Label="Target Sch." Mode="Locked" />
        <uc:FormButtonsBegin ID="fbb4" runat="server" />
            <div style="text-align:left">
                <asp:button id="btnPushName" runat="server" Text="Push Label" OnClick="btnPushName_Click"  />
                &nbsp;
                <asp:button id="btnPushFiles" runat="server" Text="Push Files"  OnClick="btnPushFiles_Click" />   
                &nbsp;
                <asp:button id="btnTrigger" runat="server" Text="Self-Upgrade" OnClick="btnTrigger_Click"  />         
            </div>
            <div style="text-align:left">
                <asp:button id="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click"/>
                &nbsp;       
            </div>
        <uc:FormButtonsEnd   ID="fbe4" runat="server" />
    <uc:FormEnd ID="f3" runat="server" />

</asp:PlaceHolder>


<asp:PlaceHolder ID="plhViewer" runat="server">
    <table>
        <tr>
          <td valign="top">
                <div style="margin:5px 0px">
                    <asp:DropDownList ID="ddFile" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddFile_SelectedIndexChanged" />
                </div>

                <iframe id="iframe" runat="server" width="1600" height="900" />
            </td>
        </tr>
    </table>




</asp:PlaceHolder>



<asp:PlaceHolder ID="plhAppSettings" runat="server">

    <div style="text-align:left; max-height:200px; max-width:1200px; overflow:auto; margin-top:10px">
        <asp:Table ID="tblConnStr" runat="server" CssClass="datagrid" />
    </div>
    <div style="text-align:left; max-height:600px; max-width:1200px; overflow:auto; margin-top:10px">
        <asp:Table ID="tblConfig" runat="server" CssClass="datagrid" />
    </div>

</asp:PlaceHolder>







        