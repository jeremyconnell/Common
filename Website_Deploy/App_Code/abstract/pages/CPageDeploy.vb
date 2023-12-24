Imports Microsoft.VisualBasic
Imports SchemaDeploy

Public Class CPageDeploy : Inherits CPageWithTableHelpers

#Region "App-Menu"
	'Public - App
	Public Sub MenuAppSearch()
		MenuApp(EGeneric.Search)
	End Sub
	Public Sub MenuAppNew()
		MenuApp(EGeneric.AddNew)
	End Sub
	Public Sub MenuAppDetails(appId As Integer)
		If appId <= 0 Then
			MenuAppNew()
		Else
			MenuApp(EGeneric.Details, CSitemap.AppEdit(appId))
			MenuAppChildren(appId)
		End If
	End Sub

	'Public - Other
	'Public Sub MenuAppDeploys(appId As Integer)
	'	MenuAppParents(appId)
	'	MenuAppChildren(appId, EAppMenu.Deploys)
	'End Sub
	'Public Sub MenuAppVersions(appId As Integer)
	'	MenuAppParents(appId)
	'	MenuAppChildren(appId, EAppMenu.Versions)
	'End Sub
	Public Sub MenuAppSchema(appId As Integer)
		MenuSelected = "Deploys"
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.Schema)
	End Sub
	Public Sub MenuAppData(appId As Integer)
		MenuSelected = "Deploys"
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.Data)
	End Sub
	Public Sub MenuAppBackups(appId As Integer)
		MenuSelected = "Backups"

		AddMenuSide("&laquo; " & CApp.Cache.GetById(appId).AppName, CSitemap.AppEdit(appId))

		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.Backups)
	End Sub
	Public Sub MenuAppBackup(appId As Integer, b As CBackup)
		MenuSelected = "Backups"

		AddMenuSide("&laquo; " & CApp.Cache.GetById(appId).AppName, CSitemap.AppEdit(appId))


		AddMenuSide(CUtilities.NameAndCount("Backups", b.SelectCount()), CSitemap.Backups(appId))
		AddMenuSide("Backup", CSitemap.Backup(b.BackupId), True)
		AddMenuSide(CUtilities.NameAndCount("Datasets", b.BackupItemsCount()), CSitemap.BackupItems(appId))
	End Sub
	Public Sub MenuAppDatasets(appId As Integer)
		MenuSelected = "Datasets"

		AddMenuSide("&laquo; " & CApp.Cache.GetById(appId).AppName, CSitemap.AppEdit(appId))

		AddMenuSide(CUtilities.NameAndCount("Backups", New CBackup().SelectCount()), CSitemap.Backups(appId))
		AddMenuSide(CUtilities.NameAndCount("Datasets", New CBackupItem().SelectCount()), CSitemap.BackupItems(appId), True)
	End Sub
	Public Sub MenuAppDataset(appId As Integer, bi As CBackupItem)
		MenuSelected = "Datasets"

		AddMenuSide("&laquo; " & CApp.Cache.GetById(appId).AppName, CSitemap.AppEdit(appId))

		AddMenuSide(CUtilities.NameAndCount("Backups", New CBackup().SelectCount()), CSitemap.Backups(appId))
		AddMenuSide("Backup", CSitemap.Backup(bi.ItemBackupId))
		AddMenuSide(CUtilities.NameAndCount("Datasets", bi.Backup.BackupItems), CSitemap.BackupItems(appId))
		AddMenuSide("Dataset", CSitemap.Backup(bi.ItemBackupId), True, bi.ItemTableName)

	End Sub
	Public Sub MenuAppFiles(appId As Integer)
		MenuSelected = "Files"
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.Files)
	End Sub
	Public Sub MenuAppReleases(appId As Integer)
		MenuSelected = "Releases"
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.Releases)
	End Sub
	Public Sub MenuAppReports(appId As Integer)
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.Reports)
	End Sub
	Public Sub MenuAppAutoUpgrades(appId As Integer)
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.AutoUpgrades)
	End Sub
	Public Sub MenuAppPushUpgrades(appId As Integer)
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.PushUpgrades)
	End Sub

	'Private
	Private Sub MenuAppParents(appId As Integer)
		AddMenuSide("&laquo; " & CUtilities.NameAndCount("Apps", CApp.Cache), CSitemap.Apps())
		AddMenuSide("&laquo; " & CApp.Cache.GetById(appId).AppName, CSitemap.AppEdit(appId))
	End Sub
	Private Sub MenuAppParent(appId As Integer)
		AddMenuSide("&laquo; " & CApp.Cache.GetById(appId).AppName, CSitemap.AppEdit(appId))
	End Sub
	Private Sub MenuApp(generic As EGeneric, Optional urlEdit As String = Nothing)
		MenuGeneric("App", "Apps", CApp.Cache.Count, CSitemap.Apps, CSitemap.AppAdd, generic, urlEdit)
	End Sub
	Private Sub MenuAppChildren(appId As Integer, Optional selected As EAppMenu = EAppMenu.Details)
		Dim a As CApp = CApp.Cache.GetById(appId)

		If a.Instances.Count > 0 OrElse selected = EAppMenu.Deploys Then AddMenuSide(CUtilities.NameAndCount("1. Deploys", a.Instances), CSitemap.Instances(appId), selected = EAppMenu.Deploys)
		If a.Instances.Count > 0 OrElse selected = EAppMenu.Schema Then AddMenuSide(CUtilities.NameAndCount("2. Schema", a.Instances), CSitemap.AppSchema(appId), selected = EAppMenu.Schema)
		If a.Instances.Count > 0 OrElse selected = EAppMenu.Data Then AddMenuSide(CUtilities.NameAndCount("3. Data", a.Instances), CSitemap.AppData(appId), selected = EAppMenu.Data)
		If a.Instances.Count > 0 OrElse selected = EAppMenu.Backups Then AddMenuSide(CUtilities.NameAndCount("4. Backups", New CBackup().SelectCount()), CSitemap.Backups(appId), selected = EAppMenu.Backups)
		If EAppMenu.Datasets Then AddMenuSide(CUtilities.NameAndCount("Datasets", New CBackupItem().SelectCount()), CSitemap.BackupItems(appId), selected = EAppMenu.Datasets)
		If a.Versions.Count > 0 OrElse selected = EAppMenu.Versions Then AddMenuSide(CUtilities.NameAndCount("5. Versions", a.Versions), CSitemap.Versions(appId), selected = EAppMenu.Versions)
		If a.BinaryFiles.Count > 0 OrElse selected = EAppMenu.Files Then AddMenuSide(CUtilities.NameAndCount("6. Files", a.BinaryFiles), CSitemap.BinaryFiles(appId), selected = EAppMenu.Files)
		If a.Releases.Count > 0 OrElse selected = EAppMenu.Releases Then AddMenuSide(CUtilities.NameAndCount("7. Releases", a.Releases), CSitemap.Releases(appId), selected = EAppMenu.Releases)
		If a.ReportsCount_ > 0 OrElse selected = EAppMenu.Reports Then AddMenuSide(CUtilities.NameAndCount("8. Reports", a.ReportsCount_), CSitemap.ReportHistorys(appId), selected = EAppMenu.Reports)
		If a.UpgradesCount_ > 0 OrElse selected = EAppMenu.AutoUpgrades Then AddMenuSide(CUtilities.NameAndCount("9. Upgrades", a.UpgradesCount_), CSitemap.UpgradeHistorys(appId), selected = EAppMenu.AutoUpgrades)
		If a.PushedCount_ > 0 OrElse selected = EAppMenu.PushUpgrades Then AddMenuSide(CUtilities.NameAndCount("9b. Upgrades", a.PushedCount_), CSitemap.PushedUpgrades(appId), selected = EAppMenu.PushUpgrades)
		If a.Groups.Count > 0 OrElse selected = EAppMenu.Groups Then AddMenuSide(CUtilities.NameAndCount("10. Config", a.Groups), CSitemap.Groups(appId), selected = EAppMenu.Groups)

		If selected = EAppMenu.Deploys Then AddMenuSide(NewText("Deploy"), CSitemap.InstanceAdd(appId))
		If selected = EAppMenu.Groups Then AddMenuSide(NewText("Group"), CSitemap.GroupAdd(appId))
		If selected = EAppMenu.Releases Then AddMenuSide(NewText("Release"), CSitemap.AppEdit(appId))
		If selected = EAppMenu.Versions Then AddMenuSide(NewText("Version"), CSitemap.VersionAdd(appId))
		If selected = EAppMenu.Schema AndAlso CConfig.IsDev Then AddMenuSide("&laquo; My Schema", CSitemap.SelfSchemaSync(appId))
		If selected = EAppMenu.Data AndAlso CConfig.IsDev Then AddMenuSide("&laquo; My Data", CSitemap.SelfDataSync(appId))

	End Sub
	Public Enum EAppMenu
		Details
		Deploys
		Versions
		Files
		Schema
		Data
		Backups
		Datasets
		Reports
		Releases
		AutoUpgrades
		PushUpgrades
		BulkSql
		AzureDb
		AzureWeb
		Groups
		Backup
		Dataset
	End Enum
