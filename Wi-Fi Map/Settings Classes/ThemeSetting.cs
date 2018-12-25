using System;
using Windows.Storage;

namespace Wi_Fi_Map
{
    sealed class ThemeSetting
    {
        private static readonly Lazy<ThemeSetting> _setting =
            new Lazy<ThemeSetting>(() => new ThemeSetting());

        private readonly ApplicationDataContainer _localContainer;
        private readonly string _key;

        private ThemeSetting()
        {
            _localContainer = ApplicationData.Current.LocalSettings;
            _key = "Theme";
        }

        public static ThemeSetting Instance
        {
            get { return _setting.Value; }
        }

        public bool ThemeIsDark
        {
            get
            {
                bool value = false;
                try
                {
                    value = (bool)_localContainer.Values[_key];
                }
                catch
                {
                    this.ThemeIsDark = value;
                }
                return value;
            }
            set
            {
                // Save a setting locally on the device   
                _localContainer.Values[_key] = value;
            }
        }
    }
}