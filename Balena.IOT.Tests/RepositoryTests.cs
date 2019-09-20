using System;
using System.Linq;
using Balena.Geolocation;
using Balena.IOT.Entity.Entities;
using Balena.IOT.Repository;
using NUnit.Framework;

namespace Tests
{
    public class RepositoryTests
    {
        private IRepository<DeviceTelemetry> _repo;
        
        [SetUp]
        public void Setup()
        {
            _repo = new InMemoryRepository<DeviceTelemetry>();
        }

        [Test]
        public void Insert()
        {
            var telemetry = new DeviceTelemetry()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                DeviceDate = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };

            _repo.AddAsync(telemetry);

            var fromRepo = _repo.FindByIdAsync(telemetry.Id).GetAwaiter().GetResult();
            
            Assert.AreEqual(fromRepo, telemetry);
        }
        
        [Test]
        public void AsQueryable()
        {
            var telemetry = new DeviceTelemetry()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                DeviceDate = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };

            _repo.AddAsync(telemetry);

            var fromRepo = _repo.AsQueryable().FirstOrDefault(q=>q.Id == telemetry.Id);
            
            Assert.AreEqual(fromRepo, telemetry);
        }
        
        [Test]
        public void DeleteByIdAsync()
        {
            var telemetry = new DeviceTelemetry()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                DeviceDate = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };

            _repo.AddAsync(telemetry);

            _repo.DeleteByIdAsync(telemetry.Id).GetAwaiter().GetResult();

            var result = _repo.FindByIdAsync(telemetry.Id).GetAwaiter().GetResult();
            
            Assert.IsNull(result);
        }
        
        [Test]
        public void DeleteAsync()
        {
            var telemetry = new DeviceTelemetry()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                DeviceDate = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };

            _repo.AddAsync(telemetry);

            _repo.DeleteAsync(telemetry).GetAwaiter().GetResult();

            var result = _repo.FindByIdAsync(telemetry.Id).GetAwaiter().GetResult();
            
            Assert.IsNull(result);
        }
        
             
        [Test]
        public void FindByIdAsync()
        {
            var telemetry = new DeviceTelemetry()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                DeviceDate = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };

            _repo.AddAsync(telemetry);

            var result = _repo.FindByIdAsync(telemetry.Id).GetAwaiter().GetResult();
            
            Assert.AreEqual(result, telemetry);
        }
        
    }
}