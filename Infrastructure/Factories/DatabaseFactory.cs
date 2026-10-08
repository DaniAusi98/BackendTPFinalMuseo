using Application.ActividadesEducativas.Repositories;
using Application.ActividadMuseo.Repositories;
using Application.Common.Repositories;
using Application.Eventos.Repositories;
using Application.MuseumResources.Repositories;
using Application.PersonalMuseo.Repositories;
using Application.Repositories;
using Application.VisitaGrupal.Repositories;
using Domain.Common.Others.Utils;
using Infrastructure.Constants;
using Infrastructure.Repositories.Sql.ActividadEducativa;
using Infrastructure.Repositories.Sql.ActividadMuseo;
using Infrastructure.Repositories.Sql.Eventos;
using Infrastructure.Repositories.Sql.PersonalMuseo;
using Infrastructure.Repositories.Sql.RecursosMuseo;
using Infrastructure.Repositories.Sql.Ubicacion;
using Infrastructure.Repositories.Sql.VisitaGrupal;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using static Domain.Common.Enums.Enums;

namespace Infrastructure.Factories
{
    internal static class DatabaseFactory
    {
        public static void CreateDataBase(this IServiceCollection services, string dbType, IConfiguration configuration)
        {
            switch (dbType.ToEnum<DatabaseType>())
            {
                case DatabaseType.MYSQL:
                    services.AddMySqlRepositories(configuration);
                    break;
                case DatabaseType.MARIADB:
                case DatabaseType.SQLSERVER:
                    services.AddSqlServerRepositories(configuration);
                    break;
                //case DatabaseType.MONGODB:
                // services.AddMongoDbRepositories(configuration);
                //break;
                default:
                    throw new NotSupportedException(InfrastructureConstants.DATABASE_TYPE_NOT_SUPPORTED);
            }
        }

        private static IServiceCollection AddSqlServerRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Repositories.Sql.MuseoDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            }, ServiceLifetime.Scoped);

            //Habilitar para trabajar con Migrations
            var context = services.BuildServiceProvider().GetRequiredService<Repositories.Sql.MuseoDbContext>();
            context.Database.Migrate();

            /* Sql Repositories */
            services.AddTransient<IDummyEntityRepository, Repositories.Sql.DummyEntityRepository>();

            //services.AddTransient<IRepositorioUsuarioVisitante,RepositorioUsuario>();



            return services;
        }

        /*rivate static IServiceCollection AddMongoDbRepositories(this IServiceCollection services, IConfiguration configuration)
         {
             ConventionRegistry.Register("Camel Case", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);

             Repositories.Mongo.StoreDbContext db = new(configuration.GetConnectionString("MongoConnection") ?? throw new NullReferenceException());
             services.AddSingleton(typeof(Repositories.Mongo.StoreDbContext), db);


             services.AddTransient<IDummyEntityRepository, Repositories.Mongo.DummyEntityRepository>();

             return services;
         }*/

        private static IServiceCollection AddMySqlRepositories(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<Repositories.Sql.MuseoDbContext>(options =>
            {
                options.UseMySql(
                    configuration.GetConnectionString("MySqlConnection"),
                    ServerVersion.AutoDetect(
                        configuration.GetConnectionString("MySqlConnection")
                    )
                );
            });

            /* Repositories */
            services.AddTransient<IDummyEntityRepository, Repositories.Sql.DummyEntityRepository>();
            // services.AddTransient<IRepositorioUsuarioVisitante, RepositorioUsuario>();
            services.AddTransient<IRepositorioVisitaGuiada, RepositorioVisitaGuiada>();
            services.AddTransient<IRepositorioTematicas, RepositorioTematicasVisita>();
            // También registrar la interfaz usada por la versión 'MuseumResources' para evitar mismatch de namespaces
            services.AddTransient<IRepositorioGuia, RepositorioGuia>();
            services.AddTransient<IRepositorioConfiguracionVisitasGrupalesGuiadas, RepositorioConfiguracionVisitasGrupalesGuiadas>();
            services.AddTransient<IRepositorioDiaCierreMuseo, Repositories.Sql.DisponibilidadActividades.RepositorioDiaCierreMuseo>();
            services.AddTransient<IRepositorioActividadMuseo, Repositories.Sql.DisponibilidadActividades.RepositorioActividadMuseo>();
            services.AddTransient<IRepositorioCalendarioMuseo, RepositorioCalendarioMuseo>();
            services.AddTransient<IRepositorioVisitaGrupalAutoguiada, RepositorioVisitaGrupalAutoguiada>();
            services.AddTransient<IRepositorioSala, RepositorioSalaMuseo>();
            services.AddTransient<IRepositorioConfiguracionSalaActividad, RepositorioConfiguracionSalaActividad>();
            services.AddTransient<IRepositorioRecurso, RepositorioRecursoMuseo>();
            services.AddTransient<IDepartamentoRepository, DepartamentoRepository>();
            services.AddTransient<ILocalidadRepository, LocalidadRepository>();
            services.AddTransient<IRepositorioConfiguracionHorarioAutoguiada, RepositorioConfiguracionHorarioAutoguiada>();
            services.AddTransient<IRepositorioEvento, RepositorioEvento>();
            services.AddTransient<IProvinciaRepository, ProvinciaRepository>();
            services.AddTransient<IRepositorioProyectoAreaEducacion, RepositorioProyectoEducativo>();
            services.AddTransient<IRepositorioActividadEducativa, RepositorioActividadEducativa>();
            services.AddTransient<IRepositorioAusenciaGuia, RepositorioAusenciaGuia>();
            services.AddTransient<IRepositorioSala, RepositorioSalaMuseo>();
            services.AddTransient<IRepositorioArea, RepositorioArea>();
            services.AddTransient<IRepositorioAreaPuesto, RepositorioAreaPuesto>();
            services.AddTransient<IRepositorioPuesto, RepositorioPuesto>();
            services.AddTransient<IRepositorioPersonal, RepositorioPersonal>();
            services.AddTransient<IRepositorioAsignacionPuestoPersonal, RepositorioAsignacionPuestoPersonal>();






            //  Migraciones automáticas al levantar la app (infra pura)
            var context = services.BuildServiceProvider()
               .GetRequiredService<Repositories.Sql.MuseoDbContext>();

            context.Database.Migrate();


            return services;
        }

    }
}




/* services.AddTransient<IRepositorioVisitaGuiada, Repositories.Sql.VisitaGrupal.RepositorioVisitaGuiada>();
            services.AddTransient<IRepositorioTematicas, Repositories.Sql.VisitaGrupal.RepositorioTematicasVisita>();
            services.AddTransient<IRepositorioGuia,Repositories.Sql.VisitaGrupal.RepositorioGuia>();
            services.AddTransient<IRepositorioDiaCierreMuseo, Repositories.Sql.DisponibilidadActividades.RepositorioDiaCierreMuseo>();*/
