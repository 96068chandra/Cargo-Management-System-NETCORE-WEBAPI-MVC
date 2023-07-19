using CargoManagementAPi.IRepository;
using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;


namespace CargoManagementAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository2<Customer> _repository2;
        private readonly ApplicationDbContext _context;

        public CustomersController(IRepository2<Customer> repository2,IConfiguration configuration, ApplicationDbContext context)
        {
            _repository2 = repository2;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        [Route("GetAllCustomers")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
        {
            return await _repository2.GetAll();
        }

        [HttpGet]
        [Route("GetCustomerById/{id}",Name = "GetCustomerById")]
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            var customer=await _repository2.GetById(id);
            if (customer != null)
            {
                return Ok(customer);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("SearchByName/{Name}")]
        public async Task<ActionResult<IEnumerable<Customer>>> SearchByName(string Name)
        {
            if (Name == null)
            {
                var a= await _repository2.GetAll();
                return a;
            }

            return await _repository2.SearchByName(Name);

        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody]Customer customer)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();

            }
            await _repository2.Create(customer);
            return CreatedAtRoute("GetCustomerById", new { id = customer.CustId }, customer);

        }
        [HttpPut]
        [Route("UpdateCustomer/{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result=await _repository2.Update(id, customer);
            if (result != null)
            {
                return NoContent();
            }
            return NotFound("Customer Not Found");
        }


        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result=await _repository2.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound($"Customer Not found with customer id:{id}");
        }


        //Login using JWt

        [HttpPost]
        [Route("Login")]

        public ActionResult Login([FromBody] CustomerLoginModel customerLoginModel)
        {
            var currentCustomer=_context.Customers.FirstOrDefault(x=>x.UserName==customerLoginModel.UserName && x.CustPassword==customerLoginModel.CustPassword);
            if(currentCustomer == null)
            {
                return NotFound("Invalid UserName or Password");

            }
            var token=GenerateToken(currentCustomer);
            if(token==null)
            {
                return NotFound("Invalid Credentials");
            }
            return Ok(token);
        }

        [NonAction]
        public string GenerateToken(Customer customer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            var myClaims = new List<Claim>
            {
                
                new Claim(ClaimTypes.Name,customer.UserName),
                new Claim(ClaimTypes.Email,customer.CustEmail),
                new Claim(ClaimTypes.MobilePhone,customer.CustPhNo)

            };
            var token = new JwtSecurityToken(issuer: _configuration["JWT:issuer"],
                                             audience: _configuration["JWT:audience"],
                                             claims:myClaims,
                                             expires:DateTime.Now.AddDays(1),
                                             signingCredentials:credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

           

            
        }





    }
}
