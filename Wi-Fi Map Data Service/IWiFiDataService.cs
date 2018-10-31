using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Wi_Fi_Map_Data_Service
{
    
    [ServiceContract]
    public interface IWiFiDataService
    {

        [OperationContract(IsOneWay = true)]
        void SendWiFiData(WiFiPointData data);

        [OperationContract]
        List<WiFiSignalWithGeoposition> GetWiFiData(int numberOfWiFiOnLocalMachine = 0);

        // TODO: Добавьте здесь операции служб
    }


    // Используйте контракт данных, как показано в примере ниже, чтобы добавить составные типы к операциям служб.


    [DataContract]
    public class WiFiSignal
    {
        [DataMember]
        public string MacAddress { get; private set; }

        [DataMember]
        public string Ssid { get; private set; }

        [DataMember]
        public double NetworkRssiInDecibelMilliwatts { get; private set; }

        [DataMember]
        public int ChannelCenterFrequencyInKilohertz { get; private set; }

        [DataMember]
        public string Encryption { get; private set; }

        public WiFiSignal() { }

        public WiFiSignal(string macAddress, string ssid, double networkRssiInDecibelMilliwatts,
            int channelCenterFrequencyInKilohertz, string encryption)
        {
            this.MacAddress = macAddress;
            this.Ssid = ssid;
            this.NetworkRssiInDecibelMilliwatts = networkRssiInDecibelMilliwatts;
            this.ChannelCenterFrequencyInKilohertz = channelCenterFrequencyInKilohertz;
            this.Encryption = encryption;
        }
    }

    [DataContract]
    public class WiFiSignalWithGeoposition : WiFiSignal
    {
        [DataMember]
        public DateTimeOffset TimeStamp { get; private set; }

        [DataMember]
        public double Latitude { get; private set; }

        [DataMember]
        public double Longitude { get; private set; }

        public WiFiSignalWithGeoposition() : base() { }

        public WiFiSignalWithGeoposition(string macAddress, string ssid, double networkRssiInDecibelMilliwatts,
            int channelCenterFrequencyInKilohertz, string encryption, DateTimeOffset timeStamp,
            double latitude, double longtitude) : base(macAddress, ssid,
                networkRssiInDecibelMilliwatts, channelCenterFrequencyInKilohertz, encryption)
        {
            this.TimeStamp = timeStamp;
            this.Latitude = latitude;
            this.Longitude = longtitude;
        }

        public WiFiSignalWithGeoposition(WiFiSignal signal, DateTimeOffset timeStamp, double latitude,
            double longtitude) : this(signal.MacAddress, signal.Ssid, signal.NetworkRssiInDecibelMilliwatts,
                signal.ChannelCenterFrequencyInKilohertz, signal.Encryption, timeStamp, latitude, longtitude)
        { }
    }

    [DataContract]
    public class WiFiPointData
    {
        [DataMember]
        public DateTimeOffset TimeStamp { get; private set; }

        [DataMember]
        public double Latitude { get; private set; }

        [DataMember]
        public double Longitude { get; private set; }

        [DataMember]
        public List<WiFiSignal> WiFiSignals { get; set; }

        public WiFiPointData()
        {
            this.WiFiSignals = new List<WiFiSignal>();
        }

        public WiFiPointData(DateTimeOffset timeStamp, double latitude, double longtitude) : this()
        {
            this.TimeStamp = timeStamp;
            this.Latitude = latitude;
            this.Longitude = longtitude;
        }
    }
}