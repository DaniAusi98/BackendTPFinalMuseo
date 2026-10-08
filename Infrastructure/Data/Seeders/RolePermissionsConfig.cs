namespace Infrastructure.Data.Seeders
{
    public static class RolePermissionsConfig
    {
        public static Dictionary<string, List<string>> RolePermissions { get; } = new()
        {
            { "Admin", new List<string>
            {
                "users.create", "users.edit", "users.delete", "users.view",
                "education.manage", "guides.manage", "absences.manage"
            }},

            { "CoordinadoraEducacion", new List<string>
            {
                "guides.view",
                "absences.view.all",           // Ver ausencias de TODOS los guías
                "turnos.view.all",
                "turnos.update",             // Actualizar sus turnos
                "reports.generate"
            }},

            { "EducadoraGuia", new List<string>
            {
                "turnos.view.own",              // Ver solo SUS turnos
                "absences.create",              // Registrar su ausencia
            }},

            { "Usuario", new List<string>
            {
                "profile.view.own"
            }
            },
                { "Recepcionista", new List<string>
            {
                "event.create",
                "groupvisit.create",
                "send.event.form"
            }
            },
            {"Visitante", new List<string>

            { "profile.view.own","create.guidedtour" } }
        };
    }
}