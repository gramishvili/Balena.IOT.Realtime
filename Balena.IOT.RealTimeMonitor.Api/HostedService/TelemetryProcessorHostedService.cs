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
            _messageBroker.SubscribeAsync<DeviceTelemetry>(telemetry =>  TelemetryReceived(telemetry).GetAwaiter().GetResult());
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
                //this is first telemetry so we can not calculate speed for the device
                await _deviceRepository.UpdateAsync(device);
                return;
            }

            //calculate distance between last and current telemetry in meters
            var distanceMeter =  GeolocationHelpers.DistanceAsMeter(device.LastKnownLatitue.Value, device.LastKnownLongitude.Value,
                telemetry.Latitude, telemetry.Longitude);

            //calculate speed
            var speed = GeolocationHelpers.SpeedAsKMH(distanceMeter, device.LastContact.Value, telemetry.DeviceDate);

            //set last known speed for the device
            device.LastKnownSpeed = speed;

            //check device behaviour
            device.State = distanceMeter <= DeviceConstants.MoveAlertInMeters ? 
                DeviceState.OperatingAbnormally : DeviceState.OperatingNormally;
            
            //update device 
            await _deviceRepository.UpdateAsync(device);
        }
    }
}