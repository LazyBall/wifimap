using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Wi_Fi_Map_Data_Service
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WiFiDataService : IWiFiDataService
    {
        private readonly object _locker;
        private readonly Dictionary<string, WiFiSignalWithGeoposition> _networks;
        private readonly List<string> _orderOfAddition;

        public WiFiDataService()
        {
            _locker = new object();
            _networks = new Dictionary<string, WiFiSignalWithGeoposition>();
            _orderOfAddition = new List<string>();
        }

        public void SendWiFiData(WiFiPointData wiFiPointData)
        {
            foreach (var signal in wiFiPointData.WiFiSignals)
            {
                var wiFi = new WiFiSignalWithGeoposition(signal, wiFiPointData.TimeStamp,
                    wiFiPointData.Latitude, wiFiPointData.Longitude);
                SaveWiFiNetwork(wiFi);
            }
        }

        private void SaveWiFiNetwork(WiFiSignalWithGeoposition wiFi)
        {
            lock (_locker)
            {
                if (_networks.ContainsKey(wiFi.MacAddress))
                {
                    if (_networks[wiFi.MacAddress].NetworkRssiInDecibelMilliwatts <
                        wiFi.NetworkRssiInDecibelMilliwatts)
                    {
                        _networks[wiFi.MacAddress] = wiFi;
                    }
                }
                else
                {
                    _networks.Add(wiFi.MacAddress, wiFi);
                    _orderOfAddition.Insert(0, wiFi.MacAddress);
                }
            }
        }

        public List<WiFiSignalWithGeoposition> GetWiFiData(int numberOfWiFiOnLocalMachine)
        {
            int count = 0;
            var list = new List<WiFiSignalWithGeoposition>();
            lock (_locker)
            {
                count = _orderOfAddition.Count - numberOfWiFiOnLocalMachine;
                foreach (var macAddress in _orderOfAddition)
                {
                    if (count < 1) break;
                    list.Add(_networks[macAddress]);
                    count--;
                }
            }
            return list;
        }
    }
}