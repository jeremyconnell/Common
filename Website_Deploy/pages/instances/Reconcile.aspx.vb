
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports SchemaAdmin.api
Imports SchemaAdmin.dto

Imports SchemaDeploy

Partial Class pages_clients_Reconcile
	Inherits CPageDeploy

#Region "Querystring"
	Public ReadOnly Property InstanceId As Integer
        Get
            Return CWeb.RequestInt("instanceId")
        End Get
    End Property
#End Region

#Region "Data"
    Public ReadOnly Property AppId As Integer
        Get
            Return App.AppId
        End Get
    End Property
    Public ReadOnly Property App As CApp
        Get
            Return Instance.App
        End Get
    End Property
	Public ReadOnly Property Client As CClient
		Get
			Return CClient.Cache.GetById(Instance.InstanceClientId)
		End Get
	End Property
	Public ReadOnly Property Instance As CInstance
        Get
            Return CInstance.Cache.GetById(InstanceId)
        End Get
    End Property

    'Session
    Public Property InstanceData As CConfigSettingList
        Get
            Dim key As String = "InstanceData_" & InstanceId
            Dim list As CConfigSettingList = CSession.Get(key)
            If IsNothing(list) Then
				Try
					Dim api As CControlTrackApi = Client.Api(Instance)
					Dim all As CConfigSettingList = api.ConfigSettingsAll()
					CSession.Set(key, all)
					Return all
				Catch ex As Exception
					CSession.PageMessageEx = ex
					Return Nothing
				End Try
            End If
            Return list
        End Get
        Set(value As CConfigSettingList)
            Dim key As String = "InstanceData_" & InstanceId
            CSession.Set(key, value)
        End Set
    End Property
#End Region

#Region "Page Events"
    Protected Overrides Sub PageInit()
		If IsNothing(Client) Then
			Response.Redirect(CSitemap.Clients)
			Exit Sub
		End If

		UnbindSideMenu()
		MenuInstanceReconcile(AppId, InstanceId)


		ddApp.DataSource = CApp.Cache
		ddApp.DataBind()
		CDropdown.SetValue(ddApp, AppId)


		ddInstance.DataSource = App.Instances
		ddInstance.DataBind()
        CDropdown.BlankItem(ddInstance, "-- Select Instance --")
        CDropdown.SetValue(ddInstance, InstanceId)

        If Not IsNothing(InstanceData) Then
            Dim diff As New CDiff(InstanceData, Instance.Values)
            Display(diff)
        End If
    End Sub
#End Region


