Imports System.Text


Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract, ProtoContract>
Public Class CMigrationHistory : Inherits List(Of CMigration)
    Private Const SQL As String = "SELECT *, ROW_NUMBER() OVER(ORDER BY MigrationId ASC) AS RowNumber FROM dbo.__MigrationHistory ORDER BY MigrationId"


    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CMigrationHistory)()
    End Sub

    'Constructors
    Friend Sub New()
    End Sub
    Public Sub New(db As CDataSrcLocal)
        Dim dr As IDataReader = db.ExecuteReader(SQL)
        Try
            While dr.Read()
                Me.Add(New CMigration(dr))
            End While
        Catch
            Throw
        Finally
            dr.Close()
        End Try
    End Sub
    Public Sub New(db As CDataSrc)
        Dim dt As DataTable = db.ExecuteDataSet(SQL).Tables(0)
        Try
            For Each dr As DataRow In dt.Rows
                Me.Add(New CMigration(dr))
            Next
        Catch
            Throw
        Finally
        End Try
    End Sub

    Public Function GetChanges(m As CMigration) As CMigrationHistory
        Dim temp As New CMigrationHistory()
        Dim num As Integer = m.RowNumber
        If num < Me.Count Then
            For i As Integer = num To Me.Count - 1
                temp.Add(Me(i))
            Next
        End If
        Return temp
    End Function


    Public Overrides Function ToString() As String
        Dim sb As New StringBuilder
        For Each i As CMigration In Me
            sb.Append(i.RowNumber).Append(". ").AppendLine(CUtilities.FileNameAndSize(i.MigrationId, i.ModelLength))
        Next
        Return sb.ToString
    End Function

End Class
