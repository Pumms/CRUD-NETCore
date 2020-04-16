using Microsoft.EntityFrameworkCore;
using NETCore.Base;
using NETCore.Context;
using NETCore.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace NETCore.Repository
{
    public class GeneralRepository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : MyContext
    {
        private readonly MyContext _myContext;
        public GeneralRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await Get(id);
            if (entity == null)
            {
                return entity;
            }
            entity.IsDelete = true;
            entity.DeleteDate = DateTimeOffset.Now;
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TEntity>> Get()
        {
            return await _myContext.Set<TEntity>().Where(x => x.IsDelete == false).ToListAsync();
        }

        public async Task<TEntity> Get(int id)
        {
            return await _myContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> Post(TEntity entity)
        {
            entity.CreateDate = DateTimeOffset.Now;
            await _myContext.Set<TEntity>().AddAsync(entity);
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Put(TEntity entity)
        {
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }
    }
}
