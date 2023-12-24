<%@ Page Language="vb" AutoEventWireup="false" CodeFile="user.aspx.vb" Inherits="pages_Users_User"
    MasterPageFile="~/pages/default.master"
    Title="Add or Change a User."
    ValidateRequest="False"
%>
<asp:Content ID="c" ContentPlaceHolderID="body" runat="server">
    <asp:ValidationSummary ID="vs" runat="server" ShowSummary="true" EnableClientScript="true" DisplayMode="BulletList" />
    <uc:FormBegin ID="fb" runat="server" />
        <uc:FormLabel ID="lblSecurity" runat="server" Text="Login" />
        <uc:Textbox  ID="txtUserLoginName"                  runat="server" Required="true"  Label="Login Name" />
        <uc:Textbox  ID="txtUserPasswordPlainText"          runat="server" Required="false" Label="Password" />
        <uc:Textbox  ID="txtUserPasswordQuestion"           runat="server" Required="false"  Label="PasswordQuestion" Visible=false />
        <uc:Textbox  ID="txtUserPasswordAnswer"             runat="server" Required="false"  Label="PasswordAnswer"   Visible=false  />
        <uc:CheckboxList ID="cblRoles"                      runat="server" Label="Roles" DataTextField="RoleName" DataValueField="RoleName" />
        <uc:FormLabel ID="lblIdentity" runat="server" Text="Identity" />
        <uc:Textbox  ID="txtUserFirstName"                  runat="server" Required="true"  Label="First Name" />
        <uc:Textbox  ID="txtUserLastName"                   runat="server" Required="true"  Label="Last Name" />
        <uc:Textbox  ID="txtUserEmail"                      runat="server" Required="true" Label="Email" /> <!-- Delete IX_UserEmailUnique if not required -->
        <uc:Textarea ID="txtUserComments"                   runat="server" Required="false" Label="Comments" />
        <uc:FormLabel ID="lblStatus" runat="server" Text="Status" />
        <uc:Checkbox ID="chkUserIsDisabled"                 runat="server" Required="false" Label="Disabled" />
        <uc:Checkbox ID="chkUserIsLockedOut"                runat="server" Required="false" Label="Locked-Out" Mode=Locked />
        <asp:PlaceHolder ID=plhActivity runat=server>
            <uc:FormLabel ID="lblActivity" runat="server" Text="Activity" />
            <uc:Textbox  ID="txtUserCreatedDate"                Runat="server" Required="false"  Label="Created"                   TextMode="Date"     Mode="Locked" />
            <uc:Textbox  ID="txtUserLastLoginDate"              Runat="server" Required="false"  Label="Last Login"                TextMode="Date"     Mode="Locked" />
            <uc:Textbox  ID="txtUserLastPasswordChangedDate"    Runat="server" Required="false"  Label="Password Changed"          TextMode="Date"     Mode="Locked" />
            <uc:Textbox  ID="txtUserFailedPasswordAttemptCount" runat="server" Required="false"  Label="Failed Attempts"           TextMode="Integer"  Mode="Locked" />
            <uc:Textbox  ID="txtUserFailedPasswordAttemptStartDate" Runat="server" Required="false" Label="First Failed Attempt"   TextMode="Date"     Mode="Locked" />
            <uc:Textbox  ID="txtUserLastLockoutDate"            Runat="server" Required="false"  Label="Lockout Date"              TextMode="Date"     Mode="Locked" />
        </asp:PlaceHolder>
        <uc:FormButtonsBegin ID="fbb" runat="server" />
            <asp:button id="btnSave"    runat="server" text="Update" causesvalidation="True"  />
            <asp:button id="btnDelete"  runat="server" text="Delete" causesvalidation="False" OnClientClick="return confirm('Delete this User?')" />
            <asp:button id="btnCancel"  runat="server" text="Cancel" causesvalidation="False" />
        <uc:FormButtonsEnd   ID="fbe" runat="server" />
    <uc:FormEnd ID="fe" runat="server" />
</asp:Content>
