using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace Wi_Fi_Map.Map_MVVM
{
    public class PushPin
    {
        public Geopoint Location { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public Uri ImageSourceUri { get; set; } = new Uri("ms-appx:///Assets/region.png");
        public Point NormalizedAnchorPoint { get; set; }

        public PushPin(Geopoint geopoint, string name, string info, Point point)
        {
            Location = geopoint;
            Name = name;
            Info = info;
            NormalizedAnchorPoint = point;
        }

        public PushPin() { }
    }
}
