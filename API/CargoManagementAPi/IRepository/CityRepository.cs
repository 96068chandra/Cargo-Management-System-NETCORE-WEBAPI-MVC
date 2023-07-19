using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoManagementAPi.IRepository
{
    public class CityRepository:IRepository5<City>
    {
        private readonly ApplicationDbContext _context5;
        public CityRepository(ApplicationDbContext context5)
        {
            
            _context5 = context5;

        }

        //CRUD For cities

        public async Task<ActionResult<IEnumerable<City>>> GetAllCities()
        {
            return await _context5.Cities.ToListAsync();

        }
        public async Task<ActionResult<City>> CityById(int id)
        {
            var city = await _context5.Cities.FindAsync(id);
            if (city != null)
            {
                return city;
            }
            return null;
        }


        public async Task<IActionResult> Create(City city)
        {
            if (city != null)
            {
                _context5.Cities.Add(city);
                await _context5.SaveChangesAsync();

            }
            return null;
        }

        public async Task<City> Update(int id, City city)
        {
            var CityInDb = await _context5.Cities.FindAsync(id);
            if (CityInDb != null)
            {
                CityInDb.CityName = city.CityName;
                CityInDb.Pincode = city.Pincode;
                CityInDb.Country = city.Country;
                _context5.Cities.Update(CityInDb);
                await _context5.SaveChangesAsync();
                return CityInDb;
            }
            return null;
        }

        public async Task<City> Delete(int id)
        {
            var CityInDb = await _context5.Cities.FindAsync(id);
            if (CityInDb != null)
            {
                _context5.Cities.Remove(CityInDb);
                await _context5.SaveChangesAsync();
                return CityInDb;
            }
            return null;
        }
    }
}
