using ElectronicJournal.API.Models;
using ElectronicJournal.API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ElectronicJournal.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<ElectronicJournalContext>(
				opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ElectronicJournal"))
			);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddAuthorization();
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = AuthorizationOptions.ISSUER,
						ValidateAudience = true,
						ValidateLifetime = false,
						ValidAudience = AuthorizationOptions.AUDIENCE,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = AuthorizationOptions.SECURITYKEY
					};
				});

			var app = builder.Build();

			app.UseAuthentication();
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}