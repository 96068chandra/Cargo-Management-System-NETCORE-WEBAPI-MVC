using CargoSample2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CargoSample2.Controllers
{
    public class AdminController : Controller
    {
        public readonly IConfiguration _configuration;
        public AdminController(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        public async Task<IActionResult> Dashboard()
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

                var resultEmp = await client.GetAsync("Employees/GetAllEmployees");
                if (resultEmp.IsSuccessStatusCode)
                {
                    var employees = await resultEmp.Content.ReadAsAsync<List<EmployeeViewModel>>();
                    var count = employees.Where(e => e.IsApproved == 0).Count();
                    ViewBag.TotalEmployee = employees.Count;
                    ViewBag.PendingApproval = count;
                }

            }

            return View(orders);
        }
        public async Task<IActionResult> Index()
        {
            List<AdminViewModel> admins = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Admins/GetAllAdmin");
                if (result.IsSuccessStatusCode)
                {
                    admins = await result.Content.ReadAsAsync<List<AdminViewModel>>();
                }

            }
            return View(admins);

        }

        public async Task<IActionResult> Details(int id)
        {
            AdminViewModel admin = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Admins/GetAllAdmin");
                if (result.IsSuccessStatusCode)
                {
                    var Adminlist = await result.Content.ReadAsAsync<List<AdminViewModel>>();
                    admin = Adminlist.Where(c => c.Id == id).FirstOrDefault();
                    if (admin != null)
                    {
                        return View(admin);
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
        public async Task<IActionResult> Create(AdminViewModel admin)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync($"Admins/CreateAdmin", admin);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");

                    }



                }

            }
            return View(admin);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AdminResponses admin = new AdminResponses();
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Admins/GetAdminById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        admin = await result.Content.ReadAsAsync<AdminResponses>();
                        return View(admin.value);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Admin does not exist");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminViewModel admin)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Admins/UpdateAdmin/{admin.Id}", admin);
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
            return View(admin);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                AdminViewModel admin = new();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync("Admins/GetAllAdmin");
                    if (result.IsSuccessStatusCode)
                    {
                        var adminList = await result.Content.ReadAsAsync<List<AdminViewModel>>();
                        admin = adminList.Where(c => c.Id == id).FirstOrDefault();
                        if (admin != null)
                        {
                            return View(admin);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Admin doesn't exist");
                        }
                    }
                }

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AdminViewModel admin)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Admins/DeleteAdmin/{admin.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");

                }
            }
            return View(admin);


        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginModel login)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Admins/Login", login);
                    if (result.IsSuccessStatusCode)
                    {
                        string token = await result.Content.ReadAsAsync<string>();
                        HttpContext.Session.SetString("token", token);
                        return RedirectToAction("Dashboard", "Admin");
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
                    var count = orders.Where(c => c.CargoTypeId == 1).Count();
                    ViewBag.OrderCount = orders.Count;
                    ViewBag.YetToDeliverOrder = count;
                }



            }

            return View(orders);
        }
        [HttpGet]
        [Route("Admin/SearchCusByName/{Name?}")]
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
        [HttpGet]
        [Route("Admin/SearchEmpByName/{Name?}")]
        public async Task<IActionResult> SearchEmpByName(string Name)
        {


            List<EmployeeViewModel> emp = new List<EmployeeViewModel>();
            using (var client = new HttpClient())

            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Employees/GetAllEmployees");
                if (result.IsSuccessStatusCode)
                {
                    emp = await result.Content.ReadAsAsync<List<EmployeeViewModel>>();
                    if (string.IsNullOrEmpty(Name))
                    {
                        return View(emp);
                    }
                    List<EmployeeViewModel> empls = emp.Where(c => c.UserName.Contains(Name)).ToList();

                    return View(empls);

                }
            }
            return View();
        }

       
       

        // GET: EmployeesController/Details/5
        public async Task<ActionResult> EmpDetails(int id)
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



        // GET: EmployeesController/Edit/5
        [HttpGet]
        public async Task<IActionResult> EmpEdit(int id)
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
        public async Task<ActionResult> EmpEdit(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Employees/UpdateEmployee/{employee.EmpId}", employee);
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
            return View(employee);
        }




    }
}
