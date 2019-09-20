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
        private readonly IRepository<DeviceTelemetry> _repository;
        private readonly IMessageBroker _messageBroker;

        public DeviceTelemetryController(IRepository<DeviceTelemetry> _repository,
            IMessageBroker messageBroker)
        {
            this._repository = _repository;
            _messageBroker = messageBroker;
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CreateTelemtryCommand command)
        {
            //validate request
            var validationResults = await command.ValidateAsync();
            if (validationResults != null)
                return BadRequest(validationResults);

            //convert request to the core entity
            var entity = command.ToEntity();
            //save entity in the repository
            await _repository.AddAsync(entity);

            //dispatch event to process telemetry
            await _messageBroker.BroadcastAsync(entity);

            // return 201
            return CreatedAtAction(nameof(Get), new {id = entity.Id}, entity);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var entity = await _repository.FindByIdAsync(id);
            if (entity == null)
                return BadRequest();
            return Ok(entity);
        }

    }
}