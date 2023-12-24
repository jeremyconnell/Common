<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Sql.aspx.vb" Inherits="pages_self_Sql"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Saved Queries"
    ValidateRequest="False"
%>
<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
	<table width="100%" height="100%">
	    <tr>
	        <td nowrap>
			    <asp:RadioButtonList ID=rbl runat=server RepeatDirection=Horizontal RepeatLayout=Flow>
			        <asp:ListItem Selected=true>Select</asp:ListItem>
			        <asp:ListItem>Update</asp:ListItem>
			    </asp:RadioButtonList>
				<asp:Button ID="btnExecute" Runat="server" Text="Execute"  style="margin-left:10px" Width=160 />
			</td>
		</tr>
	</table>
</asp:Content>

<asp:Content ID=b runat=server ContentPlaceHolderID=below>
        <fieldset>
            <legend>Raw Sql</legend>
            <div>
                <asp:DropDownList ID="ddHistory" runat="server" Width="900" DataTextField="SqlTrunc200" DataValueField="SqlId" AutoPostBack="true" Font-Size="Smaller" />
            </div>
		    <asp:TextBox ID="txtSql" Runat="server" Width="900" TextMode="MultiLine" Rows="4" Height="300px" Font-Size=12px />
		    <div style="font-size:smaller; margin-top:5px;">
		        Tables:
		        <asp:DropDownList ID=ddTables runat=server />
		        Views:
		        <asp:DropDownList ID=ddViews runat=server />
		        Functions:
		        <asp:DropDownList ID=ddFunctions runat=server />
		        Procs:
		        <asp:DropDownList ID=ddProcs runat=server />   
		    </div>
        </fieldset>
    <asp:Panel ID=pnlResults runat=server Visible=false>
        <fieldset style="width:100%; height:400px">
            <legend>Results</legend>
		    <iframe id="iframe" name="iframe" runat=server width="100%" height="100%"/>
        </fieldset>
    </asp:Panel>
</asp:Content>