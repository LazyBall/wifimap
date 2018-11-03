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

        public WiFiSignalWithGeoposition(string BSSID, string SSID, double networkRssiInDecibelMilliwatts,
            int channelCenterFrequencyInKilohertz, string encryption, DateTimeOffset timeStamp,
            double latitude, double longtitude) : base(BSSID, SSID,
                networkRssiInDecibelMilliwatts, channelCenterFrequencyInKilohertz, encryption)
        {
            this.TimeStamp = timeStamp;
            this.Latitude = latitude;
            this.Longitude = longtitude;
        }

        public WiFiSignalWithGeoposition(WiFiSignal signal, DateTimeOffset timeStamp, double latitude,
            double longtitude) : this(signal.BSSID, signal.SSID, signal.NetworkRssiInDecibelMilliwatts,
                signal.ChannelCenterFrequencyInKilohertz, signal.Encryption, timeStamp, latitude, longtitude)
        { }
    }
}