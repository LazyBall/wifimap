namespace Wi_Fi_Map
{
    class WiFiSignal
    {
        public string MacAddress { get; set; }
        public string Ssid { get; set; }
        public byte SignalBars { get; set; }
        public int ChannelCenterFrequencyInKilohertz { get; set; }
        public string NetworkKind { get; set; }
        public string PhysicalKind { get; set; }

        public override string ToString()
        {
            return string.Join("/n", "MacAddress " + MacAddress, "Ssid " + Ssid, "SignalBars " +
                SignalBars.ToString(),
               "ChannelCenterFrequencyInKilohertz " + ChannelCenterFrequencyInKilohertz.ToString(),
               "NetworkKind " + NetworkKind, "PhysicalKind " + PhysicalKind);
        }
    }
}