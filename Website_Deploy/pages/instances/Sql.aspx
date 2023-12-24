<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sql.aspx.cs" Inherits="pages_clients_Sql" 
    MasterPageFile="~/masterpages/deploy.master"
    Title="SQL"
    ValidateRequest="False"
%>

<asp:Content ID="a" ContentPlaceHolderID="above" runat="server" >
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" />

    <span style="font-weight:bold; font-size:smaller">Deploy:</span> 
    <asp:DropDownList ID="ddInstance" runat="server" DataTextField="IdAndName" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddInstance_SelectedIndexChanged" /><br />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    

    <uc:FormBegin ID="fb" runat="server" />
        <tr>
            <td>&nbsp;</td>
            <td colspan="2">
		        <div style="font-size:smaller; margin-bottom:5px;">
		            Tables:
		            <asp:DropDownList ID=ddTables runat=server AutoPostBack="true" OnSelectedIndexChanged="ddTables_SelectedIndexChanged" />
		            Views:
		            <asp:DropDownList ID=ddViews runat=server AutoPostBack="true" OnSelectedIndexChanged="ddViews_SelectedIndexChanged" />
		            Procs:
		            <asp:DropDownList ID=ddProcs runat=server AutoPostBack="true" OnSelectedIndexChanged="ddProcs_SelectedIndexChanged"  />
		        </div>
            </td>
        </tr>

        <uc:Textarea ID="txtSql" runat="server"  Label="SQL" Height="300" Width="900"  />
        <uc:Dropdown ID="ddHistory" runat="server" Width="900" DataTextField="SqlText" DataValueField="SqlId" AutoPostBack="true" Font-Size="Smaller" OnSelectedIndexChanged="ddHistory_SelectedIndexChanged" />
        <uc:Checkbox ID="chkAll" runat="server" Label="All" Text="Execute for ALL Deployments" />

        <div style="margin-top:5px">
        </div>

        <uc:FormButtonsBegin ID="fbb0" runat="server" />
            <asp:button id="btnSelect"  runat="server" Text="Select" OnClick="btnSelect_Click" />
            <asp:button id="btnUpdate"  runat="server" Text="Update" OnClick="btnUpdate_Click" />
        <uc:FormButtonsEnd   ID="fbe0" runat="server" />
    <uc:FormEnd ID="FormEnd" runat="server" />
</asp:Content>

<asp:Content ID="b" runat="server" ContentPlaceHolderID="below">


    <div id="fs2" runat="server" visible="false" style="width:100%; height:500px; min-width:1500px">
        <b>Results</b>
		<iframe id="iframe" name="iframe" runat=server width="100%" height="100%"/>
    </div>
</asp:Content>