#Region "Form Events"
	Sub ddApp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddApp.SelectedIndexChanged
		Response.Redirect(CSitemap.Instances(CDropdown.GetInt(ddApp)))
	End Sub
	Sub ddInstance_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddInstance.SelectedIndexChanged
        Response.Redirect(CSitemap.InstanceReconcile(CDropdown.GetInt(ddInstance)))
    End Sub

    Private Sub btnRefreshCache_Click(sender As Object, e As EventArgs) Handles btnRefreshCache.Click

		Try
			InstanceData = Me.Client.RefreshCache(Me.Instance)
		Catch ex As Exception
			CSession.PageMessageEx = ex
			Exit Sub
        End Try

        Response.Redirect(CSitemap.InstanceReconcile(InstanceId))
    End Sub


    Private Sub btnFileToDb_Click(sender As Object, e As EventArgs) Handles btnFileToDb.Click
		Try
			InstanceData = Me.Client.ImportConfig(Me.Instance, True)
		Catch ex As Exception
			CSession.PageMessageEx = ex
			Exit Sub
        End Try

        Response.Redirect(CSitemap.InstanceReconcile(InstanceId))
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
		Try
			InstanceData = Me.Client.ImportConfig(Me.Instance)
		Catch ex As Exception
			CSession.PageMessageEx = ex
			Exit Sub
        End Try

        Response.Redirect(CSitemap.InstanceReconcile(InstanceId))
    End Sub


    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
		Try
			Me.Client.ExportConfig(Me.Instance)
			InstanceData = Nothing
		Catch ex As Exception
			CSession.PageMessageEx = ex
			Exit Sub
        End Try

        Response.Redirect(CSitemap.InstanceReconcile(InstanceId))
    End Sub
    Private Sub btnImportJson_Click(sender As Object, e As EventArgs) Handles btnImportJson.Click
        Dim txt As String = txtJson.Text
        If txt.Length = 0 Then Exit Sub

		Try
			Dim j As JObject = JsonConvert.DeserializeObject(txt)
			Recurse(j)
		Catch ex As Exception
			CSession.PageMessageEx = ex
			Exit Sub
        End Try

        Response.Redirect(CSitemap.InstanceReconcile(InstanceId))
    End Sub
    Private Sub Recurse(j As JObject, Optional prefix As String = "")
        Dim g As CGroup = CGroup.Cache.GetOrCreate("Json")

        For Each i As JToken In j.Children
            If i.Type <> JTokenType.Property Then Continue For

            Dim p As JProperty = i
            Dim obj As JToken = p.Value
            If p.Name = "report" Then
                Continue For
            End If

            If obj.Type = JTokenType.Boolean Then
                Dim f As CFormat = CFormat.Cache.GetById(EFormat.Boolean)
                Dim k As CKey = CKey.Cache.GetOrCreate(prefix & p.Name, g, f)

                Dim b As Boolean? = obj.ToObject(Of Boolean?)

				CValue.Cache.GetOrCreate(Instance, k, b, Nothing, Nothing, True)
			ElseIf obj.Type = JTokenType.String Then
                Dim f As CFormat = CFormat.Cache.GetById(EFormat.String)
                Dim k As CKey = CKey.Cache.GetOrCreate(prefix & p.Name, g, f)

                Dim s As String = obj.ToObject(Of String)

				CValue.Cache.GetOrCreate(Instance, k, Nothing, Nothing, s, True)
			ElseIf obj.Type = JTokenType.Integer Then
                Dim f As CFormat = CFormat.Cache.GetById(EFormat.Integer)
                Dim k As CKey = CKey.Cache.GetOrCreate(prefix & p.Name, g, f)

                Dim int As Integer? = obj.ToObject(Of Integer?)

				CValue.Cache.GetOrCreate(Instance, k, Nothing, int, Nothing, True)
			ElseIf obj.Type = JTokenType.Object Then
                Recurse(p.Value, p.Name & ".")
            End If
        Next
    End Sub
#End Region


