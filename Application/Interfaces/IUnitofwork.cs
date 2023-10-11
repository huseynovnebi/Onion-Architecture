using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitofwork:IDisposable
    {
        public IUserRepo User { get; }

        Task<bool> SaveChangesAsync();
    }
}
