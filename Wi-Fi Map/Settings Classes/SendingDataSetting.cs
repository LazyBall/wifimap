using System;
using Windows.Storage;

namespace Wi_Fi_Map
{
    public sealed class SendingDataSetting
    {
        private static readonly Lazy<SendingDataSetting> _setting =
            new Lazy<SendingDataSetting>(() => new SendingDataSetting());
      
        private readonly ApplicationDataContainer _localSetting;
        private readonly string _key;

        private SendingDataSetting()
        {
            _localSetting = ApplicationData.Current.LocalSettings;
            _key = "SendingData";
        }

        public static SendingDataSetting Instance
        {
            get { return _setting.Value; }
        }

        public bool Value
        {
            get
            {
                bool value = false;
                try
                {
                    value = (bool)_localSetting.Values[_key];
                }
                catch
                {
                    this.Value = false;
                }
                return value;
            }
            set
            {
                // Save a setting locally on the device   
                _localSetting.Values[_key] = value;
            }
        }       
    }
}