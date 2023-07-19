using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using CargoSample2.Models;

namespace SampleThings.Controllers
{
    public class CargoTypesController : Controller
    {

        private readonly IConfiguration _context;

        public CargoTypesController(IConfiguration context)
        {
            _context = context;
        }

       

                [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<CargoType> cargoTypes = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_context["ApiUrl:api"]);
                var result = await client.GetAsync("CargoType/GetAllCargoTypes");
                if (result.IsSuccessStatusCode)
                {
                    cargoTypes = await result.Content.ReadAsAsync<List<CargoType>>();
                }
            }
            return View(cargoTypes);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            CargoType cargo = new CargoType();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_context["ApiUrl:api"]);
                //var result = await client.GetAsync($"CargoType/GetCargoTypeById/{id}");
                var result = await client.GetAsync("CargoType/GetAllCargoTypes");
                if (result.IsSuccessStatusCode)
                {
                    var cargolist = await result.Content.ReadAsAsync<List<CargoType>>();

                    cargo = cargolist.Where(c => c.Id == id).FirstOrDefault();
                    if (cargo != null)
                    {
                        return View(cargo);
                    }


                }
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CargoType viewModel = new CargoType();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CargoType cargo)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(_context["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("CargoType/CreateCargoType", cargo);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            CargoType viewModel = new CargoType();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (ModelState.IsValid)
            {
                CargoType cargo = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_context["ApiUrl:api"]);
                    var result = await client.GetAsync("CargoType/GetAllCargoTypes");
                    if (result.IsSuccessStatusCode)
                    {
                        var cargolist = await result.Content.ReadAsAsync<List<CargoType>>();

                        cargo = cargolist.Where(c => c.Id == id).FirstOrDefault();
                        if (cargo != null)
                        {
                            return View(cargo);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cargo type doesn't exitst");
                        }


                    }
                }

            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CargoType cargo)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_context["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"CargoType/UpdateCargoType/{cargo.Id}", cargo);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Server Error. Please try later");
                    }
                }
            }
            return View(cargo);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                CargoType cargo = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_context["ApiUrl:api"]);
                    var result = await client.GetAsync("CargoType/GetAllCargoTypes");
                    if (result.IsSuccessStatusCode)
                    {
                        var cargolist = await result.Content.ReadAsAsync<List<CargoType>>();

                        cargo = cargolist.Where(c => c.Id == id).FirstOrDefault();
                        if (cargo != null)
                        {
                            return View(cargo);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cargo type doesn't exitst");
                        }


                    }
                }

            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CargoType cargo)
        {
            using (var client = new HttpClient())
            {
               // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_context["ApiUrl:api"]);
                var result = await client.DeleteAsync($"CargoType/DeleteCargoType/{cargo.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
               
            }
            return View(cargo);
        }

        public IActionResult Display()
        {
            return View();
        }

    }
}

