Imports System.Data.SqlClient

Public Class CDiffLogic

#Region "Generic"
    Public Shared Function Diff(sourceIds As ICollection(Of CValues), targetIds As ICollection(Of CValues)) As CDiff
        Dim d As New CDiff
        For Each i As CValues In sourceIds
            If Not targetIds.Contains(i) Then
                d.SourceOnly.Add(i)
            End If
        Next
        For Each i As CValues In targetIds
            If Not sourceIds.Contains(i) Then
                d.TargetOnly.Add(i)
            Else
                d.Matching.Add(i)
            End If
        Next
        Return d
    End Function
    Public Shared Function Diff(sourceIds As ICollection(Of Integer), targetIds As ICollection(Of Integer)) As CDiff_Int
        Dim d As New CDiff_Int
        For Each i As Integer In sourceIds
            If Not targetIds.Contains(i) Then
                d.SourceOnly.Add(i)
            End If
        Next
        For Each i As Integer In targetIds
            If Not sourceIds.Contains(i) Then
                d.TargetOnly.Add(i)
            Else
                d.Matching.Add(i)
            End If
        Next
        Return d
    End Function
    Public Shared Function Diff(sourceIds As ICollection(Of Decimal), targetIds As ICollection(Of Decimal)) As CDiff_Dec
        Dim d As New CDiff_Dec
        For Each i As Decimal In sourceIds
            If Not targetIds.Contains(i) Then
                d.SourceOnly.Add(i)
            End If
        Next
        For Each i As Decimal In targetIds
            If Not sourceIds.Contains(i) Then
                d.TargetOnly.Add(i)
            Else
                d.Matching.Add(i)
            End If
        Next
        Return d
    End Function
    Public Shared Function Diff(sourceIds As ICollection(Of Long), targetIds As ICollection(Of Long)) As CDiff_Long
        Dim d As New CDiff_Long
        For Each i As Long In sourceIds
            If Not targetIds.Contains(i) Then
                d.SourceOnly.Add(i)
            End If
        Next
        For Each i As Long In targetIds
            If Not sourceIds.Contains(i) Then
                d.TargetOnly.Add(i)
            Else
                d.Matching.Add(i)
            End If
        Next
        Return d
    End Function
    Public Shared Function Diff(source As ICollection(Of Guid), target As ICollection(Of Guid)) As CDiff_Guid
        Dim d As New CDiff_Guid
        For Each i As Guid In source
            If Not target.Contains(i) Then
                d.SourceOnly.Add(i)
            End If
        Next
        For Each i As Guid In target
            If Not source.Contains(i) Then
                d.TargetOnly.Add(i)
            Else
                d.Matching.Add(i)
            End If
        Next
        Return d
    End Function
    Public Shared Function Diff(source As ICollection(Of String), target As ICollection(Of String)) As CDiff_String
        Dim d As New CDiff_String
        For Each i As String In source
            If Not target.Contains(i) Then
                d.SourceOnly.Add(i)
            End If
        Next
        For Each i As String In target
            If Not source.Contains(i) Then
                d.TargetOnly.Add(i)
            Else
                d.Matching.Add(i)
            End If
        Next
        Return d
    End Function



    'Old-style (schema-info)
    Public Shared Sub Diff(this As List(Of String), ref As List(Of String), ByRef missing As List(Of String), ByRef added As List(Of String), ByRef same As List(Of String))
        For Each i As String In this
            If Not ref.Contains(i) Then
                added.Add(i)
            Else
                same.Add(i)
            End If
        Next
        For Each i As String In ref
            If Not this.Contains(i) Then missing.Add(i)
        Next
    End Sub

    Public Shared Sub Diff(this As Dictionary(Of String, List(Of String)), ref As Dictionary(Of String, List(Of String)), ByRef missing As Dictionary(Of String, List(Of String)), ByRef added As Dictionary(Of String, List(Of String)), ByRef same As Dictionary(Of String, List(Of String)), ByRef diff As Dictionary(Of String, List(Of String)))
        For Each i As String In this.Keys
            If Not ref.ContainsKey(i) Then
                added.Add(i, this(i))
            ElseIf CUtilities.ListToString(this(i)) = CUtilities.ListToString(ref(i)) Then
                same.Add(i, this(i))
            Else
                Dim m As New List(Of String)
                Dim a As New List(Of String)
                Dim s As New List(Of String)
                CDiffLogic.Diff(this(i), ref(i), m, a, s)
                diff.Add(i, Summarise(m, a, s))
            End If
        Next
        For Each i As String In ref.Keys
            If Not this.ContainsKey(i) Then missing.Add(i, ref(i))
        Next
    End Sub
    Public Shared Function Summarise(missing As List(Of String), added As List(Of String), same As List(Of String)) As List(Of String)
        Dim temp As New List(Of String)
        For Each i As String In missing
            temp.Add("MISSING: " & i)
        Next
        For Each i As String In added
            temp.Add("EXTRA: " & i)
        Next
        temp.AddRange(same)
        Return temp
    End Function
