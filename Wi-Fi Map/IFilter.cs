using System.Collections.Generic;

namespace Wi_Fi_Map
{
    public interface IFilter
    {
        List<WiFiSignalWithGeoposition> Filtering(List<WiFiSignalWithGeoposition> wiFiSignals);
    }
}
