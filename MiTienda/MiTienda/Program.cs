using Microsoft.EntityFrameworkCore;
using MiTienda.Context;
using MiTienda.Repositories;
using MiTienda.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// sirve para conectarse a la DB 
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlString"));
});

//Le dicen a ASP.NET Core que cree y gestione automáticamente las instancias de esas clases (inyección de dependencias)
//le dice al sistema cm crear tus clases y pasarlas automáticamente donde se necesiten.
builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddScoped<CategoriaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
