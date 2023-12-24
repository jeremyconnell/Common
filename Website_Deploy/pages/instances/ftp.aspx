<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ftp.aspx.cs" Inherits="pages_instances_ftp"
    MasterPageFile="~/masterpages/deploy.master"
    Title="FTP Tool (initial deploy)"
    ValidateRequest="False"
%>

<%@ Register src="usercontrols/UCPublishProfile.ascx" tagname="UCPublishProfile" tagprefix="uc" %>
<%@ Register src="~/pages/binaryFiles/usercontrols/UCBinaryFiles.ascx" tagname="BinaryFiles" tagprefix="uc" %>

<asp:Content ID="t" runat="server" ContentPlaceHolderID="above">
    <span style="font-weight:bold; font-size:smaller">App:</span> 
    <asp:DropDownList ID="ddApp" Runat="Server" DataTextField="NameAndInstanceCount" DataValueField="AppId" AutoPostBack="true" OnSelectedIndexChanged="ddApp_SelectedIndexChanged" />

    <span style="font-weight:bold; font-size:smaller">Deploy:</span> 
    <asp:DropDownList ID="ddIns" Runat="Server" DataTextField="NameAndSuffix" DataValueField="InstanceId" AutoPostBack="true" OnSelectedIndexChanged="ddIns_SelectedIndexChanged" />
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">

    <asp:DropDownList ID="ddVersion" runat="server" Label="Initial Version" DataTextField="VersionName" DataValueField="VersionId" OnSelectedIndexChanged="ddVersion_SelectedIndexChanged" AutoPostback="true" />
    <asp:Button ID="btnPush" runat="server" Text="Push Files via FTP (initial deploy)" OnClick="btnPush_Click" Visible="false" style="margin-top:10px; margin-bottom:10px" />
    <asp:Panel ID="divFiles" runat="server" Visible="false" style="max-height:400px; width:675px; overflow:auto; margin-bottom:20px">
        <uc:BinaryFiles ID="ctrl" runat="server" PageSize="500" Visible="false" />
    </asp:Panel>


    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
    

    <hr style="margin-top:25px; margin-bottom:25px" />
    
    <b>FTP Explorer</b> &nbsp; 
    <br />
    <table cellpadding="2" cellspacing="0" border="1" style="border-collapse:collapse; border-color:#ddd; background-color:#eee">
        <tr>
            <td valign="top">
                <div style="max-height:400px; overflow:auto; padding:0px 10px"">
                    <asp:Table ID="tblDir" runat="server" CssClass="datagrid" style="min-width:120px; margin-bottom:5px"  />

                    <asp:TextBox ID="txtNewDir" runat="server" Width="80" Font-Size="10px" />
                    <asp:Button ID="btnMakeDir" runat="server" Text="Dir" OnClientClick="return confirm('Make a new directory?')" OnClick="btnMakeDir_Click" Font-Size="10px" />
                </div>

            </td>
            <td valign="top" id="colFiles" runat="server" style="padding:0px 10px">
                <div style="max-height:400px; overflow:auto;">
                    <asp:Table ID="tblFile" runat="server" CssClass="datagrid" />
                </div>
            </td>
            <td id="colViewEdit" runat="server">
                <div>
                    <asp:Button id="btnEdit" runat="server" Text="Edit&raquo;" OnClick="btnEdit_Click" style="margin-top:-30px; float:right" /> 
                    <asp:TextBox ID="txtFileName" runat="server" Width="150" Visible="false" />
                    <asp:Button ID="btnSave" runat="server" Text="Save As" Visible="false" OnClick="btnSave_Click" /> 
                    <asp:Button id="btnCancel" runat="server" Text="Cancel"  Visible="false" OnClick="btnCancel_Click" />
                </div>
                <asp:TextBox ID="txtEdit" runat="server" TextMode="MultiLine" Height="400" Width="600" Enabled="false" />
            </td>
        </tr>
    </table>
    
    <hr style="margin-top:25px; margin-bottom:25px" />
    
    <b>Publish Profiles</b>
    <asp:Panel ID="pnlProfiles" runat="server" />
</asp:Content>