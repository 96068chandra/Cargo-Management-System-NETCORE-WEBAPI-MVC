using Microsoft.AspNetCore.Mvc;
using CargoSample2.Models;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;


namespace AccessCargoService1.Controllers
{
    public class CargoesController1 : Controller
    {
        private readonly IConfiguration _configuration;
        public CargoesController1(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            List<CargoViewModel> cargoes = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Cargoes/GetAllCargo");
                if (result.IsSuccessStatusCode)
                {
                    cargoes = await result.Content.ReadAsAsync<List<CargoViewModel>>();

                }
            }
            return View(cargoes);
        }
        [HttpGet]
        [Route("/CargoesController1/Details/{CargoId}")]
        public async Task<IActionResult> Details(int CargoId)
        {
            CargoResponses cargo = new CargoResponses();
            using(var client=new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Cargoes/GetCargoId/{CargoId}");
                if(result.IsSuccessStatusCode)
                {
                    cargo = await result.Content.ReadAsAsync<CargoResponses>();
                }
            }
            return View(cargo.value);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CargoViewModel cargoViewModel = new CargoViewModel
            {
                cargotype = await this.GetAllCargoTypes()
            };
            return View(cargoViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CargoViewModel cargo)
        {
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Cargoes/Create", cargo);

                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("/CargoesController1/Edit/{CargoId}")]
        public async Task<IActionResult> Edit(int CargoId)
        {
            CargoResponses cargo = new CargoResponses();
            if (ModelState.IsValid)
            {
              
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Cargoes/GetCargoId/{CargoId}");
                    if (result.IsSuccessStatusCode)
                    {
                        cargo = await result.Content.ReadAsAsync<CargoResponses>();
                        return View(cargo.value);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cargo doesn't exist");
                    }
                }
            }
            return View(cargo.value);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CargoViewModel cargo)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Cargoes/Update/{cargo.CargoId}", cargo);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "server error please try later");
                    }
                }

            }
            return View();
        }
        [HttpGet]
        [Route("/CargoesController1/Delete/{CargoId}")]

        public async Task<IActionResult> Delete(int CargoId)
        {
            CargoViewModel cargo = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Cargoes/Delete/{CargoId}");
                if (!result.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Server Error Please try later");
                }
            }
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public async Task<IActionResult> Delete(CargoViewModel cargo)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
        //        var result = await client.DeleteAsync($"Cargoes/GetCargoById/{cargo.CargoId}");
        //        if (result.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Server Error,please try late");
        //        }


        //    }
        //    return View();
        //}

        [NonAction]
        public async Task<List<CargoType>> GetAllCargoTypes()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                //var result = await client.GetAsync($"CargoType/GetCargoTypeById/{id}");
                var result = await client.GetAsync("CargoType/GetAllCargoTypes");
                if (result.IsSuccessStatusCode)
                {
                    var cargolist = await result.Content.ReadAsAsync<List<CargoType>>();
                    return cargolist;

                }

            }
            return null;
        }

    }
}
