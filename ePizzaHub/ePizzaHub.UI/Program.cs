using ePizzaHub.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) =>lc.ReadFrom.Configuration(ctx.Configuration));


ConfigureDependencies.RegisterService(builder.Services,builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "epizzahubapp";
    options.LoginPath = new PathString("/account/Login");
    options.SlidingExpiration = true;
    options.AccessDeniedPath = new PathString("/account/unauthorize");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        const int durationInSecond = 60 * 60 * 24 * 7;
        ctx.Context.Response.Headers["cache-control"] =
        "public,max-age=" + durationInSecond;
    }
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
     name: "areas",
     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
   );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
