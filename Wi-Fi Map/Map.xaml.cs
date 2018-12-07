using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wi_Fi_Map
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Map : Page
    {
        public Map()
        {
            this.InitializeComponent();
            MapData mapData = MapData.GetInstance();

            MyMap.ColorScheme = mapData.Scheme;
            MyMap.ZoomLevel = 10;
            BasicGeoposition geoposition = CreateBasicGeoposition(mapData.Lat, mapData.Lon);
            MyMap.Center = new Geopoint(geoposition);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MapData mapData = MapData.GetInstance();
            if (e.Parameter is GPScoords position)
            {
                BasicGeoposition geoposition = CreateBasicGeoposition(position.Lat, position.Lon);
                ShowOnMapPosition(geoposition);
                AddWifiPointsToMap(mapData);
            }
            else if (e.Parameter is MapData map)
            {
                BasicGeoposition geoposition = CreateBasicGeoposition(map.Lat, map.Lon);
                ShowOnMapPosition(geoposition);
            }
            else { MyMap.ColorScheme = mapData.Scheme; }
        }

        private void AddWifiPointsToMap(MapData mapData)
        {
            MyMap.MapElements.Clear();
            string filename = "ms-appx:///Assets/region.png";
            //if (MyMap.ZoomLevel > 18)
            //{filename = "ms-appx:///Assets/wifi-circle.png";}
            List<WiFiSignalWithGeoposition> filteredSignals = mapData._signals;

            if((ComboBoxEncryptionFilter.SelectedItem as TextBlock).Text != "")
                filteredSignals = new EncryptionFilter((ComboBoxEncryptionFilter.SelectedItem as TextBlock).Text).Filtering(filteredSignals);

            foreach (WiFiSignalWithGeoposition el in filteredSignals)
            {
                BasicGeoposition geopositionIcon = CreateBasicGeoposition(el.Latitude, el.Longitude);
                Geopoint point = new Geopoint(geopositionIcon);
                MapIcon mapIcon = new MapIcon
                {
                    Location = point,
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri(filename)),
                    NormalizedAnchorPoint = new Point(0.5, 0.5),
                    Title = el.SSID,
                    Tag = String.Join('\n', el.SSID, el.Encryption, el.SignalStrength, el.Latitude + " : " + el.Longitude, el.BSSID)               
                };
                MyMap.MapElements.Add(mapIcon);
            }
            mapData._signals.Clear();
        }

        private void ShowOnMapPosition(BasicGeoposition geoposition)
        {
            Geopoint point = new Geopoint(geoposition);
            MyMap.Center = new Geopoint(geoposition);
            string fileName = "ms-appx:///Assets/circle-blue-overlay50.png";
            Image img = new Image
            {
                Source = new BitmapImage(new Uri(fileName)),
                Stretch = Stretch.None
            };

            MapControl.SetNormalizedAnchorPoint(img, new Point(0.5, 0.5));
            MapControl.SetLocation(img, point);
            MyMap.Children.Clear();
            MyMap.Children.Add(img);
        }

        private void MyMap_ZoomLevelChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            MapData mapData = MapData.GetInstance();
            if (MyMap.ZoomLevel < 9) MyMap.ZoomLevel = 9;
            if (MyMap.ZoomLevel > 20) MyMap.ZoomLevel = 20;
            //if (MyMap.ZoomLevel > 18) 
            //{
            //    foreach (MapIcon el in MyMap.MapElements)
            //    {el.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/wifi-circle.png"));}
            //    //(MyMap.Children[0] as Image).Source= new BitmapImage(new Uri("ms-appx:///Assets/circle-blue-overlay100.png"));
            //}
            //else
            //{
            //    foreach (MapIcon el in MyMap.MapElements)
            //    {el.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/region.png"));}
            //}
        }

        private static BasicGeoposition CreateBasicGeoposition(double x,double y)
        {
            return new BasicGeoposition
            {
                Latitude = x,
                Longitude = y
            };
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GPScoords gps = GPScoords.GetInstance();
            if (gps.Lat != -1 && gps.Lon != -1)
            {
                BasicGeoposition geoposition = CreateBasicGeoposition(gps.Lat, gps.Lon);
                ShowOnMapPosition(geoposition);
            }
        }

        private void MyMap_MapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            MapIcon myClickedIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            string Title = myClickedIcon.Title;
            myClickedIcon.Title = myClickedIcon.Tag.ToString();
            myClickedIcon.Tag = Title;
        }
    }
}
