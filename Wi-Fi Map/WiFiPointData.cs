using System;
using System.Collections.Generic;

namespace Wi_Fi_Map
{
    /// <summary>
    /// Class to contain data about a WiFi collection point
    /// </summary>
    public class WiFiPointData
    {
        public DateTimeOffset TimeStamp { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public List<WiFiSignal> WiFiSignals { get; set; }

        public WiFiPointData()
        {
            this.WiFiSignals = new List<WiFiSignal>();
        }

        public WiFiPointData(DateTimeOffset timeStamp, double latitude, double longtitude) : this()
        {
            this.TimeStamp = timeStamp;
            this.Latitude = latitude;
            this.Longitude = longtitude;
        }
    }
}