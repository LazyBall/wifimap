using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.WiFi;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Wi_Fi_Map
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var scanner = new WifiScanner();
            scanner.ScanForNetworks();
            var report = scanner.WiFiAdapter.NetworkReport;           
            foreach (var availableNetwork in report.AvailableNetworks)
            {
                WiFiSignal wifiSignal = new WiFiSignal()
                {
                    MacAddress = availableNetwork.Bssid,
                    Ssid = availableNetwork.Ssid,
                    SignalBars = availableNetwork.SignalBars,
                    ChannelCenterFrequencyInKilohertz =
                    availableNetwork.ChannelCenterFrequencyInKilohertz,
                    NetworkKind = availableNetwork.NetworkKind.ToString(),
                    PhysicalKind = availableNetwork.PhyKind.ToString()
                };
                texBox.Text += wifiSignal.ToString() + "/n+/n";
            }
        }
    }
}
