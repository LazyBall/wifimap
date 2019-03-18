using Wi_Fi_Map.Map_MVVM;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wi_Fi_Map
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    sealed partial class ParametersPage : Page
    {
        public delegate void ToogleThemeHandler(bool doDarkTheme);
        public static event ToogleThemeHandler Toggled;

        public ParametersPage()
        {
            this.InitializeComponent();
            ToogleSwitchSendingData.IsOn = SendingDataSetting.Instance.DataIsSent;
            ToogleSwitchDarkTheme.IsOn = ThemeSetting.Instance.ThemeIsDark;
        }

        private void ToogleSwitchSendingData_Toggled(object sender, RoutedEventArgs e)
        {
            ToogleSwitchSendingData.IsEnabled = false;
            SendingDataSetting.Instance.DataIsSent = ToogleSwitchSendingData.IsOn;
            ToogleSwitchSendingData.IsEnabled = true;
        }
       
        private void ToogleSwitchDarkTheme_Toggled(object sender, RoutedEventArgs e)
        {                    
            ToogleSwitchDarkTheme.IsEnabled = false;
            ThemeSetting.Instance.ThemeIsDark = ToogleSwitchDarkTheme.IsOn;
            Toggled?.Invoke(ToogleSwitchDarkTheme.IsOn);                    
            ToogleSwitchDarkTheme.IsEnabled = true;

            //Раньше нужно было перейти на страницу с картой, чтобы обновить тему карты. Теперь при изменении 
            //во ViewModel цветовая схема карты пробиндится и сразу изменится
            //MapViewModel vm=MapViewModel.GetInstance();
            //vm.ColorScheme=(vm.ColorScheme == MapColorScheme.Light) ? MapColorScheme.Dark : MapColorScheme.Light;
        }
    }
}