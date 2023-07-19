using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoManagementAPi.IRepository
{
    public class AdminRepository : IRepository3<Admin>
    {
        private readonly ApplicationDbContext _context3;

        public AdminRepository(ApplicationDbContext context3)
        {
            _context3 = context3;
        }

        public async Task<IActionResult> Create(Admin admin)
        {
            if(admin != null)
            {
                _context3.Admin.Add(admin);
                await _context3.SaveChangesAsync();
            }
            return null;
        }

        public async Task<Admin> Delete(int id)
        {
            var AdminInDb = await _context3.Admin.FindAsync(id);
            if(AdminInDb != null)
            {
                _context3.Admin.Remove(AdminInDb);
                await _context3.SaveChangesAsync();
                return AdminInDb;
            }
            return null;
        }

        public async Task<ActionResult<IEnumerable<Admin>>> GetAll()
        {
            return await _context3.Admin.ToListAsync();
        }

        public async Task<ActionResult<Admin>> GetById(int id)
        {
            var admin=await _context3.Admin.FindAsync(id);
            if(admin != null)
            {
                return admin;

            }
            return null;
        }

        public async Task<Admin> Update(int id, Admin admin)
        {
            var AdminInDb = await _context3.Admin.FindAsync(id);
            if (AdminInDb != null)
            {
                AdminInDb.Name= admin.Name;
                AdminInDb.Email= admin.Email;
                AdminInDb.Password= admin.Password;
                _context3.Admin.Update(AdminInDb);
                await _context3.SaveChangesAsync();
                return AdminInDb;
            }

            return null;
        }
    }
}
