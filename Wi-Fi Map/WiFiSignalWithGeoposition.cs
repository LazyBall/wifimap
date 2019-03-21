

namespace Wi_Fi_Map
{
    public class WiFiSignalWithGeoposition : WiFi
    {

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public WiFiSignalWithGeoposition()
        {

        }

        public WiFiSignalWithGeoposition(WiFiSignal signal, double latitude, double longitude)
        {
            BSSID = signal.BSSID;
            SSID = signal.SSID;
            SignalStrength = signal.SignalStrength;
            Encryption = signal.Encryption;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}