using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
        public WifiInfo()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MapData mapData = MapData.GetInstance();
            string[] infoByOne = mapData.InfoAboutSignals.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] names = infoByOne[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var infoByOne1=infoByOne.ToList();
            infoByOne1.RemoveAt(0);
            foreach (string s in infoByOne1)
            {
                string[] namecomp=s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Grid gr = new Grid();
                TextBlock tb = new TextBlock
                {
                    LineHeight = 21,
                    FontSize = 20,
                    FontFamily = new FontFamily("Verdana"),
                    Margin = new Thickness(5, 5, 5, 5),
                    Foreground = new SolidColorBrush(Colors.DimGray)
                };
                for(int i=0;i<names.Length;i++)
                {
                    tb.Text += names[i] + namecomp[i];
                }
                gr.Background = new SolidColorBrush(Colors.PowderBlue);
                gr.Children.Add(tb);
                stackPanelInfo.Children.Add(gr);
                gr = new Grid
                {
                    Height = 10
                };
                stackPanelInfo.Children.Add(gr);
            }
        }
    }
}
