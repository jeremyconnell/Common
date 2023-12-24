Imports System.ServiceModel

' NOTE: If you change the class name "ITest" here, you must also update the reference to "ITest" in Web.config.
<ServiceContract()> _
Public Interface ITest

    <OperationContract()> _
    Sub DoWork()

End Interface
