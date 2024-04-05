using DataLayer;
using Microsoft.EntityFrameworkCore;
using Natech.BussinessLayer;
using Natech.BussinessLayer.Interfaces;
using Natech.Common.Interfaces;
using Natech.Common.Services;
using Natech.DataLayer.Interface;
using Natech.DataLayer.Repositories;
using Natech.Services;
using Natech.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NatechAssignmentContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DbContext")));

builder.Services.AddLogging();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IGeolocationManager, GeolocationService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IBatchService, BatchService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
