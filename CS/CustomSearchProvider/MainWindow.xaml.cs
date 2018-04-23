using DevExpress.Xpf.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomSearchProvider {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            infoLayer.DataProvider = new SearchProvider();
        }
    }
    public class SearchProvider : InformationDataProviderBase, ISearchPanelRequestSender {
        protected new SearchData Data { get { return (SearchData)base.Data; } }
        public IEnumerable<LocationInformation> Addresses { get { return Data.Addresses; } }

        public override bool IsBusy {
            get { return false; }
        }
        protected override IInformationData CreateData() {
            return new SearchData();
        }
        public void SearchByString(string keyword) {
            Data.Search(keyword);
        }
        public override void Cancel() {
            throw new NotImplementedException();
        }
        protected override MapDependencyObject CreateObject() {
            return new SearchProvider();
        }
    }
    public class SearchData : IInformationData {
        readonly List<LocationInformation> addresses = new List<LocationInformation>();
        public IEnumerable<LocationInformation> Addresses { get { return addresses; } }
        public event EventHandler<RequestCompletedEventArgs> OnDataResponse;
        RequestCompletedEventArgs CreateEventArgs() {
            MapItem[] items = new MapItem[addresses.Count];
            for (int i = 0; i < items.Length; i++)
                items[i] = new MapPushpin() { Location = addresses[i].Location, Information = addresses[i].Address.FormattedAddress, Text = (i + 1).ToString() };
            return new RequestCompletedEventArgs(items, null, false, null);
        }
        protected void RaiseChanged() {
            if (OnDataResponse != null)
                OnDataResponse(this, CreateEventArgs());
        }
        public void Search(string keyword) {
            Random rnd = new Random(DateTime.Now.Millisecond);
            addresses.Clear();
            int length = keyword.Length;
            for (int i = 0; i < length; i++) {
                LocationInformation info = new LocationInformation();
                string address = keyword + " " + i.ToString();
                info.Address = new Address(address);
                info.Location = new GeoPoint(rnd.Next(180) - 90, rnd.Next(360) - 180);
                info.DisplayName = address;
                addresses.Add(info);
            }
            RaiseChanged();
        }
    }
    public class Address : AddressBase {
        public Address(string address) {
            this.FormattedAddress = address;
        }
        protected override MapDependencyObject CreateObject() {
            return new Address(this.FormattedAddress);
        }
    }
}
