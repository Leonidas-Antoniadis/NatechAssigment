using DataLayer;
using Microsoft.EntityFrameworkCore;
using Natech.BussinessLayer;
using Natech.BussinessLayer.Interfaces;
using Natech.DataLayer.Interface;
using Natech.DataLayer.Repositories;
using NatechAssignment.Services;
using NatechAssignment.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NatechAssignmentContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DbContext")));

builder.Services.AddLogging();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IGeolocationManager, GeolocationManager>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICommunicationService, CommunicationService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
