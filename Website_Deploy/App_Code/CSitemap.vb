Imports Microsoft.VisualBasic
Imports System.Web.HttpContext
Imports Framework
Imports SchemaDeploy


Public Enum EFix
	None = Integer.MinValue
	All = 0
	Indexes = 1
	StoredProcs = 2
	ForeignKeys = 3
	Views = 4
	Tables = 5
	Cols = 6
	DefaultVals = 7
	Migration = 10
	ScriptOnly = 1000
End Enum
Public Enum ESource
	Local = -100
	Prod = -200
End Enum

Public Class CSitemap

#Region "Home"
	Public Shared Function Home() As String
		Return "~/default.aspx"
	End Function
	Public Shared Function Home(tab As Integer) As String
		Return String.Concat(CSitemap.Home, "?tab=", CInt(tab))
	End Function
	Public Shared Function Home(tab As Integer, database As String) As String
		Return String.Concat(CSitemap.Home(tab), "&database=", Encode(database))
	End Function
	Public Shared Function Home_Diff(tab As Integer, diffClientId As Integer, Optional fixId As Integer = Integer.MinValue) As String
		Return String.Concat(CSitemap.Home(tab), "&diff=", Encode(diffClientId), "&fixId=", Str(fixId))
	End Function
#End Region

#Region "Backups"
	'List/Search
	Public Shared Function [Backups]() As String
		Return Backups(CInt(EApp.ControlTrack))
	End Function
	Public Shared Function [Backups](appId As Integer) As String
		Return String.Concat("~/pages/backups/default.aspx?appId=", Str(appId))
	End Function
	Public Shared Function [Backups](appId As Integer, instanceId As Integer) As String
		Return String.Concat(Backups(appId), "&instanceId=", Str(instanceId))
	End Function
	Public Shared Function [Backups](appId As Integer, instanceId As Integer, ByVal search As String) As String
		Return String.Concat(CSitemap.Backups(appId, instanceId), "&search=", Encode(search))
	End Function
	Public Shared Function [Backups](appId As Integer, instanceId As Integer, ByVal search As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Backups(appId, instanceId, search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function BackupAddEdit() As String
		Return "~/pages/backups/backup.aspx"
	End Function

	Public Shared Function Backup(ByVal backupId As Integer) As String
		Return String.Concat(BackupAddEdit, "?backupId=", backupId)
	End Function

	'UserControls
	Public Shared Function UCBackup() As String
		Return "~/pages//backups/usercontrols/UCBackup.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function BackupUploads() As String
		Return "~/uploads/backups/"
	End Function
#End Region


#Region "BackupItems"
	'List/Search
	Public Shared Function BackupItems() As String
		Return BackupItems(CInt(EApp.ControlTrack))
	End Function
	Public Shared Function BackupItems(appId As Integer) As String
		Return String.Concat("~/pages/backupItems/default.aspx?appId=", Str(appId))
	End Function
	Public Shared Function BackupItems(appId As Integer, instanceId As Integer) As String
		Return String.Concat(BackupItems(appId), "&instanceId=", Str(instanceId))
	End Function
	Public Shared Function BackupItems(appId As Integer, instanceId As Integer, ByVal table As String, search As String) As String
		Return String.Concat(CSitemap.BackupItems(appId, instanceId), "&table=", Encode(table), "&search=", Encode(search))
	End Function
	Public Shared Function BackupItems(appId As Integer, instanceId As Integer, table As String, ByVal search As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.BackupItems(appId, instanceId, table, search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function


	'UserControls
	Public Shared Function UCBackupItem() As String
		Return "~/pages//backupItems/usercontrols/UCBackupItem.ascx"
	End Function

	'View Details
	Public Shared Function [BackupItem](ByVal itemId As Integer) As String
		Return String.Concat("~/pages/backupItems/backupItem.aspx?itemId=", itemId)
	End Function
#End Region



#Region "Azure"
	Public Shared Function AzureDbs() As String
		Return "~/pages/azure/databases.aspx"
	End Function
	Public Shared Function AzureDbs(dbName As String) As String
		Return String.Concat(CSitemap.AzureDbs, "?database=", Encode(dbName))
	End Function
	Public Shared Function AzureWebs() As String
		Return "~/pages/azure/websites.aspx"
	End Function
#End Region

#Region "Self"
	Public Shared Function SelfDeploy() As String
		Return "~/pages/self/deploy.aspx"
	End Function
	Public Shared Function SelfSchemaSync() As String
		Return "~/pages/self/schema.aspx"
	End Function
	Public Shared Function SelfDataSync() As String
		Return "~/pages/self/data.aspx"
	End Function
	Public Shared Function SelfSql() As String
		Return "~/pages/self/sql.aspx"
	End Function


	Public Shared Function SelfSchemaSync_Diff(instanceId As Integer) As String
		Return String.Concat("~/pages/self/schema.aspx?diffInstanceId=", instanceId)
	End Function
	Public Shared Function SelfSchemaSync_Diff(instanceId As Integer, fix As Integer) As String
		Return String.Concat(SelfSchemaSync_Diff(instanceId), "&fix=", Str(fix))
	End Function
#End Region

#Region "Releases"
	'List/Search
	Public Shared Function [Releases]() As String
		Return CSitemap.Releases(EApp.ControlTrack)
	End Function
	Public Shared Function [Releases](ByVal appId As Integer) As String
		Return String.Concat("~/pages/releases/default.aspx?appId=", Str(appId))
	End Function
	Public Shared Function [Releases](ByVal appId As Integer, instanceId As Integer) As String
		Return String.Concat(CSitemap.Releases(appId), "&instanceId=", Str(instanceId))
	End Function
	Public Shared Function [Releases](ByVal appId As Integer, instanceId As Integer, ByVal search As String) As String
		Return String.Concat(CSitemap.Releases(appId, instanceId), "&search=", Encode(search))
	End Function
	Public Shared Function [Releases](ByVal appId As Integer, instanceId As Integer, ByVal search As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Releases(appId, instanceId, search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function ReleaseAddEdit() As String
		Return "~/pages/releases/release.aspx"
	End Function
	Public Shared Function ReleaseAdd() As String 'May require a parentId (follow pattern below)
		Return ReleaseAddEdit()
	End Function
	Public Shared Function ReleaseEdit(ByVal releaseId As Integer) As String
		Return String.Concat(ReleaseAddEdit, "?releaseId=", releaseId)
	End Function

	'UserControls
	Public Shared Function UCRelease() As String
		Return "~/pages//releases/usercontrols/UCRelease.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function ReleaseUploads() As String
		Return "~/uploads/releases/"
	End Function
#End Region

#Region "PushedUpgrades"
	Public Shared Function [PushedUpgrades]() As String
		Return CSitemap.PushedUpgrades(EApp.ControlTrack)
	End Function
	Public Shared Function [PushedUpgrades](appId As Integer) As String
		Return String.Concat("~/pages/pushedUpgrades/default.aspx?appId=", Str(appId))
	End Function
	Public Shared Function [PushedUpgrades](appId As Integer, instanceId As Integer) As String
		Return String.Concat(CSitemap.PushedUpgrades(appId), "&instanceId=", Str(instanceId))
	End Function
	Public Shared Function [PushedUpgrades](appId As Integer, instanceId As Integer, ByVal search As String) As String
		Return String.Concat(CSitemap.PushedUpgrades(appId, instanceId), "&search=", Encode(search))
	End Function
	Public Shared Function [PushedUpgrades](appId As Integer, instanceId As Integer, ByVal search As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.PushedUpgrades(appId, instanceId, search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function PushedUpgradeAddEdit() As String
		Return "~/pages/pushedUpgrades/pushedUpgrade.aspx"
	End Function
	Public Shared Function PushedUpgradeAdd() As String 'May require a parentId (follow pattern below)
		Return PushedUpgradeAddEdit()
	End Function
	Public Shared Function PushedUpgradeEdit(ByVal pushId As Integer) As String
		Return String.Concat(PushedUpgradeAddEdit, "?pushId=", pushId)
	End Function

	'UserControls
	Public Shared Function UCPushedUpgrade() As String
		Return "~/pages//pushedUpgrades/usercontrols/UCPushedUpgrade.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function PushedUpgradeUploads() As String
		Return "~/uploads/pushedUpgrades/"
	End Function
#End Region

#Region "ReportHistorys"
	'List/Search 
	Public Shared Function [ReportHistorys]() As String
		Return ReportHistorys(EApp.ControlTrack)
	End Function
	Public Shared Function [ReportHistorys](appId As Integer) As String
		Return String.Concat("~/pages/reportHistorys/Default.aspx?appId=", Str(appId))
	End Function
	Public Shared Function [ReportHistorys](appId As Integer, instanceId As Integer) As String
		Return String.Concat(CSitemap.ReportHistorys(appId), "&instanceId=", Str(instanceId))
	End Function
	Public Shared Function [ReportHistorys](appId As Integer, instanceId As Integer, ByVal search As String) As String
		Return String.Concat(CSitemap.ReportHistorys(appId, instanceId), "&search=", Encode(search))
	End Function
	Public Shared Function [ReportHistorys](appId As Integer, instanceId As Integer, ByVal search As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.ReportHistorys(appId, instanceId, search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function ReportHistoryAddEdit() As String
		Return "~/pages/reportHistorys/reportHistory.aspx"
	End Function
	Public Shared Function ReportHistoryAdd() As String 'May require a parentId (follow pattern below)
		Return ReportHistoryAddEdit()
	End Function
	Public Shared Function ReportHistoryEdit(ByVal reportId As Integer) As String
		Return String.Concat(ReportHistoryAddEdit, "?reportId=", reportId)
	End Function

	'UserControls
	Public Shared Function UCReportHistory() As String
		Return "~/pages//reportHistorys/usercontrols/UCReportHistory.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function ReportHistoryUploads() As String
		Return "~/uploads/reportHistorys/"
	End Function
#End Region

#Region "UpgradeHistorys"
	'List/Search
	Public Shared Function [UpgradeHistorys]() As String
		Return [UpgradeHistorys](EApp.ControlTrack)
	End Function
	Public Shared Function [UpgradeHistorys](appId As Integer) As String
		Return String.Concat("~/pages/upgradeHistorys/default.aspx?appId=", Str(appId))
	End Function
	Public Shared Function [UpgradeHistorys](appId As Integer, instanceId As Integer) As String
		Return String.Concat(CSitemap.UpgradeHistorys(appId), "&instanceId=", Str(instanceId))
	End Function
	Public Shared Function [UpgradeHistorys](appId As Integer, instanceId As Integer, ByVal search As String) As String
		Return String.Concat(CSitemap.UpgradeHistorys(appId, instanceId), "&search=", Encode(search))
	End Function
	Public Shared Function [UpgradeHistorys](appId As Integer, instanceId As Integer, ByVal search As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.UpgradeHistorys(appId, instanceId, search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'UserControls
	Public Shared Function UCUpgradeHistory() As String
		Return "~/pages//upgradeHistorys/usercontrols/UCUpgradeHistory.ascx"
	End Function

	'View Details
	Public Shared Function UpgradeHistory(ByVal changeId As Integer) As String
		Return String.Concat("~/pages/upgradeHistorys/upgradeHistory.aspx?changeId=", changeId)
	End Function
#End Region


#Region "Instances"
	'List/Search
	Public Shared Function [Instances]() As String
		Return "~/pages/instances/default.aspx"
	End Function
	Public Shared Function [Instances](ByVal search As String, ByVal appId As Integer, clientId As Integer) As String
		Return String.Concat(CSitemap.Instances(), "?search=", Encode(search), "&appId=", Str(appId), "&clientId=", Str(clientId))
	End Function
	Public Shared Function [Instances](ByVal search As String, ByVal appId As EApp, clientId As Integer) As String
		Return Instances(search, CInt(appId), clientId)
	End Function
	Public Shared Function [Instances](ByVal appId As Integer) As String
		Return String.Concat(CSitemap.Instances(), "?appId=", appId)
	End Function
	Public Shared Function [Instances](ByVal appId As EApp) As String
		Return Instances(CInt(appId))
	End Function
	Public Shared Function [InstancesByClient](ByVal clientId As Integer) As String
		Return String.Concat(CSitemap.Instances(), "?clientId=", clientId)
	End Function
	Public Shared Function [Instances](ByVal search As String, ByVal appId As Integer, clientId As Integer, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Instances(search, appId, clientId), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	Public Shared Function InstancesForTargetVersion(appId As Integer, activeVersionId As Integer) As String
		Return String.Concat(CSitemap.Instances(appId), "&targetVersionId=", activeVersionId)
	End Function
	Public Shared Function InstancesForLastVersion(appId As Integer, lastVersionId As Integer) As String
		Return String.Concat(CSitemap.Instances(appId), "&lastVersionId=", lastVersionId)
	End Function

	'Add/Edit
	Private Shared Function InstanceAddEdit() As String
		Return "~/pages/instances/instance.aspx"
	End Function
	Public Shared Function InstanceAdd(ByVal appId As Integer) As String 'May require a parentId (follow pattern below)
		Return String.Concat(InstanceAddEdit, "?appId=", Str(appId))
	End Function
	Public Shared Function InstanceAdd(ByVal appId As Integer, clientId As Integer) As String 'May require a parentId (follow pattern below)
		Return String.Concat(InstanceAdd(appId), "&clientId=", Str(clientId))
	End Function
	Public Shared Function InstanceAdd(ByVal appId As EApp, clientId As Integer) As String 'May require a parentId (follow pattern below)
		Return InstanceAdd(CInt(appId), clientId)
	End Function
	Public Shared Function Instance(ByVal instanceId As Integer) As String
		Return String.Concat(InstanceAddEdit, "?instanceId=", instanceId)
	End Function
	Public Shared Function Instance(ByVal instanceId As Integer, clientId As Integer) As String
		Return String.Concat(Instance(instanceId), "&clientId=", Str(clientId))
	End Function
	Public Shared Function InstanceSettings(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/settings.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceSettings(instanceId As Integer, groupId As Integer) As String
		Return String.Concat(InstanceSettings(instanceId), "&groupId=", Str(groupId))
	End Function
	Public Shared Function InstanceReconcile(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/reconcile.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceMonitor(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/monitor.aspx?instanceId=", Str(instanceId))
	End Function

	Public Shared Function InstanceMonitorView(instanceId As Integer, showing As String) As String
		Return String.Concat(InstanceMonitor(instanceId), "&showing=", Encode(showing))
	End Function
	Public Shared Function InstanceMonitorEdit(instanceId As Integer, showing As String) As String
		Return String.Concat(InstanceMonitorView(instanceId, showing), "&edit=true")
	End Function
	Public Shared Function InstanceMonitorDownload(instanceId As Integer, logFile As String) As String
		Return String.Concat(InstanceMonitor(instanceId), "&logFile=", Encode(logFile))
	End Function
	Public Shared Function InstanceMonitorIframe(instanceId As Integer, logFile As String) As String
		Return String.Concat(InstanceMonitorDownload(instanceId, logFile), "&iframe=true")
	End Function
	Public Shared Function InstanceSql(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/sql.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceVersion(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/version.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceWebsite(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/website.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceDatabase(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/database.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceFtp(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/ftp.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceFtp(instanceId As Integer, subdir As String) As String
		Return String.Concat(InstanceFtp(instanceId), "&subDir=", Encode(subdir))
	End Function
	Public Shared Function InstanceFtpView(instanceId As Integer, subdir As String, fileName As String) As String
		Return String.Concat(InstanceFtp(instanceId, subdir), "&view=", Encode(fileName))
	End Function
	Public Shared Function InstanceFtpEdit(instanceId As Integer, subdir As String, fileName As String) As String
		Return String.Concat(InstanceFtp(instanceId, subdir), "&edit=", Encode(fileName))
	End Function
	Public Shared Function InstanceData(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/data.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceDataImport(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/dataImport.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceSchema(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/schema.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceErrorLog(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/Audit_errors.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceErrorLog(instanceId As Integer, ByVal search As String, ByVal uniqueOnly As Boolean) As String
		Return String.Concat(InstanceErrorLog(instanceId), "&search=", Encode(search), "&uniqueOnly=", uniqueOnly)
	End Function
	Public Shared Function InstanceErrorLog(instanceId As Integer, ByVal search As String, ByVal uniqueOnly As Boolean, ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) As String
		Return String.Concat(CSitemap.InstanceErrorLog(instanceId, search, uniqueOnly), "&sortBy=", sortBy, "&desc=", descending, "&p=", pageNumber)
	End Function


	Public Shared Function InstanceFeatures(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/FeaturesForRoles.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceFeatures(instanceId As Integer, featureId As Integer) As String
		Return String.Concat(InstanceFeatures(instanceId), "&featureId=", Str(featureId))
	End Function

	Public Shared Function InstanceAppLogin(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/appLogin.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceAppLogin(instanceId As Integer, userName As String) As String
		Return String.Concat(InstanceAppLogin(instanceId), "&userName=", Encode(userName))
	End Function


	Public Shared Function InstanceAuditTrail(instanceId As Integer) As String
		Return String.Concat("~/pages/instances/audit_trail.aspx?instanceId=", Str(instanceId))
	End Function
	Public Shared Function InstanceAuditTrail(instanceId As Integer, ByVal search As String, typeId As Integer) As String
		Return String.Concat(CSitemap.InstanceAuditTrail(instanceId), "?search=", Encode(search), "&typeId=", Str(typeId))
	End Function
	Public Shared Function InstanceAuditTrail(instanceId As Integer, ByVal search As String, typeId As Integer, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.InstanceAuditTrail(instanceId, search, typeId), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function


	Public Shared Function InstanceAuditTrailLog(instanceId As Integer, logId As Integer) As String
		Return String.Concat("~/pages/instances/audit_log.aspx?instanceId=", Str(instanceId), "&logId=", logId)
	End Function


	'UserControls
	Public Shared Function UCInstance() As String
		Return "~/pages//instances/usercontrols/UCInstance.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function InstanceUploads() As String
		Return "~/uploads/instances/"
	End Function
#End Region

#Region "Versions"
	'List/Search
	Private Shared Function [Versions]() As String
		Return "~/pages/versions/default.aspx"
	End Function
	Public Shared Function [Versions](ByVal search As String) As String
		Return String.Concat(CSitemap.Versions(), "?search=", Encode(search))
	End Function
	Public Shared Function [Versions](ByVal search As String, ByVal appId As Integer) As String
		Return String.Concat(CSitemap.Versions(), "?search=", Encode(search), "&appId=", Str(appId))
	End Function
	Public Shared Function [Versions](ByVal appId As Integer) As String
		Return String.Concat(CSitemap.Versions(), "?appId=", appId)
	End Function
	Public Shared Function [Versions](ByVal search As String, ByVal appId As Integer, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Versions(search, appId), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function VersionAddEdit() As String
		Return "~/pages/versions/version.aspx"
	End Function
	Public Shared Function VersionAdd(ByVal appId As Integer) As String 'May require a parentId (follow pattern below)
		Return String.Concat(VersionAddEdit, "?appId=", appId)
	End Function
	Public Shared Function VersionEdit(ByVal versionId As Integer) As String
		Return String.Concat(VersionAddEdit, "?versionId=", versionId)
	End Function

	'UserControls
	Public Shared Function UCVersion() As String
		Return "~/pages//versions/usercontrols/UCVersion.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function VersionUploads() As String
		Return "~/uploads/versions/"
	End Function
#End Region

#Region "BinaryFiles"
	'List/Search
	Public Shared Function [BinaryFiles]() As String
		Return "~/pages/binaryFiles/default.aspx"
	End Function
	Public Shared Function [BinaryFiles](ByVal appId As Integer) As String
		Return String.Concat(CSitemap.BinaryFiles(String.Empty, appId, Integer.MinValue))
	End Function
	Public Shared Function [BinaryFiles](ByVal appId As Integer, verId As Integer) As String
		Return String.Concat(CSitemap.BinaryFiles(String.Empty, appId, verId))
	End Function
	Public Shared Function [BinaryFiles](ByVal search As String, ByVal appId As Integer, verId As Integer, Optional isSchema As Boolean = 0) As String
		Return String.Concat(CSitemap.BinaryFiles(), "?search=", Encode(search), "&appId=", Str(appId), "&verId=", Str(verId), IIf(isSchema, "&isSchema=1", ""))
	End Function
	Public Shared Function [BinaryFiles](ByVal search As String, ByVal appId As Integer, verId As Integer, isSchema As Boolean, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.BinaryFiles(search, appId, verId, isSchema), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Public Shared Function Schema(md5 As Guid) As String
		Return String.Concat("~/pages/binaryFiles/schema.aspx?md5=", md5)
	End Function
	Public Shared Function Schema(versionId As Integer) As String
		Return String.Concat("~/pages/binaryFiles/schema.aspx?versionId=", versionId)
	End Function
	Public Shared Function SchemaForInstance(instanceId As Integer) As String
		Return String.Concat("~/pages/binaryFiles/schema.aspx?instanceId=", instanceId)
	End Function


	Public Shared Function VersionDiff(v1 As Integer, v2 As Integer) As String
		Return String.Concat("~/pages/binaryFiles/VersionDiff.aspx?v1=", v1, "&v2=", v1)
	End Function
	Public Shared Function VersionDiffForInstance(instanceId As Integer) As String
		Return String.Concat("~/pages/binaryFiles/VersionDiff.aspx?instanceId=", instanceId)
	End Function

	'UserControls
	Public Shared Function UCBinaryFile() As String
		Return "~/pages/binaryFiles/usercontrols/UCBinaryFile.ascx"
	End Function
	Public Shared Function UCTableInfo() As String
		Return "~/pages/binaryFiles/usercontrols/UCTable.ascx"
	End Function
	Public Shared Function UCColumn() As String
		Return "~/pages/binaryFiles/usercontrols/UCColumn.ascx"
	End Function
	Public Shared Function UCStoredProc() As String
		Return "~/pages/binaryFiles/usercontrols/UCStoredProc.ascx"
	End Function
	Public Shared Function UCForeignKey() As String
		Return "~/pages/binaryFiles/usercontrols/UCForeignKey.ascx"
	End Function
	Public Shared Function UCDefaultValue() As String
		Return "~/pages/binaryFiles/usercontrols/UCDefaultValue.ascx"
	End Function
	Public Shared Function UCIndex() As String
		Return "~/pages/binaryFiles/usercontrols/UCIndex.ascx"
	End Function
	Public Shared Function UCView() As String
		Return "~/pages/binaryFiles/usercontrols/UCView.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function BinaryFileUploads() As String
		Return "~/uploads/binaryFiles/"
	End Function
#End Region

#Region "Apps"
	'List/Search
	Public Shared Function [Apps]() As String
		Return "~/pages/apps/default.aspx"
	End Function
	Public Shared Function [Apps](ByVal search As String) As String
		Return String.Concat(CSitemap.Apps(), "?search=", Encode(search))
	End Function
	Public Shared Function [Apps](ByVal search As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Apps(search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function AppAddEdit() As String
		Return "~/pages/apps/app.aspx"
	End Function
	Public Shared Function AppAdd() As String 'May require a parentId (follow pattern below)
		Return AppAddEdit()
	End Function
	Public Shared Function AppEdit(ByVal appId As Integer) As String
		Return String.Concat(AppAddEdit, "?appId=", appId)
	End Function



	Public Shared Function AppSchema() As String
		Return AppSchema(EApp.ControlTrack)
	End Function
	Public Shared Function AppData() As String
		Return AppSchema(EApp.ControlTrack)
	End Function

	Public Shared Function AppSchema(ByVal appId As Integer) As String
		Return String.Concat("~/pages/apps/schema.aspx?appId=", Str(appId))
	End Function
	Public Shared Function AppData(ByVal appId As Integer) As String
		Return String.Concat("~/pages/apps/data.aspx?appId=", Str(appId))
	End Function
	Private Shared Function AppSchema_Diff(ByVal diffInstanceId As Integer) As String
		Return String.Concat("~/pages/apps/schema.aspx?diffInstanceId=", Str(diffInstanceId))
	End Function
	Public Shared Function AppSchema_Diff(ByVal diffInstanceId As Integer, fixId As Integer) As String
		Return String.Concat(AppSchema_Diff(diffInstanceId), "&fix=", Str(fixId))
	End Function


	'UserControls
	Public Shared Function UCApp() As String
		Return "~/pages//apps/usercontrols/UCApp.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function AppUploads() As String
		Return "~/uploads/apps/"
	End Function
#End Region

#Region "Values"
	'List/Search
	Public Shared Function [Values]() As String
		Return "~/pages/values/default.aspx"
	End Function
	Public Shared Function [Values](instanceId As Integer, appId As Integer) As String
		Return String.Concat(CSitemap.Values(), "?instanceId=", Str(instanceId), "&appId=", Str(appId))
	End Function
	Public Shared Function [Values](ByVal search As String, instanceId As Integer, keyName As String) As String
		Return String.Concat(CSitemap.Values(), "?search=", Encode(search), "&instanceId=", Str(instanceId), "&keyName=", Encode(keyName))
	End Function
	Public Shared Function [Values](ByVal search As String, clientId As Integer, keyName As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Values(search, clientId, keyName), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function ValueAddEdit() As String
		Return "~/pages/values/value.aspx"
	End Function
	Public Shared Function ValueAdd(instanceId As Integer, keyName As String) As String 'May require a parentId (follow pattern below)
		Return String.Concat(ValueAddEdit, "?instanceId=", instanceId, "&keyName=", Encode(keyName))
	End Function
	Public Shared Function ValueEdit(ByVal valueId As Integer) As String
		Return String.Concat(ValueAddEdit, "?valueId=", valueId)
	End Function
	Public Shared Function ValueEdit(ByVal instanceId As Integer, keyName As String) As String
		Return String.Concat(ValueAddEdit, "?instanceId=", instanceId, "&keyName=", Encode(keyName))
	End Function
	Public Shared Function ValueEdit(ByVal i As CInstance, k As CKey) As String
		Return ValueEdit(i.InstanceId, k.KeyName)
	End Function

	'UserControls
	Public Shared Function UCValue() As String
		Return "~/pages//values/usercontrols/UCValue.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function ValueUploads() As String
		Return "~/uploads/values/"
	End Function
#End Region

#Region "Statuss"
	'List/Search
	Public Shared Function [Statuss]() As String
		Return "~/pages/statuss/default.aspx"
	End Function
	Public Shared Function [Statuss](ByVal search As String) As String
		Return String.Concat(CSitemap.Statuss(), "?search=", Encode(search))
	End Function
	Public Shared Function [Statuss](ByVal search As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Statuss(search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function StatusAddEdit() As String
		Return "~/pages/statuss/status.aspx"
	End Function
	Public Shared Function StatusAdd() As String 'May require a parentId (follow pattern below)
		Return StatusAddEdit()
	End Function
	Public Shared Function StatusEdit(ByVal statusId As Integer) As String
		Return String.Concat(StatusAddEdit, "?statusId=", statusId)
	End Function

	'UserControls
	Public Shared Function UCStatus() As String
		Return "~/pages//statuss/usercontrols/UCStatus.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function StatusUploads() As String
		Return "~/uploads/statuss/"
	End Function
#End Region

#Region "Clients"
	'List/Search
	Public Shared Function [Clients]() As String
		Return "~/pages/clients/default.aspx"
	End Function
	Public Shared Function [Clients](ByVal search As String, statusId As Integer) As String
		Return String.Concat(CSitemap.Clients(), "?search=", Encode(search), "&statusId=", Str(statusId))
	End Function
	Public Shared Function [Clients](ByVal search As String, statusId As Integer, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Clients(search, statusId), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function ClientAddEdit() As String
		Return "~/pages/clients/client.aspx"
	End Function
	Public Shared Function ClientAdd() As String 'May require a parentId (follow pattern below)
		Return ClientAddEdit()
	End Function
	Public Shared Function ClientEdit(ByVal clientId As Integer) As String
		Return String.Concat(ClientAddEdit, "?clientId=", Str(clientId))
	End Function

	'UserControls
	Public Shared Function UCClient() As String
		Return "~/pages/clients/usercontrols/UCClient.ascx"
	End Function
	Public Shared Function UCClientInstance() As String
		Return "~/pages/clients/usercontrols/UCClientInstance.ascx"
	End Function
	Public Shared Function UCDatabaseProfile() As String
		Return "~/pages/instances/usercontrols/UCDatabase.ascx"
	End Function
	Public Shared Function UCPublishProfile() As String
		Return "~/pages/instances/usercontrols/UCPublishProfile.ascx"
	End Function




	'Folders (relative to /web project)
	Public Shared Function ClientUploads() As String
		Return "~/uploads/clients/"
	End Function
#End Region

#Region "Groups"
	'List/Search
	Public Shared Function [Groups]() As String
		Return "~/pages/groups/default.aspx"
	End Function
	Public Shared Function [Groups](ByVal search As String) As String
		Return String.Concat(CSitemap.Groups(), "?search=", Encode(search))
	End Function
	Public Shared Function [Groups](ByVal appId As Integer) As String
		Return String.Concat(CSitemap.Groups(), "?appId=", Str(appId))
	End Function
	Public Shared Function [Groups](ByVal appId As Integer, groupId As Integer) As String
		Return String.Concat(CSitemap.Groups(appId), "&appId=", Str(groupId))
	End Function
	Public Shared Function [Groups](ByVal appId As Integer, ByVal search As String) As String
		Return String.Concat(CSitemap.Groups(), "?appId=", Str(appId), "&search=", Encode(search))
	End Function
	Public Shared Function [Groups](appId As Integer, ByVal search As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Groups(appId, search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function GroupAddEdit() As String
		Return "~/pages/groups/group.aspx"
	End Function
	Public Shared Function GroupAdd(appId As Integer) As String 'May require a parentId (follow pattern below)
		Return String.Concat(GroupAddEdit, "?appId=", appId)
	End Function
	Public Shared Function GroupEdit(ByVal groupId As Integer) As String
		Return String.Concat(GroupAddEdit, "?groupId=", groupId)
	End Function

	'UserControls
	Public Shared Function UCGroup() As String
		Return "~/pages//groups/usercontrols/UCGroup.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function GroupUploads() As String
		Return "~/uploads/groups/"
	End Function
#End Region

#Region "Keys"
	'List/Search
	Public Shared Function [Keys]() As String
		Return Keys(EApp.ControlTrack)
	End Function
	Public Shared Function [Keys](appId As EApp) As String
		Return Keys(CInt(appId))
	End Function
	Public Shared Function [Keys](appId As Integer) As String
		Return String.Concat("~/pages/keys/default.aspx?appId=", appId)
	End Function
	Public Shared Function [Keys](appId As Integer, groupId As Integer) As String
		Return String.Concat(CSitemap.Keys(appId), "&groupId=", Str(groupId))
	End Function
	Public Shared Function [Keys](appId As Integer, ByVal search As String, groupId As Integer, formatId As Integer) As String
		Return String.Concat(CSitemap.Keys(appId, groupId), "&search=", Encode(search), "&formatId=", Str(formatId))
	End Function
	Public Shared Function [Keys](appId As Integer, ByVal search As String, groupId As Integer, formatId As Integer, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Keys(appId, search, groupId, formatId), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function KeyAddEdit() As String
		Return "~/pages/keys/key.aspx"
	End Function
	Public Shared Function KeyAdd(appId As Integer, groupId As Integer) As String 'May require a parentId (follow pattern below)
		Return String.Concat(KeyAddEdit, "?appId=", Str(appId), "&groupId=", Str(groupId))
	End Function
	Public Shared Function KeyEdit(ByVal keyName As String) As String
		Return String.Concat(KeyAddEdit, "?keyName=", Encode(keyName))
	End Function
	Public Shared Function KeySetting(ByVal keyName As String) As String
		Return String.Concat("~/pages/keys/settings.aspx?keyName=", Encode(keyName))
	End Function

	'UserControls
	Public Shared Function UCKey() As String
		Return "~/pages//keys/usercontrols/UCKey.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function KeyUploads() As String
		Return "~/uploads/keys/"
	End Function
#End Region

#Region "Audit_Logs"
	'List/Search
	Public Shared Function [Audit_Logs]() As String
		Return "~/pages/audit_Logs/default.aspx"
	End Function
	Public Shared Function [Audit_Logs](ByVal search As String, typeId As Integer) As String
		Return String.Concat(CSitemap.Audit_Logs(), "?search=", Encode(search), "&typeId=", Str(typeId))
	End Function
	Public Shared Function [Audit_Logs](ByVal search As String, typeId As Integer, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Audit_Logs(search, typeId), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function Audit_LogAddEdit() As String
		Return "~/pages/audit_Logs/audit_Log.aspx"
	End Function
	Public Shared Function Audit_LogAdd() As String 'May require a parentId (follow pattern below)
		Return Audit_LogAddEdit()
	End Function
	Public Shared Function Audit_LogEdit(ByVal logId As Integer) As String
		Return String.Concat(Audit_LogAddEdit, "?logId=", logId)
	End Function

	'UserControls
	Public Shared Function UCAudit_Log() As String
		Return "~/pages//audit_Logs/usercontrols/UCAudit_Log.ascx"
	End Function

	'Folders (relative to /web project)
	Public Shared Function Audit_LogUploads() As String
		Return "~/uploads/audit_Logs/"
	End Function
#End Region

#Region "Clicks"
	'List/Search
	Public Shared Function [Clicks]() As String
		Return "~/pages/clicks/default.aspx"
	End Function
	Public Shared Function [Clicks](ByVal userName As String) As String
		Return String.Concat(CSitemap.Clicks(), "?userName=", Encode(userName))
	End Function
	Public Shared Function [Clicks](ByVal userName As String, ByVal url As String) As String
		Return String.Concat(CSitemap.Clicks(userName), "&url=", Encode(url))
	End Function
	Public Shared Function [Clicks](ByVal search As String, ByVal url As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Clicks(search, url), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'UserControls
	Public Shared Function UCClick() As String
		Return "~/pages//clicks/usercontrols/UCClick.ascx"
	End Function
#End Region

#Region "Sessions"
	'List/Search
	Public Shared Function [Sessions]() As String
		Return "~/pages/sessions/default.aspx"
	End Function
	Public Shared Function [Sessions](ByVal search As String) As String
		Return String.Concat(CSitemap.Sessions(), "?search=", Encode(search))
	End Function
	Public Shared Function [Sessions](ByVal search As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Sessions(search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Add/Edit
	Private Shared Function SessionAddEdit() As String
		Return "~/pages/sessions/session.aspx"
	End Function
	Public Shared Function SessionAdd() As String 'May require a parentId (follow pattern below)
		Return SessionAddEdit()
	End Function
	Public Shared Function SessionEdit(ByVal sessionId As Integer) As String
		Return String.Concat(SessionAddEdit, "?sessionId=", sessionId)
	End Function

	'UserControls
	Public Shared Function UCSession() As String
		Return "~/pages//sessions/usercontrols/UCSession.ascx"
	End Function
#End Region

#Region "Login"
	Public Shared Function Login() As String
		Return "~/login.aspx"
	End Function
#End Region

#Region "Audit_Errors"
	'List/Search
	Public Shared Function [Audit_Errors]() As String
		Return "~/pages/audit_Errors/default.aspx"
	End Function
	Public Shared Function [Audit_Errors](ByVal search As String, ByVal uniqueOnly As Boolean) As String
		Return String.Concat(CSitemap.Audit_Errors(), "?search=", Encode(search), "&uniqueOnly=", uniqueOnly)
	End Function
	Public Shared Function [Audit_Errors](ByVal search As String, ByVal uniqueOnly As Boolean, ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) As String
		Return String.Concat(CSitemap.Audit_Errors(search, uniqueOnly), "&sortBy=", sortBy, "&desc=", descending, "&p=", pageNumber)
	End Function

	'USercontrols
	Public Shared Function UCError() As String
		Return "~/pages/audit_Errors/usercontrols/UCAudit_Error.ascx"
	End Function

	'View Details
	Public Shared Function [Audit_Error](ByVal errorID As Integer) As String
		Return String.Concat("~/pages/audit_Errors/audit_Error.aspx?errorID=", errorID)
	End Function
	Public Shared Function Audit_Error(ByVal type1 As Integer, ByVal message1 As Integer, ByVal type2 As Integer, ByVal message2 As Integer) As String
		Return String.Concat("~/pages/audit_Errors/audit_Error.aspx?type1=", type1, "&message1=", message1, "&type2=", type2, "&message2=", message2)
	End Function
	Public Shared Function Audit_ErrorGroup(ByVal type1 As Integer, ByVal message1 As Integer, ByVal type2 As Integer, ByVal message2 As Integer) As String
		Return String.Concat("~/pages/audit_Errors/group.aspx?type1=", type1, "&message1=", message1, "&type2=", type2, "&message2=", message2)
	End Function
	Public Shared Function Audit_ErrorGroup(ByVal type1 As Integer, ByVal message1 As Integer, ByVal type2 As Integer, ByVal message2 As Integer, ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) As String
		Return String.Concat(CSitemap.Audit_ErrorGroup(type1, message1, type2, message2), "&sortBy=", sortBy, "&desc=", descending, "&p=", pageNumber)
	End Function
#End Region

#Region "Roles"
	'List/Search
	Public Shared Function [Roles]() As String
		Return "~/pages/roles/default.aspx"
	End Function
	Public Shared Function [Roles](ByVal pageIndex As Integer) As String
		Return String.Concat(CSitemap.Roles, "?p=", pageIndex)
	End Function

	'Add/Edit
	Private Shared Function RoleAddEdit() As String
		Return "~/pages/roles/role.aspx"
	End Function
	Public Shared Function RoleAdd() As String 'May require a parentId (follow pattern below)
		Return RoleAddEdit()
	End Function
	Public Shared Function RoleEdit(ByVal roleName As String) As String
		Return String.Concat(RoleAddEdit, "?roleName=", roleName)
	End Function
	Public Shared Function RoleEdit(ByVal roleName As String, ByVal search As String) As String
		Return String.Concat(RoleEdit(roleName), "&search=", Encode(search))
	End Function

	'Usercontrols
	Public Shared Function UCRole() As String
		Return "~/pages/roles/usercontrols/UCRole.ascx"
	End Function


	'Folders (relative to /web project)
	Public Shared Function RoleUploads() As String
		Return "/uploads/roles/"
	End Function
#End Region

#Region "Users"
	'List/Search
	Public Shared Function [Users]() As String
		Return "~/pages/users/default.aspx"
	End Function
	Public Shared Function [Users](ByVal search As String) As String
		Return String.Concat(CSitemap.Users, "?search=", Encode(search))
	End Function
	Public Shared Function [Users](ByVal search As String, ByVal pi As CPagingInfo) As String
		Return String.Concat(CSitemap.Users(search), "&sortBy=", pi.SortByColumn, "&desc=", pi.Descending, "&p=", pi.PageIndex + 1)
	End Function

	'Usercontrols
	Public Shared Function UCUser() As String
		Return "~/pages/users/usercontrols/UCUser.ascx"
	End Function

	'Add/Edit
	Private Shared Function UserAddEdit() As String
		Return "~/pages/users/user.aspx"
	End Function
	Public Shared Function UserAdd() As String 'May require a parentId (follow pattern below)
		Return UserAddEdit()
	End Function
	Public Shared Function UserEdit(ByVal userLoginName As String) As String
		Return String.Concat(UserAddEdit, "?userLoginName=", userLoginName)
	End Function
	Public Shared Function UserEdit(ByVal roleName As String, ByVal search As String) As String
		Return String.Concat(UserEdit(roleName), "&search=", Encode(search))
	End Function

	'Folders (relative to /web project)
	Public Shared Function UserUploads() As String
		Return "/uploads/users/"
	End Function
#End Region

#Region "UserRoles"
	'List/Search
	'Public Shared Function UserRolesForUser(ByVal userLoginName As String) As String
	'    Return String.Concat("~/pages/userRoles/ForUser.aspx?userLoginName=", userLoginName)
	'End Function
	'Public Shared Function UserRolesForRole(ByVal roleName As String) As String
	'    Return String.Concat("~/pages/userRoles/ForRole.aspx?roleName=", roleName)
	'End Function
	'Public Shared Function UserRolesForUser(ByVal userLoginName As String, ByVal search As String) As String
	'    Return String.Concat(UserRolesForUser(userLoginName), "&search=", Encode(search))
	'End Function
	'Public Shared Function UserRolesForRole(ByVal roleName As String, ByVal search As String) As String
	'    Return String.Concat(UserRolesForRole(roleName), "&search=", Encode(search))
	'End Function

	'Add/Edit
	Private Shared Function UserRoleAddEdit() As String
		Return "~/pages/userRoles/userRole.aspx"
	End Function
	Public Shared Function UserRoleAdd() As String 'May require a parentId (follow pattern below)
		Return UserRoleAddEdit()
	End Function
	Public Shared Function UserRoleEdit(ByVal uRUserLogin As String, ByVal uRRoleName As String) As String
		Return String.Concat(UserRoleAddEdit, "?uRUserLogin=", uRUserLogin)
	End Function

	'UserControls
	Public Shared Function UCUserRole() As String
		Return "~/pages/userRoles/usercontrols/UCUserRole.ascx"
	End Function
#End Region


#Region "Audit Trail"
	Public Shared Function AuditTrail() As String
		Return "~/pages/audit-trail/"
	End Function
#End Region

#Region "Standard"
	'Defaults
	Public Shared Function DefaultUploadsPath() As String
		Return "~/uploads/"
	End Function

	'Utilities
	Public Shared Function Encode(ByVal s As String) As String
		If String.IsNullOrEmpty(s) Then Return String.Empty
		Return Current.Server.UrlEncode(s)
	End Function
	Public Shared Function Str(ByVal i As Integer) As String
		If Integer.MinValue = i Then Return String.Empty
		Return i.ToString
	End Function
	Public Shared Function Str(ByVal i As Guid) As String
		If Guid.Empty.Equals(i) Then Return String.Empty
		Return i.ToString
	End Function
	Public Shared Function Str(ByVal i As DateTime) As String
		If DateTime.MinValue.Equals(i) Then Return String.Empty
		Return Encode(i.ToString)
	End Function
	Public Shared Function Str(ByVal i As Boolean?) As String
		If Not i.HasValue Then Return String.Empty
		Return i.Value
	End Function

	'Invalid ids
	Public Shared Sub RecordNotFound(ByVal entity As String)
		With HttpContext.Current.Request.QueryString
			If .Count > 0 Then RecordNotFound(entity, .Item(0)) Else RecordNotFound(entity, String.Empty)
		End With
	End Sub
	Public Shared Sub RecordNotFound(ByVal entity As String, ByVal value As Integer)
		RecordNotFound(entity, value.ToString)
	End Sub
	Public Shared Sub RecordNotFound(ByVal entity As String, ByVal pk1 As Object, ByVal pk2 As Object)
		RecordNotFound(entity, String.Concat(pk1, "/", pk2))
	End Sub
	Public Shared Sub RecordNotFound(ByVal entity As String, ByVal value As String)
		If String.IsNullOrEmpty(value) Then
			Throw New Exception(String.Concat("Invalid ", entity, "Id"))
		Else
			Throw New Exception(String.Concat("Invalid ", entity, "Id: ", value))
		End If
	End Sub
#End Region

End Class
