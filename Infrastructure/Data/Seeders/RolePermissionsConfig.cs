using Infrastructure.Identity;

namespace Infrastructure.Data.Seeders
{
    public static class RolePermissionsConfig
    {
        public static Dictionary<string, List<string>> RolePermissions { get; } = new()
        {
            {
                "Admin", new List<string>
                {
                    // USUARIOS
                    Permissions.Users.Create,
                    Permissions.Users.View,
                    Permissions.Users.Edit,
                    Permissions.Users.Delete,

                    // ACTIVIDADES EDUCATIVAS
                    Permissions.EducationalActivities.View,

                    // MUESTRAS TEMPORALES
                    Permissions.TemporaryExhibitions.View,

                    // EVENTOS
                    Permissions.Events.View,

                    // PROPUESTAS
                    Permissions.Proposals.View,

                    // VISITAS GUIADAS
                    Permissions.GuidedVisits.View,

                    // VISITAS AUTOGUIADAS
                    Permissions.SelfGuidedVisits.View,

                    // CONFIGURACIÓN DE VISITAS GUIADAS
                    Permissions.GuidedVisitsConfiguration.View,
                    Permissions.GuidedVisitsConfiguration.Edit,

                    // CONFIGURACIÓN DE VISITAS AUTOGUIADAS
                    Permissions.SelfGuidedVisitsConfiguration.View,
                    Permissions.SelfGuidedVisitsConfiguration.Edit,

                    // HORARIOS DEL MUSEO
                    Permissions.MuseumSchedules.Edit,
                    Permissions.MuseumSchedules.View,

                    // SALAS
                    Permissions.Rooms.Edit,
                    Permissions.Rooms.View,
                    Permissions.Rooms.Create,
                    Permissions.Rooms.Delete,

                    // RECURSOS
                    Permissions.Resources.Create,
                    Permissions.Resources.View,
                    Permissions.Resources.Delete,
                    Permissions.Resources.Edit,

                    // DÍAS DE CIERRE
                    Permissions.MuseumClosures.Edit,
                    Permissions.MuseumClosures.View,
                    Permissions.MuseumClosures.Delete,
                    Permissions.MuseumClosures.Create,

                    // REPORTES
                    Permissions.Reports.View,
                    Permissions.Reports.Export,

                    // CONFIGURACIÓN DE SALAS PARA EVENTOS
                    Permissions.EventRoomConfiguration.View,
                    Permissions.EventRoomConfiguration.Edit,
                }
            },

            {
                "Recepcion", new List<string>
                {
                    // ACTIVIDADES EDUCATIVAS
                    Permissions.EducationalActivities.View,

                    // MUESTRAS TEMPORALES
                    Permissions.TemporaryExhibitions.View,
                    Permissions.TemporaryExhibitions.Create,
                    Permissions.TemporaryExhibitions.Edit,
                    Permissions.TemporaryExhibitions.Cancel,
                    Permissions.TemporaryExhibitions.Reschedule,

                    // EVENTOS
                    Permissions.Events.View,
                    Permissions.Events.Create,
                    Permissions.Events.Edit,
                    Permissions.Events.Cancel,
                    Permissions.Events.Reschedule,

                    // PROPUESTAS
                    Permissions.Proposals.View,
                    Permissions.Proposals.ResendForm,

                    // VISITAS GUIADAS
                    Permissions.GuidedVisits.View,
                    Permissions.GuidedVisits.Create,
                    Permissions.GuidedVisits.Edit,
                    Permissions.GuidedVisits.Cancel,
                    Permissions.GuidedVisits.Reschedule,

                    // VISITAS AUTOGUIADAS
                    Permissions.SelfGuidedVisits.View,
                    Permissions.SelfGuidedVisits.Create,
                    Permissions.SelfGuidedVisits.Edit,
                    Permissions.SelfGuidedVisits.Cancel,
                    Permissions.SelfGuidedVisits.Reschedule,

                    // CONFIGURACIÓN DE VISITAS GUIADAS
                    Permissions.GuidedVisitsConfiguration.View,
                    Permissions.GuidedVisitsConfiguration.Edit,

                    // CONFIGURACIÓN DE VISITAS AUTOGUIADAS
                    Permissions.SelfGuidedVisitsConfiguration.View,
                    Permissions.SelfGuidedVisitsConfiguration.Edit,

                    // GUÍAS
                    Permissions.Guides.View,

                    // AUSENCIAS DE GUÍAS
                    Permissions.GuideAbsences.ViewOwn,
                    Permissions.GuideAbsences.ViewAll,

                    // HORARIOS DEL MUSEO
                    Permissions.MuseumSchedules.View,
                    Permissions.MuseumSchedules.Edit,

                    // SALAS
                    Permissions.Rooms.View,
                    Permissions.Rooms.Create,
                    Permissions.Rooms.Edit,
                    Permissions.Rooms.Delete,

                    // RECURSOS
                    Permissions.Resources.View,
                    Permissions.Resources.Create,
                    Permissions.Resources.Edit,
                    Permissions.Resources.Delete,

                    // BLOQUEOS DE VISITAS GUIADAS
                    Permissions.GuidedVisitBlocks.View,
                    Permissions.GuidedVisitBlocks.Create,
                    Permissions.GuidedVisitBlocks.Edit,
                    Permissions.GuidedVisitBlocks.Delete,

                    // BLOQUEOS DE VISITAS AUTOGUIADAS
                    Permissions.SelfGuidedVisitBlocks.View,
                    Permissions.SelfGuidedVisitBlocks.Create,
                    Permissions.SelfGuidedVisitBlocks.Edit,
                    Permissions.SelfGuidedVisitBlocks.Delete,

                    // BLOQUEOS DE EVENTOS
                    Permissions.EventBlocks.View,
                    Permissions.EventBlocks.Create,
                    Permissions.EventBlocks.Edit,
                    Permissions.EventBlocks.Delete,

                    // BLOQUEOS DE MUESTRAS
                    Permissions.ExhibitionBlocks.View,
                    Permissions.ExhibitionBlocks.Create,
                    Permissions.ExhibitionBlocks.Edit,
                    Permissions.ExhibitionBlocks.Delete,

                    // DÍAS DE CIERRE
                    Permissions.MuseumClosures.View,
                    Permissions.MuseumClosures.Create,
                    Permissions.MuseumClosures.Edit,
                    Permissions.MuseumClosures.Delete,

                    // REPORTES
                    Permissions.Reports.View,
                    Permissions.Reports.Export,

                    // CONFIGURACIÓN DE SALAS PARA EVENTOS
                    Permissions.EventRoomConfiguration.View,
                    Permissions.EventRoomConfiguration.Edit,
                }
            },

            {
                "Director", new List<string>
                {
                    // ACTIVIDADES EDUCATIVAS
                    Permissions.EducationalActivities.View,

                    // MUESTRAS TEMPORALES
                    Permissions.TemporaryExhibitions.View,

                    // EVENTOS
                    Permissions.Events.View,

                    // PROPUESTAS
                    Permissions.Proposals.View,
                    Permissions.Proposals.Create,
                    Permissions.Proposals.Edit,
                    Permissions.Proposals.Delete,
                    Permissions.Proposals.Approve,
                    Permissions.Proposals.Reject,
                    Permissions.Proposals.ResendForm,

                    // VISITAS GUIADAS
                    Permissions.GuidedVisits.View,

                    // VISITAS AUTOGUIADAS
                    Permissions.SelfGuidedVisits.View,

                    // CONFIGURACIÓN DE VISITAS GUIADAS
                    Permissions.GuidedVisitsConfiguration.View,
                    Permissions.GuidedVisitsConfiguration.Edit,

                    // CONFIGURACIÓN DE VISITAS AUTOGUIADAS
                    Permissions.SelfGuidedVisitsConfiguration.View,
                    Permissions.SelfGuidedVisitsConfiguration.Edit,

                    // GUÍAS
                    Permissions.Guides.View,

                    // AUSENCIAS DE GUÍAS
                    Permissions.GuideAbsences.ViewOwn,
                    Permissions.GuideAbsences.ViewAll,

                    // HORARIOS DEL MUSEO
                    Permissions.MuseumSchedules.View,
                    Permissions.MuseumSchedules.Edit,

                    // SALAS
                    Permissions.Rooms.View,
                    Permissions.Rooms.Create,
                    Permissions.Rooms.Edit,
                    Permissions.Rooms.Delete,

                    // RECURSOS
                    Permissions.Resources.View,
                    Permissions.Resources.Create,
                    Permissions.Resources.Edit,
                    Permissions.Resources.Delete,

                    // PROYECTOS EDUCATIVOS
                    Permissions.EducationalProjects.View,
                    Permissions.EducationalProjects.Create,

                    // BLOQUEOS DE VISITAS GUIADAS
                    Permissions.GuidedVisitBlocks.View,
                    Permissions.GuidedVisitBlocks.Create,
                    Permissions.GuidedVisitBlocks.Edit,
                    Permissions.GuidedVisitBlocks.Delete,

                    // BLOQUEOS DE VISITAS AUTOGUIADAS
                    Permissions.SelfGuidedVisitBlocks.View,
                    Permissions.SelfGuidedVisitBlocks.Create,
                    Permissions.SelfGuidedVisitBlocks.Edit,
                    Permissions.SelfGuidedVisitBlocks.Delete,

                    // BLOQUEOS DE EVENTOS
                    Permissions.EventBlocks.View,
                    Permissions.EventBlocks.Create,
                    Permissions.EventBlocks.Edit,
                    Permissions.EventBlocks.Delete,

                    // BLOQUEOS DE MUESTRAS
                    Permissions.ExhibitionBlocks.View,
                    Permissions.ExhibitionBlocks.Create,
                    Permissions.ExhibitionBlocks.Edit,
                    Permissions.ExhibitionBlocks.Delete,

                    // BLOQUEOS DE ACTIVIDADES EDUCATIVAS
                    Permissions.EducationalActivityBlocks.View,
                    Permissions.EducationalActivityBlocks.Create,
                    Permissions.EducationalActivityBlocks.Edit,
                    Permissions.EducationalActivityBlocks.Delete,

                    // DÍAS DE CIERRE
                    Permissions.MuseumClosures.View,
                    Permissions.MuseumClosures.Create,
                    Permissions.MuseumClosures.Edit,
                    Permissions.MuseumClosures.Delete,

                    // REPORTES
                    Permissions.Reports.View,
                    Permissions.Reports.Export,

                    // CONFIGURACIÓN DE SALAS PARA EVENTOS
                    Permissions.EventRoomConfiguration.View,
                    Permissions.EventRoomConfiguration.Edit,
                }
            },

            {
                "CoordinadoraEducacion", new List<string>
                {
                    // ACTIVIDADES EDUCATIVAS
                    Permissions.EducationalActivities.View,
                    Permissions.EducationalActivities.Create,
                    Permissions.EducationalActivities.Edit,
                    Permissions.EducationalActivities.Cancel,
                    Permissions.EducationalActivities.Reschedule,

                    // MUESTRAS TEMPORALES
                    Permissions.TemporaryExhibitions.View,

                    // EVENTOS
                    Permissions.Events.View,

                    // VISITAS GUIADAS
                    Permissions.GuidedVisits.View,
                    Permissions.GuidedVisits.Create,
                    Permissions.GuidedVisits.Edit,
                    Permissions.GuidedVisits.Cancel,
                    Permissions.GuidedVisits.Reschedule,

                    // VISITAS AUTOGUIADAS
                    Permissions.SelfGuidedVisits.View,
                    Permissions.SelfGuidedVisits.Create,
                    Permissions.SelfGuidedVisits.Edit,
                    Permissions.SelfGuidedVisits.Cancel,
                    Permissions.SelfGuidedVisits.Reschedule,

                    // CONFIGURACIÓN DE VISITAS GUIADAS
                    Permissions.GuidedVisitsConfiguration.View,
                    Permissions.GuidedVisitsConfiguration.Edit,

                    // GUÍAS
                    Permissions.Guides.View,
                    Permissions.Guides.Create,
                    Permissions.Guides.Edit,
                    Permissions.Guides.Delete,

                    // AUSENCIAS DE GUÍAS
                    Permissions.GuideAbsences.ViewOwn,
                    Permissions.GuideAbsences.ViewAll,
                    Permissions.GuideAbsences.CreateOwn,
                    Permissions.GuideAbsences.CreateAll,
                    Permissions.GuideAbsences.EditAll,
                    Permissions.GuideAbsences.DeleteAll,

                    // PROYECTOS EDUCATIVOS
                    Permissions.EducationalProjects.View,
                    Permissions.EducationalProjects.Create,
                    Permissions.EducationalProjects.Edit,
                    Permissions.EducationalProjects.Delete,

                    // TEMÁTICAS
                    Permissions.VisitTopics.View,
                    Permissions.VisitTopics.Create,
                    Permissions.VisitTopics.Edit,
                    Permissions.VisitTopics.Delete,

                    // BLOQUEOS DE VISITAS GUIADAS
                    Permissions.GuidedVisitBlocks.View,
                    Permissions.GuidedVisitBlocks.Create,
                    Permissions.GuidedVisitBlocks.Edit,
                    Permissions.GuidedVisitBlocks.Delete,

                    // BLOQUEOS DE ACTIVIDADES EDUCATIVAS
                    Permissions.EducationalActivityBlocks.View,
                    Permissions.EducationalActivityBlocks.Create,
                    Permissions.EducationalActivityBlocks.Edit,
                    Permissions.EducationalActivityBlocks.Delete,

                    // REPORTES
                    Permissions.Reports.View,
                    Permissions.Reports.Export,
                }
            },

            {
                "EducadoraGuia", new List<string>
                {
                    // ACTIVIDADES EDUCATIVAS
                    Permissions.EducationalActivities.View,
                    Permissions.EducationalActivities.Create,
                    Permissions.EducationalActivities.Edit,
                    Permissions.EducationalActivities.Cancel,
                    Permissions.EducationalActivities.Reschedule,

                    // MUESTRAS TEMPORALES
                    Permissions.TemporaryExhibitions.View,

                    // EVENTOS
                    Permissions.Events.View,

                    // VISITAS GUIADAS
                    Permissions.GuidedVisits.View,

                    // VISITAS AUTOGUIADAS
                    Permissions.SelfGuidedVisits.View,

                    // CONFIGURACIÓN DE VISITAS GUIADAS
                    Permissions.GuidedVisitsConfiguration.View,

                    // GUÍAS
                    Permissions.Guides.View,

                    // AUSENCIAS DE GUÍAS
                    Permissions.GuideAbsences.ViewOwn,
                    Permissions.GuideAbsences.ViewAll,
                    Permissions.GuideAbsences.CreateOwn,
                    Permissions.GuideAbsences.EditOwn,
                    Permissions.GuideAbsences.DeleteOwn,

                    // PROYECTOS EDUCATIVOS
                    Permissions.EducationalProjects.View,

                    // DÍAS DE CIERRE
                    Permissions.MuseumClosures.View,

                    // REPORTES
                    Permissions.Reports.View,
                    Permissions.Reports.Export,
                }
            },

            {
                "Usuario", new List<string>
                {
                    // ACTIVIDADES EDUCATIVAS
                    Permissions.EducationalActivities.View,

                    // MUESTRAS TEMPORALES
                    Permissions.TemporaryExhibitions.View,

                    // EVENTOS
                    Permissions.Events.View,

                    // VISITAS GUIADAS
                    Permissions.GuidedVisits.View,

                    // VISITAS AUTOGUIADAS
                    Permissions.SelfGuidedVisits.View,

                    // GUÍAS
                    Permissions.Guides.View,

                    // AUSENCIAS DE GUÍAS
                    Permissions.GuideAbsences.ViewOwn,
                    Permissions.GuideAbsences.ViewAll,

                    // DÍAS DE CIERRE
                    Permissions.MuseumClosures.View,

                    // REPORTES
                    Permissions.Reports.View,
                    Permissions.Reports.Export,
                }
            },

            {
                "Visitante", new List<string>
                {
                    // VISITAS GUIADAS: SOLAMENTE SUS PROPIAS RESERVAS
                    Permissions.GuidedVisits.ViewOwn,
                    Permissions.GuidedVisits.CreateOwn,
                    Permissions.GuidedVisits.EditOwn,
                    Permissions.GuidedVisits.CancelOwn,
                    Permissions.GuidedVisits.RescheduleOwn,

                    // VISITAS AUTOGUIADAS: SOLAMENTE SUS PROPIAS RESERVAS
                    Permissions.SelfGuidedVisits.ViewOwn,
                    Permissions.SelfGuidedVisits.CreateOwn,
                    Permissions.SelfGuidedVisits.EditOwn,
                    Permissions.SelfGuidedVisits.CancelOwn,
                    Permissions.SelfGuidedVisits.RescheduleOwn,
                }
            }
        };
    }
}