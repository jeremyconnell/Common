Imports Microsoft.VisualBasic

Public Class CConfig : Inherits CConfigBase

    Public Shared Function IsDev() As Boolean
        Return HttpContext.Current.Request.Url.Host.ToLower.Contains("localhost")
    End Function

    Public Shared Function Layout() As ELayout
        Dim s As String = Config("Layout", "Horizontal")
        Return CType([Enum].Parse(GetType(ELayout), s), ELayout)
    End Function
    Public Shared Function MenuTitle() As String
        Return Config("MenuTitle", "ControlTrack")
    End Function




End Class
