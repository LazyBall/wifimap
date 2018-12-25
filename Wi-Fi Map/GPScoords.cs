

namespace Wi_Fi_Map
{
    public class GPScoords
    {
        private static GPScoords _uniqueGPS;
        public double Latitude { get; set; } 
        public double Longitude { get; set; }
        private GPScoords()
        {
            Latitude = -1;
            Longitude = -1;
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