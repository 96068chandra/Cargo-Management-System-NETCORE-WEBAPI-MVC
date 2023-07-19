using CargoManagementAPi.IRepository;
using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoManagementAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoStatusController : ControllerBase
    {
        private readonly IRepositoryCargoStatus<CargoStatus> _repository;

        public CargoStatusController(IRepositoryCargoStatus<CargoStatus> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetAllStatus")]
        public async Task<ActionResult<IEnumerable<CargoStatus>>> GetAll()
        {
            return await _repository.GetAll();
        }
        [HttpGet]
        [Route("GetstatusById/{id}", Name = "GetstatusById")]
        public async Task<ActionResult<CargoStatus>> GetCityById(int id)
        {
            var status = await _repository.GetById(id);
            if (status != null)
            {
                return Ok(status);
            }
            return NotFound();


        }
        [HttpPost]
        [Route("CreateStatus")]
        public async Task<IActionResult> Create([FromBody] CargoStatus status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();

            }
            await _repository.Create(status);
            return CreatedAtRoute("GetCitiById", new { id = status.StatusId }, status);
        }
        [HttpPut]
        [Route("UpdateStatus/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CargoStatus status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _repository.Update(id, status);
            if (result != null)
            {
                return NoContent();
            }
            return NotFound("status Not Found");
        }

        [HttpDelete("DeleteStatus")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _repository.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound($"status Not found with id:{id}");
        }
    }
}
