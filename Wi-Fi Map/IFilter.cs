using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wi_Fi_Map
{
    public interface IFilter
    {
        List<WiFiSignalWithGeoposition> Filtering(List<WiFiSignalWithGeoposition> wiFiSignals);
    }
}
