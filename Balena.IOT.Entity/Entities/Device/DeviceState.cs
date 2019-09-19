using System;

namespace Balena.IOT.Entity.Entities
{
    public enum DeviceState : UInt16
    {
        OperatingNormally = 1,
        OperatingAbnormally = 99
    }
}