using Microsoft.EntityFrameworkCore;
using MilitaryVehicles.common;
using MilitaryVehicles.infrastructure;

var builder = WebApplication.CreateBuilder(args);

var projectDbPath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\DB\MilitaryVehiclesDB.sqlite");
var dbPath = Path.GetFullPath(projectDbPath);

builder.Services.AddDbContext<MilitaryVehiclesContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

Console.WriteLine($"DB path: {dbPath}");

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(ICrudServiceAsync<>), typeof(CrudServiceAsync<>));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<MilitaryVehiclesContext>();
Console.WriteLine($"Helicopters in DB: {db.Helicopters.Count()}");
Console.WriteLine($"CrewMembers in DB: {db.CrewMembers.Count()}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();