using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wi_Fi_Map
{
    public class WiFiSignal
    {
        public string MacAddress { get; set; }

        public string Ssid { get; set; }

        public double NetworkRssiInDecibelMilliwatts { get; set; }

        public int ChannelCenterFrequencyInKilohertz { get; set; }

        public string Encryption { get; set; }

        //TODO: add security details
    }
}