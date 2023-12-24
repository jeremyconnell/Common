<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default"
MasterPageFile="~/masterpages/deploy.master"
Title="Home"
    EnableViewState="false"
%>

<asp:Content ID="c" runat="server" ContentPlaceHolderID="body">
    

    
    <asp:Panel ID="pnlDatabases" runat="server" Visible="false">


    </asp:Panel>
    
    <asp:Panel ID="pnlWebApps" runat="server" Visible="false">
    </asp:Panel>




    <asp:Panel ID="pnlData" runat="server" Visible="false">


    </asp:Panel>


</asp:Content>



<asp:Content ID="b" runat="server" ContentPlaceHolderID="below">
    <div id="fs2" runat="server" visible="false" style="width:100%; height:500px; min-width:1500px">
        <b>Results</b>
		<iframe id="iframe" name="iframe" runat=server width="100%" height="100%"/>
    </div>
</asp:Content>