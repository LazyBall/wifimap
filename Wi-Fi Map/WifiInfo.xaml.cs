using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wi_Fi_Map
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class WifiInfo : Page
    {
        //Constants--------------------------------------------------------------
        double badSignal = -90;
        double normalSignal = -60;
        int textBlockLineHeight = 21;
        int textBlockSymbolFontSize = 30;
        int textBlockFontSize = 20;
        string symbolFontFamily = "Segoe MDL2 Assets";
        string fontFamily = "Verdana";
        Brush nameForeground = new SolidColorBrush(Colors.LightGray);
        Brush ValueForeground = new SolidColorBrush(Colors.MintCream);
        //-----------------------------------------------------------------------
        public WifiInfo()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            List<WiFiSignal> infoByOne = new List<WiFiSignal>(await ScanNetworkAsync());
            if (infoByOne.Count <= 0)
            {
                TextBlock tb = GetTb(textBlockLineHeight, textBlockFontSize, fontFamily, nameForeground, "Нет информации для отображения!");
                stackPanelInfo.Children.Add(tb);
            }
            foreach (var s in infoByOne)
            {
                TextBlock tbSSID = GetTb(textBlockLineHeight, textBlockFontSize, fontFamily, nameForeground, "SSID");
                TextBlock tbSignalStrength = GetTb(textBlockLineHeight, textBlockSymbolFontSize, symbolFontFamily, nameForeground, "\xEC3E");
                TextBlock tbEncryption = GetTb(textBlockLineHeight, textBlockSymbolFontSize, symbolFontFamily, nameForeground, "\xE785");
                TextBlock tbMAC = GetTb(textBlockLineHeight, textBlockFontSize, fontFamily, nameForeground, "BSSID");
                TextBlock tbLanLong = GetTb(textBlockLineHeight, textBlockSymbolFontSize, symbolFontFamily, new SolidColorBrush(Colors.Aqua), "\xE1C4");

                TextBlock tbSSIDValue = GetTb(textBlockLineHeight, textBlockFontSize, fontFamily, ValueForeground, " " + s.SSID);
                TextBlock tbSignalStrengthValue = GetTb(textBlockLineHeight, textBlockFontSize, fontFamily, ValueForeground, " " + s.SignalStrength);
                TextBlock tbEncryptionValue = GetTb(textBlockLineHeight, textBlockFontSize, fontFamily, ValueForeground, " " + s.Encryption);
                TextBlock tbMACValue = GetTb(textBlockLineHeight, textBlockFontSize, fontFamily, ValueForeground, " " + s.BSSID);

                CheckColor(s, tbSignalStrength, tbEncryption);

                WrapPanel gr = new WrapPanel();

                gr.Children.Add(tbSSID);
                gr.Children.Add(tbSSIDValue);
                gr.Children.Add(tbSignalStrength);
                gr.Children.Add(tbSignalStrengthValue);
                gr.Children.Add(tbEncryption);
                gr.Children.Add(tbEncryptionValue);
                gr.Children.Add(tbMAC);
                gr.Children.Add(tbMACValue);
                gr.Children.Add(tbLanLong);

                stackPanelInfo.Children.Add(gr);

                Grid grid = new Grid
                {
                    Height = 1,
                    Background = new SolidColorBrush(Colors.WhiteSmoke),
                    Opacity = 0.3
                    
                };

                stackPanelInfo.Children.Add(grid);
            }
        }

        private void CheckColor(WiFiSignal signal, TextBlock tbSignalStrength, TextBlock tbEncryption)
        {
            if (signal.SignalStrength <= badSignal)
            {
                tbSignalStrength.Foreground = new SolidColorBrush(Colors.Red);
                tbSignalStrength.Text = "\xEC3D";
            }
            else if (signal.SignalStrength > badSignal && signal.SignalStrength <= normalSignal)
            {
                tbSignalStrength.Foreground = new SolidColorBrush(Colors.Yellow);
                tbSignalStrength.Text = "\xEC3E";
            }
            else
            {
                tbSignalStrength.Foreground = new SolidColorBrush(Colors.Lime);
                tbSignalStrength.Text = "\xEC3F";
            }
            if (signal.Encryption != "None")
                tbEncryption.Foreground = new SolidColorBrush(Colors.Red);
            else
                tbEncryption.Foreground = new SolidColorBrush(Colors.LimeGreen);
        }

        private static TextBlock GetTb(int tblh,int tbfs,string ff,Brush brush,string text)
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
        private async Task ShowMessage(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        private async Task<IEnumerable<WiFiSignal>> ScanNetworkAsync()
        {
            WiFiScanner wifiScanner = new WiFiScanner();
            await wifiScanner.InitializeScanner();
            await wifiScanner.ScanForNetworks();
            WiFiNetworkReport report = wifiScanner.WiFiAdapter.NetworkReport;
            var list = new List<WiFiSignal>();
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
                list.Add(wifiSignal);
            }
            return list;
        }
    }
}