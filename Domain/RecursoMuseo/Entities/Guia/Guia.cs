using Core.Domain.Entities;
using Domain.Common.Exceptions;

namespace Domain.RecursoMuseo.Entities.Guia
{
    public class Guia : DomainEntity<string>
    {
        public string NombreCompleto { get; private set; }
        public List<HorarioGuia> HorariosGuia { get; private set; } = new();

        public List<AusenciaGuia> AusenciasProgramadas { get; private set; } = new();

        public string PersonalInternoId { get; private set; }

        public bool Activo { get; private set; } = true;

        protected Guia()
        {
            // EF Core
        }

        public Guia(string nombrecompleto, string personalInternoId, IEnumerable<HorarioGuia> horariosGuia)
        {
            Id = Guid.NewGuid().ToString();

            SetPersonalInternoId(personalInternoId);

            if (string.IsNullOrWhiteSpace(nombrecompleto))
                throw new DomainException("El nombre completo del guía no puede estar vacío.");
            NombreCompleto= nombrecompleto.Trim();
            AsignarHorarios(horariosGuia);


        }

        public void SetPersonalInternoId(string personalInternoId)
        {
            if (string.IsNullOrWhiteSpace(personalInternoId))
                throw new DomainException("El id del personal interno debe ser válido.");

            PersonalInternoId = personalInternoId;
        }

        public void AsignarHorarios(IEnumerable<HorarioGuia> nuevosHorarios)
        {
            ArgumentNullException.ThrowIfNull(nuevosHorarios);

            var horarios = nuevosHorarios.ToList();

            var hayDuplicados = horarios
                .GroupBy(h => new
                {
                    h.DiaAsignado,
                    h.HoraInicio,
                    h.HoraFin
                })
                .Any(g => g.Count() > 1);

            if (hayDuplicados)
            {
                throw new DomainException(
                    "No se pueden asignar horarios duplicados.");
            }
            // Validar solapamientos
            foreach (var grupo in horarios.GroupBy(h => h.DiaAsignado))
            {
                var horariosDelDia = grupo
                    .OrderBy(h => h.HoraInicio)
                    .ToList();

                for (int i = 0; i < horariosDelDia.Count - 1; i++)
                {
                    var actual = horariosDelDia[i];
                    var siguiente = horariosDelDia[i + 1];

                    if (actual.HoraFin > siguiente.HoraInicio)
                    {
                        throw new DomainException(
                            $"El guía posee horarios superpuestos el día {grupo.Key}.");
                    }
                }
            }

            HorariosGuia.Clear();
            HorariosGuia.AddRange(horarios);
        }
        public void AgregarAusencia(
            DateTime desde,
            DateTime hasta,
            string motivo)
        {
            AusenciasProgramadas.Add(
                new AusenciaGuia(desde, hasta, motivo));
        }

        public void EliminarAusencia(string ausenciaId)
        {
            var ausencia = AusenciasProgramadas
                .FirstOrDefault(a => a.Id == ausenciaId)
                ?? throw new DomainException(
                    "La ausencia indicada no existe.");

            AusenciasProgramadas.Remove(ausencia);
        }

        public void GuiaNoActivo()
        {
            if (!Activo)
                throw new DomainException(
                    "El guía ya está desactivado.");

            Activo = false;
        }
        public void GuiaActivo()
        {
            if (Activo)
                throw new DomainException(
                    "El guía ya está activo.");
            Activo = true;
        }
        public void ActualizarNombreCompleto(string nuevoNombreCompleto)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombreCompleto))
                throw new DomainException("El nombre completo del guía no puede estar vacío.");
            NombreCompleto = nuevoNombreCompleto.Trim();
        }

    }
}
