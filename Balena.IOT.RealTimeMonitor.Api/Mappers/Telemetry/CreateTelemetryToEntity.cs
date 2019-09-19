using System;
using Balena.IOT.Entity.Entities;
using Balena.IOT.RealTimeMonitor.Api.Commands;

namespace Balena.IOT.RealTimeMonitor.Api.Mappers.Telemetry
{
    public static class CreateTelemetryToEntity
    {

        public static DeviceTelemetry ToEntity(this CreateTelemtryCommand @this)
        {
            return new DeviceTelemetry
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}