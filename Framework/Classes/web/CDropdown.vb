Imports System.Web.UI.WebControls

Public Class CDropdown

    Public Shared Sub SetText(ByVal dd As ListControl, ByVal text As String)

        text = Trim(LCase(text))
        Dim i As ListItem
        For Each i In dd.Items
            If Trim(LCase(i.Text)) = text Then
                dd.SelectedIndex = -1
                i.Selected = True
                Exit Sub
            End If
        Next
    End Sub
    Public Shared Function Add(ByVal dd As ListControl, ByVal text As String, ByVal val As Byte) As ListItem
        Return Add(dd, text, val.ToString)
    End Function
    Public Shared Function Add(ByVal dd As ListControl, ByVal text As String, ByVal val As Integer) As ListItem
        Return Add(dd, text, val.ToString)
    End Function
    Public Shared Function Add(ByVal dd As ListControl, ByVal text As String, ByVal val As Guid) As ListItem
        Return Add(dd, text, val.ToString)
    End Function
    Public Shared Function Add(ByVal dd As ListControl, ByVal text As String, ByVal val As Long) As ListItem
        Return Add(dd, text, val.ToString)
    End Function
    Public Shared Function Add(ByVal dd As ListControl, ByVal text As String, ByVal val As String) As ListItem
        Return Add(dd, New ListItem(text, val))
    End Function
    Public Shared Function Add(ByVal dd As ListControl, ByVal text As String) As ListItem
        Return Add(dd, New ListItem(text, text))
    End Function
    Public Shared Sub AddEnums(ByVal dd As ListControl, ByVal enumType As Type)
        For Each text As String In [Enum].GetNames(enumType)
            Dim value As Integer = CInt([Enum].Parse(enumType, text))
            Add(dd, text, value)
        Next
    End Sub
    Public Shared Sub AddList(ByVal ctrl As ListControl, ByVal list As CNameValueList)
        For Each i As CNameValue In list
            Add(ctrl, i.Name, i.Value.ToString)
        Next
    End Sub

    Private Shared Function Add(ByVal dd As ListControl, ByVal li As ListItem) As ListItem
        dd.Items.Add(li)
        Return li
    End Function
    Public Shared Function GetInt(ByVal dd As ListControl) As Integer
        Try
            Return CTextbox.GetInteger(dd.SelectedValue)
        Catch ex As Exception
            Return Integer.MinValue
        End Try
    End Function
    Public Shared Function GetLong(ByVal dd As ListControl) As Long
        Try
            Return CTextbox.GetLong(dd.SelectedValue)
        Catch ex As Exception
            Return Long.MinValue
        End Try
    End Function
    Public Shared Function GetByte(ByVal dd As ListControl) As Byte
        Try
            Return CTextbox.GetByte(dd.SelectedValue)
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Shared Function GetGuid(ByVal dd As ListControl) As Guid
        Try
            If String.IsNullOrEmpty(dd.SelectedValue) Then Return Guid.Empty
            Return New Guid(dd.SelectedValue)
        Catch ex As Exception
            Return Guid.Empty
        End Try
    End Function
    Public Shared Sub SetValue(ByVal dd As ListControl, ByVal val As String)
        If TypeOf dd Is DropDownList OrElse TypeOf dd Is RadioButtonList Then
            Try
                dd.SelectedValue = val
            Catch
            End Try
            If Not String.IsNullOrEmpty(val) AndAlso dd.SelectedValue.tolower <> val Then
                For Each i As ListItem In dd.Items
                    If i.Value.ToLower = val.ToLower Then
                        dd.SelectedValue = i.Value
                        Exit Sub
                    End If
                Next
            End If
        Else
            For Each i As ListItem In dd.Items
                If i.Value.ToLower = val.ToLower Then i.Selected = True
            Next
        End If
    End Sub
    Public Shared Sub SetValue(ByVal dd As ListControl, ByVal val As Guid)
        If val = Guid.Empty Then
            SetValue(dd, String.Empty)
        Else
            SetValue(dd, val.ToString)
        End If
    End Sub
    Public Shared Sub SetValue(ByVal dd As ListControl, ByVal val As Integer)
        If val = Integer.MinValue Then
            SetValue(dd, String.Empty)
        Else
            SetValue(dd, val.ToString)
        End If
    End Sub
    Public Shared Sub SetValue(ByVal dd As ListControl, ByVal val As Byte)
        If val = 0 Then
            SetValue(dd, String.Empty)
        Else
            SetValue(dd, val.ToString)
        End If
    End Sub
    Public Shared Sub SetValue(ByVal dd As ListControl, ByVal val As Byte?)
        If Not val.HasValue OrElse val.Value = 0 Then
            SetValue(dd, String.Empty)
        Else
            SetValue(dd, val.Value.ToString)
        End If
    End Sub
    Public Shared Sub SetValue(ByVal dd As ListControl, ByVal val As Long)
        If val = Long.MinValue Then
            SetValue(dd, String.Empty)
        Else
            SetValue(dd, val.ToString)
        End If
    End Sub
    Public Shared Sub SetValues(ByVal dd As ListControl, ByVal vals As ICollection)
        For Each i As Object In vals
            SetValue(dd, i.ToString)
        Next
    End Sub
    Public Shared Sub SelectAll(ByVal dd As ListControl)
        SelectAll(dd, True)
    End Sub
    Public Shared Sub SelectAll(ByVal dd As ListControl, ByVal value As Boolean)
        For Each i As ListItem In dd.Items
            i.Selected = value
        Next
    End Sub
    Public Shared Function SelectedValues(ByVal dd As ListControl) As List(Of String)
        Dim list As New List(Of String)()
        For Each i As ListItem In dd.Items
            If i.Selected Then list.Add(i.Value)
        Next
        Return list
    End Function
    Public Shared Function SelectedInts(ByVal dd As ListControl) As List(Of Integer)
        Dim list As New List(Of Integer)(dd.Items.Count)
        For Each i As ListItem In dd.Items
            If i.Selected Then list.Add(CTextbox.GetInteger(i.Value))
        Next
        Return list
    End Function
    Public Shared Function SelectedGuids(ByVal dd As ListControl) As List(Of Guid)
        Dim list As New List(Of Guid)(dd.Items.Count)
        For Each i As ListItem In dd.Items
            If i.Selected AndAlso i.Value.Length = 36 Then list.Add(New Guid(i.Value))
        Next
        Return list
    End Function

    Public Shared Function NotSelectedValues(ByVal dd As ListControl) As List(Of String)
        Dim list As New List(Of String)()
        For Each i As ListItem In dd.Items
            If Not i.Selected Then list.Add(i.Value)
        Next
        Return list
    End Function
    Public Shared Function NotSelectedInts(ByVal dd As ListControl) As List(Of Integer)
        Dim list As New List(Of Integer)()
        For Each i As ListItem In dd.Items
            If Not i.Selected Then list.Add(CTextbox.GetInteger(i.Value))
        Next
        Return list
    End Function
    Public Shared Sub Remove(ByVal dd As ListControl, ByVal val As Integer)
        Remove(dd, val.ToString)
    End Sub
    Public Shared Sub Remove(ByVal dd As ListControl, ByVal val As String)
        Dim found As ListItem = dd.Items.FindByValue(val)
        If Not IsNothing(found) Then dd.Items.Remove(found)
    End Sub

    Public Shared Function BlankItem(ByVal dd As ListControl) As ListItem
        Return BlankItem(dd, String.Empty)
    End Function
    Public Shared Function BlankItem(ByVal dd As ListControl, ByVal text As String) As ListItem
        Return BlankItem(dd, text, String.Empty)
    End Function
    Public Shared Function BlankItem(ByVal dd As ListControl, ByVal text As String, ByVal val As String) As ListItem
        Return Insert(dd, New ListItem(text, val))
    End Function

    'Private
    Private Shared Function Insert(ByVal dd As ListControl, ByVal li As ListItem) As ListItem
        dd.Items.Insert(0, li)
        Return li
    End Function

End Class
