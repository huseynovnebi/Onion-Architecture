using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JWTToken
{
    public class JWTTokenIsExpired
    {
        public bool IsExpired(string tokenValue)
        {
            bool status = false;
            if (!string.IsNullOrEmpty(tokenValue))
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                    var token = tokenHandler.ReadToken(tokenValue) as JwtSecurityToken;

                    if (token != null)
                    {
                        if (DateTime.UtcNow >= token.ValidTo)
                        {
                            status = true;
                        }
                    }
            }
            return status;
        }
    }
}
    

