Imports Microsoft.VisualBasic
Imports SchemaAudit

Public Class CSession : Inherits Framework.CSessionBase

#Region "Current Login"
    Public Shared ReadOnly Property IsAdmin() As Boolean
        Get
            Return IsLoggedIn AndAlso CUser.Current.IsInRole("Administrators")
        End Get
    End Property
    Public Shared ReadOnly Property IsLoggedIn() As Boolean
        Get
            Return CUser.IsLoggedIn
        End Get
    End Property
    Public Shared ReadOnly Property User() As CUser
        Get
            Return CUser.Current
        End Get
    End Property
#End Region

#Region "PageMessage"
    Public Shared WriteOnly Property PageMessageEx2 As CException
        Set(ex As CException)
            PageMessage = ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            While Not IsNothing(ex.Inner)
                ex = ex.Inner
                PageMessage &= ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            End While
        End Set
    End Property
    Public Shared WriteOnly Property PageMessageEx As Exception
        Set(ex As Exception)
            CAudit_Error.Log(ex)

            PageMessage = ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            While Not IsNothing(ex.InnerException)
                ex = ex.InnerException
                PageMessage &= ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            End While
        End Set
    End Property
    Public Shared Property PageMessage() As String
        Get
            Return GetStr("PageMessage")
        End Get
        Set(ByVal value As String)
            SetStr("PageMessage", value)
        End Set
    End Property
#End Region

#Region "Self"
    Public Shared Function Db(source As ESource) As CDataSrc
        If source = ESource.Local Then Return CDataSrc.Default
        If source = ESource.Prod Then Return New CWebSrcBinary(CSession.Home_ProdUrl)
        If source = ESource.Other Then Return New CWebSrcBinary(CSession.Home_OtherUrl)
        Return Nothing
    End Function
    Public Shared Function SchemaInfo(source As ESource) As CSchemaInfo
        If source = ESource.Local Then Return SchemaLocal
        If source = ESource.Prod Then Return SchemaProd
        If source = ESource.Other Then Return SchemaOther
        Return Nothing
    End Function

    Public Shared ReadOnly Property DbSrc As CDataSrc
        Get
            Return Db(SourceId)
        End Get
    End Property
    Public Shared ReadOnly Property DbTar As CDataSrc
        Get
            Return Db(TargetId)
        End Get
    End Property

    Public Shared Property SchemaSrc As CSchemaInfo
        Get
            Return SchemaInfo(SourceId)
        End Get
        Set(value As CSchemaInfo)
            If SourceId = ESource.Local Then
                SchemaLocal = Nothing
            Else
                SchemaProd = Nothing
            End If
        End Set
    End Property
    Public Shared Property SchemaTar As CSchemaInfo
        Get
            Return SchemaInfo(TargetId)
        End Get
        Set(value As CSchemaInfo)
            If TargetId = ESource.Local Then
                SchemaLocal = Nothing
            Else
                SchemaProd = Nothing
            End If
        End Set
    End Property


    Public Shared Property SourceId() As ESource
        Get
            Return GetInt("RefSourceId", ESource.Local)
        End Get
        Set(ByVal value As ESource)
            SetInt("RefSourceId", value)
        End Set
    End Property
    Public Shared Property TargetId() As ESource
        Get
            Return GetInt("RefTargetId", ESource.Prod)
        End Get
        Set(ByVal value As ESource)
            SetInt("RefTargetId", value)
        End Set
    End Property


    Public Shared Property SchemaLocal() As CSchemaInfo
        Get
            Dim obj As CSchemaInfo = GetObj("SchemaLocal")
            If IsNothing(obj) Then
                obj = Db(ESource.Local).SchemaInfo
                SetObj("SchemaLocal", obj)
            End If
            Return obj
        End Get
        Set(ByVal value As CSchemaInfo)
            SetObj("SchemaLocal", value)
        End Set
    End Property
    Public Shared Property SchemaProd() As CSchemaInfo
        Get
            Dim obj As CSchemaInfo = GetObj("SchemaProd")
            If IsNothing(obj) Then
                obj = Db(ESource.Prod).SchemaInfo
                SetObj("SchemaProd", obj)
            End If
            Return obj
        End Get
        Set(ByVal value As CSchemaInfo)
            SetObj("SchemaProd", value)
        End Set
    End Property
    Public Shared Property SchemaOther() As CSchemaInfo
        Get
            Dim obj As CSchemaInfo = GetObj("SchemaOther")
            If IsNothing(obj) Then
                obj = Db(ESource.Other).SchemaInfo
                SetObj("SchemaOther", obj)
            End If
            Return obj
        End Get
        Set(ByVal value As CSchemaInfo)
            SetObj("SchemaOther", value)
        End Set
    End Property



    Public Shared Property Home_ViewOrEdit() As Integer
        Get
            Return GetInt("ViewOrEdit", 0)
        End Get
        Set(ByVal value As Integer)
            SetInt("ViewOrEdit", value)
        End Set
    End Property
    Public Shared Property Home_DevDir() As String
        Get
            Return GetStr("Home_DevDir", CPushUpgradeClient_Config.SELF_FOLDER)
        End Get
        Set(ByVal value As String)
            SetStr("Home_DevDir", value)
        End Set
    End Property
    Public Shared Property Home_ProdHost() As String
        Get
            Return GetStr("Home_ProdHost", CPushUpgradeClient_Config.DEFAULT_HOSTNAME)
        End Get
        Set(ByVal value As String)
            SetStr("Home_ProdHost", value)
        End Set
    End Property
    Public Shared Property Home_RemoteDir() As String
        Get
            Return GetStr("Home_RemoteDir")
        End Get
        Set(ByVal value As String)
            SetStr("Home_RemoteDir", value)
        End Set
    End Property
    Public Shared Property Home_ProdUrl() As String
        Get
            Return GetStr("Home_ProdUrl", CPushUpgradeClient_Config.DefaultProdUrl)
        End Get
        Set(ByVal value As String)
            SetStr("Home_ProdUrl", value)
        End Set
    End Property
    Public Shared Property Home_OtherUrl() As String
        Get
            Return GetStr("Home_OtherUrl", "http://")
        End Get
        Set(ByVal value As String)
            SetStr("Home_OtherUrl", value)
        End Set
    End Property
    Public Shared Property Home_Ignore() As String
        Get
            Return GetStr("Home_Ignore", CPushUpgradeClient_Config.DEFAULT_IGNORES)
        End Get
        Set(ByVal value As String)
            SetStr("Home_Ignore", value)
        End Set
    End Property
    Public Shared Property Home_FastHash() As Boolean
        Get
            Return GetBool("Home_FastHash", False)
        End Get
        Set(ByVal value As Boolean)
            SetBool("Home_FastHash", value)
        End Set
    End Property

    Public Shared Property TableName() As String
        Get
            Return GetStr("TableName", "*")
        End Get
        Set(ByVal value As String)
            SetStr("TableName", value)
        End Set
    End Property
    Public Shared Property Home_Data_FullScan() As Integer
        Get
            Return GetInt("Home_Data_FullScan", 0)
        End Get
        Set(ByVal value As Integer)
            SetStr("Home_Data_FullScan", value)
        End Set
    End Property
