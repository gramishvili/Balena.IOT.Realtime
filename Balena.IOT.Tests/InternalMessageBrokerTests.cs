using System;
using System.Linq;
using Balena.Geolocation;
using Balena.IOT.Entity.Entities;
using Balena.IOT.MessageQueue;
using NUnit.Framework;

namespace Tests
{
    public class InternalMessageBrokerTests
    {
        private InternalMessageBroker broker;

        [SetUp]
        public void Setup()
        {
            broker = new InternalMessageBroker();
        }

        [Test]
        public void Subscribe()
        {
            var action = new Action<IEntity>(telemetry => { });

            broker.SubscribeAsync<DeviceTelemetry>(action).GetAwaiter().GetResult();

            var handlers = broker.MessageTypeSubscribersAsync<DeviceTelemetry>().GetAwaiter().GetResult();

            Assert.AreEqual(action, handlers.FirstOrDefault());
        }

        [Test]
        public void Broadcast()
        {
            try
            {
                var action = new Action<IEntity>(telemetry => { });
                broker.SubscribeAsync<DeviceTelemetry>(action).GetAwaiter().GetResult();
                broker.BroadcastAsync(new DeviceTelemetry()).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, but got: " + e.Message);
            }
        }
    }
}