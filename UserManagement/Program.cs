using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagement.Domain.DomainServices;
using UserManagement.Infrastructure.Data;
using UserManagement.Infrastructure.Interfaces;
using UserManagement.Infrastructure.Repositories;
using UserManagement.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddTransient<IAuthService,AuthService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.AddServiceDefaults();
builder.Services.AddDbContext<UserDbContext>(options =>  
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddControllers();
// Register Swagger/OpenAPI services so UseSwagger() can resolve ISwaggerProvider
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Removed app.MapDefaultEndpoints() to avoid duplicate/ambiguous endpoint metadata
// that causes Swashbuckle to fail with "Ambiguous HTTP method" when generating OpenAPI.

 // Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();

    // Redirect root to Swagger UI when running in Development
    app.MapGet("/", (HttpContext ctx) =>
    {
        ctx.Response.Redirect("/swagger/index.html");
        return Task.CompletedTask;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
