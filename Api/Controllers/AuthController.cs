using Application.JWTToken;
using Application.Models.DTO.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginAuth loginAuth)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest((400, "Request is not valid!"));
            }

            var UserName = configuration["AuthLogin:UserName"];
            var Password = configuration["AuthLogin:Password"];

            if (!loginAuth.UserName.Equals(UserName) || !loginAuth.Password.Equals(Password))
            {
                return Unauthorized("UnAuthorized");

            }
            JWTTokenGenerator jwtgen = new JWTTokenGenerator(configuration);

           string token = await jwtgen.GenerateToken(loginAuth.UserName);

            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true, // Accessible only through HTTP
                Secure = true,   // Secure (HTTPS) cookie
                Expires = DateTime.UtcNow.AddMinutes(30), // Set an expiration time
                SameSite = SameSiteMode.Strict 
            });

            return Ok(new {Token = token});
        }
    }
}
