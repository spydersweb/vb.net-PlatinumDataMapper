Public Class StockImages
    Inherits Catalyst

    Sub New(Dealer_Account As String, _
            Dealer_Password As String)

        Try

            Me.Account = Dealer_Account
            Me.Password = Dealer_Password
            addToParamArray("request", "STI")
            Me.Filename = "imageExport"
            Me.Version = 11
            Me.Format = "XML"

            Me.download()

        Catch ex As Exception
            Throw ex
        End Try


    End Sub

End Class
