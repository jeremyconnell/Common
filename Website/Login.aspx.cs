using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;
using SchemaMembership;

public partial class Login : CPage
{
    protected override void PageInit()
    {
        UnbindMenu();
        Form.DefaultFocus = txtLogin.Textbox.ClientID;
        AddMenuLeft("Home", "~/");
        AddMenuRight();

        if (User.Identity.IsAuthenticated)
            Response.Redirect(FormsAuthentication.DefaultUrl);
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        ELogin result = CUser.ValidateUser(txtLogin.Text, txtPassword.Text, MembershipPasswordFormat.Encrypted, 10);
        switch (result)
        {
            case ELogin.BadUsername:
            case ELogin.BadPassword:
                lblError.Text = "Bad Username or Password";
                break;

            case ELogin.Disabled:
                lblError.Text = "Username has been Disabled!";
                break;

            case ELogin.LockedOut:
                lblError.Text = "Username has been locked out due to too many failed logins";
                break;

            case ELogin.Success:
                FormsAuthentication.RedirectFromLoginPage(txtLogin.Text, true);
                break;
        }
    }
}
