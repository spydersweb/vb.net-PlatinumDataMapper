Public Class StockLevels
    Inherits Catalyst

    Sub New(Dealer_Account As String, _
           Dealer_Password As String, _
           FileName As String)

        Try

            Me.Account = Dealer_Account
            Me.Password = Dealer_Password
            addToParamArray("request", "STL")
            Me.Filename = FileName
            Me.Version = 11
            Me.Format = "XML"

            Me.download()

        Catch ex As Exception
            Throw ex
        End Try


    End Sub

End Class
