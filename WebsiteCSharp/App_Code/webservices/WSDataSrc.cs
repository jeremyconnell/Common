using Framework;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
// Provider for the remote driver CWebSrcSoap
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class WSDataSrc : CWSDataSrc
{

    public override void ClearCache(string tableName)
    {
        this.ClearCache();
        // Can be more specific when schema project is in scope
    }
}