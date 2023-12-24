<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Schema.aspx.cs" Inherits="pages_binaryFiles_Schema"
    MasterPageFile="~/masterpages/deploy.master"
    Title="Schema"
    ValidateRequest="False"
    EnableViewState="false"
%>

<%@ Register src="usercontrols/UCView.ascx" tagname="UCView" tagprefix="uc" %>
<%@ Register src="usercontrols/UCTable.ascx" tagname="UCTable" tagprefix="uc" %>
<%@ Register src="usercontrols/UCStoredProc.ascx" tagname="UCStoredProc" tagprefix="uc" %>
<%@ Register src="usercontrols/UCDefaultValue.ascx" tagname="UCDefaultValue" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <asp:DropDownList ID="ddVersion" Runat="Server" DataTextField="AppNameAndVersion" DataValueField="VersionId" AutoPostBack="true" OnSelectedIndexChanged="ddVersion_SelectedIndexChanged"/>
    <asp:DropDownList ID="ddInstance" Runat="Server" DataTextField="AppAndName" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddInstance_SelectedIndexChanged"/>
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <table class="datagrid">
        <tr>
            <th>Tables</th>
            <td><asp:Label ID="lblTables" runat="server" /></td>
        </tr>
        <tr>
            <th>Views</th>
            <td><asp:Label ID="lblViews" runat="server" /></td>
        </tr>
        <tr>
            <th>Procs</th>
            <td><asp:Label ID="lblProcs" runat="server" /></td>
        </tr>
        <tr>
            <th>Indexes</th>
            <td><asp:Label ID="lblIndexes" runat="server" /></td>
        </tr>
        <tr>
            <th>Primary Keys</th>
            <td><asp:Label ID="lblPks" runat="server" /></td>
        </tr>
        <tr>
            <th>Foreign Keys</th>
            <td><asp:Label ID="lblFks" runat="server" /></td>
        </tr>
        <tr>
            <th>Default Vals</th>
            <td><asp:Label ID="lblDefs" runat="server" /></td>
        </tr>
        <tr id="tdMig" runat="server">
            <th>Migration</th>
            <td><asp:Label ID="lblMigration" runat="server" /></td>
        </tr>
    </table>
    
    <asp:CheckBox ID="chkDetail" runat="server" Text="Show Details" AutoPostBack="true" OnCheckedChanged="chkDetail_CheckedChanged" />

    <div>
        <asp:Table ID="tblProcs" runat="server" Visible="false" CssClass="datagrid">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="18">#</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="300">StoredProcs/Functions</asp:TableHeaderCell>
                <asp:TableHeaderCell><asp:Label ID="lblProcsHash" runat="server" /> </asp:TableHeaderCell>
                <asp:TableHeaderCell>Type</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>

        <div id="plhProcs" runat="server" style="max-height:300px; overflow:auto; width:auto; display:inline-block; margin-bottom:10px">
            <table class="datagrid" style="margin-top:0px">
                <thead>
                    <tr>
                        <th>#</th>
                        <th><asp:Literal ID="litProcs" runat="server" Text="StoredProcs" />, <asp:Literal ID="litFunc" runat="server" Text="Functions" /> </th>
                        <th>Code</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="plhScript" runat="server" />
                </tbody>
            </table>
        </div>
    </div>
    <div>
        <asp:Table ID="tblViews" runat="server" Visible="false" CssClass="datagrid">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="18">#</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="300">Views</asp:TableHeaderCell>
                <asp:TableHeaderCell><asp:Label ID="lblViewsHash" runat="server" /> </asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>

        <div id="divViews" runat="server" style="max-height:700px; overflow:auto; width:auto; display:inline-block; margin-bottom:20px; border-bottom:1px solid #aaa">
            <table class="datagrid" style="margin-top:0px">
                <thead>
                    <tr>
                        <th>#</th>
                        <th> <asp:Literal ID="litViews" runat="server" Text="Views" /> </th>
                        <th>Columns</th>
                        <th>Code</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="plhViews" runat="server" />
                </tbody>
            </table>
        </div>
    </div>
    
    <div>
        <asp:Table ID="tblTables" runat="server" Visible="false" CssClass="datagrid">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="18">#</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="300">Tables</asp:TableHeaderCell>
                <asp:TableHeaderCell><asp:Label ID="lblTablesHash" runat="server" /> </asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
        <div id="divTables" runat="server" style="max-height:1000px; overflow:auto">
            <table class="datagrid" style="margin-top:0px">
                <thead>
                    <tr>
                        <th>#</th>
                        <th> <asp:Literal ID="litTables" runat="server" Text="Tables" /> </th>
                        <th>Columns</th>
                        <th>Primary/Foreign Keys</th>
                        <th>Indexes</th>
                        <th>Script</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="plhTables" runat="server" />
                </tbody>
            </table>
        </div>
    </div>
    
    <div>
        <asp:Table ID="tblFks" runat="server" Visible="false" CssClass="datagrid">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="18">#</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="300">Foreign Keys</asp:TableHeaderCell>
                <asp:TableHeaderCell><asp:Label ID="lblFkHash" runat="server" /> </asp:TableHeaderCell>
                <asp:TableHeaderCell >Cascade</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
        <div id="divFKs" runat="server" style="max-height:1000px; overflow:auto">
            <table class="datagrid" style="margin-top:20px">
                <thead>
                    <tr>
                        <th>#</th>
                        <th> <asp:Literal ID="litFks" runat="server" Text="Foreign Keys" /> </th>
                        <th>Hash</th>
                        <th>Table</th>
                        <th>RefTable</th>
                        <th>Columns</th>
                        <th>Ref-Cols</th>
                        <th>Cascade-U</th>
                        <th>Cascade-D</th>
                        <th>Script</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="plhFks" runat="server" />
                </tbody>
            </table>
        </div>
    </div>
    
    <div>
        <asp:Table ID="tblDefs" runat="server" Visible="false" CssClass="datagrid">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="18">#</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="300">Default Name</asp:TableHeaderCell>
                <asp:TableHeaderCell><asp:Label ID="lblDefHash" runat="server" /> </asp:TableHeaderCell>
                <asp:TableHeaderCell >Table</asp:TableHeaderCell>
                <asp:TableHeaderCell >Column</asp:TableHeaderCell>
                <asp:TableHeaderCell >Definition</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
        <div id="divDefs" runat="server" style="max-height:1000px; overflow:auto">
            <table class="datagrid" style="margin-top:20px">
                <thead>
                    <tr>
                        <th>#</th>
                        <th> <asp:Literal ID="litDefs" runat="server" Text="Foreign Keys" /> </th>
                        <th>Hash</th>
                        <th>Table</th>
                        <th>RefTable</th>
                        <th>Columns</th>
                        <th>Ref-Cols</th>
                        <th>Cascade-U</th>
                        <th>Cascade-D</th>
                        <th>Script</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="plhDefVals" runat="server" />
                </tbody>
            </table>
        </div>
    </div>

</asp:Content>
