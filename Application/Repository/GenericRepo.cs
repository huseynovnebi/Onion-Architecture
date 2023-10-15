using Application.Interfaces;
using Infastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository
{
    public class GenericRepo<T> : IGenericRepository<T> where T : class
    {
        private readonly UsersDbContext _dbContext;
        public GenericRepo(UsersDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        } 


        public async Task<List<T>> GetAll()
        {
            List<T> entitiesQuery = await _dbContext.Set<T>().AsNoTracking().ToListAsync();

            return entitiesQuery;
        }

        public async Task<T?> GetByIdStrictAsync(int entityId)
        {
            T? entity = await _dbContext.Set<T>()
                .FindAsync(entityId);

            return entity;
        }

        public void Remove(T entity) { _dbContext.Set<T>().Remove(entity); }

        public void Update(T entity) {  _dbContext.Set<T>().Update(entity); }

    }
}
