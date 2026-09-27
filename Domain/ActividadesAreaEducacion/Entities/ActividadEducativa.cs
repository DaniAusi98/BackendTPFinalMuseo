using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;
using static Domain.ActividadesAreaEducacion.Enums.Enums;

namespace Domain.ActividadesAreaEducacion.Entities
{
    public class ActividadEducativa :ActividadMuseo.Entities.ActividadMuseo
    {
        public string Titulo { get; set; }
        public TipoActividadEducativa TipoActividadEducativa { get; set; }
        public string ProyectoVinculadaId { get; private set; }
        public ProyectosAreaEducacion ProyectoVinculada { get; private set; }
        public string InstitucionVinculada { get; private set; }
        public string PublicoObjetivo { get; private set; }
        public bool RequiereDifusion { get; private set; }
        public bool SolicitaFlyer { get; private set; }
        public List<string> UrlImagenes { get; private set; } = [];

        public string Observaciones { get; private set; } = string.Empty;
        public ActividadEducativa()
        {
                
        }

        public ActividadEducativa(
            string titulo,
            TipoActividadEducativa tipoActividadEducativa,
            string proyectoVinculadaId,
            string institucionVinculada,
            string publicoObjetivo,
            string observaciones,
            TimeSlot horario,
            List<Sala> salas,
            bool requiereDifusion,
            bool solicitaFlyer,
            int? cantidadAsistentes = null,
            IEnumerable<string?>? urlImagenes = null,
            List<RecursoAsignado>? recursos= null ,
            string? rrule = null
            //List<string>? nombres = null

            ) : base(
                categoria: ActividadMuseo.Enums.Enums.CategoriaActividad.ActividadEducativa,
                tipo: ActividadMuseo.Enums.Enums.TipoActividad.ActividadEducativa,
                cantidadAsistentes: cantidadAsistentes,
                horario : horario,
                salas: salas,
                recursos: recursos,
                rrule: rrule
            )
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("El título no puede estar vacío.", nameof(titulo));
            }
            if (string.IsNullOrWhiteSpace(proyectoVinculadaId))
            {
                throw new ArgumentException("El proyecto vinculado no puede estar vacío.", nameof(proyectoVinculadaId));
            }
            if (string.IsNullOrWhiteSpace(institucionVinculada))
            {
                throw new ArgumentException("La institución vinculada no puede estar vacía.", nameof(institucionVinculada));
            }
            if (string.IsNullOrWhiteSpace(publicoObjetivo))
            {
                throw new ArgumentException("El público objetivo no puede estar vacío.", nameof(publicoObjetivo));
            }
            Titulo = titulo;
            TipoActividadEducativa = tipoActividadEducativa;
            ProyectoVinculadaId = proyectoVinculadaId;
            InstitucionVinculada = institucionVinculada;
            PublicoObjetivo = publicoObjetivo;
            Observaciones =observaciones?.Trim() ?? string.Empty;
            RequiereDifusion = requiereDifusion;
            UrlImagenes = urlImagenes?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)
                .ToList()
                ?? [];
            SolicitaFlyer = solicitaFlyer;
        }
        public void ActualizarActividadEducativa(
            string titulo,
            TipoActividadEducativa tipoActividadEducativa,
            string proyectoVinculadaId,
            string institucionVinculada,
            string publicoObjetivo,
            string observaciones,
            TimeSlot horario,
            List<Sala> salas,
               bool requiereDifusion,
            bool solicitaFlyer,
            int? cantidadAsistentes = null,
            IEnumerable<string?>? urlImagenes = null,
            List<RecursoAsignado>? recursos = null,
            string? rrule = null
        )
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("El título no puede estar vacío.", nameof(titulo));
            }
            if (string.IsNullOrWhiteSpace(proyectoVinculadaId))
            {
                throw new ArgumentException("El proyecto vinculado no puede estar vacío.", nameof(proyectoVinculadaId));
            }
            if (string.IsNullOrWhiteSpace(institucionVinculada))
            {
                throw new ArgumentException("La institución vinculada no puede estar vacía.", nameof(institucionVinculada));
            }
            if (string.IsNullOrWhiteSpace(publicoObjetivo))
            {
                throw new ArgumentException("El público objetivo no puede estar vacío.", nameof(publicoObjetivo));
            }
            Titulo = titulo;
            TipoActividadEducativa = tipoActividadEducativa;
            ProyectoVinculadaId = proyectoVinculadaId;
            InstitucionVinculada = institucionVinculada;
            PublicoObjetivo = publicoObjetivo;
            Observaciones = observaciones?.Trim() ?? string.Empty;
            RequiereDifusion = requiereDifusion;
            UrlImagenes = urlImagenes?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)
                .ToList()
                ?? [];
            SolicitaFlyer = solicitaFlyer;

            // Update base class properties

            CambiarCantidadAsistentes(cantidadAsistentes);
            AsignarTimeSlots(horario);  
            AsignarSalas(salas);
            AsignarRecursos(recursos);
            ActualizarRRule(rrule);
        }





        // Additional properties and methods can be added here as needed
    }
}