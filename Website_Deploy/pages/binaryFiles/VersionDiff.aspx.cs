using Framework;
using SchemaDeploy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_binaryFiles_VersionDiff :  CPageDeploy
{
    //Querystring
    public int Version1Id { get { return CWeb.RequestInt("v1", Instance.TargetVersionId); } }
    public int Version2Id { get { return CWeb.RequestInt("v2", Instance.LastReport().ReportInitialVersionId); } }
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }


    //Data
    private CVersion V1 { get { return CVersion.Cache.GetById(Version1Id); } }
    private CVersion V2 { get { return CVersion.Cache.GetById(Version2Id); } }
    private CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }


    //Page Events
    protected override void PageInit()
    {
        
    }
    protected override void PagePreRender()
    {
        var f1 = V1.FolderHash;
        var f2 = V2.FolderHash;


        bool md5 = rbl.SelectedIndex == 0;
        if (md5)
        {
            var diffGuid = CDiffLogic.Diff(f1.Hashes, f2.Hashes);

            var newMd5 = CBinaryFile.Cache.GetByIds(diffGuid.TargetOnly);
            var delMd5 = CBinaryFile.Cache.GetByIds(diffGuid.SourceOnly);
            var matMd5 = CBinaryFile.Cache.GetByIds(diffGuid.Matching);

            ctrlNew.Display(newMd5);
            ctrlDel.Display(delMd5);
            ctrlSame.Display(delMd5);
        }
        else
        {
            var diffName = CDiffLogic.Diff(f1.Names, f2.Names);

            var newName = V2.VersionFiles.GetByPaths(diffName.TargetOnly);
            var delName = V1.VersionFiles.GetByPaths(diffName.SourceOnly);
            var matName = V1.VersionFiles.GetByPaths(diffName.Matching);

            ctrlNew.Display(newName);
            ctrlDel.Display(delName);
            ctrlSame.Display(matName);
        }
    }

    //Form events

    protected void ddVersion_SelectedIndexChanged(object sender, EventArgs e)
    {
        var verId = CDropdown.GetInt(ddVersion);
        Response.Redirect(CSitemap.VersionDiff(verId, Version2Id), true);
    }

    protected void ddInstance_SelectedIndexChanged(object sender, EventArgs e)
    {
        var instanceId = CDropdown.GetInt(ddInstance);
        Response.Redirect(CSitemap.SchemaForInstance(instanceId));

    }

}