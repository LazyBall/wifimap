using System.Collections.Generic;

namespace Wi_Fi_Map
{
    public class EncryptionFilter : IFilter
    {
        public string Encryption { get; set; }
        public EncryptionFilter(string encryption)
        {
            this.Encryption = encryption;
        }
        public IEnumerable<WiFiSignalWithGeoposition> Filtering(IEnumerable<WiFiSignalWithGeoposition> wiFiSignals)
        {
            foreach (WiFiSignalWithGeoposition el in wiFiSignals)
            {
                if (el.Encryption.ToUpper() == Encryption.ToUpper())
                    yield return el;
            }
        }
    }
}