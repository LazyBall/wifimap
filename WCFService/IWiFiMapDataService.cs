using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IWiFiMapDataService
    {
        // TODO: Добавьте здесь операции служб

        [OperationContract]
        IEnumerable<WiFiSignalWithGeoposition> GetData();

        [OperationContract]
        void SendData(IEnumerable<WiFiSignalWithGeoposition> signals);
    }


    // Используйте контракт данных, как показано в примере ниже, чтобы добавить составные типы к операциям служб.
    [DataContract]
    public class WiFiSignalWithGeoposition
    {
        [DataMember]
        public string BSSID { get; set; }

        [DataMember]
        public string SSID { get; set; }

        [DataMember]
        public short SignalStrength { get; set; }

        [DataMember]
        public string Encryption { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }
    }    
}