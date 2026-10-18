
using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada.Guia
{
    public class Guia : DomainEntity<string>
    {
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }

        public Email Email { get; private set; }
        public Telefono Telefono { get; private set; }

        public List<HorarioGuia> HorariosGuia { get; private set; } = new();
        public List<AusenciaGuia> AusenciasProgramadas { get; private set; } = new();

        public string UserId { get; private set; }
        public bool Activo { get; private set; } = true;

        protected Guia()
        {
            // EF Core
        }


        // Constructor sin UserId
        public Guia(
            string nombre,
            string apellido,
            Email email,
            Telefono telefono,
            IEnumerable<HorarioGuia> horariosGuia)
        {
            Id = Guid.NewGuid().ToString();

            ActualizarDatosPersonales(nombre, apellido, email, telefono);
            AsignarHorarios(horariosGuia);
        }
        // Vincular posteriormente con un usuario de Identity
        public void VincularUsuario(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new DomainException(
                    "El id del usuario debe ser válido.");

            if (UserId != null)
                throw new DomainException(
                    "El guía ya tiene un usuario vinculado.");

            UserId = userId;
        }

        public void SetUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new DomainException(
                    "El id del usuario debe ser válido.");

            UserId = userId;
        }

        public void ActualizarDatosPersonales(
            string nombre,
            string apellido,
            Email email,
            Telefono telefono)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException(
                    "El nombre del guía no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(apellido))
                throw new DomainException(
                    "El apellido del guía no puede estar vacío.");

            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(telefono);

            Nombre = nombre.Trim();
            Apellido = apellido.Trim();
            Email = email;
            Telefono = telefono;
        }

        public void AsignarHorarios(
            IEnumerable<HorarioGuia> nuevosHorarios)
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
                throw new DomainException(
                    "No se pueden asignar horarios duplicados.");

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
                new AusenciaGuia(Id, desde, hasta, motivo));
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
    }
}
