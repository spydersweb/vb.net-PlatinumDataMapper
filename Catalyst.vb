Imports eventLogger.EventLogger
Imports System
Imports System.IO
Imports System.Net
Imports System.Xml
Imports System.Text

<CLSCompliant(True)>
Public MustInherit Class Catalyst

    Protected base_account As String
    Protected base_password As String
    Protected base_requesttype As String
    Protected base_catalyst_find_it As Uri = New Uri("https://www.catalyst-findit.co.uk/download.php")
    Protected base_request As HttpWebRequest
    Protected base_response As HttpWebResponse = Nothing
    Protected base_byte_data() As Byte
    Protected base_post_stream As Stream = Nothing
    Protected base_reader As StreamReader
    Protected base_writer As StreamWriter
    Protected base_requestXML As XmlDocument = New XmlDocument()
    Protected base_params As New Dictionary(Of String, Object)

    Private base_version As Integer
    Private base_format As String = ""
    Protected base_vehicle_types As ArrayList

    Protected base_file_name As String = "download"
    Protected base_file_extension As String = "xml"

    ' Event Logger variable
    Public el As eventLogger.EventLogger


    Public WriteOnly Property Account() As String
        Set(value As String)
            If Not IsNothing(value) And value <> "" Then
                addToParamArray("account", value)
            Else
                Throw New ArgumentNullException("Invalid Null Value Parameter!", "account")
            End If
        End Set
    End Property

    Public WriteOnly Property Password() As String
        Set(value As String)
            If Not IsNothing(value) And value <> "" Then
                addToParamArray("password", value)
            Else
                Throw New ArgumentNullException("Invalid Null Value Parameter!", "password")
            End If
        End Set
    End Property

    Public WriteOnly Property Version() As Integer
        Set(value As Integer)
            If Not IsNothing(value) Then
                If IsNumeric(value) Then
                    addToParamArray("version", value.ToString)
                Else
                    Throw New ArgumentNullException("Incorrect parameter type!", "version")
                End If
            Else
                Throw New ArgumentNullException("Invalid Null Value Parameter!", "version")
            End If
        End Set
    End Property

    Public WriteOnly Property Format() As String
        Set(value As String)
            If Not IsNothing(value) Then
                Select Case value.ToUpper
                    Case "XML", "CSV"
                        addToParamArray("format", value)
                        base_file_extension = value.ToLower
                    Case Else
                        Throw New ArgumentException("Invalid Format!", "format")
                End Select
            Else
                Throw New ArgumentNullException("Invalid Null Value Parameter!", "version")
            End If
        End Set
    End Property

    Public ReadOnly Property XMLrequest() As String
        Get
            Return base_requestXML.InnerXml.ToString
        End Get
    End Property

    Public WriteOnly Property Filename() As String
        Set(value As String)

            If InStr(value, ".") > 0 Then
                base_file_name = Mid(value, 1, InStr(value, ".") - 1)
            End If


        End Set
    End Property

    Public WriteOnly Property Dealercode() As String
        Set(value As String)

            addToParamArray("dealer", value)

        End Set
    End Property

    Sub New()
        base_requestXML.LoadXml("<?xml version=""1.0"" encoding=""utf-8"" ?><download />")

        base_vehicle_types = New ArrayList
        base_vehicle_types.Add("MC")
        base_vehicle_types.Add("CAR")
        base_vehicle_types.Add("CV")
        base_vehicle_types.Add("MH")
        base_vehicle_types.Add("AGM")
        base_vehicle_types.Add("BOA")
        base_vehicle_types.Add("ATV")
        base_vehicle_types.Add("TRU")
        base_vehicle_types.Add("COM")
        base_vehicle_types.Add("TQC")
        base_vehicle_types.Add("CAC")
    End Sub


    Protected Sub checkParams()

        Dim required As Array = {"account", "password", "version", "request", "format"}

        For Each k As String In required
            If Not base_params.ContainsKey(k) Then
                Throw New ArgumentException("Required parameters are missing!", k)
            End If
        Next

        For Each pair As KeyValuePair(Of String, Object) In base_params
            base_requestXML.DocumentElement.SetAttribute(pair.Key.ToString, pair.Value.ToString)
        Next

    End Sub

    Protected Sub addToParamArray(k As String, v As Object)
        If Not base_params.ContainsKey(k) Then
            base_params.Add(k, v)
        Else
            base_params(k) = v
        End If
    End Sub


    Public Sub download()

        checkParams()


        Try

            base_request = DirectCast(WebRequest.Create(base_catalyst_find_it), HttpWebRequest)
            base_request.Method = "POST"
            base_request.ContentType = "application/xml"
            base_byte_data = UTF8Encoding.UTF8.GetBytes(base_requestXML.InnerXml.ToString)
            base_request.ContentLength = base_byte_data.Length


            base_post_stream = base_request.GetRequestStream()
            base_post_stream.Write(base_byte_data, 0, base_byte_data.Length)
            base_response = DirectCast(base_request.GetResponse(), HttpWebResponse)
            base_reader = New StreamReader(base_response.GetResponseStream())

            ' Ditch any extension that is provided 
            base_writer = New StreamWriter(base_file_name & "." & base_file_extension)

            Do While Not base_reader.EndOfStream
                base_writer.WriteLine(base_reader.ReadLine)
            Loop

        Catch ex As FileNotFoundException
            el.WriteToEventLog(ex.StackTrace & vbCrLf & ex.Message, "Find It Download", EventLogEntryType.Error, "CatalystDownload")
            Exit Sub
        Catch ex As IOException
            el.WriteToEventLog(ex.StackTrace & vbCrLf & ex.Message, "Find It Download", EventLogEntryType.Error, "CatalystDownload")
            Exit Sub
        Catch ex As WebException
            el.WriteToEventLog(ex.StackTrace & vbCrLf & ex.Message, "Find It Download", EventLogEntryType.Error, "CatalystDownload")
            Exit Sub
        Catch ex As Exception
            el.WriteToEventLog(ex.StackTrace & vbCrLf & ex.Message, "Find It Download", EventLogEntryType.Error, "CatalystDownload")
            Exit Sub
        Finally
            base_reader.Close()
            base_reader.Dispose()

            base_writer.Close()
            base_writer.Dispose()
        End Try

    End Sub

End Class
