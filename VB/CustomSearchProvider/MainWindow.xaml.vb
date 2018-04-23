Imports DevExpress.Xpf.Map
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes

Namespace CustomSearchProvider
    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            infoLayer.DataProvider = New SearchProvider()
        End Sub
    End Class
    Public Class SearchProvider
        Inherits InformationDataProviderBase
        Implements ISearchPanelRequestSender

        Protected Shadows ReadOnly Property Data() As SearchData
            Get
                Return CType(MyBase.Data, SearchData)
            End Get
        End Property
        Public ReadOnly Property Addresses() As IEnumerable(Of LocationInformation) Implements ISearchPanelRequestSender.Addresses
            Get
                Return Data.Addresses
            End Get
        End Property

        Public Overrides ReadOnly Property IsBusy() As Boolean
            Get
                Return False
            End Get
        End Property
        Protected Overrides Function CreateData() As IInformationData
            Return New SearchData()
        End Function
        Public Sub SearchByString(ByVal keyword As String) Implements ISearchPanelRequestSender.SearchByString
            Data.Search(keyword)
        End Sub
        Public Overrides Sub Cancel()
            Throw New NotImplementedException()
        End Sub
        Protected Overrides Function CreateObject() As MapDependencyObject
            Return New SearchProvider()
        End Function
    End Class
    Public Class SearchData
        Implements IInformationData


        Private ReadOnly addresses_Renamed As New List(Of LocationInformation)()
        Public ReadOnly Property Addresses() As IEnumerable(Of LocationInformation)
            Get
                Return addresses_Renamed
            End Get
        End Property
        Public Event OnDataResponse As EventHandler(Of RequestCompletedEventArgs) Implements IInformationData.OnDataResponse
        Private Function CreateEventArgs() As RequestCompletedEventArgs
            Dim items(addresses_Renamed.Count - 1) As MapItem
            For i As Integer = 0 To items.Length - 1
                items(i) = New MapPushpin() With { _
                    .Location = addresses_Renamed(i).Location, _
                    .Information = addresses_Renamed(i).Address.FormattedAddress, _
                    .Text = (i + 1).ToString() _
                }
            Next i
            Return New RequestCompletedEventArgs(items, Nothing, False, Nothing)
        End Function
        Protected Sub RaiseChanged()
            RaiseEvent OnDataResponse(Me, CreateEventArgs())
        End Sub
        Public Sub Search(ByVal keyword As String)
            Dim rnd As New Random(Date.Now.Millisecond)
            addresses_Renamed.Clear()
            Dim length As Integer = keyword.Length
            For i As Integer = 0 To length - 1
                Dim info As New LocationInformation()
                Dim address As New String(keyword & " " & i.ToString())
                info.Address = New Address(address)
                info.Location = New GeoPoint(rnd.Next(180) - 90, rnd.Next(360) - 180)
                info.DisplayName = address
                addresses_Renamed.Add(info)
            Next i
            RaiseChanged()
        End Sub
    End Class
    Public Class Address
        Inherits AddressBase

        Public Sub New(ByVal address As String)
            Me.FormattedAddress = address
        End Sub
        Protected Overrides Function CreateObject() As MapDependencyObject
            Return New Address(Me.FormattedAddress)
        End Function
    End Class
End Namespace
