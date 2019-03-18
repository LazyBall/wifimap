using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Wi_Fi_Map
{
    public class WhiteSchemeForWifiInfo: IColorSchemeForWifiInfo
    {
        public Brush NameForeground { get; set; } = new SolidColorBrush(Colors.Black);
        public Brush ValueForeground { get; set; } = new SolidColorBrush(Colors.DarkSlateGray);
        public Brush PositionForeground { get; set; } = new SolidColorBrush(Colors.DarkOliveGreen);
        public Brush ValuePositionForeground { get; set; } = new SolidColorBrush(Colors.DarkOrange);
        //public Brush GridColor { get; set; } = new SolidColorBrush(Colors.Azure);
        public Brush GridColorForRowDelimiter { get; set; } = new SolidColorBrush(Colors.DarkBlue);

        public WhiteSchemeForWifiInfo() { }
    }
}