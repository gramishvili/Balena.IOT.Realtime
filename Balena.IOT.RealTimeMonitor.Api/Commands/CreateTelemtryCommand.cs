using System;

namespace Balena.IOT.RealTimeMonitor.Api.Commands
{
    public class CreateTelemtryCommand : BaseCreateCommand
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string SerialNumber { get; set; }
        public DateTime DeviceDate { get; set; }
    }
}