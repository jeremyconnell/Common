Imports System.Text

' Supports any CDataSrc including CWebSrc
' Uses data readers internally, unless the driver is CWebSrc (otherwise datasets)
' Calls to datareader functions will throw an error if the driver is CWebSrc (AppCode should use Business Objects or DataSets)
<Serializable(), CLSCompliant(True)> _
Public MustInherit Class CBaseSmart : Inherits CBaseDynamic

#Region "Constants"
    Public Shared CONCURRENCY_CHECK As Boolean = Not CApplication.IsWebApplication 'Web-app shares cache instances anyway 'Chatty, extra db hit, suits winform talking to local db or using remoting via WS
#End Region

#Region "Constructors"
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

#Region "Members"
    Friend m_data As Dictionary(Of String, Object)
    Protected m_copy As Dictionary(Of String, Object)
#End Region

#Region "Public - Save/Delete, Import/Export"
    Public Overridable Overloads Sub Save(ByVal txOrNull As IDbTransaction)
        SyncLock Me
            If m_insertPending Then
                Insert(txOrNull)
                m_insertPending = False

                CacheInsert()
            Else
                If Update(txOrNull) > 0 Then CacheUpdate()
            End If
        End SyncLock
    End Sub
    Public Overridable Sub Import(ByVal data As Dictionary(Of String, Object))
        SyncLock Me
            m_data = New Dictionary(Of String, Object)(data)
            m_copy = New Dictionary(Of String, Object)(data)
        End SyncLock
    End Sub
    Public Function Export() As Dictionary(Of String, Object)
        Return m_data
    End Function
#End Region

#Region "Protected Properties"
    'Standard Columns
    Protected Overrides Property PrimaryKeyValue() As Object    'Protected - Public version has specific name and is readonly
        Get
            Return Read(PrimaryKeyName, Nothing)
        End Get
        Set(ByVal Value As Object)
            If Not InsertPrimaryKey Then
                If TypeOf Value Is Decimal Then
                    Value = CInt(Value.ToString)
                End If
            End If
            Write(PrimaryKeyName, Value)
        End Set
    End Property

    'Init
    Protected Overrides Sub InitValues()
        m_data = New Dictionary(Of String, Object)
        m_copy = New Dictionary(Of String, Object)
    End Sub

    'Column Data
    Protected Function ReadDateTime(ByVal columnName As String) As DateTime
        Return CType(Read(columnName, DateTime.MinValue), DateTime)
    End Function
    Protected Function ReadDouble(ByVal columnName As String) As Double
        Return CType(Read(columnName, Double.NaN), Double)
    End Function
    Protected Function ReadInteger(ByVal columnName As String) As Integer
        Return CType(Read(columnName, Integer.MinValue), Integer)
    End Function
    Protected Function ReadGuid(ByVal columnName As String) As Guid
        Return CType(Read(columnName, Guid.Empty), Guid)
    End Function
    Protected Function ReadString(ByVal columnName As String) As String
        Return CType(Read(columnName, String.Empty), String)
    End Function
    Protected Function ReadBoolean(ByVal columnName As String) As Boolean
        Return CType(Read(columnName, False), Boolean)
    End Function
    Protected Function ReadDecimal(ByVal columnName As String) As Decimal
        Return CType(Read(columnName, Decimal.MinValue), Decimal)
    End Function
    Protected Function Read(ByVal columnName As String) As Object
        Return Read(columnName, Nothing)
    End Function
    Protected Function Read(ByVal columnName As String, ByVal defaultValue As Object) As Object
        Return Read(m_data, columnName, defaultValue)
    End Function
    Protected Shared Function Read(ByVal data As Dictionary(Of String, Object), ByVal columnName As String, ByVal defaultValue As Object) As Object
        Dim original As String = columnName
        columnName = LCase(columnName)

        If Not data.ContainsKey(columnName) Then Return defaultValue

        Dim obj As Object = data(columnName)

        'Mysql cases
        If TypeOf obj Is String Then
            If TypeOf defaultValue Is Guid Then Return New Guid(CStr(obj)) Else Return obj
        End If
        If TypeOf obj Is SByte Then Return (obj.ToString = "1")

        'Null-equivalent values
        If TypeOf obj Is System.DBNull Or IsNothing(obj) Then Return defaultValue

        Return obj
    End Function
    Protected Sub Write(ByVal columnName As String, ByVal val As Object)
        m_data(LCase(columnName)) = val
    End Sub
#End Region

#Region "Protected - Load Logic"
    'Internal storage
    Protected Overloads Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_data.Clear()
        PrimaryKeyValue = dr.Item(PrimaryKeyName)
        Dim i As Integer
        For i = 0 To dr.FieldCount - 1
            If Not dr.IsDBNull(i) Then m_data(LCase(dr.GetName(i))) = dr.Item(i)
        Next
        m_copy = New Dictionary(Of String, Object)(m_data)
    End Sub
    Protected Overloads Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_data.Clear()
        PrimaryKeyValue = dr.Item(PrimaryKeyName)
        Dim i As DataColumn
        For Each i In dr.Table.Columns
            If Not dr.IsNull(i) Then m_data(LCase(i.ColumnName)) = dr.Item(i)
        Next
        m_copy = New Dictionary(Of String, Object)(m_data)
    End Sub
    Protected Sub Import(ByVal mergeWith As CBaseSmart)
        m_data = mergeWith.m_data
        m_copy = mergeWith.m_copy
    End Sub
