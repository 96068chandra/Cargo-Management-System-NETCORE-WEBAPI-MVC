using CargoSample2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CargoSample2.Controllers
{
    public class CustomersController : Controller
    {
        public readonly IConfiguration _configuration;
        public CustomersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Customer/Dashboard/{id?}")]
        public async Task<IActionResult> Dashboard(int id)
        {


            List<CargoOrderViewModel> orders = new List<CargoOrderViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var resultOrders = await client.GetAsync("CargoOrderDetails/GetAllCargoOrderDetails");
                if (resultOrders.IsSuccessStatusCode)
                {
                    orders = await resultOrders.Content.ReadAsAsync<List<CargoOrderViewModel>>();
                    var yourOrders = orders.Where(c => c.CustId == id).ToList();
                    int TotalOrders = yourOrders.Count();
                    int PendingOrders = yourOrders.Where(c => c.CargoStatusId == 1).Count();

                    ViewBag.PendingOrders = PendingOrders; ViewBag.TotalOrders = TotalOrders;
                    if (yourOrders != null)
                    {
                        return View(yourOrders);

                    }

                }



            }

            return View(orders);
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrder()
        {
            CargoOrderViewModel cargo = new CargoOrderViewModel
            {
                CargoStatus = await this.GetStatus(),
                CargoType = await this.GetAllCargoTypes(),
                City = await this.GetCities()
            };
            return View(cargo);
        }

        [HttpGet]
        public async Task<IActionResult> CalculatePrice(int CargoTypeId, double Weight)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("CargoType/GetAllCargoTypes");
                if (result.IsSuccessStatusCode)
                {
                    var cargolist = await result.Content.ReadAsAsync<List<CargoType>>();
                    CargoType cargoType = cargolist.Where(c => c.Id == CargoTypeId).FirstOrDefault();
                    double price = 0;
                    if (Weight > cargoType.Weight + cargoType.ExtraWeight)
                    {
                        ModelState.AddModelError("", "Sorry can carry upto" + cargoType.Weight + cargoType.ExtraWeight + "Kg");
                        price = cargoType.Price * cargoType.Weight;
                        price += cargoType.ExtraPrice * cargoType.ExtraWeight;
                        return Json(price);

                    }
                    if (Weight > cargoType.Weight)
                    {
                        double extraWeight = Weight - cargoType.Weight;
                        price = cargoType.Price * cargoType.Weight;
                        price += extraWeight * cargoType.ExtraPrice * cargoType.ExtraWeight;


                    }
                    else
                    {
                        price = cargoType.Price * Weight;

                    }
                    ViewBag.Price = price;
                    return Json(price);



                }
            }
            return View();
        }


        public async Task<IActionResult> GetCargoById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("CargoType/GetAllCargoTypes");
                if (result.IsSuccessStatusCode)
                {
                    var cargolist = await result.Content.ReadAsAsync<List<CargoType>>();
                    CargoType cargoType = cargolist.Where(c => c.Id == id).FirstOrDefault();
                    return PartialView("_PartialGetCargoById", cargoType);


                }

            }
            return null;

        }
        [NonAction]

        public async Task<List<CargoStatusViewModel>> GetStatus()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("CargoStatus/GetAllStatus");
                if (result.IsSuccessStatusCode)
                {
                    var statusList = await result.Content.ReadAsAsync<List<CargoStatusViewModel>>();
                    return statusList;

                }

            }
            return null;
        }

        [NonAction]
        public async Task<List<CargoType>> GetAllCargoTypes()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("CargoType/GetAllCargoTypes");
                if (result.IsSuccessStatusCode)
                {
                    var cargolist = await result.Content.ReadAsAsync<List<CargoType>>();
                    return cargolist;

                }

            }
            return null;
        }
        [NonAction]
        public async Task<List<CityViewModel>> GetCities()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("Cities/GetAllCities");
                if (result.IsSuccessStatusCode)
                {
                    var citylist = await result.Content.ReadAsAsync<List<CityViewModel>>();
                    return citylist;

                }

            }
            return null;

        }


        [HttpPost]

        public async Task<IActionResult> CreateOrder(CargoOrderViewModel cargoOrder)
        {
            if (ModelState.IsValid)
            {
                cargoOrder.CargoStatusId = 1;
                cargoOrder.CustId = 1;
                cargoOrder.OrderDate = DateTime.Now;
                cargoOrder.OrderId = cargoOrder.CustId.ToString() + cargoOrder.OrderDate.ToString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync($"CargoOrderDetails/Create", cargoOrder);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");

                    }



                }

            }
            return View(cargoOrder);
        }





        public async Task<IActionResult> Index()
        {
            List<CustomerViewModel> customers = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Customers/GetAllCustomers");
                if (result.IsSuccessStatusCode)
                {
                    customers = await result.Content.ReadAsAsync<List<CustomerViewModel>>();
                }

            }
            return View(customers);
        }


        public async Task<IActionResult> Details(int id)
        {
            CustomerViewModel customer = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Customers/GetAllCustomers");
                if (result.IsSuccessStatusCode)
                {
                    var customerlist = await result.Content.ReadAsAsync<List<CustomerViewModel>>();
                    customer = customerlist.Where(c => c.CustId == id).FirstOrDefault();
                    if (customer != null)
                    {
                        return View(customer);
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
        public async Task<IActionResult> Create(CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync($"Customers/Create", customer);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Login", "Customers");

                    }



                }

            }
            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CustomerResponses customer = new CustomerResponses();
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Customers/GetCustomerById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        customer = await result.Content.ReadAsAsync<CustomerResponses>();
                        return View(customer.value);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Customer does not exist");
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CustomerViewModel customer)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Customers/UpdateCustomer/{customer.CustId}", customer);
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
            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                CustomerViewModel customer = new();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync("Customers/GetAllCustomers");
                    if (result.IsSuccessStatusCode)
                    {
                        var customerList = await result.Content.ReadAsAsync<List<CustomerViewModel>>();
                        customer = customerList.Where(c => c.CustId == id).FirstOrDefault();
                        if (customer != null)
                        {
                            return View(customer);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Customer doesn't exist");
                        }
                    }
                }

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(CustomerViewModel customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Customers/DeleteCustomer/{customer.CustId}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");

                }
            }
            return View(customer);


        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(CustomerLoginModel login)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Customers/Login", login);
                    if (result.IsSuccessStatusCode)
                    {
                        string token = await result.Content.ReadAsAsync<string>();
                        HttpContext.Session.SetString("token", token);


                        return RedirectToAction("Dashboard", "Customers", new { id = login.CustId });
                    }
                    ModelState.AddModelError("", "Invalid Username or Password");
                }

            }
            return View(login);
        }
        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        [Route("Customer/SearchByName/{Name?}")]
        public async Task<IActionResult> SearchByName(string Name)
        {


            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            using (var client = new HttpClient())

            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Customers/GetAllCustomers");
                if (result.IsSuccessStatusCode)
                {
                    customers = await result.Content.ReadAsAsync<List<CustomerViewModel>>();
                    if (string.IsNullOrEmpty(Name))
                    {
                        return View(customers);
                    }
                    List<CustomerViewModel> customer = customers.Where(c => c.CustName.Contains(Name)).ToList();

                    return View(customer);

                }
            }
            return View();
        }
    }
}
