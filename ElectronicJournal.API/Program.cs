using AspNetCoreRateLimit;
using ElectronicJournal.API.DBModels;
using ElectronicJournal.API.Middlewares;
using ElectronicJournal.API.Utilities;
using ElectronicJournal.API.Utilities.Security.Hash;
using ElectronicJournal.API.Utilities.Security.JWT;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ElectronicJournal.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IHashProvider, HashProvider>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            builder.Services.AddTransient<ValidationMiddleware>();

            JwtOptions.Init(builder.Configuration);

            string connection = builder.Configuration.GetConnectionString("ElectronicJournal")
                ?? throw new ArgumentNullException(paramName: "ConnectionString");

            builder.Services.AddDbContext<ElectronicJournalContext>(
                opt => opt.UseSqlServer(connectionString: connection)
            );

            builder.Services.AddControllers().AddJsonOptions(
                configure: options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Electronic Journal \"MyJournal\" API",
                    Version = "v1",
                    Description = "API для моей информационной системы электронного журнала \"MyJournal\""
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Пожалуйста, вставьте сюда свой токен в формате: Bearer {токен}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }});
            });

            builder.Services.AddHealthChecks()
                .AddSqlServer(connectionString: connection, tags: new[] { "db" })
                .AddCheck<ApiHealthChecker>(name: nameof(ApiHealthChecker), tags: new[] { "api" });

            builder.Services.AddMemoryCache();
            builder.Services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.ClientIdHeader = "X-ClientId";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "1s",
                        Limit = 10,
                    }
                };
            });

            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            builder.Services.AddInMemoryRateLimiting();

            builder.Services.AddAuthorization();

            builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(configureOptions: options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = JwtOptions.Instance.Issuer,
                        ValidateIssuer = true,
                        ValidAudience = JwtOptions.Instance.Audience,
                        ValidateAudience = true,
                        IssuerSigningKey = JwtOptions.Instance.SymmetricKey,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = false
                    };
                });

            builder.Services.Configure<ApiBehaviorOptions>(configureOptions: options => options.SuppressModelStateInvalidFilter = true);

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseValidationMiddleware();

            app.MapHealthChecks(pattern: "/state", options: CreateHealthCheckOptions(predicate: _ => true));
            app.MapHealthChecks(pattern: "/state/api", options: CreateHealthCheckOptions(predicate: reg => reg.Tags.Contains(item: "api")));
            app.MapHealthChecks(pattern: "/state/db", options: CreateHealthCheckOptions(predicate: reg => reg.Tags.Contains(item: "db")));

            app.UseIpRateLimiting();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

            HealthCheckOptions CreateHealthCheckOptions(Func<HealthCheckRegistration, bool> predicate)
                => new HealthCheckOptions()
                {
                    Predicate = predicate,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                };
        }
    }
}