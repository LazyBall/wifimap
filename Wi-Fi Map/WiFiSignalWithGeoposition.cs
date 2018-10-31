using System;

namespace Wi_Fi_Map
{
    /// <summary>
    /// Класс, содержащий в себес информацию о сети и её координаты + время последнего обнаружения
    /// </summary>
    public class WiFiSignalWithGeoposition : WiFiSignal
    {
        public DateTimeOffset TimeStamp { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public WiFiSignalWithGeoposition() : base() { }

        public WiFiSignalWithGeoposition(string macAddress, string ssid, double networkRssiInDecibelMilliwatts,
            int channelCenterFrequencyInKilohertz, string encryption, DateTimeOffset timeStamp,
            double latitude, double longtitude) : base(macAddress, ssid,
                networkRssiInDecibelMilliwatts, channelCenterFrequencyInKilohertz, encryption)
        {
            this.TimeStamp = timeStamp;
            this.Latitude = latitude;
            this.Longitude = longtitude;
        }

        public WiFiSignalWithGeoposition(WiFiSignal signal, DateTimeOffset timeStamp, double latitude,
            double longtitude) : this(signal.MacAddress, signal.Ssid, signal.NetworkRssiInDecibelMilliwatts,
                signal.ChannelCenterFrequencyInKilohertz, signal.Encryption, timeStamp, latitude, longtitude)
        { }
    }
}