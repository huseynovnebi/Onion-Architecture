using Serilog;

namespace Api.Middleware
{
    public class RequestResponseMiddleware
    {
        private readonly RequestDelegate next;

        public RequestResponseMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpcontext)
        {
           #region Request
            var originalStream = httpcontext.Response.Body;

            MemoryStream requestbody = new MemoryStream();

            await httpcontext.Request.Body.CopyToAsync(requestbody);

            requestbody.Seek(0, SeekOrigin.Begin);
            httpcontext.Request.Body = requestbody;

            string requesttext = await new StreamReader(requestbody).ReadToEndAsync();
            Log.Information("Request body:" +requesttext);

            requestbody.Seek(0, SeekOrigin.Begin);
            httpcontext.Request.Body = requestbody;
            #endregion

            var tempStream = new MemoryStream();
            httpcontext.Response.Body = tempStream;
            await next.Invoke(httpcontext);


            #region Response
            httpcontext.Response.Body.Seek(0, SeekOrigin.Begin);
            string responsetext =  await new StreamReader(httpcontext.Response.Body).ReadToEndAsync();
            Log.Information("Response body:" + responsetext);

            httpcontext.Response.Body.Seek(0, SeekOrigin.Begin);
             
            await httpcontext.Response.Body.CopyToAsync(originalStream);

            #endregion
        }
    }
}
