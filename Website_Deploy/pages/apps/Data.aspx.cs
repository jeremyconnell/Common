using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using System.Threading.Tasks;

public partial class pages_apps_Data :  CPageDeploy
{
    //Querystring
    public int AppId { get { return CWeb.RequestInt("appId", (int)EApp.ControlTrack); } }

    //Data
    public CApp App { get { return CApp.Cache.GetById(AppId); } }


    protected override void PageInit()
    {
        if (null == App)
            Response.Redirect(CSitemap.AppData());
        MenuAppData(AppId);
    }

    protected override void PagePreRender()
    {
        var refIns = App.Instances[0];
        var refSch = refIns.DatabaseDirectOrWeb.SchemaInfo();
        var names = refSch.Tables.Names;

        var dict = new Dictionary<CInstance, Dictionary<string, int>>();
        foreach (var ins in App.Instances)
            dict.Add(ins, new Dictionary<string, int>());

        Parallel.ForEach(App.Instances, ins =>
        {
            var d = dict[ins];

            Parallel.ForEach(names, tbl =>
            {
                try
                {
                    var tbl_ = String.Concat("[", tbl.Replace(".", "].["), "]");
                    int count = ins.DatabaseDirectOrWeb.SelectCount(tbl_, null);

                    lock (d)
                    { d.Add(tbl, count); }
                }
                catch
                {
                    lock (d)
                    { d.Add(tbl, -1); }
                }
            });
        });

        //Header row
        var trh = RowH(tbl);
        CellH(trh, "#");
        foreach (var i in App.Instances)
            CellLinkH(trh, i.InstanceClientCode + i.InstanceSuffix, CSitemap.InstanceData(i.InstanceId), false);

        //Data Rows
        foreach (var i in names)
        {
            var tr = Row(tbl);
            var td = CellH(tr, i);

            var blank = 0;
            foreach (var j in App.Instances)
            {
                var d = dict[j];
                var c = d[i];
                if (c == 0)
                {
                    Cell(tr);
                    blank++;
                }
                else if (c == -1)
                    Cell(tr).Style.Add("background-color", "#FFeeee");
                else
                    Cell(tr, c.ToString("n0")).HorizontalAlign = HorizontalAlign.Right;
            }
            if (blank >= tr.Cells.Count - 5)
            {
                tr.Style.Add("background-color", "#666");
                tr.CssClass = "";
                tr.Cells[0].ForeColor = System.Drawing.Color.Yellow;
            }
        }
    }
}