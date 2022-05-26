using System.Linq;
namespace WebApp.Middlewares
{
    public class CheckAdminSessionMiddleware
    {
        private readonly RequestDelegate _next;
        public CheckAdminSessionMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext httpContext)
        {
            var _list = httpContext.Request.Path.ToString().ToLower().Split('/');
            var rule1 = Array.IndexOf(_list,"admin")>=0;
            var rule2 = Array.IndexOf(_list, "login") >= 0;

            if (rule1 && !rule2)
            {
                var user_id = httpContext.Session.GetInt32("admin_id");
                if (user_id == null)
                {
                    await Task.Run(
                      async () =>
                      {
                          var callback_url = httpContext.Request.Path.ToString();
                          httpContext.Session.SetString("callback_url", callback_url);
                          httpContext.Response.StatusCode = StatusCodes.Status301MovedPermanently;
                          httpContext.Response.Redirect("/Admin/Login");
                      }
                    );
                }
                else
                {
                    httpContext.Response.Headers.Add("Pipe1", new[] { DateTime.Now.ToString() });
                    await _next(httpContext);
                }


            }
            else
            {
                httpContext.Response.Headers.Add("Pipe1", new[] { DateTime.Now.ToString() });
                await _next(httpContext);

            }

        }
    }
}
