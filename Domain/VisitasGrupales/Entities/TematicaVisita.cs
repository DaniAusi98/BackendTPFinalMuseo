using Core.Domain.Entities;

using Domain.Common.Exceptions;
using Domain.RecursoMuseo.Entities;

namespace Domain.VisitasGrupales.Entities
{
    public class TematicaVisita : DomainEntity<string>
    {
        public string Nombre { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public bool Disponible { get; private set; } = true;
        public List<Sala> Salas { get; private set; } = new();

        protected TematicaVisita()
        {
            // EF Core
        }

        public TematicaVisita(
            string nombre,
            string descripcion,
            IEnumerable<Sala> salas)
        {
            Id = Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException(
                    "La temática es obligatoria.");

            Nombre = nombre.Trim();

            Descripcion = descripcion?.Trim() ?? string.Empty;

            AsignarSalas(salas);
        }


        public void AsignarSalas(IEnumerable<Sala> nuevasSalas)
        {
            ArgumentNullException.ThrowIfNull(nuevasSalas);

            var listaSalas = nuevasSalas
                .DistinctBy(s => s.Id)
                .ToList();

            if (!listaSalas.Any())
                throw new DomainException(
                    "Una temática debe tener al menos una sala asignada.");

            Salas.Clear();

            Salas.AddRange(listaSalas);
        }


        public void AgregarSala(Sala sala)
        {
            ArgumentNullException.ThrowIfNull(sala);

            if (Salas.Any(s => s.Id == sala.Id))
                throw new DomainException(
                    "La sala ya está asignada a esta temática.");

            Salas.Add(sala);
        }


        public void QuitarSala(string salaId)
        {
            var sala = Salas
                .FirstOrDefault(s => s.Id == salaId);

            if (sala is null)
                throw new DomainException(
                    "La sala indicada no pertenece a la temática.");

            Salas.Remove(sala);
        }


        public void Actualizar(
            string nombre,
            string descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException(
                    "La temática es obligatoria.");

            Nombre = nombre.Trim();
            Descripcion = descripcion?.Trim() ?? string.Empty;
        }


        public void MarcarComoNoDisponible()
        {
            if (!Disponible)
                throw new DomainException(
                    "La temática ya está no disponible.");

            Disponible = false;
        }


        public void MarcarComoDisponible()
        {
            if (Disponible)
                throw new DomainException(
                    "La temática ya está disponible.");

            Disponible = true;
        }
    }
}
