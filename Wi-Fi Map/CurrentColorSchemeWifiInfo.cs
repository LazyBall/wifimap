using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Wi_Fi_Map
{
    public class CurrentColorSchemeWifiInfo
    {
        private static CurrentColorSchemeWifiInfo _uniq;

        public double BadSignal { get; set; } = -82;
        public double NormalSignal { get; set; } = -60;
        public int TextBlockLineHeight { get; set; } = 21;
        public int TextBlockSymbolFontSize { get; set; } = 30;
        public int TextBlockFontSize { get; set; } = 20;
        public string BadSignalSymbol { get; set; } = "\xEC3D";
        public string NormalSignalSymbol { get; set; } = "\xEC3E";
        public string GoodSignalSymbol { get; set; } = "\xEC3F";
        public string EncriptionSymbol { get; set; } = "\xE785";
        public string PositionSymbol { get; set; } = "\xE1C4";
        public string SymbolFontFamily { get; set; } = "Segoe MDL2 Assets";
        public string FontFamily { get; set; } = "Verdana";
        public Brush NameForeground { get; set; } = new SolidColorBrush(Colors.Black);
        public Brush ValueForeground { get; set; } = new SolidColorBrush(Colors.DarkSlateGray);
        public Brush PositionForeground { get; set; } = new SolidColorBrush(Colors.DarkOliveGreen);
        public Brush ValuePositionForeground { get; set; } = new SolidColorBrush(Colors.DarkOrange);
        public Brush GoodSignalForeground { get; set; } = new SolidColorBrush(Colors.Lime);
        public Brush NormalSignalForeground { get; set; } = new SolidColorBrush(Colors.Yellow);
        public Brush BadSignalForeground { get; set; }= new SolidColorBrush(Colors.Red);
        public Brush GridColor { get; set; } = new SolidColorBrush(Colors.LightCyan);
        public Brush GridColorForRowDelimiter { get; set; } = new SolidColorBrush(Colors.DarkBlue);

        private CurrentColorSchemeWifiInfo() {}
        public static CurrentColorSchemeWifiInfo GetInstance()
        {
            if (_uniq == null)
            {
                _uniq = new CurrentColorSchemeWifiInfo();
            }
            return _uniq;
        }

        public void ChangeValues(IColorSchemeForWifiInfo colorSchemeForWifiInfo)
        {
            NameForeground = colorSchemeForWifiInfo.NameForeground;
            ValueForeground = colorSchemeForWifiInfo.ValueForeground;
            PositionForeground = colorSchemeForWifiInfo.PositionForeground;
            ValuePositionForeground = colorSchemeForWifiInfo.ValuePositionForeground;
            GridColor = colorSchemeForWifiInfo.GridColor;
            GridColorForRowDelimiter = colorSchemeForWifiInfo.GridColorForRowDelimiter;
        }
    }
}
