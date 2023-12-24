using Microsoft.VisualBasic;
using SchemaAudit;
using Framework;
using SchemaMembership;
using System;
using Framework;

public class CSession : Framework.CSessionBase
{

    public static bool IsAdmin
    {
        get
        {
            return (IsLoggedIn && CUser.Current().IsInRole("Administrators"));
        }
    }

    public static bool IsLoggedIn
    {
        get
        {
            return CUser.IsLoggedIn;
        }
    }

    public static CUser User
    {
        get
        {
            return CUser.Current();
        }
    }

    public static CException PageMessageEx2
    {
        set
        {
            PageMessage = (value.Message + ("\r\n" + ("\r\n" + value.StackTrace)));
            while (!(value.Inner == null))
            {
                value = value.Inner;
                PageMessage +=  (value.Message + ("\r\n" + ("\r\n" + value.StackTrace)));
            }

        }
    }

    public static Exception PageMessageEx
    {
        set
        {
            Exception ex = value;
            CAudit_Error.Log(ex);
            PageMessage = (ex.Message + ("\r\n" + ("\r\n" + ex.StackTrace)));
            while (!(ex.InnerException == null))
            {
                ex = ex.InnerException;
                PageMessage += (ex.Message + ("\r\n" + ("\r\n" + ex.StackTrace)));
            }

        }
    }

    public static string PageMessage
    {
        get
        {
            return GetStr("PageMessage");
        }
        set
        {
            SetStr("PageMessage", value);
        }
    }

    public static CDataSrc Db(ESource source)
    {
        if ((source == ESource.Local))
        {
            return CDataSrc.Default;
        }

        if ((source == ESource.Prod))
        {
            return new CWebSrcBinary(CSession.Home_ProdUrl);
        }

        if ((source == ESource.Other))
        {
            return new CWebSrcBinary(CSession.Home_OtherUrl);
        }

        return null;
    }

    public static CSchemaInfo SchemaInfo(ESource source)
    {
        if ((source == ESource.Local))
        {
            return SchemaLocal;
        }

        if ((source == ESource.Prod))
        {
            return SchemaProd;
        }

        if ((source == ESource.Other))
        {
            return SchemaOther;
        }

        return null;
    }

    public static CDataSrc DbSrc
    {
        get
        {
            return CSession.Db(SourceId);
        }
    }

    public static CDataSrc DbTar
    {
        get
        {
            return CSession.Db(TargetId);
        }
    }

    public static CSchemaInfo SchemaSrc
    {
        get
        {
            return CSession.SchemaInfo(SourceId);
        }
        set
        {
            if ((SourceId == ESource.Local))
            {
                SchemaLocal = null;
            }
            else
            {
                SchemaProd = null;
            }

        }
    }

    public static CSchemaInfo SchemaTar
    {
        get
        {
            return CSession.SchemaInfo(TargetId);
        }
        set
        {
            if ((TargetId == ESource.Local))
            {
                SchemaLocal = null;
            }
            else
            {
                SchemaProd = null;
            }

        }
    }

    public static ESource SourceId
    {
        get
        {
            return (ESource)GetInt("RefSourceId", (int)ESource.Local);
        }
        set
        {
            SetInt("RefSourceId", (int)value);
        }
    }

    public static ESource TargetId
    {
        get
        {
            return (ESource)GetInt("RefTargetId", (int)ESource.Prod);
        }
        set
        {
            SetInt("RefTargetId", (int)value);
        }
    }

    public static CSchemaInfo SchemaLocal
    {
        get
        {
            CSchemaInfo obj = (CSchemaInfo)GetObj("SchemaLocal");
            if ((obj == null))
            {
                obj = CSession.Db(ESource.Local).SchemaInfo();
                SetObj("SchemaLocal", obj);
            }

            return obj;
        }
        set
        {
            SetObj("SchemaLocal", value);
        }
    }

