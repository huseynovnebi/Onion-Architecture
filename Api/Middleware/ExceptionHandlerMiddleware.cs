using Serilog;

namespace Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate Next)
        {
            next = Next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
               await next.Invoke(context);
            }
            catch (Exception ex)
            {
                Log.Information("Exception ocurred:" + ex.Message);
                throw;
            }

        }
    }
}
