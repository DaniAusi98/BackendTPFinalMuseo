using Application.ApplicationMuseo.ApplicationServices;
using Application.Availability.ApplicationServices;
using Application.Common.ApplicationServices;
using Application.PersonalMuseo.ApplicationServices;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.VisitaGrupal.ApplicationServices;
using Core.Application.Adapters.Http;
using Core.Infraestructure;
using Core.Infraestructure.Adapters.Http;
using Hangfire;
using Hangfire.MySql;
using Infrastructure.Adapters;
using Infrastructure.Adapters.EmailSender.ResendEmailService;
using Infrastructure.Adapters.EmailSender.ResendEmailService.ConfirmarVisitaUrl;
using Infrastructure.Adapters.EmailSender.ResendEmailService.User;
using Infrastructure.Constants;
using Infrastructure.Data.Seeders;
using Infrastructure.Data.Seeders.Ubicacion;
using Infrastructure.Factories;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;
using System.Transactions;

namespace Infrastructure.Registrations
{
    /// <summary>
    /// Aqui se deben registrar todas las dependencias de la capa de infraestructura
    /// </summary>
    public static class InfraestructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            /* Database Context */
            services.AddRepositories(configuration);



            /* REGISTRO DE HANGFIRE JUSTO AQUÍ*/
            // Obtenemos el tipo de base de datos activa
            string dbType = configuration["Configurations:UseDatabase"] ?? "";

            // Solo configuramos Hangfire si la base de datos activa es MYSQL
            if (dbType.Equals("mysql", StringComparison.OrdinalIgnoreCase))
            {
                string connectionString = configuration.GetConnectionString("MySqlConnection")
                    ?? throw new NullReferenceException("La cadena 'MySqlConnection' no está configurada.");

                services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions
                    {
                        TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true, // Creará las tablas automáticamente al iniciar
                        DashboardJobListLimit = 50000,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        TablesPrefix = "Hangfire_"
                    })));

                // Arranca el servidor que procesa las tareas en segundo plano
                services.AddHangfireServer();
            }
            /* 🔼 TERMINA HANGFIRE 🔼 */
            services.AddScoped<IGuidedTourConfirmationExpired, HangfireGuidedTourExpirationScheduler>();


            /* EventBus */
            services.AddEventBus(configuration);

            services.Configure<ResendClientOptions>(options =>
            {
                options.ApiToken = configuration["Resend:ApiKey"]!;
            });

            services.AddHttpClient<ResendClient>();
            services.AddTransient<IResend, ResendClient>();
            services.AddScoped<IConfirmUserUrl, ConfirmUserUrl>();
            services.AddScoped<IConfirmUserUrlDashboard, ConfirmUserUrlDashboard>();


            services.AddScoped<IURLConfirmacionVisitaGrupal, UrlConfirmacionVisitaGrupal>();

            services.AddScoped<IEmailService, ResendEmailService>();

            /* Adapters */
            services.AddSingleton<IExternalApiClient, ExternalApiHttpAdapter>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<JwtTokenService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IClock, ArgentinaClock>();
            services.AddSingleton<IRecurrenceEvaluator, IcalRecurrenceEvaluator>();

            services.AddHttpClient<IConsultarProvinciasArgetina, ObtenerProvinciasArgentinaGeoRef>(client =>
            {
                // Aquí es donde va la base del Curl. Tiene que terminar siempre con una barra diagonal '/'
                client.BaseAddress = new Uri("https://apis.datos.gob.ar/");
            });
            services.AddHttpClient<IConsultaDepartamentos, ObtenerDepartamentosArg>(client =>
            {
                // Aquí es donde va la base del Curl. Tiene que terminar siempre con una barra diagonal '/'
                client.BaseAddress = new Uri("https://apis.datos.gob.ar/");
            });
            services.AddHttpClient<IConsultarLocalidades, ObtenerLocalidadesArg>(client =>
            {
                // Aquí es donde va la base del Curl. Tiene que terminar siempre con una barra diagonal '/'
                client.BaseAddress = new Uri("https://apis.datos.gob.ar/");
            });


            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            string dbType = configuration["Configurations:UseDatabase"] ?? throw new NullReferenceException(InfrastructureConstants.DATABASE_TYPE_NOT_CONFIGURED);

            services.CreateDataBase(dbType, configuration);

            return services;
        }
        // 🔽 AGREGAR ESTO - Método para seedear roles
        public static void SeedIdentityRoles(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                IdentityDataInitializer.SeedRolesAsync(roleManager).GetAwaiter().GetResult();
            }
        }
        public static void SeedCalendarioMuseo(this IApplicationBuilder app)
        {
            CalendarioMuseoSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        // 🔼
        /// <summary>
        /// Seedea la configuración inicial de visitas grupales guiadas al iniciar la aplicación
        /// </summary>
        public static void SeedConfiguracionVisitas(this IApplicationBuilder app)
        {
            ConfiguracionVisitasSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedConfiguracionHorarioAutoguiadas(this IApplicationBuilder app)
        {
            ConfiguracionHorarioAutoguiadasSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedProvinciasArgentina(this IApplicationBuilder app)
        {
            Data.Seeders.Ubicacion.ProvinciaSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedDepartamentosArgentina(this IApplicationBuilder app)
        {
            Data.Seeders.Ubicacion.SeederDepartamentos
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();

        }
        public static void SeedLocalidadesArgentina(this IApplicationBuilder app)
        {
            Data.Seeders.Ubicacion.SeederLocalidades
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedSalasMuseo(this IApplicationBuilder app)
        {
            SalaSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedTematicasVisitas(this IApplicationBuilder app)
        {
            TematicaVisitaSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedGuiasMuseo(this IApplicationBuilder app)
        {
            SeederGuias
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }

        public static void SeedConfiguracionUsoSalas(this IApplicationBuilder app)
        {
            ConfiguracionUsoSalas
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedRecursosMuseo(this IApplicationBuilder app)
        {
            RecursoSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedProyectoEducativo(this IApplicationBuilder app)
        {

            ProyectoAreaEducacionSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();

        }
        public static void SeedRolesPermissions(this IApplicationBuilder app)
        {
            RolePermissionsSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedAreasMuseo(this IApplicationBuilder app)
        {
            AreasSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedPuestosMuseo(this IApplicationBuilder app)
        {
            PuestoSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedAreaPuestoMuseo(this IApplicationBuilder app)
        {
            AreaPuestoSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
    }
}