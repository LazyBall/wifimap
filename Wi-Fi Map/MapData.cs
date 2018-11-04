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
        public HashSet<string> _bssid;
        public string InfoAboutSignals { get; set; } = "";
        public double Lat { get; set; } = 57.622020;
        public double Lon { get; set; } = 39.932172;
        public double CurrentZoom { get; set; } = 10;
        public double MinZoom { get; set; } = 10;
        public double MaxZoom { get; set; } = 19;
        public MapColorScheme Scheme { get; set; } = MapColorScheme.Light;
        private MapData()
        {
            _signals = new List<WiFiSignalWithGeoposition>();
            _bssid = new HashSet<string>();
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
                if (_bssid.Contains(el.BSSID))
                {

                }
                else
                {
                    _bssid.Add(el.BSSID);
                    _signals.Add(el);
                }
            }
        }
        public void AddData(WiFiPointData wiFiPoint)
        {
            List<WiFiSignalWithGeoposition> list = new List<WiFiSignalWithGeoposition>();
            foreach(var signal in wiFiPoint.WiFiSignals)
            {
                WiFiSignalWithGeoposition sg = new WiFiSignalWithGeoposition
                {
                    BSSID=signal.BSSID,
                    Encryption=signal.Encryption,
                    Latitude=wiFiPoint.Latitude,
                    Longitude=wiFiPoint.Longitude,
                    SignalStrength=signal.SignalStrength,
                    SSID=signal.SSID,
                    //TimeStamp=wiFiPoint.TimeStamp
                };
                list.Add(sg);
            }
            AddData(list);
        }
        public void AddFromIenum(IEnumerable<WiFiSignal> wiFiSignals)
        {
            //foreach (WiFiSignal el in wiFiSignals)
            //{
            //    _signals.Add(el);
            //    _macAdresses.Add(el.MacAddress);
            //}
        }
    }
}
