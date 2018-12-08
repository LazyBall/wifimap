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
using Windows.UI.Xaml.Documents;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wi_Fi_Map
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ParametersPage : Page
    {
        public ParametersPage()
        {
            this.InitializeComponent();
        }

        private void Hlink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            //загрузка файла соглашения и вывод на экран
        }

        private void ToogleSwitchParameters_Toggled(object sender, RoutedEventArgs e)
        {
            // если понадобится, то тут будет запоминаться значение тугла - on/off
        }
    }
}
