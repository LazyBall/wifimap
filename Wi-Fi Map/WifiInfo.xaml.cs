using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
            this.txbReport.Text = mapData.InfoAboutSignals;
            //if(e.Parameter!=null) this.txbReport.Text = e.Parameter.ToString();
            //MapData mapData = e.Parameter as MapData;
            //if (e.Parameter != null)
            //{
            //    MapData mapData = MapData.GetInstance();
            //    if (mapData._addSignals.Count() > 0)
            //    {
            //        this.AddPointsToMap(mapData._addSignals);
            //        mapData._addSignals.Clear();
            //    }
            //    if (mapData._removeSignals.Count() > 0)
            //    {
            //        this.RemovePointsFromMap(mapData._removeSignals);
            //        mapData._removeSignals.Clear();
            //    }
            //}
        }
    }
}
