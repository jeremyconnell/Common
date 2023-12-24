using Microsoft.VisualBasic;
using System.Web;
// Usage:
// 1. Implement the required logic in the Rewrite(app) method e.g. app.Context.RewritePath
// 2. Add a line into the web.config like so (may need to upgrade to 3.5):
//         <httpModules>
//             <add type="CUrlRewrite" name="CUrlRewrite" />
public class CUrlRewrite: Framework.CUrlRewrite
{

    // Constructor
    public CUrlRewrite() : base(false)
    {
    }

    // Logic
    protected override void Rewrite(HttpApplication app)
    {
        // app.Context.RewritePath()
    }
}