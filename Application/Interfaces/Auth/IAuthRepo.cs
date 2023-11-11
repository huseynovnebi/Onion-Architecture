using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Auth
{
    public interface IAuthRepo
    {
        Task<List<RefreshToken>> GetAllRefreshToken();
        Task<RefreshToken?> GetRefreshToken(string inptokenhash);

        void RemoveRefreshToken(RefreshToken rftoken);

        Task AddRefreshToken(RefreshToken rftoken);

    }
}
