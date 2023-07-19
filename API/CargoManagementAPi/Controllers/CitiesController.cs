using CargoManagementAPi.IRepository;
using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoManagementAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : Controller
    {

        private readonly IRepository5<City> _repository5;
       
        public CitiesController(IRepository5<City> repository5)
        {
            _repository5 = repository5;
         
        }

        [HttpGet]
        [Route("GetAllCities")]
        public async Task<ActionResult<IEnumerable<City>>> GetAll()
        {
            return await _repository5.GetAllCities();
        }

        [HttpGet]
        [Route("GetCitiById/{id}",Name = "GetCitiById")]
        public async Task<ActionResult<City>> GetCityById(int id)
        {
            var city = await _repository5.CityById(id);
            if (city != null)
            {
                return Ok(city);
            }
            return NotFound();
            

        }
        [HttpPost]
        [Route("CreateCity")]

        public async Task<IActionResult> Create([FromBody] City city)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();

            }
            await _repository5.Create(city);
            return CreatedAtRoute("GetCitiById", new { id = city.Id }, city);
        }

        [HttpPut]
        [Route("UpdateCity/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _repository5.Update(id, city);
            if (result != null)
            {
                return NoContent();
            }
            return NotFound("city Not Found");
        }

        [HttpDelete("DeleteCity")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _repository5.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound($"City Not found with id:{id}");
        }


    }
}