#End Region

#Region "Import Database - eg populate SqlAzure when cant restore"
    Public Shared Sub VerifyImport(src As CDataSrc, targ As CDataSrc)
        Dim ss As CSchemaInfo = src.SchemaInfo()

        Console.WriteLine("Creating schema")
        For Each i As CTableInfo In ss.Tables
            Console.WriteLine(String.Concat(i.TableName, vbTab, src.SelectCount(i.TableName, Nothing), vbTab, targ.SelectCount(i.TableName, Nothing)))
        Next



        Console.WriteLine()
    End Sub
    Public Shared Sub ImportDatabase(src As CDataSrc, targ As CSqlClient, Optional clean As Boolean = False, Optional si As CSchemaInfo = Nothing)
        Dim d As DateTime = DateTime.Now

        Try
            'Clean-up prev run
            If clean Then
                Dim old As CSchemaInfo = targ.SchemaInfo()

                For Each i As CForeignKey In old.ForeignKeys
                    TryExecute(targ, i.DropScript())
                Next

                For Each i As CIndexInfo In old.Indexes
                    TryExecute(targ, i.DropScript())
                Next

                For Each i As CViewInfo In old.Views
                    TryExecute(targ, i.DropScript())
                Next

                For Each i As CTableInfo In old.Tables
                    TryExecute(targ, i.DropScript())
                Next
            End If

            'Import table schema
            For Each i As CTableInfo In si.Tables
                ImportTableSchema(src, targ, i)
            Next

            'Import Keys, Procs, Views, Indexes
            For Each i As CForeignKey In si.ForeignKeys
                TryExecute(targ, i.CreateScript())
            Next
            Console.WriteLine(CUtilities.CountSummary(si.ForeignKeys, "foreign key"))

            For Each i As CProcedure In si.Procs
                TryExecute(targ, i.CreateScript())
            Next
            Console.WriteLine(CUtilities.CountSummary(si.Procs, "stored procs"))

            For Each i As CViewInfo In si.Views
                TryExecute(targ, i.CreateScript())
            Next

            Console.WriteLine(CUtilities.CountSummary(si.Views, "view"))
        Catch ex As Exception
            Console.WriteLine(ex.Message + vbCr & vbLf & vbCr & vbLf + ex.StackTrace)
        End Try
        Console.WriteLine(CUtilities.Timespan(DateTime.Now.Subtract(d)))
    End Sub
    Public Shared Sub ImportTableSchema(src As CDataSrc, targ As CSqlClient, t As CTableInfo)
        Console.WriteLine("Schema: " + t.TableName)
        Dim d As DateTime = DateTime.Now

        'Create table
        Dim sql As String = t.CreateScript()
        Try
            targ.ExecuteNonQuery(sql)
            Console.WriteLine(t.TableName + " Created: " & CUtilities.Timespan(d))
        Catch ex As Exception
            Console.WriteLine("Create " + t.TableName + " Failed: " + ex.Message)
            Return
        End Try

        For Each i As CIndexInfo In t.Indexes
            TryExecute(targ, i.CreateScript())
        Next
    End Sub


    Public Shared Async Sub ImportTableData(src As CDataSrc, targ As CSqlClient, t As CTableInfo)
        Console.WriteLine("Data: " + t.TableName)
        Dim d As DateTime = DateTime.Now

        'Count
        Dim count As Integer
        Try
            count = src.SelectCount(t.TableName, Nothing)
            Console.WriteLine(CUtilities.NameAndCount(t.TableName, count, "row"))
        Catch ex As Exception
            Console.WriteLine("Count failed: " + ex.Message)
        End Try

        'data import
        If count > 0 Then
            'Target
            Dim bulk As New SqlBulkCopy(targ.ConnectionString, SqlBulkCopyOptions.KeepIdentity Or SqlBulkCopyOptions.TableLock)
            bulk.DestinationTableName = t.TableName
            bulk.BulkCopyTimeout = 3600

            If src.IsRemote Then
                'Source
                Dim ds As DataSet = src.SelectAll_Dataset(t.TableName)
                Dim dt As DataTable = ds.Tables(0)
                Console.WriteLine(t.TableName + " Loaded")

                'Inserts
                Await bulk.WriteToServerAsync(dt)
                bulk.Close()
                ds.Dispose()
            Else
                'Source
                Dim dr As IDataReader = src.Local.SelectAll_DataReader(t.TableName)
                Console.WriteLine(t.TableName + " Opened")

                'Target
                Await bulk.WriteToServerAsync(dr)
                bulk.Close()
            End If
            Console.WriteLine(t.TableName + CUtilities.CountSummary(count, "row") + " inserted")
        End If

        'Create indexes
        For Each i As CIndexInfo In t.Indexes
            TryExecute(targ, i.CreateScript())
        Next
        If t.Indexes.Count > 0 Then
            Console.WriteLine(t.TableName + CUtilities.CountSummary(t.Indexes, "index") + " created")
        End If

        Console.WriteLine(t.TableName + " " + CUtilities.Timespan(DateTime.Now.Subtract(d)))
    End Sub
    Private Shared Sub TryExecute(db As CDataSrc, sql As String)
        Try
            db.ExecuteNonQuery(sql)
        Catch ex As Exception
            Console.WriteLine((sql & Convert.ToString(vbCr & vbLf)) + ex.Message + vbCr & vbLf)
        End Try
    End Sub
