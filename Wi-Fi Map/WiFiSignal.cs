
namespace Wi_Fi_Map
{
    public class WiFiSignal
    {
        public string MacAddress { get; private set; }

        public string Ssid { get; private set; }

        public double NetworkRssiInDecibelMilliwatts { get; private set; }

        public int ChannelCenterFrequencyInKilohertz { get; private set; }

        public string Encryption { get; private set; }

        public WiFiSignal() { }

        public WiFiSignal(string macAddress, string ssid, double networkRssiInDecibelMilliwatts,
            int channelCenterFrequencyInKilohertz, string encryption)
        {
            this.MacAddress = macAddress;
            this.Ssid = ssid;
            this.NetworkRssiInDecibelMilliwatts = networkRssiInDecibelMilliwatts;
            this.ChannelCenterFrequencyInKilohertz = channelCenterFrequencyInKilohertz;
            this.Encryption = encryption;
        }
    }
}