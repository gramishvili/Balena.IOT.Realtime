using System;
using System.Collections.Generic;
using Balena.IOT.Entity.Entities;

namespace Balena.Fakes
{
    public static class FakeDataProvider
    {
        public static void  FakeDevice(this List<Device> devices)
        {
            devices.Add(new Device
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow,
                LastKnownSpeed = 5,
                Model = "BLNUMB1",
                Name = "Balena umbrella drone",
                Secret = "secret",
                SerialNumber = "SN123",
                State = DeviceState.OperatingNormally,
                Status = DeviceStatus.Offline
                
            });
        }
    }
}