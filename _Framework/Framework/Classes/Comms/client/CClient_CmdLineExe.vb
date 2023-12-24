'Some specifics about the local executable to call
Public MustInherit Class CClient_CmdLineExe : Implements IPassthru

#Region "Properties"
    Private m_pathToExe As String
    Private m_tempFolder As String
#End Region

#Region "Constructors"
    Public Sub New(pathToExe As String, tempFolder As String)
        MyBase.New()
        m_pathToExe = pathToExe
        m_tempFolder = tempFolder
    End Sub
#End Region

#Region "Overrides"
    Protected MustOverride ReadOnly Property Config As IConfig
#End Region

#Region "Helpful Overloads - still need to deserialse the output object (as type must be known, and is method-specific)" '*Shadow the enum type in a derived class
    Public Function Execute(enum_ As Integer, ParamArray input As Object()) As Byte() 'Most-commonly used
        Return Execute(enum_, EGzip.None, input)
    End Function
    Public Function Execute(enum_ As Integer, gzip As EGzip, ParamArray input As Object()) As Byte()
        Return Execute(enum_, gzip, EEncryption.None, input)
    End Function
    Public Function Execute(enum_ As Integer, gzip As EGzip, encryption As EEncryption, ParamArray input As Object()) As Byte()
        Return Execute(enum_, gzip, encryption, ESerialisation.Protobuf, ESerialisation.Protobuf, input)
    End Function
    Public Function Execute(enum_ As Integer, gzip As EGzip, encryption As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation, ParamArray input As Object()) As Byte()
        Dim data As Byte() = CSerialise.Pack(input, formatIn)
        Return TransportInterface(enum_, data, gzip, encryption, formatIn, formatOut)
    End Function
#End Region

#Region "EXE Implementation"
    Public Function TransportInterface(enum_ As Integer, input() As Byte, gzip As EGzip, encrypt As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation) As Byte() Implements IPassthru.TransportInterface
        'Compress/encrypt (unlikely to be used with a local exe)
        input = CGzip.Compress(input, gzip, True)
        input = Config.Encrypt(input, encrypt)

        'Prepare the input/output location (filesystem)
        Dim tempFolder As String = String.Concat(m_tempFolder, "/", Guid.NewGuid.ToString, "/")
        Dim inputFilePath As String = String.Concat(tempFolder, "input.bin")
        Dim outputFilePath As String = String.Concat(tempFolder, "output.bin")
        IO.Directory.CreateDirectory(tempFolder)
        IO.File.WriteAllBytes(inputFilePath, input)

        'Make the system call, collect the output
        Dim output As Byte() = Nothing
        Try

            Dim p As New Process
            With p.StartInfo
                .WindowStyle = ProcessWindowStyle.Hidden
                .FileName = m_pathToExe
                .Arguments = String.Concat(CInt(enum_), " """, inputFilePath, """ """, outputFilePath, """ ", CInt(gzip), " ", CInt(encrypt), " ", CInt(formatIn), " ", CInt(formatOut))
                .UseShellExecute = False
                If Environment.OSVersion.Version.Major >= 6 Then .Verb = "runas"
            End With
            p.Start()
            p.WaitForExit()
            If p.ExitCode <> 0 Then Throw New Exception(String.Concat("Non-zero return-code from '", m_pathToExe, "'"))
            If Not IO.File.Exists(outputFilePath) Then Throw New Exception(String.Concat("No output file returned from '", m_pathToExe, "'"))
            output = IO.File.ReadAllBytes(outputFilePath)
        Catch ex2 As Exception
            Throw New Exception("Failed to execute " & inputFilePath & ": " & ex2.Message)
        Finally
            IO.Directory.Delete(tempFolder, True) 'cleanup filesystem
        End Try

        'Decompress/Decrypt the output (unlikely with an exe)
        output = Config.Decrypt(output, encrypt)
        output = CGzip.Decompress(output, gzip, False)

        'Deserialise & throw any exceptions
        Dim ex As CException = Nothing
        Try
            ex = CSerialise.Deserialise(Of CException)(output, formatOut)
        Catch
        End Try
        If Not IsNothing(ex) AndAlso Not IsNothing(ex.Message) AndAlso Not IsNothing(ex.StackTrace) Then Throw New CDeserialisedException(ex)

        Return output
    End Function
#End Region


End Class
