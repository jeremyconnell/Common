Imports System.Data
Imports Comms.PushUpgrade.Client
Imports Comms.PushUpgrade.Interface
Imports Comms.Upgrade.Client
Imports Microsoft.VisualBasic
Imports SchemaAudit
Imports SchemaDeploy

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
			Return GetStr("Home_ProdHost", CUpgradeClient_Config.DEFAULT_HOSTNAME)
		End Get
		Set(ByVal value As String)
			SetStr("Home_ProdHost", value)
		End Set
	End Property
	Public Shared Property Home_Ignore() As String
		Get
			Return GetStr("Home_Ignore", CUpgradeClient_Config.Shared.ExtensionsToIgnore)
		End Get
		Set(ByVal value As String)
			SetStr("Home_Ignore", value)
		End Set
	End Property
	Public Shared Property Home_Schema_RefInstanceId() As Integer
		Get
			Return GetInt("Home_Schema_RefInstanceId", CInstance.Cache(0).InstanceId)
		End Get
		Set(ByVal value As Integer)
			SetInt("Home_Schema_RefInstanceId", value)
		End Set
	End Property
	Public Shared Property Home_Schema_RefSourceId() As ESource
		Get
			Return GetInt("Home_Schema_RefSourceId", ESource.Local)
		End Get
		Set(ByVal value As ESource)
			SetInt("Home_Schema_RefSourceId", value)
		End Set
	End Property
	Public Shared Property Home_Data_TableName() As String
		Get
			Return GetStr("Home_Data_TableName", "*")
		End Get
		Set(ByVal value As String)
			SetStr("Home_Data_TableName", value)
		End Set
	End Property



	Public Shared Property Instance_UserLogins(instanceId As Integer) As List(Of String)
		Get
			Return GetStrList("Instance_UserLogins" & instanceId)
		End Get
		Set(ByVal value As List(Of String))
			SetObj("Instance_UserLogins" & instanceId, value)
		End Set
	End Property

	Public Shared Property MonitorTab As Integer
		Get
			Return GetInt("MonitorTab", 0)
		End Get
		Set(ByVal value As Integer)
			SetInt("MonitorTab", value)
		End Set
	End Property
	Public Shared Function MyVersion(p As CPushUpgradeClient, allData As Boolean, attempts As Integer) As CMyVersion
		Dim key As String = "MyVersion" & p.Url
		Dim v As CMyVersion = [Get](key)
		If IsNothing(v) Then
			v = p.PollVersion(allData, attempts)
			SetObj(key, v)
		End If
		Return v
	End Function

	Public Shared Sub MyVersionReset(p As CPushUpgradeClient)
		Dim key As String = "MyVersion" & p.Url
		SetObj(key, Nothing)
	End Sub
	Public Shared Property FeaturesAppId As Integer
		Get
			Return GetInt("FeaturesAppId", 1)
		End Get
		Set(ByVal value As Integer)
			SetInt("FeaturesAppId", value)
		End Set
	End Property



	Public Shared Property TableNames As List(Of String)
		Get
			Return GetObj("TableNames")
		End Get
		Set(ByVal value As List(Of String))
			SetObj("TableNames", value)
		End Set
	End Property

	Public Shared Property DataTable As DataTable
		Get
			Return GetObj("DataTable")
		End Get
		Set(ByVal value As DataTable)
			SetObj("DataTable", value)
		End Set
	End Property

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
	Public Shared Property SqlRunOnAllInstancesOfAppId() As Integer
		Get
			Return GetInt("SqlRunOnAllInstancesOfAppId")
		End Get
		Set(ByVal value As Integer)
			SetInt("SqlRunOnAllInstancesOfAppId", value)
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


	Public Shared Property TextForIframe() As String
		Get
			Return GetStr("TextForIframe")
		End Get
		Set(ByVal value As String)
			SetStr("TextForIframe", value)
		End Set
	End Property
#End Region

End Class
