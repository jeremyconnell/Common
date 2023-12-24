Imports Framework
Imports SchemaAudit

Module Module1
    Public Const SQL As String = "SELECT * FROM " & CAudit_Type.TABLE_NAME

    'Note: This demo is configured to use a virtual folder called "website" on localhost. Add the port number to use Visual Studio webserver.
    Sub Main()
        '#1 OLD WEBSERVICE: Easy to configure, url read from appSettings by framework code
        Dim oldWebservice As CWebSrcSoap = CDataSrc.Default 'See also CWebSrcBinary, both can be generalised as a CWebSrc
        Dim ds As DataSet = oldWebservice.ExecuteDataSet(SQL) 'Low-level query


        '#2 NEW WCF SERVICE: Url is configured in app.config in WCF section
        Dim newWcfWebservice As New CWebSrcWcf()

        'Have to set default-source manually for WCF, because implementation resides in the application layer.
        'Do this (on application start-up) if using generated business objects, so you can use the empty constructor on the business objects
        CDataSrc.Default = newWcfWebservice

        'Do the same query using wcf. Datasets may exceed the default limit of 65535 bytes, so have added 2 zeros to limit in config file
        ds = newWcfWebservice.ExecuteDataSet(SQL) 'Low-level query


        '#3 Now try high-level objects (cached/non-cached) using the current default datasrc (this code runs on any driver or db platform)
        '1. low-level  query using high-level object
        ds = New CAudit_Type().SelectAll_Dataset()
        '2. high-level query using high-level object (cached)
        Dim types As CAudit_TypeList = CAudit_Type.Cache
        '3. high-level query using high-level object (non-cached)
        Dim last10 As CAudit_TrailList = New CAudit_Trail().SelectAll(New CPagingInfo(10))
        '4. unit testing - update/insert/delete
        Dim t As CAudit_Type = types(0)
        Dim temp = t.TypeName
        t.TypeName = "test update"
        t.Save()
        t.TypeName = temp
        t.Save()
        t = New CAudit_Type()
        t.TypeName = "test insert"
        t.Save()
        t.Delete()
    End Sub

End Module
