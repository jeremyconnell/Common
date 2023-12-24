using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Framework;

public partial class usercontrols_UCTimepicker : System.Web.UI.UserControl
{
    #region EventHandlers - Page
    protected void Page_Init(object sender, EventArgs e)
    {
        for (int i = 0; i < 24; i++)
        {
            string hh = new DateTime(2000, 1, 1, i, 0, 0).ToString("h tt");
            if (i == 0)
                hh = "midnight";
            if (i == 12)
                hh = "midday";
            CDropdown.Add(ddHours, hh, i);
        }

        for (int i=0; i<60; i+=5)
        {
            string mm = new DateTime(2000, 1, 1, 0, i,  0).ToString("mm");
            CDropdown.Add(ddMins, mm, i);
            CDropdown.Add(ddSecs, mm, i); 
        }
    }
    #endregion

    #region Interface
    public DateTime Value { get { return Time; } set { Time = value; } }
    public DateTime Time 
    { 
        get 
        { 
            return new DateTime(2000, 1, 1).AddHours(Hours).AddMinutes(Mins).AddSeconds(Secs); 
        }
        set
        {
            Hours = value.Hour;
            Mins = value.Minute;
            Secs = value.Second;
        }
    }

    public int Hours { get { return CDropdown.GetInt(ddHours); } set { CDropdown.SetValue(ddHours, value);  } }
    public int Mins { get { return CDropdown.GetInt(ddMins); } set { CDropdown.SetValue(ddMins, value); if (Mins != value) { CDropdown.Add(ddMins, new DateTime(2000, 1, 1, 0, value, 0).ToString("mm"), value); Mins = value; } } }
    public int Secs { get { return CDropdown.GetInt(ddSecs); } set { CDropdown.SetValue(ddSecs, value);     } }

    public bool Enabled 
    {
        get { return ddHours.Enabled; }
        set
        {
            ddHours.Enabled = value;
            ddMins.Enabled  = value;
            ddSecs.Enabled  = value;
        }
    }
    public string ToolTip 
    {
        get { return ddHours.ToolTip; }
        set
        {
            ddHours.ToolTip = value;
            ddMins.ToolTip  = value;
            ddSecs.ToolTip  = value;
        }
    }
    public bool ShowSeconds { get { return cellSecs.Visible; } set { cellSecs.Visible = value; } }
    public bool ShowMinutes { get { return cellMins.Visible; } set { cellMins.Visible = value; } }
    #endregion
}
