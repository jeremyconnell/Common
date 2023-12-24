using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaAudit;


public partial class pages_globalError_ContactAdmin : CPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            AddMenuLeft("Error");

            if (CSession.IsAdmin)
            {
                lnkAdmin.Visible = true;
                lnkAdmin.NavigateUrl = CSitemap.Audit_Errors();
            }

            try
            {
                int errorId = (int)Application["errorId"];
                if (errorId < 0) return;
                CAudit_Error ex = new CAudit_Error(errorId);
                lnk.NavigateUrl += " (" + errorId.ToString() + ")";
                lit.Text = errorId.ToString();

                lnkTryAgain.Visible = true;
                lnkTryAgain.NavigateUrl = ex.ErrorUrl;

                if (CSession.IsAdmin)
                {
                    lnkAdmin.NavigateUrl = CSitemap.Audit_Error(ex.ErrorTypeHash, ex.ErrorMessageHash, ex.ErrorInnerTypeHash, ex.ErrorInnerMessageHash);
                }
                litM1.InnerText = ex.ErrorMessage;
                litS1.InnerText = ex.ErrorStacktrace;
                litM2.InnerText = ex.ErrorInnerMessage;
                litS2.InnerText = ex.ErrorInnerStacktrace;
   
            }
            catch
            {; }
        }
        catch (Exception ex2)
        {
            lnk.NavigateUrl = Request.RawUrl;
            lit.Text = "Website Down Error";

            lnkTryAgain.Visible = false;
            lnkTryAgain.NavigateUrl = "javascript:";

            //if (CSession.IsAdmin)
            //{
            //    lnkAdmin.NavigateUrl = "javascript:";

            //    litM1.InnerText = ex2.Message;
            //    litS1.InnerText = ex2.StackTrace;
            //    if (null != ex2.InnerException)
            //    {
            //        litM2.InnerText = ex2.InnerException.Message;
            //        litS2.InnerText = ex2.InnerException.StackTrace;
            //    }
            //}
        }
    }
}