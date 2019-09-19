namespace Balena.IOT.Entity.Entities
{
    public class Device : IEntity
    {
        public string SerialNumber { get; set; }
        public DeviceStatus Status { get; set; }
        public DeviceState State { get; set; }
        public decimal LastKnownSpeed { get; set; }
        public decimal LifetimeAvarageSpeed { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public string Secret { get; set; }
        
    }
}