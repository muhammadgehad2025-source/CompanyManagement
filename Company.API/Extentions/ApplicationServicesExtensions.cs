using Company.API.Errors;
using Company.API.Helpers;
using Company.Core.Interfaces;
using Company.Core.Interfaces.Services;
using Company.Infrastructure.Data;
using Company.Infrastructure.Identity;
using Company.Infrastructure.Repositories;
using Company.Infrastructure.Services;
using Company.Infrastructure.UnitOfWork;
using Company.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;

namespace Company.API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            // ---------------- DB ----------------
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            // ---------------- REPOS ----------------
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ---------------- MAPPER ----------------
            services.AddAutoMapper(typeof(MappingProfiles));

            // ---------------- SERVICES ----------------
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();

            // ---------------- FACTORIES ----------------
            services.AddScoped<Func<IEmployeeService>>(sp =>
                () => sp.GetRequiredService<IEmployeeService>());

            services.AddScoped<Func<IAuthService>>(sp =>
                () => sp.GetRequiredService<IAuthService>());

            // ---------------- SERVICE MANAGER ----------------
            services.AddScoped<IServiceManager, ServiceManager>();

            // ---------------- IDENTITY ----------------
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            // ================= JWT (🔥 FIXED PROPERLY) =================
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["JWT:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = config["JWT:Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["JWT:Key"]!)
                    )
                };

                // 🔥 DEBUG EVENTS (VERY IMPORTANT)
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("❌ JWT FAILED: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("✅ JWT VALIDATED");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine("⚠️ JWT CHALLENGE");
                        return Task.CompletedTask;
                    }
                };
            });

            // ---------------- REDIS ----------------
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(
                    config.GetConnectionString("Redis"), true);

                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddScoped<IBasketRepository, BasketRepository>();

            // ---------------- VALIDATION ----------------
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();

                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = errors
                    });
                };
            });

            // ---------------- SWAGGER AUTH ----------------
            services.AddSwaggerGen(options =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                options.AddSecurityDefinition("Bearer", securityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, new string[] { } }
                });
            });

            return services;
        }
    }
}