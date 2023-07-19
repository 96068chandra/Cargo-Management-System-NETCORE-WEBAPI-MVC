using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoManagementAPi.IRepository
{
    public class CargoRepository : IRepository<Cargo>
    {
        private readonly ApplicationDbContext _context;

        public CargoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(Cargo cargo)
        {
            if(cargo != null)
            {
                _context.Cargo.Add(cargo);
                await _context.SaveChangesAsync();

            }
            return null;
        }

        public async Task<Cargo> Delete(int cargoId)
        {
            var CargoInDb=await _context.Cargo.FindAsync(cargoId);
            if(CargoInDb != null)
            {
                _context.Cargo.Remove(CargoInDb);
                await _context.SaveChangesAsync();
                return CargoInDb;
            }
            return null;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cargo>>> GetAllCargo()
        {
            return await _context.Cargo.ToListAsync();
             
          
        }

        public async Task<ActionResult<Cargo>> GetCargoById(int id)
        {
            var cargo = await _context.Cargo.FindAsync(id);
            if (cargo != null)
            {
                return cargo;
            }
            return null;
        }

        public async Task<Cargo> Update(int id, Cargo cargo)
        {
            var CargoInDb = await _context.Cargo.FindAsync(id);
            if (CargoInDb != null)
            {
                CargoInDb.CargoName = cargo.CargoName;
                CargoInDb.Weight= cargo.Weight;
                CargoInDb.Place=cargo.Place;
                CargoInDb.OrderDate = cargo.OrderDate;
                _context.Cargo.Update(CargoInDb);
                await _context.SaveChangesAsync();
                return CargoInDb;
                
            }
            return null;
        }
    }
}
