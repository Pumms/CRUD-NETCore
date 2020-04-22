using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Model;
using NETCore.Repository.Data;
using NETCore.ViewModel;

namespace NETCore.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController<Department, DepartmentRepository>
    {
        private readonly DepartmentRepository _repository;
        public DepartmentController(DepartmentRepository departmentRepository) : base(departmentRepository)
        {
            this._repository = departmentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<DepartmentVM>> Get()
        {
            var get = await _repository.Get();
            return Ok(new { data = get });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Department entity)
        {
            entity.Id = id;
            if (id != entity.Id)
            {
                return BadRequest();
            }
            var put = await _repository.Get(id);
            put.Name = entity.Name;
            put.UpdateDate = DateTimeOffset.Now; 
            await _repository.Put(put);
            return Ok("Update Successfully");
        }
    }
}