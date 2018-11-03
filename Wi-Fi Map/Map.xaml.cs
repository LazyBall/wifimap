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
        }

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    if(e.Parameter!=null)
        //    {
        //        string answer = e.Parameter.ToString();
        //        string[] wifiPoints = answer.Split('\n');
        //        foreach(string el in wifiPoints)
        //        {
        //            string[] wifiPoint = el.Split(',');
        //            List<string> info = new List<string>();
        //            info.Add(wifiPoint[0]);
        //            info.Add(wifiPoint[4]);
        //            info.Add(wifiPoint[5]);
        //        }
        //    }
        //}

        private void MyMap_ZoomLevelChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {
            MapData mapData = MapData.GetInstance();
            mapData.CurrentZoom = MyMap.ZoomLevel;
            if (MyMap.ZoomLevel > mapData.MaxZoom)
            {
                MyMap.ZoomLevel = mapData.MaxZoom;
            }
            else if (MyMap.ZoomLevel < mapData.MinZoom)
            {
                MyMap.ZoomLevel = mapData.MinZoom;
            }
            else if (MyMap.ZoomLevel > 17)
            {
                foreach (MapIcon el in MyMap.MapElements)
                {
                    el.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/12.png"));
                }
            }
            else
            {
                foreach (MapIcon el in MyMap.MapElements)
                {
                    el.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/region.png"));
                }
            }
        }

        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            GPScoords gPScoords = GPScoords.GetInstance();
            MapData mapData = MapData.GetInstance();
            MyMap.ColorScheme = mapData.Scheme;
            
            MyMap.ZoomLevel = mapData.CurrentZoom;
            BasicGeoposition geoposition = new BasicGeoposition();
            geoposition.Latitude = mapData.Lat;
            geoposition.Longitude = mapData.Lon;
            MyMap.Center = new Geopoint(geoposition);
            //Отображение местоположения на карте
            
            if (gPScoords.Lat != -1 && gPScoords.Lon != -1)
            {
                BasicGeoposition geopositionIcon = new BasicGeoposition();
                geopositionIcon.Latitude = gPScoords.Lat;
                geopositionIcon.Longitude = gPScoords.Lon;
                Geopoint point = new Geopoint(geopositionIcon);
                point = new Geopoint(geopositionIcon);
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("ms-appx:///Assets/1.png"));
                img.Stretch = Stretch.None;
                MapControl.SetNormalizedAnchorPoint(img, new Point(0.5, 0.5));
                MapControl.SetLocation(img, point);
                MyMap.Children.Clear();
                MyMap.Children.Add(img);
            }
            //Добавление точек вайфай на карту
            foreach (WiFiSignalWithGeoposition el in mapData._signals)
            {
                BasicGeoposition geopositionIcon = new BasicGeoposition();
                geopositionIcon = new BasicGeoposition();
                geopositionIcon.Latitude = el.Latitude;
                geopositionIcon.Longitude = el.Longitude;
                Geopoint point = new Geopoint(geopositionIcon);
                point = new Geopoint(geopositionIcon);
                MapIcon mapIcon = new MapIcon();
                mapIcon.Location = point;
                mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/region.png"));
                mapIcon.NormalizedAnchorPoint = new Point(0.5, 0.5);
                mapIcon.Title = el.BSSID;
                MyMap.MapElements.Add(mapIcon);
            }
        }
    }
}
