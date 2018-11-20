using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
            List<string> infoByOne = mapData.InfoAboutSignals.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string names = infoByOne[0];
            names = Regex.Replace(names, "[,\r\n]", " ");
            string[] namesar=names.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            infoByOne.RemoveAt(0);

            foreach (string s in infoByOne)
            {
                string sReplaced = s;
                sReplaced= Regex.Replace(sReplaced,"[,\r\n]", " ");
                string[] namecomp=sReplaced.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Grid gr = new Grid();
                TextBlock tb = new TextBlock
                {
                    LineHeight = 21,
                    FontSize = 20,
                    FontFamily = new FontFamily("Verdana"),
                    Margin = new Thickness(5, 5, 5, 5),
                    Foreground = new SolidColorBrush(Colors.DimGray),
                    TextWrapping = TextWrapping.Wrap
                };
                for(int i=0;i<namesar.Length;i++)
                {
                    tb.Text += namesar[i] + ": " + namecomp[i] + ' ';
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
