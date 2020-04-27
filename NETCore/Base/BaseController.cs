using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Base;
using NETCore.Repository.Interface;

namespace NETCore.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity, TRepository>  : ControllerBase
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
    {
        private readonly TRepository _repository;

        public BaseController(TRepository repository) { this._repository = repository;  }

        [HttpPost]
        public async Task<ActionResult<TEntity>> Post(TEntity entity)
        {
            var post = await _repository.Post(entity);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEntity>> Delete(int id)
        {
            var delete = await _repository.Delete(id);
            if (delete == null)
            {
                return NotFound();
            }
            return delete;
        }
    }
}