    public static CSchemaInfo SchemaProd
    {
        get
        {
            CSchemaInfo obj = (CSchemaInfo)GetObj("SchemaProd");
            if ((obj == null))
            {
                obj = CSession.Db(ESource.Prod).SchemaInfo();
                SetObj("SchemaProd", obj);
            }

            return obj;
        }
        set
        {
            SetObj("SchemaProd", value);
        }
    }

    public static CSchemaInfo SchemaOther
    {
        get
        {
            CSchemaInfo obj = (CSchemaInfo)GetObj("SchemaOther");
            if ((obj == null))
            {
                obj = CSession.Db(ESource.Other).SchemaInfo();
                SetObj("SchemaOther", obj);
            }

            return obj;
        }
        set
        {
            SetObj("SchemaOther", value);
        }
    }

    public static int Home_ViewOrEdit
    {
        get
        {
            return GetInt("ViewOrEdit", 0);
        }
        set
        {
            SetInt("ViewOrEdit", value);
        }
    }

    public static string Home_DevDir
    {
        get
        {
            return GetStr("Home_DevDir", CPushUpgradeClient_Config.SELF_FOLDER);
        }
        set
        {
            SetStr("Home_DevDir", value);
        }
    }

    public static string Home_ProdHost
    {
        get
        {
            return GetStr("Home_ProdHost", CPushUpgradeClient_Config.DEFAULT_HOSTNAME);
        }
        set
        {
            SetStr("Home_ProdHost", value);
        }
    }

    public static string Home_ProdUrl
    {
        get
        {
            return GetStr("Home_ProdUrl", CPushUpgradeClient_Config.DefaultProdUrl);
        }
        set
        {
            SetStr("Home_ProdUrl", value);
        }
    }

    public static string Home_OtherUrl
    {
        get
        {
            return GetStr("Home_OtherUrl", "http://");
        }
        set
        {
            SetStr("Home_OtherUrl", value);
        }
    }
    public static bool Home_FastHash
    {
        get
        {
            return GetBool("Home_FastHash", true);
        }
        set
        {
            SetBool("Home_FastHash", value);
        }
    }
    public static string Home_RemoteDir
    {
        get
        {
            return GetStr("Home_RemoteDir", string.Empty);
        }
        set
        {
            SetStr("Home_RemoteDir", value);
        }
    }

    public static string Home_Ignore
    {
        get
        {
            return GetStr("Home_Ignore", CPushUpgradeClient_Config.DEFAULT_IGNORES);
        }
        set
        {
            SetStr("Home_Ignore", value);
        }
    }

    public static string TableName
    {
        get
        {
            return GetStr("TableName", "*");
        }
        set
        {
            SetStr("TableName", value);
        }
    }

    public static int Home_Data_FullScan
    {
        get
        {
            return GetInt("Home_Data_FullScan", 0);
        }
        set
        {
            SetInt("Home_Data_FullScan", value);
        }
    }

    public static CAudit_SearchFilters AuditTrailFilters()
    {
        CAudit_SearchFilters filters = ((CAudit_SearchFilters)(Get("AuditTrailFilters")));
        if ((filters == null))
        {
            filters = new CAudit_SearchFilters();
            Set("AuditTrailFilters", filters);
        }

        return filters;
    }

    public static bool SqlIsSelect
    {
        get
        {
            return GetBool("SqlIsSelect");
        }
        set
        {
            SetBool("SqlIsSelect", value);
        }
    }

    public static bool SqlAllClients
    {
        get
        {
            return GetBool("SqlAllClients");
        }
        set
        {
            SetBool("SqlAllClients", value);
        }
    }

    public static string SqlStatement
    {
        get
        {
            return GetStr("SqlStatement");
        }
        set
        {
            SetStr("SqlStatement", value);
        }
    }

    public static string SqlUseConn
    {
        get
        {
            return GetStr("SqlUseConn");
        }
        set
        {
            SetStr("SqlUseConn", value);
        }
    }

    public static bool SqlUseProd
    {
        get
        {
            return GetBool("SqlUseProd");
        }
        set
        {
            SetBool("SqlUseProd", value);
        }
    }
}