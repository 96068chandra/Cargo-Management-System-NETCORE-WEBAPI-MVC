using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoManagementAPi.IRepository
{
    public class CustomerRepository : IRepository2<Customer>
    {
        private readonly ApplicationDbContext _context2;



        public CustomerRepository(ApplicationDbContext context2)
        {
            _context2 = context2;


        }



        public async Task<IActionResult> Create(Customer customer)
        {
            if (customer != null)
            {
                _context2.Customers.Add(customer);
                await _context2.SaveChangesAsync();
            }
            return null;
        }



        public async Task<Customer> Delete(int id)
        {
            var CustInDb = await _context2.Customers.FindAsync(id);
            if (CustInDb != null)
            {
                _context2.Remove(CustInDb);
                await _context2.SaveChangesAsync();
                return CustInDb;
            }
            return null;

        }

        public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
        {
            return await _context2.Customers.ToListAsync();
        }



        public async Task<ActionResult<Customer>> GetById(int id)
        {
            var customer = await _context2.Customers.FindAsync(id);
            if (customer != null)
            {
                return customer;
            }
            return null;
        }



        public async Task<Customer> Update(int Custid, Customer customer)
        {
            var CustInDb = await _context2.Customers.FindAsync(Custid);
            if (CustInDb != null)
            {
                CustInDb.CustName = customer.CustName;
                CustInDb.CustAddress = customer.CustAddress;
                CustInDb.CustPhNo = customer.CustPhNo;
                CustInDb.CustEmail = customer.CustEmail;
                CustInDb.CustPassword = customer.CustPassword;
                _context2.Customers.Update(CustInDb);
                await _context2.SaveChangesAsync();
                return CustInDb;


            }
            return null;
        }






        public async Task<ActionResult<IEnumerable<Customer>>> SearchByName(string name)
        {
            if (name == null)
            {
                return await _context2.Customers.ToListAsync();
            }

            return await _context2.Customers.Where(c => c.CustName.Contains(name))
              .ToListAsync();
        }
    }
}
