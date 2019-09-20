using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Balena.IOT.Entity.Entities;
using Balena.IOT.Repository;
using Microsoft.Extensions.Hosting;

namespace Balena.IOT.RealTimeMonitor.Api.HostedService
{
    public class FakeDataHostedService : IHostedService
    {
        private readonly IRepository<Device> _deviceRepository;

        public FakeDataHostedService(IRepository<Device> deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var devices = new List<Device>
            {
                new Device
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    Model = "BLNUMB1",
                    Name = "Balena following umbrella drone",
                    SerialNumber = "SN12311111",
                    Type = DeviceType.Drone
                },
                new Device
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    LastKnownSpeed = 5,
                    Model = "BLNPOL12",
                    Name = "Balena police drone",
                    SerialNumber = "SN12311112",
                    Type = DeviceType.Drone
                },
                new Device
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    LastKnownSpeed = 5,
                    Model = "BLNCLM1",
                    Name = "Balena climate monitor drone",
                    SerialNumber = "SN12311113",
                    Type = DeviceType.Drone
                },
                new Device
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    LastKnownSpeed = 5,
                    Model = "BLNEXT1",
                    Name = "Balena communication extender drone",
                    SerialNumber = "SN12311114",
                    Type = DeviceType.Drone
                },
                new Device
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    LastKnownSpeed = 5,
                    Model = "BLNUMB1",
                    Name = "Balena umbrella drone",
                    SerialNumber = "SN12311115",
                    Type = DeviceType.Drone
                }
            };

            await _deviceRepository.AddManyAsync(devices);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}