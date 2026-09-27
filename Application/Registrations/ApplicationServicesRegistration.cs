using Application.ApplicationMuseo.ApplicationServices;
using Application.ApplicationMuseo.Integrations.Events;
using Application.ApplicationMuseo.Integrations.Handlers.Publishers;
using Application.ApplicationMuseo.Integrations.Handlers.Subscribers;
using Application.Availability;
using Application.Availability.Factories;
using Application.Availability.Producers;
using Application.Behaivors;
using Application.Eventos.ApplicationServices;
using Application.Eventos.Options;
using Application.MuseumResources.ApplicationServices;
using Application.Reportes.ApplicationServices;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.UseCases.Commands.UpdateUsuario;
using Application.VisitaGrupal.ApplicationServices;
using Core.Application;
using Domain.VisitasGrupales.DomainServices;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.Entities.GrupalGuiada.ReglasDisponibilidad;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Registrations
{
    /// <summary>
    /// Aqui se deben registrar todas las dependencias de la capa de aplicacion
    /// </summary>
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            /* Configuraciones de opciones */
            services.Configure<SalasParaEventosOptions>(
                configuration.GetSection(SalasParaEventosOptions.SectionName));

            /* Automapper */
            services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = configuration["LuckyPennySoftware:LicenseKey"];
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            services.AddMediatR(cfg =>
            {
                cfg.LicenseKey = configuration["LuckyPennySoftware:LicenseKey"];
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddScoped<ICommandQueryBus, MediatrCommandQueryBus>();
            services.AddScoped<IConfirmEmailService, ConfirmEmailServiceResponse>();
            /* EventBus */
            services.AddPublishers();
            services.AddSubscribers();

            /* MediatR*/


            /* Application Services */
            services.AddScoped<IDummyEntityApplicationService, DummyEntityApplicationService>();
            services.AddScoped<IEventoApplicationService, EventoApplicationService>();

            //services.AddScoped<IUsuarioApplicationService,UsuarioVisitanteApplicationService>();

            // Disponibilidad de visitas guiadas
            services.AddScoped<MotorDisponibilidadVisitasGuiadas>();
            services.Scan(scan => scan
                .FromAssemblyOf<MotorDisponibilidadVisitasGuiadas>()
                .AddClasses(classes =>
                    classes.AssignableTo<IReglaDisponibilidadVisitaGuiada>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());


            // Register availability service (now uses ConfiguracionVisitasGrupalesGuiadas entity from database)
            services.AddScoped<IServicioDisponibilidadTurnosVisitasGuiadas, ServicioDisponibilidadTurnosVisitasGuiadas>();
            services.AddScoped<IServicioDisponibilidadSlotsAutoguiadas, ServicioDisponibilidadSlotsAutoguiadas>();

            // Register availability engine, rule factory and providers (moved from Application.Availability.ServiceCollectionExtensions)
            services.AddScoped<AvailabilityEngine>();
            services.AddScoped<IRuleFactory, CompositeRuleFactory>();

            // Register providers - these will be created by DI and can create rule instances
            services.Scan(scan => scan
                .FromAssemblyOf<AvailabilityEngine>()
                .AddClasses(classes => classes.InNamespaces("Application.Availability.Providers"))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // Register availability rules automatically by scanning
            services.Scan(scan => scan
                .FromAssemblyOf<AvailabilityEngine>()
                .AddClasses(classes => classes.InNamespaces("Application.Availability.Rules"))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            // Register guided availability producer service
            services.AddScoped<GuidedAvailabilityProducerService>();
            services.AddScoped<SelfGuidedAvailabilityProducerService>();
            services.AddTransient<ActivityAvailabilityFactory>();
            // Register event availability producer service
            services.AddScoped<EventAvailabilityProducerService>();
            services.AddScoped<DispActEducativaProducerService>();



            services.AddScoped<IEventRecurrenceAvailabilityService, EventRecurrenceAvailabilityService>();

            // Register group availability producer service
            // services.AddScoped<Application.Availability.Producers.GroupAvailabilityProducerService>();
            // Register in-memory recurrence repo for testing (optional)
            // services.AddSingleton<Application.Availability.Recurrence.IRecurrenceRuleRepository, Application.Availability.Recurrence.InMemoryRecurrenceRuleRepository>();

            services.AddValidatorsFromAssemblyContaining<CrearVisitaGuiadaCommandValidator>();

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));


            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<IVisitaGuiadaSinConfirmacion, VisitaGuiadaSinConfirmar>();
            services.AddScoped<IServicioReporteVisitasGrupales,ServicioReporteVisitasGrupales>();
            services.AddScoped<IServicioReporteVisitasGuiadas, ServicioReporteVisitasGuiadas>();
            services.AddScoped<IServicioReporteVisitasAutoguiadas, ServicioReporteVisitasAutoguiadas>();

            services.AddScoped<IDisponibilidadRecursosService, DisponibilidadRecursosService>();


            return services;
        }

        private static IServiceCollection AddPublishers(this IServiceCollection services)
        {
            //Aqui se registran los handlers que publican en el bus de eventos
            services.AddTransient<IIntegrationEventHandler<DummyEntityCreatedIntegrationEvent>, DummyEntityCreatedIntegrationEventHandlerPub>();
            return services;
        }

        private static IServiceCollection AddSubscribers(this IServiceCollection services)
        {
            //Aqui se registran los handlers que se suscriben al bus de eventos
            services.AddTransient<DummyEntityCreatedIntegrationEventHandlerSub>();
            return services;
        }
    }
}
