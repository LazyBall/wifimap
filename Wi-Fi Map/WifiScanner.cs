using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;

namespace Wi_Fi_Map
{
    class WifiScanner
    {
        public WiFiAdapter WiFiAdapter { get; private set; }

        private async Task InitializeFirstAdapter()
        {
            var access = await WiFiAdapter.RequestAccessAsync();

            if (access != WiFiAccessStatus.Allowed)
            {
                throw new Exception("WiFiAccessStatus not allowed");
            }
            else
            {
                var wifiAdapterResults = await DeviceInformation.
                  FindAllAsync(WiFiAdapter.GetDeviceSelector());

                if (wifiAdapterResults.Count >= 1)
                {
                    this.WiFiAdapter = await WiFiAdapter.FromIdAsync(
                      wifiAdapterResults[0].Id);
                }
                else
                {
                    throw new Exception("WiFi Adapter not found.");
                }
            }
        }

        public async Task ScanForNetworks()
        {
            if (this.WiFiAdapter != null)
            {
                await this.WiFiAdapter.ScanAsync();
            }
        }


    }
}