#End Region


#Region "Row by Row"
    Public Delegate Sub DCopyPk(pk As CValues, tx As CIterator_PkOnly.DTransform)
    Public Delegate Sub DInsert(row As CRow)
    Public Delegate Sub DUpdate(src As CRow, tar As CRow)
    Public Delegate Sub DDeleteRow(row As CRow)
    Public Delegate Sub DDeletePk(row As CValues)
    Public Delegate Sub DDeleteId(dataId As Long)
    Public Delegate Sub DSamePk(row As CValues)
    Public Delegate Sub DSameMD5(row As CRow)
    Public Delegate Sub DPackRow(row As CRow)

    'TableInfo - No delegates
    Public Shared Sub RowByRow_Full(srcDb As CDataSrc, tarDb As CDataSrc, table As CTableInfo)
        Dim c As New CGenericActions(table.TableName, srcDb, tarDb, Nothing, table.PrimaryKey.IsIdentity)
        RowByRow_Full(srcDb, tarDb, table, AddressOf c.Insert, AddressOf c.Update, AddressOf c.Delete, Nothing)
    End Sub
    Public Shared Sub RowByRow_InsertAndDelete(srcDb As CDataSrc, tarDb As CDataSrc, table As CTableInfo, pksOnly As Boolean)
        Dim g As New CGenericActions(table.TableName, srcDb, tarDb, Nothing, table.PrimaryKey.IsIdentity)
        Dim copy As DCopyPk = AddressOf g.Copy
        If Not pksOnly Then copy = Nothing 'signals full-read
        RowByRow_InsertAndDelete(srcDb, tarDb, table, AddressOf g.Insert, AddressOf g.Delete, copy, Nothing)
    End Sub

    'TableInfo - delegates supplied
    Public Shared Sub RowByRow_Full(srcDb As CDataSrc, tarDb As CDataSrc, table As CTableInfo, insert As DInsert, update As DUpdate, delete As DDeleteRow, same As DSameMD5)
        Dim tbl As String = table.TableName_
        Dim pks As List(Of String) = table.PrimaryKey.ColumnNames
        Dim cols As List(Of String) = table.Columns.Names
        Dim pageSize As Integer = CInt(IIf(table.Columns.HasVarBinary, 1, 100))

        Dim src As CIterator = CIterator.Factory(srcDb, tbl, pks, cols, Nothing, pageSize)  'todo: pagesize=1 for binary colums
        Dim tar As CIterator = CIterator.Factory(tarDb, tbl, pks, cols, Nothing, pageSize)

        RowByRow_Full(src, tar, insert, update, delete, same)
    End Sub
    Public Shared Sub RowByRow_InsertAndDelete(srcDb As CDataSrc, tarDb As CDataSrc, table As CTableInfo, insert As DInsert, delete As DDeletePk, copy As DCopyPk, same As DSamePk)
        Dim tbl As String = table.TableName_
        Dim pks As List(Of String) = table.PrimaryKey.ColumnNames
        Dim cols As List(Of String) = table.Columns.Names
        Dim pageSize As Integer = CInt(IIf(table.Columns.HasVarBinary, 1, 100))

        If Not IsNothing(copy) Then
            'pks only (mostly skip) eg blobs, or logs
            Dim src As CIterator_PkOnly = CIterator_PkOnly.Factory(srcDb, tbl, pks, Nothing, pageSize)
            Dim tar As CIterator_PkOnly = CIterator_PkOnly.Factory(tarDb, tbl, pks, Nothing, pageSize)
            RowByRow_InsertAndDelete_PksOnly(src, tar, copy, delete, same)
        Else 'ReadAll eg First read (mostly insert) 
            Dim src As CIterator = CIterator.Factory(srcDb, tbl, pks, cols, Nothing, pageSize)
            Dim tar As CIterator_PkOnly = CIterator_PkOnly.Factory(tarDb, tbl, pks, Nothing, pageSize)
            RowByRow_InsertAndDelete_FullRead(src, tar, insert, delete, same)
        End If
    End Sub

    'tablename/pk - with delegates
    Public Shared Sub RowByRow_InMemory(srcIter As CIterator, tar As CRows, insert As DInsert, update As DUpdate, delete As DDeleteId, same As DSameMD5)
        Dim src As New CRows(srcIter)
        Dim dS As Dictionary(Of Guid, CRow) = src.Dict
        Dim dt As Dictionary(Of Guid, CRow) = tar.Dict
        Dim d As CDiff_Guid = CDiffLogic.Diff(dS.Keys, dt.Keys)
        For Each i As Guid In d.TargetOnly
            Try
                delete(dt(i).DataId.Value)
            Catch
            End Try
        Next
        For Each i As Guid In d.SourceOnly
            Dim r As CRow = dS(i)
            insert(r)
        Next
        For Each i As Guid In d.Matching
            Dim s As CRow = dS(i)
            Dim t As CRow = dt(i)
            If s.DataMD5 = t.DataMD5 Then
                If Not IsNothing(same) Then same(s)
            Else
                update(s, t)
            End If
        Next
    End Sub
    Public Shared Sub RowByRow_Full(src As CIterator, tar As CIterator, insert As DInsert, update As DUpdate, delete As DDeleteRow, same As DSameMD5)
        While src.HasItem AndAlso tar.HasItem
            Dim c As Integer = src.Item.PK.CompareTo(tar.Item.PK)
            If c = 0 Then
                If src.Item.DataMD5 <> tar.Item.DataMD5 Then
                    Dim diff As CRow = src.Item.Diff_(tar.Item)
                    If Not IsNothing(update) Then update(src.Item, tar.Item)
                Else
                    If Not IsNothing(same) Then same(src.Item)
                End If
                src.NextRow()
                tar.NextRow()
            ElseIf c > 0 Then
                If Not IsNothing(delete) Then delete(tar.Item)
                tar.NextRow()
            Else
                If Not IsNothing(insert) Then insert(src.Item)
                src.NextRow()
            End If
        End While

        While src.HasItem AndAlso Not IsNothing(insert)
            insert(src.Item)
            src.NextRow()
        End While

        While tar.HasItem AndAlso Not IsNothing(delete)
            delete(tar.Item)
            tar.NextRow()
        End While
    End Sub
    Public Shared Sub RowByRow_FullHash(src As CIterator, tar As CIterator, insert As DInsert, update As DUpdate, delete As DDeleteId, same As DSameMD5)
        While src.HasItem AndAlso tar.HasItem
            Dim c As Integer = src.Item.PK.CompareTo(tar.Item.PK)
            If c = 0 Then
                If src.Item.DataMD5 <> tar.Item.Data(0).AsGuid Then
                    If Not IsNothing(update) Then update(src.Item, tar.Item)
                Else
                    If Not IsNothing(same) Then same(src.Item)
                End If
                src.NextRow()
                tar.NextRow()
            ElseIf c > 0 Then
                If Not IsNothing(delete) Then delete(tar.Item.Data(1).AsLong)
                tar.NextRow()
            Else
                If Not IsNothing(insert) Then insert(src.Item)
                src.NextRow()
            End If
        End While

        While src.HasItem AndAlso Not IsNothing(insert)
            If Not IsNothing(insert) Then insert(src.Item)
            src.NextRow()
        End While

        While tar.HasItem AndAlso Not IsNothing(delete)
            If Not IsNothing(delete) Then delete(tar.Item.Data(1).AsLong)
            tar.NextRow()
        End While
    End Sub

    Public Shared Sub RowByRow_InsertAndDelete_PksOnly(src As CIterator_PkOnly, tar As CIterator_PkOnly, copy As DCopyPk, delete As DDeletePk, same As DSamePk)
        While src.HasItem AndAlso tar.HasItem
            Dim c As Integer = src.Item.CompareTo(tar.Item) 'unpacked
            If c = 0 Then
                If Not IsNothing(same) Then same(tar.Item)
                src.NextRow()
                tar.NextRow()
            ElseIf c > 0 Then
                If Not IsNothing(delete) Then delete(tar.Item) 'packed
                tar.NextRow()
            Else
                If Not IsNothing(copy) Then copy(src.Item, src.Tx) 'unpacked
                src.NextRow()
            End If
        End While

        While src.HasItem AndAlso Not IsNothing(copy)
            copy(src.Item, src.Tx)
            src.NextRow()
        End While

        While tar.HasItem AndAlso Not IsNothing(delete)
            delete(tar.Item)
            tar.NextRow()
        End While
    End Sub
    Public Shared Sub RowByRow_InsertAndDelete_PksOnly(src As CIterator_PkOnly, tar As CIterator, copy As DCopyPk, delete As DDeletePk, same As DSamePk)
        While src.HasItem AndAlso tar.HasItem
            Dim c As Integer = src.Item.CompareTo(tar.Item.PK)
            If c = 0 Then
                If Not IsNothing(same) Then same(tar.Item.PK)
                src.NextRow()
                tar.NextRow()
            ElseIf c > 0 Then
                If Not IsNothing(delete) Then delete(tar.Item.Data) 'special logic - second pk
                tar.NextRow()
            Else
                If Not IsNothing(copy) Then copy(src.Item, src.Tx)
                src.NextRow()
            End If
        End While

        While src.HasItem AndAlso Not IsNothing(copy)
            copy(src.Item, src.Tx)
            src.NextRow()
        End While

        While tar.HasItem AndAlso Not IsNothing(delete)
            delete(tar.Item.Data) 'special logic - second pk
            tar.NextRow()
        End While
    End Sub

    Public Shared Sub RowByRow_InsertAndDelete_FullRead(src As CIterator, tar As CIterator, insert As DInsert, delete As DDeletePk, same As DSamePk)
        While src.HasItem AndAlso tar.HasItem
            Dim c As Integer = src.Item.PK.CompareTo(tar.Item.PK)
            If c = 0 Then
                If Not IsNothing(same) Then same(tar.Item.PK)
                src.NextRow()
                tar.NextRow()
            ElseIf c > 0 Then
                If Not IsNothing(delete) Then delete(tar.Item.Data) 'second pk
                tar.NextRow()
            Else
                If Not IsNothing(insert) Then insert(src.Item)
                src.NextRow()
            End If
        End While

        While src.HasItem AndAlso Not IsNothing(insert)
            insert(src.Item)
            src.NextRow()
        End While

        While tar.HasItem AndAlso Not IsNothing(delete)
            delete(tar.Item.Data)
            tar.NextRow()
        End While
    End Sub
    Public Shared Sub RowByRow_InsertAndDelete_FullRead(src As CIterator, tar As CIterator_PkOnly, insert As DInsert, delete As DDeletePk, same As DSamePk)
        While src.HasItem AndAlso tar.HasItem
            Dim c As Integer = src.Item.PK.CompareTo(tar.Item)
            If c = 0 Then
                If Not IsNothing(same) Then same(tar.Item)
                src.NextRow()
                tar.NextRow()
            ElseIf c > 0 Then
                If Not IsNothing(delete) Then delete(tar.Item)
                tar.NextRow()
            Else
                If Not IsNothing(insert) Then insert(src.Item)
                src.NextRow()
            End If
        End While

        While src.HasItem AndAlso Not IsNothing(insert)
            insert(src.Item)
            src.NextRow()
        End While

        While tar.HasItem AndAlso Not IsNothing(delete)
            delete(tar.Item)
            tar.NextRow()
        End While
    End Sub




#End Region

End Class
