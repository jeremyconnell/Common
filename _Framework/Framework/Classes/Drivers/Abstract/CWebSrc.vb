Imports System.Net

'Covers the common logic between the webservice/webpage drivers (soap/binary)
<Serializable(), CLSCompliant(True)> _
Public MustInherit Class CWebSrc : Inherits CDataSrcRemote

#Region "Constructors"
    Public Sub New(ByVal url As String, ByVal password As String)
        MyBase.New(url)

        m_passwordBytes = CBinary.StringToBytes(password)
        If Not CConfigBase.UseRawPassword Then
            m_passwordBytes = CBinary.Sha512(m_passwordBytes)
        End If

        m_connectionString = DefaultPageName(url)
    End Sub
    Public Sub New(ByVal url As String, ByVal passwordBytes As Byte())
        MyBase.New(url)
        m_passwordBytes = passwordBytes
        m_connectionString = DefaultPageName(url)
    End Sub
#End Region

#Region "Abstract"
    Protected MustOverride Function DefaultPageName(ByVal url As String) As String
#End Region

#Region "Events"
    Public Event AsyncOperationError(ByVal ex As Exception)
    Protected Sub Completed(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        If Not IsNothing(e.Error) Then RaiseEvent AsyncOperationError(e.Error)
    End Sub
#End Region

#Region "Private - Members"
    Private m_passwordBytes As Byte()
    Private m_proxy As WebProxy
#End Region

#Region "Private/Protected - Properties"
    Protected ReadOnly Property Url() As String
        Get
            Return m_connectionString
        End Get
    End Property
    Protected ReadOnly Property Proxy() As WebProxy
        Get
            If IsNothing(m_proxy) Then
                'Use Proxy ()
                If Len(CConfigBase.ProxyAddress) = 0 Then Return Nothing

                m_proxy = New WebProxy(CConfigBase.ProxyAddress)

                'Credentials ()
                If Len(CConfigBase.ProxyUser) > 0 Then
                    If Len(CConfigBase.ProxyDomain) > 0 Then
                        m_proxy.Credentials = New NetworkCredential(CConfigBase.ProxyUser, CConfigBase.ProxyPassword, CConfigBase.ProxyDomain)
                    Else
                        m_proxy.Credentials = New NetworkCredential(CConfigBase.ProxyUser, CConfigBase.ProxyPassword)
                    End If
                End If
            End If
            Return m_proxy
        End Get
    End Property
    Protected ReadOnly Property PasswordBytes() As Byte()
        Get
            Return m_passwordBytes
        End Get
    End Property
#End Region

#Region "Private - Pack/Unpack"
    Protected Function Pack(ByVal cmd As CCommand) As Byte()
        CheckTxIsNull(cmd.Transaction)
        Return CBinary.Pack(cmd, PasswordBytes)
    End Function
    Protected Function Pack(ByVal obj As Object) As Byte()
        If TypeOf obj Is CSelectWhere Then
            Dim where As CSelectWhere = CType(obj, CSelectWhere)

            Dim o As Integer = CInt(Me.Offset.TotalHours)
            If o <> 0 Then
                With where
                    If Not IsNothing(.Criteria) Then
                        Dim c As CCriteria = .Criteria
                        If TypeOf (c.ColumnValue) Is DateTime Then
                            c.ColumnValue = CType(c.ColumnValue, DateTime).Subtract(Me.Offset)
                        End If
                    ElseIf Not IsNothing(.CriteriaList) Then
                        For Each c As CCriteria In .CriteriaList
                            If TypeOf (c.ColumnValue) Is DateTime Then
                                c.ColumnValue = CType(c.ColumnValue, DateTime).Subtract(Me.Offset)
                            End If
                        Next
                    End If
                End With
            End If
        End If

        Return CBinary.Pack(obj, PasswordBytes)
    End Function
    Protected Function Unpack(ByVal obj As Byte()) As Object
        Return CBinary.Unpack(obj, PasswordBytes)
    End Function
#End Region


#Region "Private - Logging"
    Protected Overloads Sub Log(ByVal cmd As String)
        Dim sw As New IO.StreamWriter(LogPath, True)
        sw.Write(DateTime.Now.ToString)
        sw.Write(vbTab)
        sw.Write(cmd)
        sw.Write(vbCrLf)
        sw.Close()
    End Sub
    Protected Overloads Sub Log(ByVal cmd As CCommand)
        Dim sw As New IO.StreamWriter(LogPath, True)
        sw.Write(DateTime.Now.ToString)
        sw.Write(vbTab)
        sw.Write(cmd.Text.Replace(vbCrLf, "").Replace(vbLf, ""))
        sw.Write(vbTab)
        If Not IsNothing(cmd.ParametersNamed) Then
            For Each i As CNameValue In cmd.ParametersNamed
                sw.Write("{")
                sw.Write(i.Name)
                sw.Write("=")
                If Not IsNothing(i.Value) Then sw.Write(i.Value.ToString.Replace(vbCrLf, "").Replace(vbLf, ""))
                sw.Write("}")
                sw.Write(vbTab)
            Next
        ElseIf Not IsNothing(cmd.ParametersUnnamed) Then
            sw.Write("{")
            sw.Write(CUtilities.ListToString(cmd.ParametersUnnamed))
            sw.Write("}")
        End If
        sw.Write(vbCrLf)
        sw.Close()
    End Sub
    Private Shared _logPath As String
    Private ReadOnly Property LogPath() As String
        Get
            If IsNothing(_logPath) Then
                _logPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/dblog.txt")

                Dim folder As String = IO.Path.GetDirectoryName(_logPath)
                If Not IO.Directory.Exists(folder) Then IO.Directory.CreateDirectory(folder)
            End If
            Return _logPath
        End Get
    End Property
#End Region

End Class
