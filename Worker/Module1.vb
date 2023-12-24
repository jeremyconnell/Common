Imports Comms.Upgrade.Client
Imports Comms.Upgrade.Interface

Imports framework

Module Module1

    Sub Main(args As String())
        'Self-config
        CUpgradeClient.Config_.WriteConfigSetting_Encryption(EEncryption.Rij)
        CUpgradeClient.Config_.WriteConfigSetting_HostName("cutplan.fabric-utilization.co.uk")

        'Self-upgrade
        If SelfUpgrade() Then Exit Sub

        'Continuous work loop (single job thread)
        While True
            DoWork()
        End While
    End Sub

    Private Function SelfUpgrade() As Boolean
        Dim args As String() = My.Application.CommandLineArgs.ToArray
        Try
            Dim changes As CSummary = CUpgradeClient.Factory.DoUpgrade_ThisExe(1, My.Computer.Name)
            If changes.Count = 0 Then Return False

            Console.WriteLine("Upgrading: " & changes.AddEdits.Count & " add/edits; " & changes.Deletes.Count & " deletes")
            Dim dest As String = My.Application.Info.DirectoryPath & "\"
            Dim source As String = dest & "upgrade_files\"
            Dim callback As String = args(0)

            Dim sb As New Text.StringBuilder
            sb.Append("""").Append(source).Append(""" """).Append(dest).Append(""" """).Append(callback).Append("""")
            Process.Start("upgrade.exe", sb.ToString)

            Return True
        Catch ex As Exception
            Console.WriteLine("Upgrade failed: " & ex.Message)
            Return False
        End Try
    End Function

    Private Sub DoWork()
        Threading.Thread.Sleep(1000)
        Console.WriteLine(DateTime.Now.ToString)
    End Sub

End Module
