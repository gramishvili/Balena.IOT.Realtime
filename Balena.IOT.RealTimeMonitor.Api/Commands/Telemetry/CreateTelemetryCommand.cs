using System;
using System.ComponentModel.DataAnnotations;

namespace Balena.IOT.RealTimeMonitor.Api.Commands
{
    public class CreateTelemetryCommand
    {
        [Required(ErrorMessage = "Latitude is required")]
        public double Latitude { get; set; }
        [Required(ErrorMessage = "Longitude is required")]
        public double Longitude { get; set; }
        [Required(ErrorMessage = "SerialNumber is required")]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "DeviceDate is required")]
        public DateTime DeviceDate { get; set; }
    }
}