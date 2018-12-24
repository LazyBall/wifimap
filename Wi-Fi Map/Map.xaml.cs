using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;
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
            comboBoxEncryptionFilter.SelectedItem = defaultTextBlock;
            MapData mapData = MapData.GetInstance();

            MyMap.ColorScheme = mapData.Scheme;
            MyMap.ZoomLevel = 10;
            BasicGeoposition geoposition = CreateBasicGeoposition(mapData.Latitude, mapData.Longitude);
            MyMap.Center = new Geopoint(geoposition);
            ScanOnce_Click(ScanOnce, new RoutedEventArgs());

            GridInfoOnePoint.Visibility = Visibility.Collapsed;
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
                BasicGeoposition geoposition = CreateBasicGeoposition(map.Latitude, map.Longitude);
                ShowOnMapPosition(geoposition);
            }
            else { MyMap.ColorScheme = mapData.Scheme; }
        }

        private void AddWifiPointsToMap(MapData mapData)
        {
            MyMap.MapElements.Clear();
            string filename = "ms-appx:///Assets/region.png";

            List<WiFiSignalWithGeoposition> filteredSignals = mapData._signals;

            if ((comboBoxEncryptionFilter.SelectedItem as TextBlock)?. Text != "Не выбрано")
                filteredSignals = new EncryptionFilter((comboBoxEncryptionFilter.SelectedItem as TextBlock).Text).Filtering(filteredSignals);
            var random = new Random(DateTime.Now.Millisecond);
            double divider = 50000.0;
            int digits = 5;
            foreach (WiFiSignalWithGeoposition el in filteredSignals)
            {
                double latitude = el.Latitude + (random.NextDouble() - 0.5) / divider;
                latitude = Math.Round(latitude, digits);
                double longitude = el.Longitude + (random.NextDouble() - 0.5) / divider;
                longitude = Math.Round(longitude, digits);
                BasicGeoposition geopositionIcon = CreateBasicGeoposition(latitude, longitude);
                Geopoint point = new Geopoint(geopositionIcon);
                MapIcon mapIcon = new MapIcon
                {
                    Location = point,
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri(filename)),
                    NormalizedAnchorPoint = new Point(0.5, 0.5),
                    Title = el.SSID,
                    Tag = string.Join('\n', "Имя(SSID): " + el.SSID, "Mac-адрес(BSSID): " + el.BSSID,
                    "Шифрование: " + el.Encryption, "Сила сигнала (в dBm): " + el.SignalStrength,
                    "Местоположение (широта:долгота)", latitude + " : " + longitude)
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

        private static BasicGeoposition CreateBasicGeoposition(double x,double y)
        {
            return new BasicGeoposition
            {
                Latitude = x,
                Longitude = y
            };
        }

        private async void TextBlockPosition_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var bt = sender as Control;
            if (bt != null) bt.IsEnabled = false;
            GPScoords gps = GPScoords.GetInstance();
            try
            {
                Geolocator geolocator = new Geolocator();
                Geoposition position = await geolocator.GetGeopositionAsync();
                gps.Lat = position.Coordinate.Point.Position.Latitude;
                gps.Lon = position.Coordinate.Point.Position.Longitude;
            }
            catch
            {
                MessageDialog md = new MessageDialog("Проверьте, включена ли геолокация.");
                await md.ShowAsync();
            }

            if (gps.Lat != -1 && gps.Lon != -1)
            {
                BasicGeoposition geoposition = CreateBasicGeoposition(gps.Lat, gps.Lon);
                ShowOnMapPosition(geoposition);
            }
            if (bt != null) bt.IsEnabled = true;
        }

        private void MyMap_MapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            //InfoOnePoint.Children.Clear();
            if (args.MapElements.FirstOrDefault(x => x is MapIcon) is MapIcon myClickedIcon)
            {
                InfoOnePoint.Text = myClickedIcon.Tag.ToString();
                //InfoOnePoint.Children.Add(textBlock);
            }
            GridInfoOnePoint.Visibility = Visibility.Visible;
            //if (myClickedIcon != null)
            //{
            //    string Title = myClickedIcon.Title;
            //    myClickedIcon.Title = myClickedIcon.Tag.ToString();
            //    myClickedIcon.Tag = Title;
            //}
        }

        private async void ScanOnce_Click(object sender, RoutedEventArgs e)
        {
            //обработка события нажатия на кнопку обновить данные
            var bt = sender as Control;
            if (bt != null) bt.IsEnabled = false;
            IEnumerable<WiFiSignalWithGeoposition> signals;
            try
            {
                if(!CheckForInternetConnection()) throw new Exception();
                var db = new Database();
                signals = await db.GetAllSignalsAsync();
            }
            catch
            {
                MessageDialog md = new MessageDialog("Проверьте наличие доступа в сеть.");
                await md.ShowAsync();
                signals = new List<WiFiSignalWithGeoposition>(0);
            }            
            MapData mapData = MapData.GetInstance();
            mapData.AddData(signals);
            AddWifiPointsToMap(mapData);
            if (bt != null) bt.IsEnabled = true;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    using (client.OpenRead("https://www.google.ru/"))
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GridInfoOnePoint.Visibility = Visibility.Collapsed;
        }
    }
}