using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Base;
using NETCore.Context;
using NETCore.Model;
using NETCore.Repository.Data;
using NETCore.ViewModel;
using static Dapper.SqlMapper;

namespace NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController<Employee, EmployeeRepository>
    {
        private readonly EmployeeRepository _repository;

        public EmployeeController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this._repository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<EmployeeVM>> Get()
        {
            var get = await _repository.GetAllEmployee();
            return Ok(new {data = get});
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Employee entity)
        {
            entity.Id = id;
            var put = await _repository.Get(id);
            if (put == null)
            {
                return BadRequest();
            }
            if (entity.FirstName != null)
            {
                put.FirstName = entity.FirstName;
            }
            if (entity.LastName != null)
            {
                put.LastName = entity.LastName;
            }
            if (entity.Email != null)
            {
                put.Email = entity.Email;
            }
            if (entity.BirthDate != default(DateTime))
            {
                put.BirthDate = entity.BirthDate;
            }
            if (entity.Address != null)
            {
                put.Address = entity.Address;
            }
            if (entity.PhoneNumber != null)
            {
                put.PhoneNumber = entity.PhoneNumber;
            }
            if (entity.Department_Id != 0)
            {
                put.Department_Id = entity.Department_Id;
            }
            put.UpdateDate = DateTimeOffset.Now;
            await _repository.Put(put);
            return Ok("Update Successfully");
        }
    }
}