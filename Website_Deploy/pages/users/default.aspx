<%@ Page Language="vb" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="pages_Users_default" 
  MasterPageFile="~/masterpages/default.master"
  Title="Users"
%>

<%@ Register src="usercontrols/UCUsers.ascx" tagname="UCUsers" tagprefix="uc" %>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <!-- Filters etc go here -->
    <table class=filters>
        <tr>
            <td>
                <asp:TextBox ID=txtSearch runat=server />
            </td>
            <td>
                <asp:Button ID=btnSearch runat=server Text="Search" />
            </td>
        </tr>
    </table>
    
    <uc:UCUsers ID="ctrlUsers" runat="server" />        
</asp:Content>
