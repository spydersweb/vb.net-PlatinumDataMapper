﻿Imports System.Runtime.Serialization

Public Class InvalidAccountException
    Inherits Exception

    Public Sub New()
        MyBase.New("Account name not set exception!")
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class
