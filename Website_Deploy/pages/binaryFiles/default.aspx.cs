using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using System.IO;
using System.IO.Compression;

public partial class pages_BinaryFiles_default : CPageDeploy
{ 
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    public int AppId { get { return null != Ver ? Ver.VersionAppId : CWeb.RequestInt("appId"); } }
    public int VerId { get { return CWeb.RequestInt("verId"); } }
    public bool IsSchema { get { return CWeb.RequestBool("isSchema"); } }
    #endregion
    

    #region Data
    public CApp App { get { return CApp.Cache.GetById(AppId); } }
    public CVersion Ver { get { return CVersion.Cache.GetById(VerId); } }
    public CBinaryFileList BinaryFiles
    {
        get
        {
            return AllFiles(IsSchema, txtSearch.Text);
        }
    }
    public CVersionFileList VersionFiles
    {
        get
        {
            return CVersionFile.Cache.GetByVersionId(VerId).Search(Search, IsSchema);
        }
    }

	public CBinaryFileList AllFiles(bool? isSchema, string search = null)
    {
        if (null != Ver)
            return Ver.BinaryFiles.Search(search, isSchema);

        if (null != App)
            return App.BinaryFiles.Search(search, isSchema);

        return CBinaryFile.Cache.Search(search, isSchema);
    }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
		if (null == App)
			Response.Redirect(CSitemap.BinaryFiles(0, CVersion.Cache[0].VersionId), true);

        //Default to Main Ver
        if (VerId == 0)
            if (App.AppMainVersionId > 0)
                Response.Redirect(CSitemap.BinaryFiles(AppId, App.AppMainVersionId));
            else
                Response.Redirect(CSitemap.BinaryFiles(AppId));

        //Populate Dropdowns
        ddApp.DataSource = CApp.Cache;
        ddApp.DataBind();
        CDropdown.BlankItem(ddApp, "-- Any Apps --");
        CDropdown.SetValue(ddApp, AppId);

        ddVer.DataSource = App.Versions;
        ddVer.DataBind();
        if (App.AppMainVersionId > 0)
            foreach (ListItem i in ddVer.Items)
                if (i.Value == App.AppMainVersionId.ToString())
                    i.Text += " *Main";
        CDropdown.BlankItem(ddVer, "-- All Versions --");
        CDropdown.SetValue(ddVer, VerId);

        rbl.Items[0].Text = CUtilities.NameAndCount("Files", AllFiles(false));
        rbl.Items[1].Text = CUtilities.NameAndCount("Schema", AllFiles(true));
        CDropdown.SetValue(rbl, IsSchema ? 1 : 0);


        //Search state (from querystring)
        txtSearch.Text = this.Search;

		//Display Results
		if (VerId > 0)
		{
			ctrlBinaryFiles.Display(this.VersionFiles);

			var filtered = this.Ver.Filtered;
			if (filtered.Count > 0)
			{
				plhNormallyFiltered.Visible = true;
				ctrlFiltered.Display(filtered);
				lnkOneTime.Text = this.Ver.VersionExcludedFiles;
				lnkOneTime.NavigateUrl = CSitemap.VersionEdit(VerId);
			}
		}
		else
			ctrlBinaryFiles.Display(this.BinaryFiles);

        //Client-side
        this.Form.DefaultFocus  = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);


		UnbindSideMenu();
        if (VerId > 0)
        {
            MenuVersionFiles(AppId, VerId);
            lblTotal.Text = CUtilities.FileSize(Ver.BinaryFiles.TotalBytes);
        }
        else
        {
            MenuAppFiles(AppId);
            lblTotal.Text = App.TotalBytes_;
        }


		if (AppId > 0)
        {
			this.Title = "Files for " + App.AppName;
            if (VerId > 0)
                this.Title += " (" + this.Ver.VersionName + ")";
            else
                this.Title += " (all versions)";

			if (VerId > 0)
				AddLinkSide("Download as zip", btnDownload_Click);
		}
	}
    #endregion 

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {   
        Response.Redirect(CSitemap.BinaryFiles(txtSearch.Text, CDropdown.GetInt(ddApp), CDropdown.GetInt(ddVer), rbl.SelectedIndex > 0));
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        if (null == VersionFiles || VersionFiles.Count == 0)
            return;

        var v = this.Ver;

        Response.ContentType = "application/binary";
        Response.AddHeader("content-disposition", "attachment;filename=" + v.AppNameAndVersion + ".zip");
        using (var ms = new MemoryStream())
        {
            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create))
            {
                var dirs = new List<string>();
                foreach (var i in VersionFiles)
                {
                    var entry = zip.CreateEntry(i.VFPath);
                    using (var es = entry.Open())
                    {
                        using (var esw = new StreamWriter(es))
                        {
                            esw.Write(i.BinaryFile.GetFile());
                            esw.Close();
                        }
                    }
                }
                zip.Dispose();
            }

            Response.BinaryWrite(ms.ToArray());
        }

        Response.End();
    }
    #endregion

    #region Event Handlers - Usercontrols
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.BinaryFiles(txtSearch.Text, AppId, VerId, IsSchema, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
	#endregion

	protected void btnDeleteUnused_Click(object sender, EventArgs e)
	{
		var empty = CBinary.MD5_(new byte[] { });

		var todo = new CBinaryFileList();
		foreach (var i in App.BinaryFiles)
			if (0 == i.VersionFiles.Count && !i.IsSchema && DateTime.Now.Subtract(i.Created).TotalDays > App.AppKeepOldFilesForDays)
				todo.Add(i);
			else if (i.MD5 == empty)
				todo.Add(i);

		CBinaryFile.Cache = null;
		CVersionFile.Cache = null;
		foreach (var i in todo)
			i.Delete();
		Response.Redirect(Request.RawUrl);
	}
}
