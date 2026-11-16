
namespace Infrastructure.Identity
{
    public static class Permissions
    {
        public const string ClaimType = "permission";

        // ACTIVIDADES EDUCATIVAS
        public static class EducationalActivities
        {
            public const string View = "EducationalActivities.View";
            public const string Create = "EducationalActivities.Create";
            public const string Edit = "EducationalActivities.Edit";
            public const string Cancel = "EducationalActivities.Cancel";
            public const string Reschedule = "EducationalActivities.Reschedule";
        }

        // MUESTRAS TEMPORALES
        public static class TemporaryExhibitions
        {
            public const string View = "TemporaryExhibitions.View";
            public const string Create = "TemporaryExhibitions.Create";
            public const string Edit = "TemporaryExhibitions.Edit";
            public const string Cancel = "TemporaryExhibitions.Cancel";
            public const string Reschedule = "TemporaryExhibitions.Reschedule";
        }

        // EVENTOS
        public static class Events
        {
            public const string View = "Events.View";
            public const string Create = "Events.Create";
            public const string Edit = "Events.Edit";
            public const string Cancel = "Events.Cancel";
            public const string Reschedule = "Events.Reschedule";
        }

        // PROPUESTAS DE EVENTOS Y MUESTRAS
        public static class Proposals
        {
            public const string View = "Proposals.View";
            public const string Create = "Proposals.Create";
            public const string Edit = "Proposals.Edit";
            public const string Delete = "Proposals.Delete";
            public const string Approve = "Proposals.Approve";
            public const string Reject = "Proposals.Reject";
            public const string ResendForm = "Proposals.ResendForm";
        }

        // VISITAS GUIADAS
        public static class GuidedVisits
        {
            public const string View = "GuidedVisits.View";
            public const string Create = "GuidedVisits.Create";
            public const string Edit = "GuidedVisits.Edit";
            public const string Cancel = "GuidedVisits.Cancel";
            public const string Reschedule = "GuidedVisits.Reschedule";

            public const string ViewOwn = "GuidedVisits.ViewOwn";
            public const string CreateOwn = "GuidedVisits.CreateOwn";
            public const string EditOwn = "GuidedVisits.EditOwn";
            public const string CancelOwn = "GuidedVisits.CancelOwn";
            public const string RescheduleOwn = "GuidedVisits.RescheduleOwn";

            public const string ViewAll = "GuidedVisits.ViewAll";
            public const string CreateAll = "GuidedVisits.CreateAll";
            public const string EditAll = "GuidedVisits.EditAll";
            public const string CancelAll = "GuidedVisits.CancelAll";
            public const string RescheduleAll = "GuidedVisits.RescheduleAll";
        }

        // VISITAS AUTOGUIADAS
        public static class SelfGuidedVisits
        {
            public const string View = "SelfGuidedVisits.View";
            public const string Create = "SelfGuidedVisits.Create";
            public const string Edit = "SelfGuidedVisits.Edit";
            public const string Cancel = "SelfGuidedVisits.Cancel";
            public const string Reschedule = "SelfGuidedVisits.Reschedule";

            public const string ViewOwn = "SelfGuidedVisits.ViewOwn";
            public const string CreateOwn = "SelfGuidedVisits.CreateOwn";
            public const string EditOwn = "SelfGuidedVisits.EditOwn";
            public const string CancelOwn = "SelfGuidedVisits.CancelOwn";
            public const string RescheduleOwn = "SelfGuidedVisits.RescheduleOwn";

            public const string ViewAll = "SelfGuidedVisits.ViewAll";
            public const string CreateAll = "SelfGuidedVisits.CreateAll";
            public const string EditAll = "SelfGuidedVisits.EditAll";
            public const string CancelAll = "SelfGuidedVisits.CancelAll";
            public const string RescheduleAll = "SelfGuidedVisits.RescheduleAll";
        }

        // CONFIGURACIÓN DE VISITAS GUIADAS
        public static class GuidedVisitsConfiguration
        {
            public const string View = "GuidedVisitsConfiguration.View";
            public const string Edit = "GuidedVisitsConfiguration.Edit";
        }

        // CONFIGURACIÓN DE VISITAS AUTOGUIADAS
        public static class SelfGuidedVisitsConfiguration
        {
            public const string View = "SelfGuidedVisitsConfiguration.View";
            public const string Edit = "SelfGuidedVisitsConfiguration.Edit";
        }

        // GUÍAS
        public static class Guides
        {
            public const string View = "Guides.View";
            public const string Create = "Guides.Create";
            public const string Edit = "Guides.Edit";
            public const string Delete = "Guides.Delete";
        }