#End Region

#Region "Data Acces Layer - Dynamic Sql"

#Region "Insert/Update (Protected)"
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Return New CNameValueList(m_data)
    End Function
    Protected Overrides Sub Insert(ByVal txOrNull As IDbTransaction)
        'Extra column
        If InsertPrimaryKey Then
            If IsNothing(PrimaryKeyValue) Then Throw New Exception("Primary key '" & PrimaryKeyName & "' was not set before insert operation, and InsertPrimaryKey=True for " & Me.TableName)
        End If

        'Get the autonumber
        If Not InsertPrimaryKey Then 'Note: This fails for db-generated guids, override insert with a stored procedure or set InsertPrimaryKey=true
            PrimaryKeyValue = DataSrc.Insert(Me.TableName, Me.PrimaryKeyName, Me.InsertPrimaryKey, ColumnNameValues, txOrNull, Me.OracleSequenceName)
        Else
            Dim rowsAffected As Integer = CType(DataSrc.Insert(Me.TableName, Me.PrimaryKeyName, Me.InsertPrimaryKey, ColumnNameValues, txOrNull, Me.OracleSequenceName), Integer)
            If 1 <> rowsAffected Then Throw New Exception(rowsAffected & " rows affected after insert")
        End If

        m_copy = New Dictionary(Of String, Object)(m_data)
    End Sub
    Protected Overrides Function Update(ByVal txOrNull As IDbTransaction) As Integer
        'Only update cols that changed
        Dim updateColNames As CNameValueList = GetColumsToUpdate()

        'No action taken if no changes made
        If updateColNames.Count = 0 Then Return -1

        'Special case where primary key is updatable (dont allow that)
        Dim pKeyValue As Object = PrimaryKeyValue
        If InsertPrimaryKey Then pKeyValue = m_copy(LCase(PrimaryKeyName))

        'Perform check on fields to be updated (throws custom ex)
        If CONCURRENCY_CHECK Then CheckForConflicts(updateColNames, txOrNull)

        'Commit the changes
        If updateColNames.Count > 0 Then
            Dim where As New CWhere(Me.TableName, Me.PrimaryKeys, txOrNull)
            Dim rowsAffected As Integer = DataSrc.Update(updateColNames, where)
            If 1 <> rowsAffected Then Throw New Exception(rowsAffected & " rows affected after update")
            Return rowsAffected
        End If

        Return -1
    End Function
    Private Function GetColumsToUpdate() As CNameValueList
        Dim updateColNames As New CNameValueList(m_data.Count)
        Dim name As String, value As Object
        For Each name In m_data.Keys
            'Don't include the primary key
            If String.Compare(name, PrimaryKeyName, True) = 0 Then Continue For
            value = m_data(name)

            'Dont include values that werent changed
            If Not IsNothing(m_copy) Then
                If m_copy.ContainsKey(name) Then
                    If m_copy(name).Equals(value) Then Continue For Else 
                End If
            End If

            'Special treatment of memo columns
            updateColNames.Add(name, value)
        Next
        Return updateColNames
    End Function
    Private Sub CheckForConflicts(ByVal updateColNames As CNameValueList, ByVal txOrNull As IDbTransaction)
        'Concurrency Handling: 
        '1. Read current db state
        Dim currentState As CBaseSmart = CType(Me.SelectId(txOrNull), CBaseSmart)
        Dim currentDbData As Dictionary(Of String, Object) = currentState.m_data
        '2. Compare current db state with last known state, replacing if conflict found
        Dim conflicts As New Dictionary(Of String, Object)(updateColNames.Count)
        For Each i As CNameValue In updateColNames
            CheckForConflict(i.Name, currentDbData, conflicts)
            currentDbData.Remove(i.Name)
        Next
        '3. Pull back any other fields from the database (ones not involved in the update)
        For Each i As String In currentDbData.Keys
            m_data(i) = currentDbData(i)
            m_copy(i) = currentDbData(i)
        Next
        '4. Throw custom exception if conflicts found (UI must handle)
        If conflicts.Count > 0 Then
            CacheClear()
            Throw New CConflictException(conflicts, m_data)
        End If
    End Sub
    Private Sub CheckForConflict(ByVal i As String, ByVal currentDbData As Dictionary(Of String, Object), ByVal conflicts As Dictionary(Of String, Object))
        Dim conflict As Boolean = False

        'Only check non-null dbfields for conflicts (null ones not checked as info trumps no-info)
        If Not currentDbData.ContainsKey(i) Then Exit Sub

        'For new fields (were null)
        If Not m_copy.ContainsKey(i) Then
            Exit Sub
        End If

        'Dont trust dates
        If TypeOf currentDbData(i) Is DateTime Then m_copy(i) = currentDbData(i)

        'Verify that data is known and unchanged
        If m_copy(i).Equals(currentDbData(i)) Then
            'No conflict, so update internal copy
            m_copy(i) = m_data(i)
            Exit Sub
        End If

        'Conflict - record, pull back data from database
        conflicts.Add(i, m_data(i))
        m_data(i) = currentDbData(i)
        m_copy(i) = currentDbData(i)
    End Sub
#End Region

#End Region

#Region "Default Property (Item)"
    Default Public Property Item(ByVal key As String) As Object
        Get
            Return Read(key)
        End Get
        Set(ByVal value As Object)
            Write(key, value)
        End Set
    End Property
#End Region

End Class

