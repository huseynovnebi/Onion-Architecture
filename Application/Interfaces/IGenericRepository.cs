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
        void Add(T entity);

        IQueryable<T> GetAll();
        //Task<bool> SaveChangesAsync();

        Task<T?> GetByIdStrictAsync(int entityId);
        void Remove(T entity);

        void Update(T entity);
    }
}
