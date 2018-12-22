using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


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
            ToogleSwitchParameters.IsOn = SendingDataSetting.Instance.Value;
        }

        private void ToogleSwitchParameters_Toggled(object sender, RoutedEventArgs e)
        {
            SendingDataSetting.Instance.Value = ToogleSwitchParameters.IsOn;
        }
    }
}