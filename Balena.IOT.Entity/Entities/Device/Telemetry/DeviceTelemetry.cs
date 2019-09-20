using System;

namespace Balena.IOT.Entity.Entities
{
    public class DeviceTelemetry : IEntity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string SerialNumber { get; set; }
        public DateTime DeviceDate { get; set; }
        public DateTime ServerDate { get; set; }
    }
}