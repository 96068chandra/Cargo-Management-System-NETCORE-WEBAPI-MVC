using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoManagementAPi.IRepository
{
    public class CargoStatusRepository: IRepositoryCargoStatus<CargoStatus>
    {
        private readonly ApplicationDbContext _context5;

        public CargoStatusRepository(ApplicationDbContext context5)
        {
            _context5 = context5;
        }

        public async Task<IActionResult> Create(CargoStatus status)
        {
            if (status != null)
            {
                _context5.cargoStatuses.Add(status);
                await _context5.SaveChangesAsync();
            }
            return null;
        }

        public async Task<CargoStatus> Delete(int id)
        {
            var StatusInDb = await _context5.cargoStatuses.FindAsync(id);
            if (StatusInDb != null)
            {
                _context5.cargoStatuses.Remove(StatusInDb);
                await _context5.SaveChangesAsync();
                return StatusInDb;
            }
            return null;
        }

        public async Task<ActionResult<IEnumerable<CargoStatus>>> GetAll()
        {
            return await _context5.cargoStatuses.ToListAsync();
        }

        public async Task<ActionResult<CargoStatus>> GetById(int id)
        {
            var status = await _context5.cargoStatuses.FindAsync(id);
            if(status != null)
            {
                return status;
            }
            return null;
        }

        public async Task<CargoStatus> Update(int id, CargoStatus status)
        {
            var statusInDb = await _context5.cargoStatuses.FindAsync(id);
            if (statusInDb != null)
            {
                statusInDb.StatusName = status.StatusName;
                _context5.cargoStatuses.Update(statusInDb);
                await _context5.SaveChangesAsync();
                return statusInDb;
            }
            return null;


        }
    }
}
