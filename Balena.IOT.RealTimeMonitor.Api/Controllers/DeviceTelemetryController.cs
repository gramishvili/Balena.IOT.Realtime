using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Balena.IOT.Entity.Entities;
using Balena.IOT.MessageQueue;
using Balena.IOT.RealTimeMonitor.Api.Commands;
using Balena.IOT.RealTimeMonitor.Api.Mappers.Telemetry;
using Balena.IOT.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Balena.IOT.RealTimeMonitor.Api.Controllers
{
    [Route("api/v1/device/telemetry")]
    [ApiController]
    public class DeviceTelemetryController : ControllerBase
    {
        private readonly IRepository<DeviceTelemetry> _telemetryRepository;
        private readonly IRepository<Device> _deviceRepository;
        private readonly IMessageBroker _messageBroker;

        public DeviceTelemetryController(IRepository<DeviceTelemetry> telemetryRepository,
            IRepository<Device> deviceRepository,
            IMessageBroker messageBroker)
        {
            _telemetryRepository = telemetryRepository;
            _deviceRepository = deviceRepository;
            _messageBroker = messageBroker;
        }

        /// <summary>
        /// create new telemetry
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CreateTelemetryCommand command)
        {
            //validate request   
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            //ensure telemetry is for existing device
            var device = _deviceRepository.AsQueryable().FirstOrDefault(q => q.SerialNumber == command.SerialNumber);
            
            if(device == null)
            {
                ModelState.AddModelError("SerialNumber", "Device with this serial number doesn't exist");
                return BadRequest(ModelState);
            }

            //convert request to the core entity
            var entity = command.ToCoreEntity();
            
            //save entity in the telemetryRepository
            await _telemetryRepository.AddAsync(entity);

            //dispatch event to process telemetry
            await _messageBroker.BroadcastAsync(entity);

            // return 201
            return CreatedAtAction(nameof(Get), new {id = entity.Id}, entity);
        }
        
        /// <summary>
        /// get telemetry by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeviceTelemetry</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var entity = await _telemetryRepository.FindByIdAsync(id);
            if (entity == null)
                return BadRequest();
            return Ok(entity);
        }

        /// <summary>
        /// list telemetry
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns>Ienumerable<DeviceTelemetry></returns>
        [HttpGet]
        public ActionResult Get(long take = 0, long skip = 0)
        {
            return Ok(_telemetryRepository.AsQueryable().ToList());
        }
    }
}