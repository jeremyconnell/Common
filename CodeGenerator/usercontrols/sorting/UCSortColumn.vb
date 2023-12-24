Public Class UCSortColumn

#Region "Events"
    Public Event Changed()
#End Region

#Region "Interface"
    Public Sub Display(ByVal columnNames As List(Of String))
        With columnNames
            .Insert(0, String.Empty)
        End With
        With ComboBox1
            .DataSource = columnNames
            .SelectedIndex = 0
        End With
        chkDesc.Enabled = False
        chkDesc.Checked = False
    End Sub
    Public Sub Clear()
        Dim blank As New List(Of String)
        blank.Add(String.Empty)
        ComboBox1.DataSource = blank
    End Sub
#End Region

#Region "Form"
    Public ReadOnly Property IsDescending() As Boolean
        Get
            Return chkDesc.Checked
        End Get
    End Property
    Public ReadOnly Property ColumnName() As String
        Get
            Return ComboBox1.Text
        End Get
    End Property
    Public ReadOnly Property IsSelected() As Boolean
        Get
            Return Not String.IsNullOrEmpty(ColumnName) 'SelectedIndex > 0
        End Get
    End Property
    Public Property SelectedIndex() As Integer
        Get
            Return ComboBox1.SelectedIndex
        End Get
        Set(ByVal value As Integer)
            If value >= ComboBox1.Items.Count Then Exit Property
            ComboBox1.SelectedIndex = value
        End Set
    End Property
    Public Function ShortName(ByVal table As CTable) As String
        Return table.GetShortName(ColumnName)
    End Function
#End Region

#Region "Event Handlers"
    Private Sub ComboBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox1.TextChanged
        chkDesc.Enabled = IsSelected
        RaiseEvent Changed()
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        chkDesc.Enabled = Len(ComboBox1.Text) > 0
        RaiseEvent Changed()
    End Sub
    Private Sub chkDesc_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDesc.CheckedChanged
        RaiseEvent Changed()
    End Sub
#End Region

End Class
