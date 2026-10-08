using Application.ApplicationMuseo.Integrations.Events;
using Application.ApplicationMuseo.Integrations.Handlers.Subscribers;
using Application.Registrations;
using AutoMapper;
using Core.Application;
using Core.Application.Mapping;
using Filters;
using Infrastructure.Identity;
using Infrastructure.Registrations;
using Infrastructure.Repositories.Sql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


namespace API
{
    public class Startup
    {
        public IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddApplicationServices(Configuration);
            services.AddInfrastructureServices(Configuration);

            services.AddIdentity<UsuarioSistema, IdentityRole>()
                .AddEntityFrameworkStores<MuseoDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
               {
                   var jwtSettings = Configuration.GetSection("Jwt");
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = jwtSettings["Issuer"],
                       ValidAudience = jwtSettings["Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                       // Configure your issuer, audience, and signing key here
                   };
               });

            services.AddAuthorization();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hybrid Architecture Project",
                    Version = "v1"
                });
            });

            services.AddMvc(options =>
            {
                options.Filters.Add<BaseExceptionFilter>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                );
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            CustomMapper.Instance = app.ApplicationServices.GetRequiredService<IMapper>();
            app.SeedCalendarioMuseo();
            app.SeedRolesPermissions();  // 🔽 Nueva línea
            app.SeedConfiguracionVisitas();
            app.SeedConfiguracionHorarioAutoguiadas();
            app.SeedProvinciasArgentina();
            app.SeedDepartamentosArgentina();
            app.SeedLocalidadesArgentina();
            app.SeedSalasMuseo();
            app.SeedTematicasVisitas();
            app.SeedGuiasMuseo();
            app.SeedConfiguracionUsoSalas();
            app.SeedRecursosMuseo();
            app.SeedProyectoEducativo();
            app.SeedAreasMuseo();
            app.SeedPuestosMuseo();
            app.SeedAreaPuestoMuseo();



            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();
            app.UseAuthorization();

            //UseEventBus(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void UseEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<
                DummyEntityCreatedIntegrationEvent,
                DummyEntityCreatedIntegrationEventHandlerSub>();
        }
    }
}