#End Region


#Region "Search Filters - AuditTrail"
    Public Shared Function AuditTrailFilters() As CAudit_SearchFilters
        Dim filters As CAudit_SearchFilters = CType([Get]("AuditTrailFilters"), CAudit_SearchFilters)
        If IsNothing(filters) Then
            filters = New CAudit_SearchFilters
            [Set]("AuditTrailFilters", filters)
        End If
        Return filters
    End Function
#End Region

#Region "Sql - Current Query"
    Public Shared Property SqlIsSelect() As Boolean
        Get
            Return GetBool("SqlIsSelect")
        End Get
        Set(ByVal value As Boolean)
            SetBool("SqlIsSelect", value)
        End Set
    End Property
    Public Shared Property SqlAllClients() As Boolean
        Get
            Return GetBool("SqlAllClients")
        End Get
        Set(ByVal value As Boolean)
            SetBool("SqlAllClients", value)
        End Set
    End Property
    Public Shared Property SqlStatement() As String
        Get
            Return GetStr("SqlStatement")
        End Get
        Set(ByVal value As String)
            SetStr("SqlStatement", value)
        End Set
    End Property
    Public Shared Property SqlUseConn() As String
        Get
            Return GetStr("SqlUseConn")
        End Get
        Set(ByVal value As String)
            SetStr("SqlUseConn", value)
        End Set
    End Property

    Public Shared Property SqlUseProd() As Boolean
        Get
            Return GetBool("SqlUseProd")
        End Get
        Set(ByVal value As Boolean)
            SetBool("SqlUseProd", value)
        End Set
    End Property

#End Region

End Class
