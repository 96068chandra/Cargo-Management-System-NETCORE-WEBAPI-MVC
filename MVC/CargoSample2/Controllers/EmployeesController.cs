using CargoSample2.Models;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CargoSample2.Controllers
{
    public class EmployeesController : Controller
    {

        public readonly IConfiguration _configuration;

        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]

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
                    var count = orders.Where(c => c.CargoStatusId == 1).Count();


                    return View(orders);
                }



            }

            return View();
        }




        // GET: EmployeesController
        public async Task<IActionResult> Index()
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Employees/GetAllEmployees");
                if (result.IsSuccessStatusCode)
                {
                    employees = await result.Content.ReadAsAsync<List<EmployeeViewModel>>();
                }
            }
            return View(employees);

        }

        // GET: EmployeesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            EmployeeViewModel employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Employees/GetAllEmployees");
                if (result.IsSuccessStatusCode)
                {
                    var Emplist = await result.Content.ReadAsAsync<List<EmployeeViewModel>>();
                    employee = Emplist.Where(c => c.EmpId == id).FirstOrDefault();
                    if (employee != null)
                    {
                        return View(employee);
                    }
                }

            }
            return null;
        }

        // GET: EmployeesController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: EmployeesController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync($"Employees/Create", employee);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Login", "Employees");

                    }



                }

            }
            return View(employee);
        }

        // GET: EmployeesController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            EmployeeResponses employee = new EmployeeResponses();
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Employees/GetEmployeeById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        employee = await result.Content.ReadAsAsync<EmployeeResponses>();
                        return View(employee.value);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Employee does not exist");
                    }
                }
            }
            return View(employee.value);
        }

        // POST: EmployeesController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Employees/UpdateEmployee/{employee.EmpId}", employee);
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
            return View(employee);
        }

        // GET: EmployeesController/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                EmployeeViewModel employee = new();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync("Employees/GetAllEmployees");
                    if (result.IsSuccessStatusCode)
                    {
                        var EmpList = await result.Content.ReadAsAsync<List<EmployeeViewModel>>();
                        employee = EmpList.Where(c => c.EmpId == id).FirstOrDefault();
                        if (employee != null)
                        {
                            return View(employee);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Employee doesn't exist");
                        }




                    }



                }

            }
            return View();
        }

        // POST: EmployeesController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(EmployeeViewModel employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Employees/DeleteEmployee/{employee.EmpId}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");

                }
            }
            return View(employee);

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(EmployeeLoginModel login)
        {


            if (ModelState.IsValid)
            {
                int id = 0;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Employees/Login", login);
                    if (result.IsSuccessStatusCode)
                    {
                        string token = await result.Content.ReadAsAsync<string>();
                        if (token != null)
                        {

                            if (token[token.Length - 1] == '1')
                            {
                                token = token.Remove(token.Length - 1, 1);
                                HttpContext.Session.SetString("token", token);

                                return RedirectToAction("Dashboard");
                            }
                            else if (token[token.Length - 1] == '0')
                            {
                                ModelState.AddModelError("", "Admin yet to verify you account");
                                return View(login);
                            }


                        }


                        //return RedirectToAction("Details", "Employees");
                    }
                    else
                    {
                        return View(login);
                    }
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
            }
            return View(login);
        }



        //[HttpPost]
        //public IActionResult LogOut()
        //{
        //    HttpContext.Session.Remove("token");
        //    return RedirectToAction("Index", "Home");
        //}




        public async Task<IActionResult> OrderById(int id)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("CargoOrderDetails/GetAllCargoOrderDetails");
                if (result.IsSuccessStatusCode)
                {
                    var orderList = await result.Content.ReadAsAsync<List<CargoOrderViewModel>>();
                    CargoOrderViewModel cargoOrder = orderList.Where(c => c.Id == id).FirstOrDefault();
                    if (cargoOrder != null)
                    {
                        return View(cargoOrder);
                    }

                }
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> OrderEdit(int id)
        {
            CargoOrderViewModel cargoOrder = new CargoOrderViewModel();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync("CargoOrderDetails/GetAllCargoOrderDetails");
                    if (result.IsSuccessStatusCode)
                    {
                        var orderList = await result.Content.ReadAsAsync<List<CargoOrderViewModel>>();
                        cargoOrder = orderList.Where(c => c.Id == id).FirstOrDefault();
                        if (cargoOrder != null)
                        {
                            return View(cargoOrder);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cargo order does not exist");
                        }
                    }
                }
                return View(cargoOrder);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> OrderEdit(CargoOrderViewModel cargoOrder)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"CargoOrderDetails/UpdateCargoOrderDetail/{cargoOrder.Id}", cargoOrder);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Server Error, Please try later");
                    }
                }
            }
            return View(cargoOrder);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDelete(int id)
        {
            if (ModelState.IsValid)
            {
                CargoOrderViewModel cargoOrder = new();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync("CargoOrderDetails/GetAllCargoOrderDetails");
                    if (result.IsSuccessStatusCode)
                    {
                        var orderList = await result.Content.ReadAsAsync<List<CargoOrderViewModel>>();
                        cargoOrder = orderList.Where(c => c.Id == id).FirstOrDefault();
                        if (cargoOrder != null)
                        {
                            return View(cargoOrder);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cargo Order doesn't exist");
                        }

                    }
                }
            }
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> OrderDelete(CargoOrderViewModel cargoOrder)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"/api/CargoOrderDetails/DeleteCargoOrderDetail/{cargoOrder.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");
                }
            }
            return View(cargoOrder);
        }
        [HttpGet]
        public async Task<IActionResult> PendingOrders()
        {
            List<CargoOrderViewModel> orders = new List<CargoOrderViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var resultOrders = await client.GetAsync("CargoOrderDetails/GetAllCargoOrderDetails");
                if (resultOrders.IsSuccessStatusCode)
                {
                    orders = await resultOrders.Content.ReadAsAsync<List<CargoOrderViewModel>>();
                    var count = orders.Where(c => c.CargoStatusId == 1).Count();
                    ViewBag.OrderCount = orders.Count;
                    ViewBag.YetToDeliverOrder = count;
                }



            }

            return View(orders);
        }
        [HttpGet]
        [Route("Customer/SearchCusByName/{Name?}")]
        public async Task<IActionResult> SearchCusByName(string Name)
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

        [HttpGet]
        public async Task<IActionResult> EditCustomer(int id)
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
        public async Task<IActionResult> EditCustomer(CustomerViewModel customer)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Customers/UpdateCustomer/{customer.CustId}", customer);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("SearchCusByName");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Server Error, Please try later");
                    }
                }
            }
            return View(customer);
        }


    }
}


