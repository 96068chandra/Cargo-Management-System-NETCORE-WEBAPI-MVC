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
    public class CargoesController : Controller
    
    {
        private readonly IRepository<Cargo> _repository;

        public CargoesController(IRepository<Cargo> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetAllCargo")]
        public async Task<ActionResult<IEnumerable<Cargo>>> GetAllCargo()
        {
            return await _repository.GetAllCargo();
        }



        [HttpGet]
        [Route("GetCargoId/{id}",Name = "GetCargoById")]
        public async Task<ActionResult<Cargo>> GetCargoById(int id)
        {
            var cargo=await _repository.GetCargoById(id);
            if (cargo != null)
            {
                return Ok(cargo);
            }
            return NotFound();
        }



        // GET: CargoesController
        //[Route("Index")]
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: CargoesController/Details/5
        //[Route("Details")]
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: CargoesController/Create


        [HttpPost]
        [Route("Create")]
       
        public async Task<ActionResult> Create([FromBody] Cargo cargo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _repository.Create(cargo);
            return CreatedAtRoute("GetCargoById", new { id = cargo.CargoId }, cargo);
        }

        // POST: CargoesController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: CargoesController/Edit/5
        
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cargo cargo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result=await _repository.Update(id, cargo);
            if (result != null)
            {
                return NoContent();
            }   
            return NotFound("Cargo Not Found");

        
        }

        // POST: CargoesController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: CargoesController/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result=await _repository.Delete(id);
            if(result!=null)
            {
                return Ok();
            }
            return NotFound("Cargo could not found");

        }

        // POST: CargoesController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
