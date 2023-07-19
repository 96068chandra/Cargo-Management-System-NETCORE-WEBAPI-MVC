using CargoManagementAPi.IRepository;
using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoManagementAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoTypeController : Controller
    {
        private readonly IRepository4<CargoType> _repository4;
        public CargoTypeController(IRepository4<CargoType> repository4)
        {
            _repository4 = repository4;
        }

        //CRUD operations
        [HttpGet]
        [Route("GetAllCargoTypes")]
        public async Task<ActionResult<IEnumerable<CargoType>>> GetAll()
        {
            return await _repository4.GetAll();
        }

        [HttpGet]
        [Route("GetCargoTypeById/{id}",Name = "GetCargoTypeById")]
        public async Task<ActionResult<CargoType>> CityById(int id)
        {
            var cargotype=await _repository4.GetById(id);
            if(cargotype != null)
            {
                return Ok(cargotype);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("CreateCargoType")]

        public async Task<IActionResult> Create([FromBody] CargoType cargoType)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _repository4.Create(cargoType);
            return CreatedAtRoute("GetCargoTypeById", new { id = cargoType.Id }, cargoType);
        }

        [HttpPut]
        [Route("UpdateCargoType/{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] CargoType cargoType)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();

            }
            var result = await _repository4.Update(id, cargoType);
            if (result != null)
            {
                return NoContent();

            }
            return NotFound("CargoType Not Found");
        }

        [HttpDelete("DeleteCargoType")]
        public async Task<ActionResult> Delete(int id)
        {
            var result=await _repository4.Delete(id);
            if(result!= null)
            {
                return Ok(result);
            }
            return NotFound($"Cargotype not found with id:{id}");
        }





    }
}
