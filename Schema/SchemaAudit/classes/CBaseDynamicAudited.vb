Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization
Imports Framework
Imports System.Data

<CLSCompliant(True), Serializable()> _
Public MustInherit Class CBaseDynamicAudited : Inherits Framework.CBaseDynamic

    Public Shared ENABLED As Boolean = True

#Region "Constructors (Standard)"
    'Main Constructors
    Protected Sub New()    'Used for Insert and Select-Multiple
        MyBase.New()
    End Sub
    Protected Sub New(ByVal primaryKey As Object) 'Used for Update and Select-Single
        MyBase.New(primaryKey, Nothing)
    End Sub
    Protected Sub New(ByVal primaryKey As Object, ByVal txOrNull As IDbTransaction) 'Used for Update and Select-Single within a transaction
        MyBase.New(primaryKey, txOrNull)
    End Sub
    Protected Sub New(ByVal dr As IDataReader) 'Used for Select-Multiple
        MyBase.New(dr)
    End Sub
    Protected Sub New(ByVal dr As DataRow) 'Used for Select-Multiple
        MyBase.new(dr)
    End Sub

    'As above, with CDataSrc
    Protected Sub New(ByVal dataSrc As CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal primaryKey As Object)
        MyBase.New(dataSrc, primaryKey)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal primaryKey As Object, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, primaryKey, txOrNull)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As IDataReader)
        MyBase.New(dataSrc, dr)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal row As DataRow)
        MyBase.New(dataSrc, row)
    End Sub
#End Region

#Region "Abstract/Virtual"
    'Pull a fresh instance from the database using the same PK value, to get a "before" snapshot
    Protected MustOverride Function OriginalState(ByVal txOrNull As IDbTransaction) As CBaseDynamicAudited
#End Region

#Region "Audit Trail Logic"
    Public Sub SaveWithoutAudit()
        SaveWithoutAudit(Nothing)
    End Sub
    Public Sub SaveWithoutAudit(ByVal txOrNull As IDbTransaction)
        MyBase.Save(txOrNull)
    End Sub
    Public Overloads Overrides Sub Save(ByVal txOrNull As IDbTransaction)
        'Off-switch
        If Not ENABLED Then
            MyBase.Save(txOrNull)
            Exit Sub
        End If

        'Use a transaction if none supplied, by calling this method again with a transaction
        If txOrNull Is Nothing Then
            BulkSave(Me)
            Exit Sub
        End If

        'Audit Entry
        Dim before As String
        Dim after As String
        With New CAudit_Trail(DataSrc) 'Audit trail goes in same database by default
            .AuditDataTableName = Me.TableName
            after = Me.ToXml
            If m_insertPending Then
                .AuditTypeId = EAuditType.Insert
                If InsertPrimaryKey Then
                    'Have pk data
                    .AuditDataPrimaryKey = Me.PrimaryKeyValue.ToString
                    .Save(txOrNull)
                    MyBase.Save(txOrNull)
                Else
                    'Pk data comes from db after save
                    MyBase.Save(txOrNull)
                    .AuditDataPrimaryKey = Me.PrimaryKeyValue.ToString
                    .Save(txOrNull)
                    SaveBeforeAfter(.AuditId, Nothing, after, txOrNull)
                End If
            Else
                .AuditTypeId = EAuditType.Update
                before = OriginalState(txOrNull).ToXml()
                .AuditDataPrimaryKey = Me.PrimaryKeyValue.ToString

                'Ignore trivel saves
                If after = before Then Exit Sub

                .Save(txOrNull)
                SaveBeforeAfter(.AuditId, before, after, txOrNull)
                MyBase.Save(txOrNull)
            End If
        End With
    End Sub
    Public Overloads Overrides Sub Delete(ByVal txOrNull As System.Data.IDbTransaction)
        'Off-switch
        If Not ENABLED Then
            MyBase.Delete(txOrNull)
            Exit Sub
        End If

        'Use a transaction if none supplied (if possible)
        If txOrNull Is Nothing AndAlso DataSrc.IsLocal Then
            BulkDelete(Me)
            Exit Sub
        End If

        'Audit Entry
        With New CAudit_Trail(DataSrc)
            .AuditTypeId = EAuditType.Delete
            .AuditDataTableName = Me.TableName
            .AuditDataPrimaryKey = Me.PrimaryKeyValue.ToString
            .Save(txOrNull)
            SaveBeforeAfter(.AuditId, Me.ToXml, Nothing, txOrNull)
        End With

        'Normal delete
        MyBase.Delete(txOrNull)
    End Sub
    Friend Shared Sub SaveBeforeAfter(ByVal trailId As Integer, ByVal before As String, ByVal after As String, ByVal txOrNull As IDbTransaction)
        Dim b As Dictionary(Of String, String) = Convert(before)
        Dim a As Dictionary(Of String, String) = Convert(after)

        'Save After
        For Each i As String In a.Keys
            If b.ContainsKey(i) AndAlso b(i) = a(i) Then Continue For
            Dim d As New CAudit_Data
            d.DataIsBefore = False
            d.DataTrailId = trailId
            d.DataName = i
            d.DataValue = a(i)
            d.Save(txOrNull)
        Next

        'Save before
        For Each i As String In b.Keys
            If a.ContainsKey(i) AndAlso b(i) = a(i) Then Continue For
            Dim d As New CAudit_Data
            d.DataIsBefore = True
            d.DataTrailId = trailId
            d.DataName = i
            d.DataValue = b(i)
            d.Save(txOrNull)
        Next
    End Sub
    Private Shared Function Convert(ByVal xml As String) As Dictionary(Of String, String)
        If String.IsNullOrEmpty(xml) Then Return New Dictionary(Of String, String)(0)
        Try
            Dim dom As New XmlDocument()
            dom.LoadXml(xml)
            Dim root As XmlNode = dom.ChildNodes(0)
            Dim list As New Dictionary(Of String, String)(root.Attributes.Count)
            For Each i As XmlAttribute In root.Attributes
                list.Add(i.Name, i.Value)
            Next
            Return list
        Catch
            Return New Dictionary(Of String, String)(0)
        End Try
    End Function
#End Region

End Class
