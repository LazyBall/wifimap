using Windows.UI.Xaml.Media;

namespace Wi_Fi_Map
{
    public interface IColorSchemeForWifiInfo
    {
        Brush NameForeground { get; set; }
        Brush ValueForeground { get; set; }
        Brush PositionForeground { get; set; }
        Brush ValuePositionForeground { get; set; }
        //Brush GridColor { get; set; }
        Brush GridColorForRowDelimiter { get; set; }
    }
}