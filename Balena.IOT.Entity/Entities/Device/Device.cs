using System;

namespace Balena.IOT.Entity.Entities
{
    public class Device : IEntity
    {
        public string SerialNumber { get; set; }
        public DeviceState State { get; set; }
        public double? LastKnownSpeed { get; set; }
        public double? LastKnownLatitue { get; set; }
        public double? LastKnownLongitude { get; set; }
        public DateTime? LastContact { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public DeviceType Type { get; set; }
    }
}