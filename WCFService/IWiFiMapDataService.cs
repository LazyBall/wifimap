using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

/* Статья по WCF http://www.devowl.net/2017/07/WCF-service-csharp-client-server.html */
namespace WCFService
{
   
    [ServiceContract]
    public interface IWiFiMapDataService
    {

        [OperationContract]
        WiFiNetwork[] GetNewData(DateTime lastUpdatedDate);

        [OperationContract(IsOneWay = true)]
        void SendData(IEnumerable<WiFiNetwork> networks);

    }

    // Контракт данных 
    [DataContract(IsReference = true)] //IsReference - https://docs.microsoft.com/ru-ru/dotnet/framework/wcf/feature-details/interoperable-object-references
    public class WiFiNetwork
    {
        [DataMember]
        public string BSSID { get; set; }

        [DataMember]
        public string SSID { get; set; }

        [DataMember]
        public string Encryption { get; set; }

        [DataMember]
        public int Frequency { get; set; }  //ChannelCenterFrequencyInKilohertz

        [DataMember]
        public double RSSI { get; set; } //NetworkRssiInDecibelMilliwatts

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        public DateTime LastDetection { get; set; }

    }

}