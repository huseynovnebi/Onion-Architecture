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
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("UnAuthorized");
                return;
            }
            //Basic Auth
            var header = context.Request.Headers["Authorization"].ToString();
            var encodedcreds = header.Substring(6); // Basic username:pass
            var creds = Encoding.UTF8.GetString(Convert.FromBase64String(encodedcreds));

            var uidpass = creds.Split(':');

            string uid = uidpass[0];
            string password = uidpass[1];

            if (uid != "user123" || password != "pass123")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("UnAuthorized");
                return;
            }

            //Bearer Auth
            //var header = context.Request.Headers["Authorization"].ToString();
            //var encodedcreds = header.Substring(7); // Bearer token

            //if (encodedcreds != "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c")
            //{
            //    context.Response.StatusCode = 401;
            //    await context.Response.WriteAsync("UnAuthorized");
            //    return;
            //}
            await next.Invoke(context);
        }
    }
}
