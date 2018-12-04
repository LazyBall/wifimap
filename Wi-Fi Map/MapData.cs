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
        private static MapData _uniqueMap;
        public List<WiFiSignalWithGeoposition> _signals;
        public string InfoAboutSignals { get; set; } = "";
        public double Latitude { get; set; } = 57.622020;
        public double Longitude { get; set; } = 39.932172;
        public MapColorScheme Scheme { get; set; } = MapColorScheme.Light;

        private MapData()
        {
            _signals = new List<WiFiSignalWithGeoposition>();
        }
        public static MapData GetInstance()
        {
            if (_uniqueMap == null)
            {
                _uniqueMap = new MapData();
            }
            return _uniqueMap;
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
