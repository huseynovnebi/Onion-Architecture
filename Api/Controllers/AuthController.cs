using Application.Helpers;
using Application.Interfaces;
using Application.JWTToken;
using Application.Models.DTO.Auth;
using Domain;
using Infastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly UsersDbContext _dbcontext;
        private readonly IUnitofwork unitofwork;

        public AuthController(IConfiguration configuration, UsersDbContext dbcontext,IUnitofwork unitofwork)
        {
            this.configuration = configuration;
            _dbcontext = dbcontext;
            this.unitofwork = unitofwork;
        }

        [HttpPost]
        [Route("api/[controller]/Login")]
        public async Task<IActionResult> Login([FromBody] LoginAuthDto loginAuth)
        {
            var creds = await _dbcontext.LoginCreds.ToListAsync();

            if (!loginAuth.UserName.Equals(creds[0].UserName) || !loginAuth.Password.Equals(creds[0].Password))
            {
                return Unauthorized("UnAuthorized");

            }
            JWTTokenGenerator jwtgen = new JWTTokenGenerator(configuration);
            RefreshTokenGenerator refreshtoken = new RefreshTokenGenerator(_dbcontext);


            string token = await jwtgen.GenerateToken(loginAuth.UserName);
            (string reftoken, RefreshToken refreshtokenobj) = await refreshtoken.GenerateToken();

            await unitofwork.authRepo.AddRefreshToken(refreshtokenobj);

            await unitofwork.SaveChangesAsync();

            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true, // Accessible only through HTTP
                Secure = true,   // Secure (HTTPS) cookie
                Expires = DateTime.UtcNow.AddMinutes(10), // Set an expiration time
                SameSite = SameSiteMode.Strict
            });

            return Ok(new { Token = token, RefreshToken = reftoken });
        }

        [HttpPost]
        [Route("api/[controller]/ReniewAccessToken")]
        public async Task<IActionResult> ReniewToken([FromBody] ReniewTokenAuth reniewtoken)
        {
            var refreshtoken = await _dbcontext.RefreshToken.ToListAsync();
            var creds = await _dbcontext.LoginCreds.ToListAsync();
            var salt = Guid.Parse(refreshtoken[0].Salt);
            byte[] tokenBytes = reniewtoken.RefreshToken.ToByteArray();
            byte[] saltBytes = salt.ToByteArray();
            string inptokenhash = TokenHash.GetHash(tokenBytes, saltBytes, 10000, 32);
            var existingToken = _dbcontext.RefreshToken.SingleOrDefault(t => t.RefreshTokengd == inptokenhash);
            string? tokenValue = Request.Cookies["AuthToken"];



            //Can be eperate method for toke validation
            if (existingToken is null)
            {
                return BadRequest("RefreshToken is not true");
            }

            if (existingToken.ExpirationDate < DateTime.UtcNow)
            {
                return BadRequest("RefreshToken is expired");
            }
            JWTTokenIsExpired tokenIsExpired = new JWTTokenIsExpired();

            if (!string.IsNullOrEmpty(tokenValue))
            {
                bool status = tokenIsExpired.IsExpired(tokenValue);

                if (!status)
                {
                    return BadRequest("AccessToken is not expired");
                }
            }

            JWTTokenGenerator jwtgen = new JWTTokenGenerator(configuration);
            string token = await jwtgen.GenerateToken(creds[0].UserName);

            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true, // Accessible only through HTTP
                Secure = true,   // Secure (HTTPS) cookie
                Expires = DateTime.UtcNow.AddMinutes(2), // Set an expiration time
                SameSite = SameSiteMode.Strict
            });

            return Ok(new
            {
                Token = token,
            });
        }

        [HttpPost]
        [Route("api/[controller]/ReniewRefreshToken")]

        public async Task<IActionResult> ReniewRefreshToken([FromBody] ReniewTokenAuth reniewtoken)
        {
            var refreshtoken = await unitofwork.authRepo.GetAllRefreshToken();

            var salt = Guid.Parse(refreshtoken[0].Salt);
            byte[] tokenBytes = reniewtoken.RefreshToken.ToByteArray();
            byte[] saltBytes = salt.ToByteArray();
            string inptokenhash = TokenHash.GetHash(tokenBytes, saltBytes, 10000, 32);

            var existingToken = await unitofwork.authRepo.GetRefreshToken(inptokenhash);

            if (existingToken is null)
            {
                return BadRequest("RefreshToken is not true");
            }

            if (existingToken.ExpirationDate > DateTime.UtcNow)
            {
                return BadRequest("RefreshToken is not expired");
            }

             unitofwork.authRepo.RemoveRefreshToken(existingToken);
            

            RefreshTokenGenerator rfgen = new RefreshTokenGenerator(_dbcontext);

            (string rftokennew,RefreshToken refreshtokenobj)  = await rfgen.GenerateToken();

            await unitofwork.authRepo.AddRefreshToken(refreshtokenobj);

            await unitofwork.SaveChangesAsync();

            return Ok(new { RefreshToken = rftokennew });
        }
    }
}
//var UserName = configuration["AuthLogin:UserName"];
//var Password = configuration["AuthLogin:Password"];
