using Application.Interfaces;
using Infastructure;
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
            _dbContext.Set<T>().Add(entity);
        } 


        public async Task<IQueryable<T>> GetAll()
        {
            IQueryable<T> entitiesQuery =  _dbContext.Set<T>()
               .AsQueryable();

            return entitiesQuery;
        }

        public async Task<T?> GetByIdStrictAsync(int entityId)
        {
            T? entity = await _dbContext.Set<T>()
                .FindAsync(entityId);

            return entity;
        }

        public async Task Remove(T entity) =>  _dbContext.Set<T>().Remove(entity);

        public async Task Update(T entity) {  _dbContext.Set<T>().Update(entity); }

    }
}