#End Region

#Region "Instance Menu"
	'Public - instance
	Public Sub MenuInstanceSearch(appId As Integer)
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.Deploys)
	End Sub
	Public Sub MenuInstanceNew(appId As Integer)
		MenuInstance(appId, EGeneric.AddNew)
	End Sub
	Public Sub MenuInstanceDetails(appId As Integer, instanceId As Integer, hasClient As Boolean)
		If instanceId <= 0 Then
			MenuInstanceNew(appId)
		Else
			MenuInstance(appId, EGeneric.Details, CSitemap.Instance(instanceId))
			If hasClient Then MenuInstanceChildren(instanceId, EDeployMenu.Details)
		End If
	End Sub

	'Public - other
	Public Sub MenuInstanceSettings(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Settings)
	End Sub
	Public Sub MenuInstanceReconcile(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Reconcile)
	End Sub
	Public Sub MenuInstanceMonitor(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Monitor)
	End Sub
	Public Sub MenuInstanceSql(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Sql)
	End Sub
	Public Sub MenuInstanceAppLogin(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.AppLogin)
	End Sub
	Public Sub MenuInstanceData(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Data)
	End Sub
	Public Sub MenuInstanceDataImport(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.DataImport)
	End Sub
	Public Sub MenuInstanceDatabase(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Database)
	End Sub
	Public Sub MenuInstanceWebsite(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Website)
	End Sub
	Public Sub MenuInstanceFtp(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Ftp)
	End Sub
	Public Sub MenuInstanceVersion(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Version)
	End Sub
	Public Sub MenuInstanceAudit(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Audit)
	End Sub
	Public Sub MenuInstanceSchema(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Schema)
	End Sub
	Public Sub MenuInstanceErrors(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.ErrorLog)
	End Sub
	Public Sub MenuInstanceFeatures(appId As Integer, instanceId As Integer)
		MenuInstance(appId, EGeneric.Other, CSitemap.Instance(instanceId))
		MenuInstanceChildren(instanceId, EDeployMenu.Features)
	End Sub


	'Private
	Private Sub MenuInstance(appId As Integer, generic As EGeneric, Optional urlEdit As String = Nothing)
		MenuSelected = "Deploys"
		MenuAppParent(appId)
		MenuGeneric("Deploy", "1. Deploys", CInstance.Cache.GetByAppId(appId).Count, CSitemap.Instances(appId), CSitemap.InstanceAdd(appId), generic, urlEdit)
	End Sub
	Private Sub MenuInstanceChildren(instanceId As Integer, Optional selected As EDeployMenu = EAppMenu.Details)

		Dim d As CInstance = CInstance.Cache.GetById(instanceId)
		Dim appId As Integer = d.InstanceAppId

		AddMenuSide("1. Database", CSitemap.InstanceDatabase(instanceId), selected = EDeployMenu.Database)
		AddMenuSide("2. Website", CSitemap.InstanceWebsite(instanceId), selected = EDeployMenu.Website)
		AddMenuSide("3. Ftp Init", CSitemap.InstanceFtp(instanceId), selected = EDeployMenu.Ftp)
		AddMenuSide("4. Version", CSitemap.InstanceVersion(instanceId), selected = EDeployMenu.Version)
		AddMenuSide("5. AppLogin", CSitemap.InstanceAppLogin(instanceId), selected = EDeployMenu.AppLogin)
		Dim sett As String = CUtilities.NameAndCount("6a. Settings", d.Values)
		AddMenuSide(sett, CSitemap.InstanceSettings(instanceId), selected = EDeployMenu.Settings)
		AddMenuSide("6b. Reconcile", CSitemap.InstanceReconcile(instanceId), selected = EDeployMenu.Reconcile)
		AddMenuSide("7. Monitor", CSitemap.InstanceMonitor(instanceId), selected = EDeployMenu.Monitor)
		AddMenuSide("8. Errors", CSitemap.InstanceErrorLog(instanceId), selected = EDeployMenu.ErrorLog)
		AddMenuSide("9a. Schema", CSitemap.InstanceSchema(instanceId), selected = EDeployMenu.Schema)
		AddMenuSide("9b. Data", CSitemap.InstanceData(instanceId), selected = EDeployMenu.Data)
		AddMenuSide("9c. Sql", CSitemap.InstanceSql(instanceId), selected = EDeployMenu.Sql)
		AddMenuSide("9d. Import", CSitemap.InstanceDataImport(instanceId), selected = EDeployMenu.DataImport)
		AddMenuSide("10. Audit", CSitemap.InstanceAuditTrail(instanceId), selected = EDeployMenu.Audit)
		AddMenuSide("11. Feature", CSitemap.InstanceFeatures(instanceId), selected = EDeployMenu.Features)

		'AddLinkSide("Schema", CSitemap.AppSchema(appId))

	End Sub
	Public Enum EDeployMenu
		Details
		Database
		Website
		Ftp
		Version
		AppLogin
		Settings
		Reconcile
		Monitor
		ErrorLog
		Schema
		Data
		DataImport
		Sql
		Audit
		Features
	End Enum
