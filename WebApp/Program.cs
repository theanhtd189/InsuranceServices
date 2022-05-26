

using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using WebApp.Middlewares;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

/*builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.AreaViewLocationFormats.Clear();
    options.AreaViewLocationFormats.Add("/Areas/Admin/Views/{1}/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Areas/Admin/Views/Shared/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Admin/Views/{1}/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Admin/Views/Shared/{0}.cshtml");
    // options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
});*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

//app.UseMiddleware<CheckAdminSessionMiddleware>();

app.UseMiddleware<CheckSessionMiddleware>();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
