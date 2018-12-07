using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        
        
        //-----------------------------------------------------------------------
        public WifiInfo()
        {
            this.InitializeComponent();
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrentColorSchemeWifiInfo colorScheme = CurrentColorSchemeWifiInfo.GetInstance();
            MapData mapData = MapData.GetInstance();
            GPScoords gPScoords = GPScoords.GetInstance();

            mainGrid.Background = colorScheme.GridColor;
            WiFiPointData signals = gPScoords._signalsAround;
            if (signals.WiFiSignals.Count <= 0)
            {
                TextBlock tb = GetTb(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme.NameForeground, "Нет информации для отображения!");
                stackPanelInfo.Children.Add(tb);
            }

            foreach (WiFiSignal s in signals.WiFiSignals)
            {
                TextBlock tbSSID = GetTb(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme.NameForeground, "Name");
                TextBlock tbSignalStrength = GetTb(colorScheme.TextBlockLineHeight, colorScheme.TextBlockSymbolFontSize, colorScheme.SymbolFontFamily, colorScheme.NameForeground, colorScheme.NormalSignalSymbol);
                TextBlock tbEncryption = GetTb(colorScheme.TextBlockLineHeight, colorScheme.TextBlockSymbolFontSize, colorScheme.SymbolFontFamily, colorScheme.NameForeground, colorScheme.EncriptionSymbol);
                TextBlock tbMAC = GetTb(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme.NameForeground, "MAC");
                TextBlock tbLanLong = GetTb(colorScheme.TextBlockLineHeight, colorScheme.TextBlockSymbolFontSize, colorScheme.SymbolFontFamily, colorScheme.PositionForeground, colorScheme.PositionSymbol);

                TextBlock tbSSIDValue = GetTb(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme.ValueForeground, " " + s.SSID);
                TextBlock tbSignalStrengthValue = GetTb(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme.ValueForeground, " " + s.SignalStrength);
                TextBlock tbEncryptionValue = GetTb(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme.ValueForeground, " " + s.Encryption);
                TextBlock tbMACValue = GetTb(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme.ValueForeground, " " + s.BSSID);
                TextBlock tbLanLongValue = GetTb(colorScheme.TextBlockLineHeight, colorScheme.TextBlockFontSize, colorScheme.FontFamily, colorScheme.ValuePositionForeground, " " + signals.Latitude + " : " + signals.Longitude);

                Check(s, tbSignalStrength, tbEncryption);

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
                gr.Children.Add(tbLanLongValue);

                stackPanelInfo.Children.Add(gr);

                Grid grid = new Grid
                {
                    Height = 1,
                    Background = colorScheme.GridColorForRowDelimiter,
                    Opacity = 0.3

                };

                stackPanelInfo.Children.Add(grid);
            }
        }

        private void Check(WiFiSignal s, TextBlock tbSignalStrength, TextBlock tbEncryption)
        {
            CurrentColorSchemeWifiInfo colorScheme = CurrentColorSchemeWifiInfo.GetInstance();
            if (s.SignalStrength <= colorScheme.BadSignal)
            {
                tbSignalStrength.Foreground = colorScheme.BadSignalForeground;
                tbSignalStrength.Text = colorScheme.BadSignalSymbol;
            }
            else if (s.SignalStrength > colorScheme.BadSignal && s.SignalStrength <= colorScheme.NormalSignal)
            {
                tbSignalStrength.Foreground = colorScheme.NormalSignalForeground;
                tbSignalStrength.Text = colorScheme.NormalSignalSymbol;
            }
            else
            {
                tbSignalStrength.Foreground = colorScheme.GoodSignalForeground;
                tbSignalStrength.Text = colorScheme.GoodSignalSymbol;
            }
            if (s.Encryption != "None")
                tbEncryption.Foreground = colorScheme.BadSignalForeground;
            else
                tbEncryption.Foreground = colorScheme.GoodSignalForeground;
        }

        private static TextBlock GetTb(int tblh, int tbfs, string ff, Brush brush, string text)
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
    }
}
