using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Wi_Fi_Map.Wi_Fi_Info_MVVM
{
    public class WifiSignalModel:WiFiSignal, INotifyPropertyChanged
    {
        private string bssid = string.Empty;
        public override string BSSID
        {
            get { return bssid; }
            set
            {
                bssid = value;
                Notify("BSSID");
            }
        }

        private  string ssid = string.Empty;
        public override string SSID
        {
            get { return ssid; }
            set
            {
                ssid = value;
                Notify("SSID");
            }
        }

        private string encryption = string.Empty;
        public override string Encryption
        {
            get { return encryption; }
            set
            {
                encryption = value;
                Notify("Encryprion");
            }
        }

        private short signalstrength = 0;
        public override short SignalStrength
        {
            get { return signalstrength; }
            set
            {
                signalstrength = value;
                Notify("SignalStrength");
            }
        }

        private Brush strengthBrush;
        public Brush StrengthBrush
        {
            get { return strengthBrush; }
            set
            {
                strengthBrush = value;
                Notify("StrengthBrush");
            }
        }

        private Brush encryptionBrush;
        public Brush EncryptionBrush
        {
            get { return encryptionBrush; }
            set
            {
                encryptionBrush = value;
                Notify("EncryptionBrush");
            }
        }

        private string encryptionSymbol = "\xE72E";
        public string EncryptionSymbol
        {
            get { return encryptionSymbol; }
            set
            {
                encryptionSymbol = value;
                Notify("EncryptionSymbol");
            }
        }

        private string strengthSymbol = "\xEC3E";
        public string StrengthSymbol
        {
            get { return strengthSymbol; }
            set
            {
                strengthSymbol = value;
                Notify("StrengthSymbol");
            }
        }

        //NotifyProperty реализация
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string f)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(f));
        }
    }
}
