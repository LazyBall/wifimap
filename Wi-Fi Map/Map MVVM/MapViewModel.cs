using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Wi_Fi_Map;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using System.Collections.ObjectModel;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace Wi_Fi_Map.Map_MVVM
{
    public class MapViewModel : INotifyPropertyChanged
    {
        //Singleton------
        private static MapViewModel _uniqueMap;
        public static MapViewModel GetInstance()
        {
            if (_uniqueMap == null)
            {
                _uniqueMap = new MapViewModel();
            }
            return _uniqueMap;
        }
        //---------------

        //public ObservableCollection<MapElement> Collection { get; private set; } = new ObservableCollection<MapElement>();
        //public IList<MapElement> Collection { get; set; } = new IList<MapElement>();
        public MapElementsLayer PointsLayer = new MapElementsLayer();
        public ObservableCollection<MapLayer> Collection { get; set; } = new ObservableCollection<MapLayer>();


        private MapColorScheme setColorSheme = MapColorScheme.Light;
        public MapColorScheme SetColorScheme
        {
            get { return setColorSheme; }
            set
            {
                setColorSheme = value;
                Notify("SetColorScheme");
            }
        }

        private int mapZoom = 10;
        public int MapZoom
        {
            get { return mapZoom; }
            set
            {
                mapZoom = value;
                Notify("MapZoom");
            }
        }

        private Geopoint mapGeopoint=new Geopoint(new BasicGeoposition());
        public Geopoint MapGeopoint
        {
            get { return mapGeopoint; }
            set
            {
                mapGeopoint = value;
                Notify("MapGeopoint");
            }
        }

        private Visibility posVisibility = Visibility.Collapsed;
        public Visibility PosVisibility
        {
            get { return posVisibility; }
            set
            {
                posVisibility = value;
                Notify("PosVisibility");
            }
        }

        private Geopoint posGeopoint = new Geopoint(new BasicGeoposition());
        public Geopoint PosGeopoint
        {
            get { return posGeopoint; }
            set
            {
                posGeopoint = value;
                Notify("PosGeopoint");
            }
        }

        //private MapElementsLayer pointsLayer = new MapElementsLayer();
        //public MapElementsLayer PointsLayer
        //{
        //    get { return pointsLayer; }
        //    set
        //    {
        //        pointsLayer = value;
        //        Notify("PointsLayer");
        //    }
        //}

        public void AddWifiPointsToMap(IEnumerable<WiFiSignalWithGeoposition> sigs)
        {
            //MyMap.MapElements.Clear();
            //Collection.Clear();
            var MyElements = new List<MapElement>();
            string filename = "ms-appx:///Assets/region.png";

            //IEnumerable<WiFiSignalWithGeoposition> filteredSignals = mapData._signals;

            //if ((comboBoxEncryptionFilter.SelectedItem as TextBlock)?. Text != "Не выбрано")
            //    filteredSignals = new EncryptionFilter((comboBoxEncryptionFilter.SelectedItem as TextBlock).Text).Filtering(filteredSignals);
            var random = new Random(DateTime.Now.Millisecond);
            double divider = 25000.0;
            int digits = 5;
            foreach (WiFiSignalWithGeoposition el in sigs)
            {
                double latitude = el.Latitude + (random.NextDouble() - 0.5) / divider;
                latitude = Math.Round(latitude, digits);
                double longitude = el.Longitude + (random.NextDouble() - 0.5) / divider;
                longitude = Math.Round(longitude, digits);
                //BasicGeoposition geopositionIcon = vm.CreateBasicGeoposition(latitude, longitude);
                Geopoint point = CreateBasicGeopoint(latitude, longitude);
                //PushPin mapIcon = new PushPin(
                //    geopoint,
                //    el.SSID,
                //    string.Join('\n', "Имя(SSID): " + el.SSID, "Mac-адрес(BSSID): " + el.BSSID,
                //    "Шифрование: " + el.Encryption, "Сила сигнала (в dBm): " + el.SignalStrength,
                //    "Местоположение (широта:долгота)", latitude + " : " + longitude),
                //     new Point(0.5, 0.5));
                MapIcon mapIcon = new MapIcon
                {
                    Location = point,
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri(filename)),
                    NormalizedAnchorPoint = new Point(0.5, 0.5),
                    Title = el.SSID,
                    Tag = string.Join(' ', "Имя(SSID): " + el.SSID, "Mac-адрес(BSSID): " + el.BSSID,
                    "Шифрование: " + el.Encryption, "Сила сигнала (в dBm): " + el.SignalStrength,
                    "Местоположение (широта:долгота)", latitude + " : " + longitude)
                };
                MyElements.Add(mapIcon);
                
                //MyMap.MapElements.Add(mapIcon);
            }
            var LandmarksLayer = new MapElementsLayer
            {
                ZIndex = 1,
                MapElements = MyElements
            };
            Collection.Remove(PointsLayer);
            PointsLayer = LandmarksLayer;
            Collection.Add(PointsLayer);
            //Collection.Add(LandmarksLayer);
            //mapData._signals.Clear();
        }

        public Geopoint CreateBasicGeopoint(double x, double y)
        {
            var b=new BasicGeoposition
            {
                Latitude = x,
                Longitude = y
            };
            return new Geopoint(b);
        }

        public Image DoImgPosition()
        {
            string fileName = "ms-appx:///Assets/circle-blue-overlay50.png";
            Image img = new Image
            {
                Source = new BitmapImage(new Uri(fileName)),
                Stretch = Stretch.None
            };

            MapControl.SetNormalizedAnchorPoint(img, new Point(0.5, 0.5));
            MapControl.SetLocation(img, MapGeopoint);
            return img;
        }

        //NotifyProperty реализация
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string f)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(f));
        }
    }
}
