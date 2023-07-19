using CargoManagementAPi.IRepository;
using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
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
    public class EmployeesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IRepositoryEmployee<Employee> _repository2;
        public EmployeesController(IRepositoryEmployee<Employee> repository2,ApplicationDbContext dbContext,IConfiguration configuration)
        {
            _repository2 = repository2;
            _configuration = configuration;
            _context = dbContext;
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            return await _repository2.GetAll();
        }

        [HttpGet]
        [Route("GetEmployeeById/{id}", Name = "GetEmployeeById")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await _repository2.GetById(id);
            if (employee != null)
            {
                return Ok(employee);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();

            }
            await _repository2.Create(employee);
            return CreatedAtRoute("GetEmployeeById", new { id = employee.EmpId }, employee);

        }
        [HttpPut]
        [Route("UpdateEmployee/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _repository2.Update(id, employee);
            if (result != null)
            {

                return NoContent();
            }
            return NotFound("Employee Not Found");
        }

        [HttpDelete("DeleteEmployee")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _repository2.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound($"Employee Not found with employee id:{id}");
        }

        //Login For Employee using Jwt

        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] EmployeeLoginModel employeeLoginModel)
        {
            var currentEmployee = _context.Employees.FirstOrDefault(x => x.UserName == employeeLoginModel.UserName && x.Password == employeeLoginModel.Password);
            if (currentEmployee == null)
            {
                return NotFound("Invalid UserName or Password");
            }
            var token = GenerateToken(currentEmployee);
            if (token == null)
            {
                return NotFound("Invalid Credentials");
            }
            token+= currentEmployee.IsApproved.ToString();
            return Ok(token);
        }

        [NonAction]
        public string GenerateToken(Employee employee)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            var myClaims = new List<Claim>
            {

                new Claim(ClaimTypes.Name,employee.UserName),
                new Claim(ClaimTypes.Email,employee.EmpEmail),
                new Claim(ClaimTypes.MobilePhone,employee.EmpPhNo)

            };
            var token = new JwtSecurityToken(issuer: _configuration["JWT:issuer"],
                                             audience: _configuration["JWT:audience"],
                                             claims: myClaims,
                                             expires: DateTime.Now.AddDays(1),
                                             signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);




        }

    }



}

