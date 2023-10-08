using AspNetCoreRateLimit;
using ElectronicJournal.API.DBModels;
using ElectronicJournal.API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using HealthChecks.UI.Client;

namespace ElectronicJournal.API
{
	public class Program
	{
		// https://code-maze.com/health-checks-aspnetcore/
		// https://blog.zhaytam.com/2020/04/30/health-checks-aspnetcore/
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

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
			
			builder.Services.AddSwaggerGen(); 

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
                        Limit = 5,
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
						ValidIssuer = AuthorizationOptions.ISSUER,
						ValidateIssuer = true,
						ValidAudience = AuthorizationOptions.AUDIENCE,
						ValidateAudience = true,
						IssuerSigningKey = AuthorizationOptions.SECURITYKEY,
						ValidateIssuerSigningKey = true,
						ValidateLifetime = false
					};
				});

			var app = builder.Build();
			
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

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