#Region "Display"
    Private Sub Display(diff As CDiff)
        Display(diff.OnlyOnClient)
        Display(diff.OnlyOnServer)
        Display_GroupIsDiff(diff.ExistsOnBoth.GroupIsDiff)
        Display_ValueIsDiff(diff.ExistsOnBoth.ValueIsDiff)
        Display_Reconciled(diff.ExistsOnBoth.ValueIsSame, diff.KeysMissing)
        Display_Missing(diff.KeysMissing)
    End Sub
    Private Sub Display(clientOnly As CConfigSettingList)
        If clientOnly.Count = 0 Then Exit Sub

        Dim pnl As New Panel
        plh.Controls.Add(pnl)

        Dim tbl As New Table
        tbl.CssClass = "datagrid"
        pnl.Controls.Add(tbl)

        Dim title As TableRow = RowH(tbl, "Only On Client", 3)
        'Add buttons

        For Each i As CConfigSetting In clientOnly
            Dim tr As TableRow = Row(tbl)
            Cell(tr, i.GroupName)
            Cell(tr, i.SettingName)
            Cell(tr, i.ValueAsString)
            'Add buttons
        Next
    End Sub
    Private Sub Display(serverOnly As CValueList)
        If serverOnly.Count = 0 Then Exit Sub

        Dim pnl As New Panel
        plh.Controls.Add(pnl)

        Dim tbl As New Table
        tbl.CssClass = "datagrid"
        pnl.Controls.Add(tbl)

        Dim title As TableRow = RowH(tbl, "Only On Server", 2)
        'Add buttons

        For Each i As CValue In serverOnly
            Dim tr As TableRow = Row(tbl)
            Cell(tr, i.Key.GroupAndKey)
            Cell(tr, i.ValueAsString)
            'Add buttons
        Next
    End Sub
    Private Sub Display_GroupIsDiff(diff As CListOfPairs)
        If diff.Count = 0 Then Exit Sub

        Dim pnl As New Panel
        plh.Controls.Add(pnl)

        Dim tbl As New Table
        tbl.CssClass = "datagrid"
        pnl.Controls.Add(tbl)

        Dim title As TableRow = RowH(tbl, "Group Names Different", 3)

        Dim head As TableRow = RowH(tbl)
        CellH(head, "Key")
        CellH(head, "Client-Group")
        CellH(head, "Server-Group")
        'Add buttons

        For Each i As CPair In diff
            Dim tr As TableRow = Row(tbl)
            CellH(tr, i.Client.SettingName)
            Cell(tr, i.Client.GroupName)
            Cell(tr, i.Server.Key.GroupName)
            'Add buttons
        Next
    End Sub
    Private Sub Display_ValueIsDiff(diff As CListOfPairs)
        If diff.Count = 0 Then Exit Sub

        Dim pnl As New Panel
        plh.Controls.Add(pnl)

        Dim tbl As New Table
        tbl.CssClass = "datagrid"
        pnl.Controls.Add(tbl)


        Dim head As TableRow = RowH(tbl)
        CellH(head, "#")
        CellH(head, CUtilities.NameAndCount("Different Values", diff)).ColumnSpan = 2
        CellH(head, "Client")
        CellH(head, "Server")
        'Add buttons

        For Each i As CPair In diff
            Dim tr As TableRow = Row(tbl)
            CellH(tr, (diff.IndexOf(i) + 1) & ".")
            CellH(tr, i.Server.Key.GroupName)
            CellH(tr, i.Server.Key.KeyName)
            Cell(tr, i.Client.ValueAsString)
            Cell(tr, i.Server.ValueAsString)
            'Add buttons
        Next
    End Sub
    Private Sub Display_Reconciled(diff As CListOfPairs, missing As CKeyList)
        If diff.Count = 0 Then Exit Sub

        Dim pnl As New Panel
        plh.Controls.Add(pnl)

        Dim tbl As New Table
        tbl.CssClass = "datagrid"
        pnl.Controls.Add(tbl)

        Dim tr As TableRow = Row(tbl)
        CellH(tr, "Matching")
        Cell(tr, diff.Count.ToString())

        tr = Row(tbl)
        CellH(tr, "Missing")
        Cell(tr, missing.Count.ToString())
    End Sub
    Private Sub Display_Missing(missing As CKeyList)
        If missing.Count = 0 Then Exit Sub

        If missing.Count = CKey.Cache.Count Then
			CSession.PageMessage = "No server-side settings were Found!" & vbCrLf & "First step is to click Files => Database" & vbCrLf & "This migrates config-file settings to DB"
			Exit Sub
		End If

		Dim pnl As New Panel
        plh.Controls.Add(pnl)

        Dim tbl As New Table
        tbl.CssClass = "datagrid"
        pnl.Controls.Add(tbl)


        Dim th As TableRow = RowH(tbl)
        CellH(th, "#")
        CellH(th, CUtilities.NameAndCount("Missing Values", missing)).ColumnSpan = 2
        CellH(th, "Format")
        CellH(th, "Default")

        For Each i As CKey In missing
            Dim tr As TableRow = Row(tbl)
            CellH(tr, (missing.IndexOf(i) + 1) & ".")
            CellH(tr, i.GroupName)
            CellLinkH(tr, i.KeyName, CSitemap.KeyEdit(i.KeyName), False)
            Cell(tr, i.Format.FormatName)
            Cell(tr, i.DefaultValue)
        Next
    End Sub

    Private Sub btnKeyGen_Click(sender As Object, e As EventArgs) Handles btnKeyGen.Click
        For Each i As CKey In CKey.Cache
            CValue.Cache.GetOrCreate(InstanceId, i.KeyName)
        Next
        Response.Redirect(Request.RawUrl)
    End Sub


#End Region

