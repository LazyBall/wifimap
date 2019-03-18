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
using Wi_Fi_Map.Map_MVVM;
using System.Collections.ObjectModel;
using Windows.System;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wi_Fi_Map
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Map : Page
    {
        MapViewModel vm = MapViewModel.GetInstance();

        public Map()
        {
            this.InitializeComponent();

            //MyMap.Style = MapStyle.None;
            //MyMap.TileSources.Clear();
            //string mapuri = "http://mt1.google.com/vt/lyrs=r&hl=x-local&z={zoomlevel}&x={x}&y={y}";
            //MyMap.TileSources.Add(new MapTileSource(new HttpMapTileDataSource(mapuri)));
           
            DataContext = vm;

            comboBoxEncryptionFilter.SelectedItem = defaultTextBlock;
            vm.MapGeopoint = vm.CreateBasicGeopoint(57.622020, 39.932172);
            ScanOnce_Click(ScanOnce, new RoutedEventArgs());
            GridInfoOnePoint.Visibility = Visibility.Collapsed;
        }

        private void MyMap_MapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            if (args.MapElements.FirstOrDefault(x => x is MapIcon) is MapIcon myClickedIcon)
                InfoOnePoint.Text = myClickedIcon.Tag.ToString();

            GridInfoOnePoint.Visibility = Visibility.Visible;
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

            if ((comboBoxEncryptionFilter.SelectedItem as TextBlock)?.Text != "Не выбрано")
                signals = new EncryptionFilter((comboBoxEncryptionFilter.SelectedItem as TextBlock).Text).Filtering(signals);

            vm.AddWifiPointsToMap(signals);

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

        private async void Position_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Control;
            if (bt != null) bt.IsEnabled = false;
            double x = -1, y = -1;
            try
            {
                Geolocator geolocator = new Geolocator();
                Geoposition position = await geolocator.GetGeopositionAsync();
                x = position.Coordinate.Point.Position.Latitude;
                y = position.Coordinate.Point.Position.Longitude;
            }
            catch
            {
                MessageDialog md = new MessageDialog("Проверьте, включена ли геолокация.");
                await md.ShowAsync();
            }

            if (x != -1 && y != -1)
            {
                vm.PosGeopoint = vm.CreateBasicGeopoint(x, y);
                vm.MapGeopoint = vm.PosGeopoint;
                vm.PosVisibility = Visibility.Visible;
            }
            if (bt != null) bt.IsEnabled = true;
        }

        private void More_Click(object sender, RoutedEventArgs e)
        {
            MyMap.ZoomLevel += 1;
        }

        private void Less_Click(object sender, RoutedEventArgs e)
        {
            MyMap.ZoomLevel -= 1;
        }
    }
}