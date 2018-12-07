using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wi_Fi_Map
{
    public class EncryptionFilter : IFilter
    {
        public string _type { get; set; } = "None";
        public EncryptionFilter(string s) { _type = s; }
        public List<WiFiSignalWithGeoposition> Filtering(List<WiFiSignalWithGeoposition> wiFiSignals)
        {
            List<WiFiSignalWithGeoposition> filteredSignals = new List<WiFiSignalWithGeoposition>();
            foreach(WiFiSignalWithGeoposition el in wiFiSignals)
            {
                if (el.Encryption.ToLower() == _type.ToLower()) filteredSignals.Add(el);
            }
            return filteredSignals;
        }
    }
}
