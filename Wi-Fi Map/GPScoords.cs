using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Wi_Fi_Map
{
    public class GPScoords
    {
        private static GPScoords _uniqueGPS;
        public double Lat { get; set; } 
        public double Lon { get; set; }
        public IEnumerable<WiFiSignal> _signalsAround;
        private GPScoords()
        {
            Lat = -1;
            Lon = -1;
            _signalsAround = new List<WiFiSignal>();
        }
        public static GPScoords GetInstance()
        {
            if (_uniqueGPS == null)
            {
                _uniqueGPS = new GPScoords();
            }
            return _uniqueGPS;
        }
    }
}
