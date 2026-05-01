using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MilitaryVehicles.common;
using MilitaryVehicles.infrastructure;
using MilitaryVehicles.infrastructure.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var projectDbPath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\DB\MilitaryVehiclesDB.sqlite");
var dbPath = Path.GetFullPath(projectDbPath);

builder.Services.AddDbContext<MilitaryVehiclesContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MilitaryVehiclesContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],

        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

        ValidateLifetime = true
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MilitaryVehicles.REST",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Введіть JWT токен у форматі: Bearer {токен}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(ICrudServiceAsync<>), typeof(CrudServiceAsync<>));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MilitaryVehiclesContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    db.Database.Migrate();

    string[] roles = new[] { "User", "Editor", "Admin" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var adminEmail = "admin@example.com";
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "Super Admin"
        };

        var result = await userManager.CreateAsync(admin, "Admin123!");
        if (!result.Succeeded)
        {
            Console.WriteLine("Помилка при створенні адміну: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        else
        {
            await userManager.AddToRoleAsync(admin, "Admin");
            Console.WriteLine("Admin user created: " + adminEmail);
        }
    }

    Console.WriteLine($"Helicopters in DB: {db.Helicopters.Count()}");
    Console.WriteLine($"CrewMembers in DB: {db.CrewMembers.Count()}");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();