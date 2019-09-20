using System;
using System.Collections.Generic;

namespace Belane.IOT.Simulator
{
    public class MockData
    {
        private readonly List<Location> locations = new List<Location>();
        private readonly List<string> devices = new List<string>();
        
        public MockData()
        {
            locations.Add(new Location(41.732467, 44.838535));
            locations.Add(new Location(41.732447, 44.838854));
            locations.Add(new Location(41.732331, 44.838366));
            locations.Add(new Location(41.732599, 44.838511));
            locations.Add(new Location(41.732567, 44.838843));
            locations.Add(new Location(41.732599, 44.839189));
            locations.Add(new Location(41.732293, 44.838693));
            locations.Add(new Location(41.732079, 44.838632));

            devices.Add("SN12311111");
            devices.Add("SN12311112");
            devices.Add("SN12311113");
            devices.Add("SN12311114");
            devices.Add("SN12311115");
        }

        public List<string> GetDevices()
        {
            return devices;
        }

        public Location GetRandomLocation()
        {
            var random = new Random();
            int index = random.Next(locations.Count);
            return locations[index];
        }

        public Location GetStaticLocation()
        {
            return locations[0];
        }
    }
}