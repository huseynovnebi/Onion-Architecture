using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Interfaces;
using Infastructure;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Repository
{
    public class Unitofwork : IUnitofwork
    {
        private readonly UsersDbContext _dbContext;
        public Unitofwork(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
            User = new UserRepo(_dbContext);
        }
        public IUserRepo User { get; private set; }

        public void Dispose() { 

        _dbContext.Dispose(); 

        }

        public async Task<bool> SaveChangesAsync()
        {
            int result = await _dbContext.SaveChangesAsync(CancellationToken.None);

            return result >= 0;
        }
    }
}
