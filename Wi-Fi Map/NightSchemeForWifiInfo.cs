using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Wi_Fi_Map
{
    public class NightSchemeForWifiInfo: IColorSchemeForWifiInfo
    {
        public Brush NameForeground { get; set; } = new SolidColorBrush(Colors.LightGray);
        public Brush ValueForeground { get; set; } = new SolidColorBrush(Colors.MintCream);
        public Brush PositionForeground { get; set; } = new SolidColorBrush(Colors.Aqua);
        public Brush ValuePositionForeground { get; set; } = new SolidColorBrush(Colors.LightPink);
        public Brush GridColor { get; set; } = new SolidColorBrush(Colors.DarkSlateGray);
        public Brush GridColorForRowDelimiter { get; set; } = new SolidColorBrush(Colors.WhiteSmoke);

        public NightSchemeForWifiInfo() { }
    }
}
