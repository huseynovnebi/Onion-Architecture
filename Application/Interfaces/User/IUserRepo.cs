using Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepo
    {
        EntityEntry<User> Add(User entity);

        IQueryable<User> GetAll();
        Task<bool> SaveChangesAsync();

        Task<User?> GetByIdStrictAsync(int entityId);
        void Remove(User entity);

        void Update(User entity);
    }
}
