Imports System.Threading

Module Module1

    Public SourceFolder As String
    Public Destination As String
    Public CallbackExe As String

    Sub Main()
        Try
            If IO.File.Exists(LogPath) Then IO.File.Delete(LogPath)
            If IO.File.Exists(LogPath.Replace("output", "errors")) Then IO.File.Delete(LogPath.Replace("output", "errors"))
        Catch
        End Try

        Try
            Dim args As String() = Environment.GetCommandLineArgs()
            If args.Length > 1 Then SourceFolder = args(1)
            If args.Length > 2 Then Destination = args(2)
            If args.Length > 3 Then CallbackExe = args(3)

            Log("SourceFolder: ", SourceFolder)
            Log("Destination: ", Destination)
            Log("CallbackExe: ", CallbackExe)

            Dim attempts As Integer = 0
            While attempts < 10
                attempts += 1
                System.Threading.Thread.Sleep(1000)
                Try
                    Log("Attempting to copy files - attempt #", attempts)
                    CopyFiles()
                    Log("Succeeded in copying files, launching: ", CallbackExe)
                    Process.Start(CallbackExe)
                    Return
                Catch ex As Exception
                    Log(ex)
                End Try
            End While
            Log("End While")
        Catch ex As Exception
            Log(ex)
        End Try
        System.Threading.Thread.Sleep(10000)
    End Sub

    Private Sub CopyFiles()
        Dim files As String() = IO.Directory.GetFiles(SourceFolder)
        Log("Copying ", files.Length, " files")
        For Each s As String In files
            Dim fileName As String = IO.Path.GetFileName(s)
            Log(fileName)
            Dim oldFile As String = Destination & "\" & fileName
            If IO.File.Exists(oldFile) Then IO.File.Delete(oldFile)
            IO.File.Move(s, oldFile)
        Next
    End Sub

    Private Sub Log(ByVal ParamArray msg As Object())
        Dim s As String = String.Concat(DateTime.Now, vbTab, String.Concat(msg))
        AppendLine(LogPath, s)
    End Sub
    Private Sub Log(ByVal ex As Exception, ByVal ParamArray msg As Object())
        Dim s As String = String.Concat(DateTime.Now, vbTab, String.Concat(String.Concat(msg), vbCrLf, ex, vbCrLf))
        AppendLine(ErrorPath, s)
    End Sub
    Private Sub AppendLine(ByVal filePath As String, ByVal message As String)
        Try
            Console.WriteLine(message)
            IO.File.AppendAllText(filePath, String.Concat(message, vbCrLf))
        Catch
            Thread.Sleep(1000) 'wait a sec
            Try
                Console.WriteLine(message)
                IO.File.AppendAllText(filePath, String.Concat(message, vbCrLf))
            Catch
                Thread.Sleep(1000) 'wait a sec

                'Third time lucky or fail quietly
                Console.WriteLine(message)
                Try
                    IO.File.AppendAllText(filePath, String.Concat(message, vbCrLf))
                Catch
                End Try
            End Try
        End Try
    End Sub
    Private Function LogPath() As String
        Return String.Concat(My.Application.Info.DirectoryPath, "/output(upgrade).txt")
    End Function
    Private Function ErrorPath() As String
        Return String.Concat(My.Application.Info.DirectoryPath, "/output(upgrade).txt")
    End Function

End Module