        // AUSENCIAS DE GUÍAS
        public static class GuideAbsences
        {
            public const string ViewOwn = "GuideAbsences.ViewOwn";
            public const string ViewAll = "GuideAbsences.ViewAll";
            public const string CreateOwn = "GuideAbsences.CreateOwn";
            public const string CreateAll = "GuideAbsences.CreateAll";
            public const string EditOwn = "GuideAbsences.EditOwn";
            public const string EditAll = "GuideAbsences.EditAll";

            public const string DeleteOwn = "GuideAbsences.DeleteOwn";
            public const string DeleteAll = "GuideAbsences.DeleteAll";
        }

        // CONFIGURACIÓN DE HORARIOS DEL MUSEO
        public static class MuseumSchedules
        {
            public const string View = "MuseumSchedules.View";
            public const string Edit = "MuseumSchedules.Edit";
        }

        // SALAS
        public static class Rooms
        {
            public const string View = "Rooms.View";
            public const string Create = "Rooms.Create";
            public const string Edit = "Rooms.Edit";
            public const string Delete = "Rooms.Delete";
        }

        // RECURSOS
        public static class Resources
        {
            public const string View = "Resources.View";
            public const string Create = "Resources.Create";
            public const string Edit = "Resources.Edit";
            public const string Delete = "Resources.Delete";
        }

        // PROYECTOS EDUCATIVOS
        public static class EducationalProjects
        {
            public const string View = "EducationalProjects.View";
            public const string Create = "EducationalProjects.Create";
            public const string Edit = "EducationalProjects.Edit";
            public const string Delete = "EducationalProjects.Delete";
        }

        // TEMÁTICAS DE VISITAS
        public static class VisitTopics
        {
            public const string View = "VisitTopics.View";
            public const string Create = "VisitTopics.Create";
            public const string Edit = "VisitTopics.Edit";
            public const string Delete = "VisitTopics.Delete";
        }

        // BLOQUEOS DE VISITAS GUIADAS
        public static class GuidedVisitBlocks
        {
            public const string View = "GuidedVisitBlocks.View";
            public const string Create = "GuidedVisitBlocks.Create";
            public const string Edit = "GuidedVisitBlocks.Edit";
            public const string Delete = "GuidedVisitBlocks.Delete";
        }

        // BLOQUEOS DE VISITAS AUTOGUIADAS
        public static class SelfGuidedVisitBlocks
        {
            public const string View = "SelfGuidedVisitBlocks.View";
            public const string Create = "SelfGuidedVisitBlocks.Create";
            public const string Edit = "SelfGuidedVisitBlocks.Edit";
            public const string Delete = "SelfGuidedVisitBlocks.Delete";
        }

        // BLOQUEOS DE EVENTOS
        public static class EventBlocks
        {
            public const string View = "EventBlocks.View";
            public const string Create = "EventBlocks.Create";
            public const string Edit = "EventBlocks.Edit";
            public const string Delete = "EventBlocks.Delete";
        }

        // BLOQUEOS DE MUESTRAS
        public static class ExhibitionBlocks
        {
            public const string View = "ExhibitionBlocks.View";
            public const string Create = "ExhibitionBlocks.Create";
            public const string Edit = "ExhibitionBlocks.Edit";
            public const string Delete = "ExhibitionBlocks.Delete";
        }

        // BLOQUEOS DE ACTIVIDADES EDUCATIVAS
        public static class EducationalActivityBlocks
        {
            public const string View = "EducationalActivityBlocks.View";
            public const string Create = "EducationalActivityBlocks.Create";
            public const string Edit = "EducationalActivityBlocks.Edit";
            public const string Delete = "EducationalActivityBlocks.Delete";
        }

        // DÍAS DE CIERRE DEL MUSEO
        public static class MuseumClosures
        {
            public const string View = "MuseumClosures.View";
            public const string Create = "MuseumClosures.Create";
            public const string Edit = "MuseumClosures.Edit";
            public const string Delete = "MuseumClosures.Delete";
        }

        // REPORTES
        public static class Reports
        {
            public const string View = "Reports.View";
            public const string Export = "Reports.Export";
        }

        // ADMINISTRACIÓN DE USUARIOS
        public static class Users
        {
            public const string View = "Users.View";
            public const string Create = "Users.Create";
            public const string Edit = "Users.Edit";
            public const string Delete = "Users.Delete";
        }

        // CONFIGURACIÓN DE SALAS PARA EVENTOS
        public static class EventRoomConfiguration
        {
            public const string View = "EventRoomConfiguration.View";
            public const string Edit = "EventRoomConfiguration.Edit";
        }
    }
}
