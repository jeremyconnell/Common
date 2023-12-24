<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UCContainerBegin.ascx.vb" Inherits="usercontrols_extensions_common_UCContainerBegin" %>
<%@ Register src="css_panel/UCContainerBegin.ascx"  tagname="UCContainerBegin" tagprefix="uc1" %>
<%@ Register src="table_rows/UCContainerBegin.ascx" tagname="UCContainerBegin" tagprefix="uc2" %>
<%@ Register src="table_cols/UCContainerBegin.ascx" tagname="UCContainerBegin" tagprefix="uc3" %>
<uc1:UCContainerBegin ID="ctrlCss" runat="server"  Visible="false"/><uc2:UCContainerBegin ID="ctrlRows" runat="server" Visible="false" /><uc3:UCContainerBegin ID="ctrlCols" runat="server" />