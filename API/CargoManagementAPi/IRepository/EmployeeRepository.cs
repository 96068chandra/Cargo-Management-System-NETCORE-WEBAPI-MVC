using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoManagementAPi.IRepository
{
    public class EmployeeRepository : IRepositoryEmployee<Employee>
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(Employee employee)
        {
            if (employee != null)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
            }
            return null;
        }

        

        public async Task<Employee> Delete(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
                return emp;
            }

            return null;
        }

        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {

            return await _context.Employees.ToListAsync();

        }

        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp != null)
            {
                return emp;
            }
            return null;
        }

        //only Admin Can Update i.e Pending to Approval
        public async Task<Employee> Update(int id, Employee emp)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.IsApproved = emp.IsApproved;
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return employee;
            }
            return null;
        }
    }


}
