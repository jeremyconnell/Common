<%@ Page Language="vb" AutoEventWireup="false" CodeFile="audit_Error.aspx.vb" Inherits="pages_Audit_Errors_Audit_Error" 
  MasterPageFile="~/pages/default.master"
  Title="Error Log"
%>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <uc:FormBegin ID="fb" runat="server" />
        
        <uc:FormLabel ID=lblSource runat=server Text="Source" />
        <uc:Textbox  ID="txtErrorDateCreated" runat="server" Mode="Locked" Label="Created" />
        <uc:Textbox  ID="txtErrorUrl" runat="server" Mode="Locked" Label="Url" Target=_blank />
        <uc:Textbox  ID="txtErrorUserName" runat="server" Mode="Locked" Label="UserName" />
        
        <uc:FormLabel ID=lblDetails runat=server Text="Exception" />
        <uc:Textbox  ID="txtErrorType" runat="server" Mode="Locked" Label="Type" />
        <uc:Textbox  ID="txtErrorMessage" runat="server" Mode="Locked" Label="Message" />
        <tr>
            <td class=label valign=top>Stacktrace</td>
            <td colspan=2>
                <pre style="font-size:10px"><asp:Literal ID=litStacktrace runat=server /></pre>
            </td>
        </tr>

        <uc:FormLabel ID=lblInner runat=server Text="Inner Exception" />
        <uc:Textbox  ID="txtErrorInnerType" runat="server" Mode="Locked" Label="Type" />
        <uc:Textbox  ID="txtErrorInnerMessage" runat="server" Mode="Locked" Label="Message" />
        <tr>
            <td class=label valign=top>Stacktrace</td>
            <td colspan=2>
                <pre style="font-size:10px"><asp:Literal ID=litInnerStacktrace runat=server /></pre>
            </td>
        </tr>
        
        <uc:FormLabel ID=lblServer runat=server Text="Server" />
        <uc:Textbox  ID="txtErrorMachineName" runat="server" Mode="Locked" Label="Machine" />
        <uc:Textbox  ID="txtErrorApplicationName" runat="server" Mode="Locked" Label="Application" />
        <uc:Textbox  ID="txtErrorApplicationVersion" runat="server" Mode="Locked" Label="Version" />
    <uc:FormEnd ID="fe" runat="server" />

</asp:Content>
