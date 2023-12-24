using Microsoft.VisualBasic;
using System.Web.UI;

public abstract class CPage : Page
{

    protected void Page_Init(object sender, System.EventArgs e)
    {
        this.Layout = CConfig.Layout();
        this.PageInit();
    }

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (Page.IsPostBack)
        {
            return;
        }

        this.PageLoad();
    }

    protected void Page_PreRender(object sender, System.EventArgs e)
    {
        this.PagePreRender();
    }

    // Called Automatically
    protected virtual void PageInit()
    {
    }

    protected virtual void PageLoad()
    {
    }

    protected virtual void PagePreRender()
    {
    }

    // Name standardisation only
    protected virtual void PageSave()
    {
    }

    // Casting
    public new CMasterPage Master
    {
        get
        {
            return ((CMasterPage)(base.Master));
        }
    }

    protected string PageHeading
    {
        get
        {
            return Master.PageHeading;
        }
        set
        {
            Master.PageHeading = value;
        }
    }

    protected string MenuTitle
    {
        get
        {
            return Master.MenuTitle;
        }
        set
        {
            Master.MenuTitle = value;
        }
    }

    protected string MenuDataSourceId
    {
        get
        {
            return Master.MenuDataSourceId;
        }
        set
        {
            Master.MenuDataSourceId = value;
        }
    }

    protected string MenuSideDataSourceId
    {
        get
        {
            return Master.MenuSideDataSourceID;
        }
        set
        {
            Master.MenuSideDataSourceID = value;
        }
    }

    protected string MenuSelected
    {
        get
        {
            return Master.MenuSelected;
        }
        set
        {
            Master.MenuSelected = value;
        }
    }

    public void UnbindMenu()
    {
        Master.UnBindMenu();
    }

    public void UnbindSideMenu()
    {
        Master.UnbindSideMenu();
    }

    public void AddButton(string url, string toolTip)
    {
        Master.AddButton(url, toolTip);
    }

    public void AddMenuLeft(string name, string url, bool selected, string tooltip)
    {
        Master.AddMenuLeft(name, url, selected, tooltip);
    }

    public void AddMenuRight(string name, string url, bool selected, string tooltip)
    {
        Master.AddMenuRight(name, url, selected, tooltip);
    }

    public void AddMenuSide(string name, string url, bool selected, string tooltip)
    {
        Master.AddMenuSide(name, url, selected, tooltip);
    }

    public void AddLinkSide(string name, string url, bool selected, string tooltip)
    {
        Master.AddLinkSide(name, url, selected, tooltip);
    }

    // Overloads (shorter)
    public void AddMenuLeft()
    {
        this.AddMenuLeft(Page.Title);
    }

    public void AddMenuLeft(string name)
    {
        this.AddMenuLeft(name, Request.RawUrl, true, name);
    }

    public void AddMenuLeft(string name, string url)
    {
        this.AddMenuLeft(name, url, false);
    }

    public void AddMenuLeft(string name, string url, bool selected)
    {
        this.AddMenuLeft(name, url, selected, name);
    }

    public void AddMenuRight()
    {
        this.AddMenuRight(Page.Title);
    }

    public void AddMenuRight(string name)
    {
        this.AddMenuRight(name, Request.RawUrl, true, name);
    }

    public void AddMenuRight(string name, string url)
    {
        this.AddMenuRight(name, url, false);
    }

    public void AddMenuRight(string name, string url, bool selected)
    {
        this.AddMenuRight(name, url, selected, name);
    }

    public void AddMenuSide()
    {
        this.AddMenuSide(Page.Title);
    }

    public void AddMenuSide(string name)
    {
        this.AddMenuSide(name, Request.RawUrl, true, name);
    }

    public void AddMenuSide(string name, string url)
    {
        this.AddMenuSide(name, url, false);
    }

    public void AddMenuSide(string name, string url, bool selected)
    {
        this.AddMenuSide(name, url, selected, name);
    }

    public void AddLinkSide(string name, string url)
    {
        this.AddLinkSide(name, url, string.Empty);
    }

    public void AddLinkSide(string name, string url, string tooltip)
    {
        this.AddLinkSide(name, url, false, tooltip);
    }

    public void AddLinkSide(string name, string url, bool selected)
    {
        this.AddLinkSide(name, url, selected, string.Empty);
    }

    public string PageTitle
    {
        get
        {
            return base.Title;
        }
        set
        {
            base.Title = value;
            Master.PageHeading = value;
        }
    }

    // Page - Editable/Disabled/Locked
    public virtual EControlMode Mode
    {
        set
        {
            this.SetControlMode(this, value);
        }
    }

    public ELayout Layout
    {
        set
        {
            this.SetLayout(this, value);
        }
    }

    public void SetControlMode(Control parent, EControlMode value)
    {
        CCustomControl.SetControlMode(this, value);
    }

    public void SetLayout(Control parent, ELayout value)
    {
        CCustomControlContainer.SetLayout(this, value);
    }
}