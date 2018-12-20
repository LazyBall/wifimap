using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Services.Maps;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.UI;

namespace Wi_Fi_Map
{ 
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            SetStartTheme();
            // RequestedTheme = ElementTheme.Light;
            //MapListBoxItem.IsSelected = true;
            //titleBar.ButtonForegroundColor = Colors.Black;
            BackButton.Visibility = Visibility.Collapsed;
            TitleTextBlock.Text = "Карта";          
            //MyFrame.Navigate(typeof(Map));
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
            
        }

        private void SetStartTheme()
        {
            WifiListBoxItem.IsSelected = false;
            MapListBoxItem.IsSelected = false;
            Theme.IsSelected = true;
            string theme = string.Empty;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            try
            {
                theme = localSettings.Values["Theme"] as string;
            }
            catch
            {
                theme = "Light";
                localSettings.Values["Theme"] = "Light";
            }
            if (theme == "Dark")
            {
                RequestedTheme = ElementTheme.Light;
            }
            else
            {
                RequestedTheme = ElementTheme.Dark;
            }
            IconsListBox_SelectionChanged(new object(), null);
            Theme.IsSelected = false;
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
      
        private void SplitViewON_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WifiListBoxItem.IsSelected)
            {
                BackButton.Visibility = Visibility.Visible;
                MyFrame.Navigate(typeof(WiFiInfo));
                TitleTextBlock.Text = "Список Wi-Fi";
            }
            else if (MapListBoxItem.IsSelected)
            {
                BackButton.Visibility = Visibility.Collapsed;
                MyFrame.Navigate(typeof(Map));
                TitleTextBlock.Text = "Карта";

            }
            else if (Theme.IsSelected)
            {
                var applicationView = ApplicationView.GetForCurrentView();
                var titleBar = applicationView.TitleBar;
                MapData mapData = MapData.GetInstance();
                CurrentColorSchemeWifiInfo currentColorSchemeWifi = CurrentColorSchemeWifiInfo.GetInstance();
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (RequestedTheme == ElementTheme.Light)
                {
                    RequestedTheme = ElementTheme.Dark;
                    mapData.Scheme = MapColorScheme.Dark;
                    titleBar.ButtonForegroundColor = Colors.DeepPink;
                    currentColorSchemeWifi.ChangeValues(new NightSchemeForWifiInfo());
                    localSettings.Values["Theme"] = "Dark";
                }
                else if (RequestedTheme == ElementTheme.Dark)
                {
                    RequestedTheme = ElementTheme.Light;
                    mapData.Scheme = MapColorScheme.Light;
                    titleBar.ButtonForegroundColor = Colors.Black;
                    currentColorSchemeWifi.ChangeValues(new WhiteSchemeForWifiInfo());
                    localSettings.Values["Theme"] = "Light";
                }
                MapListBoxItem.IsSelected = true;
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
            MapListBoxItem.IsSelected = true;
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
                        mapData.Latitude = result.Locations[0].Point.Position.Latitude;
                        mapData.Longitude = result.Locations[0].Point.Position.Longitude;
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
            } 
        }

        private void ParametersButton_Click(object sender, RoutedEventArgs e)
        {
            if ((IconsListBox.SelectedItem as ListBoxItem) != null)
            {
                (IconsListBox.SelectedItem as ListBoxItem).IsSelected = false;
            }
            BackButton.Visibility = Visibility.Visible;
            TitleTextBlock.Text = "Параметры";
            MyFrame.Navigate(typeof(ParametersPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MapListBoxItem.IsSelected = true;
        }      
    }
}