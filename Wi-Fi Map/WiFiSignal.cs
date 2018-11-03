
namespace Wi_Fi_Map
{
    public class WiFiSignal
    {
        public string BSSID { get; private set; }

        public string SSID { get; private set; }

        public double NetworkRssiInDecibelMilliwatts { get; private set; }

        public int ChannelCenterFrequencyInKilohertz { get; private set; }

        public string Encryption { get; private set; }

        public WiFiSignal() { }

        public WiFiSignal(string BSSID, string SSID, double networkRssiInDecibelMilliwatts,
            int channelCenterFrequencyInKilohertz, string encryption)
        {
            this.BSSID = BSSID;
            this.SSID = SSID;
            this.NetworkRssiInDecibelMilliwatts = networkRssiInDecibelMilliwatts;
            this.ChannelCenterFrequencyInKilohertz = channelCenterFrequencyInKilohertz;
            this.Encryption = encryption;
        }
    }
}