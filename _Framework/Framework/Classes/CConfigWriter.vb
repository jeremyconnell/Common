Imports System.Configuration
Imports System.Configuration.ConfigurationManager
Imports System.Web.Configuration
Imports System.Web.HttpContext

'Removed from CConfigBase due to reference to System.Web (breaks newer console apps)
'TODO: split into web/non-web versions
Public Class CConfigWriter : Inherits CConfigBase



    Protected Shared Function ConfigIntOrWrite(ByVal key As String, defaultValue As Integer) As Integer
        Dim s As String = String.Empty
        If defaultValue <> Integer.MinValue Then s = defaultValue.ToString
        s = ConfigOrWrite(key, s)
        Dim i As Integer
        If Not Integer.TryParse(s, i) Then Return defaultValue
        Return i
    End Function
    Protected Shared Function ConfigOrWrite(ByVal key As String, defaultValue As String) As String
        Dim s As String = Config(key)
        If Not String.IsNullOrEmpty(s) Then Return s

        If m_wrote.TryGetValue(key, s) Then Return s

        s = defaultValue
        WriteAppSetting(key, defaultValue)

        Return s
    End Function




    Private Shared m_checked As Boolean = False
    Public Sub CheckConfigIsEncrypted()
        If m_checked Then Exit Sub

        If IsNothing(Current) Then
            CheckConfigIsEncrypted_NonWeb()
        Else
            CheckConfigIsEncrypted_Web()
        End If

        m_checked = True
    End Sub
    Private Shared Sub CheckConfigIsEncrypted_Web()
        Try
            If IsNothing(Current.Request) Then Exit Sub
        Catch
            Exit Sub
        End Try
        If Current.Request.Url.Host.ToLower().Contains("localhost") Then Exit Sub
        Dim c As Configuration
        Try
            c = WebConfigurationManager.OpenWebConfiguration(Current.Request.ApplicationPath)
        Catch ex As Exception
            c = WebConfigurationManager.OpenWebConfiguration("~/web.config")
        End Try
        Dim cs As ConfigurationSection = c.GetSection("encryptedSettings")
        If IsNothing(cs) Then Exit Sub
        Dim si As SectionInformation = cs.SectionInformation
        If Not si.IsProtected Then
            si.ProtectSection("DataProtectionConfigurationProvider") 'RSA '"DPAPIProtectedConfigurationProvider")
            'c.SaveAs("d:\\test.txt", ConfigurationSaveMode.Full)
            Try
                c.Save(ConfigurationSaveMode.Full)
            Catch ex As Exception
                Dim path As String = Current.Server.MapPath("~/App_Data/web.config.encrypted.txt")
                Try
                    c.SaveAs(path, ConfigurationSaveMode.Full)
                Catch
                    Throw New Exception("Insufficient permissions to update web.config, or save it to ~/App_Data. Need to configure permissions to allow save")
                End Try
                Throw New Exception("Insufficient permissions to update web.config, so wrote encrypted config section to: " & path)
            End Try
        End If
    End Sub
    Private Shared Sub CheckConfigIsEncrypted_NonWeb()
        Dim c As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        Dim cs As ConfigurationSection = c.GetSection("encryptedSettings")
        If IsNothing(cs) Then Exit Sub
        Dim si As SectionInformation = cs.SectionInformation
        If Not si.IsProtected Then
            si.ProtectSection("DataProtectionConfigurationProvider") 'RSA '"DPAPIProtectedConfigurationProvider")
            c.Save(ConfigurationSaveMode.Full)
        End If
    End Sub



    'Write an appSetting
    Public Shared Sub WriteAppSetting(key As String, value As String, overwrite As Boolean)
        If IsNothing(value) Then value = String.Empty
        If m_wrote.ContainsKey(key) Then Exit Sub
        Dim v As String = Config(key, Nothing)
        If Not String.IsNullOrEmpty(v) AndAlso v = value AndAlso value.Length > 0 Then Exit Sub
        WriteAppSetting(key, value)
    End Sub
    Public Shared Sub WriteAppSetting(key As String, value As String)
        If m_wrote.ContainsKey(key) Then Exit Sub
        m_wrote(key) = value

        If CApplication.IsWebApplication Then
            Dim webConfig As String = System.Web.HttpContext.Current.Server.MapPath("~/web.config")
            WriteAppSetting(key, value, webConfig)
        Else
            Dim appConfig As String = String.Concat(System.Reflection.Assembly.GetEntryAssembly().Location, ".config")
            WriteAppSetting(key, value, appConfig)
        End If
    End Sub
    Public Shared Sub WriteConnectionString(name As String, value As String)
        Dim key As String = "ConnStr_" & name
        If m_wrote.ContainsKey(key) Then Exit Sub
        m_wrote(key) = value

        If CApplication.IsWebApplication Then
            Dim webConfig As String = System.Web.HttpContext.Current.Server.MapPath("~/web.config")
            WriteConnectionString(key, value, webConfig)
        Else
            Dim appConfig As String = String.Concat(System.Reflection.Assembly.GetEntryAssembly().Location, ".config")
            WriteConnectionString(key, value, appConfig)
        End If
    End Sub
    Public Shared Sub RemoveAppSetting(key As String)
        If CApplication.IsWebApplication Then
            Dim webConfig As String = System.Web.HttpContext.Current.Server.MapPath("~/web.config")
            RemoveAppSetting(key, webConfig)
        Else
            Dim appConfig As String = String.Concat(System.Reflection.Assembly.GetEntryAssembly().Location, ".config")
            RemoveAppSetting(key, appConfig)
        End If
    End Sub
    Public Shared Sub RemoveAppSetting(name As String, path As String)
        If Not IO.File.Exists(path) Then Exit Sub 'Throw New Exception("Config file not found: " & path)

        'Load the xml
        Dim xml As New Xml.XmlDocument
        xml.PreserveWhitespace = True
        xml.LoadXml(IO.File.ReadAllText(path))

        'Find (or create) the node
        Dim root As Xml.XmlNode = xml.DocumentElement
        Dim node As Xml.XmlNode = CXml.ChildNode(root, "appSettings")
        Dim sett As Xml.XmlNode = Nothing
        For Each i As Xml.XmlNode In CXml.ChildNodes(node, "add")
            Dim key As String = CXml.AttributeStr(i, "key")
            If key = name Then
                sett = i
                Exit For
            End If
        Next
        If Not IsNothing(sett) Then
            sett.ParentNode.RemoveChild(sett)
            xml.Save(path)
        End If
    End Sub
    Public Shared Function WriteAppSetting(name As String, value As String, path As String) As Boolean
        If Not IO.File.Exists(path) Then Throw New Exception("Config file not found: " & path)

        'Load the xml
        Dim xml As New Xml.XmlDocument
        xml.PreserveWhitespace = True
        xml.LoadXml(IO.File.ReadAllText(path))

        'Find (or create) the node
        Dim root As Xml.XmlNode = xml.DocumentElement
        Dim node As Xml.XmlNode = CXml.ChildNode(root, "appSettings")
        Dim sett As Xml.XmlNode = Nothing
        Dim all As List(Of Xml.XmlNode) = CXml.ChildNodes(node, "add")
        For Each i As Xml.XmlNode In all
            Dim key As String = CXml.AttributeStr(i, "key")
            Dim val As String = CXml.AttributeStr(i, "value")
            If key = name Then
                If val = value Then Return False 'No change required
                sett = i
                Exit For
            End If
        Next
        If IsNothing(sett) Then
            node.AppendChild(xml.CreateWhitespace(vbCrLf & "    "))

            sett = CXml.AddNode(node, "add")
            CXml.AttributeSet(sett, "key", name)

            node.AppendChild(xml.CreateWhitespace(vbCrLf & "  "))
        End If

        'Set value and save
        CXml.AttributeSet(sett, "value", value)
        xml.Save(path)
        Return True
    End Function
    Public Shared Function WriteConnectionString(name As String, connectionString As String, path As String) As Boolean
        If Not IO.File.Exists(path) Then Throw New Exception("Config file not found: " & path)

        'Load the xml
        Dim xml As New Xml.XmlDocument
        xml.PreserveWhitespace = True
        xml.LoadXml(IO.File.ReadAllText(path))

        'Find (or create) the node
        Dim root As Xml.XmlNode = xml.DocumentElement
        Dim node As Xml.XmlNode = CXml.ChildNode(root, "connectionStrings")
        Dim sett As Xml.XmlNode = Nothing
        Dim all As List(Of Xml.XmlNode) = CXml.ChildNodes(node, "add")
        For Each i As Xml.XmlNode In all
            Dim key As String = CXml.AttributeStr(i, "name")
            Dim val As String = CXml.AttributeStr(i, "connectionString")
            If key = name Then
                If val = connectionString Then Return False 'No change required
                sett = i
                Exit For
            End If
        Next
        If IsNothing(sett) Then
            Dim w As Xml.XmlNode = xml.CreateWhitespace(vbCrLf & "    ")
            If all.Count > 0 Then w = xml.CreateWhitespace("    ")
            node.AppendChild(w)

            sett = CXml.AddNode(node, "add")
            CXml.AttributeSet(sett, "name", name)
            CXml.AttributeSet(sett, "connectionString", CConfigBase.ConnectionString)

            w = xml.CreateWhitespace(vbCrLf & "  ") 'closing tag (appSettings)
            node.AppendChild(w)
        End If

        'Set value and save
        CXml.AttributeSet(sett, "connectionString", connectionString)
        xml.Save(path)
        Return True
    End Function

    Public Shared Sub RemoveConnectionString(name As String)
        If CApplication.IsWebApplication Then
            Dim webConfig As String = System.Web.HttpContext.Current.Server.MapPath("~/web.config")
            RemoveConnectionString(name, webConfig)
        Else
            Dim appConfig As String = String.Concat(System.Reflection.Assembly.GetEntryAssembly().Location, ".config")
            RemoveConnectionString(name, appConfig)
        End If
    End Sub
    Public Shared Sub RemoveConnectionString(name As String, path As String)
        If Not IO.File.Exists(path) Then Exit Sub 'Throw New Exception("Config file not found: " & path)

        'Load the xml
        Dim xml As New Xml.XmlDocument
        xml.PreserveWhitespace = True
        xml.LoadXml(IO.File.ReadAllText(path))

        'Find (or create) the node
        Dim root As Xml.XmlNode = xml.DocumentElement
        Dim node As Xml.XmlNode = CXml.ChildNode(root, "connectionStrings")
        Dim sett As Xml.XmlNode = Nothing
        For Each i As Xml.XmlNode In CXml.ChildNodes(node, "add")
            Dim key As String = CXml.AttributeStr(i, "name")
            If key = name Then
                sett = i
                Exit For
            End If
        Next
        If Not IsNothing(sett) Then
            sett.ParentNode.RemoveChild(sett)
            xml.Save(path)
        End If
    End Sub

End Class
