' NOTE: If you change the class name "Test" here, you must also update the reference to "Test" in Web.config and in the associated .svc file.
Public Class Test
    Implements ITest

    Public Sub DoWork() Implements ITest.DoWork
    End Sub

End Class
