

using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Wi_Fi_Map
{
    public abstract class WiFi
    {
        public virtual string BSSID { get; set; }

        public virtual string SSID { get; set; }

        public virtual string Encryption { get; set; }

        public virtual short SignalStrength { get; set; }
    }

    public class WiFiSignal : WiFi
    {
        public int ChannelCenterFrequencyInKilohertz { get; set; }

        public string BeaconInterval { get; set; }

        public bool IsWiFiDirect { get; set; }

        public string NetworkKind { get; set; }

        public string PhyKind { get; set; }

        public string Uptime { get; set; }

    }
}