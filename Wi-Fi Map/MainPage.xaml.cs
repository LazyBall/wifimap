﻿using System;
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
            ParametersPage.Toggled += ChangeTheme;
            SetStartTheme();
            BackButton.Visibility = Visibility.Collapsed;
            TitleTextBlock.Text = "Карта";          
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

        private void ChangeTheme(bool doDarkTheme)
        {
            var applicationView = ApplicationView.GetForCurrentView();
            var titleBar = applicationView.TitleBar;
            MapData mapData = MapData.GetInstance();
            CurrentColorSchemeWifiInfo currentColorSchemeWifi = CurrentColorSchemeWifiInfo.GetInstance();
            if (doDarkTheme)
            {
                RequestedTheme = ElementTheme.Dark;
                mapData.Scheme = MapColorScheme.Dark;
                titleBar.ButtonForegroundColor = Colors.DeepPink;
                currentColorSchemeWifi.ChangeValues(new NightSchemeForWifiInfo());
            }
            else
            {
                RequestedTheme = ElementTheme.Light;
                mapData.Scheme = MapColorScheme.Light;
                titleBar.ButtonForegroundColor = Colors.Black;
                currentColorSchemeWifi.ChangeValues(new WhiteSchemeForWifiInfo());
            }
        }

        private void SetStartTheme()
        {
            WifiListBoxItem.IsSelected = false;
            MapListBoxItem.IsSelected = false;
            bool doDarkTheme = false;
            if (ThemeSetting.Instance.ThemeIsDark)
            {
                doDarkTheme = true;
            }
            ChangeTheme(doDarkTheme);
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
                SearchTextBox.Visibility = Visibility.Collapsed;
                SearchButton.Visibility = Visibility.Collapsed;
            }
            else if (MapListBoxItem.IsSelected)
            {
                BackButton.Visibility = Visibility.Collapsed;
                MyFrame.Navigate(typeof(Map));
                TitleTextBlock.Text = "Карта";
                SearchTextBox.Visibility = Visibility.Visible;
                SearchButton.Visibility = Visibility.Visible;
            }
            else if (Parameters.IsSelected)
            {
                BackButton.Visibility = Visibility.Visible;
                MyFrame.Navigate(typeof(ParametersPage));
                TitleTextBlock.Text = "Параметры";
                SearchTextBox.Visibility = Visibility.Collapsed;
                SearchButton.Visibility = Visibility.Collapsed;
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
                      await MapLocationFinder.FindLocationsAsync(addressToGeocode, hintPoint, 5);
                if (result.Status == MapLocationFinderStatus.Success)
                {
                    if (result.Locations.Count > 1)
                    {
                        string message = string.Empty;
                        foreach (var i in result.Locations)
                        {
                            message += i.DisplayName + "\n";
                        }
                        MessageDialog md = new MessageDialog(message + "\nУточните адрес и попробуйте еще раз!");
                        await md.ShowAsync();
                    }
                    else
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
                }
                else if (result.Status == MapLocationFinderStatus.NetworkFailure)
                {
                    MessageDialog md = new MessageDialog("Проверьте наличие доступа в сеть.");
                    await md.ShowAsync();
                }
                else if (result.Status == MapLocationFinderStatus.BadLocation)
                {
                    MessageDialog md = new MessageDialog("Указанную точку нельзя преобразовать в расположение. Попробуйте другой адрес!");
                    await md.ShowAsync();
                }
                else
                {
                    MessageDialog md = new MessageDialog("Ничего не найдено. Попробуйте еще раз!");
                    await md.ShowAsync();
                }
                this.SearchTextBox.Text = "";
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MapListBoxItem.IsSelected = true;
        }                
    }
}