Imports System
Imports System.IO
Imports System.Text
Imports System.Net

Public Class Import
    Inherits Catalyst

    Sub New(uploadFile As String)
        base_catalyst_find_it = New Uri("http://www.catalyst-findit.co.uk/upload")

        Try
            If File.Exists(uploadFile) Then
                base_requestXML.LoadXml(uploadFile)
            End If
            upload()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub upload()
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
