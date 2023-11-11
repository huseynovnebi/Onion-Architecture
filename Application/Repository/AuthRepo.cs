using Application.Interfaces.Auth;
using Domain;
using Infastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository
{
    public class AuthRepo :IAuthRepo
    {
        private readonly UsersDbContext _dbcontext;

        public AuthRepo(UsersDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<List<RefreshToken>> GetAllRefreshToken () => await _dbcontext.RefreshToken.ToListAsync();

        public async Task<RefreshToken?> GetRefreshToken(string inptokenhash) => await _dbcontext.RefreshToken.SingleOrDefaultAsync(t => t.RefreshTokengd == inptokenhash);

        public void RemoveRefreshToken(RefreshToken rftoken) { _dbcontext.Remove(rftoken); }

        public async Task AddRefreshToken(RefreshToken rftoken) { _dbcontext.AddAsync(rftoken); }
    }
}
