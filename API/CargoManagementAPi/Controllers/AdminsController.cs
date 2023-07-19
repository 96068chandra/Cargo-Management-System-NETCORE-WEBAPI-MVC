using CargoManagementAPi.IRepository;
using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CargoManagementAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdminsController : Controller
    {
        private readonly IRepository2<Customer> _repository2;
        private readonly IRepository3<Admin> _repository3;
        private readonly IRepositoryEmployee<Employee> _repository6;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public AdminsController(IRepository3<Admin> repository, IRepository2<Customer> repository2, IRepositoryEmployee<Employee> repository6, ApplicationDbContext context, IConfiguration configuration)
        {

            _repository3 = repository;
            _repository2 = repository2;
            _repository6 = repository6;
            _context = context;
            _configuration = configuration;
        }


        //Admin CRUD operations
        [HttpGet]
        [Route("GetAllAdmin")]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAll()
        {
            return await _repository3.GetAll();

        }

        [HttpGet]
        [Route("GetAdminById/{id}", Name = "GetAdminById")]
        public async Task<ActionResult<Admin>> GetById(int id)
        {
            var admin = await _repository3.GetById(id);
            if (admin != null)
            {
                return Ok(admin);
            }
            return NotFound();

        }

        [HttpPost]
        [Route("CreateAdmin")]
        public async Task<IActionResult> Create([FromBody] Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _repository3.Create(admin);
            return CreatedAtRoute("GetAdminById", new { id = admin.Id }, admin);
        }
        [HttpPut]
        [Route("UpdateAdmin/{id}")]
        public async Task<IActionResult> Update(int id, Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _repository3.Update(id, admin);
            if (result != null)
            {
                return NoContent();
            }
            return NotFound("Admin Not Found");
        }
        [HttpDelete]
        [Route("DeleteAdmin/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository3.Delete(id);
            if (result != null)
            {
                return Ok();
            }
            return NotFound("Admin not found with this id");

        }

        //Admin Login using Jwt token

        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] AdminLoginModel adminLogin)
        {
            var currentAdmin=_context.Admin.FirstOrDefault(x=>x.UserName== adminLogin.UserName && x.Password==adminLogin.Password);
            if(currentAdmin == null)
            {
                return NotFound("Invalid Username or password");
            }
            var token = GenreateToken(currentAdmin);
            if (token == null)
            {
                return NotFound("Invalid Credentials");
            }
            return Ok(token);
         
        }
        [NonAction]
        public string GenreateToken(Admin admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            var myClaims = new List<Claim>
            {

                new Claim(ClaimTypes.Name,admin.UserName),
                new Claim(ClaimTypes.Email,admin.Email),
            };
            var token = new JwtSecurityToken(issuer: _configuration["JWT:issuer"],
                                             audience: _configuration["JWT:audience"],
                                             claims: myClaims,
                                             expires: DateTime.Now.AddDays(1),
                                             signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }





        //Employees CRUD operations

        //[HttpGet]
        //[Route("GetAllEmployees")]
        //public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployee()
        //{
        //    return await _repository6.GetAll();
        //}

        //[HttpGet]
        //[Route("GetEmployeeById/{id}", Name = "GetEmployeeById")]
        //public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        //{
        //    var employee = await _repository6.GetById(id);
        //    if (employee != null)
        //    {
        //        return Ok(employee);
        //    }
        //    return NotFound();
        //}

        //[HttpPut]
        //[Route("UpdateEmployee/{id}")]

        //public async Task<IActionResult> Update(int id, [FromBody] Employee employee)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    var result = await _repository6.Update(id, employee);
        //    if (result != null)
        //    {

        //        return NoContent();
        //    }
        //    return NotFound("Employee Not Found");
        //}

        //[HttpDelete("DeleteEmployee")]
        //public async Task<ActionResult> DeleteEmployee(int id)
        //{
        //    var result = await _repository6.Delete(id);
        //    if (result != null)
        //    {
        //        return Ok(result);
        //    }
        //    return NotFound($"Employee Not found with employee id:{id}");
        //}

        //Admin have the access for searching a customer based on id and admin can see all the customers at a time
        //[HttpGet]
        //[Route("GetAllCustomers")]
        //public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        //{
        //    return await _repository2.GetAll();
        //}

        //[HttpGet]
        //[Route("GetCustomerById/{id}", Name = "GetCustomerById")]
        //public async Task<ActionResult<Customer>> GetCustomerById(int id)
        //{
        //    var customer = await _repository2.GetById(id);
        //    if (customer != null)
        //    {
        //        return Ok(customer);
        //    }
        //    return NotFound();
        //}




    } 
}
