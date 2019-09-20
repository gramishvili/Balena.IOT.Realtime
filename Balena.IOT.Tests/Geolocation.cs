using System;
using Balena.Geolocation;
using NUnit.Framework;

namespace Tests
{
    public class GeolocationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SpeedAsMeterPerHour()
        {
            //Scenario 
            //assume subject moves 1 meter per second
            // calculates speed meters per hour
            
            var distance = 1; //1 meter
            var shouldBe = distance * 60 * 60;  // object should move 3600 meter per hour, distance * second * minute
            
            var date1 = DateTime.UtcNow;
            var date2 = date1.AddSeconds(1);

            var speed = GeolocationDistanceCalculator.SpeedAsMeterPerHour(distance, date2, date1);
            
            Assert.AreEqual(speed, shouldBe);
        }

        [Test]
        public void DistanceAsMeter()
        {
            var lat1 = 41.736650;
            var long1 = 44.841127;

            var lat2 = 41.736146;
            var long2 = 44.840376;

            var distance = new Coordinates(lat1, long1)
                .DistanceTo(
                    new Coordinates(lat2, long2),
                    UnitOfLength.Meter
                );
            
            Assert.AreEqual(distance, 83.804266092328888);
        }
    }
}