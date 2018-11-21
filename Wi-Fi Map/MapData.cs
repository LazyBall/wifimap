using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Maps;

namespace Wi_Fi_Map
{
    public class MapData
    {
        private static Lazy<MapData> _uniqueMap = new Lazy<MapData>(() => new MapData());
        public List<WiFiSignalWithGeoposition> _signals;
        public double Lat { get; set; } = 57.622020;
        public double Lon { get; set; } = 39.932172;
        public MapColorScheme Scheme { get; set; } = MapColorScheme.Light;

        private MapData()
        {
            _signals = new List<WiFiSignalWithGeoposition>();
        }
        public static MapData GetInstance()
        {
            return _uniqueMap.Value;
        }
        public void AddData(IEnumerable<WiFiSignalWithGeoposition> fiSignals)
        {
            foreach (WiFiSignalWithGeoposition el in fiSignals)
            {
                _signals.Add(el);
            }
        }
    }
}
