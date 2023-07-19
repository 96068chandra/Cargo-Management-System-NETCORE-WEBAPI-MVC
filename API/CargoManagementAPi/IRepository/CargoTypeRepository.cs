using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoManagementAPi.IRepository
{

   
    public class CargoTypeRepository:IRepository4<CargoType>
    {
        private readonly ApplicationDbContext _context4;
        public CargoTypeRepository(ApplicationDbContext context4) 
        {
            _context4= context4;
        }

        public async Task<IActionResult> Create(CargoType cargotype)
        {
            if(cargotype!=null)
            {
                _context4.CargoTypes.Add(cargotype);
                await _context4.SaveChangesAsync();
            }
            return null;
        }

        public async Task<CargoType> Delete(int id)
        {
            var CargoTinDb = await _context4.CargoTypes.FindAsync(id);
            if (CargoTinDb != null)
            {
                _context4.Remove(CargoTinDb);
                await _context4.SaveChangesAsync();
                return CargoTinDb;
            }
            return null;

        }

        public async Task<ActionResult<IEnumerable<CargoType>>> GetAll()
        {
            return await _context4.CargoTypes.ToListAsync();
        }

        public async Task<ActionResult<CargoType>> GetById(int id)
        {
            var cargotype = await _context4.CargoTypes.FindAsync(id);
            if (cargotype != null)
            {
                return cargotype;
            }
            return null;
        }

        public async Task<CargoType> Update(int id, CargoType cargotype)
        {
            var CargoTinDb = await _context4.CargoTypes.FindAsync(id);
            if(CargoTinDb!= null)
            {
                CargoTinDb.Name= cargotype.Name;
                CargoTinDb.Price=cargotype.Price;
                CargoTinDb.Weight=cargotype.Weight;
                CargoTinDb.ExtraPrice=cargotype.ExtraPrice;
                CargoTinDb.ExtraWeight=cargotype.ExtraWeight;
                _context4.CargoTypes.Update(CargoTinDb);
                await _context4.SaveChangesAsync();
                return CargoTinDb;

            }
            return null;
        }
    }
}
