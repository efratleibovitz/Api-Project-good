using Microsoft.EntityFrameworkCore;
using Repository;
using Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<WebApiShop216328971Context>(options => options.UseSqlServer("Data Source=srv2\\pupils;Initial Catalog=ApiShop216328971;Integrated Security=True;Trust Server Certificate=True; Pooling=False"));

builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService,PasswordService>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

