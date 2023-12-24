Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization
Imports Framework

<CLSCompliant(True), Serializable()> _
Public MustInherit Class CBaseDynamicAudited3Way : Inherits Framework.CBaseDynamic3Way

#Region "Constructors (Standard)"
    'Main Constructors
    Protected Sub New()    'Used for Insert and Select-Multiple
        MyBase.New()
    End Sub
    Protected Sub New(ByVal primaryKey As Object, ByVal secondaryKey As Object, ByVal tertiaryKey As Object) 'Used for Update and Select-Single
        Me.Load(primaryKey, secondaryKey, tertiaryKey, Nothing)
    End Sub
    Protected Sub New(ByVal primaryKey As Object, ByVal secondaryKey As Object, ByVal tertiaryKey As Object, ByVal txOrNull As IDbTransaction) 'Used for Update and Select-Single within a transaction
        Me.Load(primaryKey, secondaryKey, tertiaryKey, txOrNull)
    End Sub
    Protected Sub New(ByVal dr As IDataReader) 'Used for Select-Multiple
        Me.Load(dr)
    End Sub
    Protected Sub New(ByVal dr As DataRow) 'Used for Select-Multiple
        Me.Load(dr)
    End Sub

    'As above, with CDataSrc
    Protected Sub New(ByVal dataSrc As CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal primaryKey As Object, ByVal secondaryKey As Object, ByVal tertiaryKey As Object) 'Used for Update and Select-Single
        Me.DataSrc = dataSrc
        Me.Load(primaryKey, secondaryKey, tertiaryKey, Nothing)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal primaryKey As Object, ByVal secondaryKey As Object, ByVal tertiaryKey As Object, ByVal txOrNull As IDbTransaction) 'Used for Update and Select-Single within a transaction
        Me.DataSrc = dataSrc
        Me.Load(primaryKey, secondaryKey, tertiaryKey, txOrNull)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As IDataReader) 'Used for Select-Multiple
        MyBase.New(dataSrc, dr)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As DataRow) 'Used for Select-Multiple
        MyBase.New(dataSrc, dr)
    End Sub
#End Region

#Region "Abstract/Virtual"
    'Pull a fresh instance from the database using the same PK value, to get a "before" snapshot
    Protected MustOverride Function OriginalState(ByVal txOrNull As IDbTransaction) As CBaseDynamicAudited3Way
#End Region

#Region "Audit Trail Logic"
    Public Sub SaveWithoutAudit()
        SaveWithoutAudit(Nothing)
    End Sub
    Public Sub SaveWithoutAudit(ByVal txOrNull As IDbTransaction)
        MyBase.Save(txOrNull)
    End Sub
    Public Overloads Overrides Sub Save(ByVal txOrNull As System.Data.IDbTransaction)
        'Off-switch
        If Not CBaseDynamicAudited.ENABLED Then
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
            .AuditDataPrimaryKey = String.Concat(Me.PrimaryKeyValue, "/", Me.SecondaryKeyValue, "/", Me.TertiaryKeyValue)
            If m_insertPending Then
                .AuditTypeId = EAuditType.Insert
                before = Nothing
            Else
                .AuditTypeId = EAuditType.Update
                before = OriginalState(txOrNull).ToXml()

                'Ignore trivel saves
                If after = before Then Exit Sub
            End If
            .Save(txOrNull)
            CBaseDynamicAudited.SaveBeforeAfter(.AuditId, before, after, txOrNull)
        End With

        'Normal insert/update
        MyBase.Save(txOrNull)
    End Sub
    Public Overloads Overrides Sub Delete(ByVal txOrNull As System.Data.IDbTransaction)
        'Off-switch
        If Not CBaseDynamicAudited.ENABLED Then
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
            .AuditDataPrimaryKey = String.Concat(Me.PrimaryKeyValue, "/", Me.SecondaryKeyValue, "/", Me.TertiaryKeyValue)
            .Save(txOrNull)
            CBaseDynamicAudited.SaveBeforeAfter(.AuditId, Me.ToXml, Nothing, txOrNull)
        End With

        'Normal delete
        MyBase.Delete(txOrNull)
    End Sub
#End Region

End Class
