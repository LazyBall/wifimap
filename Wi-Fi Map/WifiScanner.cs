using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;

namespace Wi_Fi_Map
{
    public class WiFiScanner
    {
        public WiFiAdapter WiFiAdapter { get; private set; }

        public async Task InitializeScanner()
        {
            await InitializeFirstAdapter();
        }

        private async Task InitializeFirstAdapter()
        {
            var access = await WiFiAdapter.RequestAccessAsync();

            if (access != WiFiAccessStatus.Allowed)
            {
                throw new Exception("WiFiAccessStatus not allowed");
            }
            else
            {
                var wifiAdapterResults = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());

                if (wifiAdapterResults.Count >= 1)
                {
                    this.WiFiAdapter = await WiFiAdapter.FromIdAsync(wifiAdapterResults[0].Id);
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
                var startTime = DateTime.Now;
                await this.WiFiAdapter.ScanAsync();
                var endTime = DateTime.Now;

                var duration = endTime - startTime;

                var time = duration.ToString();
            }
        }
    }
}