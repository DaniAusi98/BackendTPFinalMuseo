using Domain.ActividadesAreaEducacion.Entities;
using Domain.ActividadMuseo.Entities;
using Domain.Common.Entities;
using Domain.Common.Entities.Ubicacion;
using Domain.Eventos.Entities;
using Domain.RecursoMuseo.Entities;
using Domain.RecursoMuseo.Entities.Guia;
using Domain.Reportes.Entities;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Infrastructure.Configurations;
using Infrastructure.Configurations.Reportes;
using Infrastructure.Configurations.UbicacionMundial;
using Infrastructure.Configurations.VisitasGrupales;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql
{
    /// <summary>
    /// Contexto de almacenamiento en base de datos. Aca se definen los nombres de 
    /// las tablas, y los mapeos entre los objetos
    /// </summary>
    public class MuseoDbContext
    : IdentityDbContext<UsuarioSistema, IdentityRole, string>
    {
        public MuseoDbContext(DbContextOptions<MuseoDbContext> options)
            : base(options)
        {
        }

        public DbSet<VisitaGrupalGuiada> VisitaGuiada { get; set; }
        public DbSet<VisitaGrupalAutoguiada> VisitaGrupalAutoguiada { get; set; }
        public DbSet<ReporteGeneralVisitasGrupales> ReporteGeneralVisitasGrupales { get; set; }
        public DbSet<Domain.ActividadMuseo.Entities.ActividadMuseo> ActividadAgendaMuseo { get; set; }
        public DbSet<RecursoAsignado> RecursoAsignado { get; set; }
        public DbSet<Recurso> RecursoMuseo { get; set; }
        public DbSet<Sala> SalaMuseo { get; set; }
        public DbSet<ConfiguracionSalaActividad> ConfiguracionesSalaActividad { get; set; }
        public DbSet<Guia> GuiaMuseo { get; set; }
        public DbSet<AusenciaGuia> AusenciasGuia { get; set; }
        public DbSet<DiaCierreMuseo> DiaCierreMuseo { get; set; }
        public DbSet<HorarioGuia> HorarioGuia { get; set; }
        public DbSet<TematicaVisita> TematicaVisita { get; set; }
        public DbSet<ConfiguracionVisitasGrupalesGuiadas> ConfiguracionVisitasGrupalesGuiadas { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<LocalidadArg> Localidades { get; set; }
        public DbSet<CalendarioMuseo> CalendarioMuseo { get; set; }
        public DbSet<ConfiguracionHorarioAutoguiada> ConfiguracionHorarioVisitaAutoguiada { get; set; }
        public DbSet<ReporteVisitasGuiadas> ReporteVisitasGuiadas { get; set; }
        public DbSet<ReporteVisitasAutoguiadas> ReporteVisitasAutoguiadas { get; set; }
        public DbSet<ReporteGeneralVisitasGrupales> ReporteGeneralVisitas { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<DivisionAdministrativa> DivisionesAdministrativas { get; set; }
        public DbSet<Localidad> Localidad { get; set; }
        public DbSet<Evento> Evento { get; set; }
        public DbSet<ActividadException> ActividadException { get; set; }

        public DbSet<Domain.ActividadesAreaEducacion.Entities.ActividadEducativa> ActividadEducativas { get; set; }
        public DbSet<ProyectosAreaEducacion> ProyectosAreaEducacion { get; set; }
        public DbSet<EquipoTrabajoResponsable> EquipoTrabajoResponsables { get; set; }



        protected MuseoDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DummyEntity>().ToTable("DummyEntity");
            modelBuilder.ApplyConfiguration(new VisitaGrupalGuiadaConfiguration());
            modelBuilder.ApplyConfiguration(new VisitaGrupalAutoguiadaConfiguration());
            modelBuilder.ApplyConfiguration(new ActividadMuseoConfiguration());
            modelBuilder.ApplyConfiguration(new RecursoConfiguration());
            modelBuilder.ApplyConfiguration(new SalaConfiguration());
            modelBuilder.ApplyConfiguration(new RecursoAsignadoConfiguration());
            modelBuilder.ApplyConfiguration(new GuiaConfiguration());
            modelBuilder.ApplyConfiguration(new HorarioGuiaConfiguration());
            modelBuilder.ApplyConfiguration(new AusenciaGuiaConfiguration());
            modelBuilder.ApplyConfiguration(new DiaCierreMuseoConfiguration());
            modelBuilder.ApplyConfiguration(new BloqueoSalaConfiguration());
            modelBuilder.ApplyConfiguration(new TematicaVisitaConfiguration());
            modelBuilder.ApplyConfiguration(new ProvinciaConfiguration());
            modelBuilder.ApplyConfiguration(new DepartamentoConfiguration());
            modelBuilder.ApplyConfiguration(new LocalidadConfiguration());
            modelBuilder.ApplyConfiguration(new ConfiguracionVisitasGrupalesGuiadaConfig());
            modelBuilder.ApplyConfiguration(new ConfiguracionCalendarioMuseo());
            modelBuilder.ApplyConfiguration(new ConfiguracionHorarioVisitaGrupalAutoguiadaConfiguration());
            modelBuilder.ApplyConfiguration(new ReporteVisitaGrupalConfig());
            modelBuilder.ApplyConfiguration(new ReporteVisitaGuiadaConfig());
            modelBuilder.ApplyConfiguration(new ReporteVisitaAutoguiadaConfig());
            modelBuilder.ApplyConfiguration(new PaisConfiguration());
            modelBuilder.ApplyConfiguration(new DivisionAdministrativaConfiguration());
            modelBuilder.ApplyConfiguration(new LocalidadMundialConfiguration());
            modelBuilder.ApplyConfiguration(new EventoConfiguration());
            modelBuilder.ApplyConfiguration(new ConfiguracionSalaActividadConfig());
            modelBuilder.ApplyConfiguration(new ActividadExceptionConfiguration());
            modelBuilder.ApplyConfiguration(new ActividadEducativaConfig());
            modelBuilder.ApplyConfiguration(new ProyectosAreaEducacionConfig());
            modelBuilder.ApplyConfiguration(new EquipoTrabajoResponsableConfig());

            // ... Aquí tienes tus configuraciones actuales de tablas (Entidades, Claves, etc.) ...

            //RUCO: Le indicamos a Entity Framework que ignore por completo 
            // cualquier tabla que empiece con el prefijo "Hangfire_"
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().ToList())
            {
                if (entityType.GetTableName() != null && entityType.GetTableName().StartsWith("Hangfire_"))
                {
                    modelBuilder.Ignore(entityType.ClrType);
                }
            }

        }
    }
}

/*public DbSet<PersonalInterno> PersonalInterno { get; set; }

        public DbSet<Area> Area { get; set; }

        public DbSet<AreaPuesto> AreaPuesto { get; set; }

        public DbSet<Puesto> Puesto { get; set; }*/