#End Region

#Region "Versions Menu"
	'Public - instance
	Public Sub MenuVersionSearch(appId As Integer)
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.Versions)
	End Sub
	Public Sub MenuVersionNew(appId As Integer)
		MenuVersion(appId, EGeneric.AddNew)
	End Sub
	Public Sub MenuVersionDetails(appId As Integer, versionId As Integer)
		If versionId <= 0 Then
			MenuVersionNew(appId)
		Else
			MenuVersion(appId, EGeneric.Details, CSitemap.VersionEdit(versionId))
			MenuVersionChildren(versionId, False)
		End If
	End Sub

	Public Sub MenuVersionFiles(appId As Integer, versionId As Integer)
		MenuVersion(appId, EGeneric.Other, CSitemap.VersionEdit(versionId))
		MenuVersionChildren(versionId, True)
	End Sub

	'Private
	Private Sub MenuVersion(appId As Integer, generic As EGeneric, Optional urlEdit As String = Nothing)
		MenuSelected = "Versions"
		MenuAppParent(appId)
		MenuGeneric("Version", "4. Versions", CVersion.Cache.GetByAppId(appId).Count, CSitemap.Versions(appId), CSitemap.VersionAdd(appId), generic, urlEdit)
	End Sub

	Private Sub MenuVersionChildren(versionId As Integer, selected As Boolean)
		Dim v As CVersion = CVersion.Cache.GetById(versionId)
		Dim appId As Integer = v.VersionAppId

		AddMenuSide(CUtilities.NameAndCount("Files", v.VersionFiles), CSitemap.BinaryFiles(appId, versionId), selected)
		If v.VersionSchemaMD5 <> Guid.Empty Then
			AddMenuSide("Schema", CSitemap.Schema(versionId))
		End If

	End Sub
#End Region

#Region "Groups Menu"
	'Public - instance
	Public Sub MenuGroupSearch(appId As Integer)
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.Groups)
	End Sub
	Public Sub MenuGroupNew(appId As Integer)
		MenuGroup(appId, EGeneric.AddNew)
	End Sub
	Public Sub MenuGroupDetails(appId As Integer, groupId As Integer)
		If groupId <= 0 Then
			MenuGroupNew(appId)
		Else
			MenuGroup(appId, EGeneric.Details, CSitemap.GroupEdit(groupId))
			'MenuVersionChildren(instanceId, EDeployMenu.Details)
		End If
	End Sub

	'Private (TODO: Children)
	Private Sub MenuGroup(appId As Integer, generic As EGeneric, Optional urlEdit As String = Nothing)
		MenuSelected = "Groups"
		MenuAppParent(appId)
		MenuGeneric("Group", "3. Groups", CGroup.Cache.GetByAppId(appId).Count, CSitemap.Groups(appId), CSitemap.GroupAdd(appId), generic, urlEdit)
	End Sub
#End Region

#Region "Releases Menu"
	'Public - instance
	Public Sub MenuReleaseSearch(appId As Integer)
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.Releases)
	End Sub
	Public Sub MenuReleaseNew(appId As Integer)
		MenuRelease(appId, EGeneric.AddNew)
	End Sub
	Public Sub MenuReleaseDetails(appId As Integer, releaseId As Integer)
		If releaseId <= 0 Then
			MenuReleaseNew(appId)
		Else
			MenuRelease(appId, EGeneric.Details, CSitemap.ReleaseEdit(releaseId))
			'MenuReleaseChildren(instanceId, EDeployMenu.Details)
		End If
	End Sub

	'Private
	Private Sub MenuRelease(appId As Integer, generic As EGeneric, Optional urlEdit As String = Nothing)
		MenuSelected = "Releases"
		MenuAppParent(appId)
		MenuGeneric("Release", "6. Releases", CRelease.Cache.GetByAppId(appId).Count, CSitemap.Releases(appId), CSitemap.AppEdit(appId), generic, urlEdit)
	End Sub
#End Region

#Region "Reports Menu"
	'Public - instance
	Public Sub MenuReportSearch(appId As Integer)
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.Reports)
	End Sub
	Public Sub MenuReportNew(appId As Integer)
		MenuReport(appId, EGeneric.AddNew)
	End Sub
	Public Sub MenuReportDetails(appId As Integer, ReportId As Integer)
		If ReportId <= 0 Then
			MenuReportNew(appId)
		Else
			MenuReport(appId, EGeneric.Details, CSitemap.ReportHistoryEdit(ReportId))
			'MenuReportChildren(instanceId, EDeployMenu.Details)
		End If
	End Sub

	'Private
	Private Sub MenuReport(appId As Integer, generic As EGeneric, Optional urlEdit As String = Nothing)
		MenuSelected = "Reports"
		MenuAppParent(appId)
		MenuGeneric("Report", "4. Reports", CApp.Cache.GetById(appId).ReportsCount_, CSitemap.ReportHistorys(appId), Nothing, generic, urlEdit)
	End Sub
#End Region

#Region "Reports Menu"
	'Public - instance
	Public Sub MenuAutoUpgradeSearch(appId As Integer)
		MenuAppParents(appId)
		MenuAppChildren(appId, EAppMenu.AutoUpgrades)
	End Sub
	Public Sub MenuAutoUpgradeNew(appId As Integer)
		MenuAutoUpgrade(appId, EGeneric.AddNew)
	End Sub
	Public Sub MenuAutoUpgradeDetails(appId As Integer, ReportId As Integer)
		If ReportId <= 0 Then
			MenuAutoUpgradeNew(appId)
		Else
			MenuAutoUpgrade(appId, EGeneric.Details, CSitemap.UpgradeHistory(ReportId))
			'MenuReportChildren(instanceId, EDeployMenu.Details)
		End If
	End Sub

	'Private
	Private Sub MenuAutoUpgrade(appId As Integer, generic As EGeneric, Optional urlEdit As String = Nothing)
		MenuSelected = "Pull"
		MenuAppParent(appId)
		MenuGeneric("AutoUpgrade", "8. AutoUpgrade", CApp.Cache.GetById(appId).ReportsCount_, CSitemap.UpgradeHistorys(appId), Nothing, generic, urlEdit)
	End Sub
#End Region




#Region "Menu Logic - Generic"
	Private Enum EGeneric
		Search
		Details
		AddNew
		Other
	End Enum
	Private Sub MenuGeneric(entityName As String, plural As String, count As Integer, urlSearch As String, urlAdd As String, selected As EGeneric, Optional urlEdit As String = Nothing)
		AddMenuSide(CUtilities.NameAndCount(plural, count), urlSearch, selected = EGeneric.Search)
		If Not IsNothing(urlAdd) Then
			AddMenuSide(NewText(entityName), urlAdd, selected = EGeneric.AddNew)
		End If
		If Not IsNothing(urlEdit) Then
			AddMenuSide(DetailsText(entityName), urlEdit, selected = EGeneric.Details)
		End If
	End Sub
	Private Function NewText(entityName As String) As String
		Return String.Concat("+New ", entityName, "...")
	End Function
	Private Function DetailsText(entityName As String) As String
		Return String.Concat(entityName, " Details")
	End Function
#End Region

End Class
