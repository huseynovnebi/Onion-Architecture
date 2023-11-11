using Application.Helpers;
using Domain;
using Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.JWTToken
{
    public class RefreshTokenGenerator
    {
        private readonly UsersDbContext dbcontext;

        public RefreshTokenGenerator(UsersDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task<(string token,RefreshToken refreshtoken)> GenerateToken()
        {
            
            const int IterationCount = 10000; 
            const int HashSize = 32; 
            string tokenhash = string.Empty;


            Guid tokenGuid = Guid.NewGuid();
            Guid saltGuid = Guid.NewGuid();
            byte[] tokenBytes = tokenGuid.ToByteArray();
            byte[] saltBytes = saltGuid.ToByteArray();
            Convert.ToBase64String(tokenBytes);

             await Task.Run(() =>
            {
                tokenhash = TokenHash.GetHash(tokenBytes,saltBytes,IterationCount,HashSize);
            });

            DateTime exptime = DateTime.Now + TimeSpan.FromDays(30);
            RefreshToken refreshtoken = new RefreshToken() { RefreshTokengd = tokenhash, Salt = saltGuid.ToString(),ExpirationDate = exptime };
         
           

            return (tokenGuid.ToString(), refreshtoken);

        }
    }
}
