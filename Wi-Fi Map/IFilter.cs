using System.Collections.Generic;

namespace Wi_Fi_Map
{
    public interface IFilter
    {
        IEnumerable<WiFiSignalWithGeoposition> Filtering(IEnumerable<WiFiSignalWithGeoposition> wiFiSignals);
    }
}