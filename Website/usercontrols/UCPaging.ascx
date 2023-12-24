<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCPaging.ascx.vb" Inherits="usercontrols_UCPaging" %>

<asp:panel ID="divPagination" class="pagination" runat="server">
  <asp:Literal ID="litSummary" runat="server">Showing <strong>{0} - {1}</strong> of <strong>{2}</strong> results</asp:Literal>
  <div class="page_buttons">
    <asp:HyperLink id="lnkPrev" runat="server" CssClass="nav">&laquo; Prev</asp:HyperLink>
    
    <asp:Label ID="lbl" runat="server" />
    
    <asp:HyperLink id="lnkNext" runat="server" CssClass="nav">Next &raquo;</asp:HyperLink>
      &nbsp;&nbsp; <span style="padding-bottom: 5px">Page Size:</span>
      <asp:DropDownList ID="dd" runat="server" Style="font-size: smaller" onchange="document.location=this.value">
          <asp:ListItem> 5</asp:ListItem>
          <asp:ListItem>10</asp:ListItem>
          <asp:ListItem>20</asp:ListItem>
          <asp:ListItem>50</asp:ListItem>
          <asp:ListItem>100</asp:ListItem>
          <asp:ListItem>200</asp:ListItem>
          <asp:ListItem>500</asp:ListItem>
      </asp:DropDownList>
  </div>

</asp:panel>
