using System;
using System.Text;
using ComeFrexco.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ComeFrexco
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//Hace referencia a la inyección de dependencias, para la clase de conexión
			services.AddScoped<IConfig, Config>();

			//Esta linea configura el servicio de autenticacion mediante tokens, para que cada vez que se realice una petición
			//al servidor esta valide si se esta o no autenticado 
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = "https://ComeFrexco.azurewebsites.net",
				ValidAudience = "https://ComeFrexco.azurewebsites.net",
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.getSecret())),
				ClockSkew = TimeSpan.Zero
			});

			//Agrega la version de compativilidad
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			///Agrega esto para filtrar las peticiones que se aceptan en el servidor.
			services.AddCors(options => options.AddDefaultPolicy(builder =>
				{
					builder.WithOrigins("*").WithMethods("POST", "PUT", "DELETE", "GET").AllowAnyHeader();
				}));

			///Configuración por defecto de objetos Json, al quitar esto todas las respuestas seran en XML.
			services.AddMvc().AddJsonOptions(ConfigureJson);
		}

		///Configuración por defecto de objetos Json
		private void ConfigureJson(MvcJsonOptions obj)
		{
			obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsProduction())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseCors();
			app.UseAuthentication();
			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