#Region "Classes"
    Private Class CDiff
        'Results
        Public OnlyOnClient As CConfigSettingList
        Public OnlyOnServer As CValueList
        Public ExistsOnBoth As CListOfPairs
        Public KeysMissing As CKeyList

        'Constructor
        Public Sub New(client As CConfigSettingList, server As CValueList)
            Me.OnlyOnClient = Compute_OnlyOnClient(client, server)
            Me.OnlyOnServer = Compute_OnlyOnServer(client, server)
            Me.ExistsOnBoth = Compute_OnBoth(client, server)
            Me.KeysMissing = Compute_Missing(server)
        End Sub

        'Logic
        Private Shared Function Compute_OnlyOnClient(client As CConfigSettingList, server As CValueList) As CConfigSettingList
            Dim list As New CConfigSettingList(client.Count)
            For Each i As CConfigSetting In client
                If server.GetByKeyName(i.SettingName).Count = 0 Then
                    list.Add(i)
                End If
            Next
            Return list
        End Function
        Private Shared Function Compute_OnlyOnServer(client As CConfigSettingList, server As CValueList) As CValueList
            Dim list As New CValueList(server.Count)
            For Each i As CValue In server
                If Not client.HasKey(i.ValueKeyName) Then
                    list.Add(i)
                End If
            Next
            Return list
        End Function
        Private Shared Function Compute_OnBoth(client As CConfigSettingList, server As CValueList) As CListOfPairs
            Dim list As New CListOfPairs(server.Count)
            For Each i As CValue In server
                Dim c As CConfigSetting = client.Get(i.ValueKeyName)
                If Not IsNothing(c) Then
                    Dim pair As New CPair
                    pair.Client = c
                    pair.Server = i
                    list.Add(pair)
                End If
            Next
            Return list
        End Function
        Private Shared Function Compute_Missing(server As CValueList) As CKeyList
            Dim list As New CKeyList(CKey.Cache.Count)
            For Each i As CKey In CKey.Cache
                If server.GetByKeyName(i.KeyName).Count = 0 Then
                    list.Add(i)
                End If
            Next
            Return list
        End Function
    End Class

    Private Class CPair
        Public Client As CConfigSetting
        Public Server As CValue

        Public ReadOnly Property GroupIsDiff As Boolean
            Get
                Return Client.GroupName <> Server.Key.GroupName
            End Get
        End Property
        Public ReadOnly Property ValueIsDiff As Boolean
            Get
                Select Case Server.Key.KeyFormatId_
                    Case EFormat.String : Return Client.ValueString <> Server.ValueString
                    Case EFormat.Boolean
						If Not Client.ValueBool.HasValue Then Return Server.ValueBoolean.HasValue
						If Not Server.ValueBoolean.HasValue Then Return False
                        Return Client.ValueBool.Value <> Server.ValueBoolean.Value
					Case EFormat.Integer
						If Not Client.ValueInt.HasValue AndAlso Not Server.ValueInteger.HasValue Then Return True
						If Not Client.ValueInt.HasValue Then Return Server.ValueInteger <> Integer.MinValue
                        Return Client.ValueInt <> Server.ValueInteger
                End Select
                Throw New Exception("Unknown Format: " & Server.Key.Format.FormatName)
            End Get
        End Property
    End Class

    Private Class CListOfPairs : Inherits List(Of CPair)
        Public Sub New(count As Integer)
            MyBase.New(count)
        End Sub

        Private mGroupIsDiff As CListOfPairs
        Private mValueIsDiff As CListOfPairs
        Private mValueIsSame As CListOfPairs
        Public ReadOnly Property GroupIsDiff() As CListOfPairs
            Get
                If IsNothing(mGroupIsDiff) Then
                    Dim temp As New CListOfPairs(Me.Count)
                    For Each i As CPair In Me
                        If i.GroupIsDiff Then temp.Add(i)
                    Next
                    mGroupIsDiff = temp
                End If
                Return mGroupIsDiff
            End Get
        End Property
        Public ReadOnly Property ValueIsDiff() As CListOfPairs
            Get
                If IsNothing(mValueIsDiff) Then
                    Dim tempD As New CListOfPairs(Me.Count)
                    Dim tempS As New CListOfPairs(Me.Count)
                    For Each i As CPair In Me
                        If i.ValueIsDiff Then tempD.Add(i) Else tempS.Add(i)
                    Next
                    mValueIsDiff = tempD
                    mValueIsSame = tempS
                End If
                Return mValueIsDiff
            End Get
        End Property
        Public ReadOnly Property ValueIsSame() As CListOfPairs
            Get
                If IsNothing(mValueIsSame) Then
                    Dim tempD As New CListOfPairs(Me.Count)
                    Dim tempS As New CListOfPairs(Me.Count)
                    For Each i As CPair In Me
                        If i.ValueIsDiff Then tempD.Add(i) Else tempS.Add(i)
                    Next
                    mValueIsDiff = tempD
                    mValueIsSame = tempS
                End If
                Return mValueIsSame
            End Get
        End Property
    End Class
#End Region

End Class
