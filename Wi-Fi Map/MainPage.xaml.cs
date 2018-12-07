using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.WiFi;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using System.Threading.Tasks;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Services.Maps;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Xaml.Shapes;

namespace Wi_Fi_Map
{ 
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WiFiScanner _wifiScanner;
        Rectangle rectangle = new Rectangle();
        
        public MainPage()
        {
            this.InitializeComponent();
            BackButton.Visibility = Visibility.Collapsed;
            TitleTextBlock.Text = "Map";
            MapListBoxItem.IsSelected = true;
            MyFrame.Navigate(typeof(Map));
            this._wifiScanner = new WiFiScanner();
            // Hide default title bar.
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            UpdateTitleBarLayout(coreTitleBar);

            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);

            // Register a handler for when the size of the overlaid caption control changes.
            // For example, when the app moves to a screen with a different DPI.
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            // Register a handler for when the title bar visibility changes.
            // For example, when the title bar is invoked in full screen mode.
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
            var applicationView = ApplicationView.GetForCurrentView();
            var titleBar = applicationView.TitleBar;

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.Black;
        }
        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            // Get the size of the caption controls area and back button 
            // (returned in logical pixels), and move your content around as necessary.
            LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);

            // Update title bar control size as needed to account for system size changes.
            AppTitleBar.Height = coreTitleBar.Height;
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                AppTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try{await InitializeScanner();}
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }

        private async Task InitializeScanner()
        {
            try
            {
                await this._wifiScanner.InitializeScanner();
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }

        private async void Timer_Tick(object sender, object e)
        {
            try
            {
                await RunWifiScan(); 
                if (WifiListBoxItem.IsSelected)
                {
                    MyFrame.Navigate(typeof(Map), GPScoords.GetInstance());
                    MyFrame.Navigate(typeof(WifiInfo));
                }
                else MyFrame.Navigate(typeof(Map),GPScoords.GetInstance());
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
        }

        private async Task RunWifiScan()
        {
            await this._wifiScanner.ScanForNetworks();
            
            Geolocator geolocator = new Geolocator();
            Geoposition position = await geolocator.GetGeopositionAsync();

            WiFiNetworkReport report = this._wifiScanner.WiFiAdapter.NetworkReport;

            var wifiPoint = new WiFiPointData(position.Coordinate.Timestamp,
                position.Coordinate.Point.Position.Latitude, position.Coordinate.Point.Position.Longitude);

            foreach (var availableNetwork in report.AvailableNetworks)
            {
                WiFiSignal wifiSignal = new WiFiSignal
                {
                    BeaconInterval = availableNetwork.BeaconInterval.TotalSeconds.ToString(),
                    BSSID = availableNetwork.Bssid,
                    ChannelCenterFrequencyInKilohertz = availableNetwork.ChannelCenterFrequencyInKilohertz,
                    Encryption = availableNetwork.SecuritySettings.NetworkEncryptionType.ToString(),
                    IsWiFiDirect = availableNetwork.IsWiFiDirect,
                    NetworkKind = availableNetwork.NetworkKind.ToString(),
                    PhyKind = availableNetwork.PhyKind.ToString(),
                    SignalStrength = (short)availableNetwork.NetworkRssiInDecibelMilliwatts,
                    SSID = availableNetwork.Ssid,
                    Uptime = availableNetwork.Uptime.TotalHours.ToString()
                };               
                wifiPoint.WiFiSignals.Add(wifiSignal);                
            }
            
            var db = Database.Instance;
            db.Insert(wifiPoint);
            MapData mapData = MapData.GetInstance();
            mapData.AddData(db.SelectAll());
            GPScoords gPScoords = GPScoords.GetInstance();
            gPScoords.Lat = wifiPoint.Latitude;
            gPScoords.Lon = wifiPoint.Longitude;
            gPScoords._signalsAround = wifiPoint;
        }

        private async Task ShowMessage(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
        //danmoka's page methods
        private void SplitViewON_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WifiListBoxItem.IsSelected)
            {
                BackButton.Visibility = Visibility.Visible;
                MyFrame.Navigate(typeof(WifiInfo));
                TitleTextBlock.Text = "Wifi";
            }
            else if (MapListBoxItem.IsSelected)
            {
                BackButton.Visibility = Visibility.Collapsed;
                MyFrame.Navigate(typeof(Map));
                TitleTextBlock.Text = "Map";

            }
            else if (Theme.IsSelected)
            {
                BackButton.Visibility = Visibility.Collapsed;
                TitleTextBlock.Text = "Map";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyFrame.CanGoBack)
            {
                MyFrame.GoBack();
                MapListBoxItem.IsSelected = true;
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(SearchTextBox.Text)))
            {
                string addressToGeocode = SearchTextBox.Text;
                MapData mapData = MapData.GetInstance();
                // The nearby location to use as a query hint.
                BasicGeoposition queryHint = new BasicGeoposition
                {
                    Latitude = 55.753215,
                    Longitude = 37.622504
                };
                Geopoint hintPoint = new Geopoint(queryHint);
                MapLocationFinderResult result =
                      await MapLocationFinder.FindLocationsAsync(addressToGeocode, hintPoint, 3);
                if (result.Status == MapLocationFinderStatus.Success)
                {
                    try
                    {
                        mapData.Lat = result.Locations[0].Point.Position.Latitude;
                        mapData.Lon = result.Locations[0].Point.Position.Longitude;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        MessageDialog md = new MessageDialog("По вашему запросу ничего не найдено!");
                        await md.ShowAsync();
                    }
                    MyFrame.Navigate(typeof(Map), mapData);
                }
                else
                {
                    MessageDialog md = new MessageDialog("Ничего не найдено! \nПопробуйте еще раз!");
                    await md.ShowAsync();
                }
                this.SearchTextBox.Text = "";
            } //MyMap.ColorScheme = MapColorScheme.Dark;
        }

        private void Theme_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var applicationView = ApplicationView.GetForCurrentView();
            var titleBar = applicationView.TitleBar;
            MapData mapData = MapData.GetInstance();
            CurrentColorSchemeWifiInfo currentColorSchemeWifi = CurrentColorSchemeWifiInfo.GetInstance();
            if (RequestedTheme == ElementTheme.Light || RequestedTheme == ElementTheme.Default)
            {
                RequestedTheme = ElementTheme.Dark;
                mapData.Scheme = MapColorScheme.Dark;
                titleBar.ButtonForegroundColor = Colors.DeepPink;
                currentColorSchemeWifi.ChangeValues(new NightSchemeForWifiInfo());
               
            }
            else if (RequestedTheme == ElementTheme.Dark)
            {
                RequestedTheme = ElementTheme.Light;
                mapData.Scheme = MapColorScheme.Light;
                titleBar.ButtonForegroundColor = Colors.Black;

                currentColorSchemeWifi.ChangeValues(new WhiteSchemeForWifiInfo());
            }
            MyFrame.Navigate(typeof(Map));
        }

        private async void ScanOnce_ClickAsync(object sender, RoutedEventArgs e)
        {
            ScanOnce.IsEnabled = false;
            ToogleSwitch.IsEnabled = false;
            try
            {
                await RunWifiScan();
                if (WifiListBoxItem.IsSelected)
                {
                    MyFrame.Navigate(typeof(Map), GPScoords.GetInstance());
                    MyFrame.Navigate(typeof(WifiInfo));
                }
                else MyFrame.Navigate(typeof(Map), GPScoords.GetInstance());
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
            }
            ScanOnce.IsEnabled = true;
            ToogleSwitch.IsEnabled = true;
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Timer timer = Timer.GetInstance();
            if (ToogleSwitch.IsOn==true)
            {
                timer.dispatcherTimer.Tick += Timer_Tick;//в одиночку таймер
                timer.dispatcherTimer.Start();
                ScanOnce.Visibility = Visibility.Collapsed;
            }
            else
            {
                timer.dispatcherTimer.Stop();
                ScanOnce.Visibility = Visibility.Visible;
            }
        }
    }
}