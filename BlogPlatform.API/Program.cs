// using BlogPlatform.Domain.Interfaces;
// using BlogPlatform.Infrastructure.Repositories;
// using BlogPlatform.Infrastructure.UnitOfWork;

// using BlogPlatform.Infrastructure.Persistence;
// using Microsoft.EntityFrameworkCore;

// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;
// using BlogPlatform.Application.Interfaces;
// using BlogPlatform.Infrastructure.Services;
// using System.Reflection;

// var builder = WebApplication.CreateBuilder(args);

// //adding infastructure services
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// builder.Services.AddScoped<IAuthService, AuthService>();

// // Add DbContext
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// // Add services to the container.
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
// //add open API - swagger ui 
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// builder.Services.AddSwaggerGen(c =>
// {
//     var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//     c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
// });


// //Authentication and Authorization
// var jwtKey = builder.Configuration["Jwt:Key"];
// var jwtIssuer = builder.Configuration["Jwt:Issuer"];

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = false,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = jwtIssuer,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//         };
//     });

// builder.Services.AddAuthorization();


// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();
// //using authentication and authorization
// app.UseAuthentication();
// app.UseAuthorization();

//  // Redirect root path to Swagger UI
// app.MapGet("/", context =>
// {
//     context.Response.Redirect("/swagger");
//     return Task.CompletedTask;
// });

//  app.MapControllers();
// //using swagger
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }


// app.Run();

 



using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BlogPlatform.Application.Interfaces;
using BlogPlatform.Domain.Interfaces;
using BlogPlatform.Infrastructure.Persistence;
using BlogPlatform.Infrastructure.Repositories;
using BlogPlatform.Infrastructure.Services;
using BlogPlatform.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger + XML Comments
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Auth config
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Middleware order matters
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Use Swagger (only in development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect root URL to Swagger UI
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

// Map controllers
app.MapControllers();

app.Run();
