using Core.Data;
using E_Commerce.Confiquration;
using E_Commerce.Helper;
using E_Commerce.MiddleWare;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var connection = builder.Configuration.GetConnectionString("c1");
builder.Services.AddDbContext<AppDBContext>(c =>
{
    c.UseSqlServer(connection);
});

//Add RedisCaching
builder.Services.AddSingleton<IConnectionMultiplexer>(r =>
{
    var connection=ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("redis"),true);
    return ConnectionMultiplexer.Connect(connection);
});
builder.Services.Configure<Emails>(builder.Configuration.GetSection("MailSetting"));


builder.Services.AddServices();
builder.Services.AddJWTServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();



var app = builder.Build();
#region Seeding Data 
await ApplySeeding.ApplySeedData(app);
#endregion
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMiddleware<ExceptionMiddleware>();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
