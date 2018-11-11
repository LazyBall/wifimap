﻿using System;
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
            GPScoords gPScoords = GPScoords.GetInstance();
            MapData mapData = MapData.GetInstance();

            MyMap.ColorScheme = mapData.Scheme;
            MyMap.ZoomLevel = mapData.CurrentZoom;
            BasicGeoposition geoposition = CreateBasicGeoposition(mapData.Lat, mapData.Lon);
            MyMap.Center = new Geopoint(geoposition);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GPScoords gPScoords = GPScoords.GetInstance();
            MapData mapData = MapData.GetInstance();
            if (e.Parameter is MapData)
            {
                MyMap.ZoomLevel = mapData.CurrentZoom;
                BasicGeoposition geoposition = CreateBasicGeoposition(mapData.Lat, mapData.Lon);
                MyMap.Center = new Geopoint(geoposition);
                Geopoint point = new Geopoint(geoposition);
                Image img = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/1.png")),
                    Stretch = Stretch.None
                };
                MapControl.SetNormalizedAnchorPoint(img, new Point(0.5, 0.5));
                MapControl.SetLocation(img, point);
                MyMap.Children.Clear();
                MyMap.Children.Add(img);
            }
            if(e.Parameter is GPScoords)
            {
                BasicGeoposition geoposition = CreateBasicGeoposition(gPScoords.Lat, gPScoords.Lon);
                MyMap.Center = new Geopoint(geoposition);
                if (gPScoords.Lat != -1 && gPScoords.Lon != -1)
                {
                    BasicGeoposition geopositionIcon = CreateBasicGeoposition(gPScoords.Lat, gPScoords.Lon);
                    Geopoint point = new Geopoint(geopositionIcon);
                    Image img = new Image
                    {
                        Source = new BitmapImage(new Uri("ms-appx:///Assets/1.png")),
                        Stretch = Stretch.None
                    };
                    MapControl.SetNormalizedAnchorPoint(img, new Point(0.5, 0.5));
                    MapControl.SetLocation(img, point);
                    MyMap.Children.Clear();
                    MyMap.Children.Add(img);
                }
            }
            //Отображение местоположения на карте
            
            //Добавление точек вайфай на карту
            foreach (WiFiSignalWithGeoposition el in mapData._signals)
            {
                BasicGeoposition geopositionIcon = CreateBasicGeoposition(el.Latitude, el.Longitude);
                Geopoint point = new Geopoint(geopositionIcon);
                MapIcon mapIcon = new MapIcon
                {
                    Location = point,
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/region.png")),
                    NormalizedAnchorPoint = new Point(0.5, 0.5),
                    Title = el.BSSID
                };
                MyMap.MapElements.Add(mapIcon);
            }
        }
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
            //mapData.CurrentZoom = MyMap.ZoomLevel;//хз зачем
            //if (MyMap.ZoomLevel > mapData.MaxZoom){ MyMap.ZoomLevel = mapData.MaxZoom;}
            //else if (MyMap.ZoomLevel < mapData.MinZoom){MyMap.ZoomLevel = mapData.MinZoom;}
            if (MyMap.ZoomLevel > 17)
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

        //private void MyMap_Loaded(object sender, RoutedEventArgs e)
        //{
        //    GPScoords gPScoords = GPScoords.GetInstance();
        //    MapData mapData = MapData.GetInstance();

        //    MyMap.ColorScheme = mapData.Scheme;
        //    MyMap.ZoomLevel = mapData.CurrentZoom;
        //    BasicGeoposition geoposition = CreateBasicGeoposition(mapData.Lat, mapData.Lon);
        //    MyMap.Center = new Geopoint(geoposition);
        //}

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
                MyMap.Center = new Geopoint(geoposition);
                if (gps.Lat != -1 && gps.Lon != -1)
                {
                    BasicGeoposition geopositionIcon = CreateBasicGeoposition(gps.Lat, gps.Lon);
                    Geopoint point = new Geopoint(geopositionIcon);
                    Image img = new Image
                    {
                        Source = new BitmapImage(new Uri("ms-appx:///Assets/1.png")),
                        Stretch = Stretch.None
                    };
                    MapControl.SetNormalizedAnchorPoint(img, new Point(0.5, 0.5));
                    MapControl.SetLocation(img, point);
                    MyMap.Children.Clear();
                    MyMap.Children.Add(img);
                }
            }
            //MyMap_Loaded(sender,e);
        }
    }
}
