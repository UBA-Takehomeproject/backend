
// using System.Reflection;
// using System.Text;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using BlogPlatform.Application.Interfaces;
// using BlogPlatform.Domain.Interfaces;
// using BlogPlatform.Infrastructure.Persistence;
// using BlogPlatform.Infrastructure.Repositories;
// using BlogPlatform.Infrastructure.Services;
// using BlogPlatform.Infrastructure.UnitOfWork;
// using Microsoft.OpenApi.Models;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// builder.Services.AddScoped<IAuthService, AuthService>();

// // DbContext
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// // Swagger + XML Comments
// builder.Services.AddEndpointsApiExplorer();
 

// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogPlatformAPI", Version = "v1" });

//     // XML comments
//     var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//     c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

//     // JWT Bearer token auth
//     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Name = "Authorization",
//         Type = SecuritySchemeType.ApiKey,
//         Scheme = "Bearer",
//         BearerFormat = "JWT",
//         In = ParameterLocation.Header,
//         Description = "Enter 'Bearer {your token}'"
//     });

//     c.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "Bearer"
//                 }
//             },
//             Array.Empty<string>()
//         }
//     });
// });




// // Auth config
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


//  builder.Services.AddAuthorization();


// // Controllers
// builder.Services.AddControllers();

// var app = builder.Build();

// // Middleware order matters
// app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();

// // Use Swagger (only in development)
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// // Redirect root URL to Swagger UI
// app.MapGet("/", context =>
// {
//     context.Response.Redirect("/swagger");
//     return Task.CompletedTask;
// });

// // Map controllers
// app.MapControllers();

// app.Run();
