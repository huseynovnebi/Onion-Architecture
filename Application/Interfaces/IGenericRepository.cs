using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGenericRepository<T> where T:class
    {
        Task Add(T entity);

        Task<IQueryable<T>> GetAll();
        //Task<bool> SaveChangesAsync();

        Task<T?> GetByIdStrictAsync(int entityId);
        Task Remove(T entity);

        Task Update(T entity);
    }
}
