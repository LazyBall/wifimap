

namespace Wi_Fi_Map
{
    public abstract class WiFi
    {
        public string BSSID { get; set; }

        public string SSID { get; set; }

        public string Encryption { get; set; }

        public short SignalStrength { get; set; }

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