using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace WCFService
{
    //Если не будет запускаться, то в свойствах Wi-Fi AzureCloudService "Интернет" ->
    //"Эмулятор" -> "Использовать полный эмулятор"

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WiFiMapDataService : IWiFiMapDataService
    {
        //С помощью опции задаём, к какой бд подключаемся (модели данных при смене бд должны быть одинаковы)
        readonly DbContextOptions<WiFiNetworkContext> options = (new DbContextOptionsBuilder<WiFiNetworkContext>())
            .UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
            .Options;

        public WiFiNetwork[] GetNewData(DateTime lastUpdatedDate)
        {
            using (var db = new WiFiNetworkContext(options))
            {
                var periodUpdate = lastUpdatedDate.ToUniversalTime() - new TimeSpan(0, 0, 30);
                return db.WiFiNetworks.
                    Where(t => t.LastDetection > periodUpdate).ToArray();
            }
        }

        public void SendData(IEnumerable<WiFiNetwork> networks)
        {
            using (var db = new WiFiNetworkContext(options))
            {

                foreach (var network in networks)
                {
                    if (CheckData(network))
                    {
                        network.BSSID = network.BSSID.ToUpper();
                        network.LastDetection = network.LastDetection.ToUniversalTime();
                        var stored = db.WiFiNetworks.Find(network.BSSID);
                        if (stored != null &&
                            stored.LastDetection < network.LastDetection)
                        {
                            stored.SSID = network.SSID;
                            stored.Encryption = network.Encryption;
                            stored.Frequency = network.Frequency;
                            stored.LastDetection = network.LastDetection;
                            if (Math.Abs(stored.RSSI - network.RSSI) < 5)
                            {
                                stored.RSSI = network.RSSI;
                                stored.Latitude = network.Latitude;
                                stored.Longitude = network.Longitude;
                            }
                        }
                        else
                        {
                            db.WiFiNetworks.Add(network);
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        private bool CheckData(WiFiNetwork network)
        {
            if (network.BSSID == null || network.SSID == null || network.Encryption == null) return false;
            if (network.Latitude < (-90) && network.Latitude > (90)) return false;
            if (network.Longitude < (-180) && network.Longitude > (180)) return false;
            if (network.Frequency < 0) return false;
            string pattern = @"[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}";
            if (!Regex.IsMatch(network.BSSID, pattern, RegexOptions.IgnoreCase)) return false;
            return true;
        }
    }  
}