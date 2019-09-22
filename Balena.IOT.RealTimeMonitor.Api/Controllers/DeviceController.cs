using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Balena.IOT.Entity.Entities;
using Balena.IOT.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Balena.IOT.RealTimeMonitor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IRepository<Device> _repository;

        public DeviceController(IRepository<Device> _repository)
        {
            this._repository = _repository;
        }

        //todo: Add paging and odata integration for dynamic querying
        /// <summary>
        /// Query list of devices
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(long take=0, long skip=0)
        {
            return Ok(_repository.AsQueryable().ToList());
        }

        /// <summary>
        /// Get single device by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var device = await _repository.FindByIdAsync(id);
            if (device == null)
                return BadRequest();
            
            return Ok(device);
        }
    }
}