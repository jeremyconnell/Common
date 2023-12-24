using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SchemaMembership;

public partial class usercontrols_UCMenuSide : System.Web.UI.UserControl
{
    #region Layout
    private bool _horizontal = false;
    public bool Horizontal { get { return _horizontal; } set { _horizontal = value; } }
    #endregion

    #region Manual Interface
    public void Add() { Add(Page.Title); }
    public void Add(string name)
    {
        Add(name, Request.RawUrl, true, Page.Title);
    }
    public void Add(string name, string url)
    {
        Add(name, url, false);
    }
    public void Add(string name, string url, bool selected)
    {
        Add(name, url, selected, name);
    }
    public void Add(string name, string url, bool selected, string tooltip)
    {
        Add(name, url, selected, tooltip, null);
    }
    public void Add(string name, string url, bool selected, string tooltip, IList roles)
    {
        if (!CUser.CanSee(roles)) return;
        tbl.Visible = true;

        TableRow row = new TableRow();
        if (Horizontal && tbl.Rows.Count == 1)
            row = tbl.Rows[0];
        else
            tbl.Rows.Add(row);

        TableCell cell = new TableCell();
        row.Cells.Add(cell);
        if (Horizontal) cell.CssClass = "horiz";

        HyperLink lnk = new HyperLink();
        cell.Controls.Add(lnk);

        lnk.NavigateUrl = url;
        lnk.Text = name;
        lnk.ToolTip = tooltip;
        if (selected)
            lnk.CssClass = "selected";
    }
    #endregion


    #region "Binding"
    private string m_DataSourceID;
    public string DataSourceID
    {
        get { return m_DataSourceID; }
        set { m_DataSourceID = value; }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(DataSourceID))
        {
            SiteMapDataSource ds = (SiteMapDataSource)Parent.FindControl(DataSourceID);
            if (null != ds)
                this.Provider = ds.Provider;
        }

        //Auto-hide
        this.Visible = tbl.Rows.Count > 0; ;
    }
    public SiteMapProvider Provider
    {
        set
        {
            SiteMapNode current = value.CurrentNode;
            SiteMapNode root = value.RootNode;

            //Get current (ignoring querystring)
            if ((current == null))
                current = value.FindSiteMapNode(Request.Url.AbsolutePath);

            //Error case - nothing matching
            if ((current == null))
                return;

            //Top-level match
            SiteMapNode parent = current.ParentNode;
            if ((parent.ParentNode == null))
                return;

            List<TableRow> existing = new List<TableRow>(tbl.Rows.Count);
            foreach (TableRow i in tbl.Rows)
                existing.Add(i);
            tbl.Rows.Clear();

            //Show a trail
            string q = Request.Url.Query;
            if (-1 != q.IndexOf("&sortBy="))
                q = q.Substring(0, q.IndexOf("&sortBy="));

            //Highest parent
            if (null != current.ParentNode.ParentNode)
            {
                if (null != current.ParentNode.ParentNode.ParentNode)
                {
                    Add(current.ParentNode.ParentNode.Title, current.ParentNode.ParentNode.Url, false, current.ParentNode.ParentNode.Description, current.ParentNode.ParentNode.Roles);
                    if (null != current.ParentNode.ParentNode.ParentNode.ParentNode)
                        foreach (SiteMapNode i in current.ParentNode.ParentNode.ParentNode.ChildNodes)
                            Add(i.Title, i.Url + q, false, i.Description, i.Roles);

                    foreach (SiteMapNode i in current.ParentNode.ParentNode.ChildNodes)
                        Add(i.Title, i.Url + q, false, i.Description, i.Roles);
                }
                else
                    Add(current.ParentNode.Title + "...", current.ParentNode.Url, false, current.ParentNode.Description, current.ParentNode.Roles);
            }


            //Show siblings
            foreach (SiteMapNode i in current.ParentNode.ChildNodes)
            {
                bool match = i.Url.ToLower() == Request.Url.AbsolutePath.ToLower();
                Add(i.Title, i.Url + q, match, i.Description, i.Roles);
            }

            tbl.Rows.AddRange(existing.ToArray());
        }
    }
    #endregion
}
