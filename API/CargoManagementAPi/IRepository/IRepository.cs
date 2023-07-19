using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoManagementAPi.IRepository
{
    public interface IRepository<T> where T : class
    {



        Task<ActionResult<IEnumerable<T>>> GetAllCargo();
        Task<ActionResult<T>> GetCargoById(int id);
        Task<IActionResult> Create(T cargo);
        Task<T> Update(int cargoId, T cargo);
        Task<T> Delete(int cargoId);




    }
    //Customers
    public interface IRepository2<T> where T : class
    {
        Task<ActionResult<T>> GetById(int id);
        Task<ActionResult<IEnumerable<T>>> GetAll();
        Task<IActionResult> Create(T customer);
        Task<T> Update(int Custid,T customer);
        Task<T> Delete(int id);
        Task<ActionResult<IEnumerable<T>>> SearchByName(string name);

    }

    //Admin

    public interface IRepository3<T> where T : class
    {
        Task<ActionResult<T>> GetById(int id);
        Task<ActionResult<IEnumerable<T>>> GetAll();
        Task<IActionResult> Create(T customer);
        Task<T> Update(int id, T admin);
        Task<T> Delete(int id);
    }

    //Cargo Type

    public interface IRepository4<T> where T : class
    {
        Task<ActionResult<T>> GetById(int id);
        Task<ActionResult<IEnumerable<T>>> GetAll();
        Task<IActionResult> Create(T cargotype);
        Task<T> Update(int id, T cargotype);
        Task<T> Delete(int id);
    }
    //City
    public interface IRepository5<T> where T : class
    {
        Task<ActionResult<T>> CityById(int id);
        Task<ActionResult<IEnumerable<T>>> GetAllCities();
        Task<IActionResult> Create(T city);
        Task<T> Update(int id, T city);
        Task<T> Delete(int id);
    }




    //Employee Interface
    
    public interface IRepositoryEmployee<T> where T : class
    {
        Task<ActionResult<T>> GetById(int id);
        Task<ActionResult<IEnumerable<T>>> GetAll();
        Task<IActionResult> Create(T customer);
        Task<T> Update(int id, T admin);
        Task<T> Delete(int id);
    }
    //Order details

    public interface IRepositoryCODR<T> where T : class
    {
        Task<ActionResult<T>> GetById(int id);
        Task<ActionResult<IEnumerable<T>>> GetAll();
        Task<IActionResult> Create(T customer);
        Task<T> Update(int id, T admin);
        Task<T> Delete(int id);
    }

    public interface IRepositoryCargoStatus<T> where T : class
    {
        Task<ActionResult<T>> GetById(int id);
        Task<ActionResult<IEnumerable<T>>> GetAll();
        Task<IActionResult> Create(T customer);
        Task<T> Update(int id, T admin);
        Task<T> Delete(int id);

    }





}
