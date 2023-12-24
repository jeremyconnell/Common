<%@ Page Language="VB" AutoEventWireup="false" CodeFile="iframe.aspx.vb" Inherits="pages_sql_iframe"
 %>
<html>
<head id=h runat=server></head>
<body>
    <form style="margin-top:-10px">
        <asp:DataGrid ID=dg runat=server CssClass=datagrid ItemStyle-Wrap=false>
            <HeaderStyle CssClass=heading />
            <AlternatingItemStyle CssClass=alt_row />
            <Columns>
                <asp:TemplateColumn HeaderText="#">
                    <ItemTemplate>
                        <b><%# target.Parent.Parent.Parent.Controls.Count-1 %>.</b>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>

        <asp:PlaceHolder ID="plh" runat="server" />
    </form>
</body>
</html>