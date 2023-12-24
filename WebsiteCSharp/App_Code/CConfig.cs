
using System;
using System.Web;

public class CConfig : Framework.CConfigBase
{

    public static ELayout Layout()
    {
        string s = Config("Layout", "Horizontal");
        return ((ELayout)(Enum.Parse(typeof(ELayout), s)));
    }

    public static string MenuTitle()
    {
        return Config("MenuTitle", "Picasso");
    }

    public static bool IsDev()
    {
        return HttpContext.Current.Request.Url.Host.ToLower().Contains("localhost");
    }
}