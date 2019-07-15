Imports System.Net
Imports System.IO
Imports System.Text

<CLSCompliant(True)>
Public Class Export
    Inherits Catalyst

    Private _changes As Boolean = False
    Private _dealer_code As String
    Private _vehicles As Boolean = False
    Private _new_vehicles As Boolean = False
    Private _customers As Boolean = False
    Private _items As Boolean = False
    Private _images As Boolean = False


#Region "Public Properties"
    

    



    

    

    Public WriteOnly Property Changes() As String
        Set(value As String)

            If value.ToUpper = "Y" Or (IsNumeric(value) And (Len(value) = 8 Or Len(value) = 14)) Then
                addToParamArray("changes", value)
            Else
                Throw New ArgumentException("Invalid Parameter Value!", "change")
            End If

        End Set
    End Property

    

    Public WriteOnly Property Vehicles() As Boolean
        Set(value As Boolean)

            If value Then
                addToParamArray("vehicles", "Y")
            Else
                addToParamArray("vehicles", "N")
            End If

        End Set
    End Property

    Public WriteOnly Property NewVehicles() As Boolean
        Set(value As Boolean)

            If value Then
                addToParamArray("new-vehicles", "Y")
            End If

        End Set
    End Property

    Public WriteOnly Property VehicleTypes() As Array
        Set(value As Array)
            Dim vt As String = ""
            If IsArray(value) Then
                For Each ele As String In value
                    If Not base_vehicle_types.Contains(ele) Then
                        Throw New ArgumentOutOfRangeException("VehicleTypes", ele, "Unexcpected Vehicle Type Exception")
                    End If
                    vt &= ele & " "
                Next
                addToParamArray("vehicles-types", vt)
            Else
                Throw New ArgumentOutOfRangeException("VehicleTypes", "Invalid Type, Array expected")
            End If
        End Set
    End Property

    Public WriteOnly Property Customers() As Boolean
        Set(value As Boolean)

            If value Then
                addToParamArray("customers", "Y")
            Else
                addToParamArray("customers", "N")
            End If

        End Set
    End Property

    Public WriteOnly Property Items() As Boolean
        Set(value As Boolean)

            If value Then
                addToParamArray("items", "Y")
            Else
                addToParamArray("items", "N")
            End If

        End Set
    End Property

    Public WriteOnly Property Images() As Boolean
        Set(value As Boolean)

            If value Then
                addToParamArray("images", "Y")
            Else
                addToParamArray("images", "N")
            End If

        End Set
    End Property

    

    
#End Region




#Region "Custom Constructor"
    
    Sub New(Dealer_Account As String, _
            Dealer_Password As String, _
            Optional ByVal Version_Number As Integer = 11, _
            Optional ByVal Export_Format As String = "XML", _
            Optional ByVal Changes_Only As String = Nothing, _
            Optional ByVal Dealer_Code As String = Nothing, _
            Optional ByVal Export_Vehicles As Boolean = Nothing, _
            Optional ByVal New_Vehicles_Only As Boolean = Nothing, _
            Optional ByVal Vehicle_Types As Array = Nothing, _
            Optional ByVal Export_Customers As Boolean = Nothing, _
            Optional ByVal Export_Items As Boolean = Nothing, _
            Optional ByVal Export_Images As Boolean = Nothing)


        


        Try

            addToParamArray("request", "EXP")

            Me.Account = Dealer_Account
            Me.Password = Dealer_Password

            If Not IsNothing(Version_Number) Then
                Me.Version = Version_Number
            End If

            If Not IsNothing(Export_Format) Then
                Me.Format = Export_Format
            End If

            If Not IsNothing(Changes_Only) Then
                Me.Changes = Changes_Only
            End If

            If Not IsNothing(Dealer_Code) Then
                Me.Dealercode = Dealer_Code
            End If

            If Export_Vehicles Then
                Me.Vehicles = Export_Vehicles
            End If

            If New_Vehicles_Only Then
                Me.NewVehicles = New_Vehicles_Only
            End If

            If Not IsNothing(Vehicle_Types) Then
                Me.VehicleTypes = Vehicle_Types
            End If

            If Export_Customers Then
                Me.Customers = Export_Customers
            End If

            If Export_Items Then
                Me.Items = Export_Items
            End If

            If Export_Images Then
                Me.Images = Export_Images
            End If
        Catch ex As Exception
            Throw ex
        End Try


    End Sub
#End Region




    

End Class
