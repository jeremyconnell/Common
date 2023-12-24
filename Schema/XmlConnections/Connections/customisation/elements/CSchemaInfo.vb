Imports Framework

Partial Public Class CSchemaData

    'Default values
    Protected Overrides Sub ApplyDefaultValues_Custom()
        m_architecture = 0
        m_cSharpNamespace = "SchemaSample"

        If IsNothing(Me.Root) Then
            m_lastConnectionDate = DateTime.Now
        Else
            m_lastConnectionDate = DateTime.Now.AddSeconds(Me.Root.Count)
        End If
    End Sub

    'Post-load event
    Protected Overrides Sub ImportCompleted()
        If String.IsNullOrEmpty(m_outputFolder) Then Exit Sub
        If Not IO.Directory.Exists(m_outputFolder) Then Exit Sub
        If String.IsNullOrEmpty(m_outputFolderReadonly) Then m_outputFolderReadonly = LookForDir("website", m_outputFolder)
        If String.IsNullOrEmpty(m_outputFolderEditable) Then m_outputFolderEditable = LookForDir("website", m_outputFolder)
    End Sub
    Private Shared Function LookForDir(ByVal name As String, ByVal startAtPath As String) As String
        Try
            If Not IO.Directory.Exists(startAtPath) Then Return String.Empty
            Dim parent As IO.DirectoryInfo = IO.Directory.GetParent(startAtPath)
            If IsNothing(parent) Then Return String.Empty
            Dim test As String = String.Concat(parent.FullName, "\", name)
            If IO.Directory.Exists(test) Then Return test

            test = String.Concat(parent.Parent.FullName, "\", name)
            If IO.Directory.Exists(test) Then Return test
        Catch
        End Try
        Return String.Empty
    End Function
End Class

#Region "Sorting"
Public Class SortByLastAccessed
    Implements IComparer(Of CSchemaData),
    IComparer(Of CSqlServerConnection),
    IComparer(Of CMySqlConnection),
    IComparer(Of CMSAccessConnection),
    IComparer(Of CMSExcelConnection),
    IComparer(Of CTextFileConnection),
    IComparer(Of COracleConnection),
    IComparer(Of COdbcConnection),
    IComparer(Of COleDbConnection)

    'Main sort
    Public Function Compare(ByVal x As CSchemaData, ByVal y As CSchemaData) As Integer Implements IComparer(Of CSchemaData).Compare
        Return y.LastConnectionDate.CompareTo(x.LastConnectionDate) 'Date Desc
    End Function

    'Overloads
    Public Function Compare(ByVal x As CSqlServerConnection, ByVal y As CSqlServerConnection) As Integer Implements System.Collections.Generic.IComparer(Of CSqlServerConnection).Compare
        Return Compare(x.SchemaInfo, y.SchemaInfo)
    End Function
    Public Function Compare(ByVal x As CMySqlConnection, ByVal y As CMySqlConnection) As Integer Implements System.Collections.Generic.IComparer(Of CMySqlConnection).Compare
        Return Compare(x.SchemaInfo, y.SchemaInfo)
    End Function
    Public Function Compare(ByVal x As CMSAccessConnection, ByVal y As CMSAccessConnection) As Integer Implements System.Collections.Generic.IComparer(Of CMSAccessConnection).Compare
        Return Compare(x.SchemaInfo, y.SchemaInfo)
    End Function
    Public Function Compare(ByVal x As COdbcConnection, ByVal y As COdbcConnection) As Integer Implements System.Collections.Generic.IComparer(Of COdbcConnection).Compare
        Return Compare(x.SchemaInfo, y.SchemaInfo)
    End Function
    Public Function Compare(ByVal x As COleDbConnection, ByVal y As COleDbConnection) As Integer Implements System.Collections.Generic.IComparer(Of COleDbConnection).Compare
        Return Compare(x.SchemaInfo, y.SchemaInfo)
    End Function
    Public Function Compare(ByVal x As COracleConnection, ByVal y As COracleConnection) As Integer Implements System.Collections.Generic.IComparer(Of COracleConnection).Compare
        Return Compare(x.SchemaInfo, y.SchemaInfo)
    End Function
    Public Function Compare(ByVal x As CMSExcelConnection, ByVal y As CMSExcelConnection) As Integer Implements System.Collections.Generic.IComparer(Of CMSExcelConnection).Compare
        Return Compare(x.SchemaInfo, y.SchemaInfo)
    End Function
    Public Function Compare(ByVal x As CTextFileConnection, ByVal y As CTextFileConnection) As Integer Implements System.Collections.Generic.IComparer(Of CTextFileConnection).Compare
        Return Compare(x.SchemaInfo, y.SchemaInfo)
    End Function
End Class
#End Region
