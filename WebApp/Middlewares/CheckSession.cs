using System.Web;

namespace WebApp.Middlewares
{
    public class CheckSessionMiddleware
    {
        private readonly RequestDelegate _next;
        public CheckSessionMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext httpContext)
        {
            var rule1 = httpContext.Request.Path.ToString().ToLower().Contains("account");
            var rule2 = httpContext.Request.Path.ToString().ToLower().Contains("purchase");
            var rule3 = httpContext.Request.Path.ToString().ToLower().Contains("contracts");
            var rule4 = httpContext.Request.Path.ToString().ToLower().Contains("login");


            if ( rule1 || rule2 || rule3)
            {
                var user_id = httpContext.Session.GetInt32("user_id");
                if (user_id == null)
                {
                    await Task.Run(
                      async () =>
                      {
                          var callback_url = httpContext.Request.Path.ToString();
                          httpContext.Session.SetString("callback_url",callback_url);
                          httpContext.Response.StatusCode = StatusCodes.Status301MovedPermanently;
                          httpContext.Response.Redirect("/Login?callback="+HttpUtility.UrlEncode(callback_url));
                      }
                    );
                }
                else
                {
                    httpContext.Response.Headers.Add("Pipe2", new[] { DateTime.Now.ToString() });
                    await _next(httpContext);
                }    
                

            }
            else
            {
                httpContext.Response.Headers.Add("Pipe2", new[] { DateTime.Now.ToString() });
                await _next(httpContext);

            }

        }

    }
}
