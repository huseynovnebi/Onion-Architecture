using Application.Interfaces;
using Domain;
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
    public class UserRepo : IUserRepo
    {
        private readonly UsersDbContext _dbContext;
        public UserRepo(UsersDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }


        public async Task<bool> SaveChangesAsync()
        {
            int result = await _dbContext.SaveChangesAsync(CancellationToken.None);

            return result >= 0;
        }
        public async Task<User?> GetByIdStrictAsync(int entityId)
        {
            User? entity = await _dbContext.User
                .FirstOrDefaultAsync(e => e.Id == entityId);

            return entity;
        }

        public EntityEntry<User> Add(User entity) => _dbContext.Add(entity);

        public IQueryable<User> GetAll()
        {
            IQueryable<User> entitiesQuery = _dbContext.User
                .AsQueryable();

            return entitiesQuery;
        }

        public void Remove(User entity) => _dbContext.Remove(entity);


        public void Update(User entity) => _dbContext.Update(entity);
    }
}
