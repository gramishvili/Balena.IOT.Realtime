using System;

namespace Belane.IOT.Simulator
{
    public class CreateTelemetryCommand
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string SerialNumber { get; set; }
        public DateTime DeviceDate { get; set; }
    }
}