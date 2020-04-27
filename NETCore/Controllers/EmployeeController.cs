using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.Base;
using NETCore.Context;
using NETCore.Model;
using NETCore.Repository.Data;
using NETCore.ViewModel;
using static Dapper.SqlMapper;

namespace NETCore.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController<Employee, EmployeeRepository>
    {
        private readonly EmployeeRepository _repository;

        public EmployeeController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this._repository = employeeRepository;
        }

        [HttpDelete("{email}")]
        public async Task<ActionResult<Employee>> Delete(string email)
        {
            var delete = await _repository.Delete(email);

            if (delete == null)
            {
                return NotFound();
            }
            return delete;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<Employee>> GetEmail(string email)
        {
            var getemail = await _repository.GetByEmail(email);
            return Ok(new { data = getemail });
        }

        [HttpGet]
        public async Task<ActionResult<EmployeeVM>> GetAll()
        {
            var getall = await _repository.GetAllEmployee();
            return Ok(new {data = getall});
        }

        [HttpGet]
        [Route("Chart")]
        [HttpGet]
        public async Task<IEnumerable<ChartVM>> GetChart()
        {
            return await _repository.GetJmlEmployee();
        }

        [HttpPut("{email}")]
        public async Task<ActionResult> Put(string email, Employee model)
        {
            model.Email = email;
            var put = await _repository.GetByEmail(email);
            if (put == null)
            {
                return BadRequest();
            }
            if (model.FirstName != null)
            {
                put.FirstName = model.FirstName;
            }
            if (model.LastName != null)
            {
                put.LastName = model.LastName;
            }
            if (model.BirthDate != default(DateTime))
            {
                put.BirthDate = model.BirthDate;
            }
            if (model.Address != null)
            {
                put.Address = model.Address;
            }
            if (model.PhoneNumber != null)
            {
                put.PhoneNumber = model.PhoneNumber;
            }
            if (model.Department_Id != 0)
            {
                put.Department_Id = model.Department_Id;
            }
            put.UpdateDate = DateTimeOffset.Now;
            await _repository.Put(put);
            return Ok("Update Successfully");
        }
    }
}