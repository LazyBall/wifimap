using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Wi_Fi_Map
{
    public class CurrentColorSchemeWifiInfo
    {
        private static CurrentColorSchemeWifiInfo _uniq;

        public IColorSchemeForWifiInfo _colorSchemeForWifiInfo =new WhiteSchemeForWifiInfo();

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
        public Brush GoodSignalForeground { get; set; } = new SolidColorBrush(Colors.Lime);
        public Brush NormalSignalForeground { get; set; } = new SolidColorBrush(Colors.Yellow);
        public Brush BadSignalForeground { get; set; }= new SolidColorBrush(Colors.Red);

        private CurrentColorSchemeWifiInfo() {}
        public static CurrentColorSchemeWifiInfo GetInstance()
        {
            if (_uniq == null)
            {
                _uniq = new CurrentColorSchemeWifiInfo();
            }
            return _uniq;
        }
        //стратегия и одиночка
        public void ChangeValues(IColorSchemeForWifiInfo colorSchemeForWifiInfo)
        {
            _colorSchemeForWifiInfo = colorSchemeForWifiInfo;
        }
    }
}
