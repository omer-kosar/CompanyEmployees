﻿using AspNetCoreRateLimit;
using CompanyEmployees.Formatters;
using Contracts.Logger;
using Contracts.Repository;
using Entities.ConfigurationModels;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using Service;
using Service.Contracts;
using System.Text;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) => services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("X-Pagination"));
        });
        //public static void ConfigureIISIntegration(this IServiceCollection services) => services.Configure<IISOptions>(options => { });
        public static void ConfigureLoggerService(this IServiceCollection services) => services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureRepositoryManager(this IServiceCollection services) => services.AddScoped<IRepositoryManager, RepositoryManager>();
        public static void ConfigureServiceManager(this IServiceCollection services) => services.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) => builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
        public static void ConfigureVersioning(this IServiceCollection services) => services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
        });
        public static void ConfigureResponseCaching(this IServiceCollection services) => services.AddResponseCaching();
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint="*",
                    Limit=3,
                    Period="5m"
                }
            };
            services.Configure<IpRateLimitOptions>(opt => opt.GeneralRules = rateLimitRules);
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 3;
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
        }
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfiguration = new JwtConfiguration();
            configuration.Bind(jwtConfiguration.Section, jwtConfiguration);

            var secretKey = Environment.GetEnvironmentVariable("SECRET") ?? "CodeMazeSecretKey";
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfiguration.ValidIssuer,
                    ValidAudience = jwtConfiguration.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration) => services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Code Maze API", Version = "v1" });
                swagger.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Code Maze API", Version = "v2" });
                swagger.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Name="Bearer"
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
