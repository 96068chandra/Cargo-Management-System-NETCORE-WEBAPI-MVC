using CargoManagementAPi.IRepository;
using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoManagementAPi.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class CargoOrderDetailsController : Controller
    {
        private readonly IRepositoryCODR<CargoOrderDetails> _repository2;
        public CargoOrderDetailsController(IRepositoryCODR<CargoOrderDetails> repository2)
        {
            _repository2 = repository2;
        }
        [HttpGet]
        [Route("GetAllCargoOrderDetails")]
        public async Task<ActionResult<IEnumerable<CargoOrderDetails>>> GetAll()
        {
            return await _repository2.GetAll();
        }

        [HttpGet]
        [Route("GetCargoOrderDetailById/{id}", Name = "GetCargoOrderDetailById")]
        public async Task<ActionResult<CargoOrderDetails>> GetById(int id)
        {
            var cargoOrderDetail = await _repository2.GetById(id);
            if (cargoOrderDetail != null)
            {
                return Ok(cargoOrderDetail);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CargoOrderDetails cargoOrderDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();

            }
            await _repository2.Create(cargoOrderDetail);
            return CreatedAtRoute("GetCargoOrderDetailById", new { id = cargoOrderDetail.OrderId }, cargoOrderDetail);

        }
        [HttpPut]
        [Route("UpdateCargoOrderDetail/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CargoOrderDetails cargoOrderDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _repository2.Update(id, cargoOrderDetail);
            if (result != null)
            {
                return NoContent();
            }
            return NotFound("Cargo Order Detail Not Found");
        }

        [HttpDelete("DeleteCargoOrderDetail/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _repository2.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound($"Cargo Order Detail Not found with order detail id:{id}");
        }
    }

}
