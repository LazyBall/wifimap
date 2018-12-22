using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wi_Fi_Map
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class WiFiInfo : Page
    {
        //Constants--------------------------------------------------------------
        WiFiScanner _wiFiScanner;

        //-----------------------------------------------------------------------

        public WiFiInfo()
        {
            this.InitializeComponent();
            this._wiFiScanner = new WiFiScanner();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrentColorSchemeWifiInfo colorScheme = CurrentColorSchemeWifiInfo.GetInstance();
            mainGrid.Background = colorScheme._colorSchemeForWifiInfo.GridColor;
            RefreshWifiListButton_Click(RefreshWifiListButton, new RoutedEventArgs());
        }
          
        private async void RefreshWifiListButton_Click(object sender, RoutedEventArgs e)
        {
            //обработка события нажатия на кнопку обновить
            //var timer = new Stopwatch();
            //timer.Start();
            var bt = sender as Control;
            if (bt != null) bt.IsEnabled = false;
            stackPanelInfo.Children.Clear();
            if (_wiFiScanner.WiFiAdapter == null)
            {
                try
                {
                    await _wiFiScanner.InitializeScanner();
                }
                catch
                {                    
                    var dialog = new MessageDialog("Проверьте, влючен ли Wi-Fi.");
                    await dialog.ShowAsync();
                    AddVisualInfo(new List<WiFiSignal>(0));
                }
            }
            if (_wiFiScanner.WiFiAdapter != null)
            {
                var list = await GetWiFiSignalsData();
                Thread thread = new Thread(() => SendDataToDatabase(list));
                thread.Start();
                //Task.Run(() => SendDataToDatabase(list)).GetAwaiter();

                AddVisualInfo(list);               
            }
            if (bt != null) bt.IsEnabled = true;
            //timer.Stop();
            //var dialog1 = new MessageDialog(String.Format("Завершаю обновление {0}", timer.ElapsedMilliseconds));
            //await dialog1.ShowAsync();
        }

        private void AddVisualInfo(IEnumerable<WiFiSignal> signals)
        {
            CurrentColorSchemeWifiInfo colorScheme = CurrentColorSchemeWifiInfo.GetInstance();
            int count = 0;
            foreach (WiFiSignal s in signals)
            {
                count++;
                WrapPanel panel = new WrapPanel();               
                TextBlock tbSSID = GetTextBlockInFormat(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme._colorSchemeForWifiInfo.NameForeground, "SSID: ");
                TextBlock tbSignalStrength = GetTextBlockInFormat(colorScheme.TextBlockLineHeight, colorScheme.TextBlockSymbolFontSize, colorScheme.SymbolFontFamily, colorScheme._colorSchemeForWifiInfo.NameForeground, colorScheme.NormalSignalSymbol);
                TextBlock tbEncryption = GetTextBlockInFormat(colorScheme.TextBlockLineHeight, colorScheme.TextBlockSymbolFontSize, colorScheme.SymbolFontFamily, colorScheme._colorSchemeForWifiInfo.NameForeground, colorScheme.EncriptionSymbol);
                TextBlock tbMAC = GetTextBlockInFormat(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme._colorSchemeForWifiInfo.NameForeground, "BSSID:");
                TextBlock tbSSIDValue = GetTextBlockInFormat(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme._colorSchemeForWifiInfo.ValueForeground, " " + s.SSID);
                TextBlock tbSignalStrengthValue = GetTextBlockInFormat(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme._colorSchemeForWifiInfo.ValueForeground, " " + s.SignalStrength);
                TextBlock tbEncryptionValue = GetTextBlockInFormat(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme._colorSchemeForWifiInfo.ValueForeground, " " + s.Encryption);
                TextBlock tbMACValue = GetTextBlockInFormat(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme._colorSchemeForWifiInfo.ValueForeground, " " + s.BSSID);
                Check(s, tbSignalStrength, tbEncryption);
                panel.Children.Add(tbSSID);
                panel.Children.Add(tbSSIDValue);
                panel.Children.Add(tbSignalStrength);
                panel.Children.Add(tbSignalStrengthValue);
                panel.Children.Add(tbEncryption);
                panel.Children.Add(tbEncryptionValue);
                panel.Children.Add(tbMAC);
                panel.Children.Add(tbMACValue);

                Grid grid = new Grid
                {
                    Height = 1,
                    Background = colorScheme._colorSchemeForWifiInfo.GridColorForRowDelimiter,
                    Opacity = 0.3

                };
                stackPanelInfo.Children.Add(panel);
                stackPanelInfo.Children.Add(grid);
            }
            if (count == 0)
            {
                TextBlock tb = GetTextBlockInFormat(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme._colorSchemeForWifiInfo.NameForeground, "Нет информации для отображения!");
                stackPanelInfo.Children.Add(tb);
            }
        }

        private static TextBlock GetTextBlockInFormat(int tblh, int tbfs, string ff, Brush brush, string text)
        {
            return new TextBlock
            {
                LineHeight = tblh,
                FontSize = tbfs,
                FontFamily = new FontFamily(ff),
                Margin = new Thickness(5, 5, 5, 5),
                Foreground = brush,
                TextWrapping = TextWrapping.NoWrap,
                Text = text
            };
        }

        private static void Check(WiFiSignal signal, TextBlock tbSignalStrength, TextBlock tbEncryption)
        {
            CurrentColorSchemeWifiInfo colorScheme = CurrentColorSchemeWifiInfo.GetInstance();
            if (signal.SignalStrength <= colorScheme.BadSignal)
            {
                tbSignalStrength.Foreground = colorScheme.BadSignalForeground;
                tbSignalStrength.Text = colorScheme.BadSignalSymbol;
            }
            else if (signal.SignalStrength > colorScheme.BadSignal && signal.SignalStrength <= colorScheme.NormalSignal)
            {
                tbSignalStrength.Foreground = colorScheme.NormalSignalForeground;
                tbSignalStrength.Text = colorScheme.NormalSignalSymbol;
            }
            else
            {
                tbSignalStrength.Foreground = colorScheme.GoodSignalForeground;
                tbSignalStrength.Text = colorScheme.GoodSignalSymbol;
            }
            if (signal.Encryption != "None")
                tbEncryption.Foreground = colorScheme.BadSignalForeground;
            else
                tbEncryption.Foreground = colorScheme.GoodSignalForeground;
        }

        private async Task<IEnumerable<WiFiSignal>> GetWiFiSignalsData()
        {
            await this._wiFiScanner.ScanForNetworks();
            return await Task.Run(() =>
            {
                var signals = new List<WiFiSignal>();
                foreach (var availableNetwork in _wiFiScanner.WiFiAdapter.NetworkReport.AvailableNetworks)
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
                    signals.Add(wifiSignal);
                }
                return signals;
            });
        }

        private static async void SendDataToDatabase(IEnumerable<WiFiSignal> signals)
        {
            if (SendingDataSetting.Instance.Value)
            {
                try
                {
                    Geolocator geolocator = new Geolocator();
                    Geoposition position = await geolocator.GetGeopositionAsync();
                    double latitude = position.Coordinate.Point.Position.Latitude,
                       longitude = position.Coordinate.Point.Position.Longitude;
                    var list = new List<WiFiSignalWithGeoposition>();
                    foreach (var signal in signals)
                    {
                        list.Add(new WiFiSignalWithGeoposition(signal, latitude, longitude));
                    }
                    var db = new Database();
                    db.AddSignals(list);
                }
                catch
                {
                    return;
                }
            }
        }        
    }
}