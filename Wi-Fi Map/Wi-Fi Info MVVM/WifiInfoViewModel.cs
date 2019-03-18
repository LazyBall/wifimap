using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Wi_Fi_Map.Wi_Fi_Info_MVVM
{
    public class WifiInfoViewModel: INotifyPropertyChanged
    {
        private HashSet<string> BSSIDS { get; set; } = new HashSet<string>();
        private WiFiScanner WiFiScanner { get; set; } = new WiFiScanner();
        public ObservableCollection<WifiSignalModel> Signals { get; private set; } = new ObservableCollection<WifiSignalModel>();
        public bool RefreshStop = false;

        private string startStopSymbol = "\xEDB4";
        public string StartStopSymbol
        {
            get { return startStopSymbol; }
            set
            {
                startStopSymbol = value;
                Notify("StartStopSymbol");
            }
        }

        #region Notify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string f)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(f));
        }
        #endregion

        #region GetData
        //Methods to get signals
        public async void RefreshWifiList()
        {
            while(!RefreshStop)
            {  
                if (WiFiScanner.WiFiAdapter == null)
                {
                    try
                    {
                        await WiFiScanner.InitializeScanner();
                    }
                    catch
                    {
                        var dialog = new MessageDialog("Проверьте, влючен ли Wi-Fi.");
                        await dialog.ShowAsync();
                    }
                }
                if (WiFiScanner.WiFiAdapter != null)
                {
                    var list = await GetWiFiSignalsData();
                    foreach (var el in list)
                    {
                        CheckValues(el);

                        if (!BSSIDS.Contains(el.BSSID))
                        {
                            Signals.Add(el);
                            BSSIDS.Add(el.BSSID);
                        }
                        else
                        {
                            int count = -1;
                            foreach(var sigs in Signals)
                            {
                                count++;

                                if (sigs.BSSID == el.BSSID)
                                    break;
                            }

                            if(count!=-1) Signals[count] = el;
                        }
                    }
                    Thread thread = new Thread(() => SendDataToDatabase(list));
                    thread.Start();
                }
                await Task.Delay(2000);
            }
        }

        //Some changes with IEnumerable<T>, now T is WifiSignalModel:WiFiSignal
        private async Task<IEnumerable<WifiSignalModel>> GetWiFiSignalsData()
        {
            await this.WiFiScanner.ScanForNetworks();
            return await Task.Run(() =>
            {
                var signals = new List<WifiSignalModel>();
                foreach (var availableNetwork in WiFiScanner.WiFiAdapter.NetworkReport.AvailableNetworks)
                {
                    WifiSignalModel wifiSignal = new WifiSignalModel
                    {
                        BeaconInterval = availableNetwork.BeaconInterval.TotalSeconds.ToString(),
                        BSSID = availableNetwork.Bssid,
                        ChannelCenterFrequencyInKilohertz = availableNetwork.ChannelCenterFrequencyInKilohertz,
                        Encryption = availableNetwork.SecuritySettings.NetworkEncryptionType.ToString(),
                        IsWiFiDirect = availableNetwork.IsWiFiDirect,
                        NetworkKind = availableNetwork.NetworkKind.ToString(),
                        PhyKind = availableNetwork.PhyKind.ToString(),
                        SignalStrength = (short)availableNetwork.NetworkRssiInDecibelMilliwatts,
                        SSID = availableNetwork.Ssid,
                        Uptime = availableNetwork.Uptime.TotalHours.ToString()
                    };
                    signals.Add(wifiSignal);
                }
                return signals;
            });
        }

        private static async void SendDataToDatabase(IEnumerable<WiFiSignal> signals)
        {
            if (SendingDataSetting.Instance.DataIsSent)
            {
                try
                {
                    Geolocator geolocator = new Geolocator();
                    Geoposition position = await geolocator.GetGeopositionAsync();
                    double latitude = position.Coordinate.Point.Position.Latitude,
                       longitude = position.Coordinate.Point.Position.Longitude;
                    var list = (from signal in signals select new WiFiSignalWithGeoposition(signal, latitude, longitude));
                    var db = new Database();
                    db.AddSignals(list);
                }
                catch
                {
                    return;
                }
            }
        }
        //End methods to get data.
        #endregion
        //There are methods to Check values and to sort.

        private void CheckValues(WifiSignalModel signalModel)
        {
            if (signalModel.SignalStrength >= -45)
                SetValue(signalModel, new SolidColorBrush(Colors.LimeGreen), "\xEC3F", "SignalStrength");
            else if (signalModel.SignalStrength < -45 && signalModel.SignalStrength >= -75)
                SetValue(signalModel, new SolidColorBrush(Colors.Yellow), "\xEC3E", "SignalStrength");
            else
                SetValue(signalModel, new SolidColorBrush(Colors.Red), "\xEC3D", "SignalStrength");

            if (signalModel.Encryption == "None")
                SetValue(signalModel, new SolidColorBrush(Colors.LimeGreen), "\xE785","Encryption");
            else
                SetValue(signalModel, new SolidColorBrush(Colors.Red), "\xE72E", "Encryption");

        }

        private void SetValue(WifiSignalModel signalModel, SolidColorBrush br, string symbol, string type)
        {
            switch(type)
            {
                case "Encryption":
                    signalModel.EncryptionBrush = br;
                    signalModel.EncryptionSymbol = symbol;
                    break;
                case "SignalStrength":
                    signalModel.StrengthBrush = br;
                    signalModel.StrengthSymbol = symbol;
                    break;
                default:
                    break;
            }
            
        }
        
        public void SortSignals(IComparer<WiFiSignal> comparer)
        {
            var sortebleList = new List<WifiSignalModel>(Signals);
            sortebleList.Sort(comparer);

            for(int i=0;i<sortebleList.Count;i++)
            {
                Signals.Move(Signals.IndexOf(sortebleList[i]), i);
            }
        }


        private class SSIDComparer : IComparer<WiFiSignal>
        {
            public int Compare(WiFiSignal x, WiFiSignal y)
            {
                return x.SSID.ToUpper().CompareTo(y.SSID.ToUpper());
            }
        }

        private class SignalStrengthComparer : IComparer<WiFiSignal>
        {
            public int Compare(WiFiSignal x, WiFiSignal y)
            {
                return -x.SignalStrength.CompareTo(y.SignalStrength);
            }
        }

        private class EncryptionComparer : IComparer<WiFiSignal>
        {
            public int Compare(WiFiSignal x, WiFiSignal y)
            {
                return x.Encryption.ToUpper().CompareTo(y.Encryption.ToUpper());
            }
        }

        public IComparer<WiFiSignal> GetComparer(string text)
        {
            switch (text)
            {
                case ("Шифрованию"):
                    return new EncryptionComparer();
                case ("Силе сигнала"):
                    return new SignalStrengthComparer();
                default:
                    return new SSIDComparer();
            }
        }
    }
}
