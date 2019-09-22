using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Belane.IOT.Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Delay(TimeSpan.FromSeconds(4)).GetAwaiter().GetResult();
            Console.WriteLine("INFO: Simulator -  Initializing simulator");
            var data = new MockData();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:5000/api/");
                var devices = data.GetDevices();
                while (true)
                {
                    for (int i = 0; i < devices.Count; i++)
                    {
                        var location = data.GetRandomLocation();
                        var device = devices[i];

                        var command = new CreateTelemetryCommand
                        {
                            SerialNumber =  device,
                            DeviceDate =  DateTime.UtcNow,
                            Latitude = location.Latitude,
                            Longitude = location.Longitude
                        };

                        var result = httpClient.PutAsJsonAsync("v1/device/telemetry", command).GetAwaiter().GetResult();

                        if (result.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"INFO: Simulator - Successfully sent telemetry for the device - {device}");
                        }
                        else
                        {
                            Console.WriteLine($"ERROR: Simulator - Error sending telemetry for the device - {device}, " +
                                              $"error: {result.Content.ReadAsStreamAsync().GetAwaiter().GetResult()}");
                        }
                    }

                    Task.Delay(TimeSpan.FromSeconds(10)).GetAwaiter().GetResult();
                }
            }
        }
    }
}