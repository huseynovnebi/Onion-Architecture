using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Api.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/Auth"))
            {
                await next(context);
                return;
            }

            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("UnAuthorized");
                return;
            }
            //Basic Auth
            //var header = context.Request.Headers["Authorization"].ToString();
            //var encodedcreds = header.Substring(6); // Basic username:pass
            //var creds = Encoding.UTF8.GetString(Convert.FromBase64String(encodedcreds));

            //var uidpass = creds.Split(':');

            //string uid = uidpass[0];
            //string password = uidpass[1];

            //if (uid != "user123" || password != "pass123")
            //{
            //    context.Response.StatusCode = 401;
            //    await context.Response.WriteAsync("UnAuthorized");
            //    return;
            //}

            //Bearer Auth
            var header = context.Request.Headers["Authorization"].ToString();
            var encodedcreds = header.Substring(7); // Bearer token
            context.Request.Cookies.TryGetValue("AuthToken", out string browsertoken);

            if (encodedcreds != browsertoken)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("UnAuthorized");
                return;
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                

                if (tokenHandler.CanReadToken(browsertoken))
                {
                    var securityToken = new JwtSecurityToken(browsertoken);
                    if (securityToken.ValidTo < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token has expired.");
                        return;
                    }
                }
            }
            await next.Invoke(context);
        }
    }
}
