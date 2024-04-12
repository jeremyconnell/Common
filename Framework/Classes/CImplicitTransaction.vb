Imports System.Web

'Notes:
'       This class used used as an ALTERNATIVE APPROACH TO TRANSACTIONS. It is not the recommended approach, but may be useful in some situations
'       It complements the standard transactions but does not replace them
'       It does not support nested transactions (unless they are standard transactions)
'       It is used a bit like TransactionScope, but doesnt require MSDTC
'       Use when you have a deeply-nested function call involving relationships to other tables (selects), and you can't be bothered implementing the transaction overloads for all methods concerned
'
'Usage (VB):
'       With CImplicitTransaction.CreateNew()
'          Try
'              ... code goes here ..
'             .Commit()
'          Catch
'              .Rollback()
'              Throw
'          Finally
'              .Close()
'          End Try
'       End With
'
'Usage (C#):
'       using (CImplicitTransaction tx = CImplicitTransaction.CreateNew())
'       {
'           try
'           {
'               ... code goes here ..
'               tx.Commit();
'           }
'           catch
'           {
'              tx.Rollback();
'              throw;
'           }
'       }

Public Class CImplicitTransaction : Implements IDisposable

#Region "Constructor"
    Private Sub New(ByRef connection As IDbConnection)
        If (connection Is Nothing) Then
            m_connection = CDataSrc.Default.Local.Connection()
        Else
            m_connection = connection
        End If
        m_transaction = m_connection.BeginTransaction()
    End Sub
    Protected Sub Dispose() Implements IDisposable.Dispose
        Close()
    End Sub
#End Region

#Region "Constants"
    Private Const KEY As String = "CurrentImplicitTransaction" 'Instance stored in web-request-scoped collection (if web app)
#End Region

#Region "Members"
    Protected m_connection As IDbConnection
    Protected m_transaction As IDbTransaction

    'Only used if not a web application
    <ThreadStatic()> Private Shared m_singleton As CImplicitTransaction
#End Region

#Region "Interface"
    'Shared methods
    Public Shared Function CreateNew() As CImplicitTransaction
        Return CreateNew(Nothing)
    End Function
    Public Shared Function CreateNew(ByRef connection As IDbConnection) As CImplicitTransaction
        If Not IsNothing(Current) Then Throw New Exception("A CImplicitTransaction already exists in scope - use that instead")
        Singleton = New CImplicitTransaction(connection)
        Return Singleton
    End Function

    'Instance methods
    Public Sub Commit()
        If Not IsNothing(m_transaction) Then m_transaction.Commit()
    End Sub
    Public Sub Rollback()
        If Not IsNothing(m_transaction) Then m_transaction.Rollback()
    End Sub
    Public Sub Close()
        If Not IsNothing(m_connection) AndAlso Not m_connection.State = ConnectionState.Closed Then
            m_connection.Close()
        End If
        Singleton = Nothing
    End Sub
#End Region

#Region "Properties"
    Private Property Connection() As IDbConnection
        Get
            Return m_connection
        End Get
        Set(ByVal value As IDbConnection)
            m_connection = value
        End Set
    End Property
    Public Property Transaction() As IDbTransaction
        Get
            Return m_transaction
        End Get
        Set(ByVal value As IDbTransaction)
            m_transaction = value
        End Set
    End Property
#End Region

#Region "Properties - Shared"
    'Transaction-Context Storage
    Public Shared ReadOnly Property Current() As CImplicitTransaction 'Public getter, private setter
        Get
            Return Singleton
        End Get
    End Property
    Private Shared Property Singleton() As CImplicitTransaction
        Get
            If IsNothing(HttpContext.Current) Then Return m_singleton
            If Not HttpContext.Current.Items.Contains(KEY) Then Return Nothing
            Return CType(HttpContext.Current.Items(KEY), CImplicitTransaction)
        End Get
        Set(ByVal value As CImplicitTransaction)
            If IsNothing(HttpContext.Current) Then
                m_singleton = value
            Else
                If value Is Nothing Then
                    If HttpContext.Current.Items.Contains(KEY) Then HttpContext.Current.Items.Remove(KEY)
                Else
                    HttpContext.Current.Items(KEY) = value
                End If
            End If
        End Set
    End Property
#End Region

End Class
