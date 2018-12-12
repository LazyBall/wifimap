using System;
using System.IO;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            try
            {
                ToogleSwitchParameters.IsOn = (bool)localSettings.Values["SendingData"];
            }
            catch
            {
                ToogleSwitchParameters.IsOn = true;
                localSettings.Values["SendingData"] = true;
            }
        }

        private void Hlink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Agreement\Agreement.txt");
            using (StreamReader st = new StreamReader(path, Encoding.GetEncoding(1251)))
            {
                ParametersTextBlockAgreement.Text = st.ReadToEnd();
            }
            ParametersFlyoutAgreement.ShowAt((TextBlock)ParametersTextBlockLink);
        }

        private void ToogleSwitchParameters_Toggled(object sender, RoutedEventArgs e)
        {
            // если понадобится, то тут будет запоминаться значение тугла - on/off
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            // Save a setting locally on the device               
            localSettings.Values["SendingData"] = ToogleSwitchParameters.IsOn;
        }

        private void CloseFlyoutParameters_Click(object sender, RoutedEventArgs e)
        {
            ParametersFlyoutAgreement.Hide();
        }
    }
}