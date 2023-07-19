using CargoSample2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CargoSample2.Controllers
{
    public class CityController : Controller
    {
        public readonly IConfiguration _configuration;
        public CityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<CityViewModel> cities = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Cities/GetAllCities");
                if (result.IsSuccessStatusCode)
                {
                    cities = await result.Content.ReadAsAsync<List<CityViewModel>>();
                }

            }
            return View(cities);
        }

        public async Task<IActionResult> Details(int id)
        {
            CityViewModel city = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Cities/GetAllCities");
                if (result.IsSuccessStatusCode)
                {
                    var customerlist = await result.Content.ReadAsAsync<List<CityViewModel>>();
                    city = customerlist.Where(c => c.Id == id).FirstOrDefault();
                    if (city != null)
                    {
                        return View(city);
                    }
                }

            }
            return null;
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(CityViewModel city)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync($"cities/CreateCity", city);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");

                    }



                }

            }
            return View(city);
        }

        public async Task<IActionResult> Edit(int id)
        {
            CityResponses city = new CityResponses();
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Cities/GetCitiById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        city = await result.Content.ReadAsAsync<CityResponses>();
                        return View(city.value);
                    }
                    else
                    {
                        ModelState.AddModelError("", "City does not exist");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CityViewModel city)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"cities/UpdateCity/{city.Id}", city);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Server Error, Please try later");
                    }
                }
            }
            return View(city);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                CityViewModel city = new();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync("Cities/GetAllCities");
                    if (result.IsSuccessStatusCode)
                    {
                        var cityList = await result.Content.ReadAsAsync<List<CityViewModel>>();
                        city = cityList.Where(c => c.Id== id).FirstOrDefault();
                        if (city != null)
                        {
                            return View(city);
                        }
                        else
                        {
                            ModelState.AddModelError("", "city doesn't exist");
                        }

                    }
                }

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CityViewModel city)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Cities/DeleteCity/{city.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");

                }
            }
            return View(city);


        }




    }
}
