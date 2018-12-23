﻿using System;
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
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            try
            {
                ToogleSwitchParameters.IsOn = (bool)localSettings.Values["SendingData"];
            }
            catch
            {
                ToogleSwitchParameters.IsOn = false;
                localSettings.Values["SendingData"] = false;
            }
             Toggled+= new MainPage().ChangeTheme;
        }

        private void ToogleSwitchParameters_Toggled(object sender, RoutedEventArgs e)
        {
            // если понадобится, то тут будет запоминаться значение тугла - on/off
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            // Save a setting locally on the device               
            localSettings.Values["SendingData"] = ToogleSwitchParameters.IsOn;
        }
        public delegate void ToogleThemeHandler();
        public event ToogleThemeHandler Toggled;

        private void ToogleSwitchTheme_Toggled(object sender, RoutedEventArgs e)
        {
            //можно сделать как с туглом для соглашения, типа если выключен то это светлая тема, включен - темная и запомнить положение тугла
            Toggled?.Invoke();
        }
    }
}