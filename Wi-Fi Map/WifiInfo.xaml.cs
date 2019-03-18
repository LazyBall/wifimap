using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Wi_Fi_Map.Wi_Fi_Info_MVVM;
using System.Collections.ObjectModel;
//using System.Diagnostics;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wi_Fi_Map
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class WiFiInfo : Page
    {
        //Constants--------------------------------------------------------------
        WiFiScanner _wiFiScanner;
        WifiInfoViewModel vm = new WifiInfoViewModel();
        //-----------------------------------------------------------------------

        public WiFiInfo()
        {
            this.InitializeComponent();
            this._wiFiScanner = new WiFiScanner();
            comboBoxSort.SelectedItem = defaultTextBlock;
            DataContext = vm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            vm.RefreshWifiList();
            base.OnNavigatedTo(e);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var comparer = vm.GetComparer((comboBoxSort.SelectedItem as TextBlock)?.Text);
            vm.SortSignals(comparer);
        }

        private void RefreshWifiListButton_Click(object sender, RoutedEventArgs e)
        {
            if (!vm.RefreshStop)
            {
                vm.RefreshStop = true;
                vm.StartStopSymbol = "\xEDB5";
            }
            else
            {
                vm.RefreshStop = false;
                vm.StartStopSymbol = "\xEDB4";
                vm.RefreshWifiList();
            }
        }
    }
}