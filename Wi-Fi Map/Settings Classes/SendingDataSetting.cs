﻿using System;
using Windows.Storage;

namespace Wi_Fi_Map
{
    sealed class SendingDataSetting
    {
        private static readonly Lazy<SendingDataSetting> _setting =
            new Lazy<SendingDataSetting>(() => new SendingDataSetting());

        private readonly ApplicationDataContainer _localContainer;
        private readonly string _key;

        private SendingDataSetting()
        {
            _localContainer = ApplicationData.Current.LocalSettings;
            _key = "SendingData";
        }

        public static SendingDataSetting Instance
        {
            get { return _setting.Value; }
        }

        public bool DataIsSent
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
                    this.DataIsSent = value;
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