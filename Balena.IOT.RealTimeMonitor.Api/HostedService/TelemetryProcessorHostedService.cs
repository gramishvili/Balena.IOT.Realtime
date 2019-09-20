using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Balena.Geolocation;
using Balena.IOT.Entity.Entities;
using Balena.IOT.MessageQueue;
using Balena.IOT.Repository;
using Microsoft.Extensions.Hosting;

namespace Balena.IOT.RealTimeMonitor.Api.HostedService
{
    public class TelemetryProcessorHostedService : IHostedService
    {
        private readonly IRepository<Device> _deviceRepository;
        private readonly IMessageBroker _messageBroker;
        
        public TelemetryProcessorHostedService(IRepository<Device> deviceRepository,
            IMessageBroker messageBroker)
        {
            _deviceRepository = deviceRepository;
            _messageBroker = messageBroker;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _messageBroker.SubscribeAsync<DeviceTelemetry>(telemetry =>  TelemetryReceived(telemetry as DeviceTelemetry).GetAwaiter().GetResult());
        }

        public  Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task TelemetryReceived(DeviceTelemetry telemetry)
        {
            //query device with serial number
            var device = _deviceRepository.AsQueryable().FirstOrDefault(q => q.SerialNumber == telemetry.SerialNumber);
        
            //check if device is known
            if (device == null)
            {
                Console.WriteLine($"WARN: Received telemetry for unknown device: {telemetry.SerialNumber}");
                return;
            }

            
            //check if this telemetry is first entry for the device
            if (!device.LastKnownLatitue.HasValue || !device.LastKnownLongitude.HasValue)
            {
                device.LastKnownLongitude = telemetry.Longitude;
                device.LastKnownLatitue = telemetry.Latitude;
                device.LastContact = telemetry.DeviceDate;
                //this is first telemetry so we can not calculate speed for the device
                await _deviceRepository.UpdateAsync(device);
                return;
            }

            //calculate distance in meters
            var distance = new Coordinates(device.LastKnownLatitue.Value, device.LastKnownLongitude.Value)
                .DistanceTo(
                    new Coordinates(telemetry.Latitude, telemetry.Longitude),
                    UnitOfLength.Meter
                );
            
            //calculate speed
            var speed = GeolocationDistanceCalculator.SpeedAsMeterPerHour(distance, telemetry.DeviceDate, device.LastContact.Value);

            //set last known speed for the device
            device.LastKnownSpeed = speed;

            //check device behaviour
            device.State = distance <= DeviceConstants.MoveAlertInMeters ? 
                DeviceState.OperatingAbnormally : DeviceState.OperatingNormally;
            
            device.LastContact = telemetry.DeviceDate;
            
            //update device 
            await _deviceRepository.UpdateAsync(device);
        }